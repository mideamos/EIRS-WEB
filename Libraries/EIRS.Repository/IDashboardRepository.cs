using System.Collections.Generic;
using EIRS.BOL;

namespace EIRS.Repository
{
    public interface IDashboardRepository
    {
        IList<usp_GetBillChart_Result> REP_GetBillChart(int BillTypeID, int StatusID, int FilterTypeID);
        IList<usp_GetTaxPayerBillChart_Result> REP_GetTaxPayerBillChart(int BillTypeID, int TaxPayerTypeID, int FilterTypeID);
        IList<usp_GetTaxPayerSettlementChart_Result> REP_GetTaxPayerSettlementChart(int BillTypeID, int TaxPayerTypeID, int FilterTypeID);
        IList<usp_GetBillAgingChart_Result> REP_GetBillAgingChart(int BillTypeID, int TaxPayerTypeID, int FilterTypeID);
        IList<usp_GetBuildingChart_Result> REP_GetBuildingChart(int FilterTypeID);
        IList<usp_GetBusinessChart_Result> REP_GetBusinessChart(int FilterTypeID);
        IList<usp_GetLandChart_Result> REP_GetLandChart(int FilterTypeID);
        IList<usp_GetVehicleChart_Result> REP_GetVehicleChart(int FilterTypeID);
    }
}