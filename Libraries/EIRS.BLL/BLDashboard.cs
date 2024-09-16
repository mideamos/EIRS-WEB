using EIRS.BOL;
using EIRS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EIRS.BLL
{
    public class BLDashboard
    {
        IDashboardRepository _DashboardRepository;

        public BLDashboard()
        {
            _DashboardRepository = new DashboardRepository();
        }

        public IList<usp_GetBillChart_Result> BL_GetBillChart(int BillTypeID, int StatusID, int FilterTypeID)
        {
            return _DashboardRepository.REP_GetBillChart(BillTypeID, StatusID, FilterTypeID);
        }

        public IList<usp_GetTaxPayerBillChart_Result> BL_GetTaxPayerBillChart(int BillTypeID, int TaxPayerTypeID, int FilterTypeID)
        {
            return _DashboardRepository.REP_GetTaxPayerBillChart(BillTypeID, TaxPayerTypeID, FilterTypeID);
        }

        public IList<usp_GetTaxPayerSettlementChart_Result> BL_GetTaxPayerSettlementChart(int BillTypeID, int TaxPayerTypeID, int FilterTypeID)
        {
            return _DashboardRepository.REP_GetTaxPayerSettlementChart(BillTypeID, TaxPayerTypeID, FilterTypeID);
        }

        public IList<usp_GetBillAgingChart_Result> BL_GetBillAgingChart(int BillTypeID, int TaxPayerTypeID, int FilterTypeID)
        {
            return _DashboardRepository.REP_GetBillAgingChart(BillTypeID, TaxPayerTypeID, FilterTypeID);
        }

        public IList<usp_GetBuildingChart_Result> BL_GetBuildingChart(int FilterTypeID)
        {
            return _DashboardRepository.REP_GetBuildingChart(FilterTypeID);
        }

        public IList<usp_GetBusinessChart_Result> BL_GetBusinessChart(int FilterTypeID)
        {
            return _DashboardRepository.REP_GetBusinessChart(FilterTypeID);
        }

        public IList<usp_GetLandChart_Result> BL_GetLandChart(int FilterTypeID)
        {
            return _DashboardRepository.REP_GetLandChart(FilterTypeID);
        }

        public IList<usp_GetVehicleChart_Result> BL_GetVehicleChart(int FilterTypeID)
        {
            return _DashboardRepository.REP_GetVehicleChart(FilterTypeID);
        }
    }
}
