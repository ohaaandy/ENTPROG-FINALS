using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunBuddies.DataModel;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using RunBuddies.App.Models;

namespace RunBuddies.App.Controllers
{
    [Authorize]
    public class BuddyController : Controller
    {
        private readonly AppDBContext _context;
        private readonly UserManager<User> _userManager;
        public BuddyController(AppDBContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SendInvitation(string receiverId)
        {
            try
            {
                var senderId = GetCurrentUserId();

                // Check if receiver exists
                var receiver = await _context.Users.FindAsync(receiverId);
                if (receiver == null)
                {
                    return Json(new { success = false, message = "Receiver not found." });
                }

                var existingInvitation = await _context.BuddyInvitations
                    .AnyAsync(bi => (bi.SenderID == senderId && bi.ReceiverID == receiverId) ||
                                   (bi.SenderID == receiverId && bi.ReceiverID == senderId));

                if (existingInvitation)
                {
                    return Json(new { success = false, message = "An invitation already exists between you and this user." });
                }

                var invitation = new BuddyInvitation
                {
                    SenderID = senderId,
                    ReceiverID = receiverId,
                    SentDate = DateTime.Now,
                    Status = InvitationStatus.Pending
                };

                _context.BuddyInvitations.Add(invitation);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = "Buddy invitation sent successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception
                return Json(new { success = false, message = "An error occurred while sending the invitation." });
            }
        }
        [Authorize]
        public IActionResult PendingInvitations()
        {
            var userId = GetCurrentUserId();
            var invitations = _context.BuddyInvitations
                .Where(bi => bi.ReceiverID == userId && bi.Status == InvitationStatus.Pending)
                .Include(bi => bi.Sender)
                .ToList();

            return View(invitations);
        }

        [HttpPost]
        public IActionResult RespondToInvitation(int invitationId, bool accept)
        {
            var invitation = _context.BuddyInvitations.Find(invitationId);
            if (invitation == null)
            {
                return NotFound();
            }

            if (accept)
            {
                invitation.Status = InvitationStatus.Accepted;

                var buddyPartnership = new BuddyPartner
                {
                    UserID = invitation.SenderID,
                    User = _context.Users.Find(invitation.ReceiverID)
                };

                _context.BuddyPartners.Add(buddyPartnership);
            }
            else
            {
                invitation.Status = InvitationStatus.Rejected;
            }

            _context.SaveChanges();

            return Json(new { success = true, message = accept ? "Invitation accepted" : "Invitation rejected" });
        }
        [Authorize]
        public async Task<IActionResult> MyBuddies()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var buddyPartnerships = await _context.BuddyPartners
                .Where(bp => bp.UserID == currentUser.Id)
                .Include(bp => bp.User)
                .Include(bp => bp.BuddySessions)
                .ToListAsync();

            var viewModel = new MyBuddiesViewModel
            {
                Buddies = buddyPartnerships.Select(bp => new BuddyDetailViewModel
                {
                    UserId = bp.User.Id,
                    FullName = $"{bp.User.FirstName} {bp.User.LastName}",
                    RunnerLevel = bp.User.RunnerLevel,
                    Location = bp.User.Location,
                    PreferredSchedule = bp.User.Schedule?.ToString("dddd"),
                    PreferredDistance = bp.User.Distance ?? 0,
                    Email = bp.User.Email,
                    PhoneNumber = bp.User.PhoneNumber,
                    RecentSessions = bp.BuddySessions
                        .OrderByDescending(bs => bs.DateTime)
                        .Take(3)
                        .Select(bs => new BuddySessionViewModel
                        {
                            DateTime = bs.DateTime,
                            Location = bs.Location,
                            Description = bs.Description
                        })
                        .ToList()
                }).ToList()
            };

            return View(viewModel);
        }
    

    private string GetCurrentUserId()//Must be Authenticated to work
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                return (userIdClaim.Value);
            }

            // If the user is not authenticated or the claim is not found, throw an exception
            throw new InvalidOperationException("User is not authenticated or user ID claim is missing.");
        }
        public int GetPendingInvitationsCount()
        {
            var userId = GetCurrentUserId();
            return _context.BuddyInvitations
                .Count(bi => bi.ReceiverID == userId && bi.Status == InvitationStatus.Pending);
        }
    }
}