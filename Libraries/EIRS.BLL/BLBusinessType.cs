using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLBusinessType
    {
        IBusinessTypeRepository _BusinessTypeRepository;

        public BLBusinessType()
        {
            _BusinessTypeRepository = new BusinessTypeRepository();
        }

        public IList<usp_GetBusinessTypeList_Result> BL_GetBusinessTypeList(Business_Types pObjBusinessType)
        {
            return _BusinessTypeRepository.REP_GetBusinessTypeList(pObjBusinessType);
        }

        public FuncResponse BL_InsertUpdateBusinessType(Business_Types pObjBusinessType)
        {
            return _BusinessTypeRepository.REP_InsertUpdateBusinessType(pObjBusinessType);
        }

        public usp_GetBusinessTypeList_Result BL_GetBusinessTypeDetails(Business_Types pObjBusinessType)
        {
            return _BusinessTypeRepository.REP_GetBusinessTypeDetails(pObjBusinessType);
        }

        public IList<DropDownListResult> BL_GetBusinessTypeDropDownList(Business_Types pObjBusinessType)
        {
            return _BusinessTypeRepository.REP_GetBusinessTypeDropDownList(pObjBusinessType);
        }

        public FuncResponse BL_UpdateStatus(Business_Types pObjBusinessType)
        {
            return _BusinessTypeRepository.REP_UpdateStatus(pObjBusinessType);
        }
    }
}
