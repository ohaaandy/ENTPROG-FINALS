using System.ComponentModel.DataAnnotations;

namespace RunBuddies.App.Models
{
    public class ProfileViewModel
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.Date)]
        public DateOnly Birthday { get; set; }
        public string Gender { get; set; }
        [Display(Name = "Runner Level")]
        public string? RunnerLevel { get; set; }
        [Display(Name = "Preferred Schedule")]
        public DayOfWeek? PreferredDay { get; set; }
        public string? Location { get; set; }
        [Display(Name = "Preferred Distance (km)")]
        public int? Distance { get; set; }


    }
}