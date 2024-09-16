using System;
using System.ComponentModel.DataAnnotations;

namespace EIRS.API.Models
{
    public class PaymentAccountModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tax Payer Type is Required")]
        public int TaxPayerTypeID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Tax Payer is Required")]
        public int TaxPayerID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Amount is Required")]
        public decimal Amount { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Payment Method is Required")]
        public int PaymentMethodID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Revenue Stream is Required")]
        public int RevenueStreamID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Revenue Sub Stream is Required")]
        public int RevenueSubStreamID { get; set; }

        public int? AgencyID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Payment Date is Required")]
        public DateTime PaymentDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Transaction Ref No is Required")]
        public string TransactionRefNo { get; set; }

        public string Notes { get; set; }
    }

    public class PaymentAccountRINModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Tax Payer Type is Required")]
        public int TaxPayerTypeID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Tax Payer RIN is Required")]
        public string TaxPayerRIN { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Amount is Required")]
        public decimal Amount { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Payment Method is Required")]
        public int PaymentMethodID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Payment Date is Required")]
        public DateTime PaymentDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Transaction Ref No is Required")]
        public string TransactionRefNo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Revenue Stream is Required")]
        public int RevenueStreamID { get; set; }

        public int? AgencyID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Revenue Sub Stream is Required")]
        public int RevenueSubStreamID { get; set; }

        public string Notes { get; set; }
    }
}