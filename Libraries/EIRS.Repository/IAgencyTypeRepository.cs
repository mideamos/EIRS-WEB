using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IAgencyTypeRepository
    {
        usp_GetAgencyTypeList_Result REP_GetAgencyTypeDetails(Agency_Types pObjAgencyType);
        IList<DropDownListResult> REP_GetAgencyTypeDropDownList(Agency_Types pObjAgencyType);
        IList<usp_GetAgencyTypeList_Result> REP_GetAgencyTypeList(Agency_Types pObjAgencyType);
        FuncResponse REP_InsertUpdateAgencyType(Agency_Types pObjAgencyType);
        FuncResponse REP_UpdateStatus(Agency_Types pObjAgencyType);
    }
}