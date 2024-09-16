using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IIndividualRepository
    {
        usp_GetIndividualList_Result REP_GetIndividualDetails(Individual pObjIndividual);
        IList<usp_GetIndividualList_Result> REP_GetIndividualList(Individual pObjIndividual);
        FuncResponse<Individual> REP_InsertUpdateIndividual(Individual pObjIndividual, bool pblnSkipNoValidation = false);
        FuncResponse REP_UpdateStatus(Individual pObjIndividual);

        IList<usp_GetIndividualAddressInformation_Result> REP_GetAddressInformation(Individual pObjIndividual);

        FuncResponse REP_InsertAddressInformation(MAP_Individual_AddressInformation pObjAddressInformation);

        FuncResponse<IList<usp_GetIndividualAddressInformation_Result>> REP_RemoveAddressInformation(MAP_Individual_AddressInformation pObjAddressInformation);

        IList<DropDownListResult> REP_GetIndividualDropDownList(Individual pObjIndividual);

        IList<vw_Individual> REP_GetIndividualList();
        FuncResponse<Individual> REP_CheckIndividualLoginDetails(Individual pObjIndividual);
        FuncResponse REP_ChangePassword(Individual pObjIndividual);
        FuncResponse REP_UpdatePassword(Individual pObjIndividual);
        void REP_UpdateRegisterationStatus(Individual pObjIndividual);
        void REP_UpdateOTPCode(Individual pObjIndividual);
        FuncResponse REP_CheckOTPCode(Individual pObjIndividual);
        IList<DropDownListResult> REP_GetIndividualDropDownList(string pStrIndividualName, int pIntTaxOfficeID = 0);

        IList<usp_SearchIndividualForRDMLoad_Result> REP_SearchIndividualDetails(Individual pObjIndividual);

        IDictionary<string, object> REP_SearchIndividual(Individual pObjIndividual);
        FuncResponse REP_UpdateTaxOfficer(Individual pObjIndividual);

        IDictionary<string, object> REP_SearchIndividualForSideMenu(Individual pObjIndividual);
    }
}