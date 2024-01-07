using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class ScratchCardDealerViewModel
    {
        public int ScratchCardDealerID { get; set; }

        [Display(Name = "Scratch Card Dealer")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Scratch Card Dealer")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string ScratchCardDealerName { get; set; }

        [Display(Name = "Company")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Company")]
        public int CompanyID { get; set; }

        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [Display(Name = "Dealer Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Dealer Type")]
        public int DealerTypeID { get; set; }

        [Display(Name = "Dealer Type Name")]
        public string DealerTypeName { get; set; }

        [Display(Name = "Agreed Commission Percentage")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Agreed Commission Percentage")]
        public decimal AgreedCommissionPercentage{ get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
