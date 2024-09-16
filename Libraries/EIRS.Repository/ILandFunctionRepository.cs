using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ILandFunctionRepository
    {
        usp_GetLandFunctionList_Result REP_GetLandFunctionDetails(Land_Function pObjLandFunction);
        IList<DropDownListResult> REP_GetLandFunctionDropDownList(Land_Function pObjLandFunction);
        IList<usp_GetLandFunctionList_Result> REP_GetLandFunctionList(Land_Function pObjLandFunction);
        FuncResponse REP_InsertUpdateLandFunction(Land_Function pObjLandFunction);
        FuncResponse REP_UpdateStatus(Land_Function pObjLandFunction);
    }
}