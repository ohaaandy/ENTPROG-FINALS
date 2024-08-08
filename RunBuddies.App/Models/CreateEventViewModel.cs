using System.ComponentModel.DataAnnotations;

namespace RunBuddies.App.Models
{
    public class CreateEventViewModel
    {
        public int ClubID { get; set; }

        [Required(ErrorMessage = "Event Name is required")]
        public string EventName { get; set; }

        [Required(ErrorMessage = "Event Type is required")]
        public string EventType { get; set; }

        [Required(ErrorMessage = "Date and Time is required")]
        public DateTime DateTime { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        public string Description { get; set; }
    }
}
