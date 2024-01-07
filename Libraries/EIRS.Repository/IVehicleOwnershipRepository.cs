using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IVehicleOwnershipRepository
    {
		FuncResponse REP_InsertUpdateVehicleOwnership(Vehicle_Ownership pObjVehicleOwnership);
		IList<usp_GetVehicleOwnershipList_Result> REP_GetVehicleOwnershipList(Vehicle_Ownership pObjVehicleOwnership);
        usp_GetVehicleOwnershipList_Result REP_GetVehicleOwnershipDetails(Vehicle_Ownership pObjVehicleOwnership);
        IList<DropDownListResult> REP_GetVehicleOwnershipDropDownList(Vehicle_Ownership pObjVehicleOwnership);
        FuncResponse REP_UpdateStatus(Vehicle_Ownership pObjVehicleOwnership);
    }
}