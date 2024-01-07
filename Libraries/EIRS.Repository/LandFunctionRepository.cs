using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class LandFunctionRepository : ILandFunctionRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateLandFunction(Land_Function pObjLandFunction)
        {
            using (_db = new EIRSEntities())
            {
                Land_Function mObjInsertUpdateLandFunction; //Land Function Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from lfunc in _db.Land_Function
                                       where lfunc.LandFunctionName == pObjLandFunction.LandFunctionName && lfunc.LandPurposeID == pObjLandFunction.LandPurposeID && lfunc.LandFunctionID != pObjLandFunction.LandFunctionID
                                       select lfunc);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Land Function already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Land Function
                if (pObjLandFunction.LandFunctionID != 0)
                {
                    mObjInsertUpdateLandFunction = (from LandFunction in _db.Land_Function
                                                        where LandFunction.LandFunctionID == pObjLandFunction.LandFunctionID
                                                        select LandFunction).FirstOrDefault();

                    if (mObjInsertUpdateLandFunction != null)
                    {
                        mObjInsertUpdateLandFunction.ModifiedBy = pObjLandFunction.ModifiedBy;
                        mObjInsertUpdateLandFunction.ModifiedDate = pObjLandFunction.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateLandFunction = new Land_Function();
                        mObjInsertUpdateLandFunction.CreatedBy = pObjLandFunction.CreatedBy;
                        mObjInsertUpdateLandFunction.CreatedDate = pObjLandFunction.CreatedDate;
                    }
                }
                else // Else Insert Land Function
                {
                    mObjInsertUpdateLandFunction = new Land_Function();
                    mObjInsertUpdateLandFunction.CreatedBy = pObjLandFunction.CreatedBy;
                    mObjInsertUpdateLandFunction.CreatedDate = pObjLandFunction.CreatedDate;
                }

                mObjInsertUpdateLandFunction.LandFunctionName = pObjLandFunction.LandFunctionName;
                mObjInsertUpdateLandFunction.LandPurposeID = pObjLandFunction.LandPurposeID;
                mObjInsertUpdateLandFunction.Active = pObjLandFunction.Active;

                if (pObjLandFunction.LandFunctionID == 0)
                {
                    _db.Land_Function.Add(mObjInsertUpdateLandFunction);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjLandFunction.LandFunctionID == 0)
                        mObjFuncResponse.Message = "Land Function Added Successfully";
                    else
                        mObjFuncResponse.Message = "Land Function Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjLandFunction.LandFunctionID == 0)
                        mObjFuncResponse.Message = "Land Function Addition Failed";
                    else
                        mObjFuncResponse.Message = "Land Function Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetLandFunctionList_Result> REP_GetLandFunctionList(Land_Function pObjLandFunction)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetLandFunctionList(pObjLandFunction.LandFunctionName, pObjLandFunction.LandFunctionID, pObjLandFunction.LandPurposeID, pObjLandFunction.LandFunctionIds, pObjLandFunction.intStatus, pObjLandFunction.IncludeLandFunctionIds, pObjLandFunction.ExcludeLandFunctionIds).ToList();
            }
        }

        public usp_GetLandFunctionList_Result REP_GetLandFunctionDetails(Land_Function pObjLandFunction)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetLandFunctionList(pObjLandFunction.LandFunctionName, pObjLandFunction.LandFunctionID, pObjLandFunction.LandPurposeID, pObjLandFunction.LandFunctionIds, pObjLandFunction.intStatus, pObjLandFunction.IncludeLandFunctionIds, pObjLandFunction.ExcludeLandFunctionIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetLandFunctionDropDownList(Land_Function pObjLandFunction)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from lfunc in _db.usp_GetLandFunctionList(pObjLandFunction.LandFunctionName, pObjLandFunction.LandFunctionID, pObjLandFunction.LandPurposeID, pObjLandFunction.LandFunctionIds, pObjLandFunction.intStatus, pObjLandFunction.IncludeLandFunctionIds, pObjLandFunction.ExcludeLandFunctionIds)
                               select new DropDownListResult()
                               {
                                   id = lfunc.LandFunctionID.GetValueOrDefault(),
                                   text = lfunc.LandFunctionName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Land_Function pObjLandFunction)
        {
            using (_db = new EIRSEntities())
            {
                Land_Function mObjInsertUpdateLandFunction; //Land Function Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load LandFunction
                if (pObjLandFunction.LandFunctionID != 0)
                {
                    mObjInsertUpdateLandFunction = (from lfunc in _db.Land_Function
                                                        where lfunc.LandFunctionID == pObjLandFunction.LandFunctionID
                                                        select lfunc).FirstOrDefault();

                    if (mObjInsertUpdateLandFunction != null)
                    {
                        mObjInsertUpdateLandFunction.Active = !mObjInsertUpdateLandFunction.Active;
                        mObjInsertUpdateLandFunction.ModifiedBy = pObjLandFunction.ModifiedBy;
                        mObjInsertUpdateLandFunction.ModifiedDate = pObjLandFunction.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Land Function Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetLandFunctionList(pObjLandFunction.LandFunctionName, 0, pObjLandFunction.LandPurposeID, pObjLandFunction.LandFunctionIds, pObjLandFunction.intStatus, pObjLandFunction.IncludeLandFunctionIds, pObjLandFunction.ExcludeLandFunctionIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Land Function Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
