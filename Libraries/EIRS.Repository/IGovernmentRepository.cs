using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IGovernmentRepository
    {
        IList<usp_GetGovernmentAddressInformation_Result> REP_GetAddressInformation(Government pObjGovernment);
        usp_GetGovernmentList_Result REP_GetGovernmentDetails(Government pObjGovernment);
        IList<DropDownListResult> REP_GetGovernmentDropDownList(Government pObjGovernment);
        IList<usp_GetGovernmentList_Result> REP_GetGovernmentList(Government pObjGovernment);
        FuncResponse REP_InsertAddressInformation(MAP_Government_AddressInformation pObjAddressInformation);
        FuncResponse<Government> REP_InsertUpdateGovernment(Government pObjGovernment);
        FuncResponse<IList<usp_GetGovernmentAddressInformation_Result>> REP_RemoveAddressInformation(MAP_Government_AddressInformation pObjAddressInformation);
        FuncResponse REP_UpdateStatus(Government pObjGovernment);
        IList<vw_Government> REP_GetGovernmentList();

        FuncResponse<Government> REP_CheckGovermentLoginDetails(Government pObjGovernment);
        FuncResponse REP_ChangePassword(Government pObjGovernment);
        FuncResponse REP_UpdatePassword(Government pObjGovernment);
        void REP_UpdateRegisterationStatus(Government pObjGovernment);
        void REP_UpdateOTPCode(Government pObjGovernment);
        FuncResponse REP_CheckOTPCode(Government pObjGovernment);
        IList<DropDownListResult> REP_GetGovernmentDropDownList(string pStrGovernmentName, int pIntTaxOfficeID = 0);

        IList<usp_SearchGovernmentForRDMLoad_Result> REP_SearchGovernmentDetails(Government pObjGovernment);
        IDictionary<string, object> REP_SearchGovernment(Government pObjGovernment);
        FuncResponse REP_UpdateTaxOfficer(Government pObjGovernment);

        IDictionary<string, object> REP_SearchGovernmentForSideMenu(Government pObjGovernment);
    }
}