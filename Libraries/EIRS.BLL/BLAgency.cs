using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLAgency
    {
        IAgencyRepository _AgencyRepository;

        public BLAgency()
        {
            _AgencyRepository = new AgencyRepository();
        }

        public IList<usp_GetAgencyList_Result> BL_GetAgencyList(Agency pObjAgency)
        {
            return _AgencyRepository.REP_GetAgencyList(pObjAgency);
        }

        public FuncResponse BL_InsertUpdateAgency(Agency pObjAgency)
        {
            return _AgencyRepository.REP_InsertUpdateAgency(pObjAgency);
        }

        public usp_GetAgencyList_Result BL_GetAgencyDetails(Agency pObjAgency)
        {
            return _AgencyRepository.REP_GetAgencyDetails(pObjAgency);
        }

        public IList<DropDownListResult> BL_GetAgencyDropDownList(Agency pObjAgency)
        {
            return _AgencyRepository.REP_GetAgencyDropDownList(pObjAgency);
        }

        public FuncResponse BL_UpdateStatus(Agency pObjAgency)
        {
            return _AgencyRepository.REP_UpdateStatus(pObjAgency);
        }
    }
}
