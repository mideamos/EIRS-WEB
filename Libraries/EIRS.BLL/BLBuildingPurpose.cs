using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLBuildingPurpose
    {
        IBuildingPurposeRepository _BuildingPurposeRepository;

        public BLBuildingPurpose()
        {
            _BuildingPurposeRepository = new BuildingPurposeRepository();
        }

        public IList<usp_GetBuildingPurposeList_Result> BL_GetBuildingPurposeList(Building_Purpose pObjBuildingPurpose)
        {
            return _BuildingPurposeRepository.REP_GetBuildingPurposeList(pObjBuildingPurpose);
        }

        public FuncResponse BL_InsertUpdateBuildingPurpose(Building_Purpose pObjBuildingPurpose)
        {
            return _BuildingPurposeRepository.REP_InsertUpdateBuildingPurpose(pObjBuildingPurpose);
        }

        public usp_GetBuildingPurposeList_Result BL_GetBuildingPurposeDetails(Building_Purpose pObjBuildingPurpose)
        {
            return _BuildingPurposeRepository.REP_GetBuildingPurposeDetails(pObjBuildingPurpose);
        }

        public IList<DropDownListResult> BL_GetBuildingPurposeDropDownList(Building_Purpose pObjBuildingPurpose)
        {
            return _BuildingPurposeRepository.REP_GetBuildingPurposeDropDownList(pObjBuildingPurpose);
        }

        public FuncResponse BL_UpdateStatus(Building_Purpose pObjBuildingPurpose)
        {
            return _BuildingPurposeRepository.REP_UpdateStatus(pObjBuildingPurpose);
        }
    }
}
