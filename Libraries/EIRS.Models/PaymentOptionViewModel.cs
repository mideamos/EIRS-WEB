using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class PaymentOptionViewModel
    {
        public int PaymentOptionID { get; set; }

        [Display(Name = "Payment Option")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Payment Option")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string PaymentOptionName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
