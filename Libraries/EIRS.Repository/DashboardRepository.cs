using EIRS.BOL;
using System.Collections.Generic;
using System.Linq;
namespace EIRS.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        EIRSEntities _db;

        public IList<usp_GetBillChart_Result> REP_GetBillChart(int BillTypeID, int StatusID, int FilterTypeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBillChart(BillTypeID, StatusID, FilterTypeID).ToList();
            }
        }

        public IList<usp_GetTaxPayerBillChart_Result> REP_GetTaxPayerBillChart(int BillTypeID, int TaxPayerTypeID, int FilterTypeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerBillChart(BillTypeID, TaxPayerTypeID, FilterTypeID).ToList();
            }
        }

        public IList<usp_GetTaxPayerSettlementChart_Result> REP_GetTaxPayerSettlementChart(int BillTypeID, int TaxPayerTypeID, int FilterTypeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerSettlementChart(BillTypeID, TaxPayerTypeID, FilterTypeID).ToList();
            }
        }

        public IList<usp_GetBillAgingChart_Result> REP_GetBillAgingChart(int BillTypeID, int TaxPayerTypeID, int FilterTypeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBillAgingChart(BillTypeID, TaxPayerTypeID, FilterTypeID).ToList();
            }
        }

        public IList<usp_GetBusinessChart_Result> REP_GetBusinessChart(int FilterTypeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBusinessChart(FilterTypeID).ToList();
            }
        }

        public IList<usp_GetLandChart_Result> REP_GetLandChart(int FilterTypeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetLandChart(FilterTypeID).ToList();
            }
        }

        public IList<usp_GetBuildingChart_Result> REP_GetBuildingChart(int FilterTypeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBuildingChart(FilterTypeID).ToList();
            }
        }

        public IList<usp_GetVehicleChart_Result> REP_GetVehicleChart(int FilterTypeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehicleChart(FilterTypeID).ToList();
            }
        }
    }
}
