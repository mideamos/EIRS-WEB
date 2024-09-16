using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class TaxPayerTypeRepository : ITaxPayerTypeRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateTaxPayerType(TaxPayer_Types pObjTaxPayerType)
        {
            using (_db = new EIRSEntities())
            {
                TaxPayer_Types mObjInsertUpdateTaxPayerType; //Tax Payer Type Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from tptype in _db.TaxPayer_Types
                                       where tptype.TaxPayerTypeName == pObjTaxPayerType.TaxPayerTypeName && tptype.TaxPayerTypeID != pObjTaxPayerType.TaxPayerTypeID
                                       select tptype);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Tax Payer Type already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Tax Payer Type
                if (pObjTaxPayerType.TaxPayerTypeID != 0)
                {
                    mObjInsertUpdateTaxPayerType = (from tptype in _db.TaxPayer_Types
                                                 where tptype.TaxPayerTypeID == pObjTaxPayerType.TaxPayerTypeID
                                                 select tptype).FirstOrDefault();

                    if (mObjInsertUpdateTaxPayerType != null)
                    {
                        mObjInsertUpdateTaxPayerType.ModifiedBy = pObjTaxPayerType.ModifiedBy;
                        mObjInsertUpdateTaxPayerType.ModifiedDate = pObjTaxPayerType.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateTaxPayerType = new TaxPayer_Types();
                        mObjInsertUpdateTaxPayerType.CreatedBy = pObjTaxPayerType.CreatedBy;
                        mObjInsertUpdateTaxPayerType.CreatedDate = pObjTaxPayerType.CreatedDate;
                    }
                }
                else // Else Insert Tax Payer Type
                {
                    mObjInsertUpdateTaxPayerType = new TaxPayer_Types();
                    mObjInsertUpdateTaxPayerType.CreatedBy = pObjTaxPayerType.CreatedBy;
                    mObjInsertUpdateTaxPayerType.CreatedDate = pObjTaxPayerType.CreatedDate;
                }

                mObjInsertUpdateTaxPayerType.TaxPayerTypeName = pObjTaxPayerType.TaxPayerTypeName;
                mObjInsertUpdateTaxPayerType.Active = pObjTaxPayerType.Active;

                if (pObjTaxPayerType.TaxPayerTypeID == 0)
                {
                    _db.TaxPayer_Types.Add(mObjInsertUpdateTaxPayerType);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjTaxPayerType.TaxPayerTypeID == 0)
                        mObjFuncResponse.Message = "Tax Payer Type Added Successfully";
                    else
                        mObjFuncResponse.Message = "Tax Payer Type Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjTaxPayerType.TaxPayerTypeID == 0)
                        mObjFuncResponse.Message = "Tax Payer Type Addition Failed";
                    else
                        mObjFuncResponse.Message = "Tax Payer Type Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetTaxPayerTypeList_Result> REP_GetTaxPayerTypeList(TaxPayer_Types pObjTaxPayerType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerTypeList(pObjTaxPayerType.TaxPayerTypeName, pObjTaxPayerType.TaxPayerTypeID, pObjTaxPayerType.TaxPayerTypeIds, pObjTaxPayerType.intStatus, pObjTaxPayerType.IncludeTaxPayerTypeIds, pObjTaxPayerType.ExcludeTaxPayerTypeIds).ToList();
            }
        }

        public usp_GetTaxPayerTypeList_Result REP_GetTaxPayerTypeDetails(TaxPayer_Types pObjTaxPayerType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerTypeList(pObjTaxPayerType.TaxPayerTypeName, pObjTaxPayerType.TaxPayerTypeID, pObjTaxPayerType.TaxPayerTypeIds, pObjTaxPayerType.intStatus, pObjTaxPayerType.IncludeTaxPayerTypeIds, pObjTaxPayerType.ExcludeTaxPayerTypeIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetTaxPayerTypeDropDownList(TaxPayer_Types pObjTaxPayerType)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = from pro in _db.TaxPayer_Types
                              select new DropDownListResult() { id = pro.TaxPayerTypeID, text = pro.TaxPayerTypeName };

                return vResult.ToList();
                //var vResult = (from tptype in _db.usp_GetTaxPayerTypeList(pObjTaxPayerType.TaxPayerTypeName, pObjTaxPayerType.TaxPayerTypeID, pObjTaxPayerType.TaxPayerTypeIds, pObjTaxPayerType.intStatus, pObjTaxPayerType.IncludeTaxPayerTypeIds, pObjTaxPayerType.ExcludeTaxPayerTypeIds)
                //               select new DropDownListResult()
                //               {
                //                   id = tptype.TaxPayerTypeID.GetValueOrDefault(),
                //                   text = tptype.TaxPayerTypeName
                //               }).ToList();

                //return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(TaxPayer_Types pObjTaxPayerType)
        {
            using (_db = new EIRSEntities())
            {
                TaxPayer_Types mObjInsertUpdateTaxPayerType; //Tax Payer Type Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load TaxPayerType
                if (pObjTaxPayerType.TaxPayerTypeID != 0)
                {
                    mObjInsertUpdateTaxPayerType = (from tptype in _db.TaxPayer_Types
                                                 where tptype.TaxPayerTypeID == pObjTaxPayerType.TaxPayerTypeID
                                                 select tptype).FirstOrDefault();

                    if (mObjInsertUpdateTaxPayerType != null)
                    {
                        mObjInsertUpdateTaxPayerType.Active = !mObjInsertUpdateTaxPayerType.Active;
                        mObjInsertUpdateTaxPayerType.ModifiedBy = pObjTaxPayerType.ModifiedBy;
                        mObjInsertUpdateTaxPayerType.ModifiedDate = pObjTaxPayerType.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Tax Payer Type Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetTaxPayerTypeList(pObjTaxPayerType.TaxPayerTypeName, 0, pObjTaxPayerType.TaxPayerTypeIds, pObjTaxPayerType.intStatus, pObjTaxPayerType.IncludeTaxPayerTypeIds, pObjTaxPayerType.ExcludeTaxPayerTypeIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Tax Payer Type Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
