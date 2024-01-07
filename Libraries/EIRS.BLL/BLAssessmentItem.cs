using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLAssessmentItem
    {
        IAssessmentItemRepository _AssessmentItemRepository;

        public BLAssessmentItem()
        {
            _AssessmentItemRepository = new AssessmentItemRepository();
        }

        public IList<usp_GetAssessmentItemList_Result> BL_GetAssessmentItemList(Assessment_Items pObjAssessmentItem)
        {
            return _AssessmentItemRepository.REP_GetAssessmentItemList(pObjAssessmentItem);
        }

        public FuncResponse BL_InsertUpdateAssessmentItem(Assessment_Items pObjAssessmentItem)
        {
            return _AssessmentItemRepository.REP_InsertUpdateAssessmentItem(pObjAssessmentItem);
        }

        public usp_GetAssessmentItemList_Result BL_GetAssessmentItemDetails(Assessment_Items pObjAssessmentItem)
        {
            return _AssessmentItemRepository.REP_GetAssessmentItemDetails(pObjAssessmentItem);
        }

        public IList<DropDownListResult> BL_GetAssessmentItemDropDownList(Assessment_Items pObjAssessmentItem)
        {
            return _AssessmentItemRepository.REP_GetAssessmentItemDropDownList(pObjAssessmentItem);
        }

        public FuncResponse BL_UpdateStatus(Assessment_Items pObjAssessmentItem)
        {
            return _AssessmentItemRepository.REP_UpdateStatus(pObjAssessmentItem);
        }

        public IList<usp_SearchAssessmentItemForRDMLoad_Result> BL_SearchAssessmentItemDetails(Assessment_Items pObjAssessmentItem)
        {
            return _AssessmentItemRepository.REP_SearchAssessmentItemDetails(pObjAssessmentItem);
        }

        public IDictionary<string, object> BL_SearchAssessmentItem(Assessment_Items pObjAssessmentItem)
        {
            return _AssessmentItemRepository.REP_SearchAssessmentItem(pObjAssessmentItem);
        }
    }
}
