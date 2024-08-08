using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBuddies.DataModel
{
    public class Leaderboard
    {
        public int LeaderboardID { get; set; }
        public int EventID { get; set; }
        public Event Event { get; set; }
        public List<LeaderboardEntry> Entries { get; set; }
    }
}
