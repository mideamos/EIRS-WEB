using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IAssessmentSubGroupRepository
    {
        usp_GetAssessmentSubGroupList_Result REP_GetAssessmentSubGroupDetails(Assessment_SubGroup pObjAssessmentSubGroup);
        IList<DropDownListResult> REP_GetAssessmentSubGroupDropDownList(Assessment_SubGroup pObjAssessmentSubGroup);
        IList<usp_GetAssessmentSubGroupList_Result> REP_GetAssessmentSubGroupList(Assessment_SubGroup pObjAssessmentSubGroup);
        FuncResponse REP_InsertUpdateAssessmentSubGroup(Assessment_SubGroup pObjAssessmentSubGroup);
        FuncResponse REP_UpdateStatus(Assessment_SubGroup pObjAssessmentSubGroup);
    }
}