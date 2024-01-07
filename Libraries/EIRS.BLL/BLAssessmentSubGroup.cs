using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLAssessmentSubGroup
    {
        IAssessmentSubGroupRepository _AssessmentSubGroupRepository;

        public BLAssessmentSubGroup()
        {
            _AssessmentSubGroupRepository = new AssessmentSubGroupRepository();
        }

        public IList<usp_GetAssessmentSubGroupList_Result> BL_GetAssessmentSubGroupList(Assessment_SubGroup pObjAssessmentSubGroup)
        {
            return _AssessmentSubGroupRepository.REP_GetAssessmentSubGroupList(pObjAssessmentSubGroup);
        }

        public FuncResponse BL_InsertUpdateAssessmentSubGroup(Assessment_SubGroup pObjAssessmentSubGroup)
        {
            return _AssessmentSubGroupRepository.REP_InsertUpdateAssessmentSubGroup(pObjAssessmentSubGroup);
        }

        public usp_GetAssessmentSubGroupList_Result BL_GetAssessmentSubGroupDetails(Assessment_SubGroup pObjAssessmentSubGroup)
        {
            return _AssessmentSubGroupRepository.REP_GetAssessmentSubGroupDetails(pObjAssessmentSubGroup);
        }

        public IList<DropDownListResult> BL_GetAssessmentSubGroupDropDownList(Assessment_SubGroup pObjAssessmentSubGroup)
        {
            return _AssessmentSubGroupRepository.REP_GetAssessmentSubGroupDropDownList(pObjAssessmentSubGroup);
        }

        public FuncResponse BL_UpdateStatus(Assessment_SubGroup pObjAssessmentSubGroup)
        {
            return _AssessmentSubGroupRepository.REP_UpdateStatus(pObjAssessmentSubGroup);
        }
    }
}
