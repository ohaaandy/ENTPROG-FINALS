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

            return View(club);
        }

        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var club = await _context.Clubs.FindAsync(id);
            if (club == null)
            {
                return NotFound();
            }

            var currentUser = await _userManager.GetUserAsync(User);
            if (club.ClubModerator.UserID != currentUser.Id)
            {
                return Forbid();
            }

            return View(club);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Club club)
        {
            if (id != club.ClubID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(club);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClubExists(club.ClubID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Manage), new { id = club.ClubID });
            }
            return View(club);
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