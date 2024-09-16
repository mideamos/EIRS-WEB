using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLBuildingType
    {
        IBuildingTypeRepository _BuildingTypeRepository;

        public BLBuildingType()
        {
            _BuildingTypeRepository = new BuildingTypeRepository();
        }

        public IList<usp_GetBuildingTypeList_Result> BL_GetBuildingTypeList(Building_Types pObjBuildingType)
        {
            return _BuildingTypeRepository.REP_GetBuildingTypeList(pObjBuildingType);
        }

        public FuncResponse BL_InsertUpdateBuildingType(Building_Types pObjBuildingType)
        {
            return _BuildingTypeRepository.REP_InsertUpdateBuildingType(pObjBuildingType);
        }

        public usp_GetBuildingTypeList_Result BL_GetBuildingTypeDetails(Building_Types pObjBuildingType)
        {
            return _BuildingTypeRepository.REP_GetBuildingTypeDetails(pObjBuildingType);
        }

        public IList<DropDownListResult> BL_GetBuildingTypeDropDownList(Building_Types pObjBuildingType)
        {
            return _BuildingTypeRepository.REP_GetBuildingTypeDropDownList(pObjBuildingType);
        }

        public FuncResponse BL_UpdateStatus(Building_Types pObjBuildingType)
        {
            return _BuildingTypeRepository.REP_UpdateStatus(pObjBuildingType);
        }
    }
}
