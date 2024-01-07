using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class LandOwnershipViewModel
    {
        public int LandOwnershipID { get; set; }

        [Display(Name = "Land Ownership")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter land ownership type of this land")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string LandOwnershipName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
