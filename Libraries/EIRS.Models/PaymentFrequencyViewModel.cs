using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class PaymentFrequencyViewModel
    {
        public int PaymentFrequencyID { get; set; }

        [Display(Name = "Payment Frequency")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Payment Frequency")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string PaymentFrequencyName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
