using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class SizeViewModel
    {
        public int SizeID { get; set; }

        [Display(Name = "Size")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Size")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string SizeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
