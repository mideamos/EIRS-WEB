using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class CommonRepository : ICommonRepository
    {
        EIRSEntities _ERISdb;
        ERASEntities _ERASdb;

        public IList<DropDownListResult> REP_GetDealerTypeDropDownList()
        {
            using (_ERISdb = new EIRSEntities())
            {
                var vResult = (from dtype in _ERISdb.Dealer_Types
                               select new DropDownListResult()
                               {
                                   id = dtype.DealerTypeID,
                                   text = dtype.DealerTypeName
                               }).ToList();

                return vResult;
            }
        }

        public IList<DropDownListResult> REP_GetGenderDropDownList()
        {
            using (_ERISdb = new EIRSEntities())
            {
                var vResult = (from gend in _ERISdb.Genders
                               select new DropDownListResult()
                               {
                                   id = gend.GenderID,
                                   text = gend.GenderName
                               }).ToList();

                return vResult;
            }
        }

        public IList<DropDownListResult> REP_GetNationalityDropDownList()
        {
            using (_ERISdb = new EIRSEntities())
            {
                var vResult = (from nat in _ERISdb.Nationalities
                               select new DropDownListResult()
                               {
                                   id = nat.NationalityID,
                                   text = nat.NationalityName
                               }).ToList();

                return vResult;
            }
        }

        public IList<DropDownListResult> REP_GetMaritalStatusDropDownList()
        {
            using (_ERISdb = new EIRSEntities())
            {
                var vResult = (from mstat in _ERISdb.MaritalStatus
                               select new DropDownListResult()
                               {
                                   id = mstat.MaritalStatusID,
                                   text = mstat.MaritalStatusName
                               }).ToList();

                return vResult;
            }
        }

        public IList<DropDownListResult> REP_GetComputationDropDownList()
        {
            using (_ERISdb = new EIRSEntities())
            {
                var vResult = (from comp in _ERISdb.MST_Computation
                               select new DropDownListResult()
                               {
                                   id = comp.ComputationID,
                                   text = comp.ComputationName
                               }).ToList();

                return vResult;
            }
        }

        public IList<DropDownListResult> REP_GetRuleRunDropDownList()
        {
            using (_ERISdb = new EIRSEntities())
            {
                var vResult = (from rr in _ERISdb.MST_RuleRun
                               select new DropDownListResult()
                               {
                                   id = rr.RuleRunID,
                                   text = rr.RuleRunName
                               }).ToList();

                return vResult;
            }
        }

        public IList<DropDownListResult> REP_GetUserTypeDropDownList()
        {
            using (_ERASdb = new ERASEntities())
            {
                var vResult = (from rr in _ERASdb.MST_UserType
                               select new DropDownListResult()
                               {
                                   id = rr.UserTypeID,
                                   text = rr.UserTypeName
                               }).ToList();

                return vResult;
            }
        }

        public IList<DropDownListResult> REP_GetReviewStatusDropDownList()
        {
            using (_ERISdb = new EIRSEntities())
            {
                var vResult = (from comp in _ERISdb.Review_Status
                               where comp.Active == true
                               select new DropDownListResult()
                               {
                                   id = comp.ReviewStatusID,
                                   text = comp.ReviewStatusName
                               }).ToList();

                return vResult;
            }
        }

        public IList<DropDownListResult> REP_GetFieldTypeDropDownList()
        {
            using (_ERISdb = new EIRSEntities())
            {
                var vResult = (from ft in _ERISdb.MST_FieldType
                               select new DropDownListResult()
                               {
                                   id = ft.FieldTypeID,
                                   text = ft.FieldTypeName
                               }).ToList();

                return vResult;
            }
        }

        public IList<DropDownListResult> REP_GetProfileTypeDropDownList()
        {
            using (_ERISdb = new EIRSEntities())
            {
                var vResult = (from pt in _ERISdb.Profile_Types
                               select new DropDownListResult()
                               {
                                   id = pt.ProfileTypeID,
                                   text = pt.ProfileTypeName
                               }).ToList();

                return vResult;
            }
        }

        public IList<DropDownListResult> REP_GetTaxOfficeManagerList(int pIntTaxOfficeID = 0, bool pblnIsManager = false)
        {
            using (_ERASdb = new ERASEntities())
            {
                var vData = _ERASdb.MST_Users.Where(t => t.Active == true).AsQueryable();

                if (pIntTaxOfficeID != 0)
                {
                    vData = vData.Where(t => t.TaxOfficeID == pIntTaxOfficeID).AsQueryable();
                }

                if (pblnIsManager)
                {
                    vData = vData.Where(t => t.IsTOManager == true).AsQueryable();
                }

                var vResult = vData.Select(t => new DropDownListResult()
                {
                    id = t.UserID,
                    text = t.ContactName
                }).ToList();

                return vResult;
            }
        }

        public IList<DropDownListResult> REP_GetTaxOfficerList(int pIntTaxOfficeID = 0)
        {
            using (_ERASdb = new ERASEntities())
            {
                var vData = _ERASdb.MST_Users.Where(t => t.Active == true  && t.UserTypeID == 2).AsQueryable();

                if (pIntTaxOfficeID != 0)
                {
                    vData = vData.Where(t => t.TaxOfficeID == pIntTaxOfficeID).AsQueryable();
                }

                var vResult = vData.Select(t => new DropDownListResult()
                {
                    id = t.UserID,
                    text = t.ContactName
                }).ToList();

                return vResult;
            }
        }



        public IList<DropDownListResult> REP_GetSystemRoleDropDownList()
        {
            using (_ERISdb = new EIRSEntities())
            {
                var vResult = (from rol in _ERISdb.SystemRoles
                               select new DropDownListResult()
                               {
                                   id = rol.SystemRoleID,
                                   text = rol.SystemRoleName
                               }).ToList();

                return vResult;
            }
        }

        public IList<DropDownListResult> REP_GetALScreenDropDownList()
        {
            using (_ERISdb = new EIRSEntities())
            {
                var vResult = (from scrn in _ERISdb.AL_Screen
                               select new DropDownListResult()
                               {
                                   id = scrn.ASLID,
                                   text = scrn.ASLName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_InsertPayDirectNotification(PayDirect_Notifications pObjPayDirectNotification)
        {
            using (_ERISdb = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                PayDirect_Notifications mObjInsertPayDirectNotifications = new PayDirect_Notifications()
                {
                    RequestParameter = pObjPayDirectNotification.RequestParameter,
                    CreatedBy = pObjPayDirectNotification.CreatedBy,
                    CreatedDate = pObjPayDirectNotification.CreatedDate
                };

                _ERISdb.PayDirect_Notifications.Add(pObjPayDirectNotification);

                try
                {
                    _ERISdb.SaveChanges();
                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.Message = "Paydirect Notification Added Successfully";
                }
                catch (System.Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;
                    mObjFuncResponse.Message = "Paydirect Notification Addition Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<DropDownListResult> REP_GetTCCStatusDropDownList()
        {
            using (_ERISdb = new EIRSEntities())
            {
                var vResult = (from comp in _ERISdb.MST_TCCRequestStatus
                               where comp.Active == true
                               select new DropDownListResult()
                               {
                                   id = comp.StatusID,
                                   text = comp.StatusName
                               }).ToList();

                return vResult;
            }
        }
    }
}
