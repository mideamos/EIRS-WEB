using System;
using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class VehicleLicenseViewModel
    {
        public int VehicleLicenseID { get; set; }

        [Display(Name = "Vehicle")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Vehicle")]
        public int VehicleID { get; set; }

        public string VehicleRIN { get; set; }

        [Display(Name = "License Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter license number")]
        [MaxLength(500,ErrorMessage = "Only 500 characters allowed.")]
        public string LicenseNumber { get; set; }

        [Display(Name = "Start Date")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Expiry Date")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Expiry Date")]
        public DateTime ExpiryDate { get; set; }

        [Display(Name = "Insurance Certificate Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Insurance Certificate Number")]
        public int VehicleInsuranceID { get; set; }

        public string InsuranceCertificateNumber { get; set; }

        public string InsuranceStatusName { get; set; }

        [Display(Name = "License Status")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select License Status")]
        public int LicenseStatusID { get; set; }

        public string LicenseStatusName { get; set; }

        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
