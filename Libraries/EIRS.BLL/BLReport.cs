using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.BLL
{
    public class BLReport
    {
        IReportRepository _ReportRepository;

        public BLReport()
        {
            _ReportRepository = new ReportRepository();
        }

        public IList<usp_RPT_GetRevenueStreamReport_Result> BL_GetRevenueStreamReport(int pIntTaxYear, int pIntTaxMonth)
        {
            return _ReportRepository.REP_GetRevenueStreamReport(pIntTaxYear, pIntTaxMonth);
        }

        public IList<usp_RPT_GetAssetTypeReport_Result> BL_GetAssetTypeReport(int pIntTaxYear, int pIntTaxMonth)
        {
            return _ReportRepository.REP_GetAssetTypeReport(pIntTaxMonth, pIntTaxMonth);
        }

        public IList<usp_RPT_GetDirectorateReport_Result> BL_GetDirectorateReport(int pIntTaxYear, int pIntTaxMonth)
        {
            return _ReportRepository.REP_GetDirectorateReport(pIntTaxYear, pIntTaxMonth);
        }

        public IList<usp_RPT_GetTaxPayerReport_Result> BL_GetTaxPayerReport(int pIntTaxYear, int pIntTaxMonth, int pIntTaxPayerTypeID, int pIntTaxPayerID)
        {
            return _ReportRepository.REP_GetTaxPayerReport(pIntTaxYear, pIntTaxMonth, pIntTaxPayerTypeID, pIntTaxPayerID);
        }

        public IList<usp_RPT_GetDashboardReport_Result> BL_GetDashboardReport()
        {
            return _ReportRepository.REP_GetDashboardReport();
        }

        public IList<usp_GetDailySummaryReport_Result> BL_GetDailySummaryReport(DateTime pReportDate, int pIntReferenceID)
        {
            return _ReportRepository.REP_GetDailySummaryReport(pReportDate,pIntReferenceID);
        }

        public IList<usp_GetMonthlySummaryReport_Result> BL_GetMonthlySummaryReport(DateTime pReportMonth, int pIntReferenceID)
        {
            return _ReportRepository.REP_GetMonthlySummaryReport(pReportMonth, pIntReferenceID);
        }

        public usp_GetTaxPayerLiabilityByTaxYear_Result BL_GetTaxPayerLiabilityByTaxYear(int TaxPayerTypeID, int TaxPayerID, int TaxYear)
        {
            return _ReportRepository.REP_GetTaxPayerLiabilityByTaxYear(TaxPayerTypeID, TaxPayerID, TaxYear);
        }
    }
}
