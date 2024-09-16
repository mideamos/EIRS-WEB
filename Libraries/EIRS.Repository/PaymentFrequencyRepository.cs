using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class PaymentFrequencyRepository : IPaymentFrequencyRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdatePaymentFrequency(Payment_Frequency pObjPaymentFrequency)
        {
            using (_db = new EIRSEntities())
            {
                Payment_Frequency mObjInsertUpdatePaymentFrequency; //Payment Frequency Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from pfreq in _db.Payment_Frequency
                                       where pfreq.PaymentFrequencyName == pObjPaymentFrequency.PaymentFrequencyName && pfreq.PaymentFrequencyID != pObjPaymentFrequency.PaymentFrequencyID
                                       select pfreq);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Payment Frequency already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Payment Frequency
                if (pObjPaymentFrequency.PaymentFrequencyID != 0)
                {
                    mObjInsertUpdatePaymentFrequency = (from pfreq in _db.Payment_Frequency
                                                 where pfreq.PaymentFrequencyID == pObjPaymentFrequency.PaymentFrequencyID
                                                 select pfreq).FirstOrDefault();

                    if (mObjInsertUpdatePaymentFrequency != null)
                    {
                        mObjInsertUpdatePaymentFrequency.ModifiedBy = pObjPaymentFrequency.ModifiedBy;
                        mObjInsertUpdatePaymentFrequency.ModifiedDate = pObjPaymentFrequency.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdatePaymentFrequency = new Payment_Frequency();
                        mObjInsertUpdatePaymentFrequency.CreatedBy = pObjPaymentFrequency.CreatedBy;
                        mObjInsertUpdatePaymentFrequency.CreatedDate = pObjPaymentFrequency.CreatedDate;
                    }
                }
                else // Else Insert Payment Frequency
                {
                    mObjInsertUpdatePaymentFrequency = new Payment_Frequency();
                    mObjInsertUpdatePaymentFrequency.CreatedBy = pObjPaymentFrequency.CreatedBy;
                    mObjInsertUpdatePaymentFrequency.CreatedDate = pObjPaymentFrequency.CreatedDate;
                }

                mObjInsertUpdatePaymentFrequency.PaymentFrequencyName = pObjPaymentFrequency.PaymentFrequencyName;
                mObjInsertUpdatePaymentFrequency.Active = pObjPaymentFrequency.Active;

                if (pObjPaymentFrequency.PaymentFrequencyID == 0)
                {
                    _db.Payment_Frequency.Add(mObjInsertUpdatePaymentFrequency);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjPaymentFrequency.PaymentFrequencyID == 0)
                        mObjFuncResponse.Message = "Payment Frequency Added Successfully";
                    else
                        mObjFuncResponse.Message = "Payment Frequency Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjPaymentFrequency.PaymentFrequencyID == 0)
                        mObjFuncResponse.Message = "Payment Frequency Addition Failed";
                    else
                        mObjFuncResponse.Message = "Payment Frequency Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetPaymentFrequencyList_Result> REP_GetPaymentFrequencyList(Payment_Frequency pObjPaymentFrequency)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetPaymentFrequencyList(pObjPaymentFrequency.PaymentFrequencyName, pObjPaymentFrequency.PaymentFrequencyID, pObjPaymentFrequency.PaymentFrequencyIds, pObjPaymentFrequency.intStatus, pObjPaymentFrequency.IncludePaymentFrequencyIds, pObjPaymentFrequency.ExcludePaymentFrequencyIds).ToList();
            }
        }

        public usp_GetPaymentFrequencyList_Result REP_GetPaymentFrequencyDetails(Payment_Frequency pObjPaymentFrequency)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetPaymentFrequencyList(pObjPaymentFrequency.PaymentFrequencyName, pObjPaymentFrequency.PaymentFrequencyID, pObjPaymentFrequency.PaymentFrequencyIds, pObjPaymentFrequency.intStatus, pObjPaymentFrequency.IncludePaymentFrequencyIds, pObjPaymentFrequency.ExcludePaymentFrequencyIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetPaymentFrequencyDropDownList(Payment_Frequency pObjPaymentFrequency)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from pfreq in _db.usp_GetPaymentFrequencyList(pObjPaymentFrequency.PaymentFrequencyName, pObjPaymentFrequency.PaymentFrequencyID, pObjPaymentFrequency.PaymentFrequencyIds, pObjPaymentFrequency.intStatus, pObjPaymentFrequency.IncludePaymentFrequencyIds, pObjPaymentFrequency.ExcludePaymentFrequencyIds)
                               select new DropDownListResult()
                               {
                                   id = pfreq.PaymentFrequencyID.GetValueOrDefault(),
                                   text = pfreq.PaymentFrequencyName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(Payment_Frequency pObjPaymentFrequency)
        {
            using (_db = new EIRSEntities())
            {
                Payment_Frequency mObjInsertUpdatePaymentFrequency; //Payment Frequency Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load PaymentFrequency
                if (pObjPaymentFrequency.PaymentFrequencyID != 0)
                {
                    mObjInsertUpdatePaymentFrequency = (from pfreq in _db.Payment_Frequency
                                                 where pfreq.PaymentFrequencyID == pObjPaymentFrequency.PaymentFrequencyID
                                                 select pfreq).FirstOrDefault();

                    if (mObjInsertUpdatePaymentFrequency != null)
                    {
                        mObjInsertUpdatePaymentFrequency.Active = !mObjInsertUpdatePaymentFrequency.Active;
                        mObjInsertUpdatePaymentFrequency.ModifiedBy = pObjPaymentFrequency.ModifiedBy;
                        mObjInsertUpdatePaymentFrequency.ModifiedDate = pObjPaymentFrequency.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Payment Frequency Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetPaymentFrequencyList(pObjPaymentFrequency.PaymentFrequencyName, 0, pObjPaymentFrequency.PaymentFrequencyIds, pObjPaymentFrequency.intStatus, pObjPaymentFrequency.IncludePaymentFrequencyIds, pObjPaymentFrequency.ExcludePaymentFrequencyIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Payment Frequency Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
