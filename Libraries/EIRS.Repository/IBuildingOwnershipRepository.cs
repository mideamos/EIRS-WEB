using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IBuildingOwnershipRepository
    {
        usp_GetBuildingOwnershipList_Result REP_GetBuildingOwnershipDetails(Building_Ownership pObjBuildingOwnership);
        IList<DropDownListResult> REP_GetBuildingOwnershipDropDownList(Building_Ownership pObjBuildingOwnership);
        IList<usp_GetBuildingOwnershipList_Result> REP_GetBuildingOwnershipList(Building_Ownership pObjBuildingOwnership);
        FuncResponse REP_InsertUpdateBuildingOwnership(Building_Ownership pObjBuildingOwnership);
        FuncResponse REP_UpdateStatus(Building_Ownership pObjBuildingOwnership);
    }
}