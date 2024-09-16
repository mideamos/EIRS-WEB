using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class SFTP_DataSubmitterRepository : ISFTP_DataSubmitterRepository
    {
        EIRSEntities _db;

        public FuncResponse<SFTP_DataSubmitter> REP_InsertUpdateDataSubmitter(SFTP_DataSubmitter pObjDataSubmitter)
        {
            using (_db = new EIRSEntities())
            {
                SFTP_DataSubmitter mObjInsertUpdateDataSubmitter; //DataSubmitter Insert Update Object
                FuncResponse<SFTP_DataSubmitter> mObjFuncResponse = new FuncResponse<SFTP_DataSubmitter>(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from dsub in _db.SFTP_DataSubmitter
                                       where dsub.UserName == pObjDataSubmitter.UserName
                                       && dsub.DataSubmitterID != pObjDataSubmitter.DataSubmitterID
                                       select dsub);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Data Submitter already exists";
                    return mObjFuncResponse;
                }

                //If Update Load DataSubmitter
                if (pObjDataSubmitter.DataSubmitterID != 0)
                {
                    mObjInsertUpdateDataSubmitter = (from dst in _db.SFTP_DataSubmitter
                                                     where dst.DataSubmitterID == pObjDataSubmitter.DataSubmitterID
                                                     select dst).FirstOrDefault();

                    if (mObjInsertUpdateDataSubmitter != null)
                    {
                        mObjInsertUpdateDataSubmitter.ModifiedBy = pObjDataSubmitter.ModifiedBy;
                        mObjInsertUpdateDataSubmitter.ModifiedDate = pObjDataSubmitter.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateDataSubmitter = new SFTP_DataSubmitter();
                        mObjInsertUpdateDataSubmitter.CreatedBy = pObjDataSubmitter.CreatedBy;
                        mObjInsertUpdateDataSubmitter.CreatedDate = pObjDataSubmitter.CreatedDate;
                    }
                }
                else // Else Insert DataSubmitter
                {
                    mObjInsertUpdateDataSubmitter = new SFTP_DataSubmitter();
                    mObjInsertUpdateDataSubmitter.CreatedBy = pObjDataSubmitter.CreatedBy;
                    mObjInsertUpdateDataSubmitter.CreatedDate = pObjDataSubmitter.CreatedDate;
                }

                mObjInsertUpdateDataSubmitter.RIN = pObjDataSubmitter.RIN;
                mObjInsertUpdateDataSubmitter.UserName = pObjDataSubmitter.UserName;
                mObjInsertUpdateDataSubmitter.Password = pObjDataSubmitter.Password;
                mObjInsertUpdateDataSubmitter.Active = pObjDataSubmitter.Active;

                if (pObjDataSubmitter.DataSubmitterID == 0)
                {
                    _db.SFTP_DataSubmitter.Add(mObjInsertUpdateDataSubmitter);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjDataSubmitter.DataSubmitterID == 0)
                        mObjFuncResponse.Message = "Data Submitter Added Successfully";
                    else
                        mObjFuncResponse.Message = "Data Submitter Updated Successfully";

                    mObjFuncResponse.AdditionalData = mObjInsertUpdateDataSubmitter;

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjDataSubmitter.DataSubmitterID == 0)
                        mObjFuncResponse.Message = "Data Submitter Addition Failed";
                    else
                        mObjFuncResponse.Message = "Data Submitter Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_SFTP_GetDataSubmitterList_Result> REP_GetDataSubmitterList(SFTP_DataSubmitter pObjDataSubmitter)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_SFTP_GetDataSubmitterList(pObjDataSubmitter.UserName, pObjDataSubmitter.DataSubmitterID, pObjDataSubmitter.intStatus).ToList();
            }
        }

        public usp_SFTP_GetDataSubmitterList_Result REP_GetDataSubmitterDetails(SFTP_DataSubmitter pObjDataSubmitter)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_SFTP_GetDataSubmitterList(pObjDataSubmitter.UserName, pObjDataSubmitter.DataSubmitterID, pObjDataSubmitter.intStatus).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetDataSubmitterDropDownList(SFTP_DataSubmitter pObjDataSubmitter)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from dsub in _db.usp_SFTP_GetDataSubmitterList(pObjDataSubmitter.UserName, pObjDataSubmitter.DataSubmitterID, pObjDataSubmitter.intStatus)
                               select new DropDownListResult()
                               {
                                   id = dsub.DataSubmitterID.GetValueOrDefault(),
                                   text = dsub.UserName + "(" + dsub.RIN + ")"
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_InsertDataSubmissionType(SFTP_MAP_DataSubmitter_DataSubmissionType pObjDSDST)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();

                var vExists = (from dsdst in _db.SFTP_MAP_DataSubmitter_DataSubmissionType
                               where dsdst.DataSubmitterID == pObjDSDST.DataSubmitterID && dsdst.DataSubmissionTypeID == pObjDSDST.DataSubmissionTypeID && dsdst.DSTDSID != pObjDSDST.DSTDSID
                               select dsdst).Count();

                if (vExists != 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Data Submission Type Already Exists";
                    return mObjFuncResponse;
                }

                _db.SFTP_MAP_DataSubmitter_DataSubmissionType.Add(pObjDSDST);

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.Message = "Data Submission Type added Successfully";
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Data Submission Type Add failed";
                    mObjFuncResponse.Exception = ex;
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_RemoveDataSubmissionType(SFTP_MAP_DataSubmitter_DataSubmissionType pObjDSDST)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();
                SFTP_MAP_DataSubmitter_DataSubmissionType mObjDeleteDSDST;

                mObjDeleteDSDST = _db.SFTP_MAP_DataSubmitter_DataSubmissionType.Find(pObjDSDST.DSTDSID);

                if (mObjDeleteDSDST == null)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Data Submission Type Already Deleted";
                    return mObjFuncResponse;
                }

                _db.SFTP_MAP_DataSubmitter_DataSubmissionType.Remove(mObjDeleteDSDST);

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.Message = "Data Submission Type Deleted";
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Data Submission Type Deletion Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<SFTP_MAP_DataSubmitter_DataSubmissionType> REP_GetDataSubmissionTypeList(int pIntDataSubmitterID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.SFTP_MAP_DataSubmitter_DataSubmissionType.Where(t => t.DataSubmitterID == pIntDataSubmitterID).ToList();
            }
        }

        public IList<DropDownListResult> REP_GetDataSubmissionTypeDropDownList(SFTP_DataSubmitter pObjDataSubmitter)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from dsub in _db.SFTP_MAP_DataSubmitter_DataSubmissionType
                               where dsub.DataSubmitterID == pObjDataSubmitter.DataSubmitterID
                               select new DropDownListResult()
                               {
                                   id = (int)dsub.DataSubmissionTypeID,
                                   text = dsub.SFTP_DataSubmissionType.DataSubmissionTypeName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse<SFTP_DataSubmitter> REP_CheckUserLoginDetails(SFTP_DataSubmitter pObjDataSubmitter)
        {
            FuncResponse<SFTP_DataSubmitter> mObjFuncResponse = new FuncResponse<SFTP_DataSubmitter>();

            try
            {
                using (_db = new EIRSEntities())
                {

                    var vFind = (from ds in _db.SFTP_DataSubmitter
                                 where ds.UserName.Equals(pObjDataSubmitter.UserName) && ds.Password.Equals(pObjDataSubmitter.Password)
                                 select ds);

                    if (vFind != null && vFind.Count() == 0)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = "Invalid Login Details";
                    }
                    else if (vFind.Count() > 0 && vFind.FirstOrDefault().Active == false)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = "Submitter Is Not Active";
                    }
                    else
                    {
                        mObjFuncResponse.Success = true;
                        mObjFuncResponse.Message = "Login Successfully";
                        mObjFuncResponse.AdditionalData = vFind.FirstOrDefault();
                    }

                    return mObjFuncResponse;
                }
            }
            catch (Exception ex)
            {
                mObjFuncResponse.Exception = ex;
                mObjFuncResponse.Success = false;
                return mObjFuncResponse;
            }
        }
    }
}
