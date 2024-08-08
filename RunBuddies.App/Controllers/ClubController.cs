using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunBuddies.App.Models;
using RunBuddies.DataModel;
using System.Security.Claims;
using System.Text.Json;

namespace RunBuddies.App.Controllers
{
    [Authorize]
    public class ClubController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<ClubController> _logger;


        public ClubController(AppDBContext context, UserManager<User> userManager, ILogger<ClubController> logger)
        {
            _context = context;
            _userManager = userManager;
            _logger = logger;
        }



        public IActionResult Create()
        {
            return View(new CreateClubViewModel());
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateClubViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var club = new Club
                {
                    ClubName = model.ClubName,
                    Location = model.Location,
                    Description = model.Description,
                    ContactEmail = model.ContactEmail,
                    CommunicationGroupLink = string.IsNullOrWhiteSpace(model.CommunicationGroupLink) ? null : model.CommunicationGroupLink
                };
                var clubModerator = new ClubModerator
                {
                    UserID = currentUser.Id,
                    User = currentUser
                };
                club.ClubModerator = clubModerator;
                club.ClubModeratorID = clubModerator.ClubModeratorID;

                // Add the creator as a member
                var clubMember = new ClubMember
                {
                    UserID = currentUser.Id,
                    User = currentUser
                };
                club.ClubMembers = new List<ClubMember> { clubMember };

                _context.Clubs.Add(club);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Manage), new { id = club.ClubID });
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> CreateEvent(int clubId)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge(); // This will redirect to login if the user is not authenticated
            }

            var club = await _context.Clubs
                .Include(c => c.ClubModerator)
                .FirstOrDefaultAsync(c => c.ClubID == clubId);

            if (club == null)
            {
                return NotFound($"Club with ID {clubId} not found.");
            }

            if (club.ClubModerator == null)
            {
                return BadRequest($"Club with ID {clubId} does not have a moderator.");
            }

            if (club.ClubModerator.UserID != currentUser.Id)
            {
                return Forbid(); // This should trigger the AccessDenied page
            }

            var model = new CreateEventViewModel { ClubID = clubId };
            return View(model);
        }
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent(CreateEventViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Challenge();
            }

            var club = await _context.Clubs
                .Include(c => c.ClubModerator)
                .FirstOrDefaultAsync(c => c.ClubID == model.ClubID);

            if (club == null)
            {
                return NotFound($"Club with ID {model.ClubID} not found.");
            }

            if (club.ClubModerator == null || club.ClubModerator.UserID != currentUser.Id)
            {
                return Forbid();
            }

            var newEvent = new Event
            {
                ClubID = model.ClubID,
                UserID = currentUser.Id,
                EventName = model.EventName,
                EventType = model.EventType,
                DateTime = model.DateTime,
                Location = model.Location,
                Description = model.Description
            };

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Manage), new { id = model.ClubID });
        }

        [Authorize]
        public async Task<IActionResult> MyClubs()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var userClubs = await _context.Clubs
                .Where(c => c.ClubMembers.Any(cm => cm.UserID == currentUser.Id) || c.ClubModerator.UserID == currentUser.Id)
                .Select(c => new ClubViewModel
                {
                    ClubID = c.ClubID,
                    ClubName = c.ClubName,
                    Location = c.Location,
                    Description = c.Description,
                    ContactEmail = c.ContactEmail,
                    CommunicationGroupLink = c.CommunicationGroupLink,
                    IsModerator = c.ClubModerator.UserID == currentUser.Id,
                    IsMember = c.ClubMembers.Any(cm => cm.UserID == currentUser.Id) || c.ClubModerator.UserID == currentUser.Id
                })
                .ToListAsync();

            return View(userClubs);
        }
        [Authorize]
        public async Task<IActionResult> Manage(int id)
        {
            var club = await _context.Clubs
                .Include(c => c.ClubModerator)
                .Include(c => c.ClubMembers)
                    .ThenInclude(cm => cm.User)
                .Include(c => c.Events)
                .FirstOrDefaultAsync(c => c.ClubID == id);

            if (club == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            var isModerator = club.ClubModerator?.UserID == currentUser.Id;

            if (!isModerator)
            {
                return Forbid();
            }

            var viewModel = new ClubManageViewModel
            {
                Club = club,
                MemberRequests = await _context.ClubMembershipRequests
                    .Where(r => r.ClubID == id && r.Status == MembershipRequestStatus.Pending)
                    .Include(r => r.User)
                    .ToListAsync(),
                IsModerator = isModerator
            };

            if (TempData["SuccessMessage"] != null)
            {
                viewModel.SuccessMessage = TempData["SuccessMessage"].ToString();
            }

            return View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var club = await _context.Clubs
                .Include(c => c.ClubModerator)
                .FirstOrDefaultAsync(c => c.ClubID == id);

            if (club == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (club.ClubModerator == null || club.ClubModerator.UserID != currentUser.Id)
            {
                return Forbid();
            }

            return View(club);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ClubID,ClubName,Location,Description,ContactEmail,CommunicationGroupLink")] Club club)
        {
            if (id != club.ClubID)
            {
                return Json(new { success = false, message = "ID mismatch" });
            }

            // Fetch the existing club with all its related data
            var existingClub = await _context.Clubs
                .Include(c => c.ClubModerator)
                .Include(c => c.ClubMembers)
                .Include(c => c.Events)
                .FirstOrDefaultAsync(c => c.ClubID == id);

            if (existingClub == null)
            {
                return Json(new { success = false, message = "Club not found" });
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (existingClub.ClubModerator == null || existingClub.ClubModerator.UserID != currentUser.Id)
            {
                return Json(new { success = false, message = "Unauthorized access" });
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update only the editable fields
                    existingClub.ClubName = club.ClubName;
                    existingClub.Location = club.Location;
                    existingClub.Description = club.Description;
                    existingClub.ContactEmail = club.ContactEmail;
                    existingClub.CommunicationGroupLink = club.CommunicationGroupLink;

                    // Ensure ClubModeratorID is preserved
                    existingClub.ClubModeratorID = existingClub.ClubModerator.ClubModeratorID;

                    _context.Entry(existingClub).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return Json(new { success = true });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClubExists(club.ClubID))
                    {
                        return Json(new { success = false, message = "Club no longer exists" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Concurrency error" });
                    }
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            return Json(new { success = false, message = "Invalid data", errors = errors });
        }
        private bool ClubExists(int id)
        {
            return _context.Clubs.Any(e => e.ClubID == id);
        }
        

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptMember(int requestId)
        {
            var request = await _context.ClubMembershipRequests.FindAsync(requestId);
            if (request == null)
            {
                return NotFound();
            }

            var club = await _context.Clubs.FindAsync(request.ClubID);
            if (club == null)
            {
                return NotFound();
            }

            var clubMember = new ClubMember
            {
                UserID = request.UserID,
                User = request.User
            };

            if (club.ClubMembers == null)
            {
                club.ClubMembers = new List<ClubMember>();
            }

            club.ClubMembers.Add(clubMember);
            _context.ClubMembershipRequests.Remove(request);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Manage), new { id = request.ClubID });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> JoinClub(int clubId)
        {
            var club = await _context.Clubs
                .Include(c => c.ClubMembers)
                .FirstOrDefaultAsync(c => c.ClubID == clubId);

            if (club == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);

            if (!club.ClubMembers.Any(cm => cm.UserID == currentUser.Id))
            {
                var clubMember = new ClubMember
                {
                    UserID = currentUser.Id,
                    User = currentUser
                };

                club.ClubMembers.Add(clubMember);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Details), new { id = clubId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectMember(int requestId)
        {
            var request = await _context.ClubMembershipRequests.FindAsync(requestId);
            if (request == null)
            {
                return NotFound();
            }

            request.Status = MembershipRequestStatus.Rejected;
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Manage), new { id = request.ClubID });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateClub(Club club)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    _logger.LogWarning($"Invalid ModelState: {string.Join(", ", errors)}");
                    return View("Manage", new ClubManageViewModel { Club = club, MemberRequests = await GetMemberRequests(club.ClubID) });
                }

                var existingClub = await _context.Clubs
                    .Include(c => c.ClubModerator)
                    .Include(c => c.ClubMembers)
                    .Include(c => c.Events)
                    .FirstOrDefaultAsync(c => c.ClubID == club.ClubID);

                if (existingClub == null)
                {
                    _logger.LogWarning($"Club not found: {club.ClubID}");
                    return NotFound();
                }

                var currentUser = await _userManager.GetUserAsync(User);
                if (existingClub.ClubModerator == null || existingClub.ClubModerator.UserID != currentUser.Id)
                {
                    _logger.LogWarning($"Unauthorized access attempt by user: {currentUser.Id}");
                    return Forbid();
                }

                _logger.LogInformation($"Updating club: {existingClub.ClubID}");
                _logger.LogInformation($"Old values: {JsonSerializer.Serialize(existingClub)}");
                _logger.LogInformation($"New values: {JsonSerializer.Serialize(club)}");

                // Update the existing club properties
                existingClub.ClubName = club.ClubName;
                existingClub.Location = club.Location;
                existingClub.Description = club.Description;
                existingClub.ContactEmail = club.ContactEmail;
                existingClub.CommunicationGroupLink = club.CommunicationGroupLink;

                _context.Entry(existingClub).State = EntityState.Modified;

                var changes = await _context.SaveChangesAsync();
                _logger.LogInformation($"Changes saved: {changes}");

                // Verify that changes were saved
                await _context.Entry(existingClub).ReloadAsync();

                if (existingClub.ClubName == club.ClubName &&
                    existingClub.Location == club.Location &&
                    existingClub.Description == club.Description &&
                    existingClub.ContactEmail == club.ContactEmail &&
                    existingClub.CommunicationGroupLink == club.CommunicationGroupLink)
                {
                    _logger.LogInformation("Club information updated successfully.");
                    TempData["SuccessMessage"] = "Club information updated successfully.";
                }
                else
                {
                    _logger.LogWarning("Club information may not have been fully updated.");
                    TempData["ErrorMessage"] = "Club information may not have been fully updated. Please check and try again.";
                }

                return RedirectToAction(nameof(Manage), new { id = existingClub.ClubID });
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "Error updating club");
                TempData["ErrorMessage"] = "An error occurred while updating the club information. Please try again.";
                return View("Manage", new ClubManageViewModel { Club = club, MemberRequests = await GetMemberRequests(club.ClubID) });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error updating club");
                TempData["ErrorMessage"] = "An unexpected error occurred. Please try again.";
                return View("Manage", new ClubManageViewModel { Club = club, MemberRequests = await GetMemberRequests(club.ClubID) });
            }
        }

        private async Task<List<ClubMembershipRequest>> GetMemberRequests(int clubId)
        {
            return await _context.ClubMembershipRequests
                .Where(r => r.ClubID == clubId && r.Status == MembershipRequestStatus.Pending)
                .Include(r => r.User)
                .ToListAsync();
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveMember(int clubId, string memberId)
        {
            var club = await _context.Clubs
                .Include(c => c.ClubMembers)
                .Include(c => c.ClubModerator)
                .FirstOrDefaultAsync(c => c.ClubID == clubId);

            if (club == null)
            {
                return Json(new { success = false, message = "Club not found." });
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (club.ClubModerator.UserID != currentUser.Id)
            {
                return Json(new { success = false, message = "You are not authorized to remove members." });
            }

            var member = club.ClubMembers.FirstOrDefault(m => m.UserID == memberId);

            if (member == null)
            {
                return Json(new { success = false, message = "Member not found." });
            }

            if (member.UserID == club.ClubModerator.UserID)
            {
                return Json(new { success = false, message = "You cannot remove yourself as the moderator." });
            }

            club.ClubMembers.Remove(member);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        
        public async Task<IActionResult> Details(int id)
        {
            var club = await _context.Clubs
                .Include(c => c.ClubModerator)
                .Include(c => c.ClubMembers)
                .Include(c => c.Events)
                .FirstOrDefaultAsync(c => c.ClubID == id);

            if (club == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);

            var viewModel = new ClubViewModel
            {
                ClubID = club.ClubID,
                ClubName = club.ClubName,
                Location = club.Location,
                Description = club.Description,
                ContactEmail = club.ContactEmail,
                CommunicationGroupLink = club.CommunicationGroupLink,
                IsModerator = club.ClubModerator?.UserID == currentUser.Id,
                IsMember = club.ClubMembers.Any(cm => cm.UserID == currentUser.Id),
                Events = club.Events.ToList()  // Populate the Events property
            };

            return View(viewModel);
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteEvent(int eventId)
        {
            var evt = await _context.Events.FindAsync(eventId);
            if (evt == null)
            {
                return Json(new { success = false, message = "Event not found." });
            }

            _context.Events.Remove(evt);
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

       
    }
}