//using System;
//using EIRS.BOL;
//using EIRS.Common;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace EIRS.Repository
//{
//    public class UserRepository : IUserRepository
//    {
//        ERASEntities _db;
//        EIRSEntities _EIRSdb;

//        public FuncResponse REP_InsertUpdateUser(MST_Users pObjUser)
//        {
//            using (_db = new ERASEntities())
//            {
//                MST_Users mObjInsertUpdateUser; //Business Operation Insert Update Object
//                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

//                //Check if Duplicate
//                var vDuplicateCheck = (from usr in _db.MST_Users
//                                       where usr.UserName == pObjUser.UserName && usr.UserID != pObjUser.UserID
//                                       select usr);

//                if (vDuplicateCheck.Count() > 0)
//                {
//                    mObjFuncResponse.Success = false;
//                    mObjFuncResponse.Message = "UserName already exists";
//                    return mObjFuncResponse;
//                }

//                //If Update Load Business Operation
//                if (pObjUser.UserID != 0)
//                {
//                    mObjInsertUpdateUser = (from usr in _db.MST_Users
//                                            where usr.UserID == pObjUser.UserID
//                                            select usr).FirstOrDefault();

//                    if (mObjInsertUpdateUser != null)
//                    {
//                        mObjInsertUpdateUser.ModifiedBy = pObjUser.ModifiedBy;
//                        mObjInsertUpdateUser.ModifiedDate = pObjUser.ModifiedDate;
//                    }
//                    else
//                    {
//                        mObjInsertUpdateUser = new MST_Users();
//                        mObjInsertUpdateUser.CreatedBy = pObjUser.CreatedBy;
//                        mObjInsertUpdateUser.CreatedDate = pObjUser.CreatedDate;
//                    }
//                }
//                else // Else Insert Business Operation
//                {
//                    mObjInsertUpdateUser = new MST_Users();
//                    mObjInsertUpdateUser.CreatedBy = pObjUser.CreatedBy;
//                    mObjInsertUpdateUser.CreatedDate = pObjUser.CreatedDate;
//                }

//                mObjInsertUpdateUser.UserTypeID = pObjUser.UserTypeID;
//                mObjInsertUpdateUser.UserName = pObjUser.UserName;
//                mObjInsertUpdateUser.Password = pObjUser.Password != null ? pObjUser.Password : mObjInsertUpdateUser.Password;
//                mObjInsertUpdateUser.ContactName = pObjUser.ContactName;
//                mObjInsertUpdateUser.EmailAddress = pObjUser.EmailAddress;
//                mObjInsertUpdateUser.ContactNumber = pObjUser.ContactNumber;
//                mObjInsertUpdateUser.IsTOManager = pObjUser.IsTOManager;
//                mObjInsertUpdateUser.IsDirector = pObjUser.IsDirector;
//                mObjInsertUpdateUser.TaxOfficeID = pObjUser.TaxOfficeID;
//                mObjInsertUpdateUser.TOManagerID = pObjUser.TOManagerID;
//                mObjInsertUpdateUser.SignaturePath = pObjUser.SignaturePath;
//                mObjInsertUpdateUser.Active = pObjUser.Active;

//                if (pObjUser.UserID == 0)
//                {
//                    _db.MST_Users.Add(mObjInsertUpdateUser);

//                    if (pObjUser.UserTypeID == 3)
//                    {
//                        //Adding All API to rights table.

//                        var vAPIList = (from api in _db.MST_API
//                                        select api);

//                        foreach (var vitem in vAPIList)
//                        {
//                            mObjInsertUpdateUser.MAP_API_Users_Rights.Add(new MAP_API_Users_Rights() { APIAccess = false, APIID = vitem.APIID, CreatedBy = pObjUser.CreatedBy, CreatedDate = pObjUser.CreatedDate });
//                        }
//                    }
//                }

//                try
//                {
//                    _db.SaveChanges();
//                    mObjFuncResponse.Success = true;
//                    if (pObjUser.UserID == 0)
//                        mObjFuncResponse.Message = "User Added Successfully";
//                    else
//                        mObjFuncResponse.Message = "User Updated Successfully";

//                }
//                catch (Exception Ex)
//                {
//                    mObjFuncResponse.Success = false;
//                    mObjFuncResponse.Exception = Ex;

//                    if (pObjUser.UserID == 0)
//                        mObjFuncResponse.Message = "User Addition Failed";
//                    else
//                        mObjFuncResponse.Message = "User Update Failed";
//                }

//                return mObjFuncResponse;
//            }
//        }

//        public IList<usp_GetUserList_Result> REP_GetUserList(MST_Users pObjUser)
//        {
//            using (_db = new ERASEntities())
//            {
//                return _db.usp_GetUserList(pObjUser.UserID, pObjUser.UserTypeID, pObjUser.intStatus,pObjUser.TOManagerID,pObjUser.TaxOfficeID).ToList();
//            }
//        }

//        public usp_GetUserList_Result REP_GetUserDetails(MST_Users pObjUser)
//        {
//            using (_db = new ERASEntities())
//            {
//                return _db.usp_GetUserList(pObjUser.UserID, pObjUser.UserTypeID, pObjUser.intStatus, pObjUser.TOManagerID, pObjUser.TaxOfficeID).FirstOrDefault();
//            }
//        }

//        public FuncResponse REP_UpdateStatus(MST_Users pObjUser)
//        {
//            using (_db = new ERASEntities())
//            {
//                MST_Users mObjInsertUpdateUser; //Business Operation Insert Update Object
//                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

//                //If Update Load User
//                if (pObjUser.UserID != 0)
//                {
//                    mObjInsertUpdateUser = (from usr in _db.MST_Users
//                                            where usr.UserID == pObjUser.UserID
//                                            select usr).FirstOrDefault();

//                    if (mObjInsertUpdateUser != null)
//                    {
//                        mObjInsertUpdateUser.Active = !mObjInsertUpdateUser.Active;
//                        mObjInsertUpdateUser.ModifiedBy = pObjUser.ModifiedBy;
//                        mObjInsertUpdateUser.ModifiedDate = pObjUser.ModifiedDate;

//                        try
//                        {
//                            _db.SaveChanges();
//                            mObjFuncResponse.Success = true;
//                            mObjFuncResponse.Message = "User Updated Successfully";
//                            mObjFuncResponse.AdditionalData = _db.usp_GetUserList(0, pObjUser.UserTypeID, pObjUser.intStatus, pObjUser.TOManagerID, pObjUser.TaxOfficeID).ToList();

//                        }
//                        catch (Exception Ex)
//                        {
//                            mObjFuncResponse.Success = false;
//                            mObjFuncResponse.Exception = Ex;
//                            mObjFuncResponse.Message = "User Update Failed";
//                        }
//                    }
//                }

//                return mObjFuncResponse;
//            }
//        }

//        public FuncResponse<MST_Users> REP_CheckUserLoginDetails(MST_Users pObjUser)
//        {
//            FuncResponse<MST_Users> mObjFuncResponse = new FuncResponse<MST_Users>();

//            try
//            {
//                using (_db = new ERASEntities())
//                {

//                    var vFind = (from usr in _db.MST_Users
//                                 where (usr.UserName.Equals(pObjUser.UserName) || usr.EmailAddress.Equals(pObjUser.UserName)) && usr.Password.Equals(pObjUser.Password) && usr.UserTypeID == pObjUser.UserTypeID
//                                 select usr);

//                    if (vFind != null && vFind.Count() == 0)
//                    {
//                        mObjFuncResponse.Success = false;
//                        mObjFuncResponse.Message = "Invalid Login Details";
//                    }
//                    else if (vFind.Count() > 0 && vFind.FirstOrDefault().Active == false)
//                    {
//                        mObjFuncResponse.Success = false;
//                        mObjFuncResponse.Message = "User Is Not Active";
//                    }
//                    else
//                    {
//                        mObjFuncResponse.Success = true;
//                        mObjFuncResponse.Message = "Login Successfully";
//                        mObjFuncResponse.AdditionalData = vFind.FirstOrDefault();
//                    }

//                    return mObjFuncResponse;
//                }
//            }
//            catch (Exception ex)
//            {
//                mObjFuncResponse.Exception = ex;
//                mObjFuncResponse.Success = false;
//                return mObjFuncResponse;
//            }
//        }

//        public FuncResponse REP_ForgotPassword(MST_Users pObjUser)
//        {
//            using (_db = new ERASEntities())
//            {
//                FuncResponse mObjFuncResponse = new FuncResponse();

//                MST_Users mObjUserDetails = (from usr in _db.MST_Users where usr.UserName.Equals(pObjUser.UserName) select usr).FirstOrDefault();

//                if (mObjUserDetails != null)
//                {
//                    mObjUserDetails.Password = EncryptDecrypt.Encrypt(CommUtil.GenerateUniqueNumber().ToString());
//                    mObjUserDetails.ModifiedDate = CommUtil.GetCurrentDateTime();
//                    _db.SaveChanges();


//                    mObjFuncResponse.Message = "A Temporary Password Has Generated";
//                    mObjFuncResponse.Success = true;
//                    mObjFuncResponse.AdditionalData = mObjUserDetails;
//                }
//                else
//                {
//                    mObjFuncResponse.Success = false;
//                    mObjFuncResponse.Message = "No User Found";
//                }
//                return mObjFuncResponse;
//            }
//        }

//        public FuncResponse REP_ChangePassword(MST_Users pObjUser)
//        {
//            using (_db = new ERASEntities())
//            {
//                FuncResponse mObjResponse = new FuncResponse();

//                var vUserDetails = (from usr in _db.MST_Users
//                                    where (usr.UserID.Equals(pObjUser.UserID) && usr.Password.Equals(pObjUser.OldPassword))
//                                    select usr).FirstOrDefault();

//                if (vUserDetails != null && vUserDetails.UserID > 0)
//                {
//                    vUserDetails.Password = pObjUser.Password;

//                    try
//                    {
//                        _db.SaveChanges();
//                        mObjResponse.Success = true;
//                        mObjResponse.Message = "Password has Changed Successfully";

//                    }
//                    catch (Exception ex)
//                    {
//                        mObjResponse.Success = false;
//                        mObjResponse.Message = "Password has not changed, Please try Again ";
//                    }

//                    return mObjResponse;

//                }
//                mObjResponse.Success = false;
//                mObjResponse.Message = "Invalid Old Password";
//                return mObjResponse;

//            }
//        }

//        public bool REP_UpdateLastLoginDetails(MST_Users pObjUser)
//        {
//            using (_db = new ERASEntities())
//            {
//                MST_Users mObjUpdateLogin = (from usr in _db.MST_Users
//                                             where usr.UserID == pObjUser.UserID
//                                             select usr).Take(1).SingleOrDefault();

//                if (mObjUpdateLogin != null)
//                {
//                    mObjUpdateLogin.LastLogin = DateTime.Now;
//                    _db.SaveChanges();
//                    return true;
//                }
//                else
//                {
//                    return false;
//                }
//            }
//        }

//        public void REP_InsertLoginToken(MST_UserToken pObjUserToken)
//        {
//            using (_db = new ERASEntities())
//            {
//                var lastToken = _db.MST_UserToken.Where(usr => usr.UserID == pObjUserToken.UserID).OrderByDescending(o=>o.CreatedDate).Take(1).FirstOrDefault();
//                if (lastToken.TokenExpiresDate < DateTime.Now)
//                {
//                    MST_UserToken mLoginToken = new MST_UserToken()
//                    {
//                        CreatedBy = pObjUserToken.CreatedBy,
//                        CreatedDate = DateTime.Now,
//                        UserID = pObjUserToken.UserID,
//                        Token = pObjUserToken.Token,
//                        TokenExpiresDate = pObjUserToken.TokenExpiresDate.Value.AddDays(1),
//                        TokenIssuedDate = pObjUserToken.TokenIssuedDate
//                    };

//                    _db.MST_UserToken.Add(mLoginToken);
//                    _db.SaveChanges();
//                }
//            }
//        }

//        public bool REP_ValidateLoginSession(MST_UserToken pObjUserToken)
//        {
//            using (_db = new ERASEntities())
//            {
//                MST_UserToken mLoginToken = (from lt in _db.MST_UserToken
//                                             where lt.UserID == pObjUserToken.UserID && lt.Token == pObjUserToken.Token
//                                             select lt).Take(1).SingleOrDefault();

//                if (mLoginToken != null)
//                {
//                    if (mLoginToken.TokenExpiresDate > DateTime.Now)
//                    {
//                        return true;
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                }
//                else
//                {
//                    return false;
//                }
//            }
//        }

//        public void REP_DestroySession(MST_UserToken pObjUserToken)
//        {
//            using (_db = new ERASEntities())
//            {
//                MST_UserToken mLoginToken = (from lt in _db.MST_UserToken
//                                             where lt.UserID == pObjUserToken.UserID && lt.Token == pObjUserToken.Token
//                                             select lt).Take(1).SingleOrDefault();


//                if (mLoginToken != null)
//                {
//                    _db.MST_UserToken.Remove(mLoginToken);
//                    _db.SaveChanges();
//                }
//            }
//        }

//        public FuncResponse REP_InsertUpdateTaxOfficerTarget(IList<MAP_TaxOfficer_Target> plstTaxOfficerTarget)
//        {
//            using (_EIRSdb = new EIRSEntities())
//            {
//                FuncResponse mObjFuncResponse = new FuncResponse();

//                MAP_TaxOfficer_Target mObjInsertUpdateTarget;

//                foreach (var item in plstTaxOfficerTarget)
//                {
//                    if (item.TOTID > 0)
//                    {
//                        mObjInsertUpdateTarget = _EIRSdb.MAP_TaxOfficer_Target.Find(item.TOTID);

//                        if (mObjInsertUpdateTarget != null)
//                        {
//                            mObjInsertUpdateTarget.ModifiedBy = item.ModifiedBy;
//                            mObjInsertUpdateTarget.ModifiedDate = item.ModifiedDate;
//                        }
//                    }
//                    else
//                    {
//                        mObjInsertUpdateTarget = new MAP_TaxOfficer_Target()
//                        {
//                            CreatedBy = item.CreatedBy,
//                            CreatedDate = item.CreatedDate
//                        };
//                    }

//                    mObjInsertUpdateTarget.RevenueStreamID = item.RevenueStreamID;
//                    mObjInsertUpdateTarget.TaxOfficeID = item.TaxOfficeID;
//                    mObjInsertUpdateTarget.TaxOfficerID = item.TaxOfficerID;
//                    mObjInsertUpdateTarget.TaxYear = item.TaxYear;
//                    mObjInsertUpdateTarget.TargetAmount = item.TargetAmount;

//                    if (item.TOTID == 0)
//                    {
//                        _EIRSdb.MAP_TaxOfficer_Target.Add(mObjInsertUpdateTarget);
//                    }
//                }

//                try
//                {
//                    _EIRSdb.SaveChanges();

//                    mObjFuncResponse.Message = "Target Set Successfully";
//                    mObjFuncResponse.Success = true;
//                }
//                catch (Exception ex)
//                {
//                    mObjFuncResponse.Exception = ex;
//                    mObjFuncResponse.Message = ex.Message;
//                    mObjFuncResponse.Success = false;
//                }

//                return mObjFuncResponse;
//            }
//        }

//        public IList<usp_GetTaxOfficerTargetList_Result> REP_GetTaxOfficerTarget(MAP_TaxOfficer_Target pObjTarget)
//        {
//            using (_EIRSdb = new EIRSEntities())
//            {
//                return _EIRSdb.usp_GetTaxOfficerTargetList(pObjTarget.TaxOfficerID,pObjTarget.TaxOfficeID, pObjTarget.TaxYear).ToList();
//            }
//        }

//        public IList<DropDownListResult> REP_GetUserDropDownList(MST_Users pObjUser)
//        {
//            using(_db = new ERASEntities())
//            {
//                var vResult = (from usrs in _db.usp_GetUserList(pObjUser.UserID, pObjUser.UserTypeID, pObjUser.intStatus, pObjUser.TOManagerID, pObjUser.TaxOfficeID).ToList()
//                               select new DropDownListResult()
//                               {
//                                   id = usrs.UserID.GetValueOrDefault(),
//                                   text = usrs.ContactName
//                               });

//                return vResult.ToList();
//            }
//        }

//        public IList<DropDownListResult> REP_GetApproverList(MST_Users pObjUser)
//        {
//            using (_db = new ERASEntities())
//            {
//                var vResult = (from usrs in _db.MST_Users
//                               where usrs.Active == true && usrs.UserTypeID == pObjUser.UserTypeID && (usrs.TaxOfficeID == pObjUser.TaxOfficeID || usrs.IsDirector == true)
//                               select new DropDownListResult()
//                               {
//                                   id = usrs.UserID,
//                                   text = usrs.ContactName
//                               });

//                return vResult.ToList();
//            }
//        }

//        public FuncResponse REP_ReplaceTaxOfficeManager(MST_Users pObjUser)
//        {
//            using(_db = new ERASEntities())
//            {
//                FuncResponse mObjFuncResponse = new FuncResponse();

//                var vStaffList = (from usr in _db.MST_Users
//                                  where usr.TOManagerID == pObjUser.TOManagerID
//                                  select usr);

//                foreach(var staff in vStaffList)
//                {
//                    staff.TOManagerID = pObjUser.ReplacementID;
//                    staff.ModifiedBy = pObjUser.ModifiedBy;
//                    staff.ModifiedDate = pObjUser.ModifiedDate;
//                }

//                try
//                {
//                    _db.SaveChanges();

//                    mObjFuncResponse.Success = true;
//                }
//                catch (Exception Ex)
//                {
//                    mObjFuncResponse.Success = false;
//                    mObjFuncResponse.Exception = Ex;
//                }

//                return mObjFuncResponse;
//            }
//        }

//        public FuncResponse REP_ReplaceTaxOfficer(MST_Users pObjUser)
//        {
//            using (_EIRSdb = new EIRSEntities())
//            {
//                FuncResponse mObjFuncResponse = new FuncResponse();

//                var vIndividualList = (from ind in _EIRSdb.Individuals
//                                       where ind.TaxOfficerID == pObjUser.UserID
//                                       select ind);

//                foreach (var ind in vIndividualList)
//                {
//                    ind.TaxOfficerID = pObjUser.ReplacementID;
//                    ind.ModifiedBy = pObjUser.ModifiedBy;
//                    ind.ModifiedDate = pObjUser.ModifiedDate;
//                }

//                var vCompanyList = (from comp in _EIRSdb.Companies
//                                       where comp.TaxOfficerID == pObjUser.UserID
//                                       select comp);

//                foreach (var comp in vCompanyList)
//                {
//                    comp.TaxOfficerID = pObjUser.ReplacementID;
//                    comp.ModifiedBy = pObjUser.ModifiedBy;
//                    comp.ModifiedDate = pObjUser.ModifiedDate;
//                }

//                var vGovernmentList = (from gov in _EIRSdb.Governments
//                                       where gov.TaxOfficerID == pObjUser.UserID
//                                       select gov);

//                foreach (var gov in vGovernmentList)
//                {
//                    gov.TaxOfficerID = pObjUser.ReplacementID;
//                    gov.ModifiedBy = pObjUser.ModifiedBy;
//                    gov.ModifiedDate = pObjUser.ModifiedDate;
//                }

//                var vSpecialList = (from sp in _EIRSdb.Specials
//                                       where sp.TaxOfficerID == pObjUser.UserID
//                                       select sp);

//                foreach (var sp in vSpecialList)
//                {
//                    sp.TaxOfficerID = pObjUser.ReplacementID;
//                    sp.ModifiedBy = pObjUser.ModifiedBy;
//                    sp.ModifiedDate = pObjUser.ModifiedDate;
//                }

//                try
//                {
//                    _EIRSdb.SaveChanges();

//                    mObjFuncResponse.Success = true;
//                }
//                catch (Exception Ex)
//                {
//                    mObjFuncResponse.Success = false;
//                    mObjFuncResponse.Exception = Ex;
//                }

//                return mObjFuncResponse;
//            }
//        }
//    }
//}

using System;
using EIRS.BOL;
using EIRS.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Repository
{
    public class UserRepository : IUserRepository
    {
        ERASEntities _db;
        EIRSEntities _EIRSdb;

        public FuncResponse REP_InsertUpdateUser(MST_Users pObjUser)
        {
            using (_db = new ERASEntities())
            {
                MST_Users mObjInsertUpdateUser; //Business Operation Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from usr in _db.MST_Users
                                       where usr.UserName == pObjUser.UserName && usr.UserID != pObjUser.UserID
                                       select usr);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "UserName already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Business Operation
                if (pObjUser.UserID != 0)
                {
                    mObjInsertUpdateUser = (from usr in _db.MST_Users
                                            where usr.UserID == pObjUser.UserID
                                            select usr).FirstOrDefault();

                    if (mObjInsertUpdateUser != null)
                    {
                        mObjInsertUpdateUser.ModifiedBy = pObjUser.ModifiedBy;
                        mObjInsertUpdateUser.ModifiedDate = pObjUser.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateUser = new MST_Users();
                        mObjInsertUpdateUser.CreatedBy = pObjUser.CreatedBy;
                        mObjInsertUpdateUser.CreatedDate = pObjUser.CreatedDate;
                    }
                }
                else // Else Insert Business Operation
                {
                    mObjInsertUpdateUser = new MST_Users();
                    mObjInsertUpdateUser.CreatedBy = pObjUser.CreatedBy;
                    mObjInsertUpdateUser.CreatedDate = pObjUser.CreatedDate;
                }

                mObjInsertUpdateUser.UserTypeID = pObjUser.UserTypeID;
                mObjInsertUpdateUser.UserName = pObjUser.UserName;
                mObjInsertUpdateUser.Password = pObjUser.Password != null ? pObjUser.Password : mObjInsertUpdateUser.Password;
                mObjInsertUpdateUser.ContactName = pObjUser.ContactName;
                mObjInsertUpdateUser.EmailAddress = pObjUser.EmailAddress;
                mObjInsertUpdateUser.ContactNumber = pObjUser.ContactNumber;
                mObjInsertUpdateUser.IsTOManager = pObjUser.IsTOManager;
                mObjInsertUpdateUser.IsDirector = pObjUser.IsDirector;
                mObjInsertUpdateUser.TaxOfficeID = pObjUser.TaxOfficeID;
                mObjInsertUpdateUser.TOManagerID = pObjUser.TOManagerID;
                mObjInsertUpdateUser.SignaturePath = pObjUser.SignaturePath;
                mObjInsertUpdateUser.Active = pObjUser.Active;

                if (pObjUser.UserID == 0)
                {
                    _db.MST_Users.Add(mObjInsertUpdateUser);

                    if (pObjUser.UserTypeID == 3)
                    {
                        //Adding All API to rights table.

                        var vAPIList = (from api in _db.MST_API
                                        select api);

                        foreach (var vitem in vAPIList)
                        {
                            mObjInsertUpdateUser.MAP_API_Users_Rights.Add(new MAP_API_Users_Rights() { APIAccess = false, APIID = vitem.APIID, CreatedBy = pObjUser.CreatedBy, CreatedDate = pObjUser.CreatedDate });
                        }
                    }
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjUser.UserID == 0)
                        mObjFuncResponse.Message = "User Added Successfully";
                    else
                        mObjFuncResponse.Message = "User Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjUser.UserID == 0)
                        mObjFuncResponse.Message = "User Addition Failed";
                    else
                        mObjFuncResponse.Message = "User Update Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetUserList_Result> REP_GetUserList(MST_Users pObjUser)
        {
            using (_db = new ERASEntities())
            {
                return _db.usp_GetUserList(pObjUser.UserID, pObjUser.UserTypeID, pObjUser.intStatus, pObjUser.TOManagerID, pObjUser.TaxOfficeID).ToList();
            }
        }

        public usp_GetUserList_Result REP_GetUserDetails(MST_Users pObjUser)
        {
            using (_db = new ERASEntities())
            {
                return _db.usp_GetUserList(pObjUser.UserID, pObjUser.UserTypeID, pObjUser.intStatus, pObjUser.TOManagerID, pObjUser.TaxOfficeID).FirstOrDefault();
            }
        }

        public FuncResponse REP_UpdateStatus(MST_Users pObjUser)
        {
            using (_db = new ERASEntities())
            {
                MST_Users mObjInsertUpdateUser; //Business Operation Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load User
                if (pObjUser.UserID != 0)
                {
                    mObjInsertUpdateUser = (from usr in _db.MST_Users
                                            where usr.UserID == pObjUser.UserID
                                            select usr).FirstOrDefault();

                    if (mObjInsertUpdateUser != null)
                    {
                        mObjInsertUpdateUser.Active = !mObjInsertUpdateUser.Active;
                        mObjInsertUpdateUser.ModifiedBy = pObjUser.ModifiedBy;
                        mObjInsertUpdateUser.ModifiedDate = pObjUser.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "User Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetUserList(0, pObjUser.UserTypeID, pObjUser.intStatus, pObjUser.TOManagerID, pObjUser.TaxOfficeID).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "User Update Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse<MST_Users> REP_CheckUserLoginDetails(MST_Users pObjUser)
        {
            FuncResponse<MST_Users> mObjFuncResponse = new FuncResponse<MST_Users>();

            try
            {
                using (_db = new ERASEntities())
                {

                    var vFind = (from usr in _db.MST_Users
                                 where (usr.UserName.Equals(pObjUser.UserName) || usr.EmailAddress.Equals(pObjUser.UserName)) && usr.Password.Equals(pObjUser.Password) && usr.UserTypeID == pObjUser.UserTypeID
                                 select usr);

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

        public FuncResponse REP_ForgotPassword(MST_Users pObjUser)
        {
            using (_db = new ERASEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();

                MST_Users mObjUserDetails = (from usr in _db.MST_Users where usr.UserName.Equals(pObjUser.UserName) select usr).FirstOrDefault();

                if (mObjUserDetails != null)
                {
                    mObjUserDetails.Password = EncryptDecrypt.Encrypt(CommUtil.GenerateUniqueNumber().ToString());
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

        public FuncResponse REP_ChangePassword(MST_Users pObjUser)
        {
            using (_db = new ERASEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vUserDetails = (from usr in _db.MST_Users
                                    where (usr.UserID.Equals(pObjUser.UserID) && usr.Password.Equals(pObjUser.OldPassword))
                                    select usr).FirstOrDefault();

                if (vUserDetails != null && vUserDetails.UserID > 0)
                {
                    vUserDetails.Password = pObjUser.Password;

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

        public bool REP_UpdateLastLoginDetails(MST_Users pObjUser)
        {
            using (_db = new ERASEntities())
            {
                MST_Users mObjUpdateLogin = (from usr in _db.MST_Users
                                             where usr.UserID == pObjUser.UserID
                                             select usr).Take(1).SingleOrDefault();

                if (mObjUpdateLogin != null)
                {
                    mObjUpdateLogin.LastLogin = DateTime.Now;
                    _db.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public void REP_InsertLoginToken(MST_UserToken pObjUserToken)
        {
            using (_db = new ERASEntities())
            {
                MST_UserToken mLoginToken = new MST_UserToken()
                {
                    CreatedBy = pObjUserToken.CreatedBy,
                    CreatedDate = DateTime.Now,
                    UserID = pObjUserToken.UserID,
                    Token = pObjUserToken.Token,
                    TokenExpiresDate = pObjUserToken.TokenExpiresDate,
                    TokenIssuedDate = pObjUserToken.TokenIssuedDate
                };

                //delete from the table the formal tokens
                //_db.MST_UserToken.re
                _db.MST_UserToken.Add(mLoginToken);
                _db.SaveChanges();
            }
        }

        public bool REP_ValidateLoginSession(MST_UserToken pObjUserToken)
        {
            using (_db = new ERASEntities())
            {
                MST_UserToken mLoginToken = (from lt in _db.MST_UserToken
                                             where lt.UserID == pObjUserToken.UserID && lt.Token == pObjUserToken.Token
                                             select lt).Take(1).SingleOrDefault();

                if (mLoginToken != null)
                {
                    if (mLoginToken.TokenExpiresDate > DateTime.Now)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public void REP_DestroySession(MST_UserToken pObjUserToken)
        {
            using (_db = new ERASEntities())
            {
                MST_UserToken mLoginToken = (from lt in _db.MST_UserToken
                                             where lt.UserID == pObjUserToken.UserID && lt.Token == pObjUserToken.Token
                                             select lt).Take(1).SingleOrDefault();


                if (mLoginToken != null)
                {
                    _db.MST_UserToken.Remove(mLoginToken);
                    _db.SaveChanges();
                }
            }
        }

        public FuncResponse REP_InsertUpdateTaxOfficerTarget(IList<MAP_TaxOfficer_Target> plstTaxOfficerTarget)
        {
            using (_EIRSdb = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();

                MAP_TaxOfficer_Target mObjInsertUpdateTarget;

                foreach (var item in plstTaxOfficerTarget)
                {
                    if (item.TOTID > 0)
                    {
                        mObjInsertUpdateTarget = _EIRSdb.MAP_TaxOfficer_Target.Find(item.TOTID);

                        if (mObjInsertUpdateTarget != null)
                        {
                            mObjInsertUpdateTarget.ModifiedBy = item.ModifiedBy;
                            mObjInsertUpdateTarget.ModifiedDate = item.ModifiedDate;
                        }
                    }
                    else
                    {
                        mObjInsertUpdateTarget = new MAP_TaxOfficer_Target()
                        {
                            CreatedBy = item.CreatedBy,
                            CreatedDate = item.CreatedDate
                        };
                    }

                    mObjInsertUpdateTarget.RevenueStreamID = item.RevenueStreamID;
                    mObjInsertUpdateTarget.TaxOfficeID = item.TaxOfficeID;
                    mObjInsertUpdateTarget.TaxOfficerID = item.TaxOfficerID;
                    mObjInsertUpdateTarget.TaxYear = item.TaxYear;
                    mObjInsertUpdateTarget.TargetAmount = item.TargetAmount;

                    if (item.TOTID == 0)
                    {
                        _EIRSdb.MAP_TaxOfficer_Target.Add(mObjInsertUpdateTarget);
                    }
                }

                try
                {
                    _EIRSdb.SaveChanges();

                    mObjFuncResponse.Message = "Target Set Successfully";
                    mObjFuncResponse.Success = true;
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Exception = ex;
                    mObjFuncResponse.Message = ex.Message;
                    mObjFuncResponse.Success = false;
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetTaxOfficerTargetList_Result> REP_GetTaxOfficerTarget(MAP_TaxOfficer_Target pObjTarget)
        {
            using (_EIRSdb = new EIRSEntities())
            {
                return _EIRSdb.usp_GetTaxOfficerTargetList(pObjTarget.TaxOfficerID, pObjTarget.TaxOfficeID, pObjTarget.TaxYear).ToList();
            }
        }

        public IList<DropDownListResult> REP_GetUserDropDownList(MST_Users pObjUser)
        {
            using (_db = new ERASEntities())
            {
                var vResult = (from usrs in _db.usp_GetUserList(pObjUser.UserID, pObjUser.UserTypeID, pObjUser.intStatus, pObjUser.TOManagerID, pObjUser.TaxOfficeID).ToList()
                               select new DropDownListResult()
                               {
                                   id = usrs.UserID.GetValueOrDefault(),
                                   text = usrs.ContactName
                               });

                return vResult.ToList();
            }
        }

        public IList<DropDownListResult> REP_GetApproverList(MST_Users pObjUser)
        {
            using (_db = new ERASEntities())
            {
                var vResult = (from usrs in _db.MST_Users
                               where usrs.Active == true && usrs.UserTypeID == pObjUser.UserTypeID && (usrs.TaxOfficeID == pObjUser.TaxOfficeID || usrs.IsDirector == true)
                               select new DropDownListResult()
                               {
                                   id = usrs.UserID,
                                   text = usrs.ContactName
                               });

                return vResult.ToList();
            }
        }
        public IList<DropDownListResult> REP_GetApproverDetList(MST_Users pObjUser, string det)
        {
            var vResult = new List<DropDownListResult>();
            using (_db = new ERASEntities())
            {
                switch (det)
                {
                    case "1":
                        vResult = (from usrs in _db.MST_Users
                                   where usrs.Active == true && usrs.UserTypeID == pObjUser.UserTypeID && usrs.TaxOfficeID == pObjUser.TaxOfficeID && usrs.IsTOManager == true
                                   select new DropDownListResult()
                                   {
                                       id = usrs.UserID,
                                       text = usrs.ContactName
                                   }).ToList();
                        break;   
                    case "2":
                        vResult = (from usrs in _db.MST_Users
                                   where usrs.Active == true && usrs.UserTypeID == pObjUser.UserTypeID && usrs.IsDirector == true
                                   select new DropDownListResult()
                                   {
                                       id = usrs.UserID,
                                       text = usrs.ContactName
                                   }).ToList();
                        break;
                    default:
                        break;
                }
                

                return vResult;
            }
        }

        public FuncResponse REP_ReplaceTaxOfficeManager(MST_Users pObjUser)
        {
            using (_db = new ERASEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();

                var vStaffList = (from usr in _db.MST_Users
                                  where usr.TOManagerID == pObjUser.TOManagerID
                                  select usr);

                foreach (var staff in vStaffList)
                {
                    staff.TOManagerID = pObjUser.ReplacementID;
                    staff.ModifiedBy = pObjUser.ModifiedBy;
                    staff.ModifiedDate = pObjUser.ModifiedDate;
                }

                try
                {
                    _db.SaveChanges();

                    mObjFuncResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_ReplaceTaxOfficer(MST_Users pObjUser)
        {
            using (_EIRSdb = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();

                var vIndividualList = (from ind in _EIRSdb.Individuals
                                       where ind.TaxOfficerID == pObjUser.UserID
                                       select ind);

                foreach (var ind in vIndividualList)
                {
                    ind.TaxOfficerID = pObjUser.ReplacementID;
                    ind.ModifiedBy = pObjUser.ModifiedBy;
                    ind.ModifiedDate = pObjUser.ModifiedDate;
                }

                var vCompanyList = (from comp in _EIRSdb.Companies
                                    where comp.TaxOfficerID == pObjUser.UserID
                                    select comp);

                foreach (var comp in vCompanyList)
                {
                    comp.TaxOfficerID = pObjUser.ReplacementID;
                    comp.ModifiedBy = pObjUser.ModifiedBy;
                    comp.ModifiedDate = pObjUser.ModifiedDate;
                }

                var vGovernmentList = (from gov in _EIRSdb.Governments
                                       where gov.TaxOfficerID == pObjUser.UserID
                                       select gov);

                foreach (var gov in vGovernmentList)
                {
                    gov.TaxOfficerID = pObjUser.ReplacementID;
                    gov.ModifiedBy = pObjUser.ModifiedBy;
                    gov.ModifiedDate = pObjUser.ModifiedDate;
                }

                var vSpecialList = (from sp in _EIRSdb.Specials
                                    where sp.TaxOfficerID == pObjUser.UserID
                                    select sp);

                foreach (var sp in vSpecialList)
                {
                    sp.TaxOfficerID = pObjUser.ReplacementID;
                    sp.ModifiedBy = pObjUser.ModifiedBy;
                    sp.ModifiedDate = pObjUser.ModifiedDate;
                }

                try
                {
                    _EIRSdb.SaveChanges();

                    mObjFuncResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;
                }

                return mObjFuncResponse;
            }
        }
    }
}

