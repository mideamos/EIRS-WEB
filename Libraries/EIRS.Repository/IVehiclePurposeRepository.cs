using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IVehiclePurposeRepository
    {
		FuncResponse REP_InsertUpdateVehiclePurpose(Vehicle_Purpose pObjVehiclePurpose);
		IList<usp_GetVehiclePurposeList_Result> REP_GetVehiclePurposeList(Vehicle_Purpose pObjVehiclePurpose);
        usp_GetVehiclePurposeList_Result REP_GetVehiclePurposeDetails(Vehicle_Purpose pObjVehiclePurpose);
        IList<DropDownListResult> REP_GetVehiclePurposeDropDownList(Vehicle_Purpose pObjVehiclePurpose);
        FuncResponse REP_UpdateStatus(Vehicle_Purpose pObjVehiclePurpose);
    }
}