using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ILandRepository
    {
        usp_GetLandList_Result REP_GetLandDetails(Land pObjLand);
        IList<usp_GetLandList_Result> REP_GetLandList(Land pObjLand);
        FuncResponse<Land> REP_InsertUpdateLand(Land pObjLand);
        FuncResponse REP_UpdateStatus(Land pObjLand);
        IList<vw_Land> REP_GetLandList();
        IList<usp_GetSearchLandForEdoGIS_Result> REP_SearchLandForEdoGIS(Land pObjLand);

        //IList<usp_GetLandChart_Result> REP_GetLandChart(int pIntChartType);

        IList<usp_SearchLandForRDMLoad_Result> REP_SearchLandDetails(Land pObjLand);
        IDictionary<string, object> REP_SearchLand(Land pObjLand);

        IDictionary<string, object> REP_SearchLandForSideMenu(Land pObjLand);
    }
}