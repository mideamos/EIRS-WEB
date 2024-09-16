using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class VehicleLicenseRepository : IVehicleLicenseRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateVehicleLicense(Vehicle_Licenses pObjVehicleLicense)
        {
            using (_db = new EIRSEntities())
            {
                Vehicle_Licenses mObjInsertUpdateVehicleLicense; //Vehicle License Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load VehicleLicense
                if (pObjVehicleLicense.VehicleLicenseID != 0)
                {
                    mObjInsertUpdateVehicleLicense = (from vlic in _db.Vehicle_Licenses
                                                 where vlic.VehicleLicenseID == pObjVehicleLicense.VehicleLicenseID
                                                 select vlic).FirstOrDefault();

                    if (mObjInsertUpdateVehicleLicense != null)
                    {
                        mObjInsertUpdateVehicleLicense.ModifiedBy = pObjVehicleLicense.ModifiedBy;
                        mObjInsertUpdateVehicleLicense.ModifiedDate = pObjVehicleLicense.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateVehicleLicense = new Vehicle_Licenses();
                        mObjInsertUpdateVehicleLicense.CreatedBy = pObjVehicleLicense.CreatedBy;
                        mObjInsertUpdateVehicleLicense.CreatedDate = pObjVehicleLicense.CreatedDate;
                    }
                }
                else // Else Insert VehicleLicense
                {
                    mObjInsertUpdateVehicleLicense = new Vehicle_Licenses();
                    mObjInsertUpdateVehicleLicense.CreatedBy = pObjVehicleLicense.CreatedBy;
                    mObjInsertUpdateVehicleLicense.CreatedDate = pObjVehicleLicense.CreatedDate;
                }

                mObjInsertUpdateVehicleLicense.VehicleID = pObjVehicleLicense.VehicleID;
                mObjInsertUpdateVehicleLicense.LicenseNumber = pObjVehicleLicense.LicenseNumber;
                mObjInsertUpdateVehicleLicense.StartDate = pObjVehicleLicense.StartDate;
                mObjInsertUpdateVehicleLicense.ExpiryDate = pObjVehicleLicense.ExpiryDate;
                mObjInsertUpdateVehicleLicense.VehicleInsuranceID = pObjVehicleLicense.VehicleInsuranceID;
                mObjInsertUpdateVehicleLicense.LicenseStatusID = pObjVehicleLicense.LicenseStatusID;
                mObjInsertUpdateVehicleLicense.Active = pObjVehicleLicense.Active;

                if (pObjVehicleLicense.VehicleLicenseID == 0)
                {
                    _db.Vehicle_Licenses.Add(mObjInsertUpdateVehicleLicense);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjVehicleLicense.VehicleLicenseID == 0)
                        mObjFuncResponse.Message = "Vehicle License Added Successfully";
                    else
                        mObjFuncResponse.Message = "Vehicle License Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjVehicleLicense.VehicleLicenseID == 0)
                        mObjFuncResponse.Message = "Vehicle License Addition Failed";
                    else
                        mObjFuncResponse.Message = "Vehicle License Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetVehicleLicenseList_Result> REP_GetVehicleLicenseList(Vehicle_Licenses pObjVehicleLicense)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehicleLicenseList(pObjVehicleLicense.LicenseNumber, pObjVehicleLicense.VehicleLicenseID, pObjVehicleLicense.VehicleID, pObjVehicleLicense.IntStatus).ToList();
            }
        }

        public usp_GetVehicleLicenseList_Result REP_GetVehicleLicenseDetails(Vehicle_Licenses pObjVehicleLicense)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehicleLicenseList(pObjVehicleLicense.LicenseNumber, pObjVehicleLicense.VehicleLicenseID, pObjVehicleLicense.VehicleID, pObjVehicleLicense.IntStatus).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetVehicleLicenseDropDownList(Vehicle_Licenses pObjVehicleLicense)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from vlic in _db.usp_GetVehicleLicenseList(pObjVehicleLicense.LicenseNumber, pObjVehicleLicense.VehicleLicenseID, pObjVehicleLicense.VehicleID, pObjVehicleLicense.IntStatus)
                               select new DropDownListResult()
                               {
                                   id = vlic.VehicleLicenseID.GetValueOrDefault(),
                                   text = vlic.LicenseNumber
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Vehicle_Licenses pObjVehicleLicense)
        {
            using (_db = new EIRSEntities())
            {
                Vehicle_Licenses mObjInsertUpdateVehicleLicense; //VehicleLicense Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load VehicleLicense
                if (pObjVehicleLicense.VehicleLicenseID != 0)
                {
                    mObjInsertUpdateVehicleLicense = (from vlic in _db.Vehicle_Licenses
                                                 where vlic.VehicleLicenseID == pObjVehicleLicense.VehicleLicenseID
                                                 select vlic).FirstOrDefault();

                    if (mObjInsertUpdateVehicleLicense != null)
                    {
                        mObjInsertUpdateVehicleLicense.Active = !mObjInsertUpdateVehicleLicense.Active;
                        mObjInsertUpdateVehicleLicense.ModifiedBy = pObjVehicleLicense.ModifiedBy;
                        mObjInsertUpdateVehicleLicense.ModifiedDate = pObjVehicleLicense.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Vehicle License Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetVehicleLicenseList(pObjVehicleLicense.LicenseNumber, 0, pObjVehicleLicense.VehicleID, pObjVehicleLicense.IntStatus).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Vehicle License Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
