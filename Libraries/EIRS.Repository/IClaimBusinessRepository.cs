using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IClaimBusinessRepository
    {
        usp_GetClaimBusinessList_Result REP_GetBusinessDetails(MST_Business pObjBusiness);
        IList<usp_GetClaimBusinessList_Result> REP_GetBusinessList(MST_Business pObjBusiness);
        FuncResponse REP_InsertUpdateBusiness(MST_Business pObjBusiness);
        FuncResponse REP_UpdateBusiness(MST_Business pObjBusiness);
        FuncResponse REP_UpdatePhoneVerified(MST_Business pObjBusiness);
        FuncResponse REP_UpdateStatus(MST_Business pObjBusiness);
    }
}