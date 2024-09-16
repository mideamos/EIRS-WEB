using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IRevenueSubStreamRepository
    {
		FuncResponse REP_InsertUpdateRevenueSubStream(Revenue_SubStream pObjRevenueSubStream);
		IList<usp_GetRevenueSubStreamList_Result> REP_GetRevenueSubStreamList(Revenue_SubStream pObjRevenueSubStream);
        usp_GetRevenueSubStreamList_Result REP_GetRevenueSubStreamDetails(Revenue_SubStream pObjRevenueSubStream);
        IList<DropDownListResult> REP_GetRevenueSubStreamDropDownList(Revenue_SubStream pObjRevenueSubStream);
        FuncResponse REP_UpdateStatus(Revenue_SubStream pObjRevenueSubStream);
    }
}