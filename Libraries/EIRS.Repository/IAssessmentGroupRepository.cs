using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IAssessmentGroupRepository
    {
        usp_GetAssessmentGroupList_Result REP_GetAssessmentGroupDetails(Assessment_Group pObjAssessmentGroup);
        IList<DropDownListResult> REP_GetAssessmentGroupDropDownList(Assessment_Group pObjAssessmentGroup);
        IList<usp_GetAssessmentGroupList_Result> REP_GetAssessmentGroupList(Assessment_Group pObjAssessmentGroup);
        FuncResponse REP_InsertUpdateAssessmentGroup(Assessment_Group pObjAssessmentGroup);
        FuncResponse REP_UpdateStatus(Assessment_Group pObjAssessmentGroup);
    }
}