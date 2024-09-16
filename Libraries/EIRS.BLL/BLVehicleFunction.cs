using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLVehicleFunction
    {
        IVehicleFunctionRepository _VehicleFunctionRepository;

        public BLVehicleFunction()
        {
            _VehicleFunctionRepository = new VehicleFunctionRepository();
        }

        public IList<usp_GetVehicleFunctionList_Result> BL_GetVehicleFunctionList(Vehicle_Function pObjVehicleFunction)
        {
            return _VehicleFunctionRepository.REP_GetVehicleFunctionList(pObjVehicleFunction);
        }

        public FuncResponse BL_InsertUpdateVehicleFunction(Vehicle_Function pObjVehicleFunction)
        {
            return _VehicleFunctionRepository.REP_InsertUpdateVehicleFunction(pObjVehicleFunction);
        }

        public usp_GetVehicleFunctionList_Result BL_GetVehicleFunctionDetails(Vehicle_Function pObjVehicleFunction)
        {
            return _VehicleFunctionRepository.REP_GetVehicleFunctionDetails(pObjVehicleFunction);
        }

        public IList<DropDownListResult> BL_GetVehicleFunctionDropDownList(Vehicle_Function pObjVehicleFunction)
        {
            return _VehicleFunctionRepository.REP_GetVehicleFunctionDropDownList(pObjVehicleFunction);
        }

        public FuncResponse BL_UpdateStatus(Vehicle_Function pObjVehicleFunction)
        {
            return _VehicleFunctionRepository.REP_UpdateStatus(pObjVehicleFunction);
        }
    }
}
