using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLVehicleType
    {
        IVehicleTypeRepository _VehicleTypeRepository;

        public BLVehicleType()
        {
            _VehicleTypeRepository = new VehicleTypeRepository();
        }

        public IList<usp_GetVehicleTypeList_Result> BL_GetVehicleTypeList(Vehicle_Types pObjVehicleType)
        {
            return _VehicleTypeRepository.REP_GetVehicleTypeList(pObjVehicleType);
        }

        public FuncResponse BL_InsertUpdateVehicleType(Vehicle_Types pObjVehicleType)
        {
            return _VehicleTypeRepository.REP_InsertUpdateVehicleType(pObjVehicleType);
        }

        public usp_GetVehicleTypeList_Result BL_GetVehicleTypeDetails(Vehicle_Types pObjVehicleType)
        {
            return _VehicleTypeRepository.REP_GetVehicleTypeDetails(pObjVehicleType);
        }

        public IList<DropDownListResult> BL_GetVehicleTypeDropDownList(Vehicle_Types pObjVehicleType)
        {
            return _VehicleTypeRepository.REP_GetVehicleTypeDropDownList(pObjVehicleType);
        }

        public FuncResponse BL_UpdateStatus(Vehicle_Types pObjVehicleType)
        {
            return _VehicleTypeRepository.REP_UpdateStatus(pObjVehicleType);
        }
    }
}
