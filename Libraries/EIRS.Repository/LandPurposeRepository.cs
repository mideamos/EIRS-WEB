using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class LandPurposeRepository : ILandPurposeRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateLandPurpose(Land_Purpose pObjLandPurpose)
        {
            using (_db = new EIRSEntities())
            {
                Land_Purpose mObjInsertUpdateLandPurpose; //Land Purpose Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from bpurp in _db.Land_Purpose
                                       where bpurp.LandPurposeName == pObjLandPurpose.LandPurposeName && bpurp.LandPurposeID != pObjLandPurpose.LandPurposeID
                                       select bpurp);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Land Purpose already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Land Purpose
                if (pObjLandPurpose.LandPurposeID != 0)
                {
                    mObjInsertUpdateLandPurpose = (from LandPurpose in _db.Land_Purpose
                                                 where LandPurpose.LandPurposeID == pObjLandPurpose.LandPurposeID
                                                 select LandPurpose).FirstOrDefault();

                    if (mObjInsertUpdateLandPurpose != null)
                    {
                        mObjInsertUpdateLandPurpose.ModifiedBy = pObjLandPurpose.ModifiedBy;
                        mObjInsertUpdateLandPurpose.ModifiedDate = pObjLandPurpose.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateLandPurpose = new Land_Purpose();
                        mObjInsertUpdateLandPurpose.CreatedBy = pObjLandPurpose.CreatedBy;
                        mObjInsertUpdateLandPurpose.CreatedDate = pObjLandPurpose.CreatedDate;
                    }
                }
                else // Else Insert Land Purpose
                {
                    mObjInsertUpdateLandPurpose = new Land_Purpose();
                    mObjInsertUpdateLandPurpose.CreatedBy = pObjLandPurpose.CreatedBy;
                    mObjInsertUpdateLandPurpose.CreatedDate = pObjLandPurpose.CreatedDate;
                }

                mObjInsertUpdateLandPurpose.LandPurposeName = pObjLandPurpose.LandPurposeName;
                mObjInsertUpdateLandPurpose.Active = pObjLandPurpose.Active;

                if (pObjLandPurpose.LandPurposeID == 0)
                {
                    _db.Land_Purpose.Add(mObjInsertUpdateLandPurpose);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjLandPurpose.LandPurposeID == 0)
                        mObjFuncResponse.Message = "Land Purpose Added Successfully";
                    else
                        mObjFuncResponse.Message = "Land Purpose Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjLandPurpose.LandPurposeID == 0)
                        mObjFuncResponse.Message = "Land Purpose Addition Failed";
                    else
                        mObjFuncResponse.Message = "Land Purpose Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetLandPurposeList_Result> REP_GetLandPurposeList(Land_Purpose pObjLandPurpose)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetLandPurposeList(pObjLandPurpose.LandPurposeName, pObjLandPurpose.LandPurposeID, pObjLandPurpose.LandPurposeIds, pObjLandPurpose.intStatus, pObjLandPurpose.IncludeLandPurposeIds, pObjLandPurpose.ExcludeLandPurposeIds).ToList();
            }
        }

        public usp_GetLandPurposeList_Result REP_GetLandPurposeDetails(Land_Purpose pObjLandPurpose)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetLandPurposeList(pObjLandPurpose.LandPurposeName, pObjLandPurpose.LandPurposeID, pObjLandPurpose.LandPurposeIds, pObjLandPurpose.intStatus, pObjLandPurpose.IncludeLandPurposeIds, pObjLandPurpose.ExcludeLandPurposeIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetLandPurposeDropDownList(Land_Purpose pObjLandPurpose)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from bpurp in _db.usp_GetLandPurposeList(pObjLandPurpose.LandPurposeName, pObjLandPurpose.LandPurposeID, pObjLandPurpose.LandPurposeIds, pObjLandPurpose.intStatus, pObjLandPurpose.IncludeLandPurposeIds, pObjLandPurpose.ExcludeLandPurposeIds)
                               select new DropDownListResult()
                               {
                                   id = bpurp.LandPurposeID.GetValueOrDefault(),
                                   text = bpurp.LandPurposeName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Land_Purpose pObjLandPurpose)
        {
            using (_db = new EIRSEntities())
            {
                Land_Purpose mObjInsertUpdateLandPurpose; //Land Purpose Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load LandPurpose
                if (pObjLandPurpose.LandPurposeID != 0)
                {
                    mObjInsertUpdateLandPurpose = (from bpurp in _db.Land_Purpose
                                                 where bpurp.LandPurposeID == pObjLandPurpose.LandPurposeID
                                                 select bpurp).FirstOrDefault();

                    if (mObjInsertUpdateLandPurpose != null)
                    {
                        mObjInsertUpdateLandPurpose.Active = !mObjInsertUpdateLandPurpose.Active;
                        mObjInsertUpdateLandPurpose.ModifiedBy = pObjLandPurpose.ModifiedBy;
                        mObjInsertUpdateLandPurpose.ModifiedDate = pObjLandPurpose.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Land Purpose Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetLandPurposeList(pObjLandPurpose.LandPurposeName, 0, pObjLandPurpose.LandPurposeIds, pObjLandPurpose.intStatus, pObjLandPurpose.IncludeLandPurposeIds, pObjLandPurpose.ExcludeLandPurposeIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Land Purpose Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
