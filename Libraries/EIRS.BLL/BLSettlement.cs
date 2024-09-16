using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLSettlement
    {
        ISettlementRepository _SettlementRepository;

        public BLSettlement()
        {
            _SettlementRepository = new SettlementRepository();
        }

        public IList<usp_GetSettlementList_Result> BL_GetSettlementList(Settlement pObjSettlement)
        {
            return _SettlementRepository.REP_GetSettlementList(pObjSettlement);
        }

        public FuncResponse<Settlement> BL_InsertUpdateSettlement(Settlement pObjSettlement)
        {
            return _SettlementRepository.REP_InsertUpdateSettlement(pObjSettlement);
        }

        public FuncResponse BL_InsertSettlementItem(MAP_Settlement_SettlementItem pObjSettlementItem)
        {
            return _SettlementRepository.REP_InsertSettlementItem(pObjSettlementItem);
        }

        public IList<usp_GetSettlementItemList_Result> BL_GetSettlementItemList(int pIntSettlementID)
        {
            return _SettlementRepository.REP_GetSettlementItemList(pIntSettlementID);
        }

        public usp_GetSettlementItemDetails_Result BL_GetSettlementItemDetails(long plngSIID)
        {
            return _SettlementRepository.REP_GetSettlementItemDetails(plngSIID);
        }

        public usp_GetSettlementList_Result BL_GetSettlementDetails(Settlement pObjSettlement)
        {
            return _SettlementRepository.REP_GetSettlementDetails(pObjSettlement);
        }

        public IList<usp_GetTaxPayerPayment_Result> BL_GetTaxPayerPayment(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            return _SettlementRepository.REP_GetTaxPayerPayment(pIntTaxPayerID, pIntTaxPayerTypeID);
        }

        public IList<usp_GetSettleTransactionList_Result> BL_GetSettleTransactionList(Settlement pObjSettlement)
        {
            return _SettlementRepository.REP_GetSettleTransactionList(pObjSettlement);
        }

        public IDictionary<string, object> BL_SearchSettlement(Settlement pObjSettlement)
        {
            return _SettlementRepository.REP_SearchSettlement(pObjSettlement);
        }

        public IList<usp_GetPAYEPayment_Result> BL_GetPAYEPayment(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            return _SettlementRepository.REP_GetPAYEPayment(pIntTaxPayerID, pIntTaxPayerTypeID);
        }

    }
}
