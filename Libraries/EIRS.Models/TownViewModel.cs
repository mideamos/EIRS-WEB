using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class TownViewModel
    {
        public int TownID { get; set; }

        [Display(Name = "Town Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Town Name")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string TownName { get; set; }

        [Display(Name = "Local Government Area Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Local Government Area Name")]
        public int LGAID { get; set; }

        public string LGAName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
