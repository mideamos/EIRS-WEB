using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class MenuRepository : IMenuRepository
    {
        ERASEntities _db;

        public IList<usp_GetMenuList_Result> REP_GetMenuList(MST_Menu pObjMenu)
        {
            using (_db = new ERASEntities())
            {
                return _db.usp_GetMenuList(pObjMenu.MenuID, pObjMenu.ParentMenuID, pObjMenu.intStatus, pObjMenu.MenuName, pObjMenu.MenuURL).ToList();
            }
        }

        public usp_GetMenuList_Result REP_GetMenuDetails(MST_Menu pObjMenu)
        {
            using (_db = new ERASEntities())
            {
                return _db.usp_GetMenuList(pObjMenu.MenuID, pObjMenu.ParentMenuID, pObjMenu.intStatus, pObjMenu.MenuName, pObjMenu.MenuURL).SingleOrDefault();
            }
        }

        public FuncResponse REP_InsertUpdateMenu(MST_Menu pObjMenu)
        {
            using (_db = new ERASEntities())
            {
                MST_Menu mObjInsertUpdateMenu; //Menu Insert Update Object

                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from mnu in _db.MST_Menu
                                       where (mnu.MenuName == pObjMenu.MenuName || mnu.MenuURL == pObjMenu.MenuURL) && mnu.ParentMenuID == pObjMenu.ParentMenuID && mnu.MenuID != pObjMenu.MenuID
                                       select mnu);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Menu Name or Menu Url already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Menu
                if (pObjMenu.MenuID != 0)
                {
                    mObjInsertUpdateMenu = (from mnu in _db.MST_Menu
                                            where mnu.MenuID == pObjMenu.MenuID
                                            select mnu).FirstOrDefault();

                    if (mObjInsertUpdateMenu != null)
                    {
                        mObjInsertUpdateMenu.ModifiedBy = pObjMenu.ModifiedBy;
                        mObjInsertUpdateMenu.ModifiedDate = pObjMenu.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateMenu = new MST_Menu();
                        mObjInsertUpdateMenu.CreatedBy = pObjMenu.CreatedBy;
                        mObjInsertUpdateMenu.CreatedDate = pObjMenu.CreatedDate;
                    }
                }
                else // Else Insert Menu
                {
                    mObjInsertUpdateMenu = new MST_Menu();
                    mObjInsertUpdateMenu.CreatedBy = pObjMenu.CreatedBy;
                    mObjInsertUpdateMenu.CreatedDate = pObjMenu.CreatedDate;
                }

                mObjInsertUpdateMenu.MenuName = pObjMenu.MenuName;
                mObjInsertUpdateMenu.ParentMenuID = pObjMenu.ParentMenuID;
                mObjInsertUpdateMenu.MenuURL = pObjMenu.MenuURL;
                mObjInsertUpdateMenu.SortOrder = pObjMenu.SortOrder;
                mObjInsertUpdateMenu.PageTitle = pObjMenu.PageTitle;
                mObjInsertUpdateMenu.ShortDesc = pObjMenu.ShortDesc;
                mObjInsertUpdateMenu.PageContent = pObjMenu.PageContent;
                mObjInsertUpdateMenu.PageHeader = pObjMenu.PageHeader;
                mObjInsertUpdateMenu.MetaTitle = pObjMenu.MetaTitle;
                mObjInsertUpdateMenu.MetaKeywords = pObjMenu.MetaKeywords;
                mObjInsertUpdateMenu.MetaDescription = pObjMenu.MetaDescription;
                mObjInsertUpdateMenu.Active = pObjMenu.Active;

                if (pObjMenu.MenuID == 0)
                {
                    _db.MST_Menu.Add(mObjInsertUpdateMenu);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjMenu.MenuID == 0)
                        mObjFuncResponse.Message = "Menu Added Successfully";
                    else
                        mObjFuncResponse.Message = "Menu Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;
                    if (pObjMenu.MenuID == 0)
                        mObjFuncResponse.Message = "Menu Addition Failed";
                    else
                        mObjFuncResponse.Message = "Menu Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<DropDownListResult> REP_GetParentMenuList()
        {
            using (_db = new ERASEntities())
            {
                var vResult = (from mnu in _db.MST_Menu
                               where mnu.ParentMenuID == null && mnu.Active == true
                               select new DropDownListResult()
                               {
                                   id = mnu.MenuID,
                                   text = mnu.MenuName
                               });

                return vResult.ToList();
            }
        }
    }
}
