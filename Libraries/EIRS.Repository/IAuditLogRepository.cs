using EIRS.BOL;
using EIRS.Common;
using System.Collections.Generic;

namespace EIRS.Repository
{
    public interface IAuditLogRepository
    {
        IList<usp_GetAuditLog_Result> REP_GetAuditLog(Audit_Log pObjAuditLog);
        FuncResponse REP_InsertAuditLog(Audit_Log pObjAuditLog);
    }
}