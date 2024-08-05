using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RunBuddies.DataModel;
using System.Security.Claims;

namespace RunBuddies.App.ViewComponents
{
    public class PendingInvitationsViewComponent : ViewComponent
    {
        private readonly AppDBContext _context;

        public PendingInvitationsViewComponent(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Content(string.Empty);
            }

            var userId = UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier);
            var count = await _context.BuddyInvitations
                .CountAsync(bi => bi.ReceiverID == userId && bi.Status == InvitationStatus.Pending);

            return View(count);
        }
    }
}