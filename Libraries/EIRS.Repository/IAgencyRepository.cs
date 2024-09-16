using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IAgencyRepository
    {
		FuncResponse REP_InsertUpdateAgency(Agency pObjAgency);
		IList<usp_GetAgencyList_Result> REP_GetAgencyList(Agency pObjAgency);
        usp_GetAgencyList_Result REP_GetAgencyDetails(Agency pObjAgency);
        IList<DropDownListResult> REP_GetAgencyDropDownList(Agency pObjAgency);
        FuncResponse REP_UpdateStatus(Agency pObjAgency);
    }
}