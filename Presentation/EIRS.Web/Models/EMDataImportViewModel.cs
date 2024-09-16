using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace EIRS.Web.Models
{
    public class EMDataImportViewModel
    {
        [Display(Name = "Tax Month")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Month")]
        public int TaxMonth { get; set; }

        [Display(Name = "Tax Year")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Year")]
        public int TaxYear { get; set; }

        [Display(Name = "Upload File")]
        [File(AllowedFileExtensions = new string[] { ".xlsx", ".xls", }, MaxContentLength = 1024 * 1024 * 1000, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase ExcelFile { get; set; }
    }

    public class EMPDViewModel
    {
        [Display(Name = "Tax Month")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Month")]
        public int TaxMonth { get; set; }

        [Display(Name = "Tax Year")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Year")]
        public int TaxYear { get; set; }

        [Display(Name = "Payment Ref. Number")]
        [Required(AllowEmptyStrings = false)]
        public string PaymentRefNumber { get; set; }

        [Display(Name = "Payment Date Time ")]
        [Required(AllowEmptyStrings = false)]
        public string PaymentDateTime { get; set; }

        [Display(Name = "Assessment Reference/RIN")]
        [Required(AllowEmptyStrings = false)]
        public string AssessmentReference { get; set; }

        [Display(Name = "Receipt No ")]
        [Required(AllowEmptyStrings = false)]
        public string ReceiptNo { get; set; }

        [Display(Name = "RIN")]
        [Required(AllowEmptyStrings = false)]
        public string RIN { get; set; }

        [Display(Name = "Customer Name")]
        [Required(AllowEmptyStrings = false)]
        public string CustomerName { get; set; }

        [Display(Name = "Revenue item")]
        [Required(AllowEmptyStrings = false)]
        public string RevenueItem { get; set; }

        [Display(Name = "Amount")]
        [Required(AllowEmptyStrings = false)]
        public decimal Amount { get; set; }

        [Display(Name = "Payment Method ")]
        [Required(AllowEmptyStrings = false)]
        public string PaymentMethod { get; set; }

        [Display(Name = "Deposit Slip")]
        [Required(AllowEmptyStrings = false)]
        public string DepositSlip { get; set; }

        [Display(Name = "Cheque Value Date ")]
        [Required(AllowEmptyStrings = false)]
        public string ChequeValueDate { get; set; }

        [Display(Name = "Bank")]
        [Required(AllowEmptyStrings = false)]
        public string Bank { get; set; }

        [Display(Name = "Additional Info")]
        [Required(AllowEmptyStrings = false)]
        public string AdditionalInfo { get; set; }

        [Display(Name = "Bank Branch")]
        [Required(AllowEmptyStrings = false)]
        public string BankBranch { get; set; }

        [Display(Name = "Tax Payer Type")]
        [Required(AllowEmptyStrings = false)]
        public string TaxPayerType { get; set; }

        [Display(Name = "Payment Code")]
        [Required(AllowEmptyStrings = false)]
        public string PaymentCode { get; set; }

        [Display(Name = "Retrieval Ref. Number")]
        [Required(AllowEmptyStrings = false)]
        public string RetrievalRefNumber { get; set; }

        [Display(Name = "Auth-ID")]
        [Required(AllowEmptyStrings = false)]
        public string AuthID { get; set; }

    }

    public class EMBSViewModel
    {
        [Display(Name = "Tax Month")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Month")]
        public int TaxMonth { get; set; }

        [Display(Name = "Tax Year")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Year")]
        public int TaxYear { get; set; }

        [Display(Name = "Payment Ref. Number")]
        [Required(AllowEmptyStrings = false)]
        public string PaymentRefNumber { get; set; }

        [Display(Name = "Payment Date Time ")]
        [Required(AllowEmptyStrings = false)]
        public string PaymentDateTime { get; set; }

        [Display(Name = "Customer Name")]
        [Required(AllowEmptyStrings = false)]
        public string CustomerName { get; set; }


        [Display(Name = "Category")]
        [Required(AllowEmptyStrings = false)]
        public string Category { get; set; }

        [Display(Name = "Revenue Head")]
        [Required(AllowEmptyStrings = false)]
        public string RevenueHead { get; set; }

        [Display(Name = "Amount")]
        [Required(AllowEmptyStrings = false)]
        public decimal Amount { get; set; }

        [Display(Name = "Bank")]
        [Required(AllowEmptyStrings = false)]
        public string Bank { get; set; }

    }

}