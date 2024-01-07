using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class BuildingOwnershipRepository : IBuildingOwnershipRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateBuildingOwnership(Building_Ownership pObjBuildingOwnership)
        {
            using (_db = new EIRSEntities())
            {
                Building_Ownership mObjInsertUpdateBuildingOwnership; //Building Ownership Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from bowner in _db.Building_Ownership
                                       where bowner.BuildingOwnershipName == pObjBuildingOwnership.BuildingOwnershipName && bowner.BuildingOwnershipID != pObjBuildingOwnership.BuildingOwnershipID
                                       select bowner);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Building Ownership already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Building Ownership
                if (pObjBuildingOwnership.BuildingOwnershipID != 0)
                {
                    mObjInsertUpdateBuildingOwnership = (from BuildingOwnership in _db.Building_Ownership
                                                 where BuildingOwnership.BuildingOwnershipID == pObjBuildingOwnership.BuildingOwnershipID
                                                 select BuildingOwnership).FirstOrDefault();

                    if (mObjInsertUpdateBuildingOwnership != null)
                    {
                        mObjInsertUpdateBuildingOwnership.ModifiedBy = pObjBuildingOwnership.ModifiedBy;
                        mObjInsertUpdateBuildingOwnership.ModifiedDate = pObjBuildingOwnership.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateBuildingOwnership = new Building_Ownership();
                        mObjInsertUpdateBuildingOwnership.CreatedBy = pObjBuildingOwnership.CreatedBy;
                        mObjInsertUpdateBuildingOwnership.CreatedDate = pObjBuildingOwnership.CreatedDate;
                    }
                }
                else // Else Insert Building Ownership
                {
                    mObjInsertUpdateBuildingOwnership = new Building_Ownership();
                    mObjInsertUpdateBuildingOwnership.CreatedBy = pObjBuildingOwnership.CreatedBy;
                    mObjInsertUpdateBuildingOwnership.CreatedDate = pObjBuildingOwnership.CreatedDate;
                }

                mObjInsertUpdateBuildingOwnership.BuildingOwnershipName = pObjBuildingOwnership.BuildingOwnershipName;
                mObjInsertUpdateBuildingOwnership.Active = pObjBuildingOwnership.Active;

                if (pObjBuildingOwnership.BuildingOwnershipID == 0)
                {
                    _db.Building_Ownership.Add(mObjInsertUpdateBuildingOwnership);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjBuildingOwnership.BuildingOwnershipID == 0)
                        mObjFuncResponse.Message = "Building Ownership Added Successfully";
                    else
                        mObjFuncResponse.Message = "Building Ownership Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjBuildingOwnership.BuildingOwnershipID == 0)
                        mObjFuncResponse.Message = "Building Ownership Addition Failed";
                    else
                        mObjFuncResponse.Message = "Building Ownership Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetBuildingOwnershipList_Result> REP_GetBuildingOwnershipList(Building_Ownership pObjBuildingOwnership)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBuildingOwnershipList(pObjBuildingOwnership.BuildingOwnershipName, pObjBuildingOwnership.BuildingOwnershipID, pObjBuildingOwnership.BuildingOwnershipIds, pObjBuildingOwnership.intStatus, pObjBuildingOwnership.IncludeBuildingOwnershipIds, pObjBuildingOwnership.ExcludeBuildingOwnershipIds).ToList();
            }
        }

        public usp_GetBuildingOwnershipList_Result REP_GetBuildingOwnershipDetails(Building_Ownership pObjBuildingOwnership)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBuildingOwnershipList(pObjBuildingOwnership.BuildingOwnershipName, pObjBuildingOwnership.BuildingOwnershipID, pObjBuildingOwnership.BuildingOwnershipIds, pObjBuildingOwnership.intStatus, pObjBuildingOwnership.IncludeBuildingOwnershipIds, pObjBuildingOwnership.ExcludeBuildingOwnershipIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetBuildingOwnershipDropDownList(Building_Ownership pObjBuildingOwnership)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from bowner in _db.usp_GetBuildingOwnershipList(pObjBuildingOwnership.BuildingOwnershipName, pObjBuildingOwnership.BuildingOwnershipID, pObjBuildingOwnership.BuildingOwnershipIds, pObjBuildingOwnership.intStatus, pObjBuildingOwnership.IncludeBuildingOwnershipIds, pObjBuildingOwnership.ExcludeBuildingOwnershipIds)
                               select new DropDownListResult()
                               {
                                   id = bowner.BuildingOwnershipID.GetValueOrDefault(),
                                   text = bowner.BuildingOwnershipName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Building_Ownership pObjBuildingOwnership)
        {
            using (_db = new EIRSEntities())
            {
                Building_Ownership mObjInsertUpdateBuildingOwnership; //Building Ownership Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load BuildingOwnership
                if (pObjBuildingOwnership.BuildingOwnershipID != 0)
                {
                    mObjInsertUpdateBuildingOwnership = (from bowner in _db.Building_Ownership
                                                 where bowner.BuildingOwnershipID == pObjBuildingOwnership.BuildingOwnershipID
                                                 select bowner).FirstOrDefault();

                    if (mObjInsertUpdateBuildingOwnership != null)
                    {
                        mObjInsertUpdateBuildingOwnership.Active = !mObjInsertUpdateBuildingOwnership.Active;
                        mObjInsertUpdateBuildingOwnership.ModifiedBy = pObjBuildingOwnership.ModifiedBy;
                        mObjInsertUpdateBuildingOwnership.ModifiedDate = pObjBuildingOwnership.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Building Ownership Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetBuildingOwnershipList(pObjBuildingOwnership.BuildingOwnershipName, 0, pObjBuildingOwnership.BuildingOwnershipIds, pObjBuildingOwnership.intStatus, pObjBuildingOwnership.IncludeBuildingOwnershipIds, pObjBuildingOwnership.ExcludeBuildingOwnershipIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Building Ownership Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
