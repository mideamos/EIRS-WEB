using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.BLL
{
    public class BLSystemUser
    {
        ISystemUserRepository _SystemUserRepository;

        public BLSystemUser()
        {
            _SystemUserRepository = new SystemUserRepository();
        }

        public IList<usp_GetSystemUserList_Result> BL_GetSystemUserList(SystemUser pObjSystemUser)
        {
            return _SystemUserRepository.REP_GetSystemUserList(pObjSystemUser);

        }
        public FuncResponse BL_InsertUpdateSystemUser(SystemUser pObjSysUser)
        {
            return _SystemUserRepository.REP_InsertUpdateSystemUser(pObjSysUser);
        }
        public FuncResponse<SystemUser> BL_CheckSystemUserLoginDetails(SystemUser pObjSystemUser)
        {
            return _SystemUserRepository.REP_CheckSystemUserLoginDetails(pObjSystemUser);
        }

        public FuncResponse BL_ChangePassword(SystemUser pObjSystemUser)
        {
            return _SystemUserRepository.REP_ChangePassword(pObjSystemUser);
        }

        public usp_GetSystemUserList_Result BL_GetSystemUserDetails(SystemUser pObjSysUser)
        {
            return _SystemUserRepository.REP_GetSystemUserDetails(pObjSysUser);
        }

        public FuncResponse BL_UpdateStatus(SystemUser pobjSysUser)
        {
            return _SystemUserRepository.REP_UpdateStatus(pobjSysUser);
        }

        public void BL_UpdateLastLogin(SystemUser pObjSysUser)
        {
            _SystemUserRepository.REP_UpdateLastLogin(pObjSysUser);
        }
    }
}
