using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using EIRS.BOL;
using EIRS.Common;
using Newtonsoft.Json;

namespace EIRS.Repository
{
    public class IndividualRepository : IIndividualRepository
    {
        EIRSEntities _db;

        public FuncResponse<Individual> REP_InsertUpdateIndividual(Individual pObjIndividual, bool pblnSkipNoValidation = false)
        {
            NewErrorLog.WriteFormModel("I got here 1", "SettlementResponse");
            using (_db = new EIRSEntities())
            {
                Individual mObjInsertUpdateIndividual; //Individual Insert Update Object
                FuncResponse<Individual> mObjFuncResponse = new FuncResponse<Individual>(); //Return Object

                //Duplicate Check Mobile No 1

                if (!pblnSkipNoValidation)
                {

                    var vMobileNo1Result = (from ind in _db.Individuals
                                            where (ind.MobileNumber1 == pObjIndividual.MobileNumber1 || ind.MobileNumber2 == pObjIndividual.MobileNumber1) && ind.IndividualID != pObjIndividual.IndividualID
                                            select ind);

                    if (vMobileNo1Result.Count() > 0)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = "Mobile No 1 already exists";
                        return mObjFuncResponse;
                    }


                    //Duplicate Check Mobile No 2

                    var vMobileNo2Result = (from ind in _db.Individuals
                                            where (ind.MobileNumber1 == pObjIndividual.MobileNumber2 || ind.MobileNumber2 == pObjIndividual.MobileNumber2) && ind.IndividualID != pObjIndividual.IndividualID
                                            select ind);

                    if (vMobileNo2Result.Count() > 0 && !string.IsNullOrWhiteSpace(pObjIndividual.MobileNumber2))
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = "Mobile No 2 already exists";
                        return mObjFuncResponse;
                    }
                }

                //If Update Load Individual

                if (pObjIndividual.IndividualID != 0)
                {
                    mObjInsertUpdateIndividual = (from Individual in _db.Individuals
                                                  where Individual.IndividualID == pObjIndividual.IndividualID
                                                  select Individual).FirstOrDefault();

                    if (mObjInsertUpdateIndividual != null)
                    {
                        if (pObjIndividual.Active == false && mObjInsertUpdateIndividual.Active == true)
                        {
                            //Check Conditions for marking inactive to company

                            var vBuildingList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join bld in _db.Buildings on new { BuildingID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { bld.BuildingID, bld.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateIndividual.IndividualID && tpa.TaxPayerTypeID == mObjInsertUpdateIndividual.TaxPayerTypeID && bld.Active == true
                                                 select tpa);
                            if (vBuildingList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vBusinessList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join bus in _db.Businesses on new { BusinessID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { bus.BusinessID, bus.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateIndividual.IndividualID && tpa.TaxPayerTypeID == mObjInsertUpdateIndividual.TaxPayerTypeID && bus.Active == true
                                                 select tpa);
                            if (vBusinessList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vLandList = (from tpa in _db.MAP_TaxPayer_Asset
                                             join lnd in _db.Lands on new { LandID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { lnd.LandID, lnd.AssetTypeID }
                                             where tpa.TaxPayerID == mObjInsertUpdateIndividual.IndividualID && tpa.TaxPayerTypeID == mObjInsertUpdateIndividual.TaxPayerTypeID && lnd.Active == true
                                             select tpa);
                            if (vLandList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vVechicleList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join vch in _db.Vehicles on new { VehicleID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { vch.VehicleID, vch.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateIndividual.IndividualID && tpa.TaxPayerTypeID == mObjInsertUpdateIndividual.TaxPayerTypeID && vch.Active == true
                                                 select tpa);

                            if (vVechicleList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vAssessmentCount = (from ast in _db.Assessments
                                                    where ast.TaxPayerID == mObjInsertUpdateIndividual.IndividualID && ast.TaxPayerTypeID == mObjInsertUpdateIndividual.TaxPayerTypeID && ast.Active == true && ast.SettlementStatusID == (int)EnumList.SettlementStatus.Partial
                                                    select ast);
                            if (vAssessmentCount.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are parital settled  assessment bill";
                                return mObjFuncResponse;
                            }

                            var vServiceBillCount = (from sb in _db.ServiceBills
                                                     where sb.TaxPayerID == mObjInsertUpdateIndividual.IndividualID && sb.TaxPayerTypeID == mObjInsertUpdateIndividual.TaxPayerTypeID && sb.Active == true && sb.SettlementStatusID == (int)EnumList.SettlementStatus.Partial
                                                     select sb);
                            if (vServiceBillCount.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are parital settled service bill";
                                return mObjFuncResponse;
                            }

                            var vPoABalance = _db.usp_GetWalletBalance(mObjInsertUpdateIndividual.TaxPayerTypeID, mObjInsertUpdateIndividual.IndividualID).FirstOrDefault();
                            if (vPoABalance.GetValueOrDefault() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "PoA Balance is more than 0";
                                return mObjFuncResponse;
                            }
                        }

                        if (pObjIndividual.TaxOfficeID != mObjInsertUpdateIndividual.TaxOfficeID)
                        {
                            mObjInsertUpdateIndividual.TaxOfficerID = null;
                        }

                        mObjInsertUpdateIndividual.ModifiedBy = pObjIndividual.ModifiedBy;
                        mObjInsertUpdateIndividual.ModifiedDate = pObjIndividual.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateIndividual = new Individual();
                        mObjInsertUpdateIndividual.CreatedBy = pObjIndividual.CreatedBy;
                        mObjInsertUpdateIndividual.CreatedDate = pObjIndividual.CreatedDate;
                    }
                }
                else // Else Insert Individual Completion
                {
                    mObjInsertUpdateIndividual = new Individual();
                    mObjInsertUpdateIndividual.CreatedBy = pObjIndividual.CreatedBy;
                    mObjInsertUpdateIndividual.CreatedDate = pObjIndividual.CreatedDate;
                }

                mObjInsertUpdateIndividual.GenderID = pObjIndividual.GenderID;
                mObjInsertUpdateIndividual.TitleID = pObjIndividual.TitleID;
                mObjInsertUpdateIndividual.FirstName = pObjIndividual.FirstName;
                mObjInsertUpdateIndividual.LastName = pObjIndividual.LastName;
                mObjInsertUpdateIndividual.MiddleName = pObjIndividual.MiddleName;
                mObjInsertUpdateIndividual.DOB = pObjIndividual.DOB;
                mObjInsertUpdateIndividual.TIN = pObjIndividual.TIN;
                mObjInsertUpdateIndividual.NIN = pObjIndividual.NIN;
                mObjInsertUpdateIndividual.MobileNumber1 = pObjIndividual.MobileNumber1;
                mObjInsertUpdateIndividual.MobileNumber2 = pObjIndividual.MobileNumber2;
                mObjInsertUpdateIndividual.EmailAddress1 = pObjIndividual.EmailAddress1;
                mObjInsertUpdateIndividual.EmailAddress2 = pObjIndividual.EmailAddress2;
                mObjInsertUpdateIndividual.BiometricDetails = pObjIndividual.BiometricDetails;
                mObjInsertUpdateIndividual.TaxOfficeID = pObjIndividual.TaxOfficeID;
                mObjInsertUpdateIndividual.MaritalStatusID = pObjIndividual.MaritalStatusID;
                mObjInsertUpdateIndividual.NationalityID = pObjIndividual.NationalityID;
                mObjInsertUpdateIndividual.TaxPayerTypeID = pObjIndividual.TaxPayerTypeID;
                mObjInsertUpdateIndividual.EconomicActivitiesID = pObjIndividual.EconomicActivitiesID;
                mObjInsertUpdateIndividual.NotificationMethodID = pObjIndividual.NotificationMethodID;
                mObjInsertUpdateIndividual.ContactAddress = pObjIndividual.ContactAddress;
                mObjInsertUpdateIndividual.RegisterationStatusID = pObjIndividual.RegisterationStatusID == null ? (mObjInsertUpdateIndividual.RegisterationStatusID == null ? (int)EnumList.RegisterationStatus.Not_Started : mObjInsertUpdateIndividual.RegisterationStatusID) : pObjIndividual.RegisterationStatusID;
                mObjInsertUpdateIndividual.Active = pObjIndividual.Active;
                mObjInsertUpdateIndividual.DataSourceID = pObjIndividual.DataSourceID;
                mObjInsertUpdateIndividual.DSRefID = pObjIndividual.DSRefID;

                if (pObjIndividual.IndividualID == 0)
                {
                    _db.Individuals.Add(mObjInsertUpdateIndividual);

                    NewErrorLog.WriteFormModel("I got here 2", "SettlementResponse");
                }

                try
                {
                    _db.SaveChanges();

                    NewErrorLog.WriteFormModel("I got here 1", "SettlementResponse");

                    var context = ((IObjectContextAdapter)_db).ObjectContext;
                    var refreshableObjects = _db.ChangeTracker.Entries().Select(c => c.Entity).ToList();
                    context.Refresh(RefreshMode.StoreWins, refreshableObjects);

                    mObjFuncResponse.Success = true;
                    if (pObjIndividual.IndividualID == 0)
                    {
                        mObjFuncResponse.Message = "Individual Added Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Individual Updated Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjInsertUpdateIndividual;

                }
                catch (Exception Ex)
                {

                    NewErrorLog.WriteFormModel($"I got here 4 because of {Ex.Message}", "SettlementResponse");
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjIndividual.IndividualID == 0)
                    {
                        mObjFuncResponse.Message = "Individual Addition Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Individual Updation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetIndividualList_Result> REP_GetIndividualList(Individual pObjIndividual)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetIndividualList(pObjIndividual.IndividualID, pObjIndividual.IndividualName, pObjIndividual.TIN, pObjIndividual.NIN, pObjIndividual.IndividualRIN, pObjIndividual.MobileNumber1, pObjIndividual.intStatus).ToList();
            }
        }

        public IList<vw_Individual> REP_GetIndividualList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.vw_Individual.ToList();
            }
        }

        public usp_GetIndividualList_Result REP_GetIndividualDetails(Individual pObjIndividual)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetIndividualList(pObjIndividual.IndividualID, pObjIndividual.IndividualName, pObjIndividual.TIN, pObjIndividual.NIN, pObjIndividual.IndividualRIN, pObjIndividual.MobileNumber1, pObjIndividual.intStatus).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetIndividualDropDownList(Individual pObjIndividual)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from ind in _db.usp_GetIndividualList(pObjIndividual.IndividualID, pObjIndividual.IndividualName, pObjIndividual.TIN,pObjIndividual.NIN,pObjIndividual.IndividualRIN, pObjIndividual.MobileNumber1, pObjIndividual.intStatus)
                               select new DropDownListResult()
                               {
                                   id = ind.IndividualID.GetValueOrDefault(),
                                   text = ind.FirstName + " " + ind.LastName
                               });

                return vResult.ToList();
            }
        }

        public IList<DropDownListResult> REP_GetIndividualDropDownList(string pStrIndividualName, int pIntTaxOfficeID = 0)
        {
            using (_db = new EIRSEntities())
            {
                if (pIntTaxOfficeID != 0)
                {
                    var vResult = (from ind in _db.vw_Individual
                                   where (ind.IndividualName.ToLower().Contains(pStrIndividualName.ToLower()) || ind.IndividualRIN.ToLower().Contains(pStrIndividualName.ToLower()))
                                   && ind.TaxOfficeID == pIntTaxOfficeID
                                   select new DropDownListResult()
                                   {
                                       id = ind.IndividualID,
                                       text = ind.IndividualRIN + " - " + ind.IndividualName
                                   });

                    return vResult.ToList();
                }
                else
                {

                    var vResult = (from ind in _db.vw_Individual
                                   where ind.IndividualName.ToLower().Contains(pStrIndividualName.ToLower()) || ind.IndividualRIN.ToLower().Contains(pStrIndividualName.ToLower())
                                   select new DropDownListResult()
                                   {
                                       id = ind.IndividualID,
                                       text = ind.IndividualRIN + " - " + ind.IndividualName
                                   });

                    return vResult.ToList();
                }
            }
        }

        public FuncResponse REP_UpdateStatus(Individual pObjIndividual)
        {
            using (_db = new EIRSEntities())
            {
                Individual mObjInsertUpdateIndividual; //Individual Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Individual
                if (pObjIndividual.IndividualID != 0)
                {



                    mObjInsertUpdateIndividual = (from bcomp in _db.Individuals
                                                  where bcomp.IndividualID == pObjIndividual.IndividualID
                                                  select bcomp).FirstOrDefault();

                    if (mObjInsertUpdateIndividual != null)
                    {
                        if (mObjInsertUpdateIndividual.Active == true)
                        {
                            //Check Conditions for marking inactive to company

                            var vBuildingList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join bld in _db.Buildings on new { BuildingID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { bld.BuildingID, bld.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateIndividual.IndividualID && tpa.TaxPayerTypeID == mObjInsertUpdateIndividual.TaxPayerTypeID && bld.Active == true
                                                 select tpa);
                            if (vBuildingList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vBusinessList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join bus in _db.Businesses on new { BusinessID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { bus.BusinessID, bus.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateIndividual.IndividualID && tpa.TaxPayerTypeID == mObjInsertUpdateIndividual.TaxPayerTypeID && bus.Active == true
                                                 select tpa);
                            if (vBusinessList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vLandList = (from tpa in _db.MAP_TaxPayer_Asset
                                             join lnd in _db.Lands on new { LandID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { lnd.LandID, lnd.AssetTypeID }
                                             where tpa.TaxPayerID == mObjInsertUpdateIndividual.IndividualID && tpa.TaxPayerTypeID == mObjInsertUpdateIndividual.TaxPayerTypeID && lnd.Active == true
                                             select tpa);
                            if (vLandList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vVechicleList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join vch in _db.Vehicles on new { VehicleID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { vch.VehicleID, vch.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateIndividual.IndividualID && tpa.TaxPayerTypeID == mObjInsertUpdateIndividual.TaxPayerTypeID && vch.Active == true
                                                 select tpa);

                            if (vVechicleList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vAssessmentCount = (from ast in _db.Assessments
                                                    where ast.TaxPayerID == mObjInsertUpdateIndividual.IndividualID && ast.TaxPayerTypeID == mObjInsertUpdateIndividual.TaxPayerTypeID && ast.Active == true && ast.SettlementStatusID == (int)EnumList.SettlementStatus.Partial
                                                    select ast);
                            if (vAssessmentCount.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are parital settled  assessment bill";
                                return mObjFuncResponse;
                            }

                            var vServiceBillCount = (from sb in _db.ServiceBills
                                                     where sb.TaxPayerID == mObjInsertUpdateIndividual.IndividualID && sb.TaxPayerTypeID == mObjInsertUpdateIndividual.TaxPayerTypeID && sb.Active == true && sb.SettlementStatusID == (int)EnumList.SettlementStatus.Partial
                                                     select sb);
                            if (vServiceBillCount.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are parital settled service bill";
                                return mObjFuncResponse;
                            }

                            var vPoABalance = _db.usp_GetWalletBalance(mObjInsertUpdateIndividual.TaxPayerTypeID, mObjInsertUpdateIndividual.IndividualID).FirstOrDefault();
                            if (vPoABalance.GetValueOrDefault() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "PoA Balance is more than 0";
                                return mObjFuncResponse;
                            }
                        }


                        mObjInsertUpdateIndividual.Active = !mObjInsertUpdateIndividual.Active;
                        mObjInsertUpdateIndividual.ModifiedBy = pObjIndividual.ModifiedBy;
                        mObjInsertUpdateIndividual.ModifiedDate = pObjIndividual.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Individual Updated Successfully";
                            //mObjFuncResponse.AdditionalData = _db.usp_GetIndividualList(0, pObjIndividual.IndividualName, pObjIndividual.TIN, pObjIndividual.IndividualRIN, pObjIndividual.MobileNumber1, pObjIndividual.intStatus).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Individual Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetIndividualAddressInformation_Result> REP_GetAddressInformation(Individual pObjIndividual)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetIndividualAddressInformation(pObjIndividual.IndividualID).ToList();
            }
        }

        public FuncResponse REP_InsertAddressInformation(MAP_Individual_AddressInformation pObjAddressInformation)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                _db.MAP_Individual_AddressInformation.Add(pObjAddressInformation);

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

        public FuncResponse<IList<usp_GetIndividualAddressInformation_Result>> REP_RemoveAddressInformation(MAP_Individual_AddressInformation pObjAddressInformation)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<IList<usp_GetIndividualAddressInformation_Result>> mObjFuncResponse = new FuncResponse<IList<usp_GetIndividualAddressInformation_Result>>(); //Return Object

                MAP_Individual_AddressInformation mObjDeleteAddressInformation;

                mObjDeleteAddressInformation = _db.MAP_Individual_AddressInformation.Find(pObjAddressInformation.IAIID);

                if (mObjDeleteAddressInformation == null)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Address Information Already Removed.";
                }
                else
                {
                    _db.MAP_Individual_AddressInformation.Remove(mObjDeleteAddressInformation);

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;

                        mObjFuncResponse.Message = "Address Information Removed Successfully";

                        mObjFuncResponse.AdditionalData = _db.usp_GetIndividualAddressInformation(pObjAddressInformation.IndividualID).ToList();
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

        public FuncResponse<Individual> REP_CheckIndividualLoginDetails(Individual pObjIndividual)
        {
            FuncResponse<Individual> mObjFuncResponse = new FuncResponse<Individual>();

            try
            {
                using (_db = new EIRSEntities())
                {

                    var vFind = (from ind in _db.Individuals
                                 where (ind.IndividualRIN.Equals(pObjIndividual.IndividualRIN) || ind.MobileNumber1.Equals(pObjIndividual.MobileNumber1) && ind.Password.Equals(pObjIndividual.Password))
                                 select ind);

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

        public FuncResponse REP_ChangePassword(Individual pObjIndividual)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vUserDetails = (from usr in _db.Individuals
                                    where (usr.IndividualRIN.Equals(pObjIndividual.IndividualRIN) && usr.Password.Equals(pObjIndividual.OldPassword))
                                    select usr).FirstOrDefault();

                if (vUserDetails != null && vUserDetails.IndividualID > 0)
                {
                    vUserDetails.Password = pObjIndividual.Password;

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

        public FuncResponse REP_UpdatePassword(Individual pObjIndividual)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vUserDetails = (from usr in _db.Individuals
                                    where usr.IndividualID == pObjIndividual.IndividualID
                                    select usr).FirstOrDefault();

                if (vUserDetails != null)
                {
                    vUserDetails.RegisterationStatusID = pObjIndividual.RegisterationStatusID;
                    vUserDetails.RegisterationDate = pObjIndividual.RegisterationDate;
                    vUserDetails.Password = pObjIndividual.Password;

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

        public void REP_UpdateRegisterationStatus(Individual pObjIndividual)
        {
            using (_db = new EIRSEntities())
            {
                Individual mObjUpdateIndividual = _db.Individuals.Find(pObjIndividual.IndividualID);

                if (mObjUpdateIndividual != null)
                {
                    mObjUpdateIndividual.RegisterationStatusID = pObjIndividual.RegisterationStatusID;

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

        public void REP_UpdateOTPCode(Individual pObjIndividual)
        {
            using (_db = new EIRSEntities())
            {
                Individual mObjUpdateIndividual = _db.Individuals.Find(pObjIndividual.IndividualID);

                if (mObjUpdateIndividual != null)
                {
                    mObjUpdateIndividual.VerificationOTP = pObjIndividual.VerificationOTP;

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

        public FuncResponse REP_CheckOTPCode(Individual pObjIndividual)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();
                Individual mObjUpdateIndividual = _db.Individuals.Find(pObjIndividual.IndividualID);

                if (mObjUpdateIndividual != null)
                {
                    if (mObjUpdateIndividual.VerificationOTP == pObjIndividual.VerificationOTP)
                    {
                        mObjResponse.Success = true;
                        mObjUpdateIndividual.VerificationOTP = null;

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

        public IList<usp_SearchIndividualForRDMLoad_Result> REP_SearchIndividualDetails(Individual pObjIndividual)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_SearchIndividualForRDMLoad(pObjIndividual.IndividualRIN, pObjIndividual.GenderName, pObjIndividual.TitleName, pObjIndividual.FirstName, pObjIndividual.MiddleName, pObjIndividual.LastName, pObjIndividual.TIN, pObjIndividual.MobileNumber1, pObjIndividual.MobileNumber2, pObjIndividual.EmailAddress1, pObjIndividual.EmailAddress2, pObjIndividual.BiometricDetails, pObjIndividual.TaxOfficeName, pObjIndividual.MaritalStatusName, pObjIndividual.NationalityName, pObjIndividual.TaxPayerTypeName, pObjIndividual.EconomicActivitiesName, pObjIndividual.NotificationMethodName, pObjIndividual.ActiveText, pObjIndividual.StrDOB).ToList();
            }
        }

        public IDictionary<string, object> REP_SearchIndividual(Individual pObjIndividual)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>();
                dcData["IndivudalList"] = _db.usp_SearchIndividual(pObjIndividual.WhereCondition, pObjIndividual.OrderBy, pObjIndividual.OrderByDirection, pObjIndividual.PageNumber, pObjIndividual.PageSize, pObjIndividual.MainFilter,
                                                                   pObjIndividual.IndividualRIN, pObjIndividual.GenderName, pObjIndividual.TitleName, pObjIndividual.FirstName, pObjIndividual.MiddleName,
                                                                   pObjIndividual.LastName, pObjIndividual.TIN, pObjIndividual.MobileNumber1, pObjIndividual.MobileNumber2, pObjIndividual.EmailAddress1,
                                                                   pObjIndividual.EmailAddress2, pObjIndividual.BiometricDetails, pObjIndividual.TaxOfficeName, pObjIndividual.MaritalStatusName,
                                                                   pObjIndividual.NationalityName, pObjIndividual.TaxPayerTypeName, pObjIndividual.EconomicActivitiesName,
                                                                   pObjIndividual.NotificationMethodName, pObjIndividual.ActiveText, pObjIndividual.StrDOB).ToList();

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(IndividualID) FROM Individual").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(IndividualID) ");
                sbFilteredCountQuery.Append(" FROM Individual ind ");
                sbFilteredCountQuery.Append(" INNER JOIN Gender gend ON ind.GenderID = gend.GenderID ");
                sbFilteredCountQuery.Append(" INNER JOIN Titles tit ON ind.TitleID = tit.TitleID ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Tax_Offices toff ON ind.TaxOfficeID = toff.TaxOfficeID ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN MaritalStatus mstat ON ind.MaritalStatusID = mstat.MaritalStatusID ");
                sbFilteredCountQuery.Append(" INNER JOIN Nationality nat ON ind.NationalityID = nat.NationalityID ");
                sbFilteredCountQuery.Append(" INNER JOIN TaxPayer_Types tptype ON ind.TaxPayerTypeID = tptype.TaxPayerTypeID ");
                sbFilteredCountQuery.Append(" INNER JOIN Economic_Activities eact ON ind.EconomicActivitiesID = eact.EconomicActivitiesID ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Notification_Method nmthd ON ind.NotificationMethodID = nmthd.NotificationMethodID ");
                sbFilteredCountQuery.Append(" INNER JOIN MST_RegisterationStatus rgs ON ind.RegisterationStatusID = rgs.RegisterationStatusID WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjIndividual.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@IndividualRIN", pObjIndividual.IndividualRIN ?? (object)DBNull.Value),
                    new SqlParameter("@GenderName", pObjIndividual.GenderName ?? (object)DBNull.Value),
                    new SqlParameter("@TitleName", pObjIndividual.TitleName ?? (object)DBNull.Value),
                    new SqlParameter("@FirstName", pObjIndividual.FirstName ?? (object)DBNull.Value),
                    new SqlParameter("@MiddleName", pObjIndividual.MiddleName ?? (object)DBNull.Value),
                    new SqlParameter("@LastName", pObjIndividual.LastName ?? (object)DBNull.Value),
                    new SqlParameter("@TIN", pObjIndividual.TIN ?? (object)DBNull.Value),
                    new SqlParameter("@MobileNumber1", pObjIndividual.MobileNumber1 ?? (object)DBNull.Value),
                    new SqlParameter("@MobileNumber2", pObjIndividual.MobileNumber2 ?? (object)DBNull.Value),
                    new SqlParameter("@EmailAddress1", pObjIndividual.EmailAddress1 ?? (object)DBNull.Value),
                    new SqlParameter("@EmailAddress2", pObjIndividual.EmailAddress2 ?? (object)DBNull.Value),
                    new SqlParameter("@BiometricDetails", pObjIndividual.BiometricDetails ?? (object)DBNull.Value),
                    new SqlParameter("@TaxOfficeName", pObjIndividual.TaxOfficeName ?? (object)DBNull.Value),
                    new SqlParameter("@MaritalStatusName", pObjIndividual.MaritalStatusName ?? (object)DBNull.Value),
                    new SqlParameter("@NationalityName", pObjIndividual.NationalityName ?? (object)DBNull.Value),
                    new SqlParameter("@TaxPayerTypeName", pObjIndividual.TaxPayerTypeName ?? (object)DBNull.Value),
                    new SqlParameter("@EconomicActivitiesName", pObjIndividual.EconomicActivitiesName ?? (object)DBNull.Value),
                    new SqlParameter("@NotificationMethodName", pObjIndividual.NotificationMethodName ?? (object)DBNull.Value),
                    new SqlParameter("@ActiveText", pObjIndividual.ActiveText ?? (object)DBNull.Value),
                    new SqlParameter("@DOB", pObjIndividual.StrDOB ?? (object)DBNull.Value),
                    new SqlParameter("@MainFilter", pObjIndividual.MainFilter ?? (object)DBNull.Value)
                };

                //Get Filtered Count
                int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntFilteredCount;

                return dcData;
            }
        }

        public FuncResponse REP_UpdateTaxOfficer(Individual pObjIndividual)
        {
            using (_db = new EIRSEntities())
            {
                Individual mObjUpdateIndividual; //Individual Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Individual


                mObjUpdateIndividual = (from Individual in _db.Individuals
                                        where Individual.IndividualID == pObjIndividual.IndividualID
                                        select Individual).FirstOrDefault();

                if (mObjUpdateIndividual != null)
                {
                    mObjUpdateIndividual.ModifiedBy = pObjIndividual.ModifiedBy;
                    mObjUpdateIndividual.ModifiedDate = pObjIndividual.ModifiedDate;
                }
                else
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "No Record Found";
                    return mObjFuncResponse;
                }



                mObjUpdateIndividual.TaxOfficerID = pObjIndividual.TaxOfficerID;
 
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

        public IDictionary<string, object> REP_SearchIndividualForSideMenu(Individual pObjIndividual)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>();
                dcData["IndivudalList"] = _db.usp_SearchIndividualForSideMenu(pObjIndividual.WhereCondition, pObjIndividual.OrderBy, pObjIndividual.OrderByDirection, pObjIndividual.PageNumber, pObjIndividual.PageSize, pObjIndividual.MainFilter).ToList();

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(IndividualID) FROM Individual").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(IndividualID) ");
                sbFilteredCountQuery.Append(" FROM Individual ind WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjIndividual.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@MainFilter", pObjIndividual.MainFilter ?? (object)DBNull.Value)
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
