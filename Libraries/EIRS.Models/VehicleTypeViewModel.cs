using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class VehicleTypeViewModel
    {
        public int VehicleTypeID { get; set; }

        [Display(Name = "Vehicle Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter vehicle type name")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string VehicleTypeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
