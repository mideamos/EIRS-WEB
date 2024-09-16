using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Models
{
   public  class APLandViewModel
    {

        public int LandID { get; set; }

        [Display(Name = "Plot Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter number associated with Land")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string PlotNumber { get; set; }

        [Display(Name = "Neighborhood")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Neighborhood associated with Land")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string Neighborhood { get; set; }
        

        [Display(Name = "Land Purpose")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select what Land is primarily used for")]
        public int LandPurposeID { get; set; }

        public string LandPurposeName { get; set; }

        [Display(Name = "C_OF_O_Ref Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter name of C_OF_O_Ref which building is locted")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string C_OF_O_Ref { get; set; }

        [Display(Name = "LandOccupier Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter name of LandOccupier which building is locted")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string LandOccupier { get; set; }

        [Display(Name = "LandRIN Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter name of LandRIN which building is locted")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string LandRIN { get; set; }

        [Display(Name = "Land Street Condition")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select what Land is primarily used for")]
        public int LandStreetConditionID { get; set; }

        public string LandStreetConditionName { get; set; }

        [Display(Name = "Land Purpose")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select what Land is primarily used for")]
        public int LandDevelopmentID { get; set; }

        public string LandDevelopmentName { get; set; }

        [Display(Name = "Land Function")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select what Land is primarily used for")]
        public int LandFunctionID { get; set; }

        public string LandFunctionName { get; set; }

        [Display(Name = "Land Ownership")]
        public int? LandOwnershipID { get; set; }

        public string LandOwnershipName { get; set; }

        [Display(Name = "Building Size - Length (Sqm)")]
        public decimal? LandSize_Length { get; set; }

        [Display(Name = "Building Size - Width (Sqm)")]
        public decimal? LandSize_Width { get; set; }

        [Display(Name = "ValueOfLand")]
        public decimal? ValueOfLand { get; set; }

        [Display(Name = "Street Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter name of street which building is locted")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string StreetName { get; set; }

        
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
}
