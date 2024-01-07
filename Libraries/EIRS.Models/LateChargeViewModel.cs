using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class LateChargeViewModel
    {
        public int LateChargeID { get; set; }

        [Display(Name = "Revenue Stream")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Revenue Stream")]
        public int RevenueStreamID { get; set; }

        public string RevenueStreamName { get; set; }

        [Display(Name = "Tax Year")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Year")]
        public int TaxYear { get; set; }

        [Required(ErrorMessage = "Please Enter Penalty")]
        [Display(Name = "Penalty (%)")]
        public decimal Penalty { get; set; }

        [Required(ErrorMessage = "Please Enter Interest")]
        [Display(Name = "Interest (%)")]
        public decimal Interest { get; set; }


        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
