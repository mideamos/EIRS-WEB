using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class APIRepository : IAPIRepository
    {
        ERASEntities _db;

        public IList<usp_GetAPIList_Result> REP_GetAPIList(MST_API pObjAPI)
        {
            using (_db = new ERASEntities())
            {
                return _db.usp_GetAPIList(pObjAPI.APIName, pObjAPI.APIID, pObjAPI.intStatus).ToList();
            }
        }

        public usp_GetAPIList_Result REP_GetAPIDetails(MST_API pObjAPI)
        {
            using (_db = new ERASEntities())
            {
                return _db.usp_GetAPIList(pObjAPI.APIName, pObjAPI.APIID, pObjAPI.intStatus).SingleOrDefault();
            }
        }

        public FuncResponse REP_UpdateAPI(MST_API pObjAPI)
        {
            using (_db = new ERASEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                MST_API mObjUpdateAPI = (from mnu in _db.MST_API
                                         where mnu.APIID == pObjAPI.APIID
                                         select mnu).FirstOrDefault();

                if (mObjUpdateAPI != null)
                {
                    mObjUpdateAPI.ModifiedBy = pObjAPI.ModifiedBy;
                    mObjUpdateAPI.ModifiedDate = pObjAPI.ModifiedDate;

                    mObjUpdateAPI.APIName = pObjAPI.APIName;
                    mObjUpdateAPI.APIDescription = pObjAPI.APIDescription;
                    mObjUpdateAPI.DocumentPath = pObjAPI.DocumentPath;
                    mObjUpdateAPI.Active = pObjAPI.Active;

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                        mObjFuncResponse.Message = "API Updated Successfully";

                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Exception = Ex;
                        mObjFuncResponse.Message = "API Updation Failed";
                    }

                }
                else
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "API Not Found";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetAPIUserRightList_Result> REP_GetAPIAccessList(int pIntUserID, int pIntAPIID)
        {
            using (_db = new ERASEntities())
            {
                return _db.usp_GetAPIUserRightList(pIntUserID, pIntAPIID).ToList();
            }
        }

        public FuncResponse REP_UpdateAPIAccess(MAP_API_Users_Rights pObjAPIAccess)
        {
            using (_db = new ERASEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object



                MAP_API_Users_Rights mObjUpdateAPIAccess = (from mnu in _db.MAP_API_Users_Rights
                                         where mnu.UAID == pObjAPIAccess.UAID
                                         select mnu).FirstOrDefault();

                if (mObjUpdateAPIAccess != null)
                {
                    mObjUpdateAPIAccess.ModifiedBy = pObjAPIAccess.ModifiedBy;
                    mObjUpdateAPIAccess.ModifiedDate = pObjAPIAccess.ModifiedDate;

                    mObjUpdateAPIAccess.APIAccess = pObjAPIAccess.APIAccess;

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                        mObjFuncResponse.Message = "API Access Updated Successfully";

                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Exception = Ex;
                        mObjFuncResponse.Message = "API Access Updation Failed";
                    }

                }
                else
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "API Access Not Found";
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_CheckAPIAuthorization(MST_API pObjAPI)
        {
            using (_db = new ERASEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();

                var vResult = (from uapi in _db.MAP_API_Users_Rights
                               join api in _db.MST_API on uapi.APIID equals api.APIID
                               where api.ControllerName.ToLower().Equals(pObjAPI.ControllerName.ToLower())
                               && api.ActionName.ToLower().Equals(pObjAPI.ActionName.ToLower())
                               && uapi.UserID == pObjAPI.UserID && api.Active == true
                               && uapi.APIAccess == true
                               select uapi);

                if (vResult.Count() > 0)
                {
                    mObjFuncResponse.Success = true;
                }
                else
                {
                    mObjFuncResponse.Success = false;
                }

                return mObjFuncResponse;
            }
        }
    }
}
