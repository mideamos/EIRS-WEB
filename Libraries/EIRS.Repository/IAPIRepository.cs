using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IAPIRepository
    {
        usp_GetAPIList_Result REP_GetAPIDetails(MST_API pObjAPI);
        IList<usp_GetAPIList_Result> REP_GetAPIList(MST_API pObjAPI);
        FuncResponse REP_UpdateAPI(MST_API pObjAPI);

        IList<usp_GetAPIUserRightList_Result> REP_GetAPIAccessList(int pIntUserID, int pIntAPIID);

        FuncResponse REP_UpdateAPIAccess(MAP_API_Users_Rights pObjAPIAccess);

        FuncResponse REP_CheckAPIAuthorization(MST_API pObjAPI);
    }
}