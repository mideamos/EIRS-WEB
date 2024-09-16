using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class VehicleSubTypeViewModel
    {
        public int VehicleSubTypeID { get; set; }

        [Display(Name = "Vehicle SubType")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Vehicle SubType")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string VehicleSubTypeName { get; set; }

        [Display(Name = "Vehicle Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Vehicle Type")]
        public int VehicleTypeID { get; set; }

        public string VehicleTypeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
