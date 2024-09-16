using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class EM_CategoryRepository : IEM_CategoryRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateCategory(EM_Category pObjCategory)
        {
            using (_db = new EIRSEntities())
            {
                EM_Category mObjInsertUpdateCategory; //Category Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from cat in _db.EM_Category
                                       where cat.CategoryName == pObjCategory.CategoryName && cat.CategoryID != pObjCategory.CategoryID
                                       select cat);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Category already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Category
                if (pObjCategory.CategoryID != 0)
                {
                    mObjInsertUpdateCategory = (from cat in _db.EM_Category
                                                where cat.CategoryID == pObjCategory.CategoryID
                                                select cat).FirstOrDefault();

                    if (mObjInsertUpdateCategory != null)
                    {
                        mObjInsertUpdateCategory.ModifiedBy = pObjCategory.ModifiedBy;
                        mObjInsertUpdateCategory.ModifiedDate = pObjCategory.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateCategory = new EM_Category();
                        mObjInsertUpdateCategory.CreatedBy = pObjCategory.CreatedBy;
                        mObjInsertUpdateCategory.CreatedDate = pObjCategory.CreatedDate;
                    }
                }
                else // Else Insert Category
                {
                    mObjInsertUpdateCategory = new EM_Category();
                    mObjInsertUpdateCategory.CreatedBy = pObjCategory.CreatedBy;
                    mObjInsertUpdateCategory.CreatedDate = pObjCategory.CreatedDate;
                }

                mObjInsertUpdateCategory.CategoryName = pObjCategory.CategoryName;
                mObjInsertUpdateCategory.Active = pObjCategory.Active;

                if (pObjCategory.CategoryID == 0)
                {
                    _db.EM_Category.Add(mObjInsertUpdateCategory);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjCategory.CategoryID == 0)
                        mObjFuncResponse.Message = "Category Added Successfully";
                    else
                        mObjFuncResponse.Message = "Category Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjCategory.CategoryID == 0)
                        mObjFuncResponse.Message = "Category Addition Failed";
                    else
                        mObjFuncResponse.Message = "Category Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_EM_GetCategoryList_Result> REP_GetCategoryList(EM_Category pObjCategory)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_EM_GetCategoryList(pObjCategory.CategoryName, pObjCategory.CategoryID, pObjCategory.CategoryIds, pObjCategory.intStatus, pObjCategory.IncludeCategoryIds, pObjCategory.ExcludeCategoryIds).ToList();
            }
        }

        public usp_EM_GetCategoryList_Result REP_GetCategoryDetails(EM_Category pObjCategory)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_EM_GetCategoryList(pObjCategory.CategoryName, pObjCategory.CategoryID, pObjCategory.CategoryIds, pObjCategory.intStatus, pObjCategory.IncludeCategoryIds, pObjCategory.ExcludeCategoryIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetCategoryDropDownList(EM_Category pObjCategory)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from cat in _db.usp_EM_GetCategoryList(pObjCategory.CategoryName, pObjCategory.CategoryID, pObjCategory.CategoryIds, pObjCategory.intStatus, pObjCategory.IncludeCategoryIds, pObjCategory.ExcludeCategoryIds)
                               select new DropDownListResult()
                               {
                                   id = cat.CategoryID.GetValueOrDefault(),
                                   text = cat.CategoryName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(EM_Category pObjCategory)
        {
            using (_db = new EIRSEntities())
            {
                EM_Category mObjInsertUpdateCategory; //Category Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Category
                if (pObjCategory.CategoryID != 0)
                {
                    mObjInsertUpdateCategory = (from cat in _db.EM_Category
                                                where cat.CategoryID == pObjCategory.CategoryID
                                                select cat).FirstOrDefault();

                    if (mObjInsertUpdateCategory != null)
                    {
                        mObjInsertUpdateCategory.Active = !mObjInsertUpdateCategory.Active;
                        mObjInsertUpdateCategory.ModifiedBy = pObjCategory.ModifiedBy;
                        mObjInsertUpdateCategory.ModifiedDate = pObjCategory.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Category Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_EM_GetCategoryList(pObjCategory.CategoryName, 0, pObjCategory.CategoryIds, pObjCategory.intStatus, pObjCategory.IncludeCategoryIds, pObjCategory.ExcludeCategoryIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Category Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
