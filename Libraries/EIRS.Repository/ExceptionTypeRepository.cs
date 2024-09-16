using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EIRS.Repository
{
    public class ExceptionTypeRepository : IExceptionTypeRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateExceptionType(Exception_Type pObjExceptionType)
        {
            using (_db = new EIRSEntities())
            {
                Exception_Type mObjInsertUpdateExceptionType;
                FuncResponse mObjFuncRespsonse = new FuncResponse();
                if (pObjExceptionType == null)
                {
                    mObjFuncRespsonse.Success = false;
                    mObjFuncRespsonse.Message = "No Data";
                    return mObjFuncRespsonse;
                }
                else
                {
                    var vduplicateExceptionType = (from ExcType in _db.Exception_Type where ExcType.ExceptionTypeName == pObjExceptionType.ExceptionTypeName && ExcType.ExceptionTypeID != pObjExceptionType.ExceptionTypeID select ExcType);
                    if (vduplicateExceptionType != null && vduplicateExceptionType.Count() > 0)
                    {
                        mObjFuncRespsonse.Success = false;
                        mObjFuncRespsonse.Message = "Exception Type already exists";
                        return mObjFuncRespsonse;
                    }
                    else
                    {
                        // Update Exception Type
                        if (pObjExceptionType.ExceptionTypeID != 0)
                        {
                            mObjInsertUpdateExceptionType = (from ExcType in _db.Exception_Type where (ExcType.ExceptionTypeID == pObjExceptionType.ExceptionTypeID) select ExcType).FirstOrDefault();

                            if (mObjInsertUpdateExceptionType != null)
                            {
                                mObjInsertUpdateExceptionType.ModifiedBy = pObjExceptionType.ModifiedBy;
                                mObjInsertUpdateExceptionType.ModifiedDate = pObjExceptionType.ModifiedDate;
                            }
                            else
                            {
                                mObjInsertUpdateExceptionType = new Exception_Type()
                                {
                                    CreatedBy = pObjExceptionType.CreatedBy,
                                    CreatedDate = pObjExceptionType.CreatedDate
                                };
                            }
                        }
                        // Add Exception Type
                        else
                        {
                            mObjInsertUpdateExceptionType = new Exception_Type()
                            {
                                CreatedBy = pObjExceptionType.CreatedBy,
                                CreatedDate = pObjExceptionType.CreatedDate,
                            };
                        }
                        mObjInsertUpdateExceptionType.Active = pObjExceptionType.Active;
                        mObjInsertUpdateExceptionType.ExceptionTypeName = pObjExceptionType.ExceptionTypeName;

                        if (pObjExceptionType.ExceptionTypeID == 0)
                        {
                            _db.Exception_Type.Add(mObjInsertUpdateExceptionType);
                        }

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncRespsonse.Success = true;
                            mObjFuncRespsonse.Message = pObjExceptionType.ExceptionTypeID == 0 ? "Added Successfully" : "Updated Successfully";
                        }
                        catch (Exception ex)
                        {
                            mObjFuncRespsonse.Success = false;
                            mObjFuncRespsonse.Message = pObjExceptionType.ExceptionTypeID == 0 ? "Addition Failed" : "Updation Failed";

                        }
                        return mObjFuncRespsonse;
                    }
                }
            }
        }


        public IList<usp_GetExceptionTypeList_Result> REP_GetExceptionTypeList(Exception_Type pObjExceptionType)
        {
            using (_db = new EIRSEntities())
            {
                var vlstExceptionType = _db.usp_GetExceptionTypeList(pObjExceptionType.ExceptionTypeID, pObjExceptionType.ExceptionTypeName, pObjExceptionType.ExceptionTypeIds, pObjExceptionType.intStatus, pObjExceptionType.IncludeExceptionTypeIds, pObjExceptionType.ExcludeExceptionTypeIds).ToList();
                return vlstExceptionType;
            }
        }

        public usp_GetExceptionTypeList_Result REP_GetExceptionTypeDetails(Exception_Type pObjExceptionType)
        {
            using (_db = new EIRSEntities())
            {
                var vExceptionTypeDetails = _db.usp_GetExceptionTypeList(pObjExceptionType.ExceptionTypeID, pObjExceptionType.ExceptionTypeName, pObjExceptionType.ExceptionTypeIds, pObjExceptionType.intStatus, pObjExceptionType.IncludeExceptionTypeIds, pObjExceptionType.ExcludeExceptionTypeIds).FirstOrDefault();
                return vExceptionTypeDetails;
            }
        }

        public FuncResponse REP_UpdateStatus(Exception_Type pObjExceptionType)
        {
            using (_db = new EIRSEntities())
            {
                Exception_Type mObjInsertUpdateExceptionType; //Exception Type Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Exception Type
                if (pObjExceptionType.ExceptionTypeID != 0)
                {
                    mObjInsertUpdateExceptionType = (from notM in _db.Exception_Type
                                                          where notM.ExceptionTypeID == pObjExceptionType.ExceptionTypeID
                                                          select notM).FirstOrDefault();

                    if (mObjInsertUpdateExceptionType != null)
                    {
                        mObjInsertUpdateExceptionType.Active = !mObjInsertUpdateExceptionType.Active;
                        mObjInsertUpdateExceptionType.ModifiedBy = pObjExceptionType.ModifiedBy;
                        mObjInsertUpdateExceptionType.ModifiedDate = pObjExceptionType.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Exception Type Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetExceptionTypeList(0, pObjExceptionType.ExceptionTypeName, pObjExceptionType.ExceptionTypeIds, pObjExceptionType.intStatus, pObjExceptionType.IncludeExceptionTypeIds, pObjExceptionType.ExcludeExceptionTypeIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Exception Type Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

    }
}
