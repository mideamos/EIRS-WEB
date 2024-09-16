using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class BusinessCategoryRepository : IBusinessCategoryRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateBusinessCategory(Business_Category pObjBusinessCategory)
        {
            using (_db = new EIRSEntities())
            {
                Business_Category mObjInsertUpdateBusinessCategory; //Business Category Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from bcat in _db.Business_Category
                                       where bcat.BusinessCategoryName == pObjBusinessCategory.BusinessCategoryName && bcat.BusinessTypeID == pObjBusinessCategory.BusinessTypeID && bcat.BusinessCategoryID != pObjBusinessCategory.BusinessCategoryID
                                       select bcat);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Business Category already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Business Category
                if (pObjBusinessCategory.BusinessCategoryID != 0)
                {
                    mObjInsertUpdateBusinessCategory = (from bcat in _db.Business_Category
                                                 where bcat.BusinessCategoryID == pObjBusinessCategory.BusinessCategoryID
                                                 select bcat).FirstOrDefault();

                    if (mObjInsertUpdateBusinessCategory != null)
                    {
                        mObjInsertUpdateBusinessCategory.ModifiedBy = pObjBusinessCategory.ModifiedBy;
                        mObjInsertUpdateBusinessCategory.ModifiedDate = pObjBusinessCategory.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateBusinessCategory = new Business_Category();
                        mObjInsertUpdateBusinessCategory.CreatedBy = pObjBusinessCategory.CreatedBy;
                        mObjInsertUpdateBusinessCategory.CreatedDate = pObjBusinessCategory.CreatedDate;
                    }
                }
                else // Else Insert Business Category
                {
                    mObjInsertUpdateBusinessCategory = new Business_Category();
                    mObjInsertUpdateBusinessCategory.CreatedBy = pObjBusinessCategory.CreatedBy;
                    mObjInsertUpdateBusinessCategory.CreatedDate = pObjBusinessCategory.CreatedDate;
                }

                mObjInsertUpdateBusinessCategory.BusinessCategoryName = pObjBusinessCategory.BusinessCategoryName;
                mObjInsertUpdateBusinessCategory.BusinessTypeID = pObjBusinessCategory.BusinessTypeID;
                mObjInsertUpdateBusinessCategory.Active = pObjBusinessCategory.Active;

                if (pObjBusinessCategory.BusinessCategoryID == 0)
                {
                    _db.Business_Category.Add(mObjInsertUpdateBusinessCategory);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjBusinessCategory.BusinessCategoryID == 0)
                        mObjFuncResponse.Message = "Business Category Added Successfully";
                    else
                        mObjFuncResponse.Message = "Business Category Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjBusinessCategory.BusinessCategoryID == 0)
                        mObjFuncResponse.Message = "Business Category Addition Failed";
                    else
                        mObjFuncResponse.Message = "Business Category Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetBusinessCategoryList_Result> REP_GetBusinessCategoryList(Business_Category pObjBusinessCategory)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBusinessCategoryList(pObjBusinessCategory.BusinessCategoryName, pObjBusinessCategory.BusinessCategoryID, pObjBusinessCategory.BusinessTypeID, pObjBusinessCategory.BusinessCategoryIds, pObjBusinessCategory.intStatus, pObjBusinessCategory.IncludeBusinessCategoryIds, pObjBusinessCategory.ExcludeBusinessCategoryIds).ToList();
            }
        }

        public usp_GetBusinessCategoryList_Result REP_GetBusinessCategoryDetails(Business_Category pObjBusinessCategory)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBusinessCategoryList(pObjBusinessCategory.BusinessCategoryName, pObjBusinessCategory.BusinessCategoryID, pObjBusinessCategory.BusinessTypeID, pObjBusinessCategory.BusinessCategoryIds, pObjBusinessCategory.intStatus, pObjBusinessCategory.IncludeBusinessCategoryIds, pObjBusinessCategory.ExcludeBusinessCategoryIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetBusinessCategoryDropDownList(Business_Category pObjBusinessCategory)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = from pro in _db.Business_Category
                              where pro.BusinessTypeID == pObjBusinessCategory.BusinessTypeID
                              select new DropDownListResult() { id = pro.BusinessCategoryID, text = pro.BusinessCategoryName };

                return vResult.ToList();
                //var vResult = (from bcat in _db.usp_GetBusinessCategoryList(pObjBusinessCategory.BusinessCategoryName, pObjBusinessCategory.BusinessCategoryID, pObjBusinessCategory.BusinessTypeID, pObjBusinessCategory.BusinessCategoryIds, pObjBusinessCategory.intStatus, pObjBusinessCategory.IncludeBusinessCategoryIds, pObjBusinessCategory.ExcludeBusinessCategoryIds)
                //               select new DropDownListResult()
                //               {
                //                   id = bcat.BusinessCategoryID.GetValueOrDefault(),
                //                   text = bcat.BusinessCategoryName
                //               }).ToList();

                //return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Business_Category pObjBusinessCategory)
        {
            using (_db = new EIRSEntities())
            {
                Business_Category mObjInsertUpdateBusinessCategory; //Business Category Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load BusinessCategory
                if (pObjBusinessCategory.BusinessCategoryID != 0)
                {
                    mObjInsertUpdateBusinessCategory = (from bcat in _db.Business_Category
                                                 where bcat.BusinessCategoryID == pObjBusinessCategory.BusinessCategoryID
                                                 select bcat).FirstOrDefault();

                    if (mObjInsertUpdateBusinessCategory != null)
                    {
                        mObjInsertUpdateBusinessCategory.Active = !mObjInsertUpdateBusinessCategory.Active;
                        mObjInsertUpdateBusinessCategory.ModifiedBy = pObjBusinessCategory.ModifiedBy;
                        mObjInsertUpdateBusinessCategory.ModifiedDate = pObjBusinessCategory.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Business Category Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetBusinessCategoryList(pObjBusinessCategory.BusinessCategoryName, 0,pObjBusinessCategory.BusinessTypeID, pObjBusinessCategory.BusinessCategoryIds, pObjBusinessCategory.intStatus, pObjBusinessCategory.IncludeBusinessCategoryIds, pObjBusinessCategory.ExcludeBusinessCategoryIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Business Category Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
