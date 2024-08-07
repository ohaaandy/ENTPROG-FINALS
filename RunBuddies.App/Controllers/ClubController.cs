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
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClubViewModel model)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                var club = new Club
                {
                    ClubName = model.ClubName,
                    Location = model.Location,
                    Description = model.Description,
                    ContactEmail = model.ContactEmail
                };
                var clubModerator = new ClubModerator
                {
                    UserID = currentUser.Id,
                    User = currentUser
                };
                club.ClubModerator = clubModerator;
                club.ClubModeratorID = clubModerator.ClubModeratorID;

                _context.Clubs.Add(club);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Manage), new { id = club.ClubID });
            }
            return View(model);
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
            if (club.ClubModerator.UserID != currentUser.Id)
            {
                return Forbid();
            }

            var viewModel = new ClubManageViewModel
            {
                Club = club,
                MemberRequests = await _context.ClubMembershipRequests
                    .Where(r => r.ClubID == id && r.Status == MembershipRequestStatus.Pending)
                    .Include(r => r.User)
                    .ToListAsync()
            };

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
            request.Status = MembershipRequestStatus.Accepted;
            var clubMember = new ClubMember
            {
                UserID = request.UserID,
                Clubs = new List<Club> { await _context.Clubs.FindAsync(request.ClubID) }
            };
            _context.ClubMembers.Add(clubMember);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Manage), new { id = request.ClubID });
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
        public async Task<IActionResult> UpdateClub(ClubViewModel model)
        {
            var club = await _context.Clubs.FindAsync(model.ClubID);
            if (club == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (club.ClubModerator == null || club.ClubModerator.UserID != currentUser.Id)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                club.ClubName = model.ClubName;
                club.Location = model.Location;
                club.Description = model.Description;
                club.ContactEmail = model.ContactEmail;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Manage), new { id = club.ClubID });
            }

            return RedirectToAction(nameof(Manage), new { id = model.ClubID });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveMember(int clubId, string memberId)
        {
            var club = await _context.Clubs
                .Include(c => c.ClubMembers)
                .FirstOrDefaultAsync(c => c.ClubID == clubId);

            var member = await _context.ClubMembers
                .FirstOrDefaultAsync(m => m.UserID == memberId);

            if (club == null || member == null)
            {
                return Json(new { success = false, message = "Club or member not found." });
            }

            if (!club.ClubMembers.Contains(member))
            {
                return Json(new { success = false, message = "This user is not a member of this club." });
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