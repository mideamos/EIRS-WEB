using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class DirectorateRepository : IDirectorateRepository
    {
        EIRSEntities _db;

        public FuncResponse<Directorate> REP_InsertUpdateDirectorate(Directorate pObjDirectorate)
        {
            using (_db = new EIRSEntities())
            {
                Directorate mObjInsertUpdateDirectorate; //Directorate Insert Update Object
                FuncResponse<Directorate> mObjFuncResponse = new FuncResponse<Directorate>(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from drt in _db.Directorates
                                       where drt.DirectorateName == pObjDirectorate.DirectorateName && drt.DirectorateID != pObjDirectorate.DirectorateID
                                       select drt);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Directorate already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Directorate
                if (pObjDirectorate.DirectorateID != 0)
                {
                    mObjInsertUpdateDirectorate = (from drt in _db.Directorates
                                                   where drt.DirectorateID == pObjDirectorate.DirectorateID
                                                   select drt).FirstOrDefault();

                    if (mObjInsertUpdateDirectorate != null)
                    {
                        mObjInsertUpdateDirectorate.ModifiedBy = pObjDirectorate.ModifiedBy;
                        mObjInsertUpdateDirectorate.ModifiedDate = pObjDirectorate.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateDirectorate = new Directorate();
                        mObjInsertUpdateDirectorate.CreatedBy = pObjDirectorate.CreatedBy;
                        mObjInsertUpdateDirectorate.CreatedDate = pObjDirectorate.CreatedDate;
                    }
                }
                else // Else Insert Directorate
                {
                    mObjInsertUpdateDirectorate = new Directorate();
                    mObjInsertUpdateDirectorate.CreatedBy = pObjDirectorate.CreatedBy;
                    mObjInsertUpdateDirectorate.CreatedDate = pObjDirectorate.CreatedDate;
                }

                mObjInsertUpdateDirectorate.DirectorateName = pObjDirectorate.DirectorateName;
                mObjInsertUpdateDirectorate.Active = pObjDirectorate.Active;

                if (pObjDirectorate.DirectorateID == 0)
                {
                    _db.Directorates.Add(mObjInsertUpdateDirectorate);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjDirectorate.DirectorateID == 0)
                        mObjFuncResponse.Message = "Directorate Added Successfully";
                    else
                        mObjFuncResponse.Message = "Directorate Updated Successfully";

                    mObjFuncResponse.AdditionalData = mObjInsertUpdateDirectorate;

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjDirectorate.DirectorateID == 0)
                        mObjFuncResponse.Message = "Directorate Addition Failed";
                    else
                        mObjFuncResponse.Message = "Directorate Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetDirectorateList_Result> REP_GetDirectorateList(Directorate pObjDirectorate)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetDirectorateList(pObjDirectorate.DirectorateName, pObjDirectorate.DirectorateID, pObjDirectorate.DirectorateIds, pObjDirectorate.intStatus, pObjDirectorate.IncludeDirectorateIds, pObjDirectorate.ExcludeDirectorateIds).ToList();
            }
        }

        public usp_GetDirectorateList_Result REP_GetDirectorateDetails(Directorate pObjDirectorate)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetDirectorateList(pObjDirectorate.DirectorateName, pObjDirectorate.DirectorateID, pObjDirectorate.DirectorateIds, pObjDirectorate.intStatus, pObjDirectorate.IncludeDirectorateIds, pObjDirectorate.ExcludeDirectorateIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetDirectorateDropDownList(Directorate pObjDirectorate)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from drt in _db.usp_GetDirectorateList(pObjDirectorate.DirectorateName, pObjDirectorate.DirectorateID, pObjDirectorate.DirectorateIds, pObjDirectorate.intStatus, pObjDirectorate.IncludeDirectorateIds, pObjDirectorate.ExcludeDirectorateIds)
                               select new DropDownListResult()
                               {
                                   id = drt.DirectorateID.GetValueOrDefault(),
                                   text = drt.DirectorateName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Directorate pObjDirectorate)
        {
            using (_db = new EIRSEntities())
            {
                Directorate mObjInsertUpdateDirectorate; //Directorate Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Directorate
                if (pObjDirectorate.DirectorateID != 0)
                {
                    mObjInsertUpdateDirectorate = (from drt in _db.Directorates
                                                   where drt.DirectorateID == pObjDirectorate.DirectorateID
                                                   select drt).FirstOrDefault();

                    if (mObjInsertUpdateDirectorate != null)
                    {
                        mObjInsertUpdateDirectorate.Active = !mObjInsertUpdateDirectorate.Active;
                        mObjInsertUpdateDirectorate.ModifiedBy = pObjDirectorate.ModifiedBy;
                        mObjInsertUpdateDirectorate.ModifiedDate = pObjDirectorate.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Directorate Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetDirectorateList(pObjDirectorate.DirectorateName, 0, pObjDirectorate.DirectorateIds, pObjDirectorate.intStatus, pObjDirectorate.IncludeDirectorateIds, pObjDirectorate.ExcludeDirectorateIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Directorate Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_InsertRevenueStream(MAP_Directorates_RevenueStream pObjRevenueStream)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vExists = (from bbf in _db.MAP_Directorates_RevenueStream
                               where bbf.RevenueStreamID == pObjRevenueStream.RevenueStreamID && bbf.DirectorateID == pObjRevenueStream.DirectorateID
                               select bbf);

                if (vExists.Count() > 0)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Revenue Stream Already Exists";
                }

                _db.MAP_Directorates_RevenueStream.Add(pObjRevenueStream);

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = Ex.Message;
                }

                return mObjResponse;
            }
        }

        public FuncResponse REP_RemoveRevenueStream(MAP_Directorates_RevenueStream pObjRevenueStream)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                MAP_Directorates_RevenueStream mObjDeleteRevenueStream;

                mObjDeleteRevenueStream = _db.MAP_Directorates_RevenueStream.Find(pObjRevenueStream.DRSID);

                if (mObjDeleteRevenueStream == null)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Revenue Stream Already Removed.";
                }
                else
                {
                    _db.MAP_Directorates_RevenueStream.Remove(mObjDeleteRevenueStream);

                    try
                    {
                        _db.SaveChanges();
                        mObjResponse.Success = true;
                    }
                    catch (Exception Ex)
                    {
                        mObjResponse.Success = false;
                        mObjResponse.Message = Ex.Message;
                    }
                }

                return mObjResponse;
            }
        }

        public IList<MAP_Directorates_RevenueStream> REP_GetRevenueStream(int pIntDirectorateID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.MAP_Directorates_RevenueStream.Where(t => t.DirectorateID == pIntDirectorateID).ToList();
            }
        }
    }
}
