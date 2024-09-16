using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IBuildingCompletionRepository
    {
        usp_GetBuildingCompletionList_Result REP_GetBuildingCompletionDetails(Building_Completion pObjBuildingCompletion);
        IList<DropDownListResult> REP_GetBuildingCompletionDropDownList(Building_Completion pObjBuildingCompletion);
        IList<usp_GetBuildingCompletionList_Result> REP_GetBuildingCompletionList(Building_Completion pObjBuildingCompletion);
        FuncResponse REP_InsertUpdateBuildingCompletion(Building_Completion pObjBuildingCompletion);
        FuncResponse REP_UpdateStatus(Building_Completion pObjBuildingCompletion);
    }
}