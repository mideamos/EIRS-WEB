using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLUnitPurpose
    {
        IUnitPurposeRepository _UnitPurposeRepository;

        public BLUnitPurpose()
        {
            _UnitPurposeRepository = new UnitPurposeRepository();
        }

        public IList<usp_GetUnitPurposeList_Result> BL_GetUnitPurposeList(Unit_Purpose pObjUnitPurpose)
        {
            return _UnitPurposeRepository.REP_GetUnitPurposeList(pObjUnitPurpose);
        }

        public FuncResponse BL_InsertUpdateUnitPurpose(Unit_Purpose pObjUnitPurpose)
        {
            return _UnitPurposeRepository.REP_InsertUpdateUnitPurpose(pObjUnitPurpose);
        }

        public usp_GetUnitPurposeList_Result BL_GetUnitPurposeDetails(Unit_Purpose pObjUnitPurpose)
        {
            return _UnitPurposeRepository.REP_GetUnitPurposeDetails(pObjUnitPurpose);
        }

        public IList<DropDownListResult> BL_GetUnitPurposeDropDownList(Unit_Purpose pObjUnitPurpose)
        {
            return _UnitPurposeRepository.REP_GetUnitPurposeDropDownList(pObjUnitPurpose);
        }

        public FuncResponse BL_UpdateStatus(Unit_Purpose pObjUnitPurpose)
        {
            return _UnitPurposeRepository.REP_UpdateStatus(pObjUnitPurpose);
        }
    }
}
