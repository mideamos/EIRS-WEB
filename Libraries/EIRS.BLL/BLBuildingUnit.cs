using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLBuildingUnit
    {
        IBuildingUnitRepository _BuildingUnitRepository;

        public BLBuildingUnit()
        {
            _BuildingUnitRepository = new BuildingUnitRepository();
        }

        public IList<usp_GetBuildingUnitList_Result> BL_GetBuildingUnitList(Building_Unit pObjBuildingUnit)
        {
            return _BuildingUnitRepository.REP_GetBuildingUnitList(pObjBuildingUnit);
        }

        public FuncResponse<Building_Unit> BL_InsertUpdateBuildingUnit(Building_Unit pObjBuildingUnit)
        {
            return _BuildingUnitRepository.REP_InsertUpdateBuildingUnit(pObjBuildingUnit);
        }

        public usp_GetBuildingUnitList_Result BL_GetBuildingUnitDetails(Building_Unit pObjBuildingUnit)
        {
            return _BuildingUnitRepository.REP_GetBuildingUnitDetails(pObjBuildingUnit);
        }

        public IList<DropDownListResult> BL_GetBuildingUnitDropDownList(Building_Unit pObjBuildingUnit)
        {
            return _BuildingUnitRepository.REP_GetBuildingUnitDropDownList(pObjBuildingUnit);
        }

        public FuncResponse BL_UpdateStatus(Building_Unit pObjBuildingUnit)
        {
            return _BuildingUnitRepository.REP_UpdateStatus(pObjBuildingUnit);
        }

        public IList<usp_SearchBuildingUnitForRDMLoad_Result> BL_SearchBuildingUnitDetails(Building_Unit pObjBuildingUnit)
        {
            return _BuildingUnitRepository.REP_SearchBuildingUnitDetails(pObjBuildingUnit);
        }

        public IDictionary<string, object> BL_SearchBuildingUnit(Building_Unit pObjBuildingUnit)
        {
            return _BuildingUnitRepository.REP_SearchBuildingUnit(pObjBuildingUnit);
        }
    }
}
