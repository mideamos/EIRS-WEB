using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class AssessmentItemSubCategoryViewModel
    {
        public int AssessmentItemSubCategoryID { get; set; }

        [Display(Name = "Assessment Item Sub Category")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Assessment Item SubCategory")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string AssessmentItemSubCategoryName { get; set; }

        [Display(Name = "Assessment Item Category")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Assessment Item Category")]
        public int AssessmentItemCategoryID { get; set; }

        public string AssessmentItemCategoryName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
