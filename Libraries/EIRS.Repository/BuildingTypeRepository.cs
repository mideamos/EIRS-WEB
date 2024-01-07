using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class BuildingTypeRepository : IBuildingTypeRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateBuildingType(Building_Types pObjBuildingType)
        {
            using (_db = new EIRSEntities())
            {
                Building_Types mObjInsertUpdateBuildingType; //Building Type Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from BuildingType in _db.Building_Types
                                       where BuildingType.BuildingTypeName == pObjBuildingType.BuildingTypeName && BuildingType.BuildingTypeID != pObjBuildingType.BuildingTypeID
                                       select BuildingType);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Building Type already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Building Type
                if (pObjBuildingType.BuildingTypeID != 0)
                {
                    mObjInsertUpdateBuildingType = (from BuildingType in _db.Building_Types
                                                 where BuildingType.BuildingTypeID == pObjBuildingType.BuildingTypeID
                                                 select BuildingType).FirstOrDefault();

                    if (mObjInsertUpdateBuildingType != null)
                    {
                        mObjInsertUpdateBuildingType.ModifiedBy = pObjBuildingType.ModifiedBy;
                        mObjInsertUpdateBuildingType.ModifiedDate = pObjBuildingType.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateBuildingType = new Building_Types();
                        mObjInsertUpdateBuildingType.CreatedBy = pObjBuildingType.CreatedBy;
                        mObjInsertUpdateBuildingType.CreatedDate = pObjBuildingType.CreatedDate;
                    }
                }
                else // Else Insert Building Type
                {
                    mObjInsertUpdateBuildingType = new Building_Types();
                    mObjInsertUpdateBuildingType.CreatedBy = pObjBuildingType.CreatedBy;
                    mObjInsertUpdateBuildingType.CreatedDate = pObjBuildingType.CreatedDate;
                }

                mObjInsertUpdateBuildingType.BuildingTypeName = pObjBuildingType.BuildingTypeName;
                mObjInsertUpdateBuildingType.Active = pObjBuildingType.Active;

                if (pObjBuildingType.BuildingTypeID == 0)
                {
                    _db.Building_Types.Add(mObjInsertUpdateBuildingType);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjBuildingType.BuildingTypeID == 0)
                        mObjFuncResponse.Message = "Building Type Added Successfully";
                    else
                        mObjFuncResponse.Message = "Building Type Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjBuildingType.BuildingTypeID == 0)
                        mObjFuncResponse.Message = "Building Type Addition Failed";
                    else
                        mObjFuncResponse.Message = "Building Type Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetBuildingTypeList_Result> REP_GetBuildingTypeList(Building_Types pObjBuildingType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBuildingTypeList(pObjBuildingType.BuildingTypeName, pObjBuildingType.BuildingTypeID, pObjBuildingType.BuildingTypeIds, pObjBuildingType.intStatus, pObjBuildingType.IncludeBuildingTypeIds, pObjBuildingType.ExcludeBuildingTypeIds).ToList();
            }
        }

        public usp_GetBuildingTypeList_Result REP_GetBuildingTypeDetails(Building_Types pObjBuildingType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBuildingTypeList(pObjBuildingType.BuildingTypeName, pObjBuildingType.BuildingTypeID, pObjBuildingType.BuildingTypeIds, pObjBuildingType.intStatus, pObjBuildingType.IncludeBuildingTypeIds, pObjBuildingType.ExcludeBuildingTypeIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetBuildingTypeDropDownList(Building_Types pObjBuildingType)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from BuildingType in _db.usp_GetBuildingTypeList(pObjBuildingType.BuildingTypeName, pObjBuildingType.BuildingTypeID, pObjBuildingType.BuildingTypeIds, pObjBuildingType.intStatus, pObjBuildingType.IncludeBuildingTypeIds, pObjBuildingType.ExcludeBuildingTypeIds)
                               select new DropDownListResult()
                               {
                                   id = BuildingType.BuildingTypeID.GetValueOrDefault(),
                                   text = BuildingType.BuildingTypeName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Building_Types pObjBuildingType)
        {
            using (_db = new EIRSEntities())
            {
                Building_Types mObjInsertUpdateBuildingType; //Building Type Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load BuildingType
                if (pObjBuildingType.BuildingTypeID != 0)
                {
                    mObjInsertUpdateBuildingType = (from BuildingType in _db.Building_Types
                                                 where BuildingType.BuildingTypeID == pObjBuildingType.BuildingTypeID
                                                 select BuildingType).FirstOrDefault();

                    if (mObjInsertUpdateBuildingType != null)
                    {
                        mObjInsertUpdateBuildingType.Active = !mObjInsertUpdateBuildingType.Active;
                        mObjInsertUpdateBuildingType.ModifiedBy = pObjBuildingType.ModifiedBy;
                        mObjInsertUpdateBuildingType.ModifiedDate = pObjBuildingType.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Building Type Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetBuildingTypeList(pObjBuildingType.BuildingTypeName, 0, pObjBuildingType.BuildingTypeIds, pObjBuildingType.intStatus, pObjBuildingType.IncludeBuildingTypeIds, pObjBuildingType.ExcludeBuildingTypeIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Building Type Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
