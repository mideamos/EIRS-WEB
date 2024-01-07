using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.BLL
{
    public class BLSFTPDataSubmission
    {
        readonly ISFTP_DataSubmissionRepository _DataSubmissionRepository;

        public BLSFTPDataSubmission()
        {
            _DataSubmissionRepository = new SFTP_DataSubmissionRepository();
        }

        public IList<usp_SFTP_GetDataSubmissionList_Result> BL_GetDataSubmissionList(SFTP_DataSubmission pObjDataSubmission)
        {
            return _DataSubmissionRepository.REP_GetDataSubmissionList(pObjDataSubmission);
        }

        public FuncResponse BL_InsertUpdateDataSubmission(SFTP_DataSubmission pObjDataSubmission)
        {
            return _DataSubmissionRepository.REP_InsertUpdateDataSubmission(pObjDataSubmission);
        }

        public usp_SFTP_GetDataSubmissionList_Result BL_GetDataSubmissionDetails(SFTP_DataSubmission pObjDataSubmission)
        {
            return _DataSubmissionRepository.REP_GetDataSubmissionDetails(pObjDataSubmission);
        }
    }
}
