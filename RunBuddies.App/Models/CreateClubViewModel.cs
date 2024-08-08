using System.ComponentModel.DataAnnotations;

namespace RunBuddies.App.Models
{
    public class CreateClubViewModel
    {
        [Required(ErrorMessage = "Club Name is required")]
        [Display(Name = "Club Name")]
        public string ClubName { get; set; }

        [Required(ErrorMessage = "Location is required")]
        public string Location { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Contact Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Display(Name = "Contact Email")]
        public string ContactEmail { get; set; }
        [Required]

        [Display(Name = "Communication Group Link")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string? CommunicationGroupLink { get; set; } // Make this nullable
    }
}