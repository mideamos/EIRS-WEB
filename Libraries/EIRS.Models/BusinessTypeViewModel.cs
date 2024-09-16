using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class BusinessTypeViewModel
    {
        public int BusinessTypeID { get; set; }

        [Display(Name = "Business Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter business type name")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string BusinessTypeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
