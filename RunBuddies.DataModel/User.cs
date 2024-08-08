using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBuddies.DataModel
{
    public class User : IdentityUser
    {
        //Details
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateOnly Birthday { get; set; }
        public string Gender { get; set; }

        //Preferences
        public string? RunnerLevel { get; set; }
        public DateOnly? Schedule { get; set; }
        public string? Location { get; set; }
        public int? Distance { get; set; }
        public ICollection<BuddyInvitation> SentBuddyInvitations { get; set; }
        public ICollection<BuddyInvitation> ReceivedBuddyInvitations { get; set; }

        public List<ClubModerator> ClubModerators { get; set; }     //many to one
        public List<ClubMember> ClubMembers { get; set; }           //many to one
        public List<Event> Events { get; set; }                     //many to one
        public List<BuddyPartner> BuddyPartners { get; set; }       //one to one

        public ICollection<EventParticipant> EventParticipants { get; set; }
    }
}
