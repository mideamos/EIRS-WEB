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
    public class SettlementRepository : ISettlementRepository
    {
        EIRSEntities _db;

        public FuncResponse<Settlement> REP_InsertUpdateSettlement(Settlement pObjSettlement)
        {
            using (_db = new EIRSEntities())
            {
                Settlement mObjInsertUpdateSettlement; //Settlement Insert Object
                FuncResponse<Settlement> mObjFuncResponse = new FuncResponse<Settlement>(); //Return Object

                if (pObjSettlement.ValidateDuplicateCheck)
                {
                    //Check if Duplicate
                    var vDuplicateCheck = (from smnt in _db.Settlements
                                           where smnt.TransactionRefNo == pObjSettlement.TransactionRefNo && smnt.SettlementID != pObjSettlement.SettlementID
                                           select smnt);

                    if (vDuplicateCheck.Count() > 0 && pObjSettlement.SettlementAmount > 0)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = "Transaction Ref No already exists";
                        return mObjFuncResponse;
                    }
                }

                //If Update Load Settlement Item
                if (pObjSettlement.SettlementID != 0)
                {
                    mObjInsertUpdateSettlement = (from smnt in _db.Settlements
                                                  where smnt.SettlementID == pObjSettlement.SettlementID
                                                  select smnt).FirstOrDefault();

                    if (mObjInsertUpdateSettlement != null)
                    {
                        mObjInsertUpdateSettlement.ModifiedBy = pObjSettlement.ModifiedBy;
                        mObjInsertUpdateSettlement.ModifiedDate = pObjSettlement.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateSettlement = new Settlement
                        {
                            CreatedBy = pObjSettlement.CreatedBy,
                            CreatedDate = pObjSettlement.CreatedDate
                        };
                    }
                }
                else // Else Insert Settlement Item
                {
                    mObjInsertUpdateSettlement = new Settlement
                    {
                        CreatedBy = pObjSettlement.CreatedBy,
                        CreatedDate = pObjSettlement.CreatedDate
                    };
                }

                mObjInsertUpdateSettlement.AssessmentID = pObjSettlement.AssessmentID;
                mObjInsertUpdateSettlement.ServiceBillID = pObjSettlement.ServiceBillID;
                mObjInsertUpdateSettlement.Active = pObjSettlement.Active;
                mObjInsertUpdateSettlement.SettlementID = pObjSettlement.SettlementID;
                mObjInsertUpdateSettlement.SettlementAmount = pObjSettlement.SettlementAmount;
                mObjInsertUpdateSettlement.SettlementMethodID = pObjSettlement.SettlementMethodID;
                mObjInsertUpdateSettlement.SettlementNotes = pObjSettlement.SettlementNotes;
                mObjInsertUpdateSettlement.SettlementDate = pObjSettlement.SettlementDate;
                mObjInsertUpdateSettlement.TransactionRefNo = pObjSettlement.TransactionRefNo;

                if (pObjSettlement.SettlementID == 0)
                {
                    _db.Settlements.Add(mObjInsertUpdateSettlement);
                }

                try
                {
                    _db.SaveChanges();

                    var context = ((IObjectContextAdapter)_db).ObjectContext;
                    var refreshableObjects = _db.ChangeTracker.Entries().Select(c => c.Entity).ToList();
                    context.Refresh(RefreshMode.StoreWins, refreshableObjects);

                    mObjFuncResponse.Success = true;
                    if (pObjSettlement.SettlementID == 0)
                    {
                        mObjFuncResponse.Message = "Settlement Added Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Settlement Updated Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjInsertUpdateSettlement;

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjSettlement.SettlementID == 0)
                    {
                        mObjFuncResponse.Message = "Settlement Addition Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Settlement Updation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public List<Settlement> REP_GetSettlementListById(long? serviceBillId, long? assessmentId)
        {
            using (_db = new EIRSEntities())
            {
                var settlements = new List<Settlement>();

                if (assessmentId != null)
                {
                    settlements = (from smnt in _db.Settlements
                                   where smnt.AssessmentID == assessmentId
                                   select smnt).ToList();
                    //return settlements;
                }

                if (serviceBillId != null)
                {
                    settlements = (from smnt in _db.Settlements
                                   where smnt.ServiceBillID == serviceBillId
                                   select smnt).ToList();
                }

                return settlements;
            }
        }
        public List<MAP_Assessment_Adjustment> REP_GetAdjustmentListById(long? assessmentId)
        {
            using (_db = new EIRSEntities())
            {
                var settlements = new List<MAP_Assessment_Adjustment>();
                // var settlements = new List<MAP_ServiceBill_Adjustment>();

                if (assessmentId != null)
                {
                    settlements = (from smnt in _db.MAP_Assessment_Adjustment
                                   where smnt.AAIID == assessmentId
                                   select smnt).ToList();
                    //return settlements;
                }


                return settlements;
            }
        }
        public List<MAP_ServiceBill_Adjustment> REP_GetAdjustmentServiceBillListById(long? serviceBillId)
        {
            using (_db = new EIRSEntities())
            {
                //var settlements = new List<MAP_Assessment_Adjustment>();
                var settlements = new List<MAP_ServiceBill_Adjustment>();


                if (serviceBillId != null)
                {
                    settlements = (from smnt in _db.MAP_ServiceBill_Adjustment
                                   where smnt.SBSIID == serviceBillId
                                   select smnt).ToList();
                }

                return settlements;
            }
        }
        public decimal REP_GetLateChargeListById(long? assessmentId)
        {
            using (_db = new EIRSEntities())
            {
                decimal? sumValue =0;
                //var settlements = new List<MAP_Assessment_LateCharge>();
                // var settlements = new List<MAP_ServiceBill_Adjustment>();

                if (assessmentId != null)
                {
                    var retVal = _db.MAP_Assessment_AssessmentRule.Where(o=>o.AssessmentID == assessmentId).ToList();   
                    foreach(var ret in retVal)
                    {
                        var retItem= _db.MAP_Assessment_AssessmentItem.Where(n=>n.AARID == ret.AARID).ToList();
                        //sumValue = +retItem.Sum(o => o.TaxAmount);
                        foreach(var rt in retItem) {
                          var kk = (from smnt in _db.MAP_Assessment_LateCharge
                                       where smnt.AAIID == rt.AAIID
                                       select smnt).Sum(x=>x.TotalAmount);
                            if(kk==null)
                                kk= 0;  
                            sumValue += kk;
                        }
                    }
                   
                    //return settlements;
                }

              
                return sumValue.Value;
            }
        }
        public decimal REP_GetLateChargeServiceBillListById(long? serviceBillId)
        {
            using (_db = new EIRSEntities())
            {
                //var settlements = new List<MAP_Assessment_Adjustment>();
                var settlements = new List<MAP_ServiceBill_LateCharge>();
                decimal? sumValue = 0;
                //var settlements = new List<MAP_Assessment_LateCharge>();
                // var settlements = new List<MAP_ServiceBill_Adjustment>();

                if (serviceBillId != null)
                {
                    var retVal = _db.MAP_ServiceBill_MDAService.Where(o => o.ServiceBillID == serviceBillId).ToList();
                    foreach (var ret in retVal)
                    {
                        var retItem = _db.MAP_ServiceBill_MDAServiceItem.Where(n => n.SBSID == ret.SBSID).ToList();
                        //sumValue = +retItem.Sum(o => o.TaxAmount);
                        foreach (var rt in retItem)
                        {
                            var kk = (from smnt in _db.MAP_ServiceBill_LateCharge
                                      where smnt.SBSIID == rt.SBSIID
                                      select smnt).Sum(x => x.TotalAmount);
                            if (kk == null)
                                kk = 0;
                            sumValue += kk;
                        }
                    }

                    //return settlements;
                }


               

                return sumValue.Value;
            }
        }

        public IList<usp_GetSettlementList_Result> REP_GetSettlementList(Settlement pObjSettlement)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetSettlementList(pObjSettlement.SettlementID, Convert.ToInt32(pObjSettlement.AssessmentID), Convert.ToInt32(pObjSettlement.ServiceBillID), pObjSettlement.TaxPayerTypeID, pObjSettlement.TaxPayerID).ToList();
            }
        }

        public usp_GetSettlementList_Result REP_GetSettlementDetails(Settlement pObjSettlement)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetSettlementList(pObjSettlement.SettlementID, Convert.ToInt32(pObjSettlement.AssessmentID), Convert.ToInt32(pObjSettlement.ServiceBillID), pObjSettlement.TaxPayerTypeID, pObjSettlement.TaxPayerID).FirstOrDefault();
            }
        }

        public FuncResponse REP_InsertSettlementItem(MAP_Settlement_SettlementItem pObjSettlementItem)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                _db.MAP_Settlement_SettlementItem.Add(pObjSettlementItem);


                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Exception = Ex;
                }

                return mObjResponse;
            }
        }

        public IList<usp_GetSettlementItemList_Result> REP_GetSettlementItemList(int pIntSettlementID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetSettlementItemList(pIntSettlementID).ToList();
            }
        }

        public usp_GetSettlementItemDetails_Result REP_GetSettlementItemDetails(long plngSIID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetSettlementItemDetails(plngSIID).FirstOrDefault();
            }
        }

        public IList<usp_GetTaxPayerPayment_Result> REP_GetTaxPayerPayment(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerPayment(pIntTaxPayerID, pIntTaxPayerTypeID).ToList();
            }
        }

        public IList<usp_GetSettleTransactionList_Result> REP_GetSettleTransactionList(Settlement pObjSettlement)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetSettleTransactionList(pObjSettlement.FromDate, pObjSettlement.ToDate, pObjSettlement.TransactionTypeID).ToList();
            }
        }

        public IDictionary<string, object> REP_SearchSettlement(Settlement pObjSettlement)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>
                {
                    ["SettlementList"] = _db.usp_SearchSettlement(pObjSettlement.WhereCondition, pObjSettlement.OrderBy, pObjSettlement.OrderByDirection, pObjSettlement.PageNumber, pObjSettlement.PageSize, pObjSettlement.MainFilter,
                                                                        pObjSettlement.SettlementRefNo, pObjSettlement.strSettlementDate, pObjSettlement.BillRefNo, pObjSettlement.BillAmount, pObjSettlement.strSettlementAmount, pObjSettlement.SettlementMethodName, pObjSettlement.SettlementStatusName, pObjSettlement.SettlementNotes).ToList()
                };

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(SettlementID) FROM Settlement").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(SettlementID) ");
                sbFilteredCountQuery.Append(" FROM Settlement stmt ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Assessment ast INNER JOIN TaxPayer_Types ast_tptype ON ast.TaxPayerTypeID = ast_tptype.TaxPayerTypeID INNER JOIN Settlement_Status ast_stat ON ast.SettlementStatusID = ast_stat.SettlementStatusID ON stmt.AssessmentID = ast.AssessmentID ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN ServiceBill sb INNER JOIN TaxPayer_Types sb_tptype ON sb.TaxPayerTypeID = sb_tptype.TaxPayerTypeID INNER JOIN Settlement_Status sb_stat ON sb.SettlementStatusID = sb_stat.SettlementStatusID ON stmt.ServiceBillID = sb.ServiceBillID ");
                sbFilteredCountQuery.Append(" INNER JOIN Settlement_Method smthd ON smthd.SettlementMethodID = stmt.SettlementMethodID WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjSettlement.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                   new SqlParameter("@MainFilter",pObjSettlement.MainFilter),
                    new SqlParameter("@SettlementRefNo",pObjSettlement.SettlementRefNo),
                    new SqlParameter("@SettlementDate",pObjSettlement.strSettlementDate),
                    new SqlParameter("@BillRefNo",pObjSettlement.BillRefNo),
                    new SqlParameter("@BillAmount",pObjSettlement.BillAmount),
                    new SqlParameter("@SettlementAmount",pObjSettlement.strSettlementAmount),
                    new SqlParameter("@SettlementMethodName",pObjSettlement.SettlementMethodName),
                    new SqlParameter("@SettlementStatus",pObjSettlement.SettlementStatusName),
                    new SqlParameter("@SettlementNotes",pObjSettlement.SettlementNotes),
                };

                //Get Filtered Count
                int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntFilteredCount;

                return dcData;
            }
        }

        public IList<usp_GetPAYEPayment_Result> REP_GetPAYEPayment(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetPAYEPayment(pIntTaxPayerID, pIntTaxPayerTypeID).ToList();
            }
        }

    }
}
