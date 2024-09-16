using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLAssessmentRule
    {
        IAssessmentRuleRepository _AssessmentRuleRepository;

        public BLAssessmentRule()
        {
            _AssessmentRuleRepository = new AssessmentRuleRepository();
        }

        public IList<usp_GetAssessmentRuleList_Result> BL_GetAssessmentRuleList(Assessment_Rules pObjAssessmentRule)
        {
            return _AssessmentRuleRepository.REP_GetAssessmentRuleList(pObjAssessmentRule);
        }

        public IList<vw_AssessmentRule> BL_GetAssessmentRuleList()
        {
            return _AssessmentRuleRepository.REP_GetAssessmentRuleList();
        }

        public FuncResponse<Assessment_Rules> BL_InsertUpdateAssessmentRule(Assessment_Rules pObjAssessmentRule)
        {
            return _AssessmentRuleRepository.REP_InsertUpdateAssessmentRule(pObjAssessmentRule);
        }

        public usp_GetAssessmentRuleList_Result BL_GetAssessmentRuleDetails(Assessment_Rules pObjAssessmentRule)
        {
            return _AssessmentRuleRepository.REP_GetAssessmentRuleDetails(pObjAssessmentRule);
        }

        public FuncResponse BL_UpdateStatus(Assessment_Rules pObjAssessmentRule)
        {
            return _AssessmentRuleRepository.REP_UpdateStatus(pObjAssessmentRule);
        }

        public FuncResponse BL_InsertSettlementMethod(MAP_AssessmentRule_SettlementMethod pObjSettlementMethod)
        {
            return _AssessmentRuleRepository.REP_InsertSettlementMethod(pObjSettlementMethod);
        }

        public FuncResponse BL_RemoveSettlementMethod(MAP_AssessmentRule_SettlementMethod pObjSettlementMethod)
        {
            return _AssessmentRuleRepository.REP_RemoveSettlementMethod(pObjSettlementMethod);
        }

        public IList<MAP_AssessmentRule_SettlementMethod> BL_GetSettlementMethod(int pIntSettlementMethodID)
        {
            return _AssessmentRuleRepository.REP_GetSettlementMethod(pIntSettlementMethodID);
        }

        public FuncResponse BL_InsertAssessmentItem(MAP_AssessmentRule_AssessmentItem pObjAssessmentItem)
        {
            return _AssessmentRuleRepository.REP_InsertAssessmentItem(pObjAssessmentItem);
        }

        public FuncResponse BL_RemoveAssessmentItem(MAP_AssessmentRule_AssessmentItem pObjAssessmentItem)
        {
            return _AssessmentRuleRepository.REP_RemoveAssessmentItem(pObjAssessmentItem);
        }

        public IList<MAP_AssessmentRule_AssessmentItem> BL_GetAssessmentItem(int pIntAssessmentRuleID)
        {
            return _AssessmentRuleRepository.REP_GetAssessmentItem(pIntAssessmentRuleID);
        }

        public IList<DropDownListResult> BL_GetSettlementMethodDropDownList(int pIntAssessmentRuleID)
        {
            return _AssessmentRuleRepository.REP_GetSettlementMethodDropDownList(pIntAssessmentRuleID);
        }

        public IList<usp_GetMASPriceSheet_Result> BL_GetMASPriceSheet(int pIntRequestType)
        {
            return _AssessmentRuleRepository.REP_GetMASPriceSheet(pIntRequestType);
        }


        public IList<usp_SearchAssessmentRulesForRDMLoad_Result> BL_SearchAssessmentRuleDetails(Assessment_Rules pObjAssessmentRule)
        {
            return _AssessmentRuleRepository.REP_SearchAssessmentRuleDetails(pObjAssessmentRule);
        }

        public IDictionary<string, object> BL_SearchAssessmentRule(Assessment_Rules pObjAssessmentRule)
        {
            return _AssessmentRuleRepository.REP_SearchAssessmentRule(pObjAssessmentRule);
        }

        public IDictionary<string, object> BL_SearchAssessmentRuleForSideMenu(Assessment_Rules pObjAssessmentRule)
        {
            return _AssessmentRuleRepository.REP_SearchAssessmentRuleForSideMenu(pObjAssessmentRule);
        }
    }
}
