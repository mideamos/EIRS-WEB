using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLCentralMenu
    {
        ICentralMenuRepository _CentralMenuRepository;

        public BLCentralMenu()
        {
            _CentralMenuRepository = new CentralMenuRepository();
        }

        public IList<usp_GetCentralMenuList_Result> BL_GetCentralMenuList(MST_CentralMenu pObjCentralMenu)
        {
            return _CentralMenuRepository.REP_GetCentralMenuList(pObjCentralMenu);
        }

        public usp_GetCentralMenuList_Result BL_GetCentralMenuDetails(MST_CentralMenu pObjCentralMenu)
        {
            return _CentralMenuRepository.REP_GetCentralMenuDetails(pObjCentralMenu);
        }

        public FuncResponse BL_InsertUpdateMenu(MST_CentralMenu pObjMenu)
        {
            return _CentralMenuRepository.REP_InsertUpdateMenu(pObjMenu);
        }

        public IList<DropDownListResult> BL_GetParentCentralMenuList()
        {
            return _CentralMenuRepository.REP_GetParentCentralMenuList();
        }

        public IList<usp_GetCentralMenuUserBased_Result> BL_GetCentralMenuUserBased(int pIntUserID, int pIntParentMenuID)
        {
            return _CentralMenuRepository.REP_GetCentralMenuUserBased(pIntUserID, pIntParentMenuID);
        }
    }
}
