using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EIRS.Web.GISModels
{
    public class RevenueStreamResult
    {
        public int TaxOfficeID { get; set; }
        public string TaxOfficeName { get; set; }
        public string TaxMonth { get; set; }
        public decimal TargetAmount { get; set; }
        public decimal AssessedAmount { get; set; }
        public decimal RevenueAmount { get; set; }
        public decimal Differential { get; set; }
        public decimal Performance { get; set; }
    }
    public class usp_RPT_All_TaxOffices_Performance_ByMonth_Result
    {
        public int TaxOfficeID { get; set; }
        public string TaxOfficeName { get; set; }
        public decimal? Targetamount { get; set; }
        public decimal? Settlementamount { get; set; }
        public decimal? differenitial { get; set; }
        public decimal? Perc { get; set; }
    }

    public class RevenueStreamResultDrillDown
    {
        public int? TaxOfficeID { get; set; } // Nullable int
        public string TaxOfficeName { get; set; } // Nullable string
        public int? RevenueStreamID { get; set; } // Nullable int
        public string RevenueStreamName { get; set; } // Nullable string
        public int? TaxpayerID { get; set; } // Nullable int
        public int? TaxpayerTypeID { get; set; } // Nullable int
        public long? Assessmentid { get; set; } // Nullable long
        public long? AAIID { get; set; } // Nullable long
        public long? servicebillid { get; set; } // Nullable long
        public long? SBSIID { get; set; } // Nullable long
        public decimal? settlementamount { get; set; } // Nullable decimal
        public string AssessmentRefNo { get; set; } // Nullable string
        public string TaxpayerName { get; set; } // Nullable string

    }
    public class usp_RPT_All_TaxOffices_Performance_byRevenueStreamdrilldown
    {
        [Key]
        public int TaxOfficeID { get; set; }
        public string TaxOfficeName { get; set; }
        public int RevenueStreamID { get; set; }
        public string RevenueStreamName { get; set; }
        public int TaxpayerID { get; set; }
        public int TaxpayerTypeID { get; set; }
        public long Assessmentid { get; set; }
        public long AAIID { get; set; }
        public long servicebillid { get; set; }
        public long SBSIID { get; set; }
        public decimal settlementamount { get; set; }
        public string AssessmentRefNo { get; set; }
        public string TaxpayerName { get; set; }
        public string TaxpayerRIN { get; set; }

    }
    public class usp_RPT_TaxOffice_Performance_ByAllRevenueStreamdrilldown
    {
        [Key]
        public int TaxOfficeID { get; set; }
        public string TaxOfficeName { get; set; }
        public int RevenueStreamID { get; set; }
        public string RevenueStreamName { get; set; }
        public int TaxpayerID { get; set; }
        public int TaxpayerTypeID { get; set; }
        public long Assessmentid { get; set; }
        public long AAIID { get; set; }
        public long servicebillid { get; set; }
        public long SBSIID { get; set; }
        public decimal settlementamount { get; set; }
        public string AssessmentRefNo { get; set; }
        public string TaxpayerName { get; set; }
        public string TaxpayerRIN { get; set; }

    }
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
        public int TaxOffManagerID  { get; set; }
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