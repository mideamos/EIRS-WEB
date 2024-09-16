using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class AssessmentItemCategoryRepository : IAssessmentItemCategoryRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateAssessmentItemCategory(Assessment_Item_Category pObjAssessmentItemCategory)
        {
            using (_db = new EIRSEntities())
            {
                Assessment_Item_Category mObjInsertUpdateAssessmentItemCategory; //Assessment Item Category Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from aicat in _db.Assessment_Item_Category
                                       where aicat.AssessmentItemCategoryName == pObjAssessmentItemCategory.AssessmentItemCategoryName && aicat.AssessmentItemCategoryID != pObjAssessmentItemCategory.AssessmentItemCategoryID
                                       select aicat);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Assessment Item Category already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Assessment Item Category
                if (pObjAssessmentItemCategory.AssessmentItemCategoryID != 0)
                {
                    mObjInsertUpdateAssessmentItemCategory = (from aicat in _db.Assessment_Item_Category
                                                 where aicat.AssessmentItemCategoryID == pObjAssessmentItemCategory.AssessmentItemCategoryID
                                                 select aicat).FirstOrDefault();

                    if (mObjInsertUpdateAssessmentItemCategory != null)
                    {
                        mObjInsertUpdateAssessmentItemCategory.ModifiedBy = pObjAssessmentItemCategory.ModifiedBy;
                        mObjInsertUpdateAssessmentItemCategory.ModifiedDate = pObjAssessmentItemCategory.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateAssessmentItemCategory = new Assessment_Item_Category();
                        mObjInsertUpdateAssessmentItemCategory.CreatedBy = pObjAssessmentItemCategory.CreatedBy;
                        mObjInsertUpdateAssessmentItemCategory.CreatedDate = pObjAssessmentItemCategory.CreatedDate;
                    }
                }
                else // Else Insert Assessment Item Category
                {
                    mObjInsertUpdateAssessmentItemCategory = new Assessment_Item_Category();
                    mObjInsertUpdateAssessmentItemCategory.CreatedBy = pObjAssessmentItemCategory.CreatedBy;
                    mObjInsertUpdateAssessmentItemCategory.CreatedDate = pObjAssessmentItemCategory.CreatedDate;
                }

                mObjInsertUpdateAssessmentItemCategory.AssessmentItemCategoryName = pObjAssessmentItemCategory.AssessmentItemCategoryName;
                mObjInsertUpdateAssessmentItemCategory.Active = pObjAssessmentItemCategory.Active;

                if (pObjAssessmentItemCategory.AssessmentItemCategoryID == 0)
                {
                    _db.Assessment_Item_Category.Add(mObjInsertUpdateAssessmentItemCategory);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjAssessmentItemCategory.AssessmentItemCategoryID == 0)
                        mObjFuncResponse.Message = "Assessment Item Category Added Successfully";
                    else
                        mObjFuncResponse.Message = "Assessment Item Category Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjAssessmentItemCategory.AssessmentItemCategoryID == 0)
                        mObjFuncResponse.Message = "Assessment Item Category Addition Failed";
                    else
                        mObjFuncResponse.Message = "Assessment Item Category Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetAssessmentItemCategoryList_Result> REP_GetAssessmentItemCategoryList(Assessment_Item_Category pObjAssessmentItemCategory)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentItemCategoryList(pObjAssessmentItemCategory.AssessmentItemCategoryName, pObjAssessmentItemCategory.AssessmentItemCategoryID, pObjAssessmentItemCategory.AssessmentItemCategoryIds, pObjAssessmentItemCategory.intStatus, pObjAssessmentItemCategory.IncludeAssessmentItemCategoryIds, pObjAssessmentItemCategory.ExcludeAssessmentItemCategoryIds).ToList();
            }
        }

        public usp_GetAssessmentItemCategoryList_Result REP_GetAssessmentItemCategoryDetails(Assessment_Item_Category pObjAssessmentItemCategory)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentItemCategoryList(pObjAssessmentItemCategory.AssessmentItemCategoryName, pObjAssessmentItemCategory.AssessmentItemCategoryID, pObjAssessmentItemCategory.AssessmentItemCategoryIds, pObjAssessmentItemCategory.intStatus, pObjAssessmentItemCategory.IncludeAssessmentItemCategoryIds, pObjAssessmentItemCategory.ExcludeAssessmentItemCategoryIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetAssessmentItemCategoryDropDownList(Assessment_Item_Category pObjAssessmentItemCategory)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from aicat in _db.usp_GetAssessmentItemCategoryList(pObjAssessmentItemCategory.AssessmentItemCategoryName, pObjAssessmentItemCategory.AssessmentItemCategoryID, pObjAssessmentItemCategory.AssessmentItemCategoryIds, pObjAssessmentItemCategory.intStatus, pObjAssessmentItemCategory.IncludeAssessmentItemCategoryIds, pObjAssessmentItemCategory.ExcludeAssessmentItemCategoryIds)
                               select new DropDownListResult()
                               {
                                   id = aicat.AssessmentItemCategoryID.GetValueOrDefault(),
                                   text = aicat.AssessmentItemCategoryName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Assessment_Item_Category pObjAssessmentItemCategory)
        {
            using (_db = new EIRSEntities())
            {
                Assessment_Item_Category mObjInsertUpdateAssessmentItemCategory; //Assessment Item Category Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load AssessmentItemCategory
                if (pObjAssessmentItemCategory.AssessmentItemCategoryID != 0)
                {
                    mObjInsertUpdateAssessmentItemCategory = (from aicat in _db.Assessment_Item_Category
                                                 where aicat.AssessmentItemCategoryID == pObjAssessmentItemCategory.AssessmentItemCategoryID
                                                 select aicat).FirstOrDefault();

                    if (mObjInsertUpdateAssessmentItemCategory != null)
                    {
                        mObjInsertUpdateAssessmentItemCategory.Active = !mObjInsertUpdateAssessmentItemCategory.Active;
                        mObjInsertUpdateAssessmentItemCategory.ModifiedBy = pObjAssessmentItemCategory.ModifiedBy;
                        mObjInsertUpdateAssessmentItemCategory.ModifiedDate = pObjAssessmentItemCategory.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Assessment Item Category Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetAssessmentItemCategoryList(pObjAssessmentItemCategory.AssessmentItemCategoryName, 0, pObjAssessmentItemCategory.AssessmentItemCategoryIds, pObjAssessmentItemCategory.intStatus, pObjAssessmentItemCategory.IncludeAssessmentItemCategoryIds, pObjAssessmentItemCategory.ExcludeAssessmentItemCategoryIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Assessment Item Category Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
