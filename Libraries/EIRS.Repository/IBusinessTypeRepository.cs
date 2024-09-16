using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IBusinessTypeRepository
    {
		FuncResponse REP_InsertUpdateBusinessType(Business_Types pObjBusinessType);
		IList<usp_GetBusinessTypeList_Result> REP_GetBusinessTypeList(Business_Types pObjBusinessType);
        usp_GetBusinessTypeList_Result REP_GetBusinessTypeDetails(Business_Types pObjBusinessType);
        IList<DropDownListResult> REP_GetBusinessTypeDropDownList(Business_Types pObjBusinessType);
        FuncResponse REP_UpdateStatus(Business_Types pObjBusinessType);
    }
}