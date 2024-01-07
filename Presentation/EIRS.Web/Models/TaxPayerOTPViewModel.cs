using System.ComponentModel.DataAnnotations;

namespace EIRS.Web.Models
{
    public class TaxPayerOTPViewModel
    {
        public int TaxPayerID { get; set; }

        [Display(Name = "Mobile Number")]
        public string MobileNumber { get; set; }

        public string TaxPayerName { get; set; }

        [Display(Name="OTP")]
        [Required(AllowEmptyStrings =false,ErrorMessage ="Enter OTP")]
        public int? OTP { get; set; }
    }
}