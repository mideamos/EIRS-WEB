using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class EMBankViewModel
    {
        public int BankID { get; set; }

        [Display(Name = "Bank")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Bank")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string BankName { get; set; }

        [Display(Name = "Bank Account Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Bank Account Number")]
        [MaxLength(250, ErrorMessage = "Only 250 characters allowed.")]
        public string BankAccountNumber { get; set; }

        [Display(Name = "Bank Description")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Bank Description")]
        public string BankDescription { get; set; }


        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
