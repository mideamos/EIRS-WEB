using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLLand
    {
        ILandRepository _LandRepository;

        public BLLand()
        {
            _LandRepository = new LandRepository();
        }

        public IList<usp_GetLandList_Result> BL_GetLandList(Land pObjLand)
        {
            return _LandRepository.REP_GetLandList(pObjLand);
        }

        public IList<vw_Land> BL_GetLandList()
        {
            return _LandRepository.REP_GetLandList();
        }

        public FuncResponse<Land> BL_InsertUpdateLand(Land pObjLand)
        {
            return _LandRepository.REP_InsertUpdateLand(pObjLand);
        }

        public usp_GetLandList_Result BL_GetLandDetails(Land pObjLand)
        {
            return _LandRepository.REP_GetLandDetails(pObjLand);
        }

        public FuncResponse BL_UpdateStatus(Land pObjLand)
        {
            return _LandRepository.REP_UpdateStatus(pObjLand);
        }

        public IList<usp_GetSearchLandForEdoGIS_Result> BL_SearchLandForEdoGIS(Land pObjLand)
        {
            return _LandRepository.REP_SearchLandForEdoGIS(pObjLand);
        }


        public IList<usp_SearchLandForRDMLoad_Result> BL_SearchLandDetails(Land pObjLand)
        {
            return _LandRepository.REP_SearchLandDetails(pObjLand);
        }

        public IDictionary<string, object> BL_SearchLand(Land pObjLand)
        {
            return _LandRepository.REP_SearchLand(pObjLand);
        }

        public IDictionary<string, object> BL_SearchLandForSideMenu(Land pObjLand)
        {
            return _LandRepository.REP_SearchLandForSideMenu(pObjLand);
        }
    }
}
