using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;
using static EIRS.Repository.DataControlRepository;

namespace EIRS.Repository
{
    public interface IAssessmentRuleRepository
    {
        IList<MAP_AssessmentRule_AssessmentItem> REP_GetAssessmentItem(int pIntAssessmentRuleID);
        FuncResponse<List<Assessment_Rules>> REP_InsertUpdateAssessmentRule(List<AssessmentRuleRollover> roll);
        usp_GetAssessmentRuleList_Result REP_GetAssessmentRuleDetails(Assessment_Rules pObjAssessmentRule);
        IList<usp_GetAssessmentRuleList_Result> REP_GetAssessmentRuleList(Assessment_Rules pObjAssessmentRule);
        IList<MAP_AssessmentRule_SettlementMethod> REP_GetSettlementMethod(int pIntAssessmentRuleID);
        FuncResponse REP_InsertAssessmentItem(MAP_AssessmentRule_AssessmentItem pObjAssessmentItem);
        FuncResponse REP_InsertSettlementMethod(MAP_AssessmentRule_SettlementMethod pObjSettlementMethod);
        FuncResponse<Assessment_Rules> REP_InsertUpdateAssessmentRule(Assessment_Rules pObjAssessmentRule);
        FuncResponse REP_RemoveAssessmentItem(MAP_AssessmentRule_AssessmentItem pObjAssessmentItem);
        FuncResponse REP_RemoveSettlementMethod(MAP_AssessmentRule_SettlementMethod pObjSettlementMethod);
        FuncResponse REP_UpdateStatus(Assessment_Rules pObjAssessmentRule);

        IList<DropDownListResult> REP_GetSettlementMethodDropDownList(int pIntAssessmentRuleID);

        IList<vw_AssessmentRule> REP_GetAssessmentRuleList();
        IList<usp_GetMASPriceSheet_Result> REP_GetMASPriceSheet(int pIntRequestType);

        IList<usp_SearchAssessmentRulesForRDMLoad_Result> REP_SearchAssessmentRuleDetails(Assessment_Rules pObjAssessmentRule);
        IDictionary<string, object> REP_SearchAssessmentRule(Assessment_Rules pObjAssessmentRule);

        IDictionary<string, object> REP_SearchAssessmentRuleForSideMenu(Assessment_Rules pObjAssessmentRule);
    }
}