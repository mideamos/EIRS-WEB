using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLVehicleOwnership
    {
        IVehicleOwnershipRepository _VehicleOwnershipRepository;

        public BLVehicleOwnership()
        {
            _VehicleOwnershipRepository = new VehicleOwnershipRepository();
        }

        public IList<usp_GetVehicleOwnershipList_Result> BL_GetVehicleOwnershipList(Vehicle_Ownership pObjVehicleOwnership)
        {
            return _VehicleOwnershipRepository.REP_GetVehicleOwnershipList(pObjVehicleOwnership);
        }

        public FuncResponse BL_InsertUpdateVehicleOwnership(Vehicle_Ownership pObjVehicleOwnership)
        {
            return _VehicleOwnershipRepository.REP_InsertUpdateVehicleOwnership(pObjVehicleOwnership);
        }

        public usp_GetVehicleOwnershipList_Result BL_GetVehicleOwnershipDetails(Vehicle_Ownership pObjVehicleOwnership)
        {
            return _VehicleOwnershipRepository.REP_GetVehicleOwnershipDetails(pObjVehicleOwnership);
        }

        public IList<DropDownListResult> BL_GetVehicleOwnershipDropDownList(Vehicle_Ownership pObjVehicleOwnership)
        {
            return _VehicleOwnershipRepository.REP_GetVehicleOwnershipDropDownList(pObjVehicleOwnership);
        }

        public FuncResponse BL_UpdateStatus(Vehicle_Ownership pObjVehicleOwnership)
        {
            return _VehicleOwnershipRepository.REP_UpdateStatus(pObjVehicleOwnership);
        }
    }
}
