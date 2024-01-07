using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLVehiclePurpose
    {
        IVehiclePurposeRepository _VehiclePurposeRepository;

        public BLVehiclePurpose()
        {
            _VehiclePurposeRepository = new VehiclePurposeRepository();
        }

        public IList<usp_GetVehiclePurposeList_Result> BL_GetVehiclePurposeList(Vehicle_Purpose pObjVehiclePurpose)
        {
            return _VehiclePurposeRepository.REP_GetVehiclePurposeList(pObjVehiclePurpose);
        }

        public FuncResponse BL_InsertUpdateVehiclePurpose(Vehicle_Purpose pObjVehiclePurpose)
        {
            return _VehiclePurposeRepository.REP_InsertUpdateVehiclePurpose(pObjVehiclePurpose);
        }

        public usp_GetVehiclePurposeList_Result BL_GetVehiclePurposeDetails(Vehicle_Purpose pObjVehiclePurpose)
        {
            return _VehiclePurposeRepository.REP_GetVehiclePurposeDetails(pObjVehiclePurpose);
        }

        public IList<DropDownListResult> BL_GetVehiclePurposeDropDownList(Vehicle_Purpose pObjVehiclePurpose)
        {
            return _VehiclePurposeRepository.REP_GetVehiclePurposeDropDownList(pObjVehiclePurpose);
        }

        public FuncResponse BL_UpdateStatus(Vehicle_Purpose pObjVehiclePurpose)
        {
            return _VehiclePurposeRepository.REP_UpdateStatus(pObjVehiclePurpose);
        }
    }
}
