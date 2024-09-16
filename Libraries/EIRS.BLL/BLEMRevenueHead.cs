using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLEMRevenueHead
    {
        IEM_RevenueHeadRepository _RevenueHeadRepository;

        public BLEMRevenueHead()
        {
            _RevenueHeadRepository = new EM_RevenueHeadRepository();
        }

        public IList<usp_EM_GetRevenueHeadList_Result> BL_GetRevenueHeadList(EM_RevenueHead pObjRevenueHead)
        {
            return _RevenueHeadRepository.REP_GetRevenueHeadList(pObjRevenueHead);
        }

        public FuncResponse BL_InsertUpdateRevenueHead(EM_RevenueHead pObjRevenueHead)
        {
            return _RevenueHeadRepository.REP_InsertUpdateRevenueHead(pObjRevenueHead);
        }

        public usp_EM_GetRevenueHeadList_Result BL_GetRevenueHeadDetails(EM_RevenueHead pObjRevenueHead)
        {
            return _RevenueHeadRepository.REP_GetRevenueHeadDetails(pObjRevenueHead);
        }

        public IList<DropDownListResult> BL_GetRevenueHeadDropDownList(EM_RevenueHead pObjRevenueHead)
        {
            return _RevenueHeadRepository.REP_GetRevenueHeadDropDownList(pObjRevenueHead);
        }

        public FuncResponse BL_UpdateStatus(EM_RevenueHead pObjRevenueHead)
        {
            return _RevenueHeadRepository.REP_UpdateStatus(pObjRevenueHead);
        }
    }
}
