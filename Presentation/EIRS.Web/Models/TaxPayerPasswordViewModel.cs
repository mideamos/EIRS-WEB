using System.ComponentModel.DataAnnotations;

namespace EIRS.Web.Models
{
    public class TaxPayerPasswordViewModel
    {
        public int TaxPayerID { get; set; }
        public string TaxPayerTypeName { get; set; }
        public string TaxPayerName { get; set; }
        public string TaxPayerTIN { get; set; }
        public string TaxPayerRIN { get; set; }
        public string MobileNumber { get; set; }
        public string ContactAddress { get; set; }

        public string AssetTypeName { get; set; }
        public string AssetRIN { get; set; }
        public string AssetName { get; set; }
        public string TaxPayerRoleName { get; set; }

        [Required(ErrorMessage = "Please Enter New Password")]
        [Display(Name = "Enter New Password")]
        public string NewPassword { get; set; }


        [Required(ErrorMessage = "Please Enter Confirm Password")]
        [Display(Name = "Re-Enter New Password")]
        [Compare(nameof(NewPassword), ErrorMessage = "New Password And Confirm Does Not Matched")]
        public string ConfirmPassword { get; set; }
    }
}