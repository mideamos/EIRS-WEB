using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IBuildingRepository
    {
        usp_GetBuildingList_Result REP_GetBuildingDetails(Building pObjBuilding);
        IList<usp_GetBuildingList_Result> REP_GetBuildingList(Building pObjBuilding);
        FuncResponse<Building> REP_InsertUpdateBuilding(Building pObjBuilding);
        FuncResponse REP_UpdateStatus(Building pObjBuilding);

        FuncResponse REP_InsertBuildingLand(MAP_Building_Land pObjBuildingLand);

        IList<vw_Building> REP_GetBuildingList();

        IList<usp_GetBuildingLandList_Result> REP_GetBuildingLandList(MAP_Building_Land pObjBuildingLand);

        MAP_Building_Land REP_GetBuildingLandDetails(int pIntBBID);

        FuncResponse<IList<usp_GetBuildingLandList_Result>> REP_RemoveBuildingLand(MAP_Building_Land pObjBuildingLand);

        FuncResponse REP_InsertBuildingUnitNumber(MAP_Building_BuildingUnit pObjBuildingUnitNumber);

        IList<usp_GetBuildingUnitNumberList_Result> REP_GetBuildingUnitNumberList(MAP_Building_BuildingUnit pObjBuildingUnitNumber);

        MAP_Building_BuildingUnit REP_GetBuildingUnitNumberDetails(int pIntBBID);

        FuncResponse<IList<usp_GetBuildingUnitNumberList_Result>> REP_RemoveBuildingUnitNumber(MAP_Building_BuildingUnit pObjBuildingUnitNumber);

        //IList<usp_GetBuildingChart_Result> REP_GetBuildingChart(int pIntChartType);

        IList<usp_SearchBuildingForRDMLoad_Result> REP_SearchBuildingDetails(Building pObjBuilding);
        IDictionary<string, object> REP_SearchBuilding(Building pObjBuilding);

        IDictionary<string, object> REP_SearchBuildingForSideMenu(Building pObjBuilding);
    }
}