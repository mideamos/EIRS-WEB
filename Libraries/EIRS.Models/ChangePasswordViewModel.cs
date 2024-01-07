using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage ="Please Enter Old Password")]
        [Display(Name ="Enter Old Password")]
        public string OldPassword { get; set; }


        [Required(ErrorMessage = "Please Enter New Password")]
        [Display(Name = "Enter New Password")]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "Please Enter Confirm Password")]
        [Display(Name = "Re-Enter New Password")]
        [Compare(nameof(NewPassword), ErrorMessage = "New Password And Confirm Does Not Matched")]
        public string ConfirmPassword { get; set; }
    }
}
