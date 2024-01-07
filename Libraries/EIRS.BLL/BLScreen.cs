using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLScreen
    {
        IScreenRepository _ScreenRepository;

        public BLScreen()
        {
            _ScreenRepository = new ScreenRepository();
        }

        public IList<usp_GetScreenList_Result> BL_GetScreenList(MST_Screen pObjScreen)
        {
            return _ScreenRepository.REP_GetScreenList(pObjScreen);
        }

        public usp_GetScreenList_Result BL_GetScreenDetails(MST_Screen pObjScreen)
        {
            return _ScreenRepository.REP_GetScreenDetails(pObjScreen);
        }

        public FuncResponse BL_CheckScreenAuthorization(MST_Screen pObjScreen)
        {
            return _ScreenRepository.REP_CheckScreenAuthorization(pObjScreen);
        }

        public MST_Screen BL_GetScreenUserBased(MST_Screen pObjScreen)
        {
            return _ScreenRepository.REP_GetScreenUserBased(pObjScreen);
        }

        public IList<usp_GetScreenUserList_Result> BL_GetScreenUserList(MST_Screen pObjScreen)
        {
            return _ScreenRepository.REP_GetScreenUserList(pObjScreen);
        }

        public IList<usp_GetScreenMenuList_Result> BL_GetScreenMenuList(MST_Screen pObjScreen)
        {
            return _ScreenRepository.REP_GetScreenMenuList(pObjScreen);
        }

        public FuncResponse BL_InsertUserScreen(MAP_User_Screen pObjUserScreen)
        {
            return _ScreenRepository.REP_InsertUserScreen(pObjUserScreen);
        }

        public FuncResponse<IList<usp_GetScreenUserList_Result>> BL_RemoveUserScreen(MAP_User_Screen pObjUserScreen)
        {
            return _ScreenRepository.REP_RemoveUserScreen(pObjUserScreen);
        }

        public FuncResponse BL_InsertCentralMenuScreen(MAP_CentralMenu_Screen pObjCentralMenuScreen)
        {
            return _ScreenRepository.REP_InsertCentralMenuScreen(pObjCentralMenuScreen);
        }

        public FuncResponse<IList<usp_GetScreenMenuList_Result>> BL_RemoveMenuScreen(MAP_CentralMenu_Screen pObjMenuScreen)
        {
            return _ScreenRepository.REP_RemoveMenuScreen(pObjMenuScreen);
        }
    }
}
