using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLBuildingOwnership
    {
        IBuildingOwnershipRepository _BuildingOwnershipRepository;

        public BLBuildingOwnership()
        {
            _BuildingOwnershipRepository = new BuildingOwnershipRepository();
        }

        public IList<usp_GetBuildingOwnershipList_Result> BL_GetBuildingOwnershipList(Building_Ownership pObjBuildingOwnership)
        {
            return _BuildingOwnershipRepository.REP_GetBuildingOwnershipList(pObjBuildingOwnership);
        }

        public FuncResponse BL_InsertUpdateBuildingOwnership(Building_Ownership pObjBuildingOwnership)
        {
            return _BuildingOwnershipRepository.REP_InsertUpdateBuildingOwnership(pObjBuildingOwnership);
        }

        public usp_GetBuildingOwnershipList_Result BL_GetBuildingOwnershipDetails(Building_Ownership pObjBuildingOwnership)
        {
            return _BuildingOwnershipRepository.REP_GetBuildingOwnershipDetails(pObjBuildingOwnership);
        }

        public IList<DropDownListResult> BL_GetBuildingOwnershipDropDownList(Building_Ownership pObjBuildingOwnership)
        {
            return _BuildingOwnershipRepository.REP_GetBuildingOwnershipDropDownList(pObjBuildingOwnership);
        }

        public FuncResponse BL_UpdateStatus(Building_Ownership pObjBuildingOwnership)
        {
            return _BuildingOwnershipRepository.REP_UpdateStatus(pObjBuildingOwnership);
        }
    }
}
