using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EIRS.Repository
{
    public class SystemUserRepository : ISystemUserRepository
    {
        EIRSEntities _db;

        public IList<usp_GetSystemUserList_Result> REP_GetSystemUserList(SystemUser pObjSysUser)
        {
            using (_db = new EIRSEntities())
            {
                var vSystemUserList = _db.usp_GetSystemUserList(pObjSysUser.SystemUserName, pObjSysUser.SystemUserID, pObjSysUser.SystemUserIds, pObjSysUser.intStatus, pObjSysUser.IncludeSystemUserIds, pObjSysUser.ExcludeSystemUserIds).ToList();
                return vSystemUserList;
            }
        }

        public FuncResponse REP_InsertUpdateSystemUser(SystemUser pObjSysUser)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();
                if (pObjSysUser != null)
                {
                    SystemUser mObjInsertUpdateUser;

                    var vIsExsited = (from user in _db.SystemUsers
                                      where (user.SystemUserName.Equals(pObjSysUser.SystemUserName) && user.SystemUserID != pObjSysUser.SystemUserID)
                                      select user).Count();
                    if (vIsExsited > 0)
                    {
                        mObjResponse.Message = "User Name Already Existed";
                        mObjResponse.Success = false;
                        return mObjResponse;
                    }
                    // update record
                    if (pObjSysUser.SystemUserID > 0)
                    {
                        mObjInsertUpdateUser = (from user in _db.SystemUsers where (user.SystemUserID == pObjSysUser.SystemUserID) select user).FirstOrDefault();

                        mObjInsertUpdateUser.SystemUserName = pObjSysUser.SystemUserName;
                        mObjInsertUpdateUser.UserLogin = pObjSysUser.UserLogin;
                        mObjInsertUpdateUser.SystemRoleID = pObjSysUser.SystemRoleID;
                        mObjInsertUpdateUser.Active = pObjSysUser.Active;
                        mObjInsertUpdateUser.ModifiedBy = pObjSysUser.ModifiedBy;
                        mObjInsertUpdateUser.ModifiedDate = pObjSysUser.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateUser = new SystemUser()
                        {
                            SystemUserName = pObjSysUser.SystemUserName,
                            UserLogin = pObjSysUser.UserLogin,
                            UserPassword = pObjSysUser.UserPassword,
                            SystemRoleID = pObjSysUser.SystemRoleID,
                            Active = pObjSysUser.Active,
                            CreatedBy = pObjSysUser.CreatedBy,
                            CreatedDate = pObjSysUser.CreatedDate

                        };
                    }
                    if (pObjSysUser.SystemUserID == 0)
                    {
                        _db.SystemUsers.Add(mObjInsertUpdateUser);
                    }
                    try
                    {
                        _db.SaveChanges();
                        mObjResponse.Success = true;
                        if (pObjSysUser.SystemUserID == 0)
                        {
                            mObjResponse.Message = "System User Created Successfully";
                        }
                        else
                        {
                            mObjResponse.Message = "System User Updated Successfully";
                        }
                    }
                    catch (Exception ex)
                    {
                        mObjResponse.Exception = ex;
                        mObjResponse.Success = false;
                        if (pObjSysUser.SystemUserID == 0)
                        {
                            mObjResponse.Message = "System User Creation Failed ";
                        }
                        else
                        {
                            mObjResponse.Message = "System User Update Failed";
                        }
                    }
                    return mObjResponse;
                }
                mObjResponse.Success = false;
                mObjResponse.Message = "Some Error Occurred, Please Try Again";
                return mObjResponse;
            }
        }

        public FuncResponse<SystemUser> REP_CheckSystemUserLoginDetails(SystemUser pObjSysUser)
        {
            FuncResponse<SystemUser> mObjFuncResponse = new FuncResponse<SystemUser>();
            try
            {
                using (_db = new EIRSEntities())
                {

                    var vFind = (from us in _db.SystemUsers
                                 where us.UserLogin.Equals(pObjSysUser.UserLogin) && us.UserPassword.Equals(pObjSysUser.UserPassword)
                                 select us);
                    if (vFind != null && vFind.Count() == 0)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = "Invalid Login Details";
                    }
                    else if (vFind.Count() > 0 && vFind.FirstOrDefault().Active == false)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = "User Is Not Active";
                    }
                    else
                    {
                        mObjFuncResponse.Success = true;
                        mObjFuncResponse.Message = "Login Successfully";
                        mObjFuncResponse.AdditionalData = vFind.FirstOrDefault();
                    }
                    return mObjFuncResponse;
                }
            }
            catch (Exception ex)
            {
                mObjFuncResponse.Exception = ex;
                mObjFuncResponse.Success = false;
                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_ForgotPassword(SystemUser pObjSystemUser)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();
                SystemUser mObjUserDetails = (from em in _db.SystemUsers where em.UserLogin.Equals(pObjSystemUser.UserLogin) select em).FirstOrDefault();
                if (mObjUserDetails != null && mObjUserDetails.UserLogin != null)
                {
                    mObjUserDetails.UserPassword = EncryptDecrypt.Encrypt(CommUtil.GenerateUniqueNumber().ToString());
                    mObjUserDetails.ModifiedDate = CommUtil.GetCurrentDateTime();
                    _db.SaveChanges();


                    mObjFuncResponse.Message = "A Temporary Password Has Generated";
                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.AdditionalData = mObjUserDetails;
                }
                else
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "No User Found";
                }
                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_ChangePassword(SystemUser pObjSystemUser)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();
                var vUserDetails = (from us in _db.SystemUsers
                                    where (us.SystemUserID.Equals(pObjSystemUser.SystemUserID) && us.UserPassword.Equals(pObjSystemUser.OldPassword))
                                    select us).FirstOrDefault();
                if (vUserDetails != null && vUserDetails.SystemUserID > 0)
                {
                    vUserDetails.UserPassword = pObjSystemUser.UserPassword;

                    try
                    {
                        _db.SaveChanges();
                        mObjResponse.Success = true;
                        mObjResponse.Message = "Password has Changed Successfully";

                    }
                    catch (Exception ex)
                    {
                        mObjResponse.Success = false;
                        mObjResponse.Message = "Password has not changed, Please try Again ";
                    }

                    return mObjResponse;

                }
                mObjResponse.Success = false;
                mObjResponse.Message = "Invalid Old Password";
                return mObjResponse;

            }
        }

        public usp_GetSystemUserList_Result REP_GetSystemUserDetails(SystemUser pObjSysUser)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetSystemUserList(pObjSysUser.SystemUserName, pObjSysUser.SystemUserID, pObjSysUser.SystemUserIds, pObjSysUser.intStatus, pObjSysUser.IncludeSystemUserIds, pObjSysUser.ExcludeSystemUserIds).FirstOrDefault();
            }
        }

        public FuncResponse REP_UpdateStatus(SystemUser pObjSysUser)
        {
            using (_db = new EIRSEntities())
            {
                SystemUser mObjInsertUpdateSysUser; //System User Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Country
                if (pObjSysUser.SystemUserID != 0)
                {
                    mObjInsertUpdateSysUser = (from sysUser in _db.SystemUsers
                                               where sysUser.SystemUserID == pObjSysUser.SystemUserID
                                               select sysUser).FirstOrDefault();

                    if (mObjInsertUpdateSysUser != null)
                    {
                        mObjInsertUpdateSysUser.Active = !mObjInsertUpdateSysUser.Active;
                        mObjInsertUpdateSysUser.ModifiedBy = pObjSysUser.ModifiedBy;
                        mObjInsertUpdateSysUser.ModifiedDate = pObjSysUser.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "System User Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetSystemUserList(pObjSysUser.SystemUserName, 0, pObjSysUser.SystemUserIds, 2, pObjSysUser.IncludeSystemUserIds, pObjSysUser.ExcludeSystemUserIds).ToList();
                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "System User Update Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public void REP_UpdateLastLogin(SystemUser pObjUser)
        {
            using (_db = new EIRSEntities())
            {
                if (pObjUser != null && pObjUser.SystemUserID > 0)
                {
                    SystemUser mObjInsertUpdateUser;

                    mObjInsertUpdateUser = _db.SystemUsers.Find(pObjUser.SystemUserID);
                    mObjInsertUpdateUser.LastLogin = pObjUser.LastLogin;
                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
        }

    }
}
