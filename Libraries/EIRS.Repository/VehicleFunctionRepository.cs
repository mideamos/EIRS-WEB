using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class VehicleFunctionRepository : IVehicleFunctionRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateVehicleFunction(Vehicle_Function pObjVehicleFunction)
        {
            using (_db = new EIRSEntities())
            {
                Vehicle_Function mObjInsertUpdateVehicleFunction; //Vehicle Function Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from bfunc in _db.Vehicle_Function
                                       where bfunc.VehicleFunctionName == pObjVehicleFunction.VehicleFunctionName && bfunc.VehiclePurposeID == pObjVehicleFunction.VehiclePurposeID && bfunc.VehicleFunctionID != pObjVehicleFunction.VehicleFunctionID
                                       select bfunc);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Vehicle Function already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Vehicle Function
                if (pObjVehicleFunction.VehicleFunctionID != 0)
                {
                    mObjInsertUpdateVehicleFunction = (from VehicleFunction in _db.Vehicle_Function
                                                        where VehicleFunction.VehicleFunctionID == pObjVehicleFunction.VehicleFunctionID
                                                        select VehicleFunction).FirstOrDefault();

                    if (mObjInsertUpdateVehicleFunction != null)
                    {
                        mObjInsertUpdateVehicleFunction.ModifiedBy = pObjVehicleFunction.ModifiedBy;
                        mObjInsertUpdateVehicleFunction.ModifiedDate = pObjVehicleFunction.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateVehicleFunction = new Vehicle_Function();
                        mObjInsertUpdateVehicleFunction.CreatedBy = pObjVehicleFunction.CreatedBy;
                        mObjInsertUpdateVehicleFunction.CreatedDate = pObjVehicleFunction.CreatedDate;
                    }
                }
                else // Else Insert Vehicle Function
                {
                    mObjInsertUpdateVehicleFunction = new Vehicle_Function();
                    mObjInsertUpdateVehicleFunction.CreatedBy = pObjVehicleFunction.CreatedBy;
                    mObjInsertUpdateVehicleFunction.CreatedDate = pObjVehicleFunction.CreatedDate;
                }

                mObjInsertUpdateVehicleFunction.VehicleFunctionName = pObjVehicleFunction.VehicleFunctionName;
                mObjInsertUpdateVehicleFunction.VehiclePurposeID = pObjVehicleFunction.VehiclePurposeID;
                mObjInsertUpdateVehicleFunction.Active = pObjVehicleFunction.Active;

                if (pObjVehicleFunction.VehicleFunctionID == 0)
                {
                    _db.Vehicle_Function.Add(mObjInsertUpdateVehicleFunction);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjVehicleFunction.VehicleFunctionID == 0)
                        mObjFuncResponse.Message = "Vehicle Function Added Successfully";
                    else
                        mObjFuncResponse.Message = "Vehicle Function Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjVehicleFunction.VehicleFunctionID == 0)
                        mObjFuncResponse.Message = "Vehicle Function Addition Failed";
                    else
                        mObjFuncResponse.Message = "Vehicle Function Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetVehicleFunctionList_Result> REP_GetVehicleFunctionList(Vehicle_Function pObjVehicleFunction)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehicleFunctionList(pObjVehicleFunction.VehicleFunctionName, pObjVehicleFunction.VehicleFunctionID, pObjVehicleFunction.VehiclePurposeID, pObjVehicleFunction.VehicleFunctionIds, pObjVehicleFunction.intStatus, pObjVehicleFunction.IncludeVehicleFunctionIds, pObjVehicleFunction.ExcludeVehicleFunctionIds).ToList();
            }
        }

        public usp_GetVehicleFunctionList_Result REP_GetVehicleFunctionDetails(Vehicle_Function pObjVehicleFunction)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehicleFunctionList(pObjVehicleFunction.VehicleFunctionName, pObjVehicleFunction.VehicleFunctionID, pObjVehicleFunction.VehiclePurposeID, pObjVehicleFunction.VehicleFunctionIds, pObjVehicleFunction.intStatus, pObjVehicleFunction.IncludeVehicleFunctionIds, pObjVehicleFunction.ExcludeVehicleFunctionIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetVehicleFunctionDropDownList(Vehicle_Function pObjVehicleFunction)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from bfunc in _db.usp_GetVehicleFunctionList(pObjVehicleFunction.VehicleFunctionName, pObjVehicleFunction.VehicleFunctionID, pObjVehicleFunction.VehiclePurposeID, pObjVehicleFunction.VehicleFunctionIds, pObjVehicleFunction.intStatus, pObjVehicleFunction.IncludeVehicleFunctionIds, pObjVehicleFunction.ExcludeVehicleFunctionIds)
                               select new DropDownListResult()
                               {
                                   id = bfunc.VehicleFunctionID.GetValueOrDefault(),
                                   text = bfunc.VehicleFunctionName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Vehicle_Function pObjVehicleFunction)
        {
            using (_db = new EIRSEntities())
            {
                Vehicle_Function mObjInsertUpdateVehicleFunction; //Vehicle Function Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load VehicleFunction
                if (pObjVehicleFunction.VehicleFunctionID != 0)
                {
                    mObjInsertUpdateVehicleFunction = (from bfunc in _db.Vehicle_Function
                                                        where bfunc.VehicleFunctionID == pObjVehicleFunction.VehicleFunctionID
                                                        select bfunc).FirstOrDefault();

                    if (mObjInsertUpdateVehicleFunction != null)
                    {
                        mObjInsertUpdateVehicleFunction.Active = !mObjInsertUpdateVehicleFunction.Active;
                        mObjInsertUpdateVehicleFunction.ModifiedBy = pObjVehicleFunction.ModifiedBy;
                        mObjInsertUpdateVehicleFunction.ModifiedDate = pObjVehicleFunction.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Vehicle Function Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetVehicleFunctionList(pObjVehicleFunction.VehicleFunctionName, 0, pObjVehicleFunction.VehiclePurposeID, pObjVehicleFunction.VehicleFunctionIds, pObjVehicleFunction.intStatus, pObjVehicleFunction.IncludeVehicleFunctionIds, pObjVehicleFunction.ExcludeVehicleFunctionIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Vehicle Function Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
