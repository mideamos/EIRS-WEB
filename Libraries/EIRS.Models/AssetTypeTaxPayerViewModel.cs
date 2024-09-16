using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class AssetTypeTaxPayerViewModel
    {
        [Display(Name = "Asset Type")]
        public int AssetTypeID { get; set; }

        public string AssetTypeName { get; set; }

        [Display(Name = "Tax Payer Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Payer Type")]
        public int TaxPayerTypeID { get; set; }

        [Display(Name = "Tax Payer Role")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Payer Role")]
        public int TaxPayerRoleID { get; set; }

        public string TaxPayerIds { get; set; }
    }
}
