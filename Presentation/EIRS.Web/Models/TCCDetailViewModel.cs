using System.ComponentModel.DataAnnotations;

namespace EIRS.Web.Models
{
    public class TCCDetailViewModel
    {
        public long TCCDetailID { get; set; }

        [Required(ErrorMessage = "Select Tax Payer Type")]
        [Display(Name = "Tax Payer Type")]
        public int TaxPayerTypeID { get; set; }

        [Required(ErrorMessage = "Select Tax Payer")]
        [Display(Name = "Tax Payer")]
        public int TaxPayerID { get; set; }
        public string TaxPayerName { get; set; }

        [Display(Name = "Tax Year")]
        [Required(ErrorMessage = "Select Tax Year")]
        public int TaxYear { get; set; }

        [Display(Name = "Assessable Income")]
        [Required(ErrorMessage = "Enter Assessable Income")]
        public decimal AssessableIncome { get; set; }

        [Display(Name = "TCC Tax Paid")]
        [Required(ErrorMessage = "Enter TCC Tax Paid")]
        public decimal TCCTaxPaid { get; set; }

        [Display(Name = "ERAS Tax Paid")]
        public decimal? ERASTaxPaid { get; set; }

        [Display(Name = "ERAS Assessed")]
        public decimal? ERASAssessed { get; set; }
    }

    public class TaxClearanceCertificateViewModel
    {

        [Required(ErrorMessage = "Select Tax Payer Type")]
        [Display(Name = "Tax Payer Type")]
        public int TaxPayerTypeID { get; set; }

        [Required(ErrorMessage = "Select Tax Payer")]
        [Display(Name = "Tax Payer")]
        public int TaxPayerID { get; set; }
        public string TaxPayerName { get; set; }

        [Display(Name = "Tax Year")]
        [Required(ErrorMessage = "Select Tax Year")]
        public int TaxYear { get; set; }

        [Display(Name = "Request Ref No")]
        [Required(ErrorMessage = "Enter Request Ref No")]
        public string RequestRefNo { get; set; }

        [Display(Name = "Serial Number")]
        [Required(ErrorMessage = "Enter Serial Number")]
        public string SerialNumber { get; set; }

        [Display(Name = "Source of Income")]
        [Required(ErrorMessage = "Enter Source of Income")]
        public string SourceOfIncome { get; set; }

        [Display(Name = "Tax Payer Details")]
        [Required(ErrorMessage = "Enter Tax Payer Details")]
        public string TaxPayerDetails { get; set; }

    }
}