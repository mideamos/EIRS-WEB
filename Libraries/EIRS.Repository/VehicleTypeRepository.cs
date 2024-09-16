using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class VehicleTypeRepository : IVehicleTypeRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateVehicleType(Vehicle_Types pObjVehicleType)
        {
            using (_db = new EIRSEntities())
            {
                Vehicle_Types mObjInsertUpdateVehicleType; //Vehicle Type Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from vtype in _db.Vehicle_Types
                                       where vtype.VehicleTypeName == pObjVehicleType.VehicleTypeName && vtype.VehicleTypeID != pObjVehicleType.VehicleTypeID
                                       select vtype);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Vehicle Type already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Vehicle Type
                if (pObjVehicleType.VehicleTypeID != 0)
                {
                    mObjInsertUpdateVehicleType = (from vtype in _db.Vehicle_Types
                                                 where vtype.VehicleTypeID == pObjVehicleType.VehicleTypeID
                                                 select vtype).FirstOrDefault();

                    if (mObjInsertUpdateVehicleType != null)
                    {
                        mObjInsertUpdateVehicleType.ModifiedBy = pObjVehicleType.ModifiedBy;
                        mObjInsertUpdateVehicleType.ModifiedDate = pObjVehicleType.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateVehicleType = new Vehicle_Types();
                        mObjInsertUpdateVehicleType.CreatedBy = pObjVehicleType.CreatedBy;
                        mObjInsertUpdateVehicleType.CreatedDate = pObjVehicleType.CreatedDate;
                    }
                }
                else // Else Insert Vehicle Type
                {
                    mObjInsertUpdateVehicleType = new Vehicle_Types();
                    mObjInsertUpdateVehicleType.CreatedBy = pObjVehicleType.CreatedBy;
                    mObjInsertUpdateVehicleType.CreatedDate = pObjVehicleType.CreatedDate;
                }

                mObjInsertUpdateVehicleType.VehicleTypeName = pObjVehicleType.VehicleTypeName;
                mObjInsertUpdateVehicleType.Active = pObjVehicleType.Active;

                if (pObjVehicleType.VehicleTypeID == 0)
                {
                    _db.Vehicle_Types.Add(mObjInsertUpdateVehicleType);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjVehicleType.VehicleTypeID == 0)
                        mObjFuncResponse.Message = "Vehicle Type Added Successfully";
                    else
                        mObjFuncResponse.Message = "Vehicle Type Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjVehicleType.VehicleTypeID == 0)
                        mObjFuncResponse.Message = "Vehicle Type Addition Failed";
                    else
                        mObjFuncResponse.Message = "Vehicle Type Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetVehicleTypeList_Result> REP_GetVehicleTypeList(Vehicle_Types pObjVehicleType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehicleTypeList(pObjVehicleType.VehicleTypeName, pObjVehicleType.VehicleTypeID, pObjVehicleType.VehicleTypeIds, pObjVehicleType.intStatus, pObjVehicleType.IncludeVehicleTypeIds, pObjVehicleType.ExcludeVehicleTypeIds).ToList();
            }
        }

        public usp_GetVehicleTypeList_Result REP_GetVehicleTypeDetails(Vehicle_Types pObjVehicleType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehicleTypeList(pObjVehicleType.VehicleTypeName, pObjVehicleType.VehicleTypeID, pObjVehicleType.VehicleTypeIds, pObjVehicleType.intStatus, pObjVehicleType.IncludeVehicleTypeIds, pObjVehicleType.ExcludeVehicleTypeIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetVehicleTypeDropDownList(Vehicle_Types pObjVehicleType)
        {
            using (_db = new EIRSEntities())
            {
                //var vResult = (from vtype in _db.usp_GetVehicleTypeList(pObjVehicleType.VehicleTypeName, pObjVehicleType.VehicleTypeID, pObjVehicleType.VehicleTypeIds, pObjVehicleType.intStatus, pObjVehicleType.IncludeVehicleTypeIds, pObjVehicleType.ExcludeVehicleTypeIds)
                //               select new DropDownListResult()
                //               {
                //                   id = vtype.VehicleTypeID.GetValueOrDefault(),
                //                   text = vtype.VehicleTypeName
                //               }).ToList();
                var vResult = from pro in _db.Vehicle_Types
                              select new DropDownListResult() { id = pro.VehicleTypeID, text = pro.VehicleTypeName };


                return vResult.ToList();
            }
        }

        public FuncResponse REP_UpdateStatus(Vehicle_Types pObjVehicleType)
        {
            using (_db = new EIRSEntities())
            {
                Vehicle_Types mObjInsertUpdateVehicleType; //Vehicle Type Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load VehicleType
                if (pObjVehicleType.VehicleTypeID != 0)
                {
                    mObjInsertUpdateVehicleType = (from vtype in _db.Vehicle_Types
                                                 where vtype.VehicleTypeID == pObjVehicleType.VehicleTypeID
                                                 select vtype).FirstOrDefault();

                    if (mObjInsertUpdateVehicleType != null)
                    {
                        mObjInsertUpdateVehicleType.Active = !mObjInsertUpdateVehicleType.Active;
                        mObjInsertUpdateVehicleType.ModifiedBy = pObjVehicleType.ModifiedBy;
                        mObjInsertUpdateVehicleType.ModifiedDate = pObjVehicleType.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Vehicle Type Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetVehicleTypeList(pObjVehicleType.VehicleTypeName, 0, pObjVehicleType.VehicleTypeIds, pObjVehicleType.intStatus, pObjVehicleType.IncludeVehicleTypeIds, pObjVehicleType.ExcludeVehicleTypeIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Vehicle Type Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
