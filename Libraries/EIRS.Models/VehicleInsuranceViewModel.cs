using System;
using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public sealed class VehicleInsuranceViewModel
    {
        public int VehicleInsuranceID { get; set; }

        [Display(Name = "Vehicle")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Vehicle")]
        public int VehicleID { get; set; }

        public string VehicleRIN { get; set; }


        [Display(Name = "Insurance Certificate Number")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter insurance certificate number")]
        [MaxLength(500,ErrorMessage = "Only 500 characters allowed.")]
        public string InsuranceCertificateNumber { get; set; }

        [Display(Name = "Start Date")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Expiry Date")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Expiry Date")]
        public DateTime ExpiryDate { get; set; }

        [Display(Name = "Cover Type")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Cover Type")]
        public int CoverTypeID { get; set; }

        public string CoverTypeName { get; set; }

        [Display(Name = "Insurance Status")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Insurance Status")]
        public int InsuranceStatusID { get; set; }

        public string  InsuranceStatusName { get; set; }

        [Display(Name = "Premium Amount")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Premium Amount")]
        public decimal PremiumAmount { get; set; }

        [Display(Name = "Verification Amount")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Verification Amount")]
        public decimal VerificationAmount { get; set; }

        [Display(Name = "Broker Amount")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Broker Amount")]
        public decimal BrokerAmount { get; set; }


        [Display(Name = "Status")]
        public bool Active { get; set; }

        public string ActiveText { get; set; }
    }
}
