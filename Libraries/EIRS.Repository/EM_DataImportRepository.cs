using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IEM_DataImportRepository
    {
        EM_BankStatement REP_GetBankStatementDetails(long plngBSID);
        IList<vw_EM_BankStatement> REP_GetBankStatementList();
        IList<usp_EM_GetDataSourceList_Result> REP_GetDataSourceList();
        EM_PD_Main_Authorized REP_GetPDMainAuthorizedDetails(long plngPDMAID);
        IList<vw_EM_PD_Main_Authorized> REP_GetPDMainAuthorizedList();
        EM_PD_Main_Pending REP_GetPDMainPendingDetails(long plngPDMPID);
        IList<vw_EM_PD_Main_Pending> REP_GetPDMainPendingList();
        EM_PD_MVA_Authorized REP_GetPDMVAAuthorizedDetails(long plngPDMVAID);
        IList<vw_EM_PD_MVA_Authorized> REP_GetPDMVAAuthorizedList();
        EM_PD_MVA_Pending REP_GetPDMVAPendingDetails(long plngPDMVPID);
        IList<vw_EM_PD_MVA_Pending> REP_GetPDMVAPendingList();
        FuncResponse REP_InsertBankStatement(EM_BankStatement pObjBankStatement);
        void REP_InsertImportLog(EM_ImportLog pObjImportLog);
        FuncResponse REP_InsertPDMainAuthorized(EM_PD_Main_Authorized pObjMainAuthorized);
        FuncResponse REP_InsertPDMainPending(EM_PD_Main_Pending pObjMainPending);
        FuncResponse REP_InsertPDMVAAuthorized(EM_PD_MVA_Authorized pObjMVAAuthorized);
        FuncResponse REP_InsertPDMVAPending(EM_PD_MVA_Pending pObjMVAPending);
    }

    public class EM_DataImportRepository : IEM_DataImportRepository
    {
        EIRSEntities _db;

        public IList<usp_EM_GetDataSourceList_Result> REP_GetDataSourceList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_EM_GetDataSourceList().ToList();
            }
        }

        public FuncResponse REP_InsertPDMainAuthorized(EM_PD_Main_Authorized pObjMainAuthorized)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();

                try
                {
                    _db.EM_PD_Main_Authorized.Add(pObjMainAuthorized);
                    _db.SaveChanges();

                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.Message = "Data Added Successfully";
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = Ex.Message;
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_InsertPDMainPending(EM_PD_Main_Pending pObjMainPending)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();

                try
                {
                    _db.EM_PD_Main_Pending.Add(pObjMainPending);
                    _db.SaveChanges();

                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.Message = "Data Added Successfully";
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = Ex.Message;
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_InsertPDMVAAuthorized(EM_PD_MVA_Authorized pObjMVAAuthorized)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();

                try
                {
                    _db.EM_PD_MVA_Authorized.Add(pObjMVAAuthorized);
                    _db.SaveChanges();

                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.Message = "Data Added Successfully";
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = Ex.Message;
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_InsertPDMVAPending(EM_PD_MVA_Pending pObjMVAPending)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();

                try
                {
                    _db.EM_PD_MVA_Pending.Add(pObjMVAPending);
                    _db.SaveChanges();

                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.Message = "Data Added Successfully";
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = Ex.Message;
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_InsertBankStatement(EM_BankStatement pObjBankStatement)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();

                try
                {
                    _db.EM_BankStatement.Add(pObjBankStatement);
                    _db.SaveChanges();

                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.Message = "Data Added Successfully";
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = Ex.Message;
                }

                return mObjFuncResponse;
            }
        }

        public void REP_InsertImportLog(EM_ImportLog pObjImportLog)
        {
            using (_db = new EIRSEntities())
            {
                try
                {
                    _db.EM_ImportLog.Add(pObjImportLog);
                    _db.SaveChanges();

                }
                catch (Exception Ex)
                {
                }


            }
        }

        public IList<vw_EM_BankStatement> REP_GetBankStatementList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.vw_EM_BankStatement.ToList();
            }
        }

        public EM_BankStatement REP_GetBankStatementDetails(long plngBSID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.EM_BankStatement.Find(plngBSID);
            }
        }

        public IList<vw_EM_PD_Main_Authorized> REP_GetPDMainAuthorizedList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.vw_EM_PD_Main_Authorized.ToList();
            }
        }

        public EM_PD_Main_Authorized REP_GetPDMainAuthorizedDetails(long plngPDMAID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.EM_PD_Main_Authorized.Find(plngPDMAID);
            }
        }

        public IList<vw_EM_PD_Main_Pending> REP_GetPDMainPendingList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.vw_EM_PD_Main_Pending.ToList();
            }
        }

        public EM_PD_Main_Pending REP_GetPDMainPendingDetails(long plngPDMPID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.EM_PD_Main_Pending.Find(plngPDMPID);
            }
        }

        public IList<vw_EM_PD_MVA_Authorized> REP_GetPDMVAAuthorizedList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.vw_EM_PD_MVA_Authorized.ToList();
            }
        }

        public EM_PD_MVA_Authorized REP_GetPDMVAAuthorizedDetails(long plngPDMVAID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.EM_PD_MVA_Authorized.Find(plngPDMVAID);
            }
        }

        public IList<vw_EM_PD_MVA_Pending> REP_GetPDMVAPendingList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.vw_EM_PD_MVA_Pending.ToList();
            }
        }

        public EM_PD_MVA_Pending REP_GetPDMVAPendingDetails(long plngPDMVPID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.EM_PD_MVA_Pending.Find(plngPDMVPID);
            }
        }

    }
}
