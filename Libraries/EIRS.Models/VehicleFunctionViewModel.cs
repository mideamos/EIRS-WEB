using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class VehicleFunctionViewModel
    {
        public int VehicleFunctionID { get; set; }

        [Display(Name = "Vehicle Function")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Vehicle Function")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string VehicleFunctionName { get; set; }

        [Display(Name = "Vehicle Purpose")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Vehicle Purpose")]
        public int VehiclePurposeID { get; set; }

        public string VehiclePurposeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
