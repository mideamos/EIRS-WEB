using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLEMBank
    {
        readonly IEM_BankRepository _BankRepository;

        public BLEMBank()
        {
            _BankRepository = new EM_BankRepository();
        }

        public IList<usp_EM_GetBankList_Result> BL_GetBankList(EM_Bank pObjBank)
        {
            return _BankRepository.REP_GetBankList(pObjBank);
        }

        public FuncResponse BL_InsertUpdateBank(EM_Bank pObjBank)
        {
            return _BankRepository.REP_InsertUpdateBank(pObjBank);
        }

        public usp_EM_GetBankList_Result BL_GetBankDetails(EM_Bank pObjBank)
        {
            return _BankRepository.REP_GetBankDetails(pObjBank);
        }

        public IList<DropDownListResult> BL_GetBankDropDownList(EM_Bank pObjBank)
        {
            return _BankRepository.REP_GetBankDropDownList(pObjBank);
        }

        public FuncResponse BL_UpdateStatus(EM_Bank pObjBank)
        {
            return _BankRepository.REP_UpdateStatus(pObjBank);
        }
    }
}
