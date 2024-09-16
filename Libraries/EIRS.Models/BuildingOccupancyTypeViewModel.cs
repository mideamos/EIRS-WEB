using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class BuildingOccupancyTypeViewModel
    {
        public int BuildingOccupancyTypeID { get; set; }

        [Display(Name = "Building Occupancy Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Building Occupancy Type")]
        [MaxLength(50, ErrorMessage = "Only 50 characters allowed.")]
        public string BuildingOccupancyTypeName { get; set; }

        [Display(Name = "Building Occupancy")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Building Occupancy")]
        public int BuildingOccupancyID { get; set; }

        public string BuildingOccupancyName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
