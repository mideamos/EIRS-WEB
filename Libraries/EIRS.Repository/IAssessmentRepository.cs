using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IAssessmentRepository
    {
        usp_GetAssessmentList_Result REP_GetAssessmentDetails(Assessment pObjAssessment);
        IList<usp_GetAssessmentList_Result> REP_GetAssessmentList(Assessment pObjAssessment);
        FuncResponse<Assessment> REP_InsertUpdateAssessment(Assessment pObjAssessment);
        FuncResponse<Assessment> REP_InsertUpdateAssessment(Assessment pObjAssessment,int ruleId);
        FuncResponse<Assessment> REP_InsertUpdateAssessment(Assessment pObjAssessment,int ruleId,int assId);
        FuncResponse<MAP_Assessment_AssessmentRule> REP_InsertUpdateAssessmentRule(MAP_Assessment_AssessmentRule pObjAssessmentRule);

        IList<usp_GetAssessment_AssessmentRuleList_Result> REP_GetAssessmentRules(int pIntAssessmentID);

        FuncResponse REP_InsertAssessmentItem(IList<MAP_Assessment_AssessmentItem> plstAssessmentItem);
        IList<MAP_Assessment_AssessmentItem> REP_GetAssessmentItems(long plngAARID);
        MAP_Assessment_AssessmentItem GetAssessmentItems(long aAIID);

        IList<usp_GetAssessmentRuleItemList_Result> REP_GetAssessmentRuleItem(int pIntAssessmentID);

        FuncResponse REP_UpdateAssessmentItemStatus(MAP_Assessment_AssessmentItem pObjAssessmentItem);
        FuncResponse REP_UpdateAssessmentItemStatus2(MAP_Assessment_AssessmentItem pObjAssessmentItem);

        FuncResponse REP_UpdateStatus(Assessment pObjAssessment);

        IList<DropDownListResult> REP_GetSettlementMethodAssessmentRuleBased(long pIntAssessmentID);

        FuncResponse REP_UpdateAssessmentSettlementStatus(Assessment pObjAssessment);

        FuncResponse REP_DeleteAssessmentRule(long pIntAARID);

        IList<usp_GetAssessmentRuleBasedSettlement_Result> REP_GetAssessmentRuleBasedSettlement(int pIntAssessmentID);

        IList<usp_GetTaxPayerBill_Result> REP_GetTaxPayerBill(int pIntTaxPayerID, int pIntTaxPayerTypeID, int pIntStatusID);

        IList<vw_AssessmentBill> REP_GetAssessmentList();

        IList<vw_BillForPoASettlement> REP_GetBillForPoASettlementList();

        IList<usp_GetAssessmentDataForPT_Result> REP_GetAssessmentDataForPT();
        IDictionary<string, object> REP_SearchAssessment(Assessment pObjAssessment);
        IList<usp_GetAssessmentAdjustmentList_Result> REP_GetAssessmentAdjustment(int pIntAssessmentID);
        IList<usp_GetAssessmentLateChargeList_Result> REP_GetAssessmentLateCharge(int pIntAssessmentID);
        void REP_UpdateAssessmentAmount(int pIntAssessmentID);
        FuncResponse REP_InsertAdjustment(MAP_Assessment_Adjustment pObjAdjustment);
        usp_GetAssessmentRuleItemDetails_Result REP_GetAssessmentRuleItemDetails(long plngAAIID);

        IDictionary<string, object> REP_SearchAssessmentForSideMenu(Assessment pObjAssessment);

        IList<usp_GetPAYEAssessmentBill_Result> REP_GetPAYEAssessmentBill(int pIntTaxPayerID, int pIntTaxPayerTypeID);
    }
}