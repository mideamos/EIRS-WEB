using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class BusinessTypeRepository : IBusinessTypeRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateBusinessType(Business_Types pObjBusinessType)
        {
            using (_db = new EIRSEntities())
            {
                Business_Types mObjInsertUpdateBusinessType; //Business Type Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from btype in _db.Business_Types
                                       where btype.BusinessTypeName == pObjBusinessType.BusinessTypeName && btype.BusinessTypeID != pObjBusinessType.BusinessTypeID
                                       select btype);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Business Type already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Business Type
                if (pObjBusinessType.BusinessTypeID != 0)
                {
                    mObjInsertUpdateBusinessType = (from btype in _db.Business_Types
                                                 where btype.BusinessTypeID == pObjBusinessType.BusinessTypeID
                                                 select btype).FirstOrDefault();

                    if (mObjInsertUpdateBusinessType != null)
                    {
                        mObjInsertUpdateBusinessType.ModifiedBy = pObjBusinessType.ModifiedBy;
                        mObjInsertUpdateBusinessType.ModifiedDate = pObjBusinessType.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateBusinessType = new Business_Types();
                        mObjInsertUpdateBusinessType.CreatedBy = pObjBusinessType.CreatedBy;
                        mObjInsertUpdateBusinessType.CreatedDate = pObjBusinessType.CreatedDate;
                    }
                }
                else // Else Insert Business Type
                {
                    mObjInsertUpdateBusinessType = new Business_Types();
                    mObjInsertUpdateBusinessType.CreatedBy = pObjBusinessType.CreatedBy;
                    mObjInsertUpdateBusinessType.CreatedDate = pObjBusinessType.CreatedDate;
                }

                mObjInsertUpdateBusinessType.BusinessTypeName = pObjBusinessType.BusinessTypeName;
                mObjInsertUpdateBusinessType.Active = pObjBusinessType.Active;

                if (pObjBusinessType.BusinessTypeID == 0)
                {
                    _db.Business_Types.Add(mObjInsertUpdateBusinessType);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjBusinessType.BusinessTypeID == 0)
                        mObjFuncResponse.Message = "Business Type Added Successfully";
                    else
                        mObjFuncResponse.Message = "Business Type Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjBusinessType.BusinessTypeID == 0)
                        mObjFuncResponse.Message = "Business Type Addition Failed";
                    else
                        mObjFuncResponse.Message = "Business Type Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetBusinessTypeList_Result> REP_GetBusinessTypeList(Business_Types pObjBusinessType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBusinessTypeList(pObjBusinessType.BusinessTypeName, pObjBusinessType.BusinessTypeID, pObjBusinessType.BusinessTypeIds, pObjBusinessType.intStatus, pObjBusinessType.IncludeBusinessTypeIds, pObjBusinessType.ExcludeBusinessTypeIds).ToList();
            }
        }

        public usp_GetBusinessTypeList_Result REP_GetBusinessTypeDetails(Business_Types pObjBusinessType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetBusinessTypeList(pObjBusinessType.BusinessTypeName, pObjBusinessType.BusinessTypeID, pObjBusinessType.BusinessTypeIds, pObjBusinessType.intStatus, pObjBusinessType.IncludeBusinessTypeIds, pObjBusinessType.ExcludeBusinessTypeIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetBusinessTypeDropDownList(Business_Types pObjBusinessType)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = from pro in _db.Business_Types
                              select new DropDownListResult() { id = pro.BusinessTypeID, text = pro.BusinessTypeName };

                return vResult.ToList();
                //var vResult = (from btype in _db.usp_GetBusinessTypeList(pObjBusinessType.BusinessTypeName, pObjBusinessType.BusinessTypeID, pObjBusinessType.BusinessTypeIds, pObjBusinessType.intStatus, pObjBusinessType.IncludeBusinessTypeIds, pObjBusinessType.ExcludeBusinessTypeIds)
                //               select new DropDownListResult()
                //               {
                //                   id = btype.BusinessTypeID.GetValueOrDefault(),
                //                   text = btype.BusinessTypeName
                //               }).ToList();

                //return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Business_Types pObjBusinessType)
        {
            using (_db = new EIRSEntities())
            {
                Business_Types mObjInsertUpdateBusinessType; //Business Type Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load BusinessType
                if (pObjBusinessType.BusinessTypeID != 0)
                {
                    mObjInsertUpdateBusinessType = (from btype in _db.Business_Types
                                                 where btype.BusinessTypeID == pObjBusinessType.BusinessTypeID
                                                 select btype).FirstOrDefault();

                    if (mObjInsertUpdateBusinessType != null)
                    {
                        mObjInsertUpdateBusinessType.Active = !mObjInsertUpdateBusinessType.Active;
                        mObjInsertUpdateBusinessType.ModifiedBy = pObjBusinessType.ModifiedBy;
                        mObjInsertUpdateBusinessType.ModifiedDate = pObjBusinessType.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Business Type Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetBusinessTypeList(pObjBusinessType.BusinessTypeName, 0, pObjBusinessType.BusinessTypeIds, pObjBusinessType.intStatus, pObjBusinessType.IncludeBusinessTypeIds, pObjBusinessType.ExcludeBusinessTypeIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Business Type Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
