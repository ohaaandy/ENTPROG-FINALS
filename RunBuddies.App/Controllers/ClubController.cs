using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunBuddies.App.Models;
using RunBuddies.DataModel;
using RunBuddies.ViewModels;
using System.Security.Claims;

namespace RunBuddies.App.Controllers
{

    public class ClubController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<User> _userManager;

        public ClubController(AppDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [Authorize]
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
                .Include(c => c.ClubMembers)
                    .ThenInclude(cm => cm.User)
                .Include(c => c.Events)
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

            var memberRequests = await _context.ClubMembershipRequests
                .Where(r => r.ClubID == id && r.Status == MembershipRequestStatus.Pending)
                .Include(r => r.User)
                .ToListAsync();

            var viewModel = new ClubManageViewModel
            {
                Club = club,
                MemberRequests = memberRequests
            };

            return View("Manage", viewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ClubViewModel model)
        {
            if (id != model.ClubID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
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

                    club.ClubName = model.ClubName;
                    club.Location = model.Location;
                    club.Description = model.Description;
                    club.ContactEmail = model.ContactEmail;
                    club.CommunicationGroupLink = string.IsNullOrWhiteSpace(model.CommunicationGroupLink) ? null : model.CommunicationGroupLink;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClubExists(model.ClubID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Manage), new { id = model.ClubID });
            }
            return View(model);
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
            if (!ModelState.IsValid)
            {
                var viewModel = new ClubManageViewModel
                {
                    Club = club,
                    MemberRequests = await _context.ClubMembershipRequests
                        .Where(r => r.ClubID == club.ClubID && r.Status == MembershipRequestStatus.Pending)
                        .Include(r => r.User)
                        .ToListAsync()
                };
                return View("Manage", viewModel);
            }

            var existingClub = await _context.Clubs
                .Include(c => c.ClubModerator)
                .Include(c => c.ClubMembers)
                .Include(c => c.Events)
                .FirstOrDefaultAsync(c => c.ClubID == club.ClubID);

            if (existingClub == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (existingClub.ClubModerator == null || existingClub.ClubModerator.UserID != currentUser.Id)
            {
                return Forbid();
            }

            existingClub.ClubName = club.ClubName;
            existingClub.Location = club.Location;
            existingClub.Description = club.Description;
            existingClub.ContactEmail = club.ContactEmail;
            existingClub.CommunicationGroupLink = club.CommunicationGroupLink;

            await _context.SaveChangesAsync();

            var updatedViewModel = new ClubManageViewModel
            {
                Club = existingClub,
                MemberRequests = await _context.ClubMembershipRequests
                    .Where(r => r.ClubID == existingClub.ClubID && r.Status == MembershipRequestStatus.Pending)
                    .Include(r => r.User)
                    .ToListAsync()
            };

            return View("Manage", updatedViewModel);
        }

        private async Task<Club> GetClubEntityAsync(int clubId)
        {
            return await _context.Clubs
                .Include(c => c.ClubModerator)
                .Include(c => c.ClubMembers)
                .Include(c => c.Events)
                .FirstOrDefaultAsync(c => c.ClubID == clubId);
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

        [Authorize]
        public IActionResult CreateEvent(int clubId)
        {
            var eventVM = new EventViewModel { ClubID = clubId };
            return View(eventVM);
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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateEvent(EventViewModel eventVM)
        {
            if (ModelState.IsValid)
            {
                var newEvent = new Event
                {
                    ClubID = eventVM.ClubID,
                    EventName = eventVM.EventName,
                    EventType = eventVM.EventType,
                    DateTime = eventVM.DateTime,
                    Location = eventVM.Location,
                    Description = eventVM.Description,
                    UserID = (await _userManager.GetUserAsync(User)).Id
                };

                _context.Events.Add(newEvent);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Manage), new { id = eventVM.ClubID });
            }

            return View(eventVM);
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

        private bool ClubExists(int id)
        {
            return _context.Clubs.Any(e => e.ClubID == id);
        }
    }
}