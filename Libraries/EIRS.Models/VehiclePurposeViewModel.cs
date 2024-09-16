using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class VehiclePurposeViewModel
    {
        public int VehiclePurposeID { get; set; }

        [Display(Name = "Vehicle Purpose")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter vehicle purpose")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string VehiclePurposeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
