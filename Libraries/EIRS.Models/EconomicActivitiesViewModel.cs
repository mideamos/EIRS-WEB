using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class EconomicActivitiesViewModel
    {
        public int EconomicActivitiesID { get; set; }

        [Display(Name = "Economic Activities")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "enter economic activities")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string EconomicActivitiesName { get; set; }

        [Display(Name = "Tax Payer Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Payer Type")]
        public int TaxPayerTypeID { get; set; }

        public string TaxPayerTypeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
