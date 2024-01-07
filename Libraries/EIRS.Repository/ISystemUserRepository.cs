using EIRS.BOL;
using EIRS.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EIRS.Repository
{
    public interface ISystemUserRepository
    {
        IList<usp_GetSystemUserList_Result> REP_GetSystemUserList(SystemUser pObjSysUser);

        FuncResponse REP_InsertUpdateSystemUser(SystemUser pObjSysUser);

        FuncResponse<SystemUser> REP_CheckSystemUserLoginDetails(SystemUser pObj_Sys_User);

        FuncResponse REP_ForgotPassword(SystemUser pObjSystemUser);

        FuncResponse REP_ChangePassword(SystemUser pObjSystemUser);

        usp_GetSystemUserList_Result REP_GetSystemUserDetails(SystemUser pObjSysUser);
        FuncResponse REP_UpdateStatus(SystemUser pObjSysUser);
        void REP_UpdateLastLogin(SystemUser pObjUser);
    }
}