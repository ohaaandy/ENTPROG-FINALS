using RunBuddies.DataModel;

namespace RunBuddies.App.Models
{
    public class ClubManageViewModel
    {
        public Club Club { get; set; }
        public List<ClubMembershipRequest> MemberRequests { get; set; }
    }
}
