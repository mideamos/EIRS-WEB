using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class EM_BankRepository : IEM_BankRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateBank(EM_Bank pObjBank)
        {
            using (_db = new EIRSEntities())
            {
                EM_Bank mObjInsertUpdateBank; //Bank Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplibnke
                var vDuplibnkeCheck = (from bnk in _db.EM_Bank
                                       where bnk.BankName == pObjBank.BankName && bnk.BankID != pObjBank.BankID
                                       select bnk);

                if (vDuplibnkeCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Bank already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Bank
                if (pObjBank.BankID != 0)
                {
                    mObjInsertUpdateBank = (from bnk in _db.EM_Bank
                                            where bnk.BankID == pObjBank.BankID
                                            select bnk).FirstOrDefault();

                    if (mObjInsertUpdateBank != null)
                    {
                        mObjInsertUpdateBank.ModifiedBy = pObjBank.ModifiedBy;
                        mObjInsertUpdateBank.ModifiedDate = pObjBank.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateBank = new EM_Bank();
                        mObjInsertUpdateBank.CreatedBy = pObjBank.CreatedBy;
                        mObjInsertUpdateBank.CreatedDate = pObjBank.CreatedDate;
                    }
                }
                else // Else Insert Bank
                {
                    mObjInsertUpdateBank = new EM_Bank();
                    mObjInsertUpdateBank.CreatedBy = pObjBank.CreatedBy;
                    mObjInsertUpdateBank.CreatedDate = pObjBank.CreatedDate;
                }

                mObjInsertUpdateBank.BankName = pObjBank.BankName;
                mObjInsertUpdateBank.BankAccountNumber = pObjBank.BankAccountNumber;
                mObjInsertUpdateBank.BankDescription = pObjBank.BankDescription;
                mObjInsertUpdateBank.Active = pObjBank.Active;

                if (pObjBank.BankID == 0)
                {
                    _db.EM_Bank.Add(mObjInsertUpdateBank);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjBank.BankID == 0)
                        mObjFuncResponse.Message = "Bank Added Successfully";
                    else
                        mObjFuncResponse.Message = "Bank Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjBank.BankID == 0)
                        mObjFuncResponse.Message = "Bank Addition Failed";
                    else
                        mObjFuncResponse.Message = "Bank Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_EM_GetBankList_Result> REP_GetBankList(EM_Bank pObjBank)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_EM_GetBankList(pObjBank.BankName, pObjBank.BankID, pObjBank.BankIds, pObjBank.intStatus, pObjBank.IncludeBankIds, pObjBank.ExcludeBankIds).ToList();
            }
        }

        public usp_EM_GetBankList_Result REP_GetBankDetails(EM_Bank pObjBank)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_EM_GetBankList(pObjBank.BankName, pObjBank.BankID, pObjBank.BankIds, pObjBank.intStatus, pObjBank.IncludeBankIds, pObjBank.ExcludeBankIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetBankDropDownList(EM_Bank pObjBank)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from bnk in _db.usp_EM_GetBankList(pObjBank.BankName, pObjBank.BankID, pObjBank.BankIds, pObjBank.intStatus, pObjBank.IncludeBankIds, pObjBank.ExcludeBankIds)
                               select new DropDownListResult()
                               {
                                   id = bnk.BankID.GetValueOrDefault(),
                                   text = bnk.BankName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(EM_Bank pObjBank)
        {
            using (_db = new EIRSEntities())
            {
                EM_Bank mObjInsertUpdateBank; //Bank Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Bank
                if (pObjBank.BankID != 0)
                {
                    mObjInsertUpdateBank = (from bnk in _db.EM_Bank
                                            where bnk.BankID == pObjBank.BankID
                                            select bnk).FirstOrDefault();

                    if (mObjInsertUpdateBank != null)
                    {
                        mObjInsertUpdateBank.Active = !mObjInsertUpdateBank.Active;
                        mObjInsertUpdateBank.ModifiedBy = pObjBank.ModifiedBy;
                        mObjInsertUpdateBank.ModifiedDate = pObjBank.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Bank Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_EM_GetBankList(pObjBank.BankName, 0, pObjBank.BankIds, pObjBank.intStatus, pObjBank.IncludeBankIds, pObjBank.ExcludeBankIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Bank Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }
    }
}
