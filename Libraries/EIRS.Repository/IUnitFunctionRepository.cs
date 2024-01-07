using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IUnitFunctionRepository
    {
        usp_GetUnitFunctionList_Result REP_GetUnitFunctionDetails(Unit_Function pObjUnitFunction);
        IList<DropDownListResult> REP_GetUnitFunctionDropDownList(Unit_Function pObjUnitFunction);
        IList<usp_GetUnitFunctionList_Result> REP_GetUnitFunctionList(Unit_Function pObjUnitFunction);
        FuncResponse REP_InsertUpdateUnitFunction(Unit_Function pObjUnitFunction);
        FuncResponse REP_UpdateStatus(Unit_Function pObjUnitFunction);
    }
}