using System;
using System.ComponentModel.DataAnnotations;

namespace EIRS.Models
{
    public class ServiceBillViewModel
    {
        public long ServiceBillID { get; set; }

        public int TaxPayerTypeID { get; set; }

        public int TaxPayerID { get; set; }

        public string TaxPayerRIN { get; set; }

        public string TaxPayerTIN { get; set; }

        public string MobileNumber1 { get; set; }

        public string TaxPayerName { get; set; }

        public string TaxPayerAddress { get; set; }

        [Display(Name = "Service Bill Notes")]
        public string Notes { get; set; }

        [Display(Name = "Settlement Due Date")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Settlement Due Date")]
        public DateTime SettlementDuedate { get; set; }

        public DateTime ServiceBillDate { get; set; }

    }
}
