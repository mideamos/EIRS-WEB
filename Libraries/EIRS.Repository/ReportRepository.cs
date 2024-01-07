using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EIRS.Repository
{
    public class ReportRepository : IReportRepository
    {
        EIRSEntities _db;

        public IList<usp_RPT_GetRevenueStreamReport_Result> REP_GetRevenueStreamReport(int pIntTaxYear, int pIntTaxMonth)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_GetRevenueStreamReport(pIntTaxYear, pIntTaxMonth).ToList();
            }
        }

        public IList<usp_RPT_GetAssetTypeReport_Result> REP_GetAssetTypeReport(int pIntTaxYear, int pIntTaxMonth)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_GetAssetTypeReport(pIntTaxYear, pIntTaxMonth).ToList();
            }
        }

        public IList<usp_RPT_GetDirectorateReport_Result> REP_GetDirectorateReport(int pIntTaxYear, int pIntTaxMonth)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_GetDirectorateReport(pIntTaxYear, pIntTaxMonth).ToList();
            }
        }

        public IList<usp_RPT_GetTaxPayerReport_Result> REP_GetTaxPayerReport(int pIntTaxYear, int pIntTaxMonth, int pIntTaxPayerTypeID, int pIntTaxPayerID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_GetTaxPayerReport(pIntTaxYear, pIntTaxMonth, pIntTaxPayerTypeID, pIntTaxPayerID).ToList();
            }
        }

        public IList<usp_RPT_GetDashboardReport_Result> REP_GetDashboardReport()
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_RPT_GetDashboardReport().ToList();
            }
        }

        public IList<usp_GetDailySummaryReport_Result> REP_GetDailySummaryReport(DateTime pReportDate, int pIntReferenceID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetDailySummaryReport(pReportDate, pIntReferenceID).ToList();
            }
        }

        public IList<usp_GetMonthlySummaryReport_Result> REP_GetMonthlySummaryReport(DateTime pReportMonth, int pIntReferenceID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetMonthlySummaryReport(pReportMonth, pIntReferenceID).ToList();
            }
        }

        public usp_GetTaxPayerLiabilityByTaxYear_Result REP_GetTaxPayerLiabilityByTaxYear(int TaxPayerTypeID, int TaxPayerID , int TaxYear)
        {
            using(_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerLiabilityByTaxYear(TaxPayerTypeID, TaxPayerID, TaxYear).SingleOrDefault();
            }
        }
    }
}
