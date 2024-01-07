using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLLandPurpose
    {
        ILandPurposeRepository _LandPurposeRepository;

        public BLLandPurpose()
        {
            _LandPurposeRepository = new LandPurposeRepository();
        }

        public IList<usp_GetLandPurposeList_Result> BL_GetLandPurposeList(Land_Purpose pObjLandPurpose)
        {
            return _LandPurposeRepository.REP_GetLandPurposeList(pObjLandPurpose);
        }

        public FuncResponse BL_InsertUpdateLandPurpose(Land_Purpose pObjLandPurpose)
        {
            return _LandPurposeRepository.REP_InsertUpdateLandPurpose(pObjLandPurpose);
        }

        public usp_GetLandPurposeList_Result BL_GetLandPurposeDetails(Land_Purpose pObjLandPurpose)
        {
            return _LandPurposeRepository.REP_GetLandPurposeDetails(pObjLandPurpose);
        }

        public IList<DropDownListResult> BL_GetLandPurposeDropDownList(Land_Purpose pObjLandPurpose)
        {
            return _LandPurposeRepository.REP_GetLandPurposeDropDownList(pObjLandPurpose);
        }

        public FuncResponse BL_UpdateStatus(Land_Purpose pObjLandPurpose)
        {
            return _LandPurposeRepository.REP_UpdateStatus(pObjLandPurpose);
        }
    }
}
