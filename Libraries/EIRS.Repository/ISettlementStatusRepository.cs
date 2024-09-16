using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ISettlementStatusRepository
    {
        usp_GetSettlementStatusList_Result REP_GetSettlementStatusDetails(Settlement_Status pObjSettlementStatus);
        IList<usp_GetSettlementStatusList_Result> REP_GetSettlementStatusList(Settlement_Status pObjSettlementStatus);
        FuncResponse REP_InsertUpdateSettlementStatus(Settlement_Status pObjSettlementStatus);
        FuncResponse REP_UpdateStatus(Settlement_Status pObjSettlementStatus);
        IList<DropDownListResult> REP_GetSettlementStatusDropDownList(Settlement_Status pObjSettlementStatus);
    }
}