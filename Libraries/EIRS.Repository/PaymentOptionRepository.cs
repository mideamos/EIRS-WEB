using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class PaymentOptionRepository : IPaymentOptionRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdatePaymentOption(Payment_Options pObjPaymentOption)
        {
            using (_db = new EIRSEntities())
            {
                Payment_Options mObjInsertUpdatePaymentOption; //Payment Option Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from popt in _db.Payment_Options
                                       where popt.PaymentOptionName == pObjPaymentOption.PaymentOptionName && popt.PaymentOptionID != pObjPaymentOption.PaymentOptionID
                                       select popt);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Payment Option already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Payment Option
                if (pObjPaymentOption.PaymentOptionID != 0)
                {
                    mObjInsertUpdatePaymentOption = (from popt in _db.Payment_Options
                                                 where popt.PaymentOptionID == pObjPaymentOption.PaymentOptionID
                                                 select popt).FirstOrDefault();

                    if (mObjInsertUpdatePaymentOption != null)
                    {
                        mObjInsertUpdatePaymentOption.ModifiedBy = pObjPaymentOption.ModifiedBy;
                        mObjInsertUpdatePaymentOption.ModifiedDate = pObjPaymentOption.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdatePaymentOption = new Payment_Options();
                        mObjInsertUpdatePaymentOption.CreatedBy = pObjPaymentOption.CreatedBy;
                        mObjInsertUpdatePaymentOption.CreatedDate = pObjPaymentOption.CreatedDate;
                    }
                }
                else // Else Insert Payment Option
                {
                    mObjInsertUpdatePaymentOption = new Payment_Options();
                    mObjInsertUpdatePaymentOption.CreatedBy = pObjPaymentOption.CreatedBy;
                    mObjInsertUpdatePaymentOption.CreatedDate = pObjPaymentOption.CreatedDate;
                }

                mObjInsertUpdatePaymentOption.PaymentOptionName = pObjPaymentOption.PaymentOptionName;
                mObjInsertUpdatePaymentOption.Active = pObjPaymentOption.Active;

                if (pObjPaymentOption.PaymentOptionID == 0)
                {
                    _db.Payment_Options.Add(mObjInsertUpdatePaymentOption);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjPaymentOption.PaymentOptionID == 0)
                        mObjFuncResponse.Message = "Payment Option Added Successfully";
                    else
                        mObjFuncResponse.Message = "Payment Option Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjPaymentOption.PaymentOptionID == 0)
                        mObjFuncResponse.Message = "Payment Option Addition Failed";
                    else
                        mObjFuncResponse.Message = "Payment Option Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetPaymentOptionList_Result> REP_GetPaymentOptionList(Payment_Options pObjPaymentOption)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetPaymentOptionList(pObjPaymentOption.PaymentOptionName, pObjPaymentOption.PaymentOptionID, pObjPaymentOption.PaymentOptionIds, pObjPaymentOption.intStatus, pObjPaymentOption.IncludePaymentOptionIds, pObjPaymentOption.ExcludePaymentOptionIds).ToList();
            }
        }

        public usp_GetPaymentOptionList_Result REP_GetPaymentOptionDetails(Payment_Options pObjPaymentOption)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetPaymentOptionList(pObjPaymentOption.PaymentOptionName, pObjPaymentOption.PaymentOptionID, pObjPaymentOption.PaymentOptionIds, pObjPaymentOption.intStatus, pObjPaymentOption.IncludePaymentOptionIds, pObjPaymentOption.ExcludePaymentOptionIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetPaymentOptionDropDownList(Payment_Options pObjPaymentOption)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from popt in _db.usp_GetPaymentOptionList(pObjPaymentOption.PaymentOptionName, pObjPaymentOption.PaymentOptionID, pObjPaymentOption.PaymentOptionIds, pObjPaymentOption.intStatus, pObjPaymentOption.IncludePaymentOptionIds, pObjPaymentOption.ExcludePaymentOptionIds)
                               select new DropDownListResult()
                               {
                                   id = popt.PaymentOptionID.GetValueOrDefault(),
                                   text = popt.PaymentOptionName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Payment_Options pObjPaymentOption)
        {
            using (_db = new EIRSEntities())
            {
                Payment_Options mObjInsertUpdatePaymentOption; //Payment Option Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load PaymentOption
                if (pObjPaymentOption.PaymentOptionID != 0)
                {
                    mObjInsertUpdatePaymentOption = (from popt in _db.Payment_Options
                                                 where popt.PaymentOptionID == pObjPaymentOption.PaymentOptionID
                                                 select popt).FirstOrDefault();

                    if (mObjInsertUpdatePaymentOption != null)
                    {
                        mObjInsertUpdatePaymentOption.Active = !mObjInsertUpdatePaymentOption.Active;
                        mObjInsertUpdatePaymentOption.ModifiedBy = pObjPaymentOption.ModifiedBy;
                        mObjInsertUpdatePaymentOption.ModifiedDate = pObjPaymentOption.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Payment Option Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetPaymentOptionList(pObjPaymentOption.PaymentOptionName, 0, pObjPaymentOption.PaymentOptionIds, pObjPaymentOption.intStatus, pObjPaymentOption.IncludePaymentOptionIds, pObjPaymentOption.ExcludePaymentOptionIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Payment Option Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
