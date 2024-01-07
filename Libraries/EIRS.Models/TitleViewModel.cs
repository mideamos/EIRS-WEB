using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class TitleViewModel
    {
        public int TitleID { get; set; }

        [Display(Name = "Title")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Title Name")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string TitleName { get; set; }

        [Display(Name = "Gender")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Gender")]
        public int GenderID { get; set; }

        public string GenderName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
