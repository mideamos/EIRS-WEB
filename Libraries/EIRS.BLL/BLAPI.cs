using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLAPI
    {
        IAPIRepository _APIRepository;

        public BLAPI()
        {
            _APIRepository = new APIRepository();
        }

        public IList<usp_GetAPIList_Result> BL_GetAPIList(MST_API pObjAPI)
        {
            return _APIRepository.REP_GetAPIList(pObjAPI);
        }

        public usp_GetAPIList_Result BL_GetAPIDetails(MST_API pObjAPI)
        {
            return _APIRepository.REP_GetAPIDetails(pObjAPI);
        }

        public FuncResponse BL_UpdateAPI(MST_API pObjAPI)
        {
            return _APIRepository.REP_UpdateAPI(pObjAPI);
        }

        public IList<usp_GetAPIUserRightList_Result> BL_GetAPIAccessList(int pIntUserID, int pIntAPIID)
        {
            return _APIRepository.REP_GetAPIAccessList(pIntUserID, pIntAPIID);
        }

        public FuncResponse BL_UpdateAPIAccess(MAP_API_Users_Rights pObjAPIAccess)
        {
            return _APIRepository.REP_UpdateAPIAccess(pObjAPIAccess);
        }

        public FuncResponse BL_CheckAPIAuthorization(MST_API pObjAPI)
        {
            return _APIRepository.REP_CheckAPIAuthorization(pObjAPI);
        }
    }

}
