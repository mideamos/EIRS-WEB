using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class UnitFunctionViewModel
    {
        public int UnitFunctionID { get; set; }

        [Display(Name = "Unit Function")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Unit Function")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string UnitFunctionName { get; set; }

        [Display(Name = "Unit Purpose")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Unit Purpose")]
        public int UnitPurposeID { get; set; }

        public string UnitPurposeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
