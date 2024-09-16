using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class TaxPayerTypeViewModel
    {
        public int TaxPayerTypeID { get; set; }

        [Display(Name = "Tax Payer Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Tax Payer Type")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string TaxPayerTypeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
