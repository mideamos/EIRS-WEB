using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class LandDevelopmentRepository : ILandDevelopmentRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateLandDevelopment(Land_Development pObjLandDevelopment)
        {
            using (_db = new EIRSEntities())
            {
                Land_Development mObjInsertUpdateLandDevelopment; //Land Development Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from ldev in _db.Land_Development
                                       where ldev.LandDevelopmentName == pObjLandDevelopment.LandDevelopmentName && ldev.LandDevelopmentID != pObjLandDevelopment.LandDevelopmentID
                                       select ldev);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Land Development already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Land Development
                if (pObjLandDevelopment.LandDevelopmentID != 0)
                {
                    mObjInsertUpdateLandDevelopment = (from LandDevelopment in _db.Land_Development
                                                 where LandDevelopment.LandDevelopmentID == pObjLandDevelopment.LandDevelopmentID
                                                 select LandDevelopment).FirstOrDefault();

                    if (mObjInsertUpdateLandDevelopment != null)
                    {
                        mObjInsertUpdateLandDevelopment.ModifiedBy = pObjLandDevelopment.ModifiedBy;
                        mObjInsertUpdateLandDevelopment.ModifiedDate = pObjLandDevelopment.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateLandDevelopment = new Land_Development();
                        mObjInsertUpdateLandDevelopment.CreatedBy = pObjLandDevelopment.CreatedBy;
                        mObjInsertUpdateLandDevelopment.CreatedDate = pObjLandDevelopment.CreatedDate;
                    }
                }
                else // Else Insert Land Development
                {
                    mObjInsertUpdateLandDevelopment = new Land_Development();
                    mObjInsertUpdateLandDevelopment.CreatedBy = pObjLandDevelopment.CreatedBy;
                    mObjInsertUpdateLandDevelopment.CreatedDate = pObjLandDevelopment.CreatedDate;
                }

                mObjInsertUpdateLandDevelopment.LandDevelopmentName = pObjLandDevelopment.LandDevelopmentName;
                mObjInsertUpdateLandDevelopment.Active = pObjLandDevelopment.Active;

                if (pObjLandDevelopment.LandDevelopmentID == 0)
                {
                    _db.Land_Development.Add(mObjInsertUpdateLandDevelopment);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjLandDevelopment.LandDevelopmentID == 0)
                        mObjFuncResponse.Message = "Land Development Added Successfully";
                    else
                        mObjFuncResponse.Message = "Land Development Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjLandDevelopment.LandDevelopmentID == 0)
                        mObjFuncResponse.Message = "Land Development Addition Failed";
                    else
                        mObjFuncResponse.Message = "Land Development Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetLandDevelopmentList_Result> REP_GetLandDevelopmentList(Land_Development pObjLandDevelopment)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetLandDevelopmentList(pObjLandDevelopment.LandDevelopmentName, pObjLandDevelopment.LandDevelopmentID, pObjLandDevelopment.LandDevelopmentIds, pObjLandDevelopment.intStatus, pObjLandDevelopment.IncludeLandDevelopmentIds, pObjLandDevelopment.ExcludeLandDevelopmentIds).ToList();
            }
        }

        public usp_GetLandDevelopmentList_Result REP_GetLandDevelopmentDetails(Land_Development pObjLandDevelopment)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetLandDevelopmentList(pObjLandDevelopment.LandDevelopmentName, pObjLandDevelopment.LandDevelopmentID, pObjLandDevelopment.LandDevelopmentIds, pObjLandDevelopment.intStatus, pObjLandDevelopment.IncludeLandDevelopmentIds, pObjLandDevelopment.ExcludeLandDevelopmentIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetLandDevelopmentDropDownList(Land_Development pObjLandDevelopment)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from ldev in _db.usp_GetLandDevelopmentList(pObjLandDevelopment.LandDevelopmentName, pObjLandDevelopment.LandDevelopmentID, pObjLandDevelopment.LandDevelopmentIds, pObjLandDevelopment.intStatus, pObjLandDevelopment.IncludeLandDevelopmentIds, pObjLandDevelopment.ExcludeLandDevelopmentIds)
                               select new DropDownListResult()
                               {
                                   id = ldev.LandDevelopmentID.GetValueOrDefault(),
                                   text = ldev.LandDevelopmentName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Land_Development pObjLandDevelopment)
        {
            using (_db = new EIRSEntities())
            {
                Land_Development mObjInsertUpdateLandDevelopment; //Land Development Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load LandDevelopment
                if (pObjLandDevelopment.LandDevelopmentID != 0)
                {
                    mObjInsertUpdateLandDevelopment = (from ldev in _db.Land_Development
                                                 where ldev.LandDevelopmentID == pObjLandDevelopment.LandDevelopmentID
                                                 select ldev).FirstOrDefault();

                    if (mObjInsertUpdateLandDevelopment != null)
                    {
                        mObjInsertUpdateLandDevelopment.Active = !mObjInsertUpdateLandDevelopment.Active;
                        mObjInsertUpdateLandDevelopment.ModifiedBy = pObjLandDevelopment.ModifiedBy;
                        mObjInsertUpdateLandDevelopment.ModifiedDate = pObjLandDevelopment.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Land Development Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetLandDevelopmentList(pObjLandDevelopment.LandDevelopmentName, 0, pObjLandDevelopment.LandDevelopmentIds, pObjLandDevelopment.intStatus, pObjLandDevelopment.IncludeLandDevelopmentIds, pObjLandDevelopment.ExcludeLandDevelopmentIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Land Development Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
