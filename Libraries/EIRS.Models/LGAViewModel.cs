using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class LGAViewModel
    {
        public int LGAID { get; set; }

        [Display(Name = "Local Government Area Name")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Local Government Area Name")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string LGAName { get; set; }

        [Display(Name = "LGA Class")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select LGA Class")]
        public int LGAClassID { get; set; }

        public string LGAClassName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
