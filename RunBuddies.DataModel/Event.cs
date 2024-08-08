using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBuddies.DataModel
{
    public class Event
    {
        public int EventID { get; set; }
        public int ClubID { get; set; }
        public Club Club { get; set; }
        public string UserID { get; set; }
        public User User { get; set; }
        public string EventName { get; set; }
        public string EventType { get; set; }
        public DateTime DateTime { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public int? LeaderboardID { get; set; }
        public Leaderboard Leaderboard { get; set; }
        public ICollection<EventParticipant> EventParticipants { get; set; }

        public Event()
        {
            EventParticipants = new List<EventParticipant>();
        }
    }
}
