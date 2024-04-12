using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.Web.GISModels
{
    public class usp_GetAssessmentForPendingOrDeclined_Result
    {
        public long AssessmentID { get; set; }
        public long ID { get; set; }
        public decimal? Amount { get; set; }
        public int SettlementStatusID { get; set; }
        public string Status { get; set; }
        public string  TaxPayerName  { get; set; }
        public string TaxOfficerName { get; set; }
        public string TaxOfficeName { get; set; }
        public string TaxPayerRIN { get; set; }
        public string AssessmentRefNo { get; set; }
    }
}