using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class LandStreetConditionViewModel
    {
        public int LandStreetConditionID { get; set; }

        [Display(Name = "Street Condition")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Street Condition")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string LandStreetConditionName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
