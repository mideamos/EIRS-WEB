using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class VehiclePurposeRepository : IVehiclePurposeRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateVehiclePurpose(Vehicle_Purpose pObjVehiclePurpose)
        {
            using (_db = new EIRSEntities())
            {
                Vehicle_Purpose mObjInsertUpdateVehiclePurpose; //Vehicle Purpose Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from vpurpose in _db.Vehicle_Purpose
                                       where vpurpose.VehiclePurposeName == pObjVehiclePurpose.VehiclePurposeName && vpurpose.VehiclePurposeID != pObjVehiclePurpose.VehiclePurposeID
                                       select vpurpose);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Vehicle Purpose already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Vehicle Purpose
                if (pObjVehiclePurpose.VehiclePurposeID != 0)
                {
                    mObjInsertUpdateVehiclePurpose = (from vpurpose in _db.Vehicle_Purpose
                                                      where vpurpose.VehiclePurposeID == pObjVehiclePurpose.VehiclePurposeID
                                                      select vpurpose).FirstOrDefault();

                    if (mObjInsertUpdateVehiclePurpose != null)
                    {
                        mObjInsertUpdateVehiclePurpose.ModifiedBy = pObjVehiclePurpose.ModifiedBy;
                        mObjInsertUpdateVehiclePurpose.ModifiedDate = pObjVehiclePurpose.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateVehiclePurpose = new Vehicle_Purpose();
                        mObjInsertUpdateVehiclePurpose.CreatedBy = pObjVehiclePurpose.CreatedBy;
                        mObjInsertUpdateVehiclePurpose.CreatedDate = pObjVehiclePurpose.CreatedDate;
                    }
                }
                else // Else Insert Vehicle Purpose
                {
                    mObjInsertUpdateVehiclePurpose = new Vehicle_Purpose();
                    mObjInsertUpdateVehiclePurpose.CreatedBy = pObjVehiclePurpose.CreatedBy;
                    mObjInsertUpdateVehiclePurpose.CreatedDate = pObjVehiclePurpose.CreatedDate;
                }

                mObjInsertUpdateVehiclePurpose.VehiclePurposeName = pObjVehiclePurpose.VehiclePurposeName;
                mObjInsertUpdateVehiclePurpose.Active = pObjVehiclePurpose.Active;

                if (pObjVehiclePurpose.VehiclePurposeID == 0)
                {
                    _db.Vehicle_Purpose.Add(mObjInsertUpdateVehiclePurpose);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjVehiclePurpose.VehiclePurposeID == 0)
                        mObjFuncResponse.Message = "Vehicle Purpose Added Successfully";
                    else
                        mObjFuncResponse.Message = "Vehicle Purpose Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjVehiclePurpose.VehiclePurposeID == 0)
                        mObjFuncResponse.Message = "Vehicle Purpose Addition Failed";
                    else
                        mObjFuncResponse.Message = "Vehicle Purpose Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetVehiclePurposeList_Result> REP_GetVehiclePurposeList(Vehicle_Purpose pObjVehiclePurpose)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehiclePurposeList(pObjVehiclePurpose.VehiclePurposeName, pObjVehiclePurpose.VehiclePurposeID, pObjVehiclePurpose.VehiclePurposeIds, pObjVehiclePurpose.intStatus, pObjVehiclePurpose.IncludeVehiclePurposeIds, pObjVehiclePurpose.ExcludeVehiclePurposeIds).ToList();
            }
        }

        public usp_GetVehiclePurposeList_Result REP_GetVehiclePurposeDetails(Vehicle_Purpose pObjVehiclePurpose)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehiclePurposeList(pObjVehiclePurpose.VehiclePurposeName, pObjVehiclePurpose.VehiclePurposeID, pObjVehiclePurpose.VehiclePurposeIds, pObjVehiclePurpose.intStatus, pObjVehiclePurpose.IncludeVehiclePurposeIds, pObjVehiclePurpose.ExcludeVehiclePurposeIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetVehiclePurposeDropDownList(Vehicle_Purpose pObjVehiclePurpose)
        {
            using (_db = new EIRSEntities())
            {
                //var vResult = (from vpurpose in _db.usp_GetVehiclePurposeList(pObjVehiclePurpose.VehiclePurposeName, pObjVehiclePurpose.VehiclePurposeID, pObjVehiclePurpose.VehiclePurposeIds, pObjVehiclePurpose.intStatus, pObjVehiclePurpose.IncludeVehiclePurposeIds, pObjVehiclePurpose.ExcludeVehiclePurposeIds)
                //               select new DropDownListResult()
                //               {
                //                   id = vpurpose.VehiclePurposeID.GetValueOrDefault(),
                //                   text = vpurpose.VehiclePurposeName
                //               }).ToList();
                
                var vResult = from pro in _db.Vehicle_Purpose
                              select new DropDownListResult() { id = pro.VehiclePurposeID, text = pro.VehiclePurposeName };

                return vResult.ToList();
            }
        }

        public FuncResponse REP_UpdateStatus(Vehicle_Purpose pObjVehiclePurpose)
        {
            using (_db = new EIRSEntities())
            {
                Vehicle_Purpose mObjInsertUpdateVehiclePurpose; //Vehicle Purpose Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load VehiclePurpose
                if (pObjVehiclePurpose.VehiclePurposeID != 0)
                {
                    mObjInsertUpdateVehiclePurpose = (from vpurpose in _db.Vehicle_Purpose
                                                      where vpurpose.VehiclePurposeID == pObjVehiclePurpose.VehiclePurposeID
                                                      select vpurpose).FirstOrDefault();

                    if (mObjInsertUpdateVehiclePurpose != null)
                    {
                        mObjInsertUpdateVehiclePurpose.Active = !mObjInsertUpdateVehiclePurpose.Active;
                        mObjInsertUpdateVehiclePurpose.ModifiedBy = pObjVehiclePurpose.ModifiedBy;
                        mObjInsertUpdateVehiclePurpose.ModifiedDate = pObjVehiclePurpose.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Vehicle Purpose Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetVehiclePurposeList(pObjVehiclePurpose.VehiclePurposeName, 0, pObjVehiclePurpose.VehiclePurposeIds, pObjVehiclePurpose.intStatus, pObjVehiclePurpose.IncludeVehiclePurposeIds, pObjVehiclePurpose.ExcludeVehiclePurposeIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Vehicle Purpose Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
