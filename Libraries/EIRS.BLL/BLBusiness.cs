using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLBusiness
    {
        IBusinessRepository _BusinessRepository;

        public BLBusiness()
        {
            _BusinessRepository = new BusinessRepository();
        }

        public IList<usp_GetBusinessListNewTy_Result> BL_GetBusinessList(Business pObjBusiness)
        {
            return _BusinessRepository.REP_GetBusinessList(pObjBusiness);
        }

        public IList<vw_Business> BL_GetBusinessList()
        {
            return _BusinessRepository.REP_GetBusinessList();
        }

        public FuncResponse<Business> BL_InsertUpdateBusiness(Business pObjBusiness)
        {
            return _BusinessRepository.REP_InsertUpdateBusiness(pObjBusiness);
        }

        public usp_GetBusinessListNewTy_Result BL_GetBusinessDetails(Business pObjBusiness)
        {
            return _BusinessRepository.REP_GetBusinessDetails(pObjBusiness);
        }

        public FuncResponse BL_UpdateStatus(Business pObjBusiness)
        {
            return _BusinessRepository.REP_UpdateStatus(pObjBusiness);
        }

        public FuncResponse BL_InsertBusinessBuilding(MAP_Business_Building pObjBusinessBuilding)
        {
            return _BusinessRepository.REP_InsertBusinessBuilding(pObjBusinessBuilding);
        }

        public IList<usp_GetBusinessBuildingList_Result> BL_GetBusinessBuildingList(MAP_Business_Building pObjBusinessBuilding)
        {
            return _BusinessRepository.REP_GetBusinessBuildingList(pObjBusinessBuilding);
        }

        public MAP_Business_Building BL_GetBusinessBuildingDetails(int pIntBBID)
        {
            return _BusinessRepository.REP_GetBusinessBuildingDetails(pIntBBID);
        }

        public FuncResponse<IList<usp_GetBusinessBuildingList_Result>> BL_RemoveBusinessBuilding(MAP_Business_Building pObjBusinessBuilding)
        {
            return _BusinessRepository.REP_RemoveBusinessBuilding(pObjBusinessBuilding);
        }

        public IList<usp_SearchBusinessForRDMLoad_Result> BL_SearchBusinessDetails(Business pObjBusiness)
        {
            return _BusinessRepository.REP_SearchBusinessDetails(pObjBusiness);
        }

        public IDictionary<string, object> BL_SearchBusiness(Business pObjBusiness)
        {
            return _BusinessRepository.REP_SearchBusiness(pObjBusiness);
        }

        public IDictionary<string, object> BL_SearchBusinessForSideMenu(Business pObjBusiness)
        {
            return _BusinessRepository.REP_SearchBusinessForSideMenu(pObjBusiness);
        }
    }
}
