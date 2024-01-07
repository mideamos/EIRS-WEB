using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class BuildingCompletionViewModel
    {
        public int BuildingCompletionID { get; set; }

        [Display(Name = "Building Completion")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter level of building completion")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string BuildingCompletionName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
