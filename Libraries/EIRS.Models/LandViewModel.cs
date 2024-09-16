using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class LandViewModel
    {
        public int LandID { get; set; }
        public string LandRIN { get; set; }

        [Display(Name = "Plot Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Plot Number")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string PlotNumber { get; set; }

        [Display(Name = "Street Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter name of street which Land is locted")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string StreetName { get; set; }

        [Display(Name = "Town")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select town in which Land is located")]
        public int TownID { get; set; }

        public string TownName { get; set; }

        [Display(Name = "LGA")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select LGA which Land is located")]
        public int LGAID { get; set; }

        public string LGAName { get; set; }

        [Display(Name = "Ward")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select ward in which Land is located")]
        public int WardID { get; set; }

        public string WardName { get; set; }

        [Display(Name = "Asset Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Asset Type")]
        public int AssetTypeID { get; set; }

        public string AssetTypeName { get; set; }

        [Display(Name = "Land Size - Length (Sqm)")]
        public decimal? LandSize_Length { get; set; }

        [Display(Name = "Land Size - Width (Sqm)")]
        public decimal? LandSize_Width { get; set; }

        [Display(Name = "C_OF_O_Ref")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string C_OF_O_Ref { get; set; }

        [Display(Name = "Land Purpose")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select what Land is primarily used for")]
        public int LandPurposeID { get; set; }

        public string LandPurposeName { get; set; }

        [Display(Name = "Land Function")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Land Function")]
        public int LandFunctionID { get; set; }

        public string LandFunctionName { get; set; }

        [Display(Name = "Land Ownership")]
        public int? LandOwnershipID { get; set; }

        public string LandOwnershipName { get; set; }

        [Display(Name = "Land Development")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Land Development")]
        public int LandDevelopmentID { get; set; }

        public string LandDevelopmentName { get; set; }

        [Display(Name = "Latitude")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string Latitude { get; set; }

        [Display(Name = "Longitude")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string Longitude { get; set; }

        [Display(Name = "Value of Land")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Value of Land")]
        public decimal ValueOfLand { get; set; }


        [Display(Name = "Street Condition")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Street Condition")]
        public int LandStreetConditionID { get; set; }

        public string LandStreetConditionName { get; set; }

        [Display(Name = "Neighborhood")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Neighborhood")]
        public string Neighborhood { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }

    public sealed class TPLandViewModel : LandViewModel
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
