using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.Web.Models
{
    public class AdjustmentWithPoAModel
    {
        public int AssessmentID { get; set; }
        public int ServiceBillID { get; set; }
        public long SBSIID { get; set; }
        public long AAIID { get; set; }
        public int AdjustmentTypeID { get; set; }
        public string AdjustmentLine { get; set; }
        public decimal AdjustmentAmount { get; set; }
        public string TransactionRefNo { get; set; }
        public int RevenueStreamID { get; set; }
        public int RevenueSubStreamID { get; set; }
        public int AgencyID { get; set; }
        public decimal PoAAmount { get; set; }
        public int PaymentMethodID { get; set; }
        public string Notes { get; set; }        
    }
}