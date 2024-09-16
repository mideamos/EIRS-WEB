using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ICommonRepository
    {
        IList<DropDownListResult> REP_GetDealerTypeDropDownList();
        IList<DropDownListResult> REP_GetGenderDropDownList();

        IList<DropDownListResult> REP_GetNationalityDropDownList();

        IList<DropDownListResult> REP_GetMaritalStatusDropDownList();

        IList<DropDownListResult> REP_GetComputationDropDownList();

        IList<DropDownListResult> REP_GetRuleRunDropDownList();

        IList<DropDownListResult> REP_GetUserTypeDropDownList();
        IList<DropDownListResult> REP_GetReviewStatusDropDownList();

        IList<DropDownListResult> REP_GetFieldTypeDropDownList();


        IList<DropDownListResult> REP_GetProfileTypeDropDownList();

        IList<DropDownListResult> REP_GetSystemRoleDropDownList();

        IList<DropDownListResult> REP_GetALScreenDropDownList();

        IList<DropDownListResult> REP_GetTCCStatusDropDownList();

        FuncResponse REP_InsertPayDirectNotification(PayDirect_Notifications pObjPayDirectNotification);
        IList<DropDownListResult> REP_GetTaxOfficeManagerList(int pIntTaxOfficeID = 0, bool pblnIsManager = false);
        IList<DropDownListResult> REP_GetTaxOfficerList(int pIntTaxOfficeID = 0);
    }
}