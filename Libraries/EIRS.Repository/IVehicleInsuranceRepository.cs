using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IVehicleInsuranceRepository
    {
        usp_GetVehicleInsuranceList_Result REP_GetVehicleInsuranceDetails(Vehicle_Insurance pObjVehicleInsurance);
        IList<DropDownListResult> REP_GetVehicleInsuranceDropDownList(Vehicle_Insurance pObjVehicleInsurance);
        IList<usp_GetVehicleInsuranceList_Result> REP_GetVehicleInsuranceList(Vehicle_Insurance pObjVehicleInsurance);
        FuncResponse REP_InsertUpdateVehicleInsurance(Vehicle_Insurance pObjVehicleInsurance);
        FuncResponse REP_UpdateStatus(Vehicle_Insurance pObjVehicleInsurance);
    }
}