using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class BuildingUnitViewModel
    {
        public int BuildingUnitID { get; set; }

        [Display(Name = "Unit Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Unit Number")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string UnitNumber { get; set; }

        [Display(Name = "Unit Purpose")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Unit Purpose")]
        public int UnitPurposeID { get; set; }

        public string UnitPurposeName { get; set; }

        [Display(Name = "Unit Function")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Unit Function")]
        public int UnitFunctionID { get; set; }

        public string UnitFunctionName { get; set; }

        [Display(Name = "Unit Occupancy")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Unit Occupancy")]
        public int UnitOccupancyID { get; set; }

        public string UnitOccupancyName { get; set; }

        [Display(Name = "Unit Size")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Unit Size")]
        public int SizeID { get; set; }

        public string SizeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
