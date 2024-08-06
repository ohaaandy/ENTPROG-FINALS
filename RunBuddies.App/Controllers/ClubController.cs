using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunBuddies.App.Models;
using RunBuddies.DataModel;
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
        public async Task<IActionResult> Create(Club club)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);

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
            return View(club);
        }

        [Authorize]
        public async Task<IActionResult> Manage(int id)
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
            if (club.ClubModerator.UserID != currentUser.Id)
            {
                return Forbid();
            }

            return View(club);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AcceptMember(int clubId, string userId)
        {
            var club = await _context.Clubs.FindAsync(clubId);
            var user = await _userManager.FindByIdAsync(userId);

            if (club == null || user == null)
            {
                return NotFound();
            }

            var clubMember = await _context.ClubMembers
                .FirstOrDefaultAsync(cm => cm.UserID == userId);

            if (clubMember == null)
            {
                clubMember = new ClubMember
                {
                    UserID = user.Id,
                    User = user,
                    Clubs = new List<Club>()
                };
                _context.ClubMembers.Add(clubMember);
            }

            if (!clubMember.Clubs.Any(c => c.ClubID == clubId))
            {
                clubMember.Clubs.Add(club);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Manage), new { id = clubId });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateEvent(Event newEvent)
        {
            if (ModelState.IsValid)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                newEvent.UserID = currentUser.Id;

                _context.Events.Add(newEvent);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Manage), new { id = newEvent.ClubID });
            }

            return RedirectToAction(nameof(Manage), new { id = newEvent.ClubID });
        }
    }
}