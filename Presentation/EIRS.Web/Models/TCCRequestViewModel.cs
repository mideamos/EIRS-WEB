using EIRS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRS.Web.Models
{
    public sealed class TCCRequestViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Tax Payer RIN")]
        [Display(Name = "Tax Payer RIN")]
        public int TaxPayerID { get; set; }
        public string TaxPayerName { get; set; }

        [Display(Name = "Year")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Year")]
        public int TaxYear { get; set; }
    }

    public sealed class ValidateTaxPayerInformationViewModel : IndividualViewModel
    {
        public long VTPInformationID { get; set; }
        public long RequestID { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }
    }

    public sealed class ValidateTaxPayerIncomeViewModel
    {
        public long VTPIncomeID { get; set; }

        public long TCCID { get; set; }
        public long RequestID { get; set; }
        public int TaxPayerID { get; set; }
        public int TaxPayerTypeID { get; set; }
        public string IndividualRin { get; set; }

        [Display(Name = "Tax Payer Type")]
        public string TaxPayerTypeName { get; set; }

        [Display(Name = "Tax Payer")]
        public string TaxPayerName { get; set; }

        [Display(Name = "Tax Year")]
        public int TaxYear { get; set; }
        [Display(Name = "Do You Wish To Add Bussiness Name To Your TCC")]
        public bool needBusinessName { get; set; }
        [Display(Name = "Request Ref No")]
        public string RequestRefNo { get; set; }

        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; }

        [Display(Name = "Source Of Income")]
        public string SourceOfIncome { get; set; }

        [Display(Name = "Notes")]
        public string CertificateNotes { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }
    }

    public sealed class GenerateTCCDetailViewModel
    {
        public long GTCCDetailID { get; set; }
        public long RequestID { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }
    }

    public sealed class PrepareTCCDraftViewModel
    {
        public long PTCCDraftID { get; set; }
        public long RequestID { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }

        [Display(Name = "Reason")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Reason")]
        public string Reason { get; set; }

        [Display(Name = "Location")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Location")]
        public string Location { get; set; }

        //[Display(Name = "Does Not Expiry")]
        //public bool IsExpirable { get; set; }

        [Display(Name = "Expiry Date")]
        public string ExpiryDate { get; set; }
    }

    public class GenerateViewModel
    {
        public long RGID { get; set; }
        public long CGID { get; set; }

        public long RequestID { get; set; }
        public long CertificateID { get; set; }

        public int SEDE_DocumentID { get; set; }

        public int SEDE_OrganizationID { get; set; }

        public int PDFTemplateID { get; set; }

        [Display(Name = "Reason")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Reason")]
        public string Reason { get; set; }

        [Display(Name = "Location")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Location")]
        public string Location { get; set; }

        [Display(Name = "Does Not Expiry")]
        public bool IsExpirable { get; set; }

        [Display(Name = "Expiry Date")]
        public DateTime? ExpiryDate { get; set; }

        [Display(Name = "Generate Notes")]
        public string GenerateNotes { get; set; }
        public string GeneratedPath { get; set; }
    }

    public class ValidateViewModel
    {
        public long RVID { get; set; }
        public long CVID { get; set; }
        public long RequestID { get; set; }
        public long CertificateID { get; set; }


        [Display(Name = "Validate Notes")]
        public string ValidateNotes { get; set; }
    }

    public class SignVisibleViewModel
    {
        public long RSVID { get; set; }
        public long CSVID { get; set; }

        public long RequestID { get; set; }
        public long CertificateID { get; set; }

        public string AdditionalSignatureLocation { get; set; }

        public string SavedSignaturePath { get; set; }

        public float DocumentWidth { get; set; }

        public int SignSourceID { get; set; }

        [Display(Name = "Sign Visible Notes")]
        public string SignNotes { get; set; }

        public string ImgSrc { get; set; }
    }

    public class SignDigitalViewModel
    {
        public long RSDID { get; set; }
        public long CSDID { get; set; }

        public long RequestID { get; set; }
        public long CertificateID { get; set; }


        [Display(Name = "Sign Digital Notes")]
        public string SignNotes { get; set; }
    }

    public class SealViewModel
    {
        public long RSID { get; set; }
        public long CSID { get; set; }

        public long RequestID { get; set; }
        public long CertificateID { get; set; }


        [Display(Name = "Seal Notes")]
        public string SealNotes { get; set; }
    }

    public class IssueViewModel
    {
        public long RIID { get; set; }
        public long CIID { get; set; }

        public long RequestID { get; set; }
        public long CertificateID { get; set; }


        [Display(Name = "Issue Notes")]
        public string IssueNotes { get; set; }
    }

    public class VerifyTCCFileUploadViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Upload File")]
        public HttpPostedFileBase DocumentFile { get; set; }

        [Display(Name = "Upload File")]
        public string DocumentFilePath { get; set; }

        public string DocumentFileName { get; set; }
    }

    public class VerifyTCCReferenceNumberViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter Reference Number")]
        [Display(Name = "Reference Number")]
        public string ReferenceNumber { get; set; }
    }

    public class VerifyTreasuryReceiptViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter Bill Number")]
        [Display(Name = "Bill Number")]
        public string BillNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter Receipt Number")]
        [Display(Name = "Receipt Number")]
        public string ReceiptNumber { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Please Enter Reference Number")]
        //[Display(Name = "Reference Number")]
        //public string ReferenceNumber { get; set; }


    }
}