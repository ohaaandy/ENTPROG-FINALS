using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBuddies.DataModel
{
    public class ClubMembershipRequest
    {
        [Key]
        public int ID { get; set; }
        public int ClubID { get; set; }
        public Club Club { get; set; }
        public string UserID { get; set; }
        public User User { get; set; }
        public DateTime RequestDate { get; set; }
        public MembershipRequestStatus Status { get; set; }
    }
    public enum MembershipRequestStatus
    {
        Pending,
        Accepted,
        Rejected
    }
}

