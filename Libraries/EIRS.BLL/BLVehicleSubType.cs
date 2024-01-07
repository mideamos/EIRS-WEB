using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLVehicleSubType
    {
        IVehicleSubTypeRepository _VehicleSubTypeRepository;

        public BLVehicleSubType()
        {
            _VehicleSubTypeRepository = new VehicleSubTypeRepository();
        }

        public IList<usp_GetVehicleSubTypeList_Result> BL_GetVehicleSubTypeList(Vehicle_SubTypes pObjVehicleSubType)
        {
            return _VehicleSubTypeRepository.REP_GetVehicleSubTypeList(pObjVehicleSubType);
        }

        public FuncResponse BL_InsertUpdateVehicleSubType(Vehicle_SubTypes pObjVehicleSubType)
        {
            return _VehicleSubTypeRepository.REP_InsertUpdateVehicleSubType(pObjVehicleSubType);
        }

        public usp_GetVehicleSubTypeList_Result BL_GetVehicleSubTypeDetails(Vehicle_SubTypes pObjVehicleSubType)
        {
            return _VehicleSubTypeRepository.REP_GetVehicleSubTypeDetails(pObjVehicleSubType);
        }

        public IList<DropDownListResult> BL_GetVehicleSubTypeDropDownList(Vehicle_SubTypes pObjVehicleSubType)
        {
            return _VehicleSubTypeRepository.REP_GetVehicleSubTypeDropDownList(pObjVehicleSubType);
        }

        public FuncResponse BL_UpdateStatus(Vehicle_SubTypes pObjVehicleSubType)
        {
            return _VehicleSubTypeRepository.REP_UpdateStatus(pObjVehicleSubType);
        }
    }
}
