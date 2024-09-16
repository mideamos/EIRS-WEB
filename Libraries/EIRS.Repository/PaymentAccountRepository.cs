using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EIRS.Repository
{
    public class PaymentAccountRepository : IPaymentAccountRepository
    {
        EIRSEntities _db;

        public FuncResponse<Payment_Account> REP_InsertUpdatePaymentAccount(Payment_Account pObjPaymentAccount)
        {
            using (_db = new EIRSEntities())
            {
                Payment_Account mObjInsertUpdatePaymentAccount; //Payment on Account Insert Object
                FuncResponse<Payment_Account> mObjFuncResponse = new FuncResponse<Payment_Account>(); //Return Object

                if (pObjPaymentAccount.ValidateDuplicateCheck)
                {

                    //Check if Duplicate
                    var vDuplicateCheck = (from poa in _db.Payment_Account
                                           where poa.TransactionRefNo == pObjPaymentAccount.TransactionRefNo //&& poa.PaymentAccountID == pObjPaymentAccount.PaymentAccountID
                                           select poa);

                    if (vDuplicateCheck.Count() > 0)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = "Transaction Ref No already exists";
                        return mObjFuncResponse;
                    }
                }

                //If Update Load Payment_Account Item
                if (pObjPaymentAccount.PaymentAccountID != 0)
                {
                    mObjInsertUpdatePaymentAccount = (from poa in _db.Payment_Account
                                                      where poa.PaymentAccountID == pObjPaymentAccount.PaymentAccountID
                                                      select poa).FirstOrDefault();

                    if (mObjInsertUpdatePaymentAccount != null)
                    {
                        mObjInsertUpdatePaymentAccount.ModifiedBy = pObjPaymentAccount.ModifiedBy;
                        mObjInsertUpdatePaymentAccount.ModifiedDate = pObjPaymentAccount.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdatePaymentAccount = new Payment_Account
                        {
                            CreatedBy = pObjPaymentAccount.CreatedBy,
                            CreatedDate = pObjPaymentAccount.CreatedDate
                        };
                    }
                }
                else // Else Insert Payment_Account Item
                {
                    mObjInsertUpdatePaymentAccount = new Payment_Account
                    {
                        CreatedBy = pObjPaymentAccount.CreatedBy,
                        CreatedDate = pObjPaymentAccount.CreatedDate
                    };
                }

                mObjInsertUpdatePaymentAccount.TaxPayerTypeID = pObjPaymentAccount.TaxPayerTypeID;
                mObjInsertUpdatePaymentAccount.TaxPayerID = pObjPaymentAccount.TaxPayerID;
                mObjInsertUpdatePaymentAccount.RevenueStreamID = pObjPaymentAccount.RevenueStreamID;
                mObjInsertUpdatePaymentAccount.RevenueSubStreamID = pObjPaymentAccount.RevenueSubStreamID;
                mObjInsertUpdatePaymentAccount.Amount = pObjPaymentAccount.Amount;
                mObjInsertUpdatePaymentAccount.SettlementMethodID = pObjPaymentAccount.SettlementMethodID;
                mObjInsertUpdatePaymentAccount.SettlementStatusID = pObjPaymentAccount.SettlementStatusID;
                mObjInsertUpdatePaymentAccount.PaymentDate = pObjPaymentAccount.PaymentDate;
                mObjInsertUpdatePaymentAccount.TransactionRefNo = pObjPaymentAccount.TransactionRefNo;
                mObjInsertUpdatePaymentAccount.AgencyID = pObjPaymentAccount.AgencyID;
                mObjInsertUpdatePaymentAccount.Notes = pObjPaymentAccount.Notes;
                mObjInsertUpdatePaymentAccount.Active = pObjPaymentAccount.Active;
                mObjInsertUpdatePaymentAccount.isManualEntry = pObjPaymentAccount.isManualEntry;


                if (pObjPaymentAccount.PaymentAccountID == 0)
                {
                    _db.Payment_Account.Add(mObjInsertUpdatePaymentAccount);
                }

                try
                {
                    _db.SaveChanges();

                    var context = ((IObjectContextAdapter)_db).ObjectContext;
                    var refreshableObjects = _db.ChangeTracker.Entries().Select(c => c.Entity).ToList();
                    context.Refresh(RefreshMode.StoreWins, refreshableObjects);

                    mObjFuncResponse.Success = true;
                    if (pObjPaymentAccount.PaymentAccountID == 0)
                    {
                        mObjFuncResponse.Message = "Payment on Account Added Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Payment on Account Updated Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjInsertUpdatePaymentAccount;

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjPaymentAccount.PaymentAccountID == 0)
                    {
                        mObjFuncResponse.Message = "Payment on Account Addition Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Payment on Account Updation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_UpdatePaymentAccountFromRDM(Payment_Account pObjPaymentAccount)
        {
            using (_db = new EIRSEntities())
            {
                Payment_Account mObjUpdatePaymentAccount; //Payment on Account Insert Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                mObjUpdatePaymentAccount = (from poa in _db.Payment_Account
                                            where poa.PaymentAccountID == pObjPaymentAccount.PaymentAccountID
                                            select poa).FirstOrDefault();

                if (mObjUpdatePaymentAccount != null)
                {
                    mObjUpdatePaymentAccount.ModifiedBy = pObjPaymentAccount.ModifiedBy;
                    mObjUpdatePaymentAccount.ModifiedDate = pObjPaymentAccount.ModifiedDate;
                }
                else
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Record not found";
                    return mObjFuncResponse;
                }


                mObjUpdatePaymentAccount.RevenueStreamID = pObjPaymentAccount.RevenueStreamID;
                mObjUpdatePaymentAccount.RevenueSubStreamID = pObjPaymentAccount.RevenueSubStreamID;
                mObjUpdatePaymentAccount.AgencyID = pObjPaymentAccount.AgencyID;

                try
                {
                    _db.SaveChanges();

                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.Message = "Payment on Account Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;
                    mObjFuncResponse.Message = "Payment on Account Updation Failed";

                }

                return mObjFuncResponse;
            }
        }

        public IList<vw_PaymentAccount> REP_PaymentAccountList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.vw_PaymentAccount.ToList();
            }
        }

        public IList<usp_GetPaymentAccountList_Result> REP_GetPaymentAccountList(Payment_Account pObjPaymentAccount)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetPaymentAccountList(pObjPaymentAccount.PaymentAccountID, pObjPaymentAccount.TaxPayerTypeID, pObjPaymentAccount.TaxPayerID, pObjPaymentAccount.RevenueStreamID).ToList();
            }
        }

        public usp_GetPaymentAccountList_Result REP_GetPaymentAccountDetails(Payment_Account pObjPaymentAccount)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetPaymentAccountList(pObjPaymentAccount.PaymentAccountID, pObjPaymentAccount.TaxPayerTypeID, pObjPaymentAccount.TaxPayerID, pObjPaymentAccount.RevenueStreamID).FirstOrDefault();
            }
        }

        public FuncResponse REP_InsertPaymentOperation(MAP_PaymentAccount_Operation pObjPaymentAccount)
        {
            using (_db = new EIRSEntities())
            {
                MAP_PaymentAccount_Operation mObjInsertUpdatePaymentAccount;
                FuncResponse mObjFuncResponse = new FuncResponse();

                mObjInsertUpdatePaymentAccount = new MAP_PaymentAccount_Operation
                {
                    CreatedBy = pObjPaymentAccount.CreatedBy,
                    CreatedDate = pObjPaymentAccount.CreatedDate
                };

                mObjInsertUpdatePaymentAccount.OperationDate = pObjPaymentAccount.OperationDate;
                mObjInsertUpdatePaymentAccount.OperationTypeID = pObjPaymentAccount.OperationTypeID;
                mObjInsertUpdatePaymentAccount.From_TaxPayerTypeID = pObjPaymentAccount.From_TaxPayerTypeID;
                mObjInsertUpdatePaymentAccount.From_TaxPayerID = pObjPaymentAccount.From_TaxPayerID;
                mObjInsertUpdatePaymentAccount.From_SettlementMethodID = pObjPaymentAccount.From_SettlementMethodID;
                mObjInsertUpdatePaymentAccount.To_TaxPayerTypeID = pObjPaymentAccount.To_TaxPayerTypeID;
                mObjInsertUpdatePaymentAccount.To_TaxPayerID = pObjPaymentAccount.To_TaxPayerID;
                mObjInsertUpdatePaymentAccount.To_BillID = pObjPaymentAccount.To_BillID;
                mObjInsertUpdatePaymentAccount.To_BillTypeID = pObjPaymentAccount.To_BillTypeID;
                mObjInsertUpdatePaymentAccount.POAAccountId = pObjPaymentAccount.POAAccountId;
                mObjInsertUpdatePaymentAccount.TransactionRefNo = pObjPaymentAccount.TransactionRefNo;
                mObjInsertUpdatePaymentAccount.Amount = pObjPaymentAccount.Amount;
                mObjInsertUpdatePaymentAccount.Active = pObjPaymentAccount.Active;


                if (pObjPaymentAccount.POAID == 0)
                {
                    _db.MAP_PaymentAccount_Operation.Add(mObjInsertUpdatePaymentAccount);
                }

                try
                {
                    _db.SaveChanges();

                    var context = ((IObjectContextAdapter)_db).ObjectContext;
                    var refreshableObjects = _db.ChangeTracker.Entries().Select(c => c.Entity).ToList();
                    context.Refresh(RefreshMode.StoreWins, refreshableObjects);

                    mObjFuncResponse.Success = true;
                    if (pObjPaymentAccount.POAID == 0)
                    {
                        mObjFuncResponse.Message = "Payment Transfer Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Payment Transfer Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjInsertUpdatePaymentAccount;

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjPaymentAccount.POAID == 0)
                    {
                        mObjFuncResponse.Message = "Payment Transfer Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Payment Transfer Failed";
                    }
                }

                return mObjFuncResponse;
            }

        }

        public decimal REP_GetWalletBalance(int pIntTaxPayerTypeID, int pIntTaxPayerID)
        {
            using (_db = new EIRSEntities())
            {
                decimal Balance = _db.usp_GetWalletBalance(pIntTaxPayerTypeID, pIntTaxPayerID).FirstOrDefault().GetValueOrDefault();
                return Balance;
            }
        }

        public IList<vw_PaymentAccountOperation> REP_GetPaymentAccountOperationList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.vw_PaymentAccountOperation.ToList();
            }
        }

        public IDictionary<string, object> REP_SearchPaymentAccount(Payment_Account pObjPaymentAccount)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>
                {
                    ["PaymentAccountList"] = _db.usp_SearchPaymentAccount(pObjPaymentAccount.WhereCondition, pObjPaymentAccount.OrderBy, pObjPaymentAccount.OrderByDirection, pObjPaymentAccount.PageNumber, pObjPaymentAccount.PageSize, pObjPaymentAccount.MainFilter,
                                                                        pObjPaymentAccount.PaymentRefNo, pObjPaymentAccount.strPaymentDate, pObjPaymentAccount.TaxPayerTypeName, pObjPaymentAccount.TaxPayerRIN, pObjPaymentAccount.TaxPayerName, pObjPaymentAccount.strAmount, pObjPaymentAccount.RevenueStreamName, pObjPaymentAccount.RevenueSubStreamName, pObjPaymentAccount.AgencyName, pObjPaymentAccount.SettlementMethodName, pObjPaymentAccount.SettlementStatusName, pObjPaymentAccount.Notes, pObjPaymentAccount.TransactionRefNo).ToList()
                };

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(PaymentAccountID) FROM Payment_Account").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(PaymentAccountID) ");
                sbFilteredCountQuery.Append(" FROM Payment_Account poa ");
                sbFilteredCountQuery.Append(" INNER JOIN Settlement_Status stat ON poa.SettlementStatusID = stat.SettlementStatusID ");
                sbFilteredCountQuery.Append(" INNER JOIN Settlement_Method mthd ON poa.SettlementMethodID = mthd.SettlementMethodID ");
                sbFilteredCountQuery.Append(" INNER JOIN TaxPayer_Types tptype ON poa.TaxPayerTypeID = tptype.TaxPayerTypeID ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Revenue_Stream rstream ON poa.RevenueStreamID = rstream.RevenueStreamID ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Revenue_SubStream rsstream ON poa.RevenueSubStreamID  = rsstream.RevenueSubStreamID ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Agencies agny ON poa.AgencyID = agny.AgencyID WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjPaymentAccount.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@MainFilter",pObjPaymentAccount.MainFilter),
                    new SqlParameter("@PaymentRefNo",pObjPaymentAccount.PaymentRefNo),
                    new SqlParameter("@PaymentDate",pObjPaymentAccount.strPaymentDate),
                    new SqlParameter("@TaxPayerTypeName",pObjPaymentAccount.TaxPayerTypeName),
                    new SqlParameter("@TaxPayerRIN",pObjPaymentAccount.TaxPayerRIN),
                    new SqlParameter("@TaxPayerName",pObjPaymentAccount.TaxPayerName),
                    new SqlParameter("@Amount",pObjPaymentAccount.strAmount),
                    new SqlParameter("@RevenueStreamName",pObjPaymentAccount.RevenueStreamName),
                    new SqlParameter("@RevenueSubStreamName",pObjPaymentAccount.RevenueSubStreamName),
                    new SqlParameter("@AgencyName",pObjPaymentAccount.AgencyName),
                    new SqlParameter("@SettlementMethodName",pObjPaymentAccount.SettlementMethodName),
                    new SqlParameter("@SettlementStatusName",pObjPaymentAccount.SettlementStatusName),
                    new SqlParameter("@Notes",pObjPaymentAccount.Notes),
                    new SqlParameter("@TransactionRefNo",pObjPaymentAccount.TransactionRefNo),
                };

                //Get Filtered Count
                int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntFilteredCount;

                return dcData;
            }
        }
    }
}
