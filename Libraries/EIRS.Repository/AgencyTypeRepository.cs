using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class AgencyTypeRepository : IAgencyTypeRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateAgencyType(Agency_Types pObjAgencyType)
        {
            using (_db = new EIRSEntities())
            {
                Agency_Types mObjInsertUpdateAgencyType; //Agency Type Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from AgencyType in _db.Agency_Types
                                       where AgencyType.AgencyTypeName == pObjAgencyType.AgencyTypeName && AgencyType.AgencyTypeID != pObjAgencyType.AgencyTypeID
                                       select AgencyType);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Agency Type already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Agency Type
                if (pObjAgencyType.AgencyTypeID != 0)
                {
                    mObjInsertUpdateAgencyType = (from AgencyType in _db.Agency_Types
                                                 where AgencyType.AgencyTypeID == pObjAgencyType.AgencyTypeID
                                                 select AgencyType).FirstOrDefault();

                    if (mObjInsertUpdateAgencyType != null)
                    {
                        mObjInsertUpdateAgencyType.ModifiedBy = pObjAgencyType.ModifiedBy;
                        mObjInsertUpdateAgencyType.ModifiedDate = pObjAgencyType.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateAgencyType = new Agency_Types();
                        mObjInsertUpdateAgencyType.CreatedBy = pObjAgencyType.CreatedBy;
                        mObjInsertUpdateAgencyType.CreatedDate = pObjAgencyType.CreatedDate;
                    }
                }
                else // Else Insert Agency Type
                {
                    mObjInsertUpdateAgencyType = new Agency_Types();
                    mObjInsertUpdateAgencyType.CreatedBy = pObjAgencyType.CreatedBy;
                    mObjInsertUpdateAgencyType.CreatedDate = pObjAgencyType.CreatedDate;
                }

                mObjInsertUpdateAgencyType.AgencyTypeName = pObjAgencyType.AgencyTypeName;
                mObjInsertUpdateAgencyType.Active = pObjAgencyType.Active;

                if (pObjAgencyType.AgencyTypeID == 0)
                {
                    _db.Agency_Types.Add(mObjInsertUpdateAgencyType);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjAgencyType.AgencyTypeID == 0)
                        mObjFuncResponse.Message = "Agency Type Added Successfully";
                    else
                        mObjFuncResponse.Message = "Agency Type Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjAgencyType.AgencyTypeID == 0)
                        mObjFuncResponse.Message = "Agency Type Addition Failed";
                    else
                        mObjFuncResponse.Message = "Agency Type Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetAgencyTypeList_Result> REP_GetAgencyTypeList(Agency_Types pObjAgencyType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAgencyTypeList(pObjAgencyType.AgencyTypeName, pObjAgencyType.AgencyTypeID, pObjAgencyType.AgencyTypeIds, pObjAgencyType.intStatus, pObjAgencyType.IncludeAgencyTypeIds, pObjAgencyType.ExcludeAgencyTypeIds).ToList();
            }
        }

        public usp_GetAgencyTypeList_Result REP_GetAgencyTypeDetails(Agency_Types pObjAgencyType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAgencyTypeList(pObjAgencyType.AgencyTypeName, pObjAgencyType.AgencyTypeID, pObjAgencyType.AgencyTypeIds, pObjAgencyType.intStatus, pObjAgencyType.IncludeAgencyTypeIds, pObjAgencyType.ExcludeAgencyTypeIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetAgencyTypeDropDownList(Agency_Types pObjAgencyType)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from AgencyType in _db.usp_GetAgencyTypeList(pObjAgencyType.AgencyTypeName, pObjAgencyType.AgencyTypeID, pObjAgencyType.AgencyTypeIds, pObjAgencyType.intStatus, pObjAgencyType.IncludeAgencyTypeIds, pObjAgencyType.ExcludeAgencyTypeIds)
                               select new DropDownListResult()
                               {
                                   id = AgencyType.AgencyTypeID.GetValueOrDefault(),
                                   text = AgencyType.AgencyTypeName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Agency_Types pObjAgencyType)
        {
            using (_db = new EIRSEntities())
            {
                Agency_Types mObjInsertUpdateAgencyType; //Agency Type Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load AgencyType
                if (pObjAgencyType.AgencyTypeID != 0)
                {
                    mObjInsertUpdateAgencyType = (from AgencyType in _db.Agency_Types
                                                 where AgencyType.AgencyTypeID == pObjAgencyType.AgencyTypeID
                                                 select AgencyType).FirstOrDefault();

                    if (mObjInsertUpdateAgencyType != null)
                    {
                        mObjInsertUpdateAgencyType.Active = !mObjInsertUpdateAgencyType.Active;
                        mObjInsertUpdateAgencyType.ModifiedBy = pObjAgencyType.ModifiedBy;
                        mObjInsertUpdateAgencyType.ModifiedDate = pObjAgencyType.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Agency Type Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetAgencyTypeList(pObjAgencyType.AgencyTypeName, 0, pObjAgencyType.AgencyTypeIds, pObjAgencyType.intStatus, pObjAgencyType.IncludeAgencyTypeIds, pObjAgencyType.ExcludeAgencyTypeIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Agency Type Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
