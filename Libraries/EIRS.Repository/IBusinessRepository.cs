using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IBusinessRepository
    {
        usp_GetBusinessListNewTy_Result REP_GetBusinessDetails(Business pObjBusiness);
        IList<usp_GetBusinessListNewTy_Result> REP_GetBusinessList(Business pObjBusiness);
        FuncResponse<Business> REP_InsertUpdateBusiness(Business pObjBusiness);
        FuncResponse REP_UpdateStatus(Business pObjBusiness);

        FuncResponse REP_InsertBusinessBuilding(MAP_Business_Building pObjBusinessBuilding);

        IList<vw_Business> REP_GetBusinessList();

        IList<usp_GetBusinessBuildingList_Result> REP_GetBusinessBuildingList(MAP_Business_Building pObjBusinessBuilding);

        MAP_Business_Building REP_GetBusinessBuildingDetails(int pIntBBID);

        FuncResponse<IList<usp_GetBusinessBuildingList_Result>> REP_RemoveBusinessBuilding(MAP_Business_Building pObjBusinessBuilding);

        //IList<usp_GetBusinessChart_Result> REP_GetBusinessChart(int pIntChartType);

        IList<usp_SearchBusinessForRDMLoad_Result> REP_SearchBusinessDetails(Business pObjBusiness);
        IDictionary<string, object> REP_SearchBusiness(Business pObjBusiness);

        IDictionary<string, object> REP_SearchBusinessForSideMenu(Business pObjBusiness);
    }
}