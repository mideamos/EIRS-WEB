using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLCommon
    {
        ICommonRepository _CommonRepository;

        
        public BLCommon()
        {
            _CommonRepository = new CommonRepository();
        }

        public IList<DropDownListResult> BL_GetDealerTypeDropDownList()
        {
            return _CommonRepository.REP_GetDealerTypeDropDownList();
        }
        public IList<DropDownListResult> BL_GetGenderDropDownList()
        {
            return _CommonRepository.REP_GetGenderDropDownList();
        }

        public IList<DropDownListResult> BL_GetMaritalStatusDropDownList()
        {
            return _CommonRepository.REP_GetMaritalStatusDropDownList();
        }

        public IList<DropDownListResult> BL_GetNationalityDropDownList()
        {
            return _CommonRepository.REP_GetNationalityDropDownList();
        }

        public IList<DropDownListResult> BL_GetComputationDropDownList()
        {
            return _CommonRepository.REP_GetComputationDropDownList();
        }

        public IList<DropDownListResult> BL_GetRuleRunDropDownList()
        {
            return _CommonRepository.REP_GetRuleRunDropDownList();
        }

        public IList<DropDownListResult> BL_GetUserTypeDropDownList()
        {
            return _CommonRepository.REP_GetUserTypeDropDownList();
        }

        public FuncResponse BL_InsertPayDirectNotification(PayDirect_Notifications pObjPayDirectNotification)
        {
            return _CommonRepository.REP_InsertPayDirectNotification(pObjPayDirectNotification);
        }

        public IList<DropDownListResult> BL_GetTaxOfficeManagerList(int pIntTaxOfficeID = 0, bool pblnIsManager = false)
        {
            return _CommonRepository.REP_GetTaxOfficeManagerList(pIntTaxOfficeID, pblnIsManager);
        }

        public IList<DropDownListResult> BL_GetTaxOfficerList(int pIntTaxOfficeID = 0)
        {
            return _CommonRepository.REP_GetTaxOfficerList(pIntTaxOfficeID);
        }

        public IList<DropDownListResult> BL_GetReviewStatusDropDownList()
        {
            return _CommonRepository.REP_GetReviewStatusDropDownList();
        }

        public IList<DropDownListResult> BL_GetProfileTypeDropDownList()
        {
            return _CommonRepository.REP_GetProfileTypeDropDownList();
        }

        public IList<DropDownListResult> BL_GetSystemRoleDropDownList()
        {
            return _CommonRepository.REP_GetSystemRoleDropDownList();
        }

        public IList<DropDownListResult> BL_GetALScreenDropDownList()
        {
            return _CommonRepository.REP_GetALScreenDropDownList();
        }

        public IList<DropDownListResult> BL_GetFieldTypeDropDownList()
        {
            return _CommonRepository.REP_GetFieldTypeDropDownList();
        }

        public IList<DropDownListResult> BL_GetTCCStatusDropDownList()
        {
            return _CommonRepository.REP_GetTCCStatusDropDownList();
        }

    }
}
