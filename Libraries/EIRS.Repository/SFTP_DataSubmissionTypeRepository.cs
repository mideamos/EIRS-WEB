using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class SFTP_DataSubmissionTypeRepository : ISFTP_DataSubmissionTypeRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateDataSubmissionType(SFTP_DataSubmissionType pObjDataSubmissionType)
        {
            using (_db = new EIRSEntities())
            {
                SFTP_DataSubmissionType mObjInsertUpdateDataSubmissionType; //DataSubmissionType Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from dst in _db.SFTP_DataSubmissionType
                                       where dst.DataSubmissionTypeName == pObjDataSubmissionType.DataSubmissionTypeName
                                       && dst.DataSubmissionTypeID != pObjDataSubmissionType.DataSubmissionTypeID
                                       select dst);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Data Submission Type already exists";
                    return mObjFuncResponse;
                }

                //If Update Load DataSubmissionType
                if (pObjDataSubmissionType.DataSubmissionTypeID != 0)
                {
                    mObjInsertUpdateDataSubmissionType = (from dst in _db.SFTP_DataSubmissionType
                                                          where dst.DataSubmissionTypeID == pObjDataSubmissionType.DataSubmissionTypeID
                                                          select dst).FirstOrDefault();

                    if (mObjInsertUpdateDataSubmissionType != null)
                    {
                        mObjInsertUpdateDataSubmissionType.ModifiedBy = pObjDataSubmissionType.ModifiedBy;
                        mObjInsertUpdateDataSubmissionType.ModifiedDate = pObjDataSubmissionType.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateDataSubmissionType = new SFTP_DataSubmissionType();
                        mObjInsertUpdateDataSubmissionType.CreatedBy = pObjDataSubmissionType.CreatedBy;
                        mObjInsertUpdateDataSubmissionType.CreatedDate = pObjDataSubmissionType.CreatedDate;
                    }
                }
                else // Else Insert DataSubmissionType
                {
                    mObjInsertUpdateDataSubmissionType = new SFTP_DataSubmissionType();
                    mObjInsertUpdateDataSubmissionType.CreatedBy = pObjDataSubmissionType.CreatedBy;
                    mObjInsertUpdateDataSubmissionType.CreatedDate = pObjDataSubmissionType.CreatedDate;
                }

                mObjInsertUpdateDataSubmissionType.DataSubmissionTypeName = pObjDataSubmissionType.DataSubmissionTypeName;
                mObjInsertUpdateDataSubmissionType.TemplateFilePath = pObjDataSubmissionType.TemplateFilePath;
                mObjInsertUpdateDataSubmissionType.Active = pObjDataSubmissionType.Active;

                if (pObjDataSubmissionType.DataSubmissionTypeID == 0)
                {
                    _db.SFTP_DataSubmissionType.Add(mObjInsertUpdateDataSubmissionType);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjDataSubmissionType.DataSubmissionTypeID == 0)
                        mObjFuncResponse.Message = "Data Submission Type Added Successfully";
                    else
                        mObjFuncResponse.Message = "Data Submission Type Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjDataSubmissionType.DataSubmissionTypeID == 0)
                        mObjFuncResponse.Message = "Data Submission Type Addition Failed";
                    else
                        mObjFuncResponse.Message = "Data Submission Type Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_SFTP_GetDataSubmissionTypeList_Result> REP_GetDataSubmissionTypeList(SFTP_DataSubmissionType pObjDataSubmissionType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_SFTP_GetDataSubmissionTypeList(pObjDataSubmissionType.DataSubmissionTypeName, pObjDataSubmissionType.DataSubmissionTypeID, pObjDataSubmissionType.DataSubmissionTypeIds, pObjDataSubmissionType.intStatus, pObjDataSubmissionType.IncludeDataSubmissionTypeIds, pObjDataSubmissionType.ExcludeDataSubmissionTypeIds).ToList();
            }
        }

        public usp_SFTP_GetDataSubmissionTypeList_Result REP_GetDataSubmissionTypeDetails(SFTP_DataSubmissionType pObjDataSubmissionType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_SFTP_GetDataSubmissionTypeList(pObjDataSubmissionType.DataSubmissionTypeName, pObjDataSubmissionType.DataSubmissionTypeID, pObjDataSubmissionType.DataSubmissionTypeIds, pObjDataSubmissionType.intStatus, pObjDataSubmissionType.IncludeDataSubmissionTypeIds, pObjDataSubmissionType.ExcludeDataSubmissionTypeIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetDataSubmissionTypeDropDownList(SFTP_DataSubmissionType pObjDataSubmissionType)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from cat in _db.usp_SFTP_GetDataSubmissionTypeList(pObjDataSubmissionType.DataSubmissionTypeName, pObjDataSubmissionType.DataSubmissionTypeID, pObjDataSubmissionType.DataSubmissionTypeIds, pObjDataSubmissionType.intStatus, pObjDataSubmissionType.IncludeDataSubmissionTypeIds, pObjDataSubmissionType.ExcludeDataSubmissionTypeIds)
                               select new DropDownListResult()
                               {
                                   id = cat.DataSubmissionTypeID.GetValueOrDefault(),
                                   text = cat.DataSubmissionTypeName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(SFTP_DataSubmissionType pObjDataSubmissionType)
        {
            using (_db = new EIRSEntities())
            {
                SFTP_DataSubmissionType mObjInsertUpdateDataSubmissionType; //DataSubmissionType Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load DataSubmissionType
                if (pObjDataSubmissionType.DataSubmissionTypeID != 0)
                {
                    mObjInsertUpdateDataSubmissionType = (from cat in _db.SFTP_DataSubmissionType
                                                          where cat.DataSubmissionTypeID == pObjDataSubmissionType.DataSubmissionTypeID
                                                          select cat).FirstOrDefault();

                    if (mObjInsertUpdateDataSubmissionType != null)
                    {
                        mObjInsertUpdateDataSubmissionType.Active = !mObjInsertUpdateDataSubmissionType.Active;
                        mObjInsertUpdateDataSubmissionType.ModifiedBy = pObjDataSubmissionType.ModifiedBy;
                        mObjInsertUpdateDataSubmissionType.ModifiedDate = pObjDataSubmissionType.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Data Submission Type Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_SFTP_GetDataSubmissionTypeList(pObjDataSubmissionType.DataSubmissionTypeName, 0, pObjDataSubmissionType.DataSubmissionTypeIds, pObjDataSubmissionType.intStatus, pObjDataSubmissionType.IncludeDataSubmissionTypeIds, pObjDataSubmissionType.ExcludeDataSubmissionTypeIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Data Submission Type Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
