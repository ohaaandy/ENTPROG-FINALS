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
        [Key]
        public int EventID { get; set; }

        public int ClubID { get; set; }     //FK
        public Club Club { get; set; }      //Navigation

        public string UserID { get; set; }     //FK
        public User User { get; set; }      //Navigation

        public string EventName { get; set; }
        public string EventType { get; set; }
        public DateTime DateTime { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }

        public int LeaderboardID { get; set; }
        public Leaderboard Leaderboards { get; set; }     //one to one
    }
}
