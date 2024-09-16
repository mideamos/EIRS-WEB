using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ITaxPayerRoleRepository
    {
		FuncResponse REP_InsertUpdateTaxPayerRole(TaxPayer_Roles pObjTaxPayerRole);
		IList<usp_GetTaxPayerRoleList_Result> REP_GetTaxPayerRoleList(TaxPayer_Roles pObjTaxPayerRole);
        usp_GetTaxPayerRoleList_Result REP_GetTaxPayerRoleDetails(TaxPayer_Roles pObjTaxPayerRole);
        IList<DropDownListResult> REP_GetTaxPayerRoleDropDownList(TaxPayer_Roles pObjTaxPayerRole);
        FuncResponse REP_UpdateStatus(TaxPayer_Roles pObjTaxPayerRole);

        IList<DropDownListResult> REP_GetAssetTypeDropDownList(TaxPayer_Roles pObjTaxPayerRole);
    }
}