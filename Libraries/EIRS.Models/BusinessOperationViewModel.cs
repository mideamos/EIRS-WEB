using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class BusinessOperationViewModel
    {
        public int BusinessOperationID { get; set; }

        [Display(Name = "Business Operation")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter business Operation")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string BusinessOperationName { get; set; }

        [Display(Name = "Business Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select business type")]
        public int BusinessTypeID { get; set; }

        public string BusinessTypeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
