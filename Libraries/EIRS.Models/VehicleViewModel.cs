using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class VehicleViewModel
    {
        public int VehicleID { get; set; }
        public string VehicleRIN { get; set; }

        [Display(Name = "Vehicle Reg Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter vehicle FRSC registration number")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string VehicleRegNumber { get; set; }

        [Display(Name = "VIN")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter vehicle VIN")]
        [MaxLength(17, ErrorMessage = "Only 17 characters allowed.")]
        public string VIN { get; set; }

        [Display(Name = "Vehicle Description")]
        [MaxLength(5000, ErrorMessage = "Only 5000 characters allowed.")]
        public string VehicleDescription { get; set; }


        [Display(Name = "Asset Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Asset Type")]
        public int AssetTypeID { get; set; }

        public string AssetTypeName { get; set; }

        [Display(Name = "Vehicle Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select the type of Vehicle")]
        public int VehicleTypeID { get; set; }

        public string VehicleTypeName { get; set; }

        [Display(Name = "Vehicle Sub Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select vehicle sub type")]
        public int VehicleSubTypeID { get; set; }

        public string VehicleSubTypeName { get; set; }


        [Display(Name = "Registration LGA")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select registration lga")]
        public int LGAID { get; set; }

        public string LGAName { get; set; }


        [Display(Name = "Vehicle Purpose")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select what Vehicle is primarily used for")]
        public int VehiclePurposeID { get; set; }

        public string VehiclePurposeName { get; set; }

        [Display(Name = "Vehicle Function")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Vehicle Function")]
        public int VehicleFunctionID { get; set; }

        public string VehicleFunctionName { get; set; }

        [Display(Name = "Vehicle Ownership")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Vehicle Ownership")]
        public int VehicleOwnershipID { get; set; }

        public string VehicleOwnershipName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }

    public sealed class TPVehicleViewModel : VehicleViewModel
    {
        [Display(Name = "Tax Payer Role")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Payer Role")]
        public int TaxPayerRoleID { get; set; }

        public int TaxPayerID { get; set; }
        public int TaxPayerTypeID { get; set; }
        public string TaxPayerTypeName { get; set; }
        public string TaxPayerName { get; set; }
        public string TaxPayerTIN { get; set; }
        public string TaxPayerRIN { get; set; }
        public string MobileNumber { get; set; }
        public string ContactAddress { get; set; }
    }
}
