using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Models
{
   public class MenuViewModel
    {
        public int MenuID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter Menu Name")]
        [Display(Name = "Menu Name")]
        public string MenuName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Select Parent Menu")]
        [Display(Name = "Parent Menu")]
        public int ParentMenuID { get; set; }

        [Display(Name = "Parent Menu Name")]
        public string ParentMenuName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter Menu Url")]
        [Display(Name = "Menu Url")]
        public string MenuUrl { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter Sort Order")]
        [Display(Name = "Sort Order")]
        public int SortOrder { get; set; }

        [Display(Name = "Page Header")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Page Header")]
        public string PageHeader { get; set; }

        [Display(Name = "Short Description")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Short Description")]
        [MaxLength(500,ErrorMessage = "Max Length is 500 Characters")]
        public string ShortDescription { get; set; }

        [Display(Name = "Page Content")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Page Content")]
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
