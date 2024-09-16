using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ITreasuryReceiptRepository
    {
        usp_GetTreasuryReceiptList_Result REP_GetTreasuryReceiptDetails(Treasury_Receipt pObjReceipt);
        IList<usp_GetTreasuryReceiptList_Result> REP_GetTreasuryReceiptList(Treasury_Receipt pObjReceipt);
        FuncResponse<Treasury_Receipt> REP_InsertTreasuryReceipt(Treasury_Receipt pObjReceipt);
        IDictionary<string, object> REP_SearchTreasuryReceipt(Treasury_Receipt pObjReceipt);
        FuncResponse REP_CancelTreasuryReceipt(Treasury_Receipt pObjReceipt);
        FuncResponse REP_UpdateTRSigned(Treasury_Receipt pObjReceipt);
        void REP_UpdateSignedPath(Treasury_Receipt pObjReceipt);
        FuncResponse REP_UpdateTRGenerated(Treasury_Receipt pObjReceipt);

        IList<usp_GetSettlementWithoutReceipt_Result> REP_GetSettlementWithoutReceipt(long plngAssessmentID, long plngServiceBillID);

        FuncResponse REP_InsertReceiptSettlement(MAP_TreasuryReceipt_Settlement pObjReceiptSettlement);

        usp_GetTreasuryReceiptList_Result REP_VerifyTreasuryReceipt(Treasury_Receipt pObjTreasuryReceipt);
        Treasury_Receipt REP_VerifyTreasuryReceiptNew(Treasury_Receipt pObjTreasuryReceipt);
    }
}