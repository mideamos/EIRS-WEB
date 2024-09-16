using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class AssessmentItemCategoryViewModel
    {
        public int AssessmentItemCategoryID { get; set; }

        [Display(Name = "Assessment Item Category")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Assessment Item Category")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string AssessmentItemCategoryName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
