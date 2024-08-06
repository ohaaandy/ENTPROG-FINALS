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
        public string User1ID { get; set; }
        public string User2ID { get; set; }
        public User User1 { get; set; }
        public User User2 { get; set; }
        public List<BuddySession> BuddySessions { get; set; }
    }
}