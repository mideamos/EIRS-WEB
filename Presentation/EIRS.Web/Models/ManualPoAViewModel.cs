using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRS.Web.Models
{
    public class ManualPoAViewModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Payer Type")]
        [Display(Name = "Tax Payer Type")]
        public int TaxPayerTypeID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Tax Payer")]
        [Display(Name = "Tax Payer")]
        public int TaxPayerID { get; set; }
        public string TaxPayerName { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Amount")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Revenue Stream")]
        [Display(Name = "Revenue Stream")]
        public int RevenueStreamID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Revenue Sub Stream")]
        [Display(Name = "Revenue Sub Stream")]
        public int RevenueSubStreamID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Agency")]
        [Display(Name = "Agency")]
        public int AgencyID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Transaction Ref No")]
        [Display(Name = "Transaction Ref No")]
        public string TransactionRefNo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Settlement Method")]
        [Display(Name = "Settlement Method")]
        public int SettlementMethodID { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Payment Date")]
        [Display(Name = "Payment Date")]
        public DateTime PaymentDate { get; set; }

        [Display(Name = "Notes")]
        public string Notes { get; set; }
    }
}