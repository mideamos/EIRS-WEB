using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IAssessmentItemSubCategoryRepository
    {
        usp_GetAssessmentItemSubCategoryList_Result REP_GetAssessmentItemSubCategoryDetails(Assessment_Item_SubCategory pObjAssessmentItemSubCategory);
        IList<DropDownListResult> REP_GetAssessmentItemSubCategoryDropDownList(Assessment_Item_SubCategory pObjAssessmentItemSubCategory);
        IList<usp_GetAssessmentItemSubCategoryList_Result> REP_GetAssessmentItemSubCategoryList(Assessment_Item_SubCategory pObjAssessmentItemSubCategory);
        FuncResponse REP_InsertUpdateAssessmentItemSubCategory(Assessment_Item_SubCategory pObjAssessmentItemSubCategory);
        FuncResponse REP_UpdateStatus(Assessment_Item_SubCategory pObjAssessmentItemSubCategory);
    }
}