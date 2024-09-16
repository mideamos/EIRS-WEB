using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ILateChargeRepository
    {
        usp_GetLateChargeList_Result REP_GetLateChargeDetails(Late_Charges pObjLateCharge);
        IList<usp_GetLateChargeList_Result> REP_GetLateChargeList(Late_Charges pObjLateCharge);
        FuncResponse REP_InsertUpdateLateCharge(Late_Charges pObjLateCharge);
        FuncResponse REP_UpdateStatus(Late_Charges pObjLateCharge);
    }
}