using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class ScreenRepository : IScreenRepository
    {
        ERASEntities _db;

        public FuncResponse REP_CheckScreenAuthorization(MST_Screen pObjScreen)
        {
            using (_db = new ERASEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();

                var vResult = (from uscn in _db.MAP_User_Screen
                               join scn in _db.MST_Screen on uscn.ScreenID equals scn.ScreenID
                               where scn.ControllerName.ToLower().Equals(pObjScreen.ControllerName.ToLower())
                               && scn.ActionName.ToLower().Equals(pObjScreen.ActionName.ToLower())
                               && uscn.UserID == pObjScreen.UserID && scn.Active == true && uscn.Active == true
                               select uscn);

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

        public MST_Screen REP_GetScreenUserBased(MST_Screen pObjScreen)
        {
            using (_db = new ERASEntities())
            {
                var vResult = (from uscn in _db.MAP_User_Screen
                               join scn in _db.MST_Screen on uscn.ScreenID equals scn.ScreenID
                               where scn.ControllerName.ToLower().Equals(pObjScreen.ControllerName.ToLower())
                               && scn.ActionName.ToLower().Equals(pObjScreen.ActionName.ToLower())
                               && uscn.UserID == pObjScreen.UserID && scn.Active == true && uscn.Active == true
                               select scn);

                return vResult.Count() > 0 ? vResult.FirstOrDefault() : null;
            }
        }

        public IList<usp_GetScreenList_Result> REP_GetScreenList(MST_Screen pObjScreen)
        {
            using (_db = new ERASEntities())
            {
                return _db.usp_GetScreenList(pObjScreen.ScreenName, pObjScreen.ScreenID, pObjScreen.intStatus).ToList();
            }
        }

        public usp_GetScreenList_Result REP_GetScreenDetails(MST_Screen pObjScreen)
        {
            using (_db = new ERASEntities())
            {
                return _db.usp_GetScreenList(pObjScreen.ScreenName, pObjScreen.ScreenID, pObjScreen.intStatus).SingleOrDefault();
            }
        }

        public IList<usp_GetScreenUserList_Result> REP_GetScreenUserList(MST_Screen pObjScreen)
        {
            using (_db = new ERASEntities())
            {
                return _db.usp_GetScreenUserList(pObjScreen.ScreenID, pObjScreen.UserID).ToList();
            }
        }

        public IList<usp_GetScreenMenuList_Result> REP_GetScreenMenuList(MST_Screen pObjScreen)
        {
            using (_db = new ERASEntities())
            {
                return _db.usp_GetScreenMenuList(pObjScreen.ScreenID, pObjScreen.CentralMenuID).ToList();
            }
        }

        public FuncResponse REP_InsertUserScreen(MAP_User_Screen pObjUserScreen)
        {
            using (_db = new ERASEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vExists = (from uscn in _db.MAP_User_Screen
                               where uscn.UserID == pObjUserScreen.UserID && uscn.ScreenID == pObjUserScreen.ScreenID
                               select uscn);

                if (vExists.Count() > 0)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Already Exists";
                    return mObjResponse;
                }

                _db.MAP_User_Screen.Add(pObjUserScreen);

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = Ex.Message;
                }

                return mObjResponse;
            }
        }

        public FuncResponse<IList<usp_GetScreenUserList_Result>> REP_RemoveUserScreen(MAP_User_Screen pObjUserScreen)
        {
            using (_db = new ERASEntities())
            {
                FuncResponse<IList<usp_GetScreenUserList_Result>> mObjFuncResponse = new FuncResponse<IList<usp_GetScreenUserList_Result>>(); //Return Object

                MAP_User_Screen mObjDeleteScreen;

                mObjDeleteScreen = _db.MAP_User_Screen.Find(pObjUserScreen.USID);

                if (mObjDeleteScreen == null)
                {
                    mObjFuncResponse.Success = false;
                    if (pObjUserScreen.UserID > 0)
                        mObjFuncResponse.Message = "Screen Already Removed";
                    else
                        mObjFuncResponse.Message = "User Already Removed";
                }
                else
                {
                    _db.MAP_User_Screen.Remove(mObjDeleteScreen);


                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                        if (pObjUserScreen.UserID > 0)
                            mObjFuncResponse.Message = "Screen Removed Successfully";
                        else
                            mObjFuncResponse.Message = "User Removed Successfully";
                        mObjFuncResponse.AdditionalData = _db.usp_GetScreenUserList(pObjUserScreen.ScreenID, pObjUserScreen.UserID).ToList();
                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = Ex.Message;
                    }
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_InsertCentralMenuScreen(MAP_CentralMenu_Screen pObjCentralMenuScreen)
        {
            using (_db = new ERASEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vExists = (from uscn in _db.MAP_CentralMenu_Screen
                               where uscn.CentralMenuID == pObjCentralMenuScreen.CentralMenuID && uscn.ScreenID == pObjCentralMenuScreen.ScreenID
                               select uscn);

                if (vExists.Count() > 0)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Already Exists";
                    return mObjResponse;
                }

                _db.MAP_CentralMenu_Screen.Add(pObjCentralMenuScreen);

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = Ex.Message;
                }

                return mObjResponse;
            }
        }

        public FuncResponse<IList<usp_GetScreenMenuList_Result>> REP_RemoveMenuScreen(MAP_CentralMenu_Screen pObjMenuScreen)
        {
            using (_db = new ERASEntities())
            {
                FuncResponse<IList<usp_GetScreenMenuList_Result>> mObjFuncResponse = new FuncResponse<IList<usp_GetScreenMenuList_Result>>(); //Return Object

                MAP_CentralMenu_Screen mObjDeleteScreen;

                mObjDeleteScreen = _db.MAP_CentralMenu_Screen.Find(pObjMenuScreen.CMSID);

                if (mObjDeleteScreen == null)
                {
                    mObjFuncResponse.Success = false;
                    if (pObjMenuScreen.CentralMenuID > 0)
                        mObjFuncResponse.Message = "Screen Already Removed";
                    else
                        mObjFuncResponse.Message = "Menu Already Removed";
                }
                else
                {
                    _db.MAP_CentralMenu_Screen.Remove(mObjDeleteScreen);


                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                        if (pObjMenuScreen.CentralMenuID > 0)
                            mObjFuncResponse.Message = "Screen Removed Successfully";
                        else
                            mObjFuncResponse.Message = "Menu Removed Successfully";
                        mObjFuncResponse.AdditionalData = _db.usp_GetScreenMenuList(pObjMenuScreen.ScreenID, pObjMenuScreen.CentralMenuID).ToList();
                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = Ex.Message;
                    }
                }

                return mObjFuncResponse;
            }
        }

    }
}
