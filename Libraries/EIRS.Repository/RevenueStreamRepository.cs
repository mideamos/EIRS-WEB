using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class RevenueStreamRepository : IRevenueStreamRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateRevenueStream(Revenue_Stream pObjRevenueStream)
        {
            using (_db = new EIRSEntities())
            {
                Revenue_Stream mObjInsertUpdateRevenueStream; //Revenue Stream Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from rstrm in _db.Revenue_Stream
                                       where rstrm.RevenueStreamName == pObjRevenueStream.RevenueStreamName && /*rstrm.AssetTypeID == pObjRevenueStream.AssetTypeID &&*/ rstrm.RevenueStreamID != pObjRevenueStream.RevenueStreamID
                                       select rstrm);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Revenue Stream already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Revenue Stream
                if (pObjRevenueStream.RevenueStreamID != 0)
                {
                    mObjInsertUpdateRevenueStream = (from rstrm in _db.Revenue_Stream
                                                     where rstrm.RevenueStreamID == pObjRevenueStream.RevenueStreamID
                                                     select rstrm).FirstOrDefault();

                    if (mObjInsertUpdateRevenueStream != null)
                    {
                        mObjInsertUpdateRevenueStream.ModifiedBy = pObjRevenueStream.ModifiedBy;
                        mObjInsertUpdateRevenueStream.ModifiedDate = pObjRevenueStream.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateRevenueStream = new Revenue_Stream();
                        mObjInsertUpdateRevenueStream.CreatedBy = pObjRevenueStream.CreatedBy;
                        mObjInsertUpdateRevenueStream.CreatedDate = pObjRevenueStream.CreatedDate;
                    }
                }
                else // Else Insert Revenue Stream
                {
                    mObjInsertUpdateRevenueStream = new Revenue_Stream();
                    mObjInsertUpdateRevenueStream.CreatedBy = pObjRevenueStream.CreatedBy;
                    mObjInsertUpdateRevenueStream.CreatedDate = pObjRevenueStream.CreatedDate;
                }

                mObjInsertUpdateRevenueStream.RevenueStreamName = pObjRevenueStream.RevenueStreamName;
                //mObjInsertUpdateRevenueStream.AssetTypeID = pObjRevenueStream.AssetTypeID;
                mObjInsertUpdateRevenueStream.Active = pObjRevenueStream.Active;

                if (pObjRevenueStream.RevenueStreamID == 0)
                {
                    _db.Revenue_Stream.Add(mObjInsertUpdateRevenueStream);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjRevenueStream.RevenueStreamID == 0)
                        mObjFuncResponse.Message = "Revenue Stream Added Successfully";
                    else
                        mObjFuncResponse.Message = "Revenue Stream Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjRevenueStream.RevenueStreamID == 0)
                        mObjFuncResponse.Message = "Revenue Stream Addition Failed";
                    else
                        mObjFuncResponse.Message = "Revenue Stream Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetRevenueStreamList_Result> REP_GetRevenueStreamList(Revenue_Stream pObjRevenueStream)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetRevenueStreamList(pObjRevenueStream.RevenueStreamName, pObjRevenueStream.RevenueStreamID, pObjRevenueStream.RevenueStreamIds, pObjRevenueStream.intStatus, pObjRevenueStream.IncludeRevenueStreamIds, pObjRevenueStream.ExcludeRevenueStreamIds).ToList();
            }
        }

        public usp_GetRevenueStreamList_Result REP_GetRevenueStreamDetails(Revenue_Stream pObjRevenueStream)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetRevenueStreamList(pObjRevenueStream.RevenueStreamName, pObjRevenueStream.RevenueStreamID, pObjRevenueStream.RevenueStreamIds, pObjRevenueStream.intStatus, pObjRevenueStream.IncludeRevenueStreamIds, pObjRevenueStream.ExcludeRevenueStreamIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetRevenueStreamDropDownList(Revenue_Stream pObjRevenueStream)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from rstrm in _db.usp_GetRevenueStreamList(pObjRevenueStream.RevenueStreamName, pObjRevenueStream.RevenueStreamID, pObjRevenueStream.RevenueStreamIds, pObjRevenueStream.intStatus, pObjRevenueStream.IncludeRevenueStreamIds, pObjRevenueStream.ExcludeRevenueStreamIds)
                               select new DropDownListResult()
                               {
                                   id = rstrm.RevenueStreamID.GetValueOrDefault(),
                                   text = rstrm.RevenueStreamName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Revenue_Stream pObjRevenueStream)
        {
            using (_db = new EIRSEntities())
            {
                Revenue_Stream mObjInsertUpdateRevenueStream; //Revenue Stream Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load RevenueStream
                if (pObjRevenueStream.RevenueStreamID != 0)
                {
                    mObjInsertUpdateRevenueStream = (from rstrm in _db.Revenue_Stream
                                                     where rstrm.RevenueStreamID == pObjRevenueStream.RevenueStreamID
                                                     select rstrm).FirstOrDefault();

                    if (mObjInsertUpdateRevenueStream != null)
                    {
                        mObjInsertUpdateRevenueStream.Active = !mObjInsertUpdateRevenueStream.Active;
                        mObjInsertUpdateRevenueStream.ModifiedBy = pObjRevenueStream.ModifiedBy;
                        mObjInsertUpdateRevenueStream.ModifiedDate = pObjRevenueStream.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Revenue Stream Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetRevenueStreamList(pObjRevenueStream.RevenueStreamName, 0, pObjRevenueStream.RevenueStreamIds, pObjRevenueStream.intStatus, pObjRevenueStream.IncludeRevenueStreamIds, pObjRevenueStream.ExcludeRevenueStreamIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Revenue Stream Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
