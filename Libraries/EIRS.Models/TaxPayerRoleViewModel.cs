using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class TaxPayerRoleViewModel
    {
        public int TaxPayerRoleID { get; set; }

        [Display(Name = "Tax Payer Role")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter role relationship between tax payer and assets")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string TaxPayerRoleName { get; set; }

        [Display(Name = "Tax Payer Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Payer Type")]
        public int TaxPayerTypeID { get; set; }

        public string TaxPayerTypeName { get; set; }


        [Display(Name = "Asset Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Asset Type")]
        public int AssetTypeID { get; set; }

        public string AssetTypeName { get; set; }

        [Display(Name = "Multi Link Allowed")]
        public bool isMultiLinkable { get; set; }

        public string isMultiLinkableText { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
