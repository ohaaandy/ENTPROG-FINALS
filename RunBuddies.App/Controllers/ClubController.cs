using Microsoft.AspNetCore.Authorization;
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

        public ClubController(AppDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClubViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = GetCurrentUserId(); // Get the current user ID
                if (userId == null)
                {
                    return Unauthorized("User is not authenticated.");
                }

                var clubMember = await _context.ClubMembers.SingleOrDefaultAsync(cm => cm.UserID == userId.Value);
                if (clubMember == null)
                {
                    return BadRequest("Club member not found.");
                }

                var club = new Club
                {
                    ClubName = model.ClubName,
                    Location = model.Location,
                    Description = model.Description,
                    ContactEmail = model.ContactEmail,
                    ClubMemberID = clubMember.ClubMemberID,
                    ClubModeratorID = clubMember.ClubMemberID, // Assuming the club creator is also the moderator
                };

                _context.Clubs.Add(club);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        private int? GetCurrentUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new InvalidOperationException("User is not authenticated or user ID claim is missing.");
            }
            return int.Parse(userIdClaim.Value);
        }
    }
}
