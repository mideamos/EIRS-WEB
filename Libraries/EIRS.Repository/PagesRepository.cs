using System;
using System.Linq;
using EIRS.BOL;
using System.Collections.Generic;
using EIRS.Common;

namespace EIRS.Repository
{
    public class PagesRepository : IPagesRepository
    {
        ERASEntities _db = null;

        public IList<usp_GetPageList_Result> REP_SearchPages(MST_Pages pObjPage)
        {
            using (_db = new ERASEntities())
            {
                return _db.usp_GetPageList(pObjPage.PageID).ToList();
            }
        }

        public MST_Pages REP_GetPageDetails(MST_Pages pObjPage)
        {
            using (_db = new ERASEntities())
            {
                var vResult = (from page in _db.MST_Pages
                               where page.PageID == pObjPage.PageID
                               select page).FirstOrDefault();

                return vResult;
            }
        }

        public usp_GetPageList_Result REP_GetPageDetails(int mIntPageID)
        {
            using (_db = new ERASEntities())
            {
                return _db.usp_GetPageList(mIntPageID).SingleOrDefault();
            }
        }

        public FuncResponse REP_UpdatePages(MST_Pages pObjPage)
        {
            using (_db = new ERASEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();

                MST_Pages mObjUpdatePage = (from c in _db.MST_Pages
                                            where c.PageID == pObjPage.PageID
                                            select c).SingleOrDefault();

                if (mObjUpdatePage != null)
                {
                    mObjUpdatePage.ModifiedBy = pObjPage.ModifiedBy;
                    mObjUpdatePage.ModifiedDate = pObjPage.ModifiedDate;
                    mObjUpdatePage.PageHeader = pObjPage.PageHeader;
                    mObjUpdatePage.ShortDesc = pObjPage.ShortDesc;
                    mObjUpdatePage.PageContent = pObjPage.PageContent;
                    mObjUpdatePage.PageTitle = pObjPage.PageTitle;
                    mObjUpdatePage.MetaTitle = pObjPage.MetaTitle;
                    mObjUpdatePage.MetaKeywords = pObjPage.MetaKeywords;
                    mObjUpdatePage.MetaDescription = pObjPage.MetaDescription;

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                        mObjFuncResponse.Message = "Page Updated Successfully";

                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Exception = Ex;
                        mObjFuncResponse.Message = "Page Updation Failed";
                    }
                }
                else
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Page Not Found";
                }

                return mObjFuncResponse;

            }

        }
    }
}
