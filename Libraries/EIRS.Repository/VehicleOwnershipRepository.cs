using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class VehicleOwnershipRepository : IVehicleOwnershipRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateVehicleOwnership(Vehicle_Ownership pObjVehicleOwnership)
        {
            using (_db = new EIRSEntities())
            {
                Vehicle_Ownership mObjInsertUpdateVehicleOwnership; //Vehicle Ownership Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from vownership in _db.Vehicle_Ownership
                                       where vownership.VehicleOwnershipName == pObjVehicleOwnership.VehicleOwnershipName && vownership.VehicleOwnershipID != pObjVehicleOwnership.VehicleOwnershipID
                                       select vownership);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Vehicle Ownership already exists";
                    return mObjFuncResponse;
                }

                //If Update Load VehicleOwnership
                if (pObjVehicleOwnership.VehicleOwnershipID != 0)
                {
                    mObjInsertUpdateVehicleOwnership = (from vownership in _db.Vehicle_Ownership
                                                 where vownership.VehicleOwnershipID == pObjVehicleOwnership.VehicleOwnershipID
                                                 select vownership).FirstOrDefault();

                    if (mObjInsertUpdateVehicleOwnership != null)
                    {
                        mObjInsertUpdateVehicleOwnership.ModifiedBy = pObjVehicleOwnership.ModifiedBy;
                        mObjInsertUpdateVehicleOwnership.ModifiedDate = pObjVehicleOwnership.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateVehicleOwnership = new Vehicle_Ownership();
                        mObjInsertUpdateVehicleOwnership.CreatedBy = pObjVehicleOwnership.CreatedBy;
                        mObjInsertUpdateVehicleOwnership.CreatedDate = pObjVehicleOwnership.CreatedDate;
                    }
                }
                else // Else Insert VehicleOwnership
                {
                    mObjInsertUpdateVehicleOwnership = new Vehicle_Ownership();
                    mObjInsertUpdateVehicleOwnership.CreatedBy = pObjVehicleOwnership.CreatedBy;
                    mObjInsertUpdateVehicleOwnership.CreatedDate = pObjVehicleOwnership.CreatedDate;
                }

                mObjInsertUpdateVehicleOwnership.VehicleOwnershipName = pObjVehicleOwnership.VehicleOwnershipName;
                mObjInsertUpdateVehicleOwnership.Active = pObjVehicleOwnership.Active;

                if (pObjVehicleOwnership.VehicleOwnershipID == 0)
                {
                    _db.Vehicle_Ownership.Add(mObjInsertUpdateVehicleOwnership);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjVehicleOwnership.VehicleOwnershipID == 0)
                        mObjFuncResponse.Message = "Vehicle Ownership Added Successfully";
                    else
                        mObjFuncResponse.Message = "Vehicle Ownership Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjVehicleOwnership.VehicleOwnershipID == 0)
                        mObjFuncResponse.Message = "Vehicle Ownership Addition Failed";
                    else
                        mObjFuncResponse.Message = "Vehicle Ownership Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetVehicleOwnershipList_Result> REP_GetVehicleOwnershipList(Vehicle_Ownership pObjVehicleOwnership)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehicleOwnershipList(pObjVehicleOwnership.VehicleOwnershipName, pObjVehicleOwnership.VehicleOwnershipID, pObjVehicleOwnership.VehicleOwnershipIds, pObjVehicleOwnership.intStatus, pObjVehicleOwnership.IncludeVehicleOwnershipIds, pObjVehicleOwnership.ExcludeVehicleOwnershipIds).ToList();
            }
        }

        public usp_GetVehicleOwnershipList_Result REP_GetVehicleOwnershipDetails(Vehicle_Ownership pObjVehicleOwnership)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehicleOwnershipList(pObjVehicleOwnership.VehicleOwnershipName, pObjVehicleOwnership.VehicleOwnershipID, pObjVehicleOwnership.VehicleOwnershipIds, pObjVehicleOwnership.intStatus, pObjVehicleOwnership.IncludeVehicleOwnershipIds, pObjVehicleOwnership.ExcludeVehicleOwnershipIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetVehicleOwnershipDropDownList(Vehicle_Ownership pObjVehicleOwnership)
        {
            using (_db = new EIRSEntities())
            {
                //var vResult = (from vownership in _db.usp_GetVehicleOwnershipList(pObjVehicleOwnership.VehicleOwnershipName, pObjVehicleOwnership.VehicleOwnershipID, pObjVehicleOwnership.VehicleOwnershipIds, pObjVehicleOwnership.intStatus, pObjVehicleOwnership.IncludeVehicleOwnershipIds, pObjVehicleOwnership.ExcludeVehicleOwnershipIds)
                //               select new DropDownListResult()
                //               {
                //                   id = vownership.VehicleOwnershipID.GetValueOrDefault(),
                //                   text = vownership.VehicleOwnershipName
                //               }).ToList();

                var vResult = from pro in _db.Vehicle_Ownership
                              select new DropDownListResult() { id = pro.VehicleOwnershipID, text = pro.VehicleOwnershipName };

                return vResult.ToList();
            }
        }

        public FuncResponse REP_UpdateStatus(Vehicle_Ownership pObjVehicleOwnership)
        {
            using (_db = new EIRSEntities())
            {
                Vehicle_Ownership mObjInsertUpdateVehicleOwnership; //VehicleOwnership Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load VehicleOwnership
                if (pObjVehicleOwnership.VehicleOwnershipID != 0)
                {
                    mObjInsertUpdateVehicleOwnership = (from vownership in _db.Vehicle_Ownership
                                                 where vownership.VehicleOwnershipID == pObjVehicleOwnership.VehicleOwnershipID
                                                 select vownership).FirstOrDefault();

                    if (mObjInsertUpdateVehicleOwnership != null)
                    {
                        mObjInsertUpdateVehicleOwnership.Active = !mObjInsertUpdateVehicleOwnership.Active;
                        mObjInsertUpdateVehicleOwnership.ModifiedBy = pObjVehicleOwnership.ModifiedBy;
                        mObjInsertUpdateVehicleOwnership.ModifiedDate = pObjVehicleOwnership.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Vehicle Ownership Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetVehicleOwnershipList(pObjVehicleOwnership.VehicleOwnershipName, 0, pObjVehicleOwnership.VehicleOwnershipIds, pObjVehicleOwnership.intStatus, pObjVehicleOwnership.IncludeVehicleOwnershipIds, pObjVehicleOwnership.ExcludeVehicleOwnershipIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Vehicle Ownership Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
