using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class GovernmentTypeViewModel
    {
        public int GovernmentTypeID { get; set; }

        [Display(Name = "Government Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Government Type")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string GovernmentTypeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
