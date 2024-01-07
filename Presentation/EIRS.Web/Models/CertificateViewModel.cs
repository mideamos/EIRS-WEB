using System;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace EIRS.Web.Models
{
    public sealed class GenerateCertificateViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Certificate Type")]
        [Display(Name = "Certificate Type")]
        public int CertificateTypeID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Payer Type")]
        [Display(Name = "Tax Payer Type")]
        public int TaxPayerTypeID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Tax Payer RIN")]
        [Display(Name = "Tax Payer RIN")]
        public int TaxPayerID { get; set; }
        public string TaxPayerName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Profile")]
        [Display(Name = "Profile")]
        public int ProfileID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Asset")]
        [Display(Name = "Asset")]
        public int AssetID { get; set; }
    }

    public sealed class UpdateCertificateViewModel
    {
        public long CertificateID { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Display(Name = "Other Information")]
        public string OtherInformation { get; set; }

        [Display(Name = "Signer")]
        public int? SignerID { get; set; }

        [Display(Name = "Signer Role")]
        public int? SignerRoleID { get; set; }

        [Display(Name = "QR Code")]
        public int? QRCodeID { get; set; }
    }
}