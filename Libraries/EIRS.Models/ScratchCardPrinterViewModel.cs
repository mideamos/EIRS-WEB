using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class ScratchCardPrinterViewModel
    {
        public int ScratchCardPrinterID { get; set; }

        [Display(Name = "Scratch Card Printer")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Scratch Card Printer")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string ScratchCardPrinterName { get; set; }

        [Display(Name = "Company")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Company")]
        public int CompanyID { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Agreed Unit Price")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Agreed Unit Price")]
        public decimal AgreedUnitPrice { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
