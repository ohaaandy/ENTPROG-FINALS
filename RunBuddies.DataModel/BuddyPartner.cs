using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBuddies.DataModel
{
    public class BuddyPartner
    {
        [Key]
        public int BuddyID { get; set; }
        public string UserID { get; set; }     // This is the foreign key
        public User User { get; set; }      // This is the navigation property
        public List<BuddySession> BuddySessions { get; set; }   //one to many
    }
}
