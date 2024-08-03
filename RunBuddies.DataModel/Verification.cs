using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBuddies.DataModel
{
    public class Verification
    {
        [Key]
        public int VerificationID { get; set; }

        public bool IsVerified { get; set; }

        public BuddySession BuddySessions { get; set; }   //one to one
    }
}
