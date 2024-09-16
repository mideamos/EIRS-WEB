using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class EM_RevenueHeadRepository : IEM_RevenueHeadRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateRevenueHead(EM_RevenueHead pObjRevenueHead)
        {
            using (_db = new EIRSEntities())
            {
                EM_RevenueHead mObjInsertUpdateRevenueHead; //Revenue Head Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from rhead in _db.EM_RevenueHead
                                       where rhead.RevenueHeadName == pObjRevenueHead.RevenueHeadName && rhead.CategoryID == pObjRevenueHead.CategoryID && rhead.RevenueHeadID != pObjRevenueHead.RevenueHeadID
                                       select rhead);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Revenue Head already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Revenue Head
                if (pObjRevenueHead.RevenueHeadID != 0)
                {
                    mObjInsertUpdateRevenueHead = (from rhead in _db.EM_RevenueHead
                                                   where rhead.RevenueHeadID == pObjRevenueHead.RevenueHeadID
                                                   select rhead).FirstOrDefault();

                    if (mObjInsertUpdateRevenueHead != null)
                    {
                        mObjInsertUpdateRevenueHead.ModifiedBy = pObjRevenueHead.ModifiedBy;
                        mObjInsertUpdateRevenueHead.ModifiedDate = pObjRevenueHead.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateRevenueHead = new EM_RevenueHead();
                        mObjInsertUpdateRevenueHead.CreatedBy = pObjRevenueHead.CreatedBy;
                        mObjInsertUpdateRevenueHead.CreatedDate = pObjRevenueHead.CreatedDate;
                    }
                }
                else // Else Insert Revenue Head
                {
                    mObjInsertUpdateRevenueHead = new EM_RevenueHead();
                    mObjInsertUpdateRevenueHead.CreatedBy = pObjRevenueHead.CreatedBy;
                    mObjInsertUpdateRevenueHead.CreatedDate = pObjRevenueHead.CreatedDate;
                }

                mObjInsertUpdateRevenueHead.RevenueHeadName = pObjRevenueHead.RevenueHeadName;
                mObjInsertUpdateRevenueHead.CategoryID = pObjRevenueHead.CategoryID;
                mObjInsertUpdateRevenueHead.Active = pObjRevenueHead.Active;

                if (pObjRevenueHead.RevenueHeadID == 0)
                {
                    _db.EM_RevenueHead.Add(mObjInsertUpdateRevenueHead);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjRevenueHead.RevenueHeadID == 0)
                        mObjFuncResponse.Message = "Revenue Head Added Successfully";
                    else
                        mObjFuncResponse.Message = "Revenue Head Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjRevenueHead.RevenueHeadID == 0)
                        mObjFuncResponse.Message = "Revenue Head Addition Failed";
                    else
                        mObjFuncResponse.Message = "Revenue Head Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_EM_GetRevenueHeadList_Result> REP_GetRevenueHeadList(EM_RevenueHead pObjRevenueHead)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_EM_GetRevenueHeadList(pObjRevenueHead.RevenueHeadName, pObjRevenueHead.RevenueHeadID, pObjRevenueHead.CategoryID, pObjRevenueHead.RevenueHeadIds, pObjRevenueHead.intStatus, pObjRevenueHead.IncludeRevenueHeadIds, pObjRevenueHead.ExcludeRevenueHeadIds).ToList();
            }
        }

        public usp_EM_GetRevenueHeadList_Result REP_GetRevenueHeadDetails(EM_RevenueHead pObjRevenueHead)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_EM_GetRevenueHeadList(pObjRevenueHead.RevenueHeadName, pObjRevenueHead.RevenueHeadID,
                     pObjRevenueHead.CategoryID, pObjRevenueHead.RevenueHeadIds, pObjRevenueHead.intStatus, pObjRevenueHead.IncludeRevenueHeadIds, pObjRevenueHead.ExcludeRevenueHeadIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetRevenueHeadDropDownList(EM_RevenueHead pObjRevenueHead)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from rhead in _db.usp_EM_GetRevenueHeadList(pObjRevenueHead.RevenueHeadName, pObjRevenueHead.RevenueHeadID, pObjRevenueHead.CategoryID, pObjRevenueHead.RevenueHeadIds, pObjRevenueHead.intStatus, pObjRevenueHead.IncludeRevenueHeadIds, pObjRevenueHead.ExcludeRevenueHeadIds)
                               select new DropDownListResult()
                               {
                                   id = rhead.RevenueHeadID.GetValueOrDefault(),
                                   text = rhead.RevenueHeadName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(EM_RevenueHead pObjRevenueHead)
        {
            using (_db = new EIRSEntities())
            {
                EM_RevenueHead mObjInsertUpdateRevenueHead; //Revenue Head Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load RevenueHead
                if (pObjRevenueHead.RevenueHeadID != 0)
                {
                    mObjInsertUpdateRevenueHead = (from rhead in _db.EM_RevenueHead
                                                   where rhead.RevenueHeadID == pObjRevenueHead.RevenueHeadID
                                                   select rhead).FirstOrDefault();

                    if (mObjInsertUpdateRevenueHead != null)
                    {
                        mObjInsertUpdateRevenueHead.Active = !mObjInsertUpdateRevenueHead.Active;
                        mObjInsertUpdateRevenueHead.ModifiedBy = pObjRevenueHead.ModifiedBy;
                        mObjInsertUpdateRevenueHead.ModifiedDate = pObjRevenueHead.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Revenue Head Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_EM_GetRevenueHeadList(pObjRevenueHead.RevenueHeadName, 0, pObjRevenueHead.CategoryID, pObjRevenueHead.RevenueHeadIds, pObjRevenueHead.intStatus, pObjRevenueHead.IncludeRevenueHeadIds, pObjRevenueHead.ExcludeRevenueHeadIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Revenue Head Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
