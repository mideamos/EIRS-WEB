using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class SettlementMethodViewModel
    {
        public int SettlementMethodID { get; set; }

        [Display(Name = "Settlement Method")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Settlement Method")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string SettlementMethodName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
