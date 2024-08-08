using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunBuddies.DataModel
{
    public class EventParticipant
    {
        public int EventID { get; set; }
        public Event Event { get; set; }
        public string UserID { get; set; }
        public User User { get; set; }
    }
}
