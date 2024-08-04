using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBuddies.DataModel
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        //Details
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly Birthday { get; set; }
        public string Gender { get; set; }
        public int ContactNumber { get; set; }

        //Preferences
        public string RunnerLevel { get; set; }
        public DateOnly Schedule { get; set; }

        public string Location { get; set; }
        public int Distance { get; set; }
        public ICollection<BuddyInvitation> SentBuddyInvitations { get; set; }
        public ICollection<BuddyInvitation> ReceivedBuddyInvitations { get; set; }

        public List<ClubModerator> ClubModerators { get; set; }     //many to one
        public List<ClubMember> ClubMembers { get; set; }           //many to one
        public List<Event> Events { get; set; }                     //many to one
        public List<BuddyPartner> BuddyPartners { get; set; }       //one to one
    }
}
