using System;
using System.ComponentModel.DataAnnotations;

namespace RunBuddies.ViewModels
{
    public class EventViewModel
    {
        public int ClubID { get; set; }
        public int EventID { get; set; }

        [Required]
        public string EventName { get; set; }

        [Required]
        public string EventType { get; set; }

        [Required]
        public DateTime DateTime { get; set; }

        [Required]
        public string Location { get; set; }

        public string Description { get; set; }
        public string OrganizerName { get; set; }
        public bool HasUserJoined { get; set; }
    }
}