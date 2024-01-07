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
    public class TreasuryReceiptRepository : ITreasuryReceiptRepository
    {
        EIRSEntities _db;

        public FuncResponse<Treasury_Receipt> REP_InsertTreasuryReceipt(Treasury_Receipt pObjReceipt)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<Treasury_Receipt> mObjFuncResponse = new FuncResponse<Treasury_Receipt>(); //Return Object

                Treasury_Receipt mObjInsertReceipt; //TreasuryReceipt Insert Object

                bool blnIsNotDuplicate = false;
                string mStrUrlCode = "";

                while (!blnIsNotDuplicate)
                {
                    mStrUrlCode = CommUtil.GenerateUniqueAphaNumeric();

                    var vDuplicateCheck = from tr in _db.Treasury_Receipt
                                          where tr.DocumentUrl.Equals(mStrUrlCode)
                                          select tr;

                    if (vDuplicateCheck.Count() > 0)
                    {
                        blnIsNotDuplicate = false;
                    }
                    else
                    {
                        blnIsNotDuplicate = true;
                    }
                }


                mObjInsertReceipt = new Treasury_Receipt
                {
                    CreatedBy = pObjReceipt.CreatedBy,
                    CreatedDate = pObjReceipt.CreatedDate
                };


                mObjInsertReceipt.AssessmentID = pObjReceipt.AssessmentID;
                mObjInsertReceipt.ServiceBillID = pObjReceipt.ServiceBillID;
                mObjInsertReceipt.ReceiptAmount = pObjReceipt.ReceiptAmount;
                mObjInsertReceipt.ReceiptDate = pObjReceipt.ReceiptDate;
                mObjInsertReceipt.StatusID = pObjReceipt.StatusID;
                mObjInsertReceipt.CancelNotes = pObjReceipt.CancelNotes;
                mObjInsertReceipt.DocumentUrl = mStrUrlCode;

                _db.Treasury_Receipt.Add(mObjInsertReceipt);

                try
                {
                    _db.SaveChanges();

                    var context = ((IObjectContextAdapter)_db).ObjectContext;
                    var refreshableObjects = _db.ChangeTracker.Entries().Select(c => c.Entity).ToList();
                    context.Refresh(RefreshMode.StoreWins, refreshableObjects);

                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.Message = "Receipt Added Successfully";

                    mObjFuncResponse.AdditionalData = mObjInsertReceipt;
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;
                    mObjFuncResponse.Message = "Receipt Addition Failed";
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_InsertReceiptSettlement(MAP_TreasuryReceipt_Settlement pObjReceiptSettlement)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                _db.MAP_TreasuryReceipt_Settlement.Add(pObjReceiptSettlement);

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.Message = "Receipt Added Successfully";
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;
                    mObjFuncResponse.Message = "Receipt Addition Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetTreasuryReceiptList_Result> REP_GetTreasuryReceiptList(Treasury_Receipt pObjReceipt)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTreasuryReceiptList(pObjReceipt.ReceiptID, pObjReceipt.AssessmentID, pObjReceipt.ServiceBillID, pObjReceipt.TaxPayerTypeID, pObjReceipt.TaxPayerID, pObjReceipt.DocumentUrl, pObjReceipt.StatusID).ToList();
            }
        }

        public usp_GetTreasuryReceiptList_Result REP_GetTreasuryReceiptDetails(Treasury_Receipt pObjReceipt)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTreasuryReceiptList(pObjReceipt.ReceiptID, pObjReceipt.AssessmentID, pObjReceipt.ServiceBillID, pObjReceipt.TaxPayerTypeID, pObjReceipt.TaxPayerID, pObjReceipt.DocumentUrl, pObjReceipt.StatusID).FirstOrDefault();
            }
        }

        public IDictionary<string, object> REP_SearchTreasuryReceipt(Treasury_Receipt pObjReceipt)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>
                {
                    ["TreasuryReceiptList"] = _db.usp_SearchTreasuryReceipt(pObjReceipt.WhereCondition, pObjReceipt.OrderBy, pObjReceipt.OrderByDirection, pObjReceipt.PageNumber, pObjReceipt.PageSize, pObjReceipt.MainFilter,
                                                                        pObjReceipt.ReceiptRefNo, pObjReceipt.StrReceiptDate, pObjReceipt.BillRefNo, pObjReceipt.StrReceiptAmount, pObjReceipt.ReceiptStatusName).ToList()
                };

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(ReceiptID) FROM Treasury_Receipt").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(ReceiptID) ");
                sbFilteredCountQuery.Append(" FROM Treasury_Receipt rcpt ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Assessment ast INNER JOIN TaxPayer_Types ast_tptype ON ast.TaxPayerTypeID = ast_tptype.TaxPayerTypeID ON rcpt.AssessmentID = ast.AssessmentID ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN ServiceBill sb INNER JOIN TaxPayer_Types sb_tptype ON sb.TaxPayerTypeID = sb_tptype.TaxPayerTypeID ON rcpt.ServiceBillID = sb.ServiceBillID ");
                sbFilteredCountQuery.Append("INNER JOIN Receipt_Status rs ON rcpt.StatusID = rs.ReceiptStatusID WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjReceipt.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                   new SqlParameter("@MainFilter",pObjReceipt.MainFilter),
                    new SqlParameter("@ReceiptRefNo",pObjReceipt.ReceiptRefNo??""),
                    new SqlParameter("@ReceiptDate",pObjReceipt.StrReceiptDate??""),
                    new SqlParameter("@BillRefNo",pObjReceipt.BillRefNo??""),
                    new SqlParameter("@ReceiptAmount",pObjReceipt.StrReceiptAmount??""),
                    new SqlParameter("@ReceiptStatus",pObjReceipt.ReceiptStatusName??"")
                };

                //Get Filtered Count
                int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntFilteredCount;

                return dcData;
            }
        }

        public FuncResponse REP_CancelTreasuryReceipt(Treasury_Receipt pObjReceipt)
        {
            using (_db = new EIRSEntities())
            {
                Treasury_Receipt mObjInsertUpdateReceipt; //Treasury Receipt Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Receipt
                if (pObjReceipt.ReceiptID != 0)
                {
                    mObjInsertUpdateReceipt = (from rcpt in _db.Treasury_Receipt
                                               where rcpt.ReceiptID == pObjReceipt.ReceiptID
                                               select rcpt).FirstOrDefault();

                    if (mObjInsertUpdateReceipt != null)
                    {
                        mObjInsertUpdateReceipt.StatusID = pObjReceipt.StatusID;
                        mObjInsertUpdateReceipt.CancelNotes = pObjReceipt.CancelNotes;
                        mObjInsertUpdateReceipt.ModifiedBy = pObjReceipt.ModifiedBy;
                        mObjInsertUpdateReceipt.ModifiedDate = pObjReceipt.ModifiedDate;
                        mObjInsertUpdateReceipt.CancelledBy = pObjReceipt.CancelledBy;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Receipt Cancelled Successfully";
                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Receipt Cancellation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_UpdateTRGenerated(Treasury_Receipt pObjReceipt)
        {
            using (_db = new EIRSEntities())
            {
                Treasury_Receipt mObjInsertUpdateReceipt; //Treasury Receipt Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Receipt
                if (pObjReceipt.ReceiptID != 0)
                {
                    mObjInsertUpdateReceipt = (from rcpt in _db.Treasury_Receipt
                                               where rcpt.ReceiptID == pObjReceipt.ReceiptID
                                               select rcpt).FirstOrDefault();

                    if (mObjInsertUpdateReceipt != null)
                    {
                        mObjInsertUpdateReceipt.GeneratedPath = pObjReceipt.GeneratedPath;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_UpdateTRSigned(Treasury_Receipt pObjReceipt)
        {
            using (_db = new EIRSEntities())
            {
                Treasury_Receipt mObjInsertUpdateReceipt; //Treasury Receipt Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load Receipt
                if (pObjReceipt.ReceiptID != 0)
                {
                    mObjInsertUpdateReceipt = (from rcpt in _db.Treasury_Receipt
                                               where rcpt.ReceiptID == pObjReceipt.ReceiptID
                                               select rcpt).FirstOrDefault();

                    if (mObjInsertUpdateReceipt != null)
                    {
                        mObjInsertUpdateReceipt.StatusID = pObjReceipt.StatusID;
                        mObjInsertUpdateReceipt.SignSourceID = pObjReceipt.SignSourceID;
                        mObjInsertUpdateReceipt.SignImgSrc = pObjReceipt.SignImgSrc;
                        mObjInsertUpdateReceipt.SignedPath = pObjReceipt.SignedPath;
                        mObjInsertUpdateReceipt.Notes = pObjReceipt.Notes;
                        mObjInsertUpdateReceipt.ModifiedBy = pObjReceipt.ModifiedBy;
                        mObjInsertUpdateReceipt.ModifiedDate = pObjReceipt.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();


                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Receipt Updated Successfully";
                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Receipt Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public void REP_UpdateSignedPath(Treasury_Receipt pObjReceipt)
        {
            using (_db = new EIRSEntities())
            {
                Treasury_Receipt mObjInsertUpdateReceipt; //Treasury Receipt Update Object

                //If Update Load Receipt
                if (pObjReceipt.ReceiptID != 0)
                {
                    mObjInsertUpdateReceipt = (from rcpt in _db.Treasury_Receipt
                                               where rcpt.ReceiptID == pObjReceipt.ReceiptID
                                               select rcpt).FirstOrDefault();

                    if (mObjInsertUpdateReceipt != null)
                    {
                        mObjInsertUpdateReceipt.SignedPath = pObjReceipt.SignedPath;

                        try
                        {
                            _db.SaveChanges();
                        }
                        catch (Exception Ex)
                        {
                        }
                    }
                }
            }
        }

        public IList<usp_GetSettlementWithoutReceipt_Result> REP_GetSettlementWithoutReceipt(long plngAssessmentID, long plngServiceBillID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetSettlementWithoutReceipt(plngAssessmentID, plngServiceBillID).ToList();
            }
        }

        public usp_GetTreasuryReceiptList_Result REP_VerifyTreasuryReceipt(Treasury_Receipt pObjTreasuryReceipt)
        {
            using (_db = new EIRSEntities())
            {
                var vReceipt = (from tr in _db.Treasury_Receipt
                                where tr.ReceiptRefNo == pObjTreasuryReceipt.ReceiptRefNo && tr.StatusID == 1
                                select tr).FirstOrDefault();

                if (vReceipt != null)
                {
                    var vReceiptData = _db.usp_GetTreasuryReceiptList(vReceipt.ReceiptID, null, null, null, null, null, null).FirstOrDefault();

                    if (vReceiptData.ASRefNo == pObjTreasuryReceipt.BillRefNo)
                    {
                        return vReceiptData;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        public Treasury_Receipt REP_VerifyTreasuryReceiptNew(Treasury_Receipt pObjTreasuryReceipt)
        {
            using (_db = new EIRSEntities())
            {
                var vReceipt = (from tr in _db.Treasury_Receipt
                                where tr.ReceiptRefNo == pObjTreasuryReceipt.ReceiptRefNo && tr.StatusID == 1
                                select tr).FirstOrDefault();

                if (vReceipt != null)
                {

                    return vReceipt;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
