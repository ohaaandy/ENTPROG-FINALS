using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBuddies.DataModel
{
    public class ClubMembership
    {
        public int ClubID { get; set; }
        public Club Club { get; set; }

        public int ClubMemberID { get; set; }
        public ClubMember ClubMember { get; set; }
    }
}
