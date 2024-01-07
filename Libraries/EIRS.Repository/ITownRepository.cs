using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ITownRepository
    {
        usp_GetTownList_Result REP_GetTownDetails(Town pObjTown);
        IList<DropDownListResult> REP_GetTownDropDownList(Town pObjTown);
        IList<usp_GetTownList_Result> REP_GetTownList(Town pObjTown);
        FuncResponse REP_InsertUpdateTown(Town pObjTown);
        FuncResponse REP_UpdateStatus(Town pObjTown);
    }
}