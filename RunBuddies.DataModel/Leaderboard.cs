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
        [Key]
        public int LeaderboardID { get; set; }

        public int EventID { get; set; }    //FK
        public Event Event { get; set; }    //Navigation

        public int Ranking { get; set; }
        public TimeOnly Time { get; set; }
    }
}
