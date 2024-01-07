using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class BuildingOccupancyViewModel
    {
        public int BuildingOccupancyID { get; set; }

        [Display(Name = "Building Occupancy")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Building Occupancy")]
        [MaxLength(25,ErrorMessage = "Only 25 characters allowed.")]
        public string BuildingOccupancyName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
