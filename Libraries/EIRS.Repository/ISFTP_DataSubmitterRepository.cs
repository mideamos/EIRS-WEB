using EIRS.BOL;
using EIRS.Common;
using System.Collections.Generic;

namespace EIRS.Repository
{
    public interface ISFTP_DataSubmitterRepository
    {
        IList<SFTP_MAP_DataSubmitter_DataSubmissionType> REP_GetDataSubmissionTypeList(int pIntDataSubmitterID);
        usp_SFTP_GetDataSubmitterList_Result REP_GetDataSubmitterDetails(SFTP_DataSubmitter pObjDataSubmitter);
        IList<DropDownListResult> REP_GetDataSubmitterDropDownList(SFTP_DataSubmitter pObjDataSubmitter);
        IList<usp_SFTP_GetDataSubmitterList_Result> REP_GetDataSubmitterList(SFTP_DataSubmitter pObjDataSubmitter);
        FuncResponse REP_InsertDataSubmissionType(SFTP_MAP_DataSubmitter_DataSubmissionType pObjDSDST);
        FuncResponse<SFTP_DataSubmitter> REP_InsertUpdateDataSubmitter(SFTP_DataSubmitter pObjDataSubmitter);
        FuncResponse REP_RemoveDataSubmissionType(SFTP_MAP_DataSubmitter_DataSubmissionType pObjDSDST);

        IList<DropDownListResult> REP_GetDataSubmissionTypeDropDownList(SFTP_DataSubmitter pObjDataSubmitter);
        FuncResponse<SFTP_DataSubmitter> REP_CheckUserLoginDetails(SFTP_DataSubmitter pObjDataSubmitter);
    }
}