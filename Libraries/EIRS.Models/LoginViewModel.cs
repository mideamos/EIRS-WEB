using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed partial class LoginViewModel
    {
        public string returnUrl { get; set; }

        [Required(ErrorMessage = "Please enter email address")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [Required(ErrorMessage = "Enter your password")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}
