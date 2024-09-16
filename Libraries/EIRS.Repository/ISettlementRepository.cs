using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ISettlementRepository
    {
        usp_GetSettlementList_Result REP_GetSettlementDetails(Settlement pObjSettlement);
        IList<usp_GetSettlementList_Result> REP_GetSettlementList(Settlement pObjSettlement);
        FuncResponse<Settlement> REP_InsertUpdateSettlement(Settlement pObjSettlement);
        FuncResponse REP_InsertSettlementItem(MAP_Settlement_SettlementItem pObjSettlementItem);

        IList<usp_GetSettlementItemList_Result> REP_GetSettlementItemList(int pIntSettlementID);

        usp_GetSettlementItemDetails_Result REP_GetSettlementItemDetails(long plngSIID);

        IList<usp_GetTaxPayerPayment_Result> REP_GetTaxPayerPayment(int pIntTaxPayerID, int pIntTaxPayerTypeID);

        IList<usp_GetSettleTransactionList_Result> REP_GetSettleTransactionList(Settlement pObjSettlement);

        IDictionary<string, object> REP_SearchSettlement(Settlement pObjSettlement);
        IList<usp_GetPAYEPayment_Result> REP_GetPAYEPayment(int pIntTaxPayerID, int pIntTaxPayerTypeID);
    }
}