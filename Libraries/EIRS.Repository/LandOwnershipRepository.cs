using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class LandOwnershipRepository : ILandOwnershipRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateLandOwnership(Land_Ownership pObjLandOwnership)
        {
            using (_db = new EIRSEntities())
            {
                Land_Ownership mObjInsertUpdateLandOwnership; //Land Ownership Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from bowner in _db.Land_Ownership
                                       where bowner.LandOwnershipName == pObjLandOwnership.LandOwnershipName && bowner.LandOwnershipID != pObjLandOwnership.LandOwnershipID
                                       select bowner);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Land Ownership already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Land Ownership
                if (pObjLandOwnership.LandOwnershipID != 0)
                {
                    mObjInsertUpdateLandOwnership = (from LandOwnership in _db.Land_Ownership
                                                 where LandOwnership.LandOwnershipID == pObjLandOwnership.LandOwnershipID
                                                 select LandOwnership).FirstOrDefault();

                    if (mObjInsertUpdateLandOwnership != null)
                    {
                        mObjInsertUpdateLandOwnership.ModifiedBy = pObjLandOwnership.ModifiedBy;
                        mObjInsertUpdateLandOwnership.ModifiedDate = pObjLandOwnership.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateLandOwnership = new Land_Ownership();
                        mObjInsertUpdateLandOwnership.CreatedBy = pObjLandOwnership.CreatedBy;
                        mObjInsertUpdateLandOwnership.CreatedDate = pObjLandOwnership.CreatedDate;
                    }
                }
                else // Else Insert Land Ownership
                {
                    mObjInsertUpdateLandOwnership = new Land_Ownership();
                    mObjInsertUpdateLandOwnership.CreatedBy = pObjLandOwnership.CreatedBy;
                    mObjInsertUpdateLandOwnership.CreatedDate = pObjLandOwnership.CreatedDate;
                }

                mObjInsertUpdateLandOwnership.LandOwnershipName = pObjLandOwnership.LandOwnershipName;
                mObjInsertUpdateLandOwnership.Active = pObjLandOwnership.Active;

                if (pObjLandOwnership.LandOwnershipID == 0)
                {
                    _db.Land_Ownership.Add(mObjInsertUpdateLandOwnership);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjLandOwnership.LandOwnershipID == 0)
                        mObjFuncResponse.Message = "Land Ownership Added Successfully";
                    else
                        mObjFuncResponse.Message = "Land Ownership Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjLandOwnership.LandOwnershipID == 0)
                        mObjFuncResponse.Message = "Land Ownership Addition Failed";
                    else
                        mObjFuncResponse.Message = "Land Ownership Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetLandOwnershipList_Result> REP_GetLandOwnershipList(Land_Ownership pObjLandOwnership)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetLandOwnershipList(pObjLandOwnership.LandOwnershipName, pObjLandOwnership.LandOwnershipID, pObjLandOwnership.LandOwnershipIds, pObjLandOwnership.intStatus, pObjLandOwnership.IncludeLandOwnershipIds, pObjLandOwnership.ExcludeLandOwnershipIds).ToList();
            }
        }

        public usp_GetLandOwnershipList_Result REP_GetLandOwnershipDetails(Land_Ownership pObjLandOwnership)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetLandOwnershipList(pObjLandOwnership.LandOwnershipName, pObjLandOwnership.LandOwnershipID, pObjLandOwnership.LandOwnershipIds, pObjLandOwnership.intStatus, pObjLandOwnership.IncludeLandOwnershipIds, pObjLandOwnership.ExcludeLandOwnershipIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetLandOwnershipDropDownList(Land_Ownership pObjLandOwnership)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from bowner in _db.usp_GetLandOwnershipList(pObjLandOwnership.LandOwnershipName, pObjLandOwnership.LandOwnershipID, pObjLandOwnership.LandOwnershipIds, pObjLandOwnership.intStatus, pObjLandOwnership.IncludeLandOwnershipIds, pObjLandOwnership.ExcludeLandOwnershipIds)
                               select new DropDownListResult()
                               {
                                   id = bowner.LandOwnershipID.GetValueOrDefault(),
                                   text = bowner.LandOwnershipName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Land_Ownership pObjLandOwnership)
        {
            using (_db = new EIRSEntities())
            {
                Land_Ownership mObjInsertUpdateLandOwnership; //Land Ownership Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load LandOwnership
                if (pObjLandOwnership.LandOwnershipID != 0)
                {
                    mObjInsertUpdateLandOwnership = (from bowner in _db.Land_Ownership
                                                 where bowner.LandOwnershipID == pObjLandOwnership.LandOwnershipID
                                                 select bowner).FirstOrDefault();

                    if (mObjInsertUpdateLandOwnership != null)
                    {
                        mObjInsertUpdateLandOwnership.Active = !mObjInsertUpdateLandOwnership.Active;
                        mObjInsertUpdateLandOwnership.ModifiedBy = pObjLandOwnership.ModifiedBy;
                        mObjInsertUpdateLandOwnership.ModifiedDate = pObjLandOwnership.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Land Ownership Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetLandOwnershipList(pObjLandOwnership.LandOwnershipName, 0, pObjLandOwnership.LandOwnershipIds, pObjLandOwnership.intStatus, pObjLandOwnership.IncludeLandOwnershipIds, pObjLandOwnership.ExcludeLandOwnershipIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Land Ownership Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
