using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class BuildingFunctionViewModel
    {
        public int BuildingFunctionID { get; set; }

        [Display(Name = "Building Function")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Building Function")]
        [MaxLength(25,ErrorMessage = "Only 25 characters allowed.")]
        public string BuildingFunctionName { get; set; }

        [Display(Name = "Building Purpose")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Building Purpose")]
        public int BuildingPurposeID { get; set; }

        public string BuildingPurposeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
