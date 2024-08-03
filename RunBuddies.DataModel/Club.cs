using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBuddies.DataModel
{
    public class Club
    {
        public int ClubID { get; set; }

        public int ClubModeratorID { get; set; }            //FK
        public ClubModerator ClubModerator { get; set; }    //Navigation

        public int ClubMemberID { get; set; }       //FK
        public ClubMember ClubMember { get; set; }  //Navigation

        public string ClubName { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string ContactEmail { get; set; }

        public List<Event> Events { get; set; }     //one to many
    }
}
