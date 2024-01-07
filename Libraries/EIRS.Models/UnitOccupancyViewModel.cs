using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class UnitOccupancyViewModel
    {
        public int UnitOccupancyID { get; set; }

        [Display(Name = "Unit Occupancy")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Unit Occupancy")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string UnitOccupancyName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
