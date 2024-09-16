using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class SettlementMethodRepository : ISettlementMethodRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateSettlementMethod(Settlement_Method pObjSettlementMethod)
        {
            using (_db = new EIRSEntities())
            {
                Settlement_Method mObjInsertUpdateSettlementMethod; //Settlement Method Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from smthd in _db.Settlement_Method
                                       where smthd.SettlementMethodName == pObjSettlementMethod.SettlementMethodName && smthd.SettlementMethodID != pObjSettlementMethod.SettlementMethodID
                                       select smthd);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Settlement Method already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Settlement Method
                if (pObjSettlementMethod.SettlementMethodID != 0)
                {
                    mObjInsertUpdateSettlementMethod = (from smthd in _db.Settlement_Method
                                                 where smthd.SettlementMethodID == pObjSettlementMethod.SettlementMethodID
                                                 select smthd).FirstOrDefault();

                    if (mObjInsertUpdateSettlementMethod != null)
                    {
                        mObjInsertUpdateSettlementMethod.ModifiedBy = pObjSettlementMethod.ModifiedBy;
                        mObjInsertUpdateSettlementMethod.ModifiedDate = pObjSettlementMethod.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateSettlementMethod = new Settlement_Method();
                        mObjInsertUpdateSettlementMethod.CreatedBy = pObjSettlementMethod.CreatedBy;
                        mObjInsertUpdateSettlementMethod.CreatedDate = pObjSettlementMethod.CreatedDate;
                    }
                }
                else // Else Insert Settlement Method
                {
                    mObjInsertUpdateSettlementMethod = new Settlement_Method();
                    mObjInsertUpdateSettlementMethod.CreatedBy = pObjSettlementMethod.CreatedBy;
                    mObjInsertUpdateSettlementMethod.CreatedDate = pObjSettlementMethod.CreatedDate;
                }

                mObjInsertUpdateSettlementMethod.SettlementMethodName = pObjSettlementMethod.SettlementMethodName;
                mObjInsertUpdateSettlementMethod.Active = pObjSettlementMethod.Active;

                if (pObjSettlementMethod.SettlementMethodID == 0)
                {
                    _db.Settlement_Method.Add(mObjInsertUpdateSettlementMethod);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjSettlementMethod.SettlementMethodID == 0)
                        mObjFuncResponse.Message = "Settlement Method Added Successfully";
                    else
                        mObjFuncResponse.Message = "Settlement Method Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjSettlementMethod.SettlementMethodID == 0)
                        mObjFuncResponse.Message = "Settlement Method Addition Failed";
                    else
                        mObjFuncResponse.Message = "Settlement Method Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetSettlementMethodList_Result> REP_GetSettlementMethodList(Settlement_Method pObjSettlementMethod)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetSettlementMethodList(pObjSettlementMethod.SettlementMethodName, pObjSettlementMethod.SettlementMethodID, pObjSettlementMethod.SettlementMethodIds, pObjSettlementMethod.intStatus, pObjSettlementMethod.IncludeSettlementMethodIds, pObjSettlementMethod.ExcludeSettlementMethodIds).ToList();
            }
        }

        public usp_GetSettlementMethodList_Result REP_GetSettlementMethodDetails(Settlement_Method pObjSettlementMethod)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetSettlementMethodList(pObjSettlementMethod.SettlementMethodName, pObjSettlementMethod.SettlementMethodID, pObjSettlementMethod.SettlementMethodIds, pObjSettlementMethod.intStatus, pObjSettlementMethod.IncludeSettlementMethodIds, pObjSettlementMethod.ExcludeSettlementMethodIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetSettlementMethodDropDownList(Settlement_Method pObjSettlementMethod)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from smthd in _db.usp_GetSettlementMethodList(pObjSettlementMethod.SettlementMethodName, pObjSettlementMethod.SettlementMethodID, pObjSettlementMethod.SettlementMethodIds, pObjSettlementMethod.intStatus, pObjSettlementMethod.IncludeSettlementMethodIds, pObjSettlementMethod.ExcludeSettlementMethodIds)
                               select new DropDownListResult()
                               {
                                   id = smthd.SettlementMethodID.GetValueOrDefault(),
                                   text = smthd.SettlementMethodName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Settlement_Method pObjSettlementMethod)
        {
            using (_db = new EIRSEntities())
            {
                Settlement_Method mObjInsertUpdateSettlementMethod; //Settlement Method Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load SettlementMethod
                if (pObjSettlementMethod.SettlementMethodID != 0)
                {
                    mObjInsertUpdateSettlementMethod = (from smthd in _db.Settlement_Method
                                                 where smthd.SettlementMethodID == pObjSettlementMethod.SettlementMethodID
                                                 select smthd).FirstOrDefault();

                    if (mObjInsertUpdateSettlementMethod != null)
                    {
                        mObjInsertUpdateSettlementMethod.Active = !mObjInsertUpdateSettlementMethod.Active;
                        mObjInsertUpdateSettlementMethod.ModifiedBy = pObjSettlementMethod.ModifiedBy;
                        mObjInsertUpdateSettlementMethod.ModifiedDate = pObjSettlementMethod.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Settlement Method Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetSettlementMethodList(pObjSettlementMethod.SettlementMethodName, 0, pObjSettlementMethod.SettlementMethodIds, pObjSettlementMethod.intStatus, pObjSettlementMethod.IncludeSettlementMethodIds, pObjSettlementMethod.ExcludeSettlementMethodIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Settlement Method Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
