using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLVehicleInsurance
    {
        IVehicleInsuranceRepository _VehicleInsuranceRepository;

        public BLVehicleInsurance()
        {
            _VehicleInsuranceRepository = new VehicleInsuranceRepository();
        }

        public IList<usp_GetVehicleInsuranceList_Result> BL_GetVehicleInsuranceList(Vehicle_Insurance pObjVehicleInsurance)
        {
            return _VehicleInsuranceRepository.REP_GetVehicleInsuranceList(pObjVehicleInsurance);
        }

        public FuncResponse BL_InsertUpdateVehicleInsurance(Vehicle_Insurance pObjVehicleInsurance)
        {
            return _VehicleInsuranceRepository.REP_InsertUpdateVehicleInsurance(pObjVehicleInsurance);
        }

        public usp_GetVehicleInsuranceList_Result BL_GetVehicleInsuranceDetails(Vehicle_Insurance pObjVehicleInsurance)
        {
            return _VehicleInsuranceRepository.REP_GetVehicleInsuranceDetails(pObjVehicleInsurance);
        }

        public IList<DropDownListResult> BL_GetVehicleInsuranceDropDownList(Vehicle_Insurance pObjVehicleInsurance)
        {
            return _VehicleInsuranceRepository.REP_GetVehicleInsuranceDropDownList(pObjVehicleInsurance);
        }

        public FuncResponse BL_UpdateStatus(Vehicle_Insurance pObjVehicleInsurance)
        {
            return _VehicleInsuranceRepository.REP_UpdateStatus(pObjVehicleInsurance);
        }
    }
}
