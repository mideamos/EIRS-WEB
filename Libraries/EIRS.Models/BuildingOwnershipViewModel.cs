using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class BuildingOwnershipViewModel
    {
        public int BuildingOwnershipID { get; set; }

        [Display(Name = "Building Ownership")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter building ownership type of this building")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string BuildingOwnershipName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
