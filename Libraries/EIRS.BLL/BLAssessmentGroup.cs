using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLAssessmentGroup
    {
        IAssessmentGroupRepository _AssessmentGroupRepository;

        public BLAssessmentGroup()
        {
            _AssessmentGroupRepository = new AssessmentGroupRepository();
        }

        public IList<usp_GetAssessmentGroupList_Result> BL_GetAssessmentGroupList(Assessment_Group pObjAssessmentGroup)
        {
            return _AssessmentGroupRepository.REP_GetAssessmentGroupList(pObjAssessmentGroup);
        }

        public FuncResponse BL_InsertUpdateAssessmentGroup(Assessment_Group pObjAssessmentGroup)
        {
            return _AssessmentGroupRepository.REP_InsertUpdateAssessmentGroup(pObjAssessmentGroup);
        }

        public usp_GetAssessmentGroupList_Result BL_GetAssessmentGroupDetails(Assessment_Group pObjAssessmentGroup)
        {
            return _AssessmentGroupRepository.REP_GetAssessmentGroupDetails(pObjAssessmentGroup);
        }

        public IList<DropDownListResult> BL_GetAssessmentGroupDropDownList(Assessment_Group pObjAssessmentGroup)
        {
            return _AssessmentGroupRepository.REP_GetAssessmentGroupDropDownList(pObjAssessmentGroup);
        }

        public FuncResponse BL_UpdateStatus(Assessment_Group pObjAssessmentGroup)
        {
            return _AssessmentGroupRepository.REP_UpdateStatus(pObjAssessmentGroup);
        }
    }
}
