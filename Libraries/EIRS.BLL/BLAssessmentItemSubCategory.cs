using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLAssessmentItemSubCategory
    {
        IAssessmentItemSubCategoryRepository _AssessmentItemSubCategoryRepository;

        public BLAssessmentItemSubCategory()
        {
            _AssessmentItemSubCategoryRepository = new AssessmentItemSubCategoryRepository();
        }

        public IList<usp_GetAssessmentItemSubCategoryList_Result> BL_GetAssessmentItemSubCategoryList(Assessment_Item_SubCategory pObjAssessmentItemSubCategory)
        {
            return _AssessmentItemSubCategoryRepository.REP_GetAssessmentItemSubCategoryList(pObjAssessmentItemSubCategory);
        }

        public FuncResponse BL_InsertUpdateAssessmentItemSubCategory(Assessment_Item_SubCategory pObjAssessmentItemSubCategory)
        {
            return _AssessmentItemSubCategoryRepository.REP_InsertUpdateAssessmentItemSubCategory(pObjAssessmentItemSubCategory);
        }

        public usp_GetAssessmentItemSubCategoryList_Result BL_GetAssessmentItemSubCategoryDetails(Assessment_Item_SubCategory pObjAssessmentItemSubCategory)
        {
            return _AssessmentItemSubCategoryRepository.REP_GetAssessmentItemSubCategoryDetails(pObjAssessmentItemSubCategory);
        }

        public IList<DropDownListResult> BL_GetAssessmentItemSubCategoryDropDownList(Assessment_Item_SubCategory pObjAssessmentItemSubCategory)
        {
            return _AssessmentItemSubCategoryRepository.REP_GetAssessmentItemSubCategoryDropDownList(pObjAssessmentItemSubCategory);
        }

        public FuncResponse BL_UpdateStatus(Assessment_Item_SubCategory pObjAssessmentItemSubCategory)
        {
            return _AssessmentItemSubCategoryRepository.REP_UpdateStatus(pObjAssessmentItemSubCategory);
        }
    }
}
