using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IBuildingUnitRepository
    {
        usp_GetBuildingUnitList_Result REP_GetBuildingUnitDetails(Building_Unit pObjBuildingUnit);
        IList<DropDownListResult> REP_GetBuildingUnitDropDownList(Building_Unit pObjBuildingUnit);
        IList<usp_GetBuildingUnitList_Result> REP_GetBuildingUnitList(Building_Unit pObjBuildingUnit);
        FuncResponse<Building_Unit> REP_InsertUpdateBuildingUnit(Building_Unit pObjBuildingUnit);
        FuncResponse REP_UpdateStatus(Building_Unit pObjBuildingUnit);

        IList<usp_SearchBuildingUnitForRDMLoad_Result> REP_SearchBuildingUnitDetails(Building_Unit pObjBuildingUnit);
        IDictionary<string, object> REP_SearchBuildingUnit(Building_Unit pObjBuildingUnit);
    }
}