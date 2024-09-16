using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class RevenueSubStreamRepository : IRevenueSubStreamRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateRevenueSubStream(Revenue_SubStream pObjRevenueSubStream)
        {
            using (_db = new EIRSEntities())
            {
                Revenue_SubStream mObjInsertUpdateRevenueSubStream; //Revenue Sub Stream Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from rsstrm in _db.Revenue_SubStream
                                       where rsstrm.RevenueSubStreamName == pObjRevenueSubStream.RevenueSubStreamName && rsstrm.RevenueStreamID == pObjRevenueSubStream.RevenueStreamID && rsstrm.RevenueSubStreamID != pObjRevenueSubStream.RevenueSubStreamID
                                       select rsstrm);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Revenue Sub Stream already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Revenue Sub Stream
                if (pObjRevenueSubStream.RevenueSubStreamID != 0)
                {
                    mObjInsertUpdateRevenueSubStream = (from rsstrm in _db.Revenue_SubStream
                                                        where rsstrm.RevenueSubStreamID == pObjRevenueSubStream.RevenueSubStreamID
                                                        select rsstrm).FirstOrDefault();

                    if (mObjInsertUpdateRevenueSubStream != null)
                    {
                        mObjInsertUpdateRevenueSubStream.ModifiedBy = pObjRevenueSubStream.ModifiedBy;
                        mObjInsertUpdateRevenueSubStream.ModifiedDate = pObjRevenueSubStream.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateRevenueSubStream = new Revenue_SubStream();
                        mObjInsertUpdateRevenueSubStream.CreatedBy = pObjRevenueSubStream.CreatedBy;
                        mObjInsertUpdateRevenueSubStream.CreatedDate = pObjRevenueSubStream.CreatedDate;
                    }
                }
                else // Else Insert Revenue Sub Stream
                {
                    mObjInsertUpdateRevenueSubStream = new Revenue_SubStream();
                    mObjInsertUpdateRevenueSubStream.CreatedBy = pObjRevenueSubStream.CreatedBy;
                    mObjInsertUpdateRevenueSubStream.CreatedDate = pObjRevenueSubStream.CreatedDate;
                }

                mObjInsertUpdateRevenueSubStream.RevenueSubStreamName = pObjRevenueSubStream.RevenueSubStreamName;
                mObjInsertUpdateRevenueSubStream.RevenueStreamID = pObjRevenueSubStream.RevenueStreamID;
                mObjInsertUpdateRevenueSubStream.Active = pObjRevenueSubStream.Active;

                if (pObjRevenueSubStream.RevenueSubStreamID == 0)
                {
                    _db.Revenue_SubStream.Add(mObjInsertUpdateRevenueSubStream);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjRevenueSubStream.RevenueSubStreamID == 0)
                        mObjFuncResponse.Message = "Revenue Sub Stream Added Successfully";
                    else
                        mObjFuncResponse.Message = "Revenue Sub Stream Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjRevenueSubStream.RevenueSubStreamID == 0)
                        mObjFuncResponse.Message = "Revenue Sub Stream Addition Failed";
                    else
                        mObjFuncResponse.Message = "Revenue Sub Stream Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetRevenueSubStreamList_Result> REP_GetRevenueSubStreamList(Revenue_SubStream pObjRevenueSubStream)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetRevenueSubStreamList(pObjRevenueSubStream.RevenueSubStreamName, pObjRevenueSubStream.RevenueSubStreamID,  pObjRevenueSubStream.RevenueStreamID, pObjRevenueSubStream.RevenueSubStreamIds, pObjRevenueSubStream.intStatus, pObjRevenueSubStream.IncludeRevenueSubStreamIds, pObjRevenueSubStream.ExcludeRevenueSubStreamIds).ToList();
            }
        }

        public usp_GetRevenueSubStreamList_Result REP_GetRevenueSubStreamDetails(Revenue_SubStream pObjRevenueSubStream)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetRevenueSubStreamList(pObjRevenueSubStream.RevenueSubStreamName, pObjRevenueSubStream.RevenueSubStreamID,
                     pObjRevenueSubStream.RevenueStreamID, pObjRevenueSubStream.RevenueSubStreamIds, pObjRevenueSubStream.intStatus, pObjRevenueSubStream.IncludeRevenueSubStreamIds, pObjRevenueSubStream.ExcludeRevenueSubStreamIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetRevenueSubStreamDropDownList(Revenue_SubStream pObjRevenueSubStream)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from rsstrm in _db.usp_GetRevenueSubStreamList(pObjRevenueSubStream.RevenueSubStreamName, pObjRevenueSubStream.RevenueSubStreamID,  pObjRevenueSubStream.RevenueStreamID, pObjRevenueSubStream.RevenueSubStreamIds, pObjRevenueSubStream.intStatus, pObjRevenueSubStream.IncludeRevenueSubStreamIds, pObjRevenueSubStream.ExcludeRevenueSubStreamIds)
                               select new DropDownListResult()
                               {
                                   id = rsstrm.RevenueSubStreamID.GetValueOrDefault(),
                                   text = rsstrm.RevenueSubStreamName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Revenue_SubStream pObjRevenueSubStream)
        {
            using (_db = new EIRSEntities())
            {
                Revenue_SubStream mObjInsertUpdateRevenueSubStream; //Revenue Sub Stream Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load RevenueSubStream
                if (pObjRevenueSubStream.RevenueSubStreamID != 0)
                {
                    mObjInsertUpdateRevenueSubStream = (from rsstrm in _db.Revenue_SubStream
                                                        where rsstrm.RevenueSubStreamID == pObjRevenueSubStream.RevenueSubStreamID
                                                        select rsstrm).FirstOrDefault();

                    if (mObjInsertUpdateRevenueSubStream != null)
                    {
                        mObjInsertUpdateRevenueSubStream.Active = !mObjInsertUpdateRevenueSubStream.Active;
                        mObjInsertUpdateRevenueSubStream.ModifiedBy = pObjRevenueSubStream.ModifiedBy;
                        mObjInsertUpdateRevenueSubStream.ModifiedDate = pObjRevenueSubStream.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Revenue Sub Stream Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetRevenueSubStreamList(pObjRevenueSubStream.RevenueSubStreamName, 0,  pObjRevenueSubStream.RevenueStreamID, pObjRevenueSubStream.RevenueSubStreamIds, pObjRevenueSubStream.intStatus, pObjRevenueSubStream.IncludeRevenueSubStreamIds, pObjRevenueSubStream.ExcludeRevenueSubStreamIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Revenue Sub Stream Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
