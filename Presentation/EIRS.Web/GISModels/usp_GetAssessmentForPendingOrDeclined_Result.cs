using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EIRS.Web.GISModels
{
    public class usp_GetTccDownloadByYearResult
    {
        public Nullable<long> TccId { get; set; }
        public string FullName { get; set; }
        public string IndividualRIn { get; set; }
        public string TccRefNo { get; set; }
        public string DownloadStatus { get; set; }
        public Nullable<System.DateTime> RequestDate { get; set; }
        public Nullable<bool> IsDownloaded { get; set; }
    }
    public class usp_GetAssessmentForPendingOrDeclined_Result
    {
        //public int SN { get; set; }
        public long AssessmentID { get; set; }
        public int ID { get; set; }
        public int UserID { get; set; }
        public decimal Amount { get; set; }
        public int SettlementStatusID { get; set; }
        public string Status { get; set; }
        public string TaxPayerName { get; set; }
        public string TaxOfficerName { get; set; }
        public string TaxOfficeName { get; set; }
        public string TaxPayerRIN { get; set; }
        public string AssessmentRefNo { get; set; }
    }
}