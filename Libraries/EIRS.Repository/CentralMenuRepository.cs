using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class CentralMenuRepository : ICentralMenuRepository
    {
        ERASEntities _db;

        public IList<usp_GetCentralMenuList_Result> REP_GetCentralMenuList(MST_CentralMenu pObjCentralMenu)
        {
            using (_db = new ERASEntities())
            {
                return _db.usp_GetCentralMenuList(pObjCentralMenu.CentralMenuID, pObjCentralMenu.ParentCentralMenuID, pObjCentralMenu.intStatus, pObjCentralMenu.MenuType, pObjCentralMenu.CentralMenuName).ToList();
            }
        }

        public usp_GetCentralMenuList_Result REP_GetCentralMenuDetails(MST_CentralMenu pObjCentralMenu)
        {
            using (_db = new ERASEntities())
            {
                return _db.usp_GetCentralMenuList(pObjCentralMenu.CentralMenuID, pObjCentralMenu.ParentCentralMenuID, pObjCentralMenu.intStatus, pObjCentralMenu.MenuType, pObjCentralMenu.CentralMenuName).SingleOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetParentCentralMenuList()
        {
            using (_db = new ERASEntities())
            {
                var vResult = (from mnu in _db.MST_CentralMenu
                               where mnu.ParentCentralMenuID == null && mnu.Active == true
                               select new DropDownListResult()
                               {
                                   id = mnu.CentralMenuID,
                                   text = mnu.CentralMenuName
                               });

                return vResult.ToList();
            }
        }

        public FuncResponse REP_InsertUpdateMenu(MST_CentralMenu pObjMenu)
        {
            using (_db = new ERASEntities())
            {
                MST_CentralMenu mObjInsertUpdateMenu; //Menu Insert Update Object

                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                ////Check if Duplicate
                //var vDuplicateCheck = (from mnu in _db.MST_CentralMenu
                //                       where (mnu.MenuName == pObjMenu.MenuName || mnu.MenuURL == pObjMenu.MenuURL) && mnu.ParentMenuID == pObjMenu.ParentMenuID && mnu.MenuID != pObjMenu.MenuID
                //                       select mnu);

                //if (vDuplicateCheck.Count() > 0)
                //{
                //    mObjFuncResponse.Success = false;
                //    mObjFuncResponse.Message = "Menu Name or Menu Url already exists";
                //    return mObjFuncResponse;
                //}

                //If Update Load Menu
                if (pObjMenu.CentralMenuID != 0)
                {
                    mObjInsertUpdateMenu = (from mnu in _db.MST_CentralMenu
                                            where mnu.CentralMenuID == pObjMenu.CentralMenuID
                                            select mnu).FirstOrDefault();

                    if (mObjInsertUpdateMenu != null)
                    {
                        mObjInsertUpdateMenu.ModifiedBy = pObjMenu.ModifiedBy;
                        mObjInsertUpdateMenu.ModifiedDate = pObjMenu.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateMenu = new MST_CentralMenu();
                        mObjInsertUpdateMenu.CreatedBy = pObjMenu.CreatedBy;
                        mObjInsertUpdateMenu.CreatedDate = pObjMenu.CreatedDate;
                    }
                }
                else // Else Insert Menu
                {
                    mObjInsertUpdateMenu = new MST_CentralMenu();
                    mObjInsertUpdateMenu.CreatedBy = pObjMenu.CreatedBy;
                    mObjInsertUpdateMenu.CreatedDate = pObjMenu.CreatedDate;
                }

                mObjInsertUpdateMenu.CentralMenuName = pObjMenu.CentralMenuName;
                mObjInsertUpdateMenu.ParentCentralMenuID = pObjMenu.ParentCentralMenuID;
                mObjInsertUpdateMenu.SortOrder = pObjMenu.SortOrder;
                mObjInsertUpdateMenu.Active = pObjMenu.Active;

                if (pObjMenu.CentralMenuID == 0)
                {
                    _db.MST_CentralMenu.Add(mObjInsertUpdateMenu);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjMenu.CentralMenuID == 0)
                        mObjFuncResponse.Message = "Menu Added Successfully";
                    else
                        mObjFuncResponse.Message = "Menu Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;
                    if (pObjMenu.CentralMenuID == 0)
                        mObjFuncResponse.Message = "Menu Addition Failed";
                    else
                        mObjFuncResponse.Message = "Menu Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetCentralMenuUserBased_Result> REP_GetCentralMenuUserBased(int pIntUserID, int pIntParentMenuID)
        {
            using(_db = new ERASEntities())
            {
                return _db.usp_GetCentralMenuUserBased(pIntUserID, pIntParentMenuID).ToList();
            }
        }
    }
}
