using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class AssessmentItemSubCategoryRepository : IAssessmentItemSubCategoryRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateAssessmentItemSubCategory(Assessment_Item_SubCategory pObjAssessmentItemSubCategory)
        {
            using (_db = new EIRSEntities())
            {
                Assessment_Item_SubCategory mObjInsertUpdateAssessmentItemSubCategory; //Assessment Item Sub Category Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from aiscat in _db.Assessment_Item_SubCategory
                                       where aiscat.AssessmentItemSubCategoryName == pObjAssessmentItemSubCategory.AssessmentItemSubCategoryName && aiscat.AssessmentItemCategoryID == pObjAssessmentItemSubCategory.AssessmentItemCategoryID && aiscat.AssessmentItemSubCategoryID != pObjAssessmentItemSubCategory.AssessmentItemSubCategoryID
                                       select aiscat);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Assessment Item Sub Category already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Assessment Item Sub Category
                if (pObjAssessmentItemSubCategory.AssessmentItemSubCategoryID != 0)
                {
                    mObjInsertUpdateAssessmentItemSubCategory = (from aiscat in _db.Assessment_Item_SubCategory
                                                 where aiscat.AssessmentItemSubCategoryID == pObjAssessmentItemSubCategory.AssessmentItemSubCategoryID
                                                 select aiscat).FirstOrDefault();

                    if (mObjInsertUpdateAssessmentItemSubCategory != null)
                    {
                        mObjInsertUpdateAssessmentItemSubCategory.ModifiedBy = pObjAssessmentItemSubCategory.ModifiedBy;
                        mObjInsertUpdateAssessmentItemSubCategory.ModifiedDate = pObjAssessmentItemSubCategory.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateAssessmentItemSubCategory = new Assessment_Item_SubCategory();
                        mObjInsertUpdateAssessmentItemSubCategory.CreatedBy = pObjAssessmentItemSubCategory.CreatedBy;
                        mObjInsertUpdateAssessmentItemSubCategory.CreatedDate = pObjAssessmentItemSubCategory.CreatedDate;
                    }
                }
                else // Else Insert Assessment Item Sub Category
                {
                    mObjInsertUpdateAssessmentItemSubCategory = new Assessment_Item_SubCategory();
                    mObjInsertUpdateAssessmentItemSubCategory.CreatedBy = pObjAssessmentItemSubCategory.CreatedBy;
                    mObjInsertUpdateAssessmentItemSubCategory.CreatedDate = pObjAssessmentItemSubCategory.CreatedDate;
                }

                mObjInsertUpdateAssessmentItemSubCategory.AssessmentItemSubCategoryName = pObjAssessmentItemSubCategory.AssessmentItemSubCategoryName;
                mObjInsertUpdateAssessmentItemSubCategory.AssessmentItemCategoryID = pObjAssessmentItemSubCategory.AssessmentItemCategoryID;
                mObjInsertUpdateAssessmentItemSubCategory.Active = pObjAssessmentItemSubCategory.Active;

                if (pObjAssessmentItemSubCategory.AssessmentItemSubCategoryID == 0)
                {
                    _db.Assessment_Item_SubCategory.Add(mObjInsertUpdateAssessmentItemSubCategory);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjAssessmentItemSubCategory.AssessmentItemSubCategoryID == 0)
                        mObjFuncResponse.Message = "Assessment Item Sub Category Added Successfully";
                    else
                        mObjFuncResponse.Message = "Assessment Item Sub Category Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjAssessmentItemSubCategory.AssessmentItemSubCategoryID == 0)
                        mObjFuncResponse.Message = "Assessment Item Sub Category Addition Failed";
                    else
                        mObjFuncResponse.Message = "Assessment Item Sub Category Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetAssessmentItemSubCategoryList_Result> REP_GetAssessmentItemSubCategoryList(Assessment_Item_SubCategory pObjAssessmentItemSubCategory)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentItemSubCategoryList(pObjAssessmentItemSubCategory.AssessmentItemSubCategoryName, pObjAssessmentItemSubCategory.AssessmentItemSubCategoryID, pObjAssessmentItemSubCategory.AssessmentItemCategoryID, pObjAssessmentItemSubCategory.AssessmentItemSubCategoryIds, pObjAssessmentItemSubCategory.intStatus, pObjAssessmentItemSubCategory.IncludeAssessmentItemSubCategoryIds, pObjAssessmentItemSubCategory.ExcludeAssessmentItemSubCategoryIds).ToList();
            }
        }

        public usp_GetAssessmentItemSubCategoryList_Result REP_GetAssessmentItemSubCategoryDetails(Assessment_Item_SubCategory pObjAssessmentItemSubCategory)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentItemSubCategoryList(pObjAssessmentItemSubCategory.AssessmentItemSubCategoryName, pObjAssessmentItemSubCategory.AssessmentItemSubCategoryID, pObjAssessmentItemSubCategory.AssessmentItemCategoryID, pObjAssessmentItemSubCategory.AssessmentItemSubCategoryIds, pObjAssessmentItemSubCategory.intStatus, pObjAssessmentItemSubCategory.IncludeAssessmentItemSubCategoryIds, pObjAssessmentItemSubCategory.ExcludeAssessmentItemSubCategoryIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetAssessmentItemSubCategoryDropDownList(Assessment_Item_SubCategory pObjAssessmentItemSubCategory)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from aiscat in _db.usp_GetAssessmentItemSubCategoryList(pObjAssessmentItemSubCategory.AssessmentItemSubCategoryName, pObjAssessmentItemSubCategory.AssessmentItemSubCategoryID, pObjAssessmentItemSubCategory.AssessmentItemCategoryID, pObjAssessmentItemSubCategory.AssessmentItemSubCategoryIds, pObjAssessmentItemSubCategory.intStatus, pObjAssessmentItemSubCategory.IncludeAssessmentItemSubCategoryIds, pObjAssessmentItemSubCategory.ExcludeAssessmentItemSubCategoryIds)
                               select new DropDownListResult()
                               {
                                   id = aiscat.AssessmentItemSubCategoryID.GetValueOrDefault(),
                                   text = aiscat.AssessmentItemSubCategoryName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Assessment_Item_SubCategory pObjAssessmentItemSubCategory)
        {
            using (_db = new EIRSEntities())
            {
                Assessment_Item_SubCategory mObjInsertUpdateAssessmentItemSubCategory; //Assessment Item Sub Category Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load AssessmentItemSubCategory
                if (pObjAssessmentItemSubCategory.AssessmentItemSubCategoryID != 0)
                {
                    mObjInsertUpdateAssessmentItemSubCategory = (from aiscat in _db.Assessment_Item_SubCategory
                                                 where aiscat.AssessmentItemSubCategoryID == pObjAssessmentItemSubCategory.AssessmentItemSubCategoryID
                                                 select aiscat).FirstOrDefault();

                    if (mObjInsertUpdateAssessmentItemSubCategory != null)
                    {
                        mObjInsertUpdateAssessmentItemSubCategory.Active = !mObjInsertUpdateAssessmentItemSubCategory.Active;
                        mObjInsertUpdateAssessmentItemSubCategory.ModifiedBy = pObjAssessmentItemSubCategory.ModifiedBy;
                        mObjInsertUpdateAssessmentItemSubCategory.ModifiedDate = pObjAssessmentItemSubCategory.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Assessment Item Sub Category Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetAssessmentItemSubCategoryList(pObjAssessmentItemSubCategory.AssessmentItemSubCategoryName, 0, pObjAssessmentItemSubCategory.AssessmentItemCategoryID, pObjAssessmentItemSubCategory.AssessmentItemSubCategoryIds, pObjAssessmentItemSubCategory.intStatus, pObjAssessmentItemSubCategory.IncludeAssessmentItemSubCategoryIds, pObjAssessmentItemSubCategory.ExcludeAssessmentItemSubCategoryIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Assessment Item Sub Category Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
