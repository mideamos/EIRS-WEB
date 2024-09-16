using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ICompanyRepository
    {
        usp_GetCompanyList_Result REP_GetCompanyDetails(Company pObjCompany);
        IList<usp_GetCompanyList_Result> REP_GetCompanyList(Company pObjCompany);

        IList<DropDownListResult> REP_GetCompanyDropDownList(Company pObjCompany);
        FuncResponse<Company> REP_InsertUpdateCompany(Company pObjCompany, bool pblnSkipNoValidation = false);
        FuncResponse REP_UpdateStatus(Company pObjCompany);
        IList<usp_GetCompanyAddressInformation_Result> REP_GetAddressInformation(Company pObjCompany);
        FuncResponse REP_InsertAddressInformation(MAP_Company_AddressInformation pObjAddressInformation);
        FuncResponse<IList<usp_GetCompanyAddressInformation_Result>> REP_RemoveAddressInformation(MAP_Company_AddressInformation pObjAddressInformation);
        IList<vw_Company> REP_GetCompanyList();
        FuncResponse<Company> REP_CheckIndividualLoginDetails(Company pObjCompany);
        FuncResponse REP_ChangePassword(Company pObjCompany);
        FuncResponse REP_UpdatePassword(Company pObjCompany);
        void REP_UpdateRegisterationStatus(Company pObjCompany);
        void REP_UpdateOTPCode(Company pObjCompany);
        FuncResponse REP_CheckOTPCode(Company pObjCompany);
        IList<DropDownListResult> REP_GetCompanyDropDownList(string pStrCompanyName, int pIntTaxOfficeID = 0);

        //IList<usp_GetCompanyDetails_Result> REP_SearchCompanyDetails(Company pObjCompany);
        IList<usp_SearchCompanyForRDMLoad_Result> REP_SearchCompanyDetails(Company pObjCompany);
        IDictionary<string, object> REP_SearchCompany(Company pObjCompany);
        FuncResponse REP_UpdateTaxOfficer(Company pObjCompany);

        IDictionary<string, object> REP_SearchCompanyForSideMenu(Company pObjCompany);
    }
}