using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class BuildingPurposeRepository : IBuildingPurposeRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateBuildingPurpose(Building_Purpose pObjBuildingPurpose)
        {
            using (_db = new EIRSEntities())
            {
                Building_Purpose mObjInsertUpdateBuildingPurpose; //Building Purpose Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from bpurp in _db.Building_Purpose
                                       where bpurp.BuildingPurposeName == pObjBuildingPurpose.BuildingPurposeName && bpurp.BuildingPurposeID != pObjBuildingPurpose.BuildingPurposeID
                                       select bpurp);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Building Purpose already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Building Purpose
                if (pObjBuildingPurpose.BuildingPurposeID != 0)
                {
                    mObjInsertUpdateBuildingPurpose = (from BuildingPurpose in _db.Building_Purpose
                                                 where BuildingPurpose.BuildingPurposeID == pObjBuildingPurpose.BuildingPurposeID
                                                 select BuildingPurpose).FirstOrDefault();

                    if (mObjInsertUpdateBuildingPurpose != null)
                    {
                        mObjInsertUpdateBuildingPurpose.ModifiedBy = pObjBuildingPurpose.ModifiedBy;
                        mObjInsertUpdateBuildingPurpose.ModifiedDate = pObjBuildingPurpose.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateBuildingPurpose = new Building_Purpose();
                        mObjInsertUpdateBuildingPurpose.CreatedBy = pObjBuildingPurpose.CreatedBy;
                        mObjInsertUpdateBuildingPurpose.CreatedDate = pObjBuildingPurpose.CreatedDate;
                    }
                }
                else // Else Insert Building Purpose
                {
                    mObjInsertUpdateBuildingPurpose = new Building_Purpose();
                    mObjInsertUpdateBuildingPurpose.CreatedBy = pObjBuildingPurpose.CreatedBy;
                    mObjInsertUpdateBuildingPurpose.CreatedDate = pObjBuildingPurpose.CreatedDate;
                }

                mObjInsertUpdateBuildingPurpose.BuildingPurposeName = pObjBuildingPurpose.BuildingPurposeName;
                mObjInsertUpdateBuildingPurpose.Active = pObjBuildingPurpose.Active;

                if (pObjBuildingPurpose.BuildingPurposeID == 0)
                {
                    _db.Building_Purpose.Add(mObjInsertUpdateBuildingPurpose);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjBuildingPurpose.BuildingPurposeID == 0)
                        mObjFuncResponse.Message = "Building Purpose Added Successfully";
                    else
                        mObjFuncResponse.Message = "Building Purpose Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjBuildingPurpose.BuildingPurposeID == 0)
                        mObjFuncResponse.Message = "Building Purpose Addition Failed";
                    else
                        mObjFuncResponse.Message = "Building Purpose Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetBuildingPurposeList_Result> REP_GetBuildingPurposeList(Building_Purpose pObjBuildingPurpose)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBuildingPurposeList(pObjBuildingPurpose.BuildingPurposeName, pObjBuildingPurpose.BuildingPurposeID, pObjBuildingPurpose.BuildingPurposeIds, pObjBuildingPurpose.intStatus, pObjBuildingPurpose.IncludeBuildingPurposeIds, pObjBuildingPurpose.ExcludeBuildingPurposeIds).ToList();
            }
        }

        public usp_GetBuildingPurposeList_Result REP_GetBuildingPurposeDetails(Building_Purpose pObjBuildingPurpose)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBuildingPurposeList(pObjBuildingPurpose.BuildingPurposeName, pObjBuildingPurpose.BuildingPurposeID, pObjBuildingPurpose.BuildingPurposeIds, pObjBuildingPurpose.intStatus, pObjBuildingPurpose.IncludeBuildingPurposeIds, pObjBuildingPurpose.ExcludeBuildingPurposeIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetBuildingPurposeDropDownList(Building_Purpose pObjBuildingPurpose)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from bpurp in _db.usp_GetBuildingPurposeList(pObjBuildingPurpose.BuildingPurposeName, pObjBuildingPurpose.BuildingPurposeID, pObjBuildingPurpose.BuildingPurposeIds, pObjBuildingPurpose.intStatus, pObjBuildingPurpose.IncludeBuildingPurposeIds, pObjBuildingPurpose.ExcludeBuildingPurposeIds)
                               select new DropDownListResult()
                               {
                                   id = bpurp.BuildingPurposeID.GetValueOrDefault(),
                                   text = bpurp.BuildingPurposeName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Building_Purpose pObjBuildingPurpose)
        {
            using (_db = new EIRSEntities())
            {
                Building_Purpose mObjInsertUpdateBuildingPurpose; //Building Purpose Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load BuildingPurpose
                if (pObjBuildingPurpose.BuildingPurposeID != 0)
                {
                    mObjInsertUpdateBuildingPurpose = (from bpurp in _db.Building_Purpose
                                                 where bpurp.BuildingPurposeID == pObjBuildingPurpose.BuildingPurposeID
                                                 select bpurp).FirstOrDefault();

                    if (mObjInsertUpdateBuildingPurpose != null)
                    {
                        mObjInsertUpdateBuildingPurpose.Active = !mObjInsertUpdateBuildingPurpose.Active;
                        mObjInsertUpdateBuildingPurpose.ModifiedBy = pObjBuildingPurpose.ModifiedBy;
                        mObjInsertUpdateBuildingPurpose.ModifiedDate = pObjBuildingPurpose.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Building Purpose Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetBuildingPurposeList(pObjBuildingPurpose.BuildingPurposeName, 0, pObjBuildingPurpose.BuildingPurposeIds, pObjBuildingPurpose.intStatus, pObjBuildingPurpose.IncludeBuildingPurposeIds, pObjBuildingPurpose.ExcludeBuildingPurposeIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Building Purpose Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
