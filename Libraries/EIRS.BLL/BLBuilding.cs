using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLBuilding
    {
        IBuildingRepository _BuildingRepository;

        public BLBuilding()
        {
            _BuildingRepository = new BuildingRepository();
        }

        public IList<usp_GetBuildingList_Result> BL_GetBuildingList(Building pObjBuilding)
        {
            return _BuildingRepository.REP_GetBuildingList(pObjBuilding);
        }

        public IList<vw_Building> BL_GetBuildingList()
        {
            return _BuildingRepository.REP_GetBuildingList();
        }

        public FuncResponse<Building> BL_InsertUpdateBuilding(Building pObjBuilding)
        {
            return _BuildingRepository.REP_InsertUpdateBuilding(pObjBuilding);
        }

        public usp_GetBuildingList_Result BL_GetBuildingDetails(Building pObjBuilding)
        {
            return _BuildingRepository.REP_GetBuildingDetails(pObjBuilding);
        }

        public FuncResponse BL_UpdateStatus(Building pObjBuilding)
        {
            return _BuildingRepository.REP_UpdateStatus(pObjBuilding);
        }

        public FuncResponse BL_InsertBuildingLand(MAP_Building_Land pObjBuildingLand)
        {
            return _BuildingRepository.REP_InsertBuildingLand(pObjBuildingLand);
        }

        public IList<usp_GetBuildingLandList_Result> BL_GetBuildingLandList(MAP_Building_Land pObjBuildingLand)
        {
            return _BuildingRepository.REP_GetBuildingLandList(pObjBuildingLand);
        }

        public MAP_Building_Land BL_GetBuildingLandDetails(int pIntBBID)
        {
            return _BuildingRepository.REP_GetBuildingLandDetails(pIntBBID);
        }

        public FuncResponse<IList<usp_GetBuildingLandList_Result>> BL_RemoveBuildingLand(MAP_Building_Land pObjBuildingLand)
        {
            return _BuildingRepository.REP_RemoveBuildingLand(pObjBuildingLand);
        }

        public FuncResponse BL_InsertBuildingUnitNumber(MAP_Building_BuildingUnit pObjBuildingUnitNumber)
        {
            return _BuildingRepository.REP_InsertBuildingUnitNumber(pObjBuildingUnitNumber);
        }

        public IList<usp_GetBuildingUnitNumberList_Result> BL_GetBuildingUnitNumberList(MAP_Building_BuildingUnit pObjBuildingUnitNumber)
        {
            return _BuildingRepository.REP_GetBuildingUnitNumberList(pObjBuildingUnitNumber);
        }

        public MAP_Building_BuildingUnit BL_GetBuildingUnitNumberDetails(int pIntBBID)
        {
            return _BuildingRepository.REP_GetBuildingUnitNumberDetails(pIntBBID);
        }

        public FuncResponse<IList<usp_GetBuildingUnitNumberList_Result>> BL_RemoveBuildingUnitNumber(MAP_Building_BuildingUnit pObjBuildingUnitNumber)
        {
            return _BuildingRepository.REP_RemoveBuildingUnitNumber(pObjBuildingUnitNumber);
        }

        //public IList<usp_GetBuildingChart_Result> BL_GetBuildingChart(int pIntChartType)
        //{
        //    return _BuildingRepository.REP_GetBuildingChart(pIntChartType);
        //}

        public IList<usp_SearchBuildingForRDMLoad_Result> BL_SearchBuildingDetails(Building pObjBuilding)
        {
            return _BuildingRepository.REP_SearchBuildingDetails(pObjBuilding);
        }

        public IDictionary<string, object> BL_SearchBuilding(Building pObjBuilding)
        {
            return _BuildingRepository.REP_SearchBuilding(pObjBuilding);
        }

        public IDictionary<string, object> BL_SearchBuildingForSideMenu(Building pObjBuilding)
        {
            return _BuildingRepository.REP_SearchBuildingForSideMenu(pObjBuilding);
        }
    }
}
