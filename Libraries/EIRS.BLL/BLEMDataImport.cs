using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLEMDataImport
    {
        readonly IEM_DataImportRepository _DataImportRepository;

        public BLEMDataImport()
        {
            _DataImportRepository = new EM_DataImportRepository();
        }

        public IList<usp_EM_GetDataSourceList_Result> BL_GetDataSourceList()
        {
            return _DataImportRepository.REP_GetDataSourceList();
        }

        public FuncResponse BL_InsertPDMainAuthorized(EM_PD_Main_Authorized pObjMainAuthorized)
        {
            return _DataImportRepository.REP_InsertPDMainAuthorized(pObjMainAuthorized);
        }

        public FuncResponse BL_InsertPDMainPending(EM_PD_Main_Pending pObjMainPending)
        {
            return _DataImportRepository.REP_InsertPDMainPending(pObjMainPending);
        }

        public FuncResponse BL_InsertPDMVAAuthorized(EM_PD_MVA_Authorized pObjMVAAuthorized)
        {
            return _DataImportRepository.REP_InsertPDMVAAuthorized(pObjMVAAuthorized);
        }

        public FuncResponse BL_InsertPDMVAPending(EM_PD_MVA_Pending pObjMVAPending)
        {
            return _DataImportRepository.REP_InsertPDMVAPending(pObjMVAPending);
        }

        public FuncResponse BL_InsertBankStatement(EM_BankStatement pObjBankStatement)
        {
            return _DataImportRepository.REP_InsertBankStatement(pObjBankStatement);
        }

        public void BL_InsertImportLog(EM_ImportLog pObjImportLog)
        {
            _DataImportRepository.REP_InsertImportLog(pObjImportLog);
        }

        public IList<vw_EM_BankStatement> BL_GetBankStatementList()
        {
            return _DataImportRepository.REP_GetBankStatementList();
        }

        public EM_BankStatement BL_GetBankStatementDetails(long plngBSID)
        {
            return _DataImportRepository.REP_GetBankStatementDetails(plngBSID);
        }

        public IList<vw_EM_PD_Main_Authorized> BL_GetPDMainAuthorizedList()
        {
            return _DataImportRepository.REP_GetPDMainAuthorizedList();
        }

        public EM_PD_Main_Authorized BL_GetPDMainAuthorizedDetails(long plngPDMAID)
        {
            return _DataImportRepository.REP_GetPDMainAuthorizedDetails(plngPDMAID);
        }

        public IList<vw_EM_PD_Main_Pending> BL_GetPDMainPendingList()
        {
            return _DataImportRepository.REP_GetPDMainPendingList();
        }

        public EM_PD_Main_Pending BL_GetPDMainPendingDetails(long plngPDMPID)
        {
            return _DataImportRepository.REP_GetPDMainPendingDetails(plngPDMPID);
        }

        public IList<vw_EM_PD_MVA_Authorized> BL_GetPDMVAAuthorizedList()
        {
            return _DataImportRepository.REP_GetPDMVAAuthorizedList();
        }

        public EM_PD_MVA_Authorized BL_GetPDMVAAuthorizedDetails(long plngPDMVAID)
        {
            return _DataImportRepository.REP_GetPDMVAAuthorizedDetails(plngPDMVAID);
        }

        public IList<vw_EM_PD_MVA_Pending> BL_GetPDMVAPendingList()
        {
            return _DataImportRepository.REP_GetPDMVAPendingList();
        }

        public EM_PD_MVA_Pending BL_GetPDMVAPendingDetails(long plngPDMVPID)
        {
            return _DataImportRepository.REP_GetPDMVAPendingDetails(plngPDMVPID);
        }
    }
}
