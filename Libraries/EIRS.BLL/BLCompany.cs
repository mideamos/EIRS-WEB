using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLCompany
    {
        ICompanyRepository _CompanyRepository;
        IUtility _utility;

        public BLCompany()
        {
            _CompanyRepository = new CompanyRepository(); 
            _utility = new Utility();
        }

        public IList<usp_GetCompanyList_Result> BL_GetCompanyList(Company pObjCompany)
        {
            return _CompanyRepository.REP_GetCompanyList(pObjCompany);
        }

        public IList<vw_Company> BL_GetCompanyList()
        {
            return _CompanyRepository.REP_GetCompanyList();
        }

        public IList<DropDownListResult> BL_GetCompanyDropDownList(Company pObjCompany)
        {
            return _CompanyRepository.REP_GetCompanyDropDownList(pObjCompany);
        }

        public IList<DropDownListResult> BL_GetCompanyDropDownList(string pStrCompanyName, int pIntTaxOfficeID = 0)
        {
            return _CompanyRepository.REP_GetCompanyDropDownList(pStrCompanyName,pIntTaxOfficeID);
        }

        public FuncResponse<Company> BL_InsertUpdateCompany(Company pObjCompany, bool pblnSendNotification = true, bool pblnSkipNoValidation = false)
        {
            FuncResponse<Company> mObjFuncResponse = _CompanyRepository.REP_InsertUpdateCompany(pObjCompany, pblnSkipNoValidation);

            if (pObjCompany.CompanyID == 0 && mObjFuncResponse.Success && GlobalDefaultValues.SendNotification && pblnSendNotification)
            {
                //Send Notification
                EmailDetails mObjEmailDetails = new EmailDetails()
                {
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                    TaxPayerTypeName = "Company",
                    TaxPayerID = mObjFuncResponse.AdditionalData.CompanyID,
                    TaxPayerName = mObjFuncResponse.AdditionalData.CompanyName,
                    TaxPayerRIN = mObjFuncResponse.AdditionalData.CompanyRIN,
                    TaxPayerMobileNumber = mObjFuncResponse.AdditionalData.MobileNumber1,
                    TaxPayerEmail = mObjFuncResponse.AdditionalData.EmailAddress1,
                    ContactAddress = mObjFuncResponse.AdditionalData.ContactAddress,
                    TaxPayerTIN = mObjFuncResponse.AdditionalData.TIN,
                    LoggedInUserID = pObjCompany.CreatedBy,
                };

                if (!string.IsNullOrWhiteSpace(mObjFuncResponse.AdditionalData.EmailAddress1))
                {
                    BLEmailHandler.BL_TaxPayerCreated(mObjEmailDetails);
                }

                //if (!string.IsNullOrWhiteSpace(mObjFuncResponse.AdditionalData.MobileNumber1) && !pblnSkipNoValidation)
                //{
                //    _utility.BL_TaxPayerCreated(mObjEmailDetails);
                //}
            }

            return mObjFuncResponse;
        }

        public usp_GetCompanyList_Result BL_GetCompanyDetails(Company pObjCompany)
        {
            return _CompanyRepository.REP_GetCompanyDetails(pObjCompany);
        }

        public FuncResponse BL_UpdateStatus(Company pObjCompany)
        {
            return _CompanyRepository.REP_UpdateStatus(pObjCompany);
        }

        public IList<usp_GetCompanyAddressInformation_Result> BL_GetAddressInformation(Company pObjCompany)
        {
            return _CompanyRepository.REP_GetAddressInformation(pObjCompany);
        }

        public FuncResponse BL_InsertAddressInformation(MAP_Company_AddressInformation pObjAddressInformation)
        {
            return _CompanyRepository.REP_InsertAddressInformation(pObjAddressInformation);
        }

        public FuncResponse<IList<usp_GetCompanyAddressInformation_Result>> BL_RemoveAddressInformation(MAP_Company_AddressInformation pObjAddressInformation)
        {
            return _CompanyRepository.REP_RemoveAddressInformation(pObjAddressInformation);
        }

        public FuncResponse<Company> BL_CheckCompanyLoginDetails(Company pObjCompany)
        {
            return _CompanyRepository.REP_CheckIndividualLoginDetails(pObjCompany);
        }

        public FuncResponse BL_ChangePassword(Company pObjCompany)
        {
            return _CompanyRepository.REP_ChangePassword(pObjCompany);
        }

        public FuncResponse BL_UpdatePassword(Company pObjCompany)
        {
            return _CompanyRepository.REP_UpdatePassword(pObjCompany);
        }

        public void BL_UpdateRegisterationStatus(Company pObjCompany)
        {
            _CompanyRepository.REP_UpdateRegisterationStatus(pObjCompany);
        }

        public void REP_UpdateOTPCode(Company pObjCompany)
        {
            _CompanyRepository.REP_UpdateOTPCode(pObjCompany);
        }

        public FuncResponse REP_CheckOTPCode(Company pObjCompany)
        {
            return _CompanyRepository.REP_CheckOTPCode(pObjCompany);
        }

        //public IList<usp_GetCompanyDetails_Result> BL_SearchCompanyDetails(Company pObjCompany)
        //{
        //    return _CompanyRepository.REP_SearchCompanyDetails(pObjCompany);
        //}

        public IList<usp_SearchCompanyForRDMLoad_Result> BL_SearchCompanyDetails(Company pObjCompany)
        {
            return _CompanyRepository.REP_SearchCompanyDetails(pObjCompany);
        }

        public IDictionary<string, object> BL_SearchCompany(Company pObjCompany)
        {
            return _CompanyRepository.REP_SearchCompany(pObjCompany);
        }

        public FuncResponse BL_UpdateTaxOfficer(Company pObjCompany)
        {
            return _CompanyRepository.REP_UpdateTaxOfficer(pObjCompany);
        }

        public IDictionary<string, object> BL_SearchCompanyForSideMenu(Company pObjCompany)
        {
            return _CompanyRepository.REP_SearchCompanyForSideMenu(pObjCompany);
        }
    }
}
