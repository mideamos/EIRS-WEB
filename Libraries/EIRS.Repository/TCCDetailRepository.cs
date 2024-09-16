using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class TCCDetailRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateTCCDetail(TCCDetail pObjTCCDetail)
        {
            using (_db = new EIRSEntities())
            {
                TCCDetail mObjInsertUpdateTCCDetail; //TCC Detail Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                //var vDuplicateCheck = (from TCCDetail in _db.TCCDetail
                //                       where TCCDetail.TCCDetailName == pObjTCCDetail.TCCDetailName && TCCDetail.TCCDetailID != pObjTCCDetail.TCCDetailID
                //                       select TCCDetail);

                //if (vDuplicateCheck.Count() > 0)
                //{
                //    mObjFuncResponse.Success = false;
                //    mObjFuncResponse.Message = "TCC Detail already exists";
                //    return mObjFuncResponse;
                //}

                //If Update Load TCC Detail
                if (pObjTCCDetail.TCCDetailID != 0)
                {
                    mObjInsertUpdateTCCDetail = (from TCCDetail in _db.TCCDetails
                                                    where TCCDetail.TCCDetailID == pObjTCCDetail.TCCDetailID
                                                    select TCCDetail).FirstOrDefault();

                    if (mObjInsertUpdateTCCDetail != null)
                    {
                        mObjInsertUpdateTCCDetail.ModifiedBy = pObjTCCDetail.ModifiedBy;
                        mObjInsertUpdateTCCDetail.ModifiedDate = pObjTCCDetail.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateTCCDetail = new TCCDetail();
                        mObjInsertUpdateTCCDetail.CreatedBy = pObjTCCDetail.CreatedBy;
                        mObjInsertUpdateTCCDetail.CreatedDate = pObjTCCDetail.CreatedDate;
                    }
                }
                else // Else Insert TCC Detail
                {
                    mObjInsertUpdateTCCDetail = new TCCDetail();
                    mObjInsertUpdateTCCDetail.CreatedBy = pObjTCCDetail.CreatedBy;
                    mObjInsertUpdateTCCDetail.CreatedDate = pObjTCCDetail.CreatedDate;
                }

                mObjInsertUpdateTCCDetail.TaxPayerTypeID = pObjTCCDetail.TaxPayerTypeID;
                mObjInsertUpdateTCCDetail.TaxPayerID = pObjTCCDetail.TaxPayerID;
                mObjInsertUpdateTCCDetail.TaxYear = pObjTCCDetail.TaxYear;
                mObjInsertUpdateTCCDetail.AssessableIncome = pObjTCCDetail.AssessableIncome;
                mObjInsertUpdateTCCDetail.TCCTaxPaid = pObjTCCDetail.TCCTaxPaid;
                mObjInsertUpdateTCCDetail.ERASTaxPaid = pObjTCCDetail.ERASTaxPaid;
                mObjInsertUpdateTCCDetail.ERASAssessed = pObjTCCDetail.ERASAssessed;

                if (pObjTCCDetail.TCCDetailID == 0)
                {
                    _db.TCCDetails.Add(mObjInsertUpdateTCCDetail);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjTCCDetail.TCCDetailID == 0)
                        mObjFuncResponse.Message = "TCC Detail Added Successfully";
                    else
                        mObjFuncResponse.Message = "TCC Detail Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjTCCDetail.TCCDetailID == 0)
                        mObjFuncResponse.Message = "TCC Detail Addition Failed";
                    else
                        mObjFuncResponse.Message = "TCC Detail Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IDictionary<string, object> REP_SearchTCCDetail(TCCDetail pObjTCCDetails)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>
                {
                    ["TCCDetailList"] = _db.usp_SearchTCCDetails(pObjTCCDetails.WhereCondition, pObjTCCDetails.OrderBy, pObjTCCDetails.OrderByDirection, pObjTCCDetails.PageNumber, pObjTCCDetails.PageSize, pObjTCCDetails.MainFilter,
                                                                        pObjTCCDetails.TaxPayerName, pObjTCCDetails.TaxPayerRIN, pObjTCCDetails.TIN, pObjTCCDetails.strTaxYear, pObjTCCDetails.strTCCTaxPaid).ToList()
                };

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(TCCDetailID) FROM TCCDetails").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(TCCDetailID) ");
                sbFilteredCountQuery.Append(" FROM TCCDetails tcd ");
                sbFilteredCountQuery.Append(" WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjTCCDetails.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                   new SqlParameter("@MainFilter",pObjTCCDetails.MainFilter),
                    new SqlParameter("@TaxPayerName",pObjTCCDetails.TaxPayerName??""),
                    new SqlParameter("@TaxPayerRIN",pObjTCCDetails.TaxPayerRIN??""),
                    new SqlParameter("@TIN",pObjTCCDetails.TIN??""),
                    new SqlParameter("@TaxYear",pObjTCCDetails.strTaxYear??""),
                    new SqlParameter("@TCCTaxPaid",pObjTCCDetails.strTCCTaxPaid??"")
                };

                //Get Filtered Count
                int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntFilteredCount;

                return dcData;
            }
        }

        //public usp_GetTCCDetailList_Result REP_GetTCCDetailDetails(TCCDetail pObjTCCDetail)
        //{
        //    using (_db = new EIRSEntities())
        //    {
        //        return _db.usp_GetTCCDetailList(pObjTCCDetail.TCCDetailName, pObjTCCDetail.TCCDetailID, pObjTCCDetail.TCCDetailIds, pObjTCCDetail.intStatus, pObjTCCDetail.IncludeTCCDetailIds, pObjTCCDetail.ExcludeTCCDetailIds).FirstOrDefault();
        //    }
        //}
    }
}
