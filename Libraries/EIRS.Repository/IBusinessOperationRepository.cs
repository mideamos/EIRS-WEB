using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IBusinessOperationRepository
    {
        usp_GetBusinessOperationList_Result REP_GetBusinessOperationDetails(Business_Operation pObjBusinessOperation);
        IList<DropDownListResult> REP_GetBusinessOperationDropDownList(Business_Operation pObjBusinessOperation);
        IList<usp_GetBusinessOperationList_Result> REP_GetBusinessOperationList(Business_Operation pObjBusinessOperation);
        FuncResponse REP_InsertUpdateBusinessOperation(Business_Operation pObjBusinessOperation);
        FuncResponse REP_UpdateStatus(Business_Operation pObjBusinessOperation);
    }
}