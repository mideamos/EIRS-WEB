using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class AssessmentSubGroupViewModel
    {
        public int AssessmentSubGroupID { get; set; }

        [Display(Name = "Assessment Sub Group")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Assessment Sub Group")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string AssessmentSubGroupName { get; set; }


        [Display(Name = "Assessment Group")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Assessment Group")]
        public int AssessmentGroupID { get; set; }

        public string AssessmentGroupName { get; set; }

        [Display(Name = "Asset Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Asset Type")]
        public int AssetTypeID { get; set; }

        public string AssetTypeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
