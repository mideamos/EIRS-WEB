using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class LandFunctionViewModel
    {
        public int LandFunctionID { get; set; }

        [Display(Name = "Land Function")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Land Function")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string LandFunctionName { get; set; }

        [Display(Name = "Land Purpose")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Land Purpose")]
        public int LandPurposeID { get; set; }

        public string LandPurposeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
