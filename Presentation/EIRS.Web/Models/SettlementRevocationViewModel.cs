using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.Web.Models
{
    public class SettlementRevocationViewModel
    {
        public int ServiceBillID { get; set; }
        public int AssessmentID { get; set; }
        public int SettlementID { get; set; }
        public long SIID { get; set; }
        public int RevenueStreamID { get; set; }
        public int RevenueSubStreamID { get; set; }
        public int AgencyID { get; set; }
        public decimal ReversalAmount { get; set; }
        public string PoANotes { get; set; }
    }
}