using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IBuildingTypeRepository
    {
        usp_GetBuildingTypeList_Result REP_GetBuildingTypeDetails(Building_Types pObjBuildingType);
        IList<DropDownListResult> REP_GetBuildingTypeDropDownList(Building_Types pObjBuildingType);
        IList<usp_GetBuildingTypeList_Result> REP_GetBuildingTypeList(Building_Types pObjBuildingType);
        FuncResponse REP_InsertUpdateBuildingType(Building_Types pObjBuildingType);
        FuncResponse REP_UpdateStatus(Building_Types pObjBuildingType);
    }
}