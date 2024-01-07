using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLTaxPayerRole
    {
        ITaxPayerRoleRepository _TaxPayerRoleRepository;

        public BLTaxPayerRole()
        {
            _TaxPayerRoleRepository = new TaxPayerRoleRepository();
        }

        public IList<usp_GetTaxPayerRoleList_Result> BL_GetTaxPayerRoleList(TaxPayer_Roles pObjTaxPayerRole)
        {
            return _TaxPayerRoleRepository.REP_GetTaxPayerRoleList(pObjTaxPayerRole);
        }

        public FuncResponse BL_InsertUpdateTaxPayerRole(TaxPayer_Roles pObjTaxPayerRole)
        {
            return _TaxPayerRoleRepository.REP_InsertUpdateTaxPayerRole(pObjTaxPayerRole);
        }

        public usp_GetTaxPayerRoleList_Result BL_GetTaxPayerRoleDetails(TaxPayer_Roles pObjTaxPayerRole)
        {
            return _TaxPayerRoleRepository.REP_GetTaxPayerRoleDetails(pObjTaxPayerRole);
        }

        public IList<DropDownListResult> BL_GetTaxPayerRoleDropDownList(TaxPayer_Roles pObjTaxPayerRole)
        {
            return _TaxPayerRoleRepository.REP_GetTaxPayerRoleDropDownList(pObjTaxPayerRole);
        }

        public FuncResponse BL_UpdateStatus(TaxPayer_Roles pObjTaxPayerRole)
        {
            return _TaxPayerRoleRepository.REP_UpdateStatus(pObjTaxPayerRole);
        }

        public IList<DropDownListResult> BL_GetAssetTypeDropDownList(TaxPayer_Roles pObjTaxPayerRole)
        {
            return _TaxPayerRoleRepository.REP_GetAssetTypeDropDownList(pObjTaxPayerRole);
        }
    }
}
