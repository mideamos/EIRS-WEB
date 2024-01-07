using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLRevenueSubStream
    {
        IRevenueSubStreamRepository _RevenueSubStreamRepository;

        public BLRevenueSubStream()
        {
            _RevenueSubStreamRepository = new RevenueSubStreamRepository();
        }

        public IList<usp_GetRevenueSubStreamList_Result> BL_GetRevenueSubStreamList(Revenue_SubStream pObjRevenueSubStream)
        {
            return _RevenueSubStreamRepository.REP_GetRevenueSubStreamList(pObjRevenueSubStream);
        }

        public FuncResponse BL_InsertUpdateRevenueSubStream(Revenue_SubStream pObjRevenueSubStream)
        {
            return _RevenueSubStreamRepository.REP_InsertUpdateRevenueSubStream(pObjRevenueSubStream);
        }

        public usp_GetRevenueSubStreamList_Result BL_GetRevenueSubStreamDetails(Revenue_SubStream pObjRevenueSubStream)
        {
            return _RevenueSubStreamRepository.REP_GetRevenueSubStreamDetails(pObjRevenueSubStream);
        }

        public IList<DropDownListResult> BL_GetRevenueSubStreamDropDownList(Revenue_SubStream pObjRevenueSubStream)
        {
            return _RevenueSubStreamRepository.REP_GetRevenueSubStreamDropDownList(pObjRevenueSubStream);
        }

        public FuncResponse BL_UpdateStatus(Revenue_SubStream pObjRevenueSubStream)
        {
            return _RevenueSubStreamRepository.REP_UpdateStatus(pObjRevenueSubStream);
        }
    }
}
