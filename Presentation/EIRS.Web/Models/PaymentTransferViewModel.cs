using System.ComponentModel.DataAnnotations;

namespace EIRS.Web.Models
{
    public class PaymentTransferViewModel
    {
        public int POAID { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Enter From Tax Payer Type")]
        //[Display(Name = "From Tax Payer Type")]
        public int FromTaxPayerTypeID { get; set; }
        public string FromTaxPayerTypeName { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Enter From Tax Payer")]
        //[Display(Name = "From Tax Payer")]
        public int FromTaxPayerID { get; set; }
        public string FromTaxPayerName { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Enter To Tax Payer Type")]
        //[Display(Name = "To Tax Payer Type")]
        public int ToTaxPayerTypeID { get; set; }
        public string ToTaxPayerTypeName { get; set; }
        public string TransactionRefNumber { get; set; }
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Enter To Tax Payer")]
        //[Display(Name = "To Tax Payer")]
        public int ToTaxPayerID { get; set; }
        //[Display(Name = "POA Account Id")]
        public int POAAccountId { get; set; }
        public string ToTaxPayerName { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Enter Amount")]
        //[Display(Name = "Transfer Amount")]
        public decimal? Amount { get; set; }

    }
}