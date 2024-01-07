using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class BusinessSectorViewModel
    {
        public int BusinessSectorID { get; set; }

        [Display(Name = "Business Sector")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter business Sector")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string BusinessSectorName { get; set; }

        [Display(Name = "Business Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select business type")]
        public int BusinessTypeID { get; set; }

        public string BusinessTypeName { get; set; }

        [Display(Name = "Business Category")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select applicable business category")]
        public int BusinessCategoryID { get; set; }

        public string BusinessCategoryName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
