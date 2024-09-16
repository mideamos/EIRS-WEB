using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLAssessment
    {
        IAssessmentRepository _AssessmentRepository;

        public BLAssessment()
        {
            _AssessmentRepository = new AssessmentRepository();
        }

        public IList<usp_GetAssessmentList_Result> BL_GetAssessmentList(Assessment pObjAssessment)
        {
            return _AssessmentRepository.REP_GetAssessmentList(pObjAssessment);
        }

        public IList<vw_AssessmentBill> BL_GetAssessmentList()
        {
            return _AssessmentRepository.REP_GetAssessmentList();
        }

        public FuncResponse<Assessment> BL_InsertUpdateAssessment(Assessment pObjAssessment)
        {
            return _AssessmentRepository.REP_InsertUpdateAssessment(pObjAssessment);
        }

        public IList<usp_GetAssessmentDataForPT_Result> BL_GetAssessmentDataForPT()
        {
            return _AssessmentRepository.REP_GetAssessmentDataForPT();
        }

        public FuncResponse<MAP_Assessment_AssessmentRule> BL_InsertUpdateAssessmentRule(MAP_Assessment_AssessmentRule pObjAssessmentRule)
        {
            return _AssessmentRepository.REP_InsertUpdateAssessmentRule(pObjAssessmentRule);
        }

        public IList<usp_GetAssessment_AssessmentRuleList_Result> BL_GetAssessmentRules(long pIntAssessmentID)
        {
            return _AssessmentRepository.REP_GetAssessmentRules(Convert.ToInt32(pIntAssessmentID));
        }

        public usp_GetAssessmentList_Result BL_GetAssessmentDetails(Assessment pObjAssessment)
        {
            return _AssessmentRepository.REP_GetAssessmentDetails(pObjAssessment);
        }

        public FuncResponse BL_InsertAssessmentItem(IList<MAP_Assessment_AssessmentItem> plstAssessmentItem)
        {
            return _AssessmentRepository.REP_InsertAssessmentItem(plstAssessmentItem);
        }

        public IList<MAP_Assessment_AssessmentItem> BL_GetAssessmentItems(long plngAARID)
        {
            return _AssessmentRepository.REP_GetAssessmentItems(plngAARID);
        }
        public MAP_Assessment_AssessmentItem GetAssessmentItems(long aAIID)
        {
            return _AssessmentRepository.GetAssessmentItems(aAIID);
        }

        public IList<usp_GetAssessmentRuleItemList_Result> BL_GetAssessmentRuleItem(long pIntAssessmentID)
        {
            return _AssessmentRepository.REP_GetAssessmentRuleItem(Convert.ToInt32(pIntAssessmentID));
        }

        public usp_GetAssessmentRuleItemDetails_Result BL_GetAssessmentRuleItemDetails(long plngAAIID)
        {
            return _AssessmentRepository.REP_GetAssessmentRuleItemDetails(plngAAIID);
        }

        public IList<usp_GetAssessmentAdjustmentList_Result> BL_GetAssessmentAdjustment(long pIntAssessmentID)
        {
            return _AssessmentRepository.REP_GetAssessmentAdjustment(Convert.ToInt32(pIntAssessmentID));
        }

        public IList<usp_GetAssessmentLateChargeList_Result> BL_GetAssessmentLateCharge(long pIntAssessmentID)
        {
            return _AssessmentRepository.REP_GetAssessmentLateCharge(Convert.ToInt32(pIntAssessmentID));
        }

        public IList<usp_GetAssessmentRuleBasedSettlement_Result> BL_GetAssessmentRuleBasedSettlement(long pIntAssessmentID)
        {
            return _AssessmentRepository.REP_GetAssessmentRuleBasedSettlement(Convert.ToInt32(pIntAssessmentID));
        }

        public FuncResponse BL_UpdateAssessmentItemStatus(MAP_Assessment_AssessmentItem pObjAssessmentItem)
        {
            return _AssessmentRepository.REP_UpdateAssessmentItemStatus(pObjAssessmentItem);
        }
        public FuncResponse BL_UpdateAssessmentItemStatus2(MAP_Assessment_AssessmentItem pObjAssessmentItem)
        {
            return _AssessmentRepository.REP_UpdateAssessmentItemStatus2(pObjAssessmentItem);
        }

        public FuncResponse BL_UpdateStatus(Assessment pObjAssessment)
        {
            return _AssessmentRepository.REP_UpdateStatus(pObjAssessment);
        }

        public IList<DropDownListResult> BL_GetSettlementMethodAssessmentRuleBased(long pIntAssessmentID)
        {
            return _AssessmentRepository.REP_GetSettlementMethodAssessmentRuleBased(pIntAssessmentID);
        }

        public FuncResponse BL_UpdateAssessmentSettlementStatus(Assessment pObjAssessment)
        {
            return _AssessmentRepository.REP_UpdateAssessmentSettlementStatus(pObjAssessment);
        }

        public FuncResponse BL_DeleteAssessmentRule(long pIntAARID)
        {
            return _AssessmentRepository.REP_DeleteAssessmentRule(pIntAARID);
        }

        public IList<usp_GetTaxPayerBill_Result> BL_GetTaxPayerBill(int pIntTaxPayerID, int pIntTaxPayerTypeID, int pIntStatusID)
        {
            return _AssessmentRepository.REP_GetTaxPayerBill(pIntTaxPayerID, pIntTaxPayerTypeID, pIntStatusID);
        }

        public IList<vw_BillForPoASettlement> BL_GetBillForPoASettlementList()
        {
            return _AssessmentRepository.REP_GetBillForPoASettlementList();
        }

        public void BL_UpdateAssessmentAmount(long pIntAssessmentID)
        {
            _AssessmentRepository.REP_UpdateAssessmentAmount(Convert.ToInt32(pIntAssessmentID));
        }

        public IDictionary<string, object> BL_SearchAssessment(Assessment pObjAssessment)
        {
            return _AssessmentRepository.REP_SearchAssessment(pObjAssessment);
        }

        public FuncResponse BL_InsertAdjustment(MAP_Assessment_Adjustment pObjAdjustment)
        {
            return _AssessmentRepository.REP_InsertAdjustment(pObjAdjustment);
        }

        public IDictionary<string, object> BL_SearchAssessmentForSideMenu(Assessment pObjAssessment)
        {
            return _AssessmentRepository.REP_SearchAssessmentForSideMenu(pObjAssessment);
        }

        public IList<usp_GetPAYEAssessmentBill_Result> BL_GetPAYEAssessmentBill(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            return _AssessmentRepository.REP_GetPAYEAssessmentBill(pIntTaxPayerID, pIntTaxPayerTypeID);
        }
    }

}
