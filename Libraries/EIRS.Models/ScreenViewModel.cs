using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class ScreenViewModel
    {
        public int ScreenID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter Screen Name")]
        [Display(Name = "Screen Name")]
        public string ScreenName { get; set; }

        [Display(Name = "Screen Url")]
        public string ScreenUrl { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }

      
    }
}
