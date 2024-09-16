using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLLateCharge
    {
        ILateChargeRepository _LateChargeRepository;

        public BLLateCharge()
        {
            _LateChargeRepository = new LateChargeRepository();
        }

        public IList<usp_GetLateChargeList_Result> BL_GetLateChargeList(Late_Charges pObjLateCharge)
        {
            return _LateChargeRepository.REP_GetLateChargeList(pObjLateCharge);
        }

        public FuncResponse BL_InsertUpdateLateCharge(Late_Charges pObjLateCharge)
        {
            return _LateChargeRepository.REP_InsertUpdateLateCharge(pObjLateCharge);
        }

        public usp_GetLateChargeList_Result BL_GetLateChargeDetails(Late_Charges pObjLateCharge)
        {
            return _LateChargeRepository.REP_GetLateChargeDetails(pObjLateCharge);
        }

        public FuncResponse BL_UpdateStatus(Late_Charges pObjLateCharge)
        {
            return _LateChargeRepository.REP_UpdateStatus(pObjLateCharge);
        }
    }
}
