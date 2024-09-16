using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class LandPurposeViewModel
    {
        public int LandPurposeID { get; set; }

        [Display(Name = "Land Purpose")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Land Purpose")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string LandPurposeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
