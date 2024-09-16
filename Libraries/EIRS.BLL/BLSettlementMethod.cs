using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLSettlementMethod
    {
        ISettlementMethodRepository _SettlementMethodRepository;

        public BLSettlementMethod()
        {
            _SettlementMethodRepository = new SettlementMethodRepository();
        }

        public IList<usp_GetSettlementMethodList_Result> BL_GetSettlementMethodList(Settlement_Method pObjSettlementMethod)
        {
            return _SettlementMethodRepository.REP_GetSettlementMethodList(pObjSettlementMethod);
        }

        public FuncResponse BL_InsertUpdateSettlementMethod(Settlement_Method pObjSettlementMethod)
        {
            return _SettlementMethodRepository.REP_InsertUpdateSettlementMethod(pObjSettlementMethod);
        }

        public usp_GetSettlementMethodList_Result BL_GetSettlementMethodDetails(Settlement_Method pObjSettlementMethod)
        {
            return _SettlementMethodRepository.REP_GetSettlementMethodDetails(pObjSettlementMethod);
        }

        public IList<DropDownListResult> BL_GetSettlementMethodDropDownList(Settlement_Method pObjSettlementMethod)
        {
            return _SettlementMethodRepository.REP_GetSettlementMethodDropDownList(pObjSettlementMethod);
        }

        public FuncResponse BL_UpdateStatus(Settlement_Method pObjSettlementMethod)
        {
            return _SettlementMethodRepository.REP_UpdateStatus(pObjSettlementMethod);
        }
    }
}
