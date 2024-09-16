using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ITaxPayerTypeRepository
    {
		FuncResponse REP_InsertUpdateTaxPayerType(TaxPayer_Types pObjTaxPayerType);
		IList<usp_GetTaxPayerTypeList_Result> REP_GetTaxPayerTypeList(TaxPayer_Types pObjTaxPayerType);
        usp_GetTaxPayerTypeList_Result REP_GetTaxPayerTypeDetails(TaxPayer_Types pObjTaxPayerType);
        IList<DropDownListResult> REP_GetTaxPayerTypeDropDownList(TaxPayer_Types pObjTaxPayerType);
        FuncResponse REP_UpdateStatus(TaxPayer_Types pObjTaxPayerType);
    }
}