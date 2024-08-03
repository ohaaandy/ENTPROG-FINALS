using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBuddies.DataModel
{
    public class BuddySession
    {
        [Key]
        public int BuddySessionID { get; set; }

        public int BuddyID { get; set; }                 //FK
        public BuddyPartner BuddyPartner { get; set; }  //Navigation

        public int VerificationID { get; set; }         //FK
        public Verification Verification { get; set; }  //Navigation

        public DateTime DateTime { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
    }
}
