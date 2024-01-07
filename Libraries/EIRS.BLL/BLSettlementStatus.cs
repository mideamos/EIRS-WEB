using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLSettlementStatus
    {
        ISettlementStatusRepository _SettlementStatusRepository;
        public BLSettlementStatus()
        {
            _SettlementStatusRepository = new SettlementStatusRepository();
        }

        public FuncResponse BL_InsertUpdateSettlementStatus(Settlement_Status pObjNotificationMethod)
        {
            return _SettlementStatusRepository.REP_InsertUpdateSettlementStatus(pObjNotificationMethod);
        }

        public IList<usp_GetSettlementStatusList_Result> BL_GetSettlementStatusList(Settlement_Status pObjSettlementStatus)
        {
            return _SettlementStatusRepository.REP_GetSettlementStatusList(pObjSettlementStatus);
        }

        public usp_GetSettlementStatusList_Result BL_GetSettlementStatusDetails(Settlement_Status pObjSettlementStatus)
        {
            return _SettlementStatusRepository.REP_GetSettlementStatusDetails(pObjSettlementStatus);
        }

        public IList<DropDownListResult> BL_GetSettlementStatusDropDownList(Settlement_Status pObjSettlementStatus)
        {
            return _SettlementStatusRepository.REP_GetSettlementStatusDropDownList(pObjSettlementStatus);
        }

        public FuncResponse BL_UpdateStatus(Settlement_Status pObjSettlementStatus)
        {
            return _SettlementStatusRepository.REP_UpdateStatus(pObjSettlementStatus);
        }
    }
}
