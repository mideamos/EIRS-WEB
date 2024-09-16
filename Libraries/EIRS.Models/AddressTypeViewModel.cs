using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class AddressTypeViewModel
    {
        public int AddressTypeID { get; set; }

        [Display(Name = "Address Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Address Type")]
        [MaxLength(250,ErrorMessage = "Only 250 characters allowed.")]
        public string AddressTypeName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
