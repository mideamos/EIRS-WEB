using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IDirectorateRepository
    {
        usp_GetDirectorateList_Result REP_GetDirectorateDetails(Directorate pObjDirectorate);
        IList<DropDownListResult> REP_GetDirectorateDropDownList(Directorate pObjDirectorate);
        IList<usp_GetDirectorateList_Result> REP_GetDirectorateList(Directorate pObjDirectorate);
        IList<MAP_Directorates_RevenueStream> REP_GetRevenueStream(int pIntDirectorateID);
        FuncResponse REP_InsertRevenueStream(MAP_Directorates_RevenueStream pObjRevenueStream);
        FuncResponse<Directorate> REP_InsertUpdateDirectorate(Directorate pObjDirectorate);
        FuncResponse REP_RemoveRevenueStream(MAP_Directorates_RevenueStream pObjRevenueStream);
        FuncResponse REP_UpdateStatus(Directorate pObjDirectorate);
    }
}