using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class VehicleSubTypeRepository : IVehicleSubTypeRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateVehicleSubType(Vehicle_SubTypes pObjVehicleSubType)
        {
            using (_db = new EIRSEntities())
            {
                Vehicle_SubTypes mObjInsertUpdateVehicleSubType; //Vehicle Sub Type Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from vstype in _db.Vehicle_SubTypes
                                       where vstype.VehicleSubTypeName == pObjVehicleSubType.VehicleSubTypeName && vstype.VehicleTypeID == pObjVehicleSubType.VehicleTypeID && vstype.VehicleSubTypeID != pObjVehicleSubType.VehicleSubTypeID
                                       select vstype);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Vehicle Sub Type already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Vehicle Sub Type
                if (pObjVehicleSubType.VehicleSubTypeID != 0)
                {
                    mObjInsertUpdateVehicleSubType = (from vstype in _db.Vehicle_SubTypes
                                                 where vstype.VehicleSubTypeID == pObjVehicleSubType.VehicleSubTypeID
                                                 select vstype).FirstOrDefault();

                    if (mObjInsertUpdateVehicleSubType != null)
                    {
                        mObjInsertUpdateVehicleSubType.ModifiedBy = pObjVehicleSubType.ModifiedBy;
                        mObjInsertUpdateVehicleSubType.ModifiedDate = pObjVehicleSubType.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateVehicleSubType = new Vehicle_SubTypes();
                        mObjInsertUpdateVehicleSubType.CreatedBy = pObjVehicleSubType.CreatedBy;
                        mObjInsertUpdateVehicleSubType.CreatedDate = pObjVehicleSubType.CreatedDate;
                    }
                }
                else // Else Insert Vehicle Sub Type
                {
                    mObjInsertUpdateVehicleSubType = new Vehicle_SubTypes();
                    mObjInsertUpdateVehicleSubType.CreatedBy = pObjVehicleSubType.CreatedBy;
                    mObjInsertUpdateVehicleSubType.CreatedDate = pObjVehicleSubType.CreatedDate;
                }

                mObjInsertUpdateVehicleSubType.VehicleSubTypeName = pObjVehicleSubType.VehicleSubTypeName;
                mObjInsertUpdateVehicleSubType.VehicleTypeID = pObjVehicleSubType.VehicleTypeID;
                mObjInsertUpdateVehicleSubType.Active = pObjVehicleSubType.Active;

                if (pObjVehicleSubType.VehicleSubTypeID == 0)
                {
                    _db.Vehicle_SubTypes.Add(mObjInsertUpdateVehicleSubType);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjVehicleSubType.VehicleSubTypeID == 0)
                        mObjFuncResponse.Message = "Vehicle Sub Type Added Successfully";
                    else
                        mObjFuncResponse.Message = "Vehicle Sub Type Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjVehicleSubType.VehicleSubTypeID == 0)
                        mObjFuncResponse.Message = "Vehicle Sub Type Addition Failed";
                    else
                        mObjFuncResponse.Message = "Vehicle Sub Type Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetVehicleSubTypeList_Result> REP_GetVehicleSubTypeList(Vehicle_SubTypes pObjVehicleSubType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehicleSubTypeList(pObjVehicleSubType.VehicleSubTypeName, pObjVehicleSubType.VehicleSubTypeID, pObjVehicleSubType.VehicleTypeID, pObjVehicleSubType.VehicleSubTypeIds, pObjVehicleSubType.intStatus, pObjVehicleSubType.IncludeVehicleSubTypeIds, pObjVehicleSubType.ExcludeVehicleSubTypeIds).ToList();
            }
        }

        public usp_GetVehicleSubTypeList_Result REP_GetVehicleSubTypeDetails(Vehicle_SubTypes pObjVehicleSubType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehicleSubTypeList(pObjVehicleSubType.VehicleSubTypeName, pObjVehicleSubType.VehicleSubTypeID, pObjVehicleSubType.VehicleTypeID, pObjVehicleSubType.VehicleSubTypeIds, pObjVehicleSubType.intStatus, pObjVehicleSubType.IncludeVehicleSubTypeIds, pObjVehicleSubType.ExcludeVehicleSubTypeIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetVehicleSubTypeDropDownList(Vehicle_SubTypes pObjVehicleSubType)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from vstype in _db.usp_GetVehicleSubTypeList(pObjVehicleSubType.VehicleSubTypeName, pObjVehicleSubType.VehicleSubTypeID, pObjVehicleSubType.VehicleTypeID, pObjVehicleSubType.VehicleSubTypeIds, pObjVehicleSubType.intStatus, pObjVehicleSubType.IncludeVehicleSubTypeIds, pObjVehicleSubType.ExcludeVehicleSubTypeIds)
                               select new DropDownListResult()
                               {
                                   id = vstype.VehicleSubTypeID.GetValueOrDefault(),
                                   text = vstype.VehicleSubTypeName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Vehicle_SubTypes pObjVehicleSubType)
        {
            using (_db = new EIRSEntities())
            {
                Vehicle_SubTypes mObjInsertUpdateVehicleSubType; //Vehicle Sub Type Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load VehicleSubType
                if (pObjVehicleSubType.VehicleSubTypeID != 0)
                {
                    mObjInsertUpdateVehicleSubType = (from vstype in _db.Vehicle_SubTypes
                                                 where vstype.VehicleSubTypeID == pObjVehicleSubType.VehicleSubTypeID
                                                 select vstype).FirstOrDefault();

                    if (mObjInsertUpdateVehicleSubType != null)
                    {
                        mObjInsertUpdateVehicleSubType.Active = !mObjInsertUpdateVehicleSubType.Active;
                        mObjInsertUpdateVehicleSubType.ModifiedBy = pObjVehicleSubType.ModifiedBy;
                        mObjInsertUpdateVehicleSubType.ModifiedDate = pObjVehicleSubType.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Vehicle Sub Type Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetVehicleSubTypeList(pObjVehicleSubType.VehicleSubTypeName, 0, pObjVehicleSubType.VehicleTypeID, pObjVehicleSubType.VehicleSubTypeIds, pObjVehicleSubType.intStatus, pObjVehicleSubType.IncludeVehicleSubTypeIds, pObjVehicleSubType.ExcludeVehicleSubTypeIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Vehicle Sub Type Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
