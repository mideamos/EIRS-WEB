using EIRS.BOL;
using EIRS.Common;
using System.Collections.Generic;

namespace EIRS.Repository
{
    public interface IScreenRepository
    {
        FuncResponse REP_CheckScreenAuthorization(MST_Screen pObjScreen);

        MST_Screen REP_GetScreenUserBased(MST_Screen pObjScreen);

        usp_GetScreenList_Result REP_GetScreenDetails(MST_Screen pObjScreen);

        IList<usp_GetScreenList_Result> REP_GetScreenList(MST_Screen pObjScreen);

        IList<usp_GetScreenUserList_Result> REP_GetScreenUserList(MST_Screen pObjScreen);

        IList<usp_GetScreenMenuList_Result> REP_GetScreenMenuList(MST_Screen pObjScreen);

        FuncResponse REP_InsertUserScreen(MAP_User_Screen pObjUserScreen);

        FuncResponse<IList<usp_GetScreenUserList_Result>> REP_RemoveUserScreen(MAP_User_Screen pObjUserScreen);

        FuncResponse REP_InsertCentralMenuScreen(MAP_CentralMenu_Screen pObjCentralMenuScreen);

        FuncResponse<IList<usp_GetScreenMenuList_Result>> REP_RemoveMenuScreen(MAP_CentralMenu_Screen pObjMenuScreen);
    }
}