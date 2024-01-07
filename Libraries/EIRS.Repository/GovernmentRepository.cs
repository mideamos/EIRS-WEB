using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class GovernmentRepository : IGovernmentRepository
    {
        EIRSEntities _db;

        public FuncResponse<Government> REP_InsertUpdateGovernment(Government pObjGovernment)
        {
            using (_db = new EIRSEntities())
            {
                Government mObjInsertUpdateGovernment; //Government Insert Update Object
                FuncResponse<Government> mObjFuncResponse = new FuncResponse<Government>(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from gov in _db.Governments
                                       where gov.GovernmentName == pObjGovernment.GovernmentName && gov.GovernmentID != pObjGovernment.GovernmentID
                                       select gov);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Government with same name already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Government

                if (pObjGovernment.GovernmentID != 0)
                {
                    mObjInsertUpdateGovernment = (from gov in _db.Governments
                                                  where gov.GovernmentID == pObjGovernment.GovernmentID
                                                  select gov).FirstOrDefault();

                    if (mObjInsertUpdateGovernment != null)
                    {
                        //Check if status is changed

                        if (pObjGovernment.Active == false && mObjInsertUpdateGovernment.Active == true)
                        {
                            //Check Conditions for marking inactive to company

                            var vBuildingList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join bld in _db.Buildings on new { BuildingID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { bld.BuildingID, bld.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateGovernment.GovernmentID && tpa.TaxPayerTypeID == mObjInsertUpdateGovernment.TaxPayerTypeID && bld.Active == true
                                                 select tpa);
                            if (vBuildingList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vBusinessList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join bus in _db.Businesses on new { BusinessID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { bus.BusinessID, bus.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateGovernment.GovernmentID && tpa.TaxPayerTypeID == mObjInsertUpdateGovernment.TaxPayerTypeID && bus.Active == true
                                                 select tpa);
                            if (vBusinessList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vLandList = (from tpa in _db.MAP_TaxPayer_Asset
                                             join lnd in _db.Lands on new { LandID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { lnd.LandID, lnd.AssetTypeID }
                                             where tpa.TaxPayerID == mObjInsertUpdateGovernment.GovernmentID && tpa.TaxPayerTypeID == mObjInsertUpdateGovernment.TaxPayerTypeID && lnd.Active == true
                                             select tpa);
                            if (vLandList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vVechicleList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join vch in _db.Vehicles on new { VehicleID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { vch.VehicleID, vch.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateGovernment.GovernmentID && tpa.TaxPayerTypeID == mObjInsertUpdateGovernment.TaxPayerTypeID && vch.Active == true
                                                 select tpa);

                            if (vVechicleList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vAssessmentCount = (from ast in _db.Assessments
                                                    where ast.TaxPayerID == mObjInsertUpdateGovernment.GovernmentID && ast.TaxPayerTypeID == mObjInsertUpdateGovernment.TaxPayerTypeID && ast.Active == true && ast.SettlementStatusID == (int)EnumList.SettlementStatus.Partial
                                                    select ast);
                            if (vAssessmentCount.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are parital settled  assessment bill";
                                return mObjFuncResponse;
                            }

                            var vServiceBillCount = (from sb in _db.ServiceBills
                                                     where sb.TaxPayerID == mObjInsertUpdateGovernment.GovernmentID && sb.TaxPayerTypeID == mObjInsertUpdateGovernment.TaxPayerTypeID && sb.Active == true && sb.SettlementStatusID == (int)EnumList.SettlementStatus.Partial
                                                     select sb);
                            if (vServiceBillCount.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are parital settled service bill";
                                return mObjFuncResponse;
                            }

                            var vPoABalance = _db.usp_GetWalletBalance(mObjInsertUpdateGovernment.TaxPayerTypeID, mObjInsertUpdateGovernment.GovernmentID).FirstOrDefault();
                            if (vPoABalance.GetValueOrDefault() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "PoA Balance is more than 0";
                                return mObjFuncResponse;
                            }
                        }

                        if (pObjGovernment.TaxOfficeID != mObjInsertUpdateGovernment.TaxOfficeID)
                        {
                            mObjInsertUpdateGovernment.TaxOfficerID = null;
                        }

                        mObjInsertUpdateGovernment.ModifiedBy = pObjGovernment.ModifiedBy;
                        mObjInsertUpdateGovernment.ModifiedDate = pObjGovernment.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateGovernment = new Government();
                        mObjInsertUpdateGovernment.CreatedBy = pObjGovernment.CreatedBy;
                        mObjInsertUpdateGovernment.CreatedDate = pObjGovernment.CreatedDate;
                    }
                }
                else // Else Insert Government Completion
                {
                    mObjInsertUpdateGovernment = new Government();
                    mObjInsertUpdateGovernment.CreatedBy = pObjGovernment.CreatedBy;
                    mObjInsertUpdateGovernment.CreatedDate = pObjGovernment.CreatedDate;
                }

                mObjInsertUpdateGovernment.GovernmentName = pObjGovernment.GovernmentName;
                mObjInsertUpdateGovernment.GovernmentTypeID = pObjGovernment.GovernmentTypeID;
                mObjInsertUpdateGovernment.TIN = pObjGovernment.TIN;
                mObjInsertUpdateGovernment.TaxOfficeID = pObjGovernment.TaxOfficeID;
                mObjInsertUpdateGovernment.TaxPayerTypeID = pObjGovernment.TaxPayerTypeID;
                mObjInsertUpdateGovernment.ContactNumber = pObjGovernment.ContactNumber;
                mObjInsertUpdateGovernment.ContactEmail = pObjGovernment.ContactEmail;
                mObjInsertUpdateGovernment.ContactName = pObjGovernment.ContactName;
                mObjInsertUpdateGovernment.NotificationMethodID = pObjGovernment.NotificationMethodID;
                mObjInsertUpdateGovernment.ContactAddress = pObjGovernment.ContactAddress;
                mObjInsertUpdateGovernment.RegisterationStatusID = pObjGovernment.RegisterationStatusID == null ? (mObjInsertUpdateGovernment.RegisterationStatusID == null ? (int)EnumList.RegisterationStatus.Not_Started : mObjInsertUpdateGovernment.RegisterationStatusID) : pObjGovernment.RegisterationStatusID;
                mObjInsertUpdateGovernment.Active = pObjGovernment.Active;

                if (pObjGovernment.GovernmentID == 0)
                {
                    _db.Governments.Add(mObjInsertUpdateGovernment);
                }

                try
                {
                    _db.SaveChanges();

                    var context = ((IObjectContextAdapter)_db).ObjectContext;
                    var refreshableObjects = _db.ChangeTracker.Entries().Select(c => c.Entity).ToList();
                    context.Refresh(RefreshMode.StoreWins, refreshableObjects);

                    mObjFuncResponse.Success = true;
                    if (pObjGovernment.GovernmentID == 0)
                    {
                        mObjFuncResponse.Message = "Government Added Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Government Updated Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjInsertUpdateGovernment;

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjGovernment.GovernmentID == 0)
                    {
                        mObjFuncResponse.Message = "Government Addition Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Government Updation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetGovernmentList_Result> REP_GetGovernmentList(Government pObjGovernment)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetGovernmentList(pObjGovernment.GovernmentID, pObjGovernment.GovernmentName, pObjGovernment.TIN, pObjGovernment.GovernmentRIN, pObjGovernment.ContactNumber, pObjGovernment.intStatus).ToList();
            }
        }

        public IList<vw_Government> REP_GetGovernmentList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.vw_Government.ToList();
            }
        }

        public IList<DropDownListResult> REP_GetGovernmentDropDownList(Government pObjGovernment)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from gov in _db.usp_GetGovernmentList(pObjGovernment.GovernmentID, pObjGovernment.GovernmentName, pObjGovernment.TIN, pObjGovernment.GovernmentRIN, pObjGovernment.ContactNumber, pObjGovernment.intStatus)
                               select new DropDownListResult()
                               {
                                   id = gov.GovernmentID.GetValueOrDefault(),
                                   text = gov.GovernmentName
                               });

                return vResult.ToList();
            }
        }

        public IList<DropDownListResult> REP_GetGovernmentDropDownList(string pStrGovernmentName, int pIntTaxOfficeID = 0)
        {
            using (_db = new EIRSEntities())
            {
                if (pIntTaxOfficeID != 0)
                {
                    var vResult = (from gov in _db.vw_Government
                                   where (gov.GovernmentName.ToLower().Contains(pStrGovernmentName.ToLower()) || gov.GovernmentRIN.ToLower().Contains(pStrGovernmentName.ToLower()))
                                   && gov.TaxOfficeID == pIntTaxOfficeID
                                   select new DropDownListResult()
                                   {
                                       id = gov.GovernmentID,
                                       text = gov.GovernmentRIN + " - " + gov.GovernmentName
                                   });

                    return vResult.ToList();

                }
                else
                {
                    var vResult = (from gov in _db.vw_Government
                                   where gov.GovernmentName.ToLower().Contains(pStrGovernmentName.ToLower()) || gov.GovernmentRIN.ToLower().Contains(pStrGovernmentName.ToLower())
                                   select new DropDownListResult()
                                   {
                                       id = gov.GovernmentID,
                                       text = gov.GovernmentRIN + " - " + gov.GovernmentName
                                   });

                    return vResult.ToList();
                }
            }
        }

        public usp_GetGovernmentList_Result REP_GetGovernmentDetails(Government pObjGovernment)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetGovernmentList(pObjGovernment.GovernmentID, pObjGovernment.GovernmentName, pObjGovernment.TIN, pObjGovernment.GovernmentRIN, pObjGovernment.ContactNumber, pObjGovernment.intStatus).FirstOrDefault();
            }
        }

        public FuncResponse REP_UpdateStatus(Government pObjGovernment)
        {
            using (_db = new EIRSEntities())
            {
                Government mObjInsertUpdateGovernment; //Government Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Government
                if (pObjGovernment.GovernmentID != 0)
                {
                    mObjInsertUpdateGovernment = (from gov in _db.Governments
                                                  where gov.GovernmentID == pObjGovernment.GovernmentID
                                                  select gov).FirstOrDefault();

                    if (mObjInsertUpdateGovernment != null)
                    {
                        if (mObjInsertUpdateGovernment.Active == true)
                        {
                            //Check Conditions for marking inactive to company

                            var vBuildingList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join bld in _db.Buildings on new { BuildingID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { bld.BuildingID, bld.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateGovernment.GovernmentID && tpa.TaxPayerTypeID == mObjInsertUpdateGovernment.TaxPayerTypeID && bld.Active == true
                                                 select tpa);
                            if (vBuildingList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vBusinessList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join bus in _db.Businesses on new { BusinessID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { bus.BusinessID, bus.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateGovernment.GovernmentID && tpa.TaxPayerTypeID == mObjInsertUpdateGovernment.TaxPayerTypeID && bus.Active == true
                                                 select tpa);
                            if (vBusinessList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vLandList = (from tpa in _db.MAP_TaxPayer_Asset
                                             join lnd in _db.Lands on new { LandID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { lnd.LandID, lnd.AssetTypeID }
                                             where tpa.TaxPayerID == mObjInsertUpdateGovernment.GovernmentID && tpa.TaxPayerTypeID == mObjInsertUpdateGovernment.TaxPayerTypeID && lnd.Active == true
                                             select tpa);
                            if (vLandList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vVechicleList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join vch in _db.Vehicles on new { VehicleID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { vch.VehicleID, vch.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateGovernment.GovernmentID && tpa.TaxPayerTypeID == mObjInsertUpdateGovernment.TaxPayerTypeID && vch.Active == true
                                                 select tpa);

                            if (vVechicleList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vAssessmentCount = (from ast in _db.Assessments
                                                    where ast.TaxPayerID == mObjInsertUpdateGovernment.GovernmentID && ast.TaxPayerTypeID == mObjInsertUpdateGovernment.TaxPayerTypeID && ast.Active == true && ast.SettlementStatusID == (int)EnumList.SettlementStatus.Partial
                                                    select ast);
                            if (vAssessmentCount.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are parital settled  assessment bill";
                                return mObjFuncResponse;
                            }

                            var vServiceBillCount = (from sb in _db.ServiceBills
                                                     where sb.TaxPayerID == mObjInsertUpdateGovernment.GovernmentID && sb.TaxPayerTypeID == mObjInsertUpdateGovernment.TaxPayerTypeID && sb.Active == true && sb.SettlementStatusID == (int)EnumList.SettlementStatus.Partial
                                                     select sb);
                            if (vServiceBillCount.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are parital settled service bill";
                                return mObjFuncResponse;
                            }

                            var vPoABalance = _db.usp_GetWalletBalance(mObjInsertUpdateGovernment.TaxPayerTypeID, mObjInsertUpdateGovernment.GovernmentID).FirstOrDefault();
                            if (vPoABalance.GetValueOrDefault() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "PoA Balance is more than 0";
                                return mObjFuncResponse;
                            }
                        }

                        mObjInsertUpdateGovernment.Active = !mObjInsertUpdateGovernment.Active;
                        mObjInsertUpdateGovernment.ModifiedBy = pObjGovernment.ModifiedBy;
                        mObjInsertUpdateGovernment.ModifiedDate = pObjGovernment.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Government Updated Successfully";
                            //mObjFuncResponse.AdditionalData = _db.usp_GetGovernmentList(0, pObjGovernment.GovernmentName, pObjGovernment.TIN, pObjGovernment.GovernmentRIN, pObjGovernment.ContactNumber, pObjGovernment.intStatus).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Government Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetGovernmentAddressInformation_Result> REP_GetAddressInformation(Government pObjGovernment)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetGovernmentAddressInformation(pObjGovernment.GovernmentID).ToList();
            }
        }

        public FuncResponse REP_InsertAddressInformation(MAP_Government_AddressInformation pObjAddressInformation)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                _db.MAP_Government_AddressInformation.Add(pObjAddressInformation);

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.Message = "Address Information Added Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;
                    mObjFuncResponse.Message = "Address Information Addition Failed";
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse<IList<usp_GetGovernmentAddressInformation_Result>> REP_RemoveAddressInformation(MAP_Government_AddressInformation pObjAddressInformation)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<IList<usp_GetGovernmentAddressInformation_Result>> mObjFuncResponse = new FuncResponse<IList<usp_GetGovernmentAddressInformation_Result>>(); //Return Object

                MAP_Government_AddressInformation mObjDeleteAddressInformation;

                mObjDeleteAddressInformation = _db.MAP_Government_AddressInformation.Find(pObjAddressInformation.GAIID);

                if (mObjDeleteAddressInformation == null)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Address Information Already Removed.";
                }
                else
                {
                    _db.MAP_Government_AddressInformation.Remove(mObjDeleteAddressInformation);

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;

                        mObjFuncResponse.Message = "Address Information Removed Successfully";

                        mObjFuncResponse.AdditionalData = _db.usp_GetGovernmentAddressInformation(pObjAddressInformation.GovernmentID).ToList();
                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = Ex.Message;
                    }
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse<Government> REP_CheckGovermentLoginDetails(Government pObjGoverment)
        {
            FuncResponse<Government> mObjFuncResponse = new FuncResponse<Government>();

            try
            {
                using (_db = new EIRSEntities())
                {

                    var vFind = (from gov in _db.Governments
                                 where (gov.GovernmentRIN.Equals(pObjGoverment.GovernmentRIN) && gov.Password.Equals(pObjGoverment.Password))
                                 select gov);

                    if (vFind != null && vFind.Count() == 0)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = "Invaild Login Details";
                    }
                    else if (vFind.Count() > 0 && vFind.FirstOrDefault().Active == false)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = "User Is Not Active";
                    }
                    else
                    {
                        mObjFuncResponse.Success = true;
                        mObjFuncResponse.Message = "Login Successfully";
                        mObjFuncResponse.AdditionalData = vFind.FirstOrDefault();
                    }

                    return mObjFuncResponse;
                }
            }
            catch (Exception ex)
            {
                mObjFuncResponse.Exception = ex;
                mObjFuncResponse.Success = false;
                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_ChangePassword(Government pObjGoverment)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vUserDetails = (from gov in _db.Governments
                                    where (gov.GovernmentRIN.Equals(pObjGoverment.GovernmentRIN) && gov.Password.Equals(pObjGoverment.OldPassword))
                                    select gov).FirstOrDefault();

                if (vUserDetails != null && vUserDetails.GovernmentID > 0)
                {
                    vUserDetails.Password = pObjGoverment.Password;

                    try
                    {
                        _db.SaveChanges();
                        mObjResponse.Success = true;
                        mObjResponse.Message = "Password has Changed Successfully";

                    }
                    catch (Exception ex)
                    {
                        mObjResponse.Success = false;
                        mObjResponse.Message = "Password has not changed, Please try Again ";
                    }

                    return mObjResponse;

                }
                mObjResponse.Success = false;
                mObjResponse.Message = "Invaild Old Password";
                return mObjResponse;

            }
        }

        public FuncResponse REP_UpdatePassword(Government pObjGovernment)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vUserDetails = (from usr in _db.Governments
                                    where usr.GovernmentID == pObjGovernment.GovernmentID
                                    select usr).FirstOrDefault();

                if (vUserDetails != null)
                {
                    vUserDetails.RegisterationStatusID = pObjGovernment.RegisterationStatusID;
                    vUserDetails.RegisterationDate = pObjGovernment.RegisterationDate;
                    vUserDetails.Password = pObjGovernment.Password;

                    try
                    {
                        _db.SaveChanges();
                        mObjResponse.Success = true;
                        mObjResponse.Message = "Password has Changed Successfully";
                    }
                    catch (Exception ex)
                    {
                        mObjResponse.Success = false;
                        mObjResponse.Message = "Password has not changed, Please try Again ";
                    }

                    return mObjResponse;

                }
                mObjResponse.Success = false;
                mObjResponse.Message = "Invaild Old Password";
                return mObjResponse;

            }
        }

        public void REP_UpdateRegisterationStatus(Government pObjGovernment)
        {
            using (_db = new EIRSEntities())
            {
                Government mObjUpdateGovernment = _db.Governments.Find(pObjGovernment.GovernmentID);

                if (mObjUpdateGovernment != null)
                {
                    mObjUpdateGovernment.RegisterationStatusID = pObjGovernment.RegisterationStatusID;

                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (Exception Ex)
                    {
                        throw (Ex);
                    }
                }
            }
        }

        public void REP_UpdateOTPCode(Government pObjGovernment)
        {
            using (_db = new EIRSEntities())
            {
                Government mObjUpdateGovernment = _db.Governments.Find(pObjGovernment.GovernmentID);

                if (mObjUpdateGovernment != null)
                {
                    mObjUpdateGovernment.VerificationOTP = pObjGovernment.VerificationOTP;

                    try
                    {
                        _db.SaveChanges();
                    }
                    catch (Exception Ex)
                    {
                        throw (Ex);
                    }
                }
            }
        }

        public FuncResponse REP_CheckOTPCode(Government pObjGovernment)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();
                Government mObjUpdateGovernment = _db.Governments.Find(pObjGovernment.GovernmentID);

                if (mObjUpdateGovernment != null)
                {
                    if (mObjUpdateGovernment.VerificationOTP == pObjGovernment.VerificationOTP)
                    {
                        mObjResponse.Success = true;
                        mObjUpdateGovernment.VerificationOTP = null;

                        try
                        {
                            _db.SaveChanges();
                        }
                        catch (Exception Ex)
                        {
                            throw (Ex);
                        }
                    }
                    else
                    {
                        mObjResponse.Success = false;
                    }


                }

                return mObjResponse;
            }
        }

        public IList<usp_SearchGovernmentForRDMLoad_Result> REP_SearchGovernmentDetails(Government pObjGovernment)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_SearchGovernmentForRDMLoad(pObjGovernment.GovernmentRIN, pObjGovernment.GovernmentName, pObjGovernment.TIN, pObjGovernment.GovernmentTypeName, pObjGovernment.TaxOfficeName, pObjGovernment.TaxPayerTypeName, pObjGovernment.ContactName, pObjGovernment.ContactEmail, pObjGovernment.ContactNumber, pObjGovernment.NotificationMethodName, pObjGovernment.ActiveText).ToList();
            }
        }

        public IDictionary<string, object> REP_SearchGovernment(Government pObjGovernment)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>
                {
                    ["GovernmentList"] = _db.usp_SearchGovernment(pObjGovernment.WhereCondition, pObjGovernment.OrderBy, pObjGovernment.OrderByDirection, pObjGovernment.PageNumber, pObjGovernment.PageSize, pObjGovernment.MainFilter,
                                                                   pObjGovernment.GovernmentRIN, pObjGovernment.GovernmentName, pObjGovernment.TIN, pObjGovernment.GovernmentTypeName, pObjGovernment.TaxOfficeName,
                                                                   pObjGovernment.TaxPayerTypeName, pObjGovernment.ContactName, pObjGovernment.ContactEmail, pObjGovernment.ContactNumber, pObjGovernment.NotificationMethodName, pObjGovernment.ActiveText).ToList()
                };

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(GovernmentID) FROM Government").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(GovernmentID) ");
                sbFilteredCountQuery.Append(" FROM Government gov ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Tax_Offices toff ON gov.TaxOfficeID = toff.TaxOfficeID ");
                sbFilteredCountQuery.Append(" INNER JOIN TaxPayer_Types tptype ON gov.TaxPayerTypeID = tptype.TaxPayerTypeID ");
                sbFilteredCountQuery.Append(" INNER JOIN Government_Types gtype ON gov.GovernmentTypeID = gtype.GovernmentTypeID ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Notification_Method nmthd ON gov.NotificationMethodID = nmthd.NotificationMethodID ");
                sbFilteredCountQuery.Append(" INNER JOIN MST_RegisterationStatus rgs ON gov.RegisterationStatusID = rgs.RegisterationStatusID WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjGovernment.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@GovernmentRIN", pObjGovernment.GovernmentRIN),
                    new SqlParameter("@GovernmentName", pObjGovernment.GovernmentName),
                    new SqlParameter("@TIN", pObjGovernment.TIN),
                    new SqlParameter("@GovernmentTypeName", pObjGovernment.GovernmentTypeName),
                    new SqlParameter("@TaxOfficeName", pObjGovernment.TaxOfficeName),
                    new SqlParameter("@TaxPayerTypeName", pObjGovernment.TaxPayerTypeName),
                    new SqlParameter("@ContactName", pObjGovernment.ContactName),
                    new SqlParameter("@ContactEmail", pObjGovernment.ContactEmail),
                    new SqlParameter("@ContactNumber", pObjGovernment.ContactNumber),
                    new SqlParameter("@NotificationMethodName", pObjGovernment.NotificationMethodName),
                    new SqlParameter("@ActiveText", pObjGovernment.ActiveText),
                    new SqlParameter("@MainFilter", pObjGovernment.MainFilter)
                };

                //Get Filtered Count
                int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntFilteredCount;

                return dcData;
            }
        }

        public FuncResponse REP_UpdateTaxOfficer(Government pObjGovernment)
        {
            using (_db = new EIRSEntities())
            {
                Government mObjUpdateGovernment; //Government Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Government


                mObjUpdateGovernment = (from Government in _db.Governments
                                        where Government.GovernmentID == pObjGovernment.GovernmentID
                                        select Government).FirstOrDefault();

                if (mObjUpdateGovernment != null)
                {
                    mObjUpdateGovernment.ModifiedBy = pObjGovernment.ModifiedBy;
                    mObjUpdateGovernment.ModifiedDate = pObjGovernment.ModifiedDate;
                }
                else
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "No Record Found";
                    return mObjFuncResponse;
                }



                mObjUpdateGovernment.TaxOfficerID = pObjGovernment.TaxOfficerID;

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;
                }

                return mObjFuncResponse;
            }
        }


        public IDictionary<string, object> REP_SearchGovernmentForSideMenu(Government pObjGovernment)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>
                {
                    ["GovernmentList"] = _db.usp_SearchGovernmentForSideMenu(pObjGovernment.WhereCondition, pObjGovernment.OrderBy, pObjGovernment.OrderByDirection, pObjGovernment.PageNumber, pObjGovernment.PageSize, pObjGovernment.MainFilter).ToList()
                };

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(GovernmentID) FROM Government").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(GovernmentID) ");
                sbFilteredCountQuery.Append(" FROM Government gov WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjGovernment.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@MainFilter", pObjGovernment.MainFilter)
                };

                //Get Filtered Count
                int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntFilteredCount;

                return dcData;
            }
        }
    }
}
