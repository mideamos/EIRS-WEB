using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ICentralMenuRepository
    {
        usp_GetCentralMenuList_Result REP_GetCentralMenuDetails(MST_CentralMenu pObjCentralMenu);
        IList<usp_GetCentralMenuList_Result> REP_GetCentralMenuList(MST_CentralMenu pObjCentralMenu);
        IList<usp_GetCentralMenuUserBased_Result> REP_GetCentralMenuUserBased(int pIntUserID, int pIntParentMenuID);
        IList<DropDownListResult> REP_GetParentCentralMenuList();
        FuncResponse REP_InsertUpdateMenu(MST_CentralMenu pObjMenu);
    }
}