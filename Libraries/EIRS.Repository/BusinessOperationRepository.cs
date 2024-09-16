using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class BusinessOperationRepository : IBusinessOperationRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateBusinessOperation(Business_Operation pObjBusinessOperation)
        {
            using (_db = new EIRSEntities())
            {
                Business_Operation mObjInsertUpdateBusinessOperation; //Business Operation Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from boperat in _db.Business_Operation
                                       where boperat.BusinessOperationName == pObjBusinessOperation.BusinessOperationName && boperat.BusinessTypeID == pObjBusinessOperation.BusinessTypeID && boperat.BusinessOperationID != pObjBusinessOperation.BusinessOperationID
                                       select boperat);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Business Operation already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Business Operation
                if (pObjBusinessOperation.BusinessOperationID != 0)
                {
                    mObjInsertUpdateBusinessOperation = (from boperat in _db.Business_Operation
                                                 where boperat.BusinessOperationID == pObjBusinessOperation.BusinessOperationID
                                                 select boperat).FirstOrDefault();

                    if (mObjInsertUpdateBusinessOperation != null)
                    {
                        mObjInsertUpdateBusinessOperation.ModifiedBy = pObjBusinessOperation.ModifiedBy;
                        mObjInsertUpdateBusinessOperation.ModifiedDate = pObjBusinessOperation.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateBusinessOperation = new Business_Operation();
                        mObjInsertUpdateBusinessOperation.CreatedBy = pObjBusinessOperation.CreatedBy;
                        mObjInsertUpdateBusinessOperation.CreatedDate = pObjBusinessOperation.CreatedDate;
                    }
                }
                else // Else Insert Business Operation
                {
                    mObjInsertUpdateBusinessOperation = new Business_Operation();
                    mObjInsertUpdateBusinessOperation.CreatedBy = pObjBusinessOperation.CreatedBy;
                    mObjInsertUpdateBusinessOperation.CreatedDate = pObjBusinessOperation.CreatedDate;
                }

                mObjInsertUpdateBusinessOperation.BusinessOperationName = pObjBusinessOperation.BusinessOperationName;
                mObjInsertUpdateBusinessOperation.BusinessTypeID = pObjBusinessOperation.BusinessTypeID;
                mObjInsertUpdateBusinessOperation.Active = pObjBusinessOperation.Active;

                if (pObjBusinessOperation.BusinessOperationID == 0)
                {
                    _db.Business_Operation.Add(mObjInsertUpdateBusinessOperation);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjBusinessOperation.BusinessOperationID == 0)
                        mObjFuncResponse.Message = "Business Operation Added Successfully";
                    else
                        mObjFuncResponse.Message = "Business Operation Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjBusinessOperation.BusinessOperationID == 0)
                        mObjFuncResponse.Message = "Business Operation Addition Failed";
                    else
                        mObjFuncResponse.Message = "Business Operation Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetBusinessOperationList_Result> REP_GetBusinessOperationList(Business_Operation pObjBusinessOperation)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBusinessOperationList(pObjBusinessOperation.BusinessOperationName, pObjBusinessOperation.BusinessOperationID, pObjBusinessOperation.BusinessTypeID, pObjBusinessOperation.BusinessOperationIds, pObjBusinessOperation.intStatus, pObjBusinessOperation.IncludeBusinessOperationIds, pObjBusinessOperation.ExcludeBusinessOperationIds).ToList();
            }
        }

        public usp_GetBusinessOperationList_Result REP_GetBusinessOperationDetails(Business_Operation pObjBusinessOperation)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBusinessOperationList(pObjBusinessOperation.BusinessOperationName, pObjBusinessOperation.BusinessOperationID, pObjBusinessOperation.BusinessTypeID, pObjBusinessOperation.BusinessOperationIds, pObjBusinessOperation.intStatus, pObjBusinessOperation.IncludeBusinessOperationIds, pObjBusinessOperation.ExcludeBusinessOperationIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetBusinessOperationDropDownList(Business_Operation pObjBusinessOperation)
        {
            using (_db = new EIRSEntities())
            {
                //var vResult = (from boperat in _db.usp_GetBusinessOperationList(pObjBusinessOperation.BusinessOperationName, pObjBusinessOperation.BusinessOperationID, pObjBusinessOperation.BusinessTypeID, pObjBusinessOperation.BusinessOperationIds, pObjBusinessOperation.intStatus, pObjBusinessOperation.IncludeBusinessOperationIds, pObjBusinessOperation.ExcludeBusinessOperationIds)
                //               select new DropDownListResult()
                //               {
                //                   id = boperat.BusinessOperationID.GetValueOrDefault(),
                //                   text = boperat.BusinessOperationName
                //               }).ToList();

                //return vResult;
                var vResult = from pro in _db.Business_Operation
                              where pro.BusinessTypeID == pObjBusinessOperation.BusinessTypeID
                              select new DropDownListResult() { id = pro.BusinessOperationID, text = pro.BusinessOperationName };

                return vResult.ToList();
            }
        }

        public FuncResponse REP_UpdateStatus(Business_Operation pObjBusinessOperation)
        {
            using (_db = new EIRSEntities())
            {
                Business_Operation mObjInsertUpdateBusinessOperation; //Business Operation Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load BusinessOperation
                if (pObjBusinessOperation.BusinessOperationID != 0)
                {
                    mObjInsertUpdateBusinessOperation = (from boperat in _db.Business_Operation
                                                 where boperat.BusinessOperationID == pObjBusinessOperation.BusinessOperationID
                                                 select boperat).FirstOrDefault();

                    if (mObjInsertUpdateBusinessOperation != null)
                    {
                        mObjInsertUpdateBusinessOperation.Active = !mObjInsertUpdateBusinessOperation.Active;
                        mObjInsertUpdateBusinessOperation.ModifiedBy = pObjBusinessOperation.ModifiedBy;
                        mObjInsertUpdateBusinessOperation.ModifiedDate = pObjBusinessOperation.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Business Operation Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetBusinessOperationList(pObjBusinessOperation.BusinessOperationName, 0,pObjBusinessOperation.BusinessTypeID, pObjBusinessOperation.BusinessOperationIds, pObjBusinessOperation.intStatus, pObjBusinessOperation.IncludeBusinessOperationIds, pObjBusinessOperation.ExcludeBusinessOperationIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Business Operation Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
