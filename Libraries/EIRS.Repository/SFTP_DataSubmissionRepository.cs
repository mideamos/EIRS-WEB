using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ISFTP_DataSubmissionRepository
    {
        usp_SFTP_GetDataSubmissionList_Result REP_GetDataSubmissionDetails(SFTP_DataSubmission pObjDataSubmission);
        IList<usp_SFTP_GetDataSubmissionList_Result> REP_GetDataSubmissionList(SFTP_DataSubmission pObjDataSubmission);
        FuncResponse REP_InsertUpdateDataSubmission(SFTP_DataSubmission pObjDataSubmission);
    }

    public class SFTP_DataSubmissionRepository : ISFTP_DataSubmissionRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateDataSubmission(SFTP_DataSubmission pObjDataSubmission)
        {
            using (_db = new EIRSEntities())
            {
                SFTP_DataSubmission mObjInsertUpdateDataSubmission; //DataSubmission Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                ////Check if Duplicate
                //var vDuplicateCheck = (from dst in _db.SFTP_DataSubmission
                //                       where dst.DataSubmissionName == pObjDataSubmission.DataSubmissionName
                //                       && dst.DataSubmissionID != pObjDataSubmission.DataSubmissionID
                //                       select dst);

                //if (vDuplicateCheck.Count() > 0)
                //{
                //    mObjFuncResponse.Success = false;
                //    mObjFuncResponse.Message = "Data Submission Type already exists";
                //    return mObjFuncResponse;
                //}

                //If Update Load DataSubmission
                if (pObjDataSubmission.DataSubmissionID != 0)
                {
                    mObjInsertUpdateDataSubmission = (from dst in _db.SFTP_DataSubmission
                                                      where dst.DataSubmissionID == pObjDataSubmission.DataSubmissionID
                                                      select dst).FirstOrDefault();

                    if (mObjInsertUpdateDataSubmission != null)
                    {
                        //mObjInsertUpdateDataSubmission.ModifiedBy = pObjDataSubmission.ModifiedBy;
                        //mObjInsertUpdateDataSubmission.ModifiedDate = pObjDataSubmission.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateDataSubmission = new SFTP_DataSubmission();
                        mObjInsertUpdateDataSubmission.CreatedBy = pObjDataSubmission.CreatedBy;
                        mObjInsertUpdateDataSubmission.CreatedDate = pObjDataSubmission.CreatedDate;
                    }
                }
                else // Else Insert DataSubmission
                {
                    mObjInsertUpdateDataSubmission = new SFTP_DataSubmission();
                    mObjInsertUpdateDataSubmission.CreatedBy = pObjDataSubmission.CreatedBy;
                    mObjInsertUpdateDataSubmission.CreatedDate = pObjDataSubmission.CreatedDate;
                    mObjInsertUpdateDataSubmission.SubmissionDate = pObjDataSubmission.SubmissionDate;
                }

                mObjInsertUpdateDataSubmission.DataSubmitterID = pObjDataSubmission.DataSubmitterID;
                mObjInsertUpdateDataSubmission.TaxYear = pObjDataSubmission.TaxYear;
                mObjInsertUpdateDataSubmission.DataSubmissionTypeID = pObjDataSubmission.DataSubmissionTypeID;
                mObjInsertUpdateDataSubmission.DocumentPath = pObjDataSubmission.DocumentPath;

                if (pObjDataSubmission.DataSubmissionID == 0)
                {
                    _db.SFTP_DataSubmission.Add(mObjInsertUpdateDataSubmission);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjDataSubmission.DataSubmissionID == 0)
                        mObjFuncResponse.Message = "Data Submission Added Successfully";
                    else
                        mObjFuncResponse.Message = "Data Submission Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjDataSubmission.DataSubmissionID == 0)
                        mObjFuncResponse.Message = "Data Submission Addition Failed";
                    else
                        mObjFuncResponse.Message = "Data Submission Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_SFTP_GetDataSubmissionList_Result> REP_GetDataSubmissionList(SFTP_DataSubmission pObjDataSubmission)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_SFTP_GetDataSubmissionList(pObjDataSubmission.DataSubmitterID, pObjDataSubmission.DataSubmissionTypeID, pObjDataSubmission.DataSubmissionID).ToList();
            }
        }

        public usp_SFTP_GetDataSubmissionList_Result REP_GetDataSubmissionDetails(SFTP_DataSubmission pObjDataSubmission)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_SFTP_GetDataSubmissionList(pObjDataSubmission.DataSubmitterID, pObjDataSubmission.DataSubmissionTypeID, pObjDataSubmission.DataSubmissionID).FirstOrDefault();
            }
        }
    }
}
