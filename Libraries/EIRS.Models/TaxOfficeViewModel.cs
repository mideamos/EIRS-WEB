using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class TaxOfficeViewModel
    {
        public int TaxOfficeID { get; set; }

        [Display(Name = "Tax Office")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Tax Office")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string TaxOfficeName { get; set; }

        [Display(Name = "Approver 1")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Approver 1")]
        public int Approver1 { get; set; }
        [Display(Name = "Zone")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Zone")]
        public int Zone { get; set; }

        [Display(Name = "Approver 2")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Approver 2")]
        public int Approver2 { get; set; }

        [Display(Name = "Approver 3")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Approver 3")]
        public int Approver3 { get; set; } 
        [Display(Name = "IncomeDirector")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Income Director")]
        public int IncomeDirector { get; set; }
        [Display(Name = "Office Manager")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Office Manager")]
        public int OfficeManager { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
        public string ZoneName { get; set; }
    }
}
