using EIRS.BOL;
using EIRS.Common;
using System.Collections.Generic;

namespace EIRS.Repository
{
    public interface ISFTP_DataSubmissionTypeRepository
    {
        usp_SFTP_GetDataSubmissionTypeList_Result REP_GetDataSubmissionTypeDetails(SFTP_DataSubmissionType pObjDataSubmissionType);
        IList<DropDownListResult> REP_GetDataSubmissionTypeDropDownList(SFTP_DataSubmissionType pObjDataSubmissionType);
        IList<usp_SFTP_GetDataSubmissionTypeList_Result> REP_GetDataSubmissionTypeList(SFTP_DataSubmissionType pObjDataSubmissionType);
        FuncResponse REP_InsertUpdateDataSubmissionType(SFTP_DataSubmissionType pObjDataSubmissionType);
        FuncResponse REP_UpdateStatus(SFTP_DataSubmissionType pObjDataSubmissionType);
    }
}