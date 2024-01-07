using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class BuildingViewModel
    {
        public int BuildingID { get; set; }
        public string BuildingRIN { get; set; }
        public string BuildingTAGNumber { get; set; }

        [Display(Name = "Building Name")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string BuildingName { get; set; }

        [Display(Name = "Building Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter number associated with building")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string BuildingNumber { get; set; }

        [Display(Name = "Street Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter name of street which building is locted")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string StreetName { get; set; }

        [Display(Name = "Off Street Name")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string OffStreetName { get; set; }


        [Display(Name = "Town")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select town in which building is located")]
        public int TownID { get; set; }

        public string TownName { get; set; }

        [Display(Name = "LGA")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select LGA which building is located")]
        public int LGAID { get; set; }

        public string LGAName { get; set; }

        [Display(Name = "Ward")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select ward in which building is located")]
        public int WardID { get; set; }

        public string WardName { get; set; }

        [Display(Name = "Asset Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Asset Type")]
        public int AssetTypeID { get; set; }

        public string AssetTypeName { get; set; }

        [Display(Name = "Building Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select the type of building")]
        public int BuildingTypeID { get; set; }

        public string BuildingTypeName { get; set; }

        [Display(Name = "Building Completion")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select completion level of building")]
        public int BuildingCompletionID { get; set; }

        public string BuildingCompletionName { get; set; }


        [Display(Name = "Building Purpose")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select what building is primarily used for")]
        public int BuildingPurposeID { get; set; }

        public string BuildingPurposeName { get; set; }


        [Display(Name = "Building Ownership")]
        public int? BuildingOwnershipID { get; set; }

        public string BuildingOwnershipName { get; set; }

        [Display(Name = "No of Units")]
        public int NoOfUnits { get; set; }

        [Display(Name = "Building Size - Length (Sqm)")]
        public decimal? BuildingSize_Length { get; set; }

        [Display(Name = "Building Size - Width (Sqm)")]
        public decimal? BuildingSize_Width { get; set; }

        [Display(Name = "Latitude")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string Latitude { get; set; }

        [Display(Name = "Longitude")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string Longitude { get; set; }


        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }

    public sealed class TPBuildingViewModel : BuildingViewModel
    {
        [Display(Name = "Tax Payer Role")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Payer Role")]
        public int TaxPayerRoleID { get; set; }

        [Display(Name = "Building Unit")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Building Unit")]
        public int BuildingUnitID { get; set; }

        public int TaxPayerID { get; set; }
        public int TaxPayerTypeID { get; set; }
        public string TaxPayerTypeName { get; set; }
        public string TaxPayerName { get; set; }
        public string TaxPayerTIN { get; set; }
        public string TaxPayerRIN { get; set; }
        public string MobileNumber { get; set; }
        public string ContactAddress { get; set; }
    }

    public sealed class BuildingWithLandViewModel : BuildingViewModel
    {
        public int LandID { get; set; }

        public int[] BuildingUnitID { get; set; }
    }
}
