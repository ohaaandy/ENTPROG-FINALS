using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBuddies.DataModel
{
    public class LeaderboardEntry
    {
        public int LeaderboardEntryID { get; set; }
        public int LeaderboardID { get; set; }
        public Leaderboard Leaderboard { get; set; }
        public string UserID { get; set; }
        public User User { get; set; }
        public int Rank { get; set; }
        public TimeSpan Time { get; set; }
    }
}
