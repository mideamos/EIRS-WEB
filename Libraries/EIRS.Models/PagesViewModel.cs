using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class PagesViewModel
    {
        public int PageID { get; set; }

        [Display(Name = "Page Name")]
        public string PageName { get; set; }

        [Display(Name = "Page Header")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Page Header")]
        public string PageHeader { get; set; }

        [Display(Name = "Short Description")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Short Description")]
        [MaxLength(500, ErrorMessage = "Max Length is 500 Characters")]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "Enter Page Content")]
        [Display(Name = "Page Content")]
        public string PageContent { get; set; }

        [Display(Name = "Page Title")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Page Title")]
        public string PageTitle { get; set; }

        [Display(Name = "Meta Title")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Meta Title")]
        public string MetaTitle { get; set; }

        [Display(Name = "Meta Keywords")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Meta Keywords")]
        public string MetaKeyword { get; set; }

        [Display(Name = "Meta Description")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Meta Description")]
        public string MetaDescription { get; set; }
    }
}