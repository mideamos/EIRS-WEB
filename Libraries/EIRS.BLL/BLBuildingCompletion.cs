using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLBuildingCompletion
    {
        IBuildingCompletionRepository _BuildingCompletionRepository;

        public BLBuildingCompletion()
        {
            _BuildingCompletionRepository = new BuildingCompletionRepository();
        }

        public IList<usp_GetBuildingCompletionList_Result> BL_GetBuildingCompletionList(Building_Completion pObjBuildingCompletion)
        {
            return _BuildingCompletionRepository.REP_GetBuildingCompletionList(pObjBuildingCompletion);
        }

        public FuncResponse BL_InsertUpdateBuildingCompletion(Building_Completion pObjBuildingCompletion)
        {
            return _BuildingCompletionRepository.REP_InsertUpdateBuildingCompletion(pObjBuildingCompletion);
        }

        public usp_GetBuildingCompletionList_Result BL_GetBuildingCompletionDetails(Building_Completion pObjBuildingCompletion)
        {
            return _BuildingCompletionRepository.REP_GetBuildingCompletionDetails(pObjBuildingCompletion);
        }

        public IList<DropDownListResult> BL_GetBuildingCompletionDropDownList(Building_Completion pObjBuildingCompletion)
        {
            return _BuildingCompletionRepository.REP_GetBuildingCompletionDropDownList(pObjBuildingCompletion);
        }

        public FuncResponse BL_UpdateStatus(Building_Completion pObjBuildingCompletion)
        {
            return _BuildingCompletionRepository.REP_UpdateStatus(pObjBuildingCompletion);
        }
    }
}
