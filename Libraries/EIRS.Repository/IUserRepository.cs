using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IUserRepository
    {
        FuncResponse REP_ChangePassword(MST_Users pObjUser);
        FuncResponse<MST_Users> REP_CheckUserLoginDetails(MST_Users pObjUser);
        FuncResponse REP_ForgotPassword(MST_Users pObjUser);
        usp_GetUserList_Result REP_GetUserDetails(MST_Users pObjUser);
        IList<usp_GetUserList_Result> REP_GetUserList(MST_Users pObjUser);
        FuncResponse REP_InsertUpdateUser(MST_Users pObjUser);
        FuncResponse REP_UpdateStatus(MST_Users pObjUser);

        bool REP_UpdateLastLoginDetails(MST_Users pObjUser);

        void REP_InsertLoginToken(MST_UserToken pObjUserToken);

        bool REP_ValidateLoginSession(MST_UserToken pObjUserToken);

        void REP_DestroySession(MST_UserToken pObjUserToken);

        FuncResponse REP_InsertUpdateTaxOfficerTarget(IList<MAP_TaxOfficer_Target> plstTaxOfficerTarget);
        IList<usp_GetTaxOfficerTargetList_Result> REP_GetTaxOfficerTarget(MAP_TaxOfficer_Target pObjTarget);
        IList<DropDownListResult> REP_GetUserDropDownList(MST_Users pObjUser);
        IList<DropDownListResult> REP_GetApproverList(MST_Users pObjUser);
        IList<DropDownListResult> REP_GetApproverDetList(MST_Users pObjUser, string det);
        FuncResponse REP_ReplaceTaxOfficeManager(MST_Users pObjUser);

        FuncResponse REP_ReplaceTaxOfficer(MST_Users pObjUser);
    }
}