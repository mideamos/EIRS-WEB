using System;
using System.Collections.Generic;
using EIRS.BOL;

namespace EIRS.Repository
{
    public interface IReportRepository
    {
        IList<usp_RPT_GetRevenueStreamReport_Result> REP_GetRevenueStreamReport(int pIntTaxYear, int pIntTaxMonth);

        IList<usp_RPT_GetAssetTypeReport_Result> REP_GetAssetTypeReport(int pIntTaxYear, int pIntTaxMonth);

        IList<usp_RPT_GetDirectorateReport_Result> REP_GetDirectorateReport(int pIntTaxYear, int pIntTaxMonth);

        IList<usp_RPT_GetDashboardReport_Result> REP_GetDashboardReport();

        IList<usp_RPT_GetTaxPayerReport_Result> REP_GetTaxPayerReport(int pIntTaxYear, int pIntTaxMonth, int pIntTaxPayerTypeID, int pIntTaxPayerID);

        IList<usp_GetDailySummaryReport_Result> REP_GetDailySummaryReport(DateTime pReportDate, int pIntReferenceID);

        IList<usp_GetMonthlySummaryReport_Result> REP_GetMonthlySummaryReport(DateTime pReportMonth, int pIntReferenceID);

        usp_GetTaxPayerLiabilityByTaxYear_Result REP_GetTaxPayerLiabilityByTaxYear(int TaxPayerTypeID, int TaxPayerID, int TaxYear);
    }
}