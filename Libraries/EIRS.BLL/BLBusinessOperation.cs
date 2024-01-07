using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLBusinessOperation
    {
        IBusinessOperationRepository _BusinessOperationRepository;

        public BLBusinessOperation()
        {
            _BusinessOperationRepository = new BusinessOperationRepository();
        }

        public IList<usp_GetBusinessOperationList_Result> BL_GetBusinessOperationList(Business_Operation pObjBusinessOperation)
        {
            return _BusinessOperationRepository.REP_GetBusinessOperationList(pObjBusinessOperation);
        }

        public FuncResponse BL_InsertUpdateBusinessOperation(Business_Operation pObjBusinessOperation)
        {
            return _BusinessOperationRepository.REP_InsertUpdateBusinessOperation(pObjBusinessOperation);
        }

        public usp_GetBusinessOperationList_Result BL_GetBusinessOperationDetails(Business_Operation pObjBusinessOperation)
        {
            return _BusinessOperationRepository.REP_GetBusinessOperationDetails(pObjBusinessOperation);
        }

        public IList<DropDownListResult> BL_GetBusinessOperationDropDownList(Business_Operation pObjBusinessOperation)
        {
            return _BusinessOperationRepository.REP_GetBusinessOperationDropDownList(pObjBusinessOperation);
        }

        public FuncResponse BL_UpdateStatus(Business_Operation pObjBusinessOperation)
        {
            return _BusinessOperationRepository.REP_UpdateStatus(pObjBusinessOperation);
        }
    }
}
