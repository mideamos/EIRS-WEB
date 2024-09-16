using System.ComponentModel.DataAnnotations;

namespace EIRS.Web.Models
{
    public class TaxPayerReportViewModel
    {
        [Display(Name = "Tax Year")]
        public int TaxYear { get; set; }

        [Display(Name = "Tax Month")]
        public int? TaxMonth { get; set; }

        [Required(ErrorMessage = "Select Tax Payer Type")]
        [Display(Name = "Tax Payer Type")]
        public int TaxPayerTypeID { get; set; }

        [Required(ErrorMessage = "Select Tax Payer")]
        [Display(Name = "Tax Payer")]
        public int TaxPayerID { get; set; }
    }
}