using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBuddies.DataModel
{
    public class BuddyInvitation
    {
        [Key]
        public int InvitationID { get; set; }
        public int SenderID { get; set; }
        public int ReceiverID { get; set; }
        public DateTime SentDate { get; set; }
        public InvitationStatus Status { get; set; }

        public User Sender { get; set; }
        public User Receiver { get; set; }
    }

    public enum InvitationStatus
    {
        Pending,
        Accepted,
        Rejected
    }
}
