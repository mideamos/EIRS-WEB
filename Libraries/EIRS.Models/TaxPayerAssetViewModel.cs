using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class TaxPayerAssetViewModel
    {
        public int TaxPayerID { get; set; }

        public int AssetID { get; set; }

        public string TaxPayerRIN { get; set; }
        public string TaxPayerName { get; set; }

        public string AssetRIN { get; set; }
        public string AssetName { get; set; }

        [Display(Name = "Tax Payer Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Payer Type")]
        public int TaxPayerTypeID { get; set; }


        [Display(Name = "Asset Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Asset Type")]
        public int AssetTypeID { get; set; }


        [Display(Name = "Tax Payer Role")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Payer Role")]
        public int TaxPayerRoleID { get; set; }

        public string AssetIds { get; set; }

        public string TaxPayerIds { get; set; }

        public int BuildingUnitID { get; set; }

        public bool Active { get; set; }
    }
}
