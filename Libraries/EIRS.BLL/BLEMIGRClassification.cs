using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLEMIGRClassification
    {
        readonly IEM_IGRClassificationRepository _IGRClassificationRepository;

        public BLEMIGRClassification()
        {
            _IGRClassificationRepository = new EM_IGRClassificationRepository();
        }

        public IList<usp_EM_GetIGRClassificationList_Result> BL_GetIGRClassificationList(EM_IGRClassification pObjIGRClassification)
        {
            return _IGRClassificationRepository.REP_GetIGRClassificationList(pObjIGRClassification);
        }

        public FuncResponse BL_InsertUpdateIGRClassification(EM_IGRClassification pObjIGRClassification)
        {
            return _IGRClassificationRepository.REP_InsertUpdateIGRClassification(pObjIGRClassification);
        }

        public usp_EM_GetIGRClassificationList_Result BL_GetIGRClassificationDetails(EM_IGRClassification pObjIGRClassification)
        {
            return _IGRClassificationRepository.REP_GetIGRClassificationDetails(pObjIGRClassification);
        }

        
        public FuncResponse BL_UpdateStatus(EM_IGRClassification pObjIGRClassification)
        {
            return _IGRClassificationRepository.REP_UpdateStatus(pObjIGRClassification);
        }

        public IList<usp_EM_GetClassificationDataSourceList_Result> BL_GetClassificationDataSource(long plngIGRClassificationID)
        {
            return _IGRClassificationRepository.REP_GetClassificationDataSource(plngIGRClassificationID);
        }

        public IList<vw_EM_PD_Main_Authorized> BL_GetPDMainAuthorizedList(long plngIGRClassificationID)
        {
            return _IGRClassificationRepository.REP_GetPDMainAuthorizedList(plngIGRClassificationID);
        }

        public IList<vw_EM_PD_Main_Pending> BL_GetPDMainPendingList(long plngIGRClassificationID)
        {
            return _IGRClassificationRepository.REP_GetPDMainPendingList(plngIGRClassificationID);
        }

        public IList<vw_EM_PD_MVA_Authorized> BL_GetPDMVAAuthorizedList(long plngIGRClassificationID)
        {
            return _IGRClassificationRepository.REP_GetPDMVAAuthorizedList(plngIGRClassificationID);
        }

        public IList<vw_EM_PD_MVA_Pending> BL_GetPDMVAPendingList(long plngIGRClassificationID)
        {
            return _IGRClassificationRepository.REP_GetPDMVAPendingList(plngIGRClassificationID);
        }

        public IList<vw_EM_BankStatement> BL_GetBankStatementList(long plngIGRClassificationID)
        {
            return _IGRClassificationRepository.REP_GetBankStatementList(plngIGRClassificationID);
        }

        public FuncResponse BL_InsertClassificationEntry(EM_MAP_IGRClassification_Entry pObjClassificationEntry)
        {
            return _IGRClassificationRepository.REP_InsertClassificationEntry(pObjClassificationEntry);
        }
    }
}
