using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class LandDevelopmentViewModel
    {
        public int LandDevelopmentID { get; set; }

        [Display(Name = "Land Development")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Land Development")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string LandDevelopmentName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
