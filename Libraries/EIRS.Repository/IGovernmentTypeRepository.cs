using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IGovernmentTypeRepository
    {
        usp_GetGovernmentTypeList_Result REP_GetGovernmentTypeDetails(Government_Types pObjGovernmentType);
        IList<DropDownListResult> REP_GetGovernmentTypeDropDownList(Government_Types pObjGovernmentType);
        IList<usp_GetGovernmentTypeList_Result> REP_GetGovernmentTypeList(Government_Types pObjGovernmentType);
        FuncResponse REP_InsertUpdateGovernmentType(Government_Types pObjGovernmentType);
        FuncResponse REP_UpdateStatus(Government_Types pObjGovernmentType);
    }
}