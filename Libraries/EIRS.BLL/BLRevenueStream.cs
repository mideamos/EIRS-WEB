using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLRevenueStream
    {
        IRevenueStreamRepository _RevenueStreamRepository;

        public BLRevenueStream()
        {
            _RevenueStreamRepository = new RevenueStreamRepository();
        }

        public IList<usp_GetRevenueStreamList_Result> BL_GetRevenueStreamList(Revenue_Stream pObjRevenueStream)
        {
            return _RevenueStreamRepository.REP_GetRevenueStreamList(pObjRevenueStream);
        }

        public FuncResponse BL_InsertUpdateRevenueStream(Revenue_Stream pObjRevenueStream)
        {
            return _RevenueStreamRepository.REP_InsertUpdateRevenueStream(pObjRevenueStream);
        }

        public usp_GetRevenueStreamList_Result BL_GetRevenueStreamDetails(Revenue_Stream pObjRevenueStream)
        {
            return _RevenueStreamRepository.REP_GetRevenueStreamDetails(pObjRevenueStream);
        }

        public IList<DropDownListResult> BL_GetRevenueStreamDropDownList(Revenue_Stream pObjRevenueStream)
        {
            return _RevenueStreamRepository.REP_GetRevenueStreamDropDownList(pObjRevenueStream);
        }

        public FuncResponse BL_UpdateStatus(Revenue_Stream pObjRevenueStream)
        {
            return _RevenueStreamRepository.REP_UpdateStatus(pObjRevenueStream);
        }
    }
}
