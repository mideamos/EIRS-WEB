using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class AssetTypeRepository : IAssetTypeRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateAssetType(Asset_Types pObjAssetType)
        {
            using (_db = new EIRSEntities())
            {
                Asset_Types mObjInsertUpdateAssetType; //Asset Type Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from AssetType in _db.Asset_Types
                                       where AssetType.AssetTypeName == pObjAssetType.AssetTypeName && AssetType.AssetTypeID != pObjAssetType.AssetTypeID
                                       select AssetType);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Asset Type already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Asset Type
                if (pObjAssetType.AssetTypeID != 0)
                {
                    mObjInsertUpdateAssetType = (from AssetType in _db.Asset_Types
                                                 where AssetType.AssetTypeID == pObjAssetType.AssetTypeID
                                                 select AssetType).FirstOrDefault();

                    if (mObjInsertUpdateAssetType != null)
                    {
                        mObjInsertUpdateAssetType.ModifiedBy = pObjAssetType.ModifiedBy;
                        mObjInsertUpdateAssetType.ModifiedDate = pObjAssetType.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateAssetType = new Asset_Types();
                        mObjInsertUpdateAssetType.CreatedBy = pObjAssetType.CreatedBy;
                        mObjInsertUpdateAssetType.CreatedDate = pObjAssetType.CreatedDate;
                    }
                }
                else // Else Insert Asset Type
                {
                    mObjInsertUpdateAssetType = new Asset_Types();
                    mObjInsertUpdateAssetType.CreatedBy = pObjAssetType.CreatedBy;
                    mObjInsertUpdateAssetType.CreatedDate = pObjAssetType.CreatedDate;
                }

                mObjInsertUpdateAssetType.AssetTypeName = pObjAssetType.AssetTypeName;
                mObjInsertUpdateAssetType.Active = pObjAssetType.Active;

                if (pObjAssetType.AssetTypeID == 0)
                {
                    _db.Asset_Types.Add(mObjInsertUpdateAssetType);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjAssetType.AssetTypeID == 0)
                        mObjFuncResponse.Message = "Asset Type Added Successfully";
                    else
                        mObjFuncResponse.Message = "Asset Type Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjAssetType.AssetTypeID == 0)
                        mObjFuncResponse.Message = "Asset Type Addition Failed";
                    else
                        mObjFuncResponse.Message = "Asset Type Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetAssetTypeList_Result> REP_GetAssetTypeList(Asset_Types pObjAssetType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssetTypeList(pObjAssetType.AssetTypeName, pObjAssetType.AssetTypeID, pObjAssetType.AssetTypeIds, pObjAssetType.intStatus, pObjAssetType.IncludeAssetTypeIds, pObjAssetType.ExcludeAssetTypeIds).ToList();
            }
        }

        public usp_GetAssetTypeList_Result REP_GetAssetTypeDetails(Asset_Types pObjAssetType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssetTypeList(pObjAssetType.AssetTypeName, pObjAssetType.AssetTypeID, pObjAssetType.AssetTypeIds, pObjAssetType.intStatus, pObjAssetType.IncludeAssetTypeIds, pObjAssetType.ExcludeAssetTypeIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetAssetTypeDropDownList(Asset_Types pObjAssetType)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from AssetType in _db.usp_GetAssetTypeList(pObjAssetType.AssetTypeName, pObjAssetType.AssetTypeID, pObjAssetType.AssetTypeIds, pObjAssetType.intStatus, pObjAssetType.IncludeAssetTypeIds, pObjAssetType.ExcludeAssetTypeIds)
                               select new DropDownListResult()
                               {
                                   id = AssetType.AssetTypeID.GetValueOrDefault(),
                                   text = AssetType.AssetTypeName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Asset_Types pObjAssetType)
        {
            using (_db = new EIRSEntities())
            {
                Asset_Types mObjInsertUpdateAssetType; //Asset Type Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load AssetType
                if (pObjAssetType.AssetTypeID != 0)
                {
                    mObjInsertUpdateAssetType = (from AssetType in _db.Asset_Types
                                                 where AssetType.AssetTypeID == pObjAssetType.AssetTypeID
                                                 select AssetType).FirstOrDefault();

                    if (mObjInsertUpdateAssetType != null)
                    {
                        mObjInsertUpdateAssetType.Active = !mObjInsertUpdateAssetType.Active;
                        mObjInsertUpdateAssetType.ModifiedBy = pObjAssetType.ModifiedBy;
                        mObjInsertUpdateAssetType.ModifiedDate = pObjAssetType.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Asset Type Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetAssetTypeList(pObjAssetType.AssetTypeName, 0, pObjAssetType.AssetTypeIds, pObjAssetType.intStatus, pObjAssetType.IncludeAssetTypeIds, pObjAssetType.ExcludeAssetTypeIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Asset Type Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
