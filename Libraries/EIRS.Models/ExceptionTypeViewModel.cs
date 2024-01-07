using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class ExceptionTypeViewModel
    {
        public int ExceptionTypeID { get; set; }

        [Display(Name = "Exception Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Exception Type")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string ExceptionTypeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}