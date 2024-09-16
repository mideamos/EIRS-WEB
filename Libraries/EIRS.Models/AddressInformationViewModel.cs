using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class AddressInformationViewModel
    {
        public int TaxPayerID { get; set; }

        public string TaxPayerRIN { get; set; }

        public string TaxPayerName { get; set; }

        [Display(Name = "Address Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Address Type")]
        public int AddressTypeID { get; set; }


        [Display(Name = "Building")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Building")]
        public int BuildingID { get; set; }

    }
}
