using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class BuildingPurposeViewModel
    {
        public int BuildingPurposeID { get; set; }

        [Display(Name = "Building Purpose")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Building Purpose")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string BuildingPurposeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
