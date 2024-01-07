using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ISpecialRepository
    {
        IList<usp_GetSpecialAddressInformation_Result> REP_GetAddressInformation(Special pObjSpecial);
        usp_GetSpecialList_Result REP_GetSpecialDetails(Special pObjSpecial);
        IList<DropDownListResult> REP_GetSpecialDropDownList(Special pObjSpecial);
        IList<usp_GetSpecialList_Result> REP_GetSpecialList(Special pObjSpecial);
        FuncResponse REP_InsertAddressInformation(MAP_Special_AddressInformation pObjAddressInformation);
        FuncResponse<Special> REP_InsertUpdateSpecial(Special pObjSpecial);
        FuncResponse<IList<usp_GetSpecialAddressInformation_Result>> REP_RemoveAddressInformation(MAP_Special_AddressInformation pObjAddressInformation);
        FuncResponse REP_UpdateStatus(Special pObjSpecial);
        IList<vw_Special> REP_GetSpecialList();
        IList<DropDownListResult> REP_GetSpecialDropDownList(string pStrSpecialName, int pIntTaxOfficeID = 0);

        IList<usp_SearchSpecialForRDMLoad_Result> REP_SearchSpecialDetails(Special pObjSpecial);
        IDictionary<string, object> REP_SearchSpecial(Special pObjSpecial);
        FuncResponse REP_UpdateTaxOfficer(Special pObjSpecial);
    }
}