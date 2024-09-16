using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class BuildingCompletionRepository : IBuildingCompletionRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateBuildingCompletion(Building_Completion pObjBuildingCompletion)
        {
            using (_db = new EIRSEntities())
            {
                Building_Completion mObjInsertUpdateBuildingCompletion; //Building Completion Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from bcomp in _db.Building_Completion
                                       where bcomp.BuildingCompletionName == pObjBuildingCompletion.BuildingCompletionName && bcomp.BuildingCompletionID != pObjBuildingCompletion.BuildingCompletionID
                                       select bcomp);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Building Completion already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Building Completion
                if (pObjBuildingCompletion.BuildingCompletionID != 0)
                {
                    mObjInsertUpdateBuildingCompletion = (from BuildingCompletion in _db.Building_Completion
                                                 where BuildingCompletion.BuildingCompletionID == pObjBuildingCompletion.BuildingCompletionID
                                                 select BuildingCompletion).FirstOrDefault();

                    if (mObjInsertUpdateBuildingCompletion != null)
                    {
                        mObjInsertUpdateBuildingCompletion.ModifiedBy = pObjBuildingCompletion.ModifiedBy;
                        mObjInsertUpdateBuildingCompletion.ModifiedDate = pObjBuildingCompletion.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateBuildingCompletion = new Building_Completion();
                        mObjInsertUpdateBuildingCompletion.CreatedBy = pObjBuildingCompletion.CreatedBy;
                        mObjInsertUpdateBuildingCompletion.CreatedDate = pObjBuildingCompletion.CreatedDate;
                    }
                }
                else // Else Insert Building Completion
                {
                    mObjInsertUpdateBuildingCompletion = new Building_Completion();
                    mObjInsertUpdateBuildingCompletion.CreatedBy = pObjBuildingCompletion.CreatedBy;
                    mObjInsertUpdateBuildingCompletion.CreatedDate = pObjBuildingCompletion.CreatedDate;
                }

                mObjInsertUpdateBuildingCompletion.BuildingCompletionName = pObjBuildingCompletion.BuildingCompletionName;
                mObjInsertUpdateBuildingCompletion.Active = pObjBuildingCompletion.Active;

                if (pObjBuildingCompletion.BuildingCompletionID == 0)
                {
                    _db.Building_Completion.Add(mObjInsertUpdateBuildingCompletion);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjBuildingCompletion.BuildingCompletionID == 0)
                        mObjFuncResponse.Message = "Building Completion Added Successfully";
                    else
                        mObjFuncResponse.Message = "Building Completion Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjBuildingCompletion.BuildingCompletionID == 0)
                        mObjFuncResponse.Message = "Building Completion Addition Failed";
                    else
                        mObjFuncResponse.Message = "Building Completion Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetBuildingCompletionList_Result> REP_GetBuildingCompletionList(Building_Completion pObjBuildingCompletion)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBuildingCompletionList(pObjBuildingCompletion.BuildingCompletionName, pObjBuildingCompletion.BuildingCompletionID, pObjBuildingCompletion.BuildingCompletionIds, pObjBuildingCompletion.intStatus, pObjBuildingCompletion.IncludeBuildingCompletionIds, pObjBuildingCompletion.ExcludeBuildingCompletionIds).ToList();
            }
        }

        public usp_GetBuildingCompletionList_Result REP_GetBuildingCompletionDetails(Building_Completion pObjBuildingCompletion)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBuildingCompletionList(pObjBuildingCompletion.BuildingCompletionName, pObjBuildingCompletion.BuildingCompletionID, pObjBuildingCompletion.BuildingCompletionIds, pObjBuildingCompletion.intStatus, pObjBuildingCompletion.IncludeBuildingCompletionIds, pObjBuildingCompletion.ExcludeBuildingCompletionIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetBuildingCompletionDropDownList(Building_Completion pObjBuildingCompletion)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from bcomp in _db.usp_GetBuildingCompletionList(pObjBuildingCompletion.BuildingCompletionName, pObjBuildingCompletion.BuildingCompletionID, pObjBuildingCompletion.BuildingCompletionIds, pObjBuildingCompletion.intStatus, pObjBuildingCompletion.IncludeBuildingCompletionIds, pObjBuildingCompletion.ExcludeBuildingCompletionIds)
                               select new DropDownListResult()
                               {
                                   id = bcomp.BuildingCompletionID.GetValueOrDefault(),
                                   text = bcomp.BuildingCompletionName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Building_Completion pObjBuildingCompletion)
        {
            using (_db = new EIRSEntities())
            {
                Building_Completion mObjInsertUpdateBuildingCompletion; //Building Completion Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load BuildingCompletion
                if (pObjBuildingCompletion.BuildingCompletionID != 0)
                {
                    mObjInsertUpdateBuildingCompletion = (from bcomp in _db.Building_Completion
                                                 where bcomp.BuildingCompletionID == pObjBuildingCompletion.BuildingCompletionID
                                                 select bcomp).FirstOrDefault();

                    if (mObjInsertUpdateBuildingCompletion != null)
                    {
                        mObjInsertUpdateBuildingCompletion.Active = !mObjInsertUpdateBuildingCompletion.Active;
                        mObjInsertUpdateBuildingCompletion.ModifiedBy = pObjBuildingCompletion.ModifiedBy;
                        mObjInsertUpdateBuildingCompletion.ModifiedDate = pObjBuildingCompletion.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Building Completion Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetBuildingCompletionList(pObjBuildingCompletion.BuildingCompletionName, 0, pObjBuildingCompletion.BuildingCompletionIds, pObjBuildingCompletion.intStatus, pObjBuildingCompletion.IncludeBuildingCompletionIds, pObjBuildingCompletion.ExcludeBuildingCompletionIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Building Completion Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
