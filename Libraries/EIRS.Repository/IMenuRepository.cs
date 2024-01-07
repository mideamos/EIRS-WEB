using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IMenuRepository
    {
        IList<usp_GetMenuList_Result> REP_GetMenuList(MST_Menu pObjMenu);

        usp_GetMenuList_Result REP_GetMenuDetails(MST_Menu pObjMenu);

        FuncResponse REP_InsertUpdateMenu(MST_Menu pObjMenu);

        IList<DropDownListResult> REP_GetParentMenuList();
    }
}