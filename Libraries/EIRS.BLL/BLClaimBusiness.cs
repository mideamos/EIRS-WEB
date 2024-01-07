using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLClaimBusiness
    {
        IClaimBusinessRepository _ClaimBusinessRepository;

        public BLClaimBusiness()
        {
            _ClaimBusinessRepository = new ClaimBusinessRepository();
        }

        public FuncResponse BL_InsertUpdateBusiness(MST_Business pObjBusiness)
        {
            return _ClaimBusinessRepository.REP_InsertUpdateBusiness(pObjBusiness);
        }

        public usp_GetClaimBusinessList_Result BL_GetBusinessDetails(MST_Business pObjBusiness)
        {
            return _ClaimBusinessRepository.REP_GetBusinessDetails(pObjBusiness);
        }

        public IList<usp_GetClaimBusinessList_Result> BL_GetBusinessList(MST_Business pObjBusiness)
        {
            return _ClaimBusinessRepository.REP_GetBusinessList(pObjBusiness);
        }

        public FuncResponse BL_UpdateBusiness(MST_Business pObjBusiness)
        {
            return _ClaimBusinessRepository.REP_UpdateBusiness(pObjBusiness);
        }

        public FuncResponse BL_UpdateStatus(MST_Business pObjBusiness)
        {
            return _ClaimBusinessRepository.REP_UpdateStatus(pObjBusiness);
        }

        public FuncResponse BL_UpdatePhoneVerified(MST_Business pObjBusiness)
        {
            return _ClaimBusinessRepository.REP_UpdatePhoneVerified(pObjBusiness);
        }
    }
}
