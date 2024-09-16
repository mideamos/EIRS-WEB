using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class EMRevenueHeadViewModel
    {
        public int RevenueHeadID { get; set; }

        [Display(Name = "Revenue Head")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Revenue Head")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string RevenueHeadName { get; set; }


        [Display(Name = "Category")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Category")]
        public int CategoryID { get; set; }


        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
