using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLTaxPayerType
    {
        ITaxPayerTypeRepository _TaxPayerTypeRepository;

        public BLTaxPayerType()
        {
            _TaxPayerTypeRepository = new TaxPayerTypeRepository();
        }

        public IList<usp_GetTaxPayerTypeList_Result> BL_GetTaxPayerTypeList(TaxPayer_Types pObjTaxPayerType)
        {
            return _TaxPayerTypeRepository.REP_GetTaxPayerTypeList(pObjTaxPayerType);
        }

        public FuncResponse BL_InsertUpdateTaxPayerType(TaxPayer_Types pObjTaxPayerType)
        {
            return _TaxPayerTypeRepository.REP_InsertUpdateTaxPayerType(pObjTaxPayerType);
        }

        public usp_GetTaxPayerTypeList_Result BL_GetTaxPayerTypeDetails(TaxPayer_Types pObjTaxPayerType)
        {
            return _TaxPayerTypeRepository.REP_GetTaxPayerTypeDetails(pObjTaxPayerType);
        }

        public IList<DropDownListResult> BL_GetTaxPayerTypeDropDownList(TaxPayer_Types pObjTaxPayerType)
        {
            return _TaxPayerTypeRepository.REP_GetTaxPayerTypeDropDownList(pObjTaxPayerType);
        }

        public FuncResponse BL_UpdateStatus(TaxPayer_Types pObjTaxPayerType)
        {
            return _TaxPayerTypeRepository.REP_UpdateStatus(pObjTaxPayerType);
        }
    }
}
