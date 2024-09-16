using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class MDAServiceViewModel
    {
        public int MDAServiceID { get; set; }

        public string MDAServiceCode { get; set; }
        
        [Display(Name = "MDA Service Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter MDA Service Name")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string MDAServiceName { get; set; }

        [Display(Name = "Rule Run")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Rule Run")]
        public int RuleRunID { get; set; }

        public string RuleRunName { get; set; }

        [Display(Name = "Frequency")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Frequency")]
        public int PaymentFrequencyID { get; set; }

        public string PaymentFrequencyName { get; set; }

        [Display(Name = "Tax Year")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Year")]
        public int TaxYear { get; set; }

        [Display(Name = "Payment Options")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Payment Options")]
        public int PaymentOptionID { get; set; }

        public string PaymentOptionName { get; set; }

        [Display(Name = "Settlement Method")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Settlement Method")]
        public int[] SettlementMethodIds { get; set; }

        public string SettlementMethodNames { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
