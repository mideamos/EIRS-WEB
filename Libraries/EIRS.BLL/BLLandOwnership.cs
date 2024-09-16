using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLLandOwnership
    {
        ILandOwnershipRepository _LandOwnershipRepository;

        public BLLandOwnership()
        {
            _LandOwnershipRepository = new LandOwnershipRepository();
        }

        public IList<usp_GetLandOwnershipList_Result> BL_GetLandOwnershipList(Land_Ownership pObjLandOwnership)
        {
            return _LandOwnershipRepository.REP_GetLandOwnershipList(pObjLandOwnership);
        }

        public FuncResponse BL_InsertUpdateLandOwnership(Land_Ownership pObjLandOwnership)
        {
            return _LandOwnershipRepository.REP_InsertUpdateLandOwnership(pObjLandOwnership);
        }

        public usp_GetLandOwnershipList_Result BL_GetLandOwnershipDetails(Land_Ownership pObjLandOwnership)
        {
            return _LandOwnershipRepository.REP_GetLandOwnershipDetails(pObjLandOwnership);
        }

        public IList<DropDownListResult> BL_GetLandOwnershipDropDownList(Land_Ownership pObjLandOwnership)
        {
            return _LandOwnershipRepository.REP_GetLandOwnershipDropDownList(pObjLandOwnership);
        }

        public FuncResponse BL_UpdateStatus(Land_Ownership pObjLandOwnership)
        {
            return _LandOwnershipRepository.REP_UpdateStatus(pObjLandOwnership);
        }
    }
}
