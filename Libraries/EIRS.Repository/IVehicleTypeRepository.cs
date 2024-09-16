using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IVehicleTypeRepository
    {
		FuncResponse REP_InsertUpdateVehicleType(Vehicle_Types pObjVehicleType);
		IList<usp_GetVehicleTypeList_Result> REP_GetVehicleTypeList(Vehicle_Types pObjVehicleType);
        usp_GetVehicleTypeList_Result REP_GetVehicleTypeDetails(Vehicle_Types pObjVehicleType);
        IList<DropDownListResult> REP_GetVehicleTypeDropDownList(Vehicle_Types pObjVehicleType);
        FuncResponse REP_UpdateStatus(Vehicle_Types pObjVehicleType);
    }
}