using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLMenu
    {
        IMenuRepository _MenuRepository;

        public BLMenu()
        {
            _MenuRepository = new MenuRepository();
        }

        public IList<usp_GetMenuList_Result> BL_GetMenuList(MST_Menu pObjMenu)
        {
            return _MenuRepository.REP_GetMenuList(pObjMenu);
        }

        public usp_GetMenuList_Result BL_GetMenuDetails(MST_Menu pObjMenu)
        {
            return _MenuRepository.REP_GetMenuDetails(pObjMenu);
        }

        public FuncResponse BL_InsertUpdateMenu(MST_Menu pObjMenu)
        {
            return _MenuRepository.REP_InsertUpdateMenu(pObjMenu);
        }

        public IList<DropDownListResult> BL_GetParentMenuList()
        {
            return _MenuRepository.REP_GetParentMenuList();
        }
    }
}
