using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IEM_RevenueHeadRepository
    {
        usp_EM_GetRevenueHeadList_Result REP_GetRevenueHeadDetails(EM_RevenueHead pObjRevenueHead);
        IList<DropDownListResult> REP_GetRevenueHeadDropDownList(EM_RevenueHead pObjRevenueHead);
        IList<usp_EM_GetRevenueHeadList_Result> REP_GetRevenueHeadList(EM_RevenueHead pObjRevenueHead);
        FuncResponse REP_InsertUpdateRevenueHead(EM_RevenueHead pObjRevenueHead);
        FuncResponse REP_UpdateStatus(EM_RevenueHead pObjRevenueHead);
    }
}