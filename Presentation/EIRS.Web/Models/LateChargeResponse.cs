using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.Web.Models
{
    public class LateChargeResponse
    {
        public int Id { get; set; }
        public long AAIID { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public decimal LateChargeAmount { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsLateChargeApplicable { get; set; }
        public decimal LC_Penatly { get; set; }
        public decimal LC_Interest { get; set; }
        public decimal SettlementAmount { get; set; }
        public decimal PendingAmount { get; set; }
        public string TaxPayerRIN { get; set; }
        public string TaxPayerName { get; set; }
        public string TaxPayerId { get; set; }
        public int AssessmentItemID { get; set; }
        public string AssessmentRefNo { get; set; }
        public DateTime ChargeDate { get; set; } = DateTime.Now;    
        public String CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;   
    }
}