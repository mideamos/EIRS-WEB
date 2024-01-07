using System.ComponentModel.DataAnnotations;

namespace EIRS.Web.Models
{
    public class ReviewViewModel
    {
        public int TaxPayerID { get; set; }
        public int TaxPayerTypeID { get; set; }
        public string TaxPayerTypeName { get; set; }
        public string TaxPayerName { get; set; }
        public string TaxPayerTIN { get; set; }
        public string TaxPayerRIN { get; set; }
        public string ContactNumber { get; set; }
        public string ContactAddress { get; set; }

        [Display(Name = "Notes")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Notes")]
        public string Notes { get; set; }

        [Display(Name = "Review Status")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Review Status")]
        public int ReviewStatusID { get; set; }
    }
}