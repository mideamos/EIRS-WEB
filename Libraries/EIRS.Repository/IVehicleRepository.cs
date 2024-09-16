using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IVehicleRepository
    {
        usp_GetVehicleList_Result REP_GetVehicleDetails(Vehicle pObjVehicle);
        IList<usp_GetVehicleList_Result> REP_GetVehicleList(Vehicle pObjVehicle);
        FuncResponse<Vehicle> REP_InsertUpdateVehicle(Vehicle pObjVehicle);
        FuncResponse REP_UpdateStatus(Vehicle pObjVehicle);
        IList<vw_Vehicle> REP_GetVehicleList();
        IList<DropDownListResult> REP_GetVehicleDropDownList(Vehicle pObjVehicle);
        IList<usp_SearchVehicleByRegNumber_Result> REP_SearchVehicleByRegNumber(string pStrRegNumber);
        IList<usp_GetVehicleTaxPayerList_Result> REP_GetVehicleTaxPayerList(int pIntVehicleID);
        //IList<usp_GetVehicleChart_Result> REP_GetVehicleChart(int pIntChartType);

        IList<usp_SearchVehicleForRDMLoad_Result> REP_SearchVehicleDetails(Vehicle pObjVehicle);
        IDictionary<string, object> REP_SearchVehicle(Vehicle pObjVehicle);

        IDictionary<string, object> REP_SearchVehicleForSideMenu(Vehicle pObjVehicle);
    }
}