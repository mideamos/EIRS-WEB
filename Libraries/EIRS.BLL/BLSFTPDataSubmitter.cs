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
    public class BLSFTPDataSubmitter
    {
        readonly ISFTP_DataSubmitterRepository _DataSubmitterRepository;

        public BLSFTPDataSubmitter()
        {
            _DataSubmitterRepository = new SFTP_DataSubmitterRepository();
        }

        public IList<usp_SFTP_GetDataSubmitterList_Result> BL_GetDataSubmitterList(SFTP_DataSubmitter pObjDataSubmitter)
        {
            return _DataSubmitterRepository.REP_GetDataSubmitterList(pObjDataSubmitter);
        }

        public FuncResponse<SFTP_DataSubmitter> BL_InsertUpdateDataSubmitter(SFTP_DataSubmitter pObjDataSubmitter)
        {
            return _DataSubmitterRepository.REP_InsertUpdateDataSubmitter(pObjDataSubmitter);
        }

        public usp_SFTP_GetDataSubmitterList_Result BL_GetDataSubmitterDetails(SFTP_DataSubmitter pObjDataSubmitter)
        {
            return _DataSubmitterRepository.REP_GetDataSubmitterDetails(pObjDataSubmitter);
        }

        public IList<DropDownListResult> BL_GetDataSubmitterDropDownList(SFTP_DataSubmitter pObjDataSubmitter)
        {
            return _DataSubmitterRepository.REP_GetDataSubmitterDropDownList(pObjDataSubmitter);
        }

        public FuncResponse BL_InsertDataSubmissionType(SFTP_MAP_DataSubmitter_DataSubmissionType pObjDSDST)
        {
            return _DataSubmitterRepository.REP_InsertDataSubmissionType(pObjDSDST);
        }

        public FuncResponse BL_RemoveDataSubmissionType(SFTP_MAP_DataSubmitter_DataSubmissionType pObjDSDST)
        {
            return _DataSubmitterRepository.REP_RemoveDataSubmissionType(pObjDSDST);
        }

        public IList<SFTP_MAP_DataSubmitter_DataSubmissionType> BL_GetDataSubmissionTypeList(int pIntDataSubmitterID)
        {
            return _DataSubmitterRepository.REP_GetDataSubmissionTypeList(pIntDataSubmitterID);
        }

        public IList<DropDownListResult> BL_GetDataSubmissionTypeDropDownList(SFTP_DataSubmitter pObjDataSubmitter)
        {
            return _DataSubmitterRepository.REP_GetDataSubmissionTypeDropDownList(pObjDataSubmitter);
        }

        public FuncResponse<SFTP_DataSubmitter> BL_CheckUserLoginDetails(SFTP_DataSubmitter pObjDataSubmitter)
        {
            return _DataSubmitterRepository.REP_CheckUserLoginDetails(pObjDataSubmitter);
        }
    }
}
