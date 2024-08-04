using System.ComponentModel.DataAnnotations;

namespace RunBuddies.App.Models
{
    public class SignInViewModel
    {
        public SignInViewModel()
        {
            UserName = Password = ReturnUrl = "";
        }

        [Required]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string ReturnUrl { get; set; }
    }
}
