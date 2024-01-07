using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IEM_BankRepository
    {
        usp_EM_GetBankList_Result REP_GetBankDetails(EM_Bank pObjBank);
        IList<DropDownListResult> REP_GetBankDropDownList(EM_Bank pObjBank);
        IList<usp_EM_GetBankList_Result> REP_GetBankList(EM_Bank pObjBank);
        FuncResponse REP_InsertUpdateBank(EM_Bank pObjBank);
        FuncResponse REP_UpdateStatus(EM_Bank pObjBank);
    }
}