using System.ComponentModel.DataAnnotations;

namespace RunBuddies.App.Models
{
    public class RegisterViewModel
    {
        public RegisterViewModel()
        {
            Email = FirstName = LastName = Gender = "";
        }

        //Details
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        
        [Required]
        public DateOnly Birthday { get; set; }
        
        [Required]
        public string Gender { get; set; }
        
        [Required]
        public string PhoneNumber { get; set; }

        
        [Required]
        [Length(8, 20, ErrorMessage = "Username must be between 8 to 20 characters only.")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Length(10, 50, ErrorMessage = "Password must be at least 10 Characters.")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password Does Not Match.")]
        public string ConfirmPassword { get; set; }
    }
}
