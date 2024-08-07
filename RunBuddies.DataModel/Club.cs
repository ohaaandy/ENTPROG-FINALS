using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBuddies.DataModel
{
    public class Club
    {
        [Key]
        public int ClubID { get; set; }
        public int ClubModeratorID { get; set; }
        public string ClubName { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string ContactEmail { get; set; }
        public ClubModerator ClubModerator { get; set; }
        public List<ClubMember> ClubMembers { get; set; } // Changed from single ClubMember to List
        public List<Event> Events { get; set; }
        public string CommunicationGroupLink { get; set; }

    }
}
