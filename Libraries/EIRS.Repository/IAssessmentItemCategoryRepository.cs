using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IAssessmentItemCategoryRepository
    {
		FuncResponse REP_InsertUpdateAssessmentItemCategory(Assessment_Item_Category pObjAssessmentItemCategory);
		IList<usp_GetAssessmentItemCategoryList_Result> REP_GetAssessmentItemCategoryList(Assessment_Item_Category pObjAssessmentItemCategory);
        usp_GetAssessmentItemCategoryList_Result REP_GetAssessmentItemCategoryDetails(Assessment_Item_Category pObjAssessmentItemCategory);
        IList<DropDownListResult> REP_GetAssessmentItemCategoryDropDownList(Assessment_Item_Category pObjAssessmentItemCategory);
        FuncResponse REP_UpdateStatus(Assessment_Item_Category pObjAssessmentItemCategory);
    }
}