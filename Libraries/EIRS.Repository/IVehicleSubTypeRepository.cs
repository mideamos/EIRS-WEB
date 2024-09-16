using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IVehicleSubTypeRepository
    {
        usp_GetVehicleSubTypeList_Result REP_GetVehicleSubTypeDetails(Vehicle_SubTypes pObjVehicleSubType);
        IList<DropDownListResult> REP_GetVehicleSubTypeDropDownList(Vehicle_SubTypes pObjVehicleSubType);
        IList<usp_GetVehicleSubTypeList_Result> REP_GetVehicleSubTypeList(Vehicle_SubTypes pObjVehicleSubType);
        FuncResponse REP_InsertUpdateVehicleSubType(Vehicle_SubTypes pObjVehicleSubType);
        FuncResponse REP_UpdateStatus(Vehicle_SubTypes pObjVehicleSubType);
    }
}