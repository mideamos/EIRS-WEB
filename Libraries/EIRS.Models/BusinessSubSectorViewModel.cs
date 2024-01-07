using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class BusinessSubSectorViewModel
    {
        public int BusinessSubSectorID { get; set; }

        [Display(Name = "Business Sub Sector")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter business Sub Sector")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string BusinessSubSectorName { get; set; }

        [Display(Name = "Business Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select business type")]
        public int BusinessTypeID { get; set; }

        public string BusinessTypeName { get; set; }

        [Display(Name = "Business Category")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select applicable business category")]
        public int BusinessCategoryID { get; set; }

        public string BusinessCategoryName { get; set; }

        [Display(Name = "Business Sector")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select business sector")]
        public int BusinessSectorID { get; set; }

        public string BusinessSectorName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
