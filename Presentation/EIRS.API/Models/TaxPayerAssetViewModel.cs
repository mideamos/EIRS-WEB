using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRS.API.Models
{
    public class TaxPayerAssetViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tax Payer Required")]
        public int TaxPayerID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Asset Required")]
        public int AssetID { get; set; }

        [Display(Name = "Tax Payer Role")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Payer Role")]
        public int TaxPayerRoleID { get; set; }

        public int BuildingUnitID { get; set; }
    }
}