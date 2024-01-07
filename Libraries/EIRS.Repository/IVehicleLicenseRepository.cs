using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IVehicleLicenseRepository
    {
        usp_GetVehicleLicenseList_Result REP_GetVehicleLicenseDetails(Vehicle_Licenses pObjVehicleLicense);
        IList<DropDownListResult> REP_GetVehicleLicenseDropDownList(Vehicle_Licenses pObjVehicleLicense);
        IList<usp_GetVehicleLicenseList_Result> REP_GetVehicleLicenseList(Vehicle_Licenses pObjVehicleLicense);
        FuncResponse REP_InsertUpdateVehicleLicense(Vehicle_Licenses pObjVehicleLicense);
        FuncResponse REP_UpdateStatus(Vehicle_Licenses pObjVehicleLicense);
    }
}