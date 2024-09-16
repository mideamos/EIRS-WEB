using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IBuildingPurposeRepository
    {
        usp_GetBuildingPurposeList_Result REP_GetBuildingPurposeDetails(Building_Purpose pObjBuildingPurpose);
        IList<DropDownListResult> REP_GetBuildingPurposeDropDownList(Building_Purpose pObjBuildingPurpose);
        IList<usp_GetBuildingPurposeList_Result> REP_GetBuildingPurposeList(Building_Purpose pObjBuildingPurpose);
        FuncResponse REP_InsertUpdateBuildingPurpose(Building_Purpose pObjBuildingPurpose);
        FuncResponse REP_UpdateStatus(Building_Purpose pObjBuildingPurpose);
    }
}