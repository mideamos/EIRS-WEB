using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class VehicleInsuranceRepository : IVehicleInsuranceRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateVehicleInsurance(Vehicle_Insurance pObjVehicleInsurance)
        {
            using (_db = new EIRSEntities())
            {
                Vehicle_Insurance mObjInsertUpdateVehicleInsurance; //Vehicle Insurance Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object


                //If Update Load VehicleInsurance
                if (pObjVehicleInsurance.VehicleInsuranceID != 0)
                {
                    mObjInsertUpdateVehicleInsurance = (from vins in _db.Vehicle_Insurance
                                                 where vins.VehicleInsuranceID == pObjVehicleInsurance.VehicleInsuranceID
                                                 select vins).FirstOrDefault();

                    if (mObjInsertUpdateVehicleInsurance != null)
                    {
                        mObjInsertUpdateVehicleInsurance.ModifiedBy = pObjVehicleInsurance.ModifiedBy;
                        mObjInsertUpdateVehicleInsurance.ModifiedDate = pObjVehicleInsurance.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateVehicleInsurance = new Vehicle_Insurance();
                        mObjInsertUpdateVehicleInsurance.CreatedBy = pObjVehicleInsurance.CreatedBy;
                        mObjInsertUpdateVehicleInsurance.CreatedDate = pObjVehicleInsurance.CreatedDate;
                    }
                }
                else // Else Insert VehicleInsurance
                {
                    mObjInsertUpdateVehicleInsurance = new Vehicle_Insurance();
                    mObjInsertUpdateVehicleInsurance.CreatedBy = pObjVehicleInsurance.CreatedBy;
                    mObjInsertUpdateVehicleInsurance.CreatedDate = pObjVehicleInsurance.CreatedDate;
                }

                mObjInsertUpdateVehicleInsurance.VehicleID = pObjVehicleInsurance.VehicleID;
                mObjInsertUpdateVehicleInsurance.InsuranceCertificateNumber = pObjVehicleInsurance.InsuranceCertificateNumber;
                mObjInsertUpdateVehicleInsurance.StartDate = pObjVehicleInsurance.StartDate;
                mObjInsertUpdateVehicleInsurance.ExpiryDate = pObjVehicleInsurance.ExpiryDate;
                mObjInsertUpdateVehicleInsurance.CoverTypeID = pObjVehicleInsurance.CoverTypeID;
                mObjInsertUpdateVehicleInsurance.InsuranceStatusID = pObjVehicleInsurance.InsuranceStatusID;
                mObjInsertUpdateVehicleInsurance.PremiumAmount = pObjVehicleInsurance.PremiumAmount;
                mObjInsertUpdateVehicleInsurance.VerificationAmount = pObjVehicleInsurance.VerificationAmount;
                mObjInsertUpdateVehicleInsurance.BrokerAmount = pObjVehicleInsurance.BrokerAmount;
                mObjInsertUpdateVehicleInsurance.Active = pObjVehicleInsurance.Active;

                if (pObjVehicleInsurance.VehicleInsuranceID == 0)
                {
                    _db.Vehicle_Insurance.Add(mObjInsertUpdateVehicleInsurance);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjVehicleInsurance.VehicleInsuranceID == 0)
                        mObjFuncResponse.Message = "Vehicle Insurance Added Successfully";
                    else
                        mObjFuncResponse.Message = "Vehicle Insurance Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjVehicleInsurance.VehicleInsuranceID == 0)
                        mObjFuncResponse.Message = "Vehicle Insurance Addition Failed";
                    else
                        mObjFuncResponse.Message = "Vehicle Insurance Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetVehicleInsuranceList_Result> REP_GetVehicleInsuranceList(Vehicle_Insurance pObjVehicleInsurance)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehicleInsuranceList(pObjVehicleInsurance.InsuranceCertificateNumber,pObjVehicleInsurance.VehicleInsuranceID, pObjVehicleInsurance.VehicleID, pObjVehicleInsurance.IntStatus).ToList();
            }
        }

        public usp_GetVehicleInsuranceList_Result REP_GetVehicleInsuranceDetails(Vehicle_Insurance pObjVehicleInsurance)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehicleInsuranceList(pObjVehicleInsurance.InsuranceCertificateNumber, pObjVehicleInsurance.VehicleInsuranceID, pObjVehicleInsurance.VehicleID, pObjVehicleInsurance.IntStatus).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetVehicleInsuranceDropDownList(Vehicle_Insurance pObjVehicleInsurance)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from vins in _db.usp_GetVehicleInsuranceList(pObjVehicleInsurance.InsuranceCertificateNumber, pObjVehicleInsurance.VehicleInsuranceID, pObjVehicleInsurance.VehicleID, pObjVehicleInsurance.IntStatus)
                               select new DropDownListResult()
                               {
                                   id = vins.VehicleInsuranceID.GetValueOrDefault(),
                                   text = vins.InsuranceCertificateNumber
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Vehicle_Insurance pObjVehicleInsurance)
        {
            using (_db = new EIRSEntities())
            {
                Vehicle_Insurance mObjInsertUpdateVehicleInsurance; //VehicleInsurance Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load VehicleInsurance
                if (pObjVehicleInsurance.VehicleInsuranceID != 0)
                {
                    mObjInsertUpdateVehicleInsurance = (from vins in _db.Vehicle_Insurance
                                                 where vins.VehicleInsuranceID == pObjVehicleInsurance.VehicleInsuranceID
                                                 select vins).FirstOrDefault();

                    if (mObjInsertUpdateVehicleInsurance != null)
                    {
                        mObjInsertUpdateVehicleInsurance.Active = !mObjInsertUpdateVehicleInsurance.Active;
                        mObjInsertUpdateVehicleInsurance.ModifiedBy = pObjVehicleInsurance.ModifiedBy;
                        mObjInsertUpdateVehicleInsurance.ModifiedDate = pObjVehicleInsurance.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Vehicle Insurance Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetVehicleInsuranceList(pObjVehicleInsurance.InsuranceCertificateNumber, 0, pObjVehicleInsurance.VehicleID, pObjVehicleInsurance.IntStatus).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Vehicle Insurance Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
