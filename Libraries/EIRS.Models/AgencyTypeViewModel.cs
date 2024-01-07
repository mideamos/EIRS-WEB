using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class AgencyTypeViewModel
    {
        public int AgencyTypeID { get; set; }

        [Display(Name = "Agency Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Agency Type")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string AgencyTypeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
