using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class UnitPurposeViewModel
    {
        public int UnitPurposeID { get; set; }

        [Display(Name = "Unit Purpose")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Unit Purpose")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string UnitPurposeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
