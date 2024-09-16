using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class EMIGRClassificationViewModel
    {
        public long IGRClassificationID { get; set; }

        [Display(Name = "Category")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Category")]
        public int CategoryID { get; set; }

        [Display(Name = "Revenue Head")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Revenue Head")]
        public int RevenueHeadID { get; set; }

        [Display(Name = "Tax Month")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Month")]
        public int TaxMonth { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

    }

    public class ClassificationEntryViewModel
    {
        public long ClassificationID { get; set; }

        public string EntryIds { get; set; }

    }
}
