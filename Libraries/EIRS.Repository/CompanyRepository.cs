using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EIRS.Repository
{
    public class CompanyRepository : ICompanyRepository
    {
        EIRSEntities _db;

        public FuncResponse<Company> REP_InsertUpdateCompany(Company pObjCompany, bool pblnSkipNoValidation = false)
        {
            using (_db = new EIRSEntities())
            {
                Company mObjInsertUpdateCompany; //Company Insert Update Object
                FuncResponse<Company> mObjFuncResponse = new FuncResponse<Company>(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from comp in _db.Companies
                                       where comp.CompanyName == pObjCompany.CompanyName && comp.CompanyID != pObjCompany.CompanyID
                                       select comp);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Company with same name already exists";
                    return mObjFuncResponse;
                }

                //Duplicate Check Mobile No 1
                if (!pblnSkipNoValidation)
                {
                    var vMobileNo1Result = (from comp in _db.Companies
                                            where (comp.MobileNumber1 == pObjCompany.MobileNumber1 || comp.MobileNumber2 == pObjCompany.MobileNumber1) && comp.CompanyID != pObjCompany.CompanyID
                                            select comp);

                    if (vMobileNo1Result.Count() > 0)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = "Mobile No 1 already exists";
                        return mObjFuncResponse;
                    }

                    //Duplicate Check Mobile No 2
                    if (!string.IsNullOrWhiteSpace(pObjCompany.MobileNumber2))
                    {
                        var vMobileNo2Result = (from comp in _db.Companies
                                                where (comp.MobileNumber1 == pObjCompany.MobileNumber2 || comp.MobileNumber2 == pObjCompany.MobileNumber2) && comp.CompanyID != pObjCompany.CompanyID
                                                select comp);

                        if (vMobileNo2Result.Count() > 0)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Message = "Mobile No 2 already exists";
                            return mObjFuncResponse;
                        }
                    }
                }

                //If Update Load Company

                if (pObjCompany.CompanyID != 0)
                {
                    mObjInsertUpdateCompany = (from Company in _db.Companies
                                               where Company.CompanyID == pObjCompany.CompanyID
                                               select Company).FirstOrDefault();

                    if (mObjInsertUpdateCompany != null)
                    {
                        if (pObjCompany.Active == false && mObjInsertUpdateCompany.Active == true)
                        {
                            //Check Conditions for marking inactive to company

                            var vBuildingList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join bld in _db.Buildings on new { BuildingID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { bld.BuildingID, bld.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateCompany.CompanyID && tpa.TaxPayerTypeID == mObjInsertUpdateCompany.TaxPayerTypeID && bld.Active == true
                                                 select tpa);
                            if (vBuildingList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vBusinessList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join bus in _db.Businesses on new { BusinessID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { bus.BusinessID, bus.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateCompany.CompanyID && tpa.TaxPayerTypeID == mObjInsertUpdateCompany.TaxPayerTypeID && bus.Active == true
                                                 select tpa);
                            if (vBusinessList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vLandList = (from tpa in _db.MAP_TaxPayer_Asset
                                             join lnd in _db.Lands on new { LandID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { lnd.LandID, lnd.AssetTypeID }
                                             where tpa.TaxPayerID == mObjInsertUpdateCompany.CompanyID && tpa.TaxPayerTypeID == mObjInsertUpdateCompany.TaxPayerTypeID && lnd.Active == true
                                             select tpa);
                            if (vLandList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vVechicleList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join vch in _db.Vehicles on new { VehicleID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { vch.VehicleID, vch.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateCompany.CompanyID && tpa.TaxPayerTypeID == mObjInsertUpdateCompany.TaxPayerTypeID && vch.Active == true
                                                 select tpa);

                            if (vVechicleList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vAssessmentCount = (from ast in _db.Assessments
                                                    where ast.TaxPayerID == mObjInsertUpdateCompany.CompanyID && ast.TaxPayerTypeID == mObjInsertUpdateCompany.TaxPayerTypeID && ast.Active == true && ast.SettlementStatusID == (int)EnumList.SettlementStatus.Partial
                                                    select ast);
                            if (vAssessmentCount.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are parital settled  assessment bill";
                                return mObjFuncResponse;
                            }

                            var vServiceBillCount = (from sb in _db.ServiceBills
                                                     where sb.TaxPayerID == mObjInsertUpdateCompany.CompanyID && sb.TaxPayerTypeID == mObjInsertUpdateCompany.TaxPayerTypeID && sb.Active == true && sb.SettlementStatusID == (int)EnumList.SettlementStatus.Partial
                                                     select sb);
                            if (vServiceBillCount.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are parital settled service bill";
                                return mObjFuncResponse;
                            }

                            var vPoABalance = _db.usp_GetWalletBalance(mObjInsertUpdateCompany.TaxPayerTypeID, mObjInsertUpdateCompany.CompanyID).FirstOrDefault();
                            if (vPoABalance.GetValueOrDefault() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "PoA Balance is more than 0";
                                return mObjFuncResponse;
                            }
                        }

                        if (pObjCompany.TaxOfficeID != mObjInsertUpdateCompany.TaxOfficeID)
                        {
                            mObjInsertUpdateCompany.TaxOfficerID = null;
                        }

                        mObjInsertUpdateCompany.ModifiedBy = pObjCompany.ModifiedBy;
                        mObjInsertUpdateCompany.ModifiedDate = pObjCompany.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateCompany = new Company();
                        mObjInsertUpdateCompany.CreatedBy = pObjCompany.CreatedBy;
                        mObjInsertUpdateCompany.CreatedDate = pObjCompany.CreatedDate;
                    }
                }
                else // Else Insert Company Completion
                {
                    mObjInsertUpdateCompany = new Company();
                    mObjInsertUpdateCompany.CreatedBy = pObjCompany.CreatedBy;
                    mObjInsertUpdateCompany.CreatedDate = pObjCompany.CreatedDate;
                }

                mObjInsertUpdateCompany.CompanyName = pObjCompany.CompanyName;
                mObjInsertUpdateCompany.TIN = pObjCompany.TIN;
                mObjInsertUpdateCompany.MobileNumber1 = pObjCompany.MobileNumber1;
                mObjInsertUpdateCompany.MobileNumber2 = pObjCompany.MobileNumber2;
                mObjInsertUpdateCompany.EmailAddress1 = pObjCompany.EmailAddress1;
                mObjInsertUpdateCompany.EmailAddress2 = pObjCompany.EmailAddress2;
                mObjInsertUpdateCompany.TaxOfficeID = pObjCompany.TaxOfficeID;
                mObjInsertUpdateCompany.TaxPayerTypeID = pObjCompany.TaxPayerTypeID;
                mObjInsertUpdateCompany.EconomicActivitiesID = pObjCompany.EconomicActivitiesID;
                mObjInsertUpdateCompany.NotificationMethodID = pObjCompany.NotificationMethodID;
                mObjInsertUpdateCompany.ContactAddress = pObjCompany.ContactAddress;
                mObjInsertUpdateCompany.CACRegistrationNumber = pObjCompany.CACRegistrationNumber;
                mObjInsertUpdateCompany.RegisterationStatusID = pObjCompany.RegisterationStatusID == null ? (mObjInsertUpdateCompany.RegisterationStatusID == null ? (int)EnumList.RegisterationStatus.Not_Started : mObjInsertUpdateCompany.RegisterationStatusID) : pObjCompany.RegisterationStatusID;
                mObjInsertUpdateCompany.DataSourceID = pObjCompany.DataSourceID;
                mObjInsertUpdateCompany.DSRefID = pObjCompany.DSRefID;
                mObjInsertUpdateCompany.Active = pObjCompany.Active;

                if (pObjCompany.CompanyID == 0)
                {
                    _db.Companies.Add(mObjInsertUpdateCompany);
                }

                try
                {
                    _db.SaveChanges();

                    var context = ((IObjectContextAdapter)_db).ObjectContext;
                    var refreshableObjects = _db.ChangeTracker.Entries().Select(c => c.Entity).ToList();
                    context.Refresh(RefreshMode.StoreWins, refreshableObjects);

                    mObjFuncResponse.Success = true;
                    if (pObjCompany.CompanyID == 0)
                    {
                        mObjFuncResponse.Message = "Company Added Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Company Updated Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjInsertUpdateCompany;

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjCompany.CompanyID == 0)
                    {
                        mObjFuncResponse.Message = "Company Addition Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Company Updation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetCompanyList_Result> REP_GetCompanyList(Company pObjCompany)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetCompanyList(pObjCompany.CompanyID, pObjCompany.TIN, pObjCompany.CompanyRIN, pObjCompany.CompanyName,pObjCompany.CACRegistrationNumber,  pObjCompany.MobileNumber1, pObjCompany.intStatus).ToList();
            }
        }

        public IList<vw_Company> REP_GetCompanyList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.vw_Company.ToList();
            }
        }

        public IList<DropDownListResult> REP_GetCompanyDropDownList(Company pObjCompany)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from comp in _db.usp_GetCompanyList(pObjCompany.CompanyID, pObjCompany.TIN, pObjCompany.CompanyName, pObjCompany.CompanyRIN, pObjCompany.CACRegistrationNumber, pObjCompany.MobileNumber1, pObjCompany.intStatus)
                               select new DropDownListResult()
                               {
                                   id = comp.CompanyID.GetValueOrDefault(),
                                   text = comp.CompanyName
                               });

                return vResult.ToList();
            }
        }

        public IList<DropDownListResult> REP_GetCompanyDropDownList(string pStrCompanyName, int pIntTaxOfficeID = 0)
        {
            using (_db = new EIRSEntities())
            {
                if (pIntTaxOfficeID != 0)
                {
                    var vResult = (from comp in _db.vw_Company
                                   where (comp.CompanyName.ToLower().Contains(pStrCompanyName.ToLower()) || comp.CompanyRIN.ToLower().Contains(pStrCompanyName.ToLower()))
                                   && comp.TaxOfficeID == pIntTaxOfficeID
                                   select new DropDownListResult()
                                   {
                                       id = comp.CompanyID,
                                       text = comp.CompanyRIN + " - " + comp.CompanyName
                                   });

                    return vResult.ToList();
                }
                else
                {

                    var vResult = (from comp in _db.vw_Company
                                   where comp.CompanyName.ToLower().Contains(pStrCompanyName.ToLower()) || comp.CompanyRIN.ToLower().Contains(pStrCompanyName.ToLower())
                                   select new DropDownListResult()
                                   {
                                       id = comp.CompanyID,
                                       text = comp.CompanyRIN + " - " + comp.CompanyName
                                   });

                    return vResult.ToList();
                }
            }
        }

        public usp_GetCompanyList_Result REP_GetCompanyDetails(Company pObjCompany)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetCompanyList(pObjCompany.CompanyID, pObjCompany.TIN, pObjCompany.CompanyName,pObjCompany.CompanyRIN, pObjCompany.CACRegistrationNumber,  pObjCompany.MobileNumber1, pObjCompany.intStatus).FirstOrDefault();
            }
        }

        public FuncResponse REP_UpdateStatus(Company pObjCompany)
        {
            using (_db = new EIRSEntities())
            {
                Company mObjInsertUpdateCompany; //Company Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Company
                if (pObjCompany.CompanyID != 0)
                {

                    mObjInsertUpdateCompany = (from bcomp in _db.Companies
                                               where bcomp.CompanyID == pObjCompany.CompanyID
                                               select bcomp).FirstOrDefault();

                    if (mObjInsertUpdateCompany != null)
                    {

                        if (mObjInsertUpdateCompany.Active == true)
                        {
                            //Check Conditions for marking inactive to company

                            var vBuildingList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join bld in _db.Buildings on new { BuildingID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { bld.BuildingID, bld.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateCompany.CompanyID && tpa.TaxPayerTypeID == mObjInsertUpdateCompany.TaxPayerTypeID && bld.Active == true
                                                 select tpa);
                            if (vBuildingList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vBusinessList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join bus in _db.Businesses on new { BusinessID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { bus.BusinessID, bus.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateCompany.CompanyID && tpa.TaxPayerTypeID == mObjInsertUpdateCompany.TaxPayerTypeID && bus.Active == true
                                                 select tpa);
                            if (vBusinessList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vLandList = (from tpa in _db.MAP_TaxPayer_Asset
                                             join lnd in _db.Lands on new { LandID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { lnd.LandID, lnd.AssetTypeID }
                                             where tpa.TaxPayerID == mObjInsertUpdateCompany.CompanyID && tpa.TaxPayerTypeID == mObjInsertUpdateCompany.TaxPayerTypeID && lnd.Active == true
                                             select tpa);
                            if (vLandList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vVechicleList = (from tpa in _db.MAP_TaxPayer_Asset
                                                 join vch in _db.Vehicles on new { VehicleID = (int)tpa.AssetID, tpa.AssetTypeID } equals new { vch.VehicleID, vch.AssetTypeID }
                                                 where tpa.TaxPayerID == mObjInsertUpdateCompany.CompanyID && tpa.TaxPayerTypeID == mObjInsertUpdateCompany.TaxPayerTypeID && vch.Active == true
                                                 select tpa);

                            if (vVechicleList.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are active asset associated with tax payer";
                                return mObjFuncResponse;
                            }

                            var vAssessmentCount = (from ast in _db.Assessments
                                                    where ast.TaxPayerID == mObjInsertUpdateCompany.CompanyID && ast.TaxPayerTypeID == mObjInsertUpdateCompany.TaxPayerTypeID && ast.Active == true && ast.SettlementStatusID == (int)EnumList.SettlementStatus.Partial
                                                    select ast);
                            if (vAssessmentCount.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are parital settled  assessment bill";
                                return mObjFuncResponse;
                            }

                            var vServiceBillCount = (from sb in _db.ServiceBills
                                                     where sb.TaxPayerID == mObjInsertUpdateCompany.CompanyID && sb.TaxPayerTypeID == mObjInsertUpdateCompany.TaxPayerTypeID && sb.Active == true && sb.SettlementStatusID == (int)EnumList.SettlementStatus.Partial
                                                     select sb);
                            if (vServiceBillCount.Count() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "There are parital settled service bill";
                                return mObjFuncResponse;
                            }

                            var vPoABalance = _db.usp_GetWalletBalance(mObjInsertUpdateCompany.TaxPayerTypeID, mObjInsertUpdateCompany.CompanyID).FirstOrDefault();
                            if (vPoABalance.GetValueOrDefault() > 0)
                            {
                                mObjFuncResponse.Success = false;
                                mObjFuncResponse.Message = "PoA Balance is more than 0";
                                return mObjFuncResponse;
                            }
                        }

                        mObjInsertUpdateCompany.Active = !mObjInsertUpdateCompany.Active;
                        mObjInsertUpdateCompany.ModifiedBy = pObjCompany.ModifiedBy;
                        mObjInsertUpdateCompany.ModifiedDate = pObjCompany.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Company Updated Successfully";
                            //mObjFuncResponse.AdditionalData = _db.usp_GetCompanyList(0, pObjCompany.TIN, pObjCompany.CompanyName, pObjCompany.CompanyRIN, pObjCompany.MobileNumber1, pObjCompany.intStatus).ToList();



                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Company Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetCompanyAddressInformation_Result> REP_GetAddressInformation(Company pObjCompany)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetCompanyAddressInformation(pObjCompany.CompanyID).ToList();
            }
        }

        public FuncResponse REP_InsertAddressInformation(MAP_Company_AddressInformation pObjAddressInformation)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                _db.MAP_Company_AddressInformation.Add(pObjAddressInformation);

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

        public FuncResponse<IList<usp_GetCompanyAddressInformation_Result>> REP_RemoveAddressInformation(MAP_Company_AddressInformation pObjAddressInformation)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<IList<usp_GetCompanyAddressInformation_Result>> mObjFuncResponse = new FuncResponse<IList<usp_GetCompanyAddressInformation_Result>>(); //Return Object

                MAP_Company_AddressInformation mObjDeleteAddressInformation;

                mObjDeleteAddressInformation = _db.MAP_Company_AddressInformation.Find(pObjAddressInformation.CAIID);

                if (mObjDeleteAddressInformation == null)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Address Information Already Removed.";
                }
                else
                {
                    _db.MAP_Company_AddressInformation.Remove(mObjDeleteAddressInformation);

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;

                        mObjFuncResponse.Message = "Address Information Removed Successfully";

                        mObjFuncResponse.AdditionalData = _db.usp_GetCompanyAddressInformation(pObjAddressInformation.CompanyID).ToList();
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

        public FuncResponse<Company> REP_CheckIndividualLoginDetails(Company pObjCompany)
        {
            FuncResponse<Company> mObjFuncResponse = new FuncResponse<Company>();

            try
            {
                using (_db = new EIRSEntities())
                {

                    var vFind = (from cmp in _db.Companies
                                 where (cmp.CompanyRIN.Equals(pObjCompany.CompanyRIN) && cmp.Password.Equals(pObjCompany.Password))
                                 select cmp);

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

        public FuncResponse REP_ChangePassword(Company pObjCompany)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vUserDetails = (from cmp in _db.Companies
                                    where (cmp.CompanyRIN.Equals(pObjCompany.CompanyRIN) && cmp.Password.Equals(pObjCompany.OldPassword))
                                    select cmp).FirstOrDefault();

                if (vUserDetails != null && vUserDetails.CompanyID > 0)
                {
                    vUserDetails.Password = pObjCompany.Password;

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

        public FuncResponse REP_UpdatePassword(Company pObjCompany)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vUserDetails = (from usr in _db.Companies
                                    where usr.CompanyID == pObjCompany.CompanyID
                                    select usr).FirstOrDefault();

                if (vUserDetails != null)
                {
                    vUserDetails.RegisterationStatusID = pObjCompany.RegisterationStatusID;
                    vUserDetails.RegisterationDate = pObjCompany.RegisterationDate;
                    vUserDetails.Password = pObjCompany.Password;

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

        public void REP_UpdateRegisterationStatus(Company pObjCompany)
        {
            using (_db = new EIRSEntities())
            {
                Company mObjUpdateCompany = _db.Companies.Find(pObjCompany.CompanyID);

                if (mObjUpdateCompany != null)
                {
                    mObjUpdateCompany.RegisterationStatusID = pObjCompany.RegisterationStatusID;

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

        public void REP_UpdateOTPCode(Company pObjCompany)
        {
            using (_db = new EIRSEntities())
            {
                Company mObjUpdateCompany = _db.Companies.Find(pObjCompany.CompanyID);

                if (mObjUpdateCompany != null)
                {
                    mObjUpdateCompany.VerificationOTP = pObjCompany.VerificationOTP;

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

        public FuncResponse REP_CheckOTPCode(Company pObjCompany)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();
                Company mObjUpdateCompany = _db.Companies.Find(pObjCompany.CompanyID);

                if (mObjUpdateCompany != null)
                {
                    if (mObjUpdateCompany.VerificationOTP == pObjCompany.VerificationOTP)
                    {
                        mObjResponse.Success = true;
                        mObjUpdateCompany.VerificationOTP = null;

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

        //public IList<usp_GetCompanyDetails_Result> REP_SearchCompanyDetails(Company pObjCompany)
        //{
        //    using (_db = new EIRSEntities())
        //    {
        //        return _db.usp_GetCompanyDetails(pObjCompany.CompanyID, pObjCompany.TIN, pObjCompany.CompanyName, pObjCompany.CompanyRIN, pObjCompany.MobileNumber1, pObjCompany.MobileNumber2, pObjCompany.EmailAddress1, pObjCompany.EmailAddress2, pObjCompany.TaxOfficeName, pObjCompany.TaxPayerTypeName, pObjCompany.EconomicActivitiesName, pObjCompany.NotificationMethodName, pObjCompany.intStatus).ToList();
        //    }
        //}

        public IList<usp_SearchCompanyForRDMLoad_Result> REP_SearchCompanyDetails(Company pObjCompany)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_SearchCompanyForRDMLoad(pObjCompany.TIN, pObjCompany.CompanyName, pObjCompany.CompanyRIN, pObjCompany.MobileNumber1, pObjCompany.MobileNumber2, pObjCompany.EmailAddress1, pObjCompany.EmailAddress2, pObjCompany.TaxOfficeName, pObjCompany.TaxPayerTypeName, pObjCompany.EconomicActivitiesName, pObjCompany.NotificationMethodName, pObjCompany.ActiveText).ToList();
            }
        }

        public IDictionary<string, object> REP_SearchCompany(Company pObjCompany)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>();
                dcData["CompanyList"] = _db.usp_SearchCompany(pObjCompany.WhereCondition, pObjCompany.OrderBy, pObjCompany.OrderByDirection, pObjCompany.PageNumber, pObjCompany.PageSize, pObjCompany.MainFilter,
                                                                   pObjCompany.TIN, pObjCompany.CompanyName, pObjCompany.CompanyRIN, pObjCompany.MobileNumber1, pObjCompany.MobileNumber2,
                                                                   pObjCompany.EmailAddress1, pObjCompany.EmailAddress2, pObjCompany.TaxOfficeName, pObjCompany.TaxPayerTypeName, pObjCompany.EconomicActivitiesName,
                                                                   pObjCompany.NotificationMethodName, pObjCompany.ActiveText).ToList();

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(CompanyID) FROM Company").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(CompanyID) ");
                sbFilteredCountQuery.Append(" FROM Company comp ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Tax_Offices toff ON comp.TaxOfficeID = toff.TaxOfficeID ");
                sbFilteredCountQuery.Append(" INNER JOIN TaxPayer_Types tptype ON comp.TaxPayerTypeID = tptype.TaxPayerTypeID ");
                sbFilteredCountQuery.Append(" INNER JOIN Economic_Activities eact ON comp.EconomicActivitiesID = eact.EconomicActivitiesID ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Notification_Method nmthd ON comp.NotificationMethodID = nmthd.NotificationMethodID ");
                sbFilteredCountQuery.Append(" INNER JOIN MST_RegisterationStatus rgs ON comp.RegisterationStatusID = rgs.RegisterationStatusID WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjCompany.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@CompanyName", pObjCompany.CompanyName),
                    new SqlParameter("@TIN", pObjCompany.TIN),
                    new SqlParameter("@CompanyRIN", pObjCompany.CompanyRIN),
                    new SqlParameter("@MobileNumber1", pObjCompany.MobileNumber1),
                    new SqlParameter("@MobileNumber2", pObjCompany.MobileNumber2),
                    new SqlParameter("@EmailAddress1", pObjCompany.EmailAddress1),
                    new SqlParameter("@EmailAddress2", pObjCompany.EmailAddress2),
                    new SqlParameter("@TaxOfficeName", pObjCompany.TaxOfficeName),
                    new SqlParameter("@TaxPayerTypeName", pObjCompany.TaxPayerTypeName),
                    new SqlParameter("@EconomicActivitiesName", pObjCompany.EconomicActivitiesName),
                    new SqlParameter("@NotificationMethodName", pObjCompany.NotificationMethodName),
                    new SqlParameter("@ActiveText", pObjCompany.ActiveText),
                    new SqlParameter("@MainFilter", pObjCompany.MainFilter)
                };

                //Get Filtered Count
                int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntFilteredCount;

                return dcData;
            }
        }


        public FuncResponse REP_UpdateTaxOfficer(Company pObjCompany)
        {
            using (_db = new EIRSEntities())
            {
                Company mObjUpdateCompany; //Company Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Company


                mObjUpdateCompany = (from Company in _db.Companies
                                     where Company.CompanyID == pObjCompany.CompanyID
                                     select Company).FirstOrDefault();

                if (mObjUpdateCompany != null)
                {
                    mObjUpdateCompany.ModifiedBy = pObjCompany.ModifiedBy;
                    mObjUpdateCompany.ModifiedDate = pObjCompany.ModifiedDate;
                }
                else
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "No Record Found";
                    return mObjFuncResponse;
                }



                mObjUpdateCompany.TaxOfficerID = pObjCompany.TaxOfficerID;

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

        public IDictionary<string, object> REP_SearchCompanyForSideMenu(Company pObjCompany)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>();
                dcData["CompanyList"] = _db.usp_SearchCompanyForSideMenu(pObjCompany.WhereCondition, pObjCompany.OrderBy, pObjCompany.OrderByDirection, pObjCompany.PageNumber, pObjCompany.PageSize, pObjCompany.MainFilter).ToList();

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(CompanyID) FROM Company").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(CompanyID) ");
                sbFilteredCountQuery.Append(" FROM Company comp WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjCompany.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@MainFilter", pObjCompany.MainFilter)
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
