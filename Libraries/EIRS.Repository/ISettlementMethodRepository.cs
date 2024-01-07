using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ISettlementMethodRepository
    {
		FuncResponse REP_InsertUpdateSettlementMethod(Settlement_Method pObjSettlementMethod);
		IList<usp_GetSettlementMethodList_Result> REP_GetSettlementMethodList(Settlement_Method pObjSettlementMethod);
        usp_GetSettlementMethodList_Result REP_GetSettlementMethodDetails(Settlement_Method pObjSettlementMethod);
        IList<DropDownListResult> REP_GetSettlementMethodDropDownList(Settlement_Method pObjSettlementMethod);
        FuncResponse REP_UpdateStatus(Settlement_Method pObjSettlementMethod);
    }
}