using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Models
{
    public partial class SettlementViewModel
    {
        public long AssessmentID { get; set; }

        public long ServiceBillID { get; set; }

        public int TaxPayerID { get; set; }

        public string TaxPayerRIN { get; set; }

        public string TaxPayerName { get; set; }

        public int? TaxPayerTypeID { get; set; }
        public string TaxPayerTypeName { get; set; }

        [Display(Name = "Settlement Method")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Select Settlement Method")]
        public int SettlementMethod { get; set; }

        [Display(Name = "Transaction Ref No")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Transaction Ref No")]
        public string TransactionRefNo { get; set; }

        [Display(Name = "Settlement Date")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Enter Settlement Date")]
        public DateTime SettlementDate { get; set; }

        [Display(Name = "Settlement Notes")]
        public string Notes { get; set; }

        public DateTime? BillDate { get; set; }

        public DateTime? DueDate { get; set; }

        public string BillRefNo { get; set; }

        public string StatusName { get; set; }

        public string BillNotes { get; set; }

        public decimal? BillAmount { get; set; }

        public decimal? TotalPaid { get; set; }

    }

    public partial class PoASettlementViewModel
    {
        public long AssessmentID { get; set; }

        public long ServiceBillID { get; set; }

        public int TaxPayerID { get; set; }

        public string TaxPayerRIN { get; set; }

        public string TaxPayerName { get; set; }

        public int? TaxPayerTypeID { get; set; }
        public string TaxPayerTypeName { get; set; }

        [Display(Name = "Settlement Notes")]
        public string Notes { get; set; }

        public DateTime? BillDate { get; set; }

        public DateTime? DueDate { get; set; }

        public string BillRefNo { get; set; }

        public string StatusName { get; set; }

        public string BillNotes { get; set; }

        public decimal? BillAmount { get; set; }

        public decimal? TotalPaid { get; set; }

    }
}
