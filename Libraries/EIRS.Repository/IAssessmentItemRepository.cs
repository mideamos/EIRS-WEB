using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IAssessmentItemRepository
    {
        usp_GetAssessmentItemList_Result REP_GetAssessmentItemDetails(Assessment_Items pObjAssessmentItem);
        IList<DropDownListResult> REP_GetAssessmentItemDropDownList(Assessment_Items pObjAssessmentItem);
        IList<usp_GetAssessmentItemList_Result> REP_GetAssessmentItemList(Assessment_Items pObjAssessmentItem);
        FuncResponse REP_InsertUpdateAssessmentItem(Assessment_Items pObjAssessmentItem);
        FuncResponse REP_UpdateStatus(Assessment_Items pObjAssessmentItem);

        IList<usp_SearchAssessmentItemForRDMLoad_Result> REP_SearchAssessmentItemDetails(Assessment_Items pObjAssessmentItem);
        IDictionary<string, object> REP_SearchAssessmentItem(Assessment_Items pObjAssessmentItem);
    }
}