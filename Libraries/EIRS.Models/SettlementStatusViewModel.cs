using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class SettlementStatusViewModel
    {
        public int SettlementStatusID { get; set; }

        [Display(Name = "Settlement Status Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Settlement Status Name")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string SettlementStatusName { get; set; }

        [Display(Name = "Settlement Status Description")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Status Description")]
        [MaxLength(200, ErrorMessage = "Only 200 characters allowed.")]
        public string StatusDescription { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}