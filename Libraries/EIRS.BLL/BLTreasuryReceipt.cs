using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
   public class BLTreasuryReceipt
    {
        ITreasuryReceiptRepository _TreasuryReceiptRepository;

        public BLTreasuryReceipt()
        {
            _TreasuryReceiptRepository = new TreasuryReceiptRepository();
        }

        public IList<usp_GetTreasuryReceiptList_Result> BL_GetTreasuryReceiptList(Treasury_Receipt pObjTreasuryReceipt)
        {
            return _TreasuryReceiptRepository.REP_GetTreasuryReceiptList(pObjTreasuryReceipt);
        }

        public FuncResponse<Treasury_Receipt> BL_InsertTreasuryReceipt(Treasury_Receipt pObjTreasuryReceipt)
        {
            return _TreasuryReceiptRepository.REP_InsertTreasuryReceipt(pObjTreasuryReceipt);
        }

        public FuncResponse BL_InsertReceiptSettlement(MAP_TreasuryReceipt_Settlement pObjReceiptSettlement)
        {
            return _TreasuryReceiptRepository.REP_InsertReceiptSettlement(pObjReceiptSettlement);
        }

        public usp_GetTreasuryReceiptList_Result BL_GetTreasuryReceiptDetails(Treasury_Receipt pObjTreasuryReceipt)
        {
            return _TreasuryReceiptRepository.REP_GetTreasuryReceiptDetails(pObjTreasuryReceipt);
        }

        public IDictionary<string, object> BL_SearchTreasuryReceipt(Treasury_Receipt pObjTreasuryReceipt)
        {
            return _TreasuryReceiptRepository.REP_SearchTreasuryReceipt(pObjTreasuryReceipt);
        }

        public FuncResponse BL_CancelTreasuryReceipt(Treasury_Receipt pObjTreasuryReceipt)
        {
            return _TreasuryReceiptRepository.REP_CancelTreasuryReceipt(pObjTreasuryReceipt);
        }

        public FuncResponse BL_UpdateTRSigned(Treasury_Receipt pObjReceipt)
        {
            return _TreasuryReceiptRepository.REP_UpdateTRSigned(pObjReceipt);
        }

        public void BL_UpdateSignedPath(Treasury_Receipt pObjReceipt)
        {
            _TreasuryReceiptRepository.REP_UpdateSignedPath(pObjReceipt);
        }

        public FuncResponse BL_UpdateTRGenerated(Treasury_Receipt pObjReceipt)
        {
            return _TreasuryReceiptRepository.REP_UpdateTRGenerated(pObjReceipt);
        }

        public IList<usp_GetSettlementWithoutReceipt_Result> BL_GetSettlementWithoutReceipt(long plngAssessmentID, long plngServiceBillID)
        {
            return _TreasuryReceiptRepository.REP_GetSettlementWithoutReceipt(plngAssessmentID,plngServiceBillID);
        }

        public usp_GetTreasuryReceiptList_Result BL_VerifyTreasuryReceipt(Treasury_Receipt pObjTreasuryReceipt)
        {
            return _TreasuryReceiptRepository.REP_VerifyTreasuryReceipt(pObjTreasuryReceipt);
        }
    }
}
