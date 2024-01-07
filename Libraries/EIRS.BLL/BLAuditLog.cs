using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.BLL
{
    public class BLAuditLog
    {
        IAuditLogRepository _AuditLogRepository;

        public BLAuditLog()
        {
            _AuditLogRepository = new AuditLogRepository();
        }

        public IList<usp_GetAuditLog_Result> BL_GetAuditLog(Audit_Log pObjAuditLog)
        {
            return _AuditLogRepository.REP_GetAuditLog(pObjAuditLog);
        }

        public FuncResponse BL_InsertAuditLog(Audit_Log pObjAuditLog)
        {
            return _AuditLogRepository.REP_InsertAuditLog(pObjAuditLog);
        }
    }
}
