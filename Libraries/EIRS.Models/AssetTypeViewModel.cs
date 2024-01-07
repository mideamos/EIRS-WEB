using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class AssetTypeViewModel
    {
        public int AssetTypeID { get; set; }

        [Display(Name = "Asset Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Asset Type")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string AssetTypeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
