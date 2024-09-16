using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLWard
    {
        IWardRepository _WardRepository;

        public BLWard()
        {
            _WardRepository = new WardRepository();
        }

        public IList<usp_GetWardList_Result> BL_GetWardList(Ward pObjWard)
        {
            return _WardRepository.REP_GetWardList(pObjWard);
        }

        public FuncResponse BL_InsertUpdateWard(Ward pObjWard)
        {
            return _WardRepository.REP_InsertUpdateWard(pObjWard);
        }

        public usp_GetWardList_Result BL_GetWardDetails(Ward pObjWard)
        {
            return _WardRepository.REP_GetWardDetails(pObjWard);
        }

        public IList<DropDownListResult> BL_GetWardDropDownList(Ward pObjWard)
        {
            return _WardRepository.REP_GetWardDropDownList(pObjWard);
        }

        public FuncResponse BL_UpdateStatus(Ward pObjWard)
        {
            return _WardRepository.REP_UpdateStatus(pObjWard);
        }
    }
}
