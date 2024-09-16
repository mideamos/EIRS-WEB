using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLAssessmentItemCategory
    {
        IAssessmentItemCategoryRepository _AssessmentItemCategoryRepository;

        public BLAssessmentItemCategory()
        {
            _AssessmentItemCategoryRepository = new AssessmentItemCategoryRepository();
        }

        public IList<usp_GetAssessmentItemCategoryList_Result> BL_GetAssessmentItemCategoryList(Assessment_Item_Category pObjAssessmentItemCategory)
        {
            return _AssessmentItemCategoryRepository.REP_GetAssessmentItemCategoryList(pObjAssessmentItemCategory);
        }

        public FuncResponse BL_InsertUpdateAssessmentItemCategory(Assessment_Item_Category pObjAssessmentItemCategory)
        {
            return _AssessmentItemCategoryRepository.REP_InsertUpdateAssessmentItemCategory(pObjAssessmentItemCategory);
        }

        public usp_GetAssessmentItemCategoryList_Result BL_GetAssessmentItemCategoryDetails(Assessment_Item_Category pObjAssessmentItemCategory)
        {
            return _AssessmentItemCategoryRepository.REP_GetAssessmentItemCategoryDetails(pObjAssessmentItemCategory);
        }

        public IList<DropDownListResult> BL_GetAssessmentItemCategoryDropDownList(Assessment_Item_Category pObjAssessmentItemCategory)
        {
            return _AssessmentItemCategoryRepository.REP_GetAssessmentItemCategoryDropDownList(pObjAssessmentItemCategory);
        }

        public FuncResponse BL_UpdateStatus(Assessment_Item_Category pObjAssessmentItemCategory)
        {
            return _AssessmentItemCategoryRepository.REP_UpdateStatus(pObjAssessmentItemCategory);
        }
    }
}
