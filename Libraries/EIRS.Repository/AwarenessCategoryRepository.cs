using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;


namespace EIRS.Repository
{
    public class AwarenessCategoryRepository : IAwarenessCategoryRepository
    {
        ERASEntities _db;

        public FuncResponse REP_InsertUpdateAwarenessCategory(MST_AwarenessCategory pObjAwarenessCategory)
        {
            using (_db = new ERASEntities())
            {
                MST_AwarenessCategory mObjInsertUpdateAwarenessCategory; //Awareness Category Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from fsec in _db.MST_AwarenessCategory
                                       where fsec.AwarenessCategoryName == pObjAwarenessCategory.AwarenessCategoryName && fsec.AwarenessCategoryID != pObjAwarenessCategory.AwarenessCategoryID
                                       select fsec);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "AwarenessCategory already exists";
                    return mObjFuncResponse;
                }

                //If Update Load AwarenessCategory
                if (pObjAwarenessCategory.AwarenessCategoryID != 0)
                {
                    mObjInsertUpdateAwarenessCategory = (from fsec in _db.MST_AwarenessCategory
                                                         where fsec.AwarenessCategoryID == pObjAwarenessCategory.AwarenessCategoryID
                                                         select fsec).FirstOrDefault();

                    if (mObjInsertUpdateAwarenessCategory != null)
                    {
                        mObjInsertUpdateAwarenessCategory.ModifiedBy = pObjAwarenessCategory.ModifiedBy;
                        mObjInsertUpdateAwarenessCategory.ModifiedDate = pObjAwarenessCategory.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateAwarenessCategory = new MST_AwarenessCategory();
                        mObjInsertUpdateAwarenessCategory.CreatedBy = pObjAwarenessCategory.CreatedBy;
                        mObjInsertUpdateAwarenessCategory.CreatedDate = pObjAwarenessCategory.CreatedDate;
                    }
                }
                else // Else Insert AwarenessCategory
                {
                    mObjInsertUpdateAwarenessCategory = new MST_AwarenessCategory();
                    mObjInsertUpdateAwarenessCategory.CreatedBy = pObjAwarenessCategory.CreatedBy;
                    mObjInsertUpdateAwarenessCategory.CreatedDate = pObjAwarenessCategory.CreatedDate;
                }

                mObjInsertUpdateAwarenessCategory.AwarenessCategoryName = pObjAwarenessCategory.AwarenessCategoryName;
                mObjInsertUpdateAwarenessCategory.SectionDescription = pObjAwarenessCategory.SectionDescription;
                mObjInsertUpdateAwarenessCategory.Active = pObjAwarenessCategory.Active;

                if (pObjAwarenessCategory.AwarenessCategoryID == 0)
                {
                    _db.MST_AwarenessCategory.Add(mObjInsertUpdateAwarenessCategory);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjAwarenessCategory.AwarenessCategoryID == 0)
                        mObjFuncResponse.Message = "Awareness Category Added Successfully";
                    else
                        mObjFuncResponse.Message = "Awareness Category Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjAwarenessCategory.AwarenessCategoryID == 0)
                        mObjFuncResponse.Message = "Awareness Category Addition Failed";
                    else
                        mObjFuncResponse.Message = "Awareness Category Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetAwarenessCategoryList_Result> REP_GetAwarenessCategoryList(MST_AwarenessCategory pObjAwarenessCategory)
        {
            using (_db = new ERASEntities())
            {
                return _db.usp_GetAwarenessCategoryList(pObjAwarenessCategory.AwarenessCategoryName, pObjAwarenessCategory.AwarenessCategoryID, pObjAwarenessCategory.AwarenessCategoryIds, pObjAwarenessCategory.intStatus, pObjAwarenessCategory.IncludeAwarenessCategoryIds, pObjAwarenessCategory.ExcludeAwarenessCategoryIds).ToList();
            }
        }

        public usp_GetAwarenessCategoryList_Result REP_GetAwarenessCategoryDetails(MST_AwarenessCategory pObjAwarenessCategory)
        {
            using (_db = new ERASEntities())
            {
                return _db.usp_GetAwarenessCategoryList(pObjAwarenessCategory.AwarenessCategoryName, pObjAwarenessCategory.AwarenessCategoryID, pObjAwarenessCategory.AwarenessCategoryIds, pObjAwarenessCategory.intStatus, pObjAwarenessCategory.IncludeAwarenessCategoryIds, pObjAwarenessCategory.ExcludeAwarenessCategoryIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetAwarenessCategoryDropDownList(MST_AwarenessCategory pObjAwarenessCategory)
        {
            using (_db = new ERASEntities())
            {
                var vResult = (from st in _db.usp_GetAwarenessCategoryList(pObjAwarenessCategory.AwarenessCategoryName, pObjAwarenessCategory.AwarenessCategoryID, pObjAwarenessCategory.AwarenessCategoryIds, pObjAwarenessCategory.intStatus, pObjAwarenessCategory.IncludeAwarenessCategoryIds, pObjAwarenessCategory.ExcludeAwarenessCategoryIds)
                               select new DropDownListResult()
                               {
                                   id = st.AwarenessCategoryID.GetValueOrDefault(),
                                   text = st.AwarenessCategoryName
                               }).ToList();

                return vResult;
            }
        }
    }
}
