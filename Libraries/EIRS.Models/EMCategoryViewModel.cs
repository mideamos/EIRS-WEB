using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class EMCategoryViewModel
    {
        public int CategoryID { get; set; }

        [Display(Name = "Category")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Category")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string CategoryName { get; set; }

      
        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
