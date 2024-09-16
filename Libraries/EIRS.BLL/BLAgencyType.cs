using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLAgencyType
    {
        IAgencyTypeRepository _AgencyTypeRepository;

        public BLAgencyType()
        {
            _AgencyTypeRepository = new AgencyTypeRepository();
        }

        public IList<usp_GetAgencyTypeList_Result> BL_GetAgencyTypeList(Agency_Types pObjAgencyType)
        {
            return _AgencyTypeRepository.REP_GetAgencyTypeList(pObjAgencyType);
        }

        public FuncResponse BL_InsertUpdateAgencyType(Agency_Types pObjAgencyType)
        {
            return _AgencyTypeRepository.REP_InsertUpdateAgencyType(pObjAgencyType);
        }

        public usp_GetAgencyTypeList_Result BL_GetAgencyTypeDetails(Agency_Types pObjAgencyType)
        {
            return _AgencyTypeRepository.REP_GetAgencyTypeDetails(pObjAgencyType);
        }

        public IList<DropDownListResult> BL_GetAgencyTypeDropDownList(Agency_Types pObjAgencyType)
        {
            return _AgencyTypeRepository.REP_GetAgencyTypeDropDownList(pObjAgencyType);
        }

        public FuncResponse BL_UpdateStatus(Agency_Types pObjAgencyType)
        {
            return _AgencyTypeRepository.REP_UpdateStatus(pObjAgencyType);
        }
    }
}
