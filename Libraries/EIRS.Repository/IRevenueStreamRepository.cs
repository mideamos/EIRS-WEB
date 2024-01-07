using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IRevenueStreamRepository
    {
		FuncResponse REP_InsertUpdateRevenueStream(Revenue_Stream pObjRevenueStream);
		IList<usp_GetRevenueStreamList_Result> REP_GetRevenueStreamList(Revenue_Stream pObjRevenueStream);
        usp_GetRevenueStreamList_Result REP_GetRevenueStreamDetails(Revenue_Stream pObjRevenueStream);
        IList<DropDownListResult> REP_GetRevenueStreamDropDownList(Revenue_Stream pObjRevenueStream);
        FuncResponse REP_UpdateStatus(Revenue_Stream pObjRevenueStream);
    }
}