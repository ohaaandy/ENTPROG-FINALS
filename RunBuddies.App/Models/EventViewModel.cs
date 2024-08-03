using System;
using System.ComponentModel.DataAnnotations;

namespace RunBuddies.ViewModels
{
    public class EventViewModel
    {
        public int EventID { get; set; }

        [Required(ErrorMessage = "Event name is required")]
        [Display(Name = "Event Name")]
        public string EventName { get; set; }

        [Required(ErrorMessage = "Date and time are required")]
        [Display(Name = "Date and Time")]
        [DataType(DataType.DateTime)]
        public DateTime DateTime { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Organizer")]
        public string OrganizerName { get; set; }

        public bool HasUserJoined { get; set; }

        // New property for the event image URL
        [Display(Name = "Event Image")]
        public string ImageUrl { get; set; }
    }
}