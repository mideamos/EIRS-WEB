using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IVehicleFunctionRepository
    {
        usp_GetVehicleFunctionList_Result REP_GetVehicleFunctionDetails(Vehicle_Function pObjVehicleFunction);
        IList<DropDownListResult> REP_GetVehicleFunctionDropDownList(Vehicle_Function pObjVehicleFunction);
        IList<usp_GetVehicleFunctionList_Result> REP_GetVehicleFunctionList(Vehicle_Function pObjVehicleFunction);
        FuncResponse REP_InsertUpdateVehicleFunction(Vehicle_Function pObjVehicleFunction);
        FuncResponse REP_UpdateStatus(Vehicle_Function pObjVehicleFunction);
    }
}