using RunBuddies.DataModel;
using System.ComponentModel.DataAnnotations;

namespace RunBuddies.App.Models
{
    public class ClubViewModel
    {
        public int ClubID { get; set; }
        [Required]
        public string ClubName { get; set; }
        [Required]
        public string Location { get; set; }
        public string Description { get; set; }
        [Required]
        [EmailAddress]
        public string ContactEmail { get; set; }
        public bool IsModerator { get; set; }
        public bool IsMember { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL")]
        [Display(Name = "Communication Group Link (Optional)")]
        public string CommunicationGroupLink { get; set; }
        public List<Event> Events { get; set; }
    }
}