using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IWardRepository
    {
        usp_GetWardList_Result REP_GetWardDetails(Ward pObjWard);
        IList<DropDownListResult> REP_GetWardDropDownList(Ward pObjWard);
        IList<usp_GetWardList_Result> REP_GetWardList(Ward pObjWard);
        FuncResponse REP_InsertUpdateWard(Ward pObjWard);
        FuncResponse REP_UpdateStatus(Ward pObjWard);
    }
}