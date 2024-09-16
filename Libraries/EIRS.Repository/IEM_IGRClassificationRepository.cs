using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IEM_IGRClassificationRepository
    {
        IList<vw_EM_BankStatement> REP_GetBankStatementList(long plngIGRClassificationID);
        IList<usp_EM_GetClassificationDataSourceList_Result> REP_GetClassificationDataSource(long plngIGRClassificationID);
        usp_EM_GetIGRClassificationList_Result REP_GetIGRClassificationDetails(EM_IGRClassification pObjIGRClassification);
        IList<usp_EM_GetIGRClassificationList_Result> REP_GetIGRClassificationList(EM_IGRClassification pObjIGRClassification);
        IList<vw_EM_PD_Main_Authorized> REP_GetPDMainAuthorizedList(long plngIGRClassificationID);
        IList<vw_EM_PD_Main_Pending> REP_GetPDMainPendingList(long plngIGRClassificationID);
        IList<vw_EM_PD_MVA_Authorized> REP_GetPDMVAAuthorizedList(long plngIGRClassificationID);
        IList<vw_EM_PD_MVA_Pending> REP_GetPDMVAPendingList(long plngIGRClassificationID);
        FuncResponse REP_InsertUpdateIGRClassification(EM_IGRClassification pObjIGRClassification);
        FuncResponse REP_UpdateStatus(EM_IGRClassification pObjIGRClassification);

        FuncResponse REP_InsertClassificationEntry(EM_MAP_IGRClassification_Entry pObjClassificationEntry);
    }
}