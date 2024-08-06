using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBuddies.DataModel
{
    public class ClubMember
    {
        public int ClubMemberID { get; set; }
        public string UserID { get; set; }
        public User User { get; set; }
        public List<Club> Clubs { get; set; }
    }
}
