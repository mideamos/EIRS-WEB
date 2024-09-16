using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLUser
    {
        IUserRepository _UserRepository;

        public BLUser()
        {
            _UserRepository = new UserRepository();
        }

        public IList<usp_GetUserList_Result> BL_GetUserList(MST_Users pObjUser)
        {
            return _UserRepository.REP_GetUserList(pObjUser);
        }

        public FuncResponse BL_InsertUpdateUser(MST_Users pObjUser)
        {
            return _UserRepository.REP_InsertUpdateUser(pObjUser);
        }

        public usp_GetUserList_Result BL_GetUserDetails(MST_Users pObjUser)
        {
            return _UserRepository.REP_GetUserDetails(pObjUser);
        }

        public FuncResponse BL_UpdateStatus(MST_Users pObjUser)
        {
            return _UserRepository.REP_UpdateStatus(pObjUser);
        }

        public FuncResponse<MST_Users> BL_CheckUserLoginDetails(MST_Users pObjUser)
        {
            return _UserRepository.REP_CheckUserLoginDetails(pObjUser);
        }

        public FuncResponse BL_ChangePassword(MST_Users pObjUser)
        {
            return _UserRepository.REP_ChangePassword(pObjUser);
        }

        public bool BL_UpdateLastUserDetails(MST_Users pObjUser)
        {
            return _UserRepository.REP_UpdateLastLoginDetails(pObjUser);
        }

        public void BL_InsertLoginToken(MST_UserToken pObjUserToken)
        {
            _UserRepository.REP_InsertLoginToken(pObjUserToken);
        }

        public bool BL_ValidateLoginSession(MST_UserToken pObjUserToken)
        {
            return _UserRepository.REP_ValidateLoginSession(pObjUserToken);
        }

        public void BL_DestroySession(MST_UserToken pObjUserToken)
        {
            _UserRepository.REP_DestroySession(pObjUserToken);
        }

        public FuncResponse BL_InsertUpdateTaxOfficerTarget(IList<MAP_TaxOfficer_Target> plstTaxOfficerTarget)
        {
            return _UserRepository.REP_InsertUpdateTaxOfficerTarget(plstTaxOfficerTarget);
        }

        public IList<usp_GetTaxOfficerTargetList_Result> BL_GetTaxOfficerTarget(MAP_TaxOfficer_Target pObjTarget)
        {
            return _UserRepository.REP_GetTaxOfficerTarget(pObjTarget);
        }

        public IList<DropDownListResult> BL_GetUserDropDownList(MST_Users pObjUser)
        {
            return _UserRepository.REP_GetUserDropDownList(pObjUser);
        }

        public IList<DropDownListResult> BL_GetApproverList(MST_Users pObjUser)
        {
            return _UserRepository.REP_GetApproverList(pObjUser);
        }
        public IList<DropDownListResult> REP_GetApproverDetList(MST_Users pObjUser, string det)
        {
            return _UserRepository.REP_GetApproverDetList(pObjUser, det);
        }


        public FuncResponse BL_ReplaceTaxOfficeManager(MST_Users pObjUser)
        {
            return _UserRepository.REP_ReplaceTaxOfficeManager(pObjUser);
        }

        public FuncResponse BL_ReplaceTaxOfficer(MST_Users pObjUser)
        {
            return _UserRepository.REP_ReplaceTaxOfficer(pObjUser);
        }

    }
}
