using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Repository
{
    public class AuditLogRepository : IAuditLogRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertAuditLog(Audit_Log pObjAuditLog)
        {
            using (_db = new EIRSEntities())
            {
                Audit_Log mObjInsertAuditLog = new Audit_Log();
                FuncResponse mObjFuncResponse = new FuncResponse();

                mObjInsertAuditLog.ASLID = pObjAuditLog.ASLID;
                mObjInsertAuditLog.StaffID = pObjAuditLog.StaffID;
                mObjInsertAuditLog.Comment = pObjAuditLog.Comment;
                mObjInsertAuditLog.IPAddress = pObjAuditLog.IPAddress;
                mObjInsertAuditLog.LogDate = pObjAuditLog.LogDate;


                _db.Audit_Log.Add(mObjInsertAuditLog);


                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.Message = "Audit Log Added";
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Audit Log Addition Failed";
                }

                return mObjFuncResponse;
            }
        }


        public IList<usp_GetAuditLog_Result> REP_GetAuditLog(Audit_Log pObjAuditLog)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAuditLog(pObjAuditLog.StaffID, pObjAuditLog.ASLID, pObjAuditLog.FromDate, pObjAuditLog.ToDate).ToList();
            }
        }
    }
}
