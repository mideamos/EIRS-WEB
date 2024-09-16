using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class BuildingTypeViewModel
    {
        public int BuildingTypeID { get; set; }

        [Display(Name = "Building Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Building Type")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string BuildingTypeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
