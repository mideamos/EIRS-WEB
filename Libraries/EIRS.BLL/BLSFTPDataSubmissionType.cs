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
    public class BLSFTPDataSubmissionType
    {
        readonly ISFTP_DataSubmissionTypeRepository _DataSubmissionTypeRepository;

        public BLSFTPDataSubmissionType()
        {
            _DataSubmissionTypeRepository = new SFTP_DataSubmissionTypeRepository();
        }

        public IList<usp_SFTP_GetDataSubmissionTypeList_Result> BL_GetDataSubmissionTypeList(SFTP_DataSubmissionType pObjDataSubmissionType)
        {
            return _DataSubmissionTypeRepository.REP_GetDataSubmissionTypeList(pObjDataSubmissionType);
        }

        public FuncResponse BL_InsertUpdateDataSubmissionType(SFTP_DataSubmissionType pObjDataSubmissionType)
        {
            return _DataSubmissionTypeRepository.REP_InsertUpdateDataSubmissionType(pObjDataSubmissionType);
        }

        public usp_SFTP_GetDataSubmissionTypeList_Result BL_GetDataSubmissionTypeDetails(SFTP_DataSubmissionType pObjDataSubmissionType)
        {
            return _DataSubmissionTypeRepository.REP_GetDataSubmissionTypeDetails(pObjDataSubmissionType);
        }

        public IList<DropDownListResult> BL_GetDataSubmissionTypeDropDownList(SFTP_DataSubmissionType pObjDataSubmissionType)
        {
            return _DataSubmissionTypeRepository.REP_GetDataSubmissionTypeDropDownList(pObjDataSubmissionType);
        }

        public FuncResponse BL_UpdateStatus(SFTP_DataSubmissionType pObjDataSubmissionType)
        {
            return _DataSubmissionTypeRepository.REP_UpdateStatus(pObjDataSubmissionType);
        }
    }
}
