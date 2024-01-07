using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class VehicleOwnershipViewModel
    {
        public int VehicleOwnershipID { get; set; }

        [Display(Name = "Vehicle Ownership")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter vehicle ownership type")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string VehicleOwnershipName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
