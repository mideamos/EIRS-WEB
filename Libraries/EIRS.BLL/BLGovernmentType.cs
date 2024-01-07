using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLGovernmentType
    {
        IGovernmentTypeRepository _GovernmentTypeRepository;

        public BLGovernmentType()
        {
            _GovernmentTypeRepository = new GovernmentTypeRepository();
        }

        public IList<usp_GetGovernmentTypeList_Result> BL_GetGovernmentTypeList(Government_Types pObjGovernmentType)
        {
            return _GovernmentTypeRepository.REP_GetGovernmentTypeList(pObjGovernmentType);
        }

        public FuncResponse BL_InsertUpdateGovernmentType(Government_Types pObjGovernmentType)
        {
            return _GovernmentTypeRepository.REP_InsertUpdateGovernmentType(pObjGovernmentType);
        }

        public usp_GetGovernmentTypeList_Result BL_GetGovernmentTypeDetails(Government_Types pObjGovernmentType)
        {
            return _GovernmentTypeRepository.REP_GetGovernmentTypeDetails(pObjGovernmentType);
        }

        public IList<DropDownListResult> BL_GetGovernmentTypeDropDownList(Government_Types pObjGovernmentType)
        {
            return _GovernmentTypeRepository.REP_GetGovernmentTypeDropDownList(pObjGovernmentType);
        }

        public FuncResponse BL_UpdateStatus(Government_Types pObjGovernmentType)
        {
            return _GovernmentTypeRepository.REP_UpdateStatus(pObjGovernmentType);
        }
    }
}
