using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLLandStreetCondition
    {
        ILandStreetConditionRepository _LandStreetConditionRepository;

        public BLLandStreetCondition()
        {
            _LandStreetConditionRepository = new LandStreetConditionRepository();
        }

        public IList<usp_GetLandStreetConditionList_Result> BL_GetLandStreetConditionList(Land_StreetCondition pObjLandStreetCondition)
        {
            return _LandStreetConditionRepository.REP_GetLandStreetConditionList(pObjLandStreetCondition);
        }

        public FuncResponse BL_InsertUpdateLandStreetCondition(Land_StreetCondition pObjLandStreetCondition)
        {
            return _LandStreetConditionRepository.REP_InsertUpdateLandStreetCondition(pObjLandStreetCondition);
        }

        public usp_GetLandStreetConditionList_Result BL_GetLandStreetConditionDetails(Land_StreetCondition pObjLandStreetCondition)
        {
            return _LandStreetConditionRepository.REP_GetLandStreetConditionDetails(pObjLandStreetCondition);
        }

        public IList<DropDownListResult> BL_GetLandStreetConditionDropDownList(Land_StreetCondition pObjLandStreetCondition)
        {
            return _LandStreetConditionRepository.REP_GetLandStreetConditionDropDownList(pObjLandStreetCondition);
        }

        public FuncResponse BL_UpdateStatus(Land_StreetCondition pObjLandStreetCondition)
        {
            return _LandStreetConditionRepository.REP_UpdateStatus(pObjLandStreetCondition);
        }
    }
}
