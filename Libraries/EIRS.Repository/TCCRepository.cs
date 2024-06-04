using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace EIRS.Repository
{
    public class TCCRepository : ITCCRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertTCCDetail(TCCDetail pObjTCCDetails)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                TCCDetail mObjInsertTCCDetails; //TCC Details Insert Object
                mObjInsertTCCDetails = new TCCDetail
                {
                    CreatedBy = pObjTCCDetails.CreatedBy,
                    CreatedDate = pObjTCCDetails.CreatedDate
                };


                mObjInsertTCCDetails.TaxPayerTypeID = pObjTCCDetails.TaxPayerTypeID;
                mObjInsertTCCDetails.TaxPayerID = pObjTCCDetails.TaxPayerID;
                mObjInsertTCCDetails.TaxYear = pObjTCCDetails.TaxYear;
                mObjInsertTCCDetails.AssessableIncome = pObjTCCDetails.AssessableIncome;
                mObjInsertTCCDetails.TCCTaxPaid = pObjTCCDetails.TCCTaxPaid;
                mObjInsertTCCDetails.ERASTaxPaid = pObjTCCDetails.ERASTaxPaid;
                mObjInsertTCCDetails.ERASAssessed = pObjTCCDetails.ERASAssessed;

                _db.TCCDetails.Add(mObjInsertTCCDetails);

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.Message = "TCC Details Added Successfully";
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;
                    mObjFuncResponse.Message = "TCC Details Addition Failed";
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_InsertUpdateTCCDetail(TCCDetail pObjTCCDetail)
        {
            using (_db = new EIRSEntities())
            {
                TCCDetail mObjInsertUpdateTCCDetail;
                FuncResponse mObjFuncResponse = new FuncResponse();

                if (pObjTCCDetail.TCCDetailID != 0)
                {
                    mObjInsertUpdateTCCDetail = (from ist in _db.TCCDetails
                                                 where ist.TCCDetailID == pObjTCCDetail.TCCDetailID
                                                 select ist).FirstOrDefault();

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
                else
                {
                    mObjInsertUpdateTCCDetail = new TCCDetail();
                    mObjInsertUpdateTCCDetail.CreatedBy = pObjTCCDetail.CreatedBy;
                    mObjInsertUpdateTCCDetail.CreatedDate = pObjTCCDetail.CreatedDate;
                }

                //mObjInsertUpdateTCCDetail.TCCRequestID = pObjTCCDetail.TCCRequestID != null ? pObjTCCDetail.TCCRequestID : mObjInsertUpdateTCCDetail.TCCRequestID;
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
                    {
                        mObjFuncResponse.Message = "TCC Detail Added Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "TCC Detail Updated Successfully";
                    }
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = ex;
                    if (pObjTCCDetail.TCCDetailID == 0)
                    {
                        mObjFuncResponse.Message = "TCC Detail Addition Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "TCC Detail Updation Failed";
                    }
                }
                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_RemoveTCCDetail(TCCDetail pObjTCCDetail)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();
                TCCDetail mObjRecipient = _db.TCCDetails.Find(pObjTCCDetail.TCCDetailID);

                if (mObjFuncResponse == null)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "TCC Detail Already Removed";
                }

                else
                {
                    _db.TCCDetails.Remove(mObjRecipient);

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = Ex.Message;
                    }
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

        public IList<usp_GetTCCDetail_Result> REP_GetTCCDetail(int TaxPayerID, int TaxPayerTypeID, int TaxYear)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTCCDetail(TaxPayerID, TaxPayerTypeID, TaxYear).ToList();
            }
        }
        public IList<DirectAssessmentIncomeStreams_Result> REP_GetTCCDetailNew(int TaxPayerTypeID, int TaxPayerID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.DirectAssessmentIncomeStreams(TaxPayerTypeID, TaxPayerID).ToList();
            }
        }

        public FuncResponse REP_InsertTaxClearanceCertificate(TaxClearanceCertificate pObjTaxClearanceCertificate)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                TaxClearanceCertificate mObjInsertTaxClearanceCertificate; //Tax Clearance Certificate Insert Object
                mObjInsertTaxClearanceCertificate = new TaxClearanceCertificate
                {
                    CreatedBy = pObjTaxClearanceCertificate.CreatedBy,
                    CreatedDate = pObjTaxClearanceCertificate.CreatedDate
                };


                mObjInsertTaxClearanceCertificate.TCCDate = pObjTaxClearanceCertificate.TCCDate;
                mObjInsertTaxClearanceCertificate.TaxYear = pObjTaxClearanceCertificate.TaxYear;
                mObjInsertTaxClearanceCertificate.TaxPayerTypeID = pObjTaxClearanceCertificate.TaxPayerTypeID;
                mObjInsertTaxClearanceCertificate.TaxPayerID = pObjTaxClearanceCertificate.TaxPayerID;
                mObjInsertTaxClearanceCertificate.RequestRefNo = pObjTaxClearanceCertificate.RequestRefNo;
                mObjInsertTaxClearanceCertificate.SerialNumber = pObjTaxClearanceCertificate.SerialNumber;
                mObjInsertTaxClearanceCertificate.TaxPayerDetails = pObjTaxClearanceCertificate.TaxPayerDetails;
                mObjInsertTaxClearanceCertificate.IncomeSource = pObjTaxClearanceCertificate.IncomeSource;
                mObjInsertTaxClearanceCertificate.StatusID = pObjTaxClearanceCertificate.StatusID;

                _db.TaxClearanceCertificates.Add(mObjInsertTaxClearanceCertificate);

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.Message = "Tax Clearance Certificate Added Successfully";
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;
                    mObjFuncResponse.Message = "Tax Clearance Certificate Addition Failed";
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_InsertUpdateTaxClearanceCertificate(TaxClearanceCertificate pObjTaxClearanceCertificate)
        {
            using (_db = new EIRSEntities())
            {
                TaxClearanceCertificate mObjInsertUpdateTaxClearanceCertificate;
                FuncResponse mObjFuncResponse = new FuncResponse();

                if (pObjTaxClearanceCertificate.TCCID != 0)
                {
                    mObjInsertUpdateTaxClearanceCertificate = (from ist in _db.TaxClearanceCertificates
                                                               where ist.TCCID == pObjTaxClearanceCertificate.TCCID
                                                               select ist).FirstOrDefault();

                    if (mObjInsertUpdateTaxClearanceCertificate != null)
                    {
                        mObjInsertUpdateTaxClearanceCertificate.ModifiedBy = pObjTaxClearanceCertificate.ModifiedBy;
                        mObjInsertUpdateTaxClearanceCertificate.ModifiedDate = pObjTaxClearanceCertificate.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateTaxClearanceCertificate = new TaxClearanceCertificate();
                        mObjInsertUpdateTaxClearanceCertificate.CreatedBy = pObjTaxClearanceCertificate.CreatedBy;
                        mObjInsertUpdateTaxClearanceCertificate.CreatedDate = pObjTaxClearanceCertificate.CreatedDate;
                    }
                }
                else
                {
                    mObjInsertUpdateTaxClearanceCertificate = new TaxClearanceCertificate();
                    mObjInsertUpdateTaxClearanceCertificate.CreatedBy = pObjTaxClearanceCertificate.CreatedBy;
                    mObjInsertUpdateTaxClearanceCertificate.CreatedDate = pObjTaxClearanceCertificate.CreatedDate;
                }

                mObjInsertUpdateTaxClearanceCertificate.TCCDate = pObjTaxClearanceCertificate.TCCDate;
                mObjInsertUpdateTaxClearanceCertificate.TaxYear = pObjTaxClearanceCertificate.TaxYear;
                mObjInsertUpdateTaxClearanceCertificate.TaxPayerTypeID = pObjTaxClearanceCertificate.TaxPayerTypeID;
                mObjInsertUpdateTaxClearanceCertificate.TaxPayerID = pObjTaxClearanceCertificate.TaxPayerID;
                mObjInsertUpdateTaxClearanceCertificate.RequestRefNo = pObjTaxClearanceCertificate.RequestRefNo;
                mObjInsertUpdateTaxClearanceCertificate.SerialNumber = pObjTaxClearanceCertificate.SerialNumber;
                mObjInsertUpdateTaxClearanceCertificate.TaxPayerDetails = pObjTaxClearanceCertificate.TaxPayerDetails;
                mObjInsertUpdateTaxClearanceCertificate.IncomeSource = pObjTaxClearanceCertificate.IncomeSource;
                mObjInsertUpdateTaxClearanceCertificate.StatusID = pObjTaxClearanceCertificate.StatusID;


                if (pObjTaxClearanceCertificate.TCCID == 0)
                {
                    _db.TaxClearanceCertificates.Add(mObjInsertUpdateTaxClearanceCertificate);
                }
                try
                {
                    _db.SaveChanges();

                    mObjFuncResponse.Success = true;
                    if (pObjTaxClearanceCertificate.TCCID == 0)
                    {
                        mObjFuncResponse.Message = "Tax Clearance Certificate Added Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Tax Clearance Certificate Updated Successfully";
                    }
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = ex;
                    if (pObjTaxClearanceCertificate.TCCID == 0)
                    {
                        mObjFuncResponse.Message = "Tax Clearance Certificate Addition Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Tax Clearance Certificate Updation Failed";
                    }
                }
                return mObjFuncResponse;
            }
        }

        public IDictionary<string, object> REP_SearchTaxClearanceCertificate(TaxClearanceCertificate pObjTaxClearanceCertificate)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>
                {
                    ["TaxClearanceCertificateList"] = _db.usp_SearchTaxClearanceCertificate(pObjTaxClearanceCertificate.WhereCondition, pObjTaxClearanceCertificate.OrderBy, pObjTaxClearanceCertificate.OrderByDirection, pObjTaxClearanceCertificate.PageNumber, pObjTaxClearanceCertificate.PageSize, pObjTaxClearanceCertificate.MainFilter).ToList()
                };

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(TCCID) FROM TaxClearanceCertificate").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(TCCID) ");
                sbFilteredCountQuery.Append(" FROM TaxClearanceCertificate ");
                sbFilteredCountQuery.Append(" WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjTaxClearanceCertificate.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                   new SqlParameter("@MainFilter",pObjTaxClearanceCertificate.MainFilter)
                };

                //Get Filtered Count
                int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntFilteredCount;

                return dcData;
            }
        }

        public IList<usp_GetTaxClearanceCertificateDetails_Result> REP_GetTaxClearanceCertificateList(TaxClearanceCertificate pObjTaxClearanceCertificate)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxClearanceCertificateDetails(pObjTaxClearanceCertificate.TCCID, pObjTaxClearanceCertificate.TaxPayerID, pObjTaxClearanceCertificate.TaxPayerTypeID, pObjTaxClearanceCertificate.TaxYear, pObjTaxClearanceCertificate.RequestRefNo).ToList();
            }
        }

        public usp_GetTaxClearanceCertificateDetails_Result REP_GetTaxClearanceCertificateDetail(TaxClearanceCertificate pObjTaxClearanceCertificate)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxClearanceCertificateDetails(pObjTaxClearanceCertificate.TCCID, pObjTaxClearanceCertificate.TaxPayerID, pObjTaxClearanceCertificate.TaxPayerTypeID, pObjTaxClearanceCertificate.TaxYear, pObjTaxClearanceCertificate.RequestRefNo).FirstOrDefault();
            }
        }

        public FuncResponse<TCC_Request> REP_GetIncompleteRequest(TCC_Request pObjRequest)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<TCC_Request> mObjFuncResponse = new FuncResponse<TCC_Request>();

                var vRequest = (from req in _db.TCC_Request
                                where req.TaxPayerID == pObjRequest.TaxPayerID
                                && req.TaxPayerTypeID == pObjRequest.TaxPayerTypeID
                                && req.TaxYear == pObjRequest.TaxYear
                                select req);

                if (vRequest.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.AdditionalData = vRequest.FirstOrDefault();
                }
                else
                {
                    mObjFuncResponse.Success = true;
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse<TCC_Request> REP_InsertTCCRequest(TCC_Request pObjRequest)
        {
            using (_db = new EIRSEntities())
            {
                using (_db = new EIRSEntities())
                {
                    FuncResponse<TCC_Request> mObjFuncResponse = new FuncResponse<TCC_Request>(); //Return Object

                    TCC_Request mObjInsertTCC_Requests; //TCC Details Insert Object
                    mObjInsertTCC_Requests = new TCC_Request
                    {
                        CreatedBy = pObjRequest.CreatedBy,
                        CreatedDate = pObjRequest.CreatedDate
                    };


                    mObjInsertTCC_Requests.RequestDate = pObjRequest.RequestDate;
                    mObjInsertTCC_Requests.TaxPayerTypeID = pObjRequest.TaxPayerTypeID;
                    mObjInsertTCC_Requests.TaxPayerID = pObjRequest.TaxPayerID;
                    mObjInsertTCC_Requests.TaxYear = pObjRequest.TaxYear;
                    mObjInsertTCC_Requests.StatusID = pObjRequest.StatusID;
                    mObjInsertTCC_Requests.TaxOfficeId = pObjRequest.TaxOfficeId;


                    _db.TCC_Request.Add(mObjInsertTCC_Requests);

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                        mObjFuncResponse.Message = "TCC Request Added Successfully";

                        mObjFuncResponse.AdditionalData = mObjInsertTCC_Requests;
                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Exception = Ex;
                        mObjFuncResponse.Message = "TCC Request Addition Failed";
                    }

                    return mObjFuncResponse;
                }
            }
        }

        public FuncResponse REP_UpdateRequestStatus(TCC_Request pObjRequest)
        {
            using (_db = new EIRSEntities())
            {
                TCC_Request mObjUpdateRequest;
                FuncResponse mObjFuncResponse = new FuncResponse();

                if (pObjRequest.TCCRequestID != 0)
                {
                    mObjUpdateRequest = (from req in _db.TCC_Request
                                         where req.TCCRequestID == pObjRequest.TCCRequestID
                                         select req).FirstOrDefault();

                    if (mObjUpdateRequest != null)
                    {
                        mObjUpdateRequest.StatusID = pObjRequest.StatusID;
                        mObjUpdateRequest.ModifiedBy = pObjRequest.ModifiedBy;
                        mObjUpdateRequest.GeneratedPath = pObjRequest.GeneratedPath;
                        mObjUpdateRequest.ValidatedPath = pObjRequest.ValidatedPath;
                        mObjUpdateRequest.SEDE_OrderID = pObjRequest.SEDE_OrderID;
                        mObjUpdateRequest.ServiceBillID = pObjRequest.ServiceBillID;
                        mObjUpdateRequest.VisibleSignStatusID = pObjRequest.VisibleSignStatusID;
                        mObjUpdateRequest.GeneratePathForPrint = pObjRequest.GeneratePathForPrint;
                        mObjUpdateRequest.SEDE_DocumentID = pObjRequest.SEDE_DocumentID;
                        mObjUpdateRequest.ModifiedDate = pObjRequest.ModifiedDate;
                        mObjUpdateRequest.RequestDate = pObjRequest.RequestDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Request Updated Successfully";
                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Request Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetRequestIncomeStreamList_Result> REP_GetIncomeStreamList(long plngRequestID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetRequestIncomeStreamList(plngRequestID).ToList();
            }
        }

        public FuncResponse REP_InsertUpdateIncomeStream(MAP_TCCRequest_IncomeStream pObjIncomeStream)
        {
            using (_db = new EIRSEntities())
            {
                MAP_TCCRequest_IncomeStream mObjInsertUpdateIncomeStream;
                FuncResponse mObjFuncResponse = new FuncResponse();

                if (pObjIncomeStream.TRISID != 0)
                {
                    mObjInsertUpdateIncomeStream = (from ist in _db.MAP_TCCRequest_IncomeStream
                                                    where ist.TRISID == pObjIncomeStream.TRISID
                                                    select ist).FirstOrDefault();

                    if (mObjInsertUpdateIncomeStream != null)
                    {
                        mObjInsertUpdateIncomeStream.ModifiedBy = pObjIncomeStream.ModifiedBy;
                        mObjInsertUpdateIncomeStream.ModifiedDate = pObjIncomeStream.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateIncomeStream = new MAP_TCCRequest_IncomeStream();
                        mObjInsertUpdateIncomeStream.CreatedBy = pObjIncomeStream.CreatedBy;
                        mObjInsertUpdateIncomeStream.CreatedDate = pObjIncomeStream.CreatedDate;
                    }
                }
                else
                {
                    mObjInsertUpdateIncomeStream = new MAP_TCCRequest_IncomeStream();
                    mObjInsertUpdateIncomeStream.CreatedBy = pObjIncomeStream.CreatedBy;
                    mObjInsertUpdateIncomeStream.CreatedDate = pObjIncomeStream.CreatedDate;
                }

                mObjInsertUpdateIncomeStream.TCCRequestID = pObjIncomeStream.TCCRequestID != null ? pObjIncomeStream.TCCRequestID : mObjInsertUpdateIncomeStream.TCCRequestID;
                mObjInsertUpdateIncomeStream.TaxYear = pObjIncomeStream.TaxYear != null ? pObjIncomeStream.TaxYear : mObjInsertUpdateIncomeStream.TaxYear;
                mObjInsertUpdateIncomeStream.TotalIncomeEarned = pObjIncomeStream.TotalIncomeEarned != null ? pObjIncomeStream.TotalIncomeEarned : mObjInsertUpdateIncomeStream.TotalIncomeEarned;
                mObjInsertUpdateIncomeStream.TaxPayerRoleID = pObjIncomeStream.TaxPayerRoleID != null ? pObjIncomeStream.TaxPayerRoleID : mObjInsertUpdateIncomeStream.TaxPayerRoleID;
                mObjInsertUpdateIncomeStream.BusinessID = pObjIncomeStream.BusinessID != null ? pObjIncomeStream.BusinessID : mObjInsertUpdateIncomeStream.BusinessID;
                mObjInsertUpdateIncomeStream.Notes = pObjIncomeStream.Notes != null ? pObjIncomeStream.Notes : mObjInsertUpdateIncomeStream.Notes;
                mObjInsertUpdateIncomeStream.BusinessName = pObjIncomeStream.BusinessName != null ? pObjIncomeStream.BusinessName : mObjInsertUpdateIncomeStream.BusinessName;
                mObjInsertUpdateIncomeStream.BusinessTypeID = pObjIncomeStream.BusinessTypeID != null ? pObjIncomeStream.BusinessTypeID : mObjInsertUpdateIncomeStream.BusinessTypeID;
                mObjInsertUpdateIncomeStream.LGAID = pObjIncomeStream.LGAID != null ? pObjIncomeStream.LGAID : mObjInsertUpdateIncomeStream.LGAID;
                mObjInsertUpdateIncomeStream.BusinessOperationID = pObjIncomeStream.BusinessOperationID != null ? pObjIncomeStream.BusinessOperationID : mObjInsertUpdateIncomeStream.BusinessOperationID;
                mObjInsertUpdateIncomeStream.BusinessAddress = pObjIncomeStream.BusinessAddress != null ? pObjIncomeStream.BusinessAddress : mObjInsertUpdateIncomeStream.BusinessAddress;
                mObjInsertUpdateIncomeStream.BusinessNumber = pObjIncomeStream.BusinessNumber != null ? pObjIncomeStream.BusinessNumber : mObjInsertUpdateIncomeStream.BusinessNumber;
                mObjInsertUpdateIncomeStream.ContactPersonName = pObjIncomeStream.ContactPersonName != null ? pObjIncomeStream.ContactPersonName : mObjInsertUpdateIncomeStream.ContactPersonName;


                if (pObjIncomeStream.TRISID == 0)
                {
                    _db.MAP_TCCRequest_IncomeStream.Add(mObjInsertUpdateIncomeStream);
                }
                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.AdditionalData = mObjInsertUpdateIncomeStream;

                    mObjFuncResponse.Success = true;
                    if (pObjIncomeStream.TRISID == 0)
                    {
                        mObjFuncResponse.Message = "Income Stream Added Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Income Stream Updated Successfully";
                    }
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = ex;
                    if (pObjIncomeStream.TRISID == 0)
                    {
                        mObjFuncResponse.Message = "Income Stream Addition Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Income Stream Updation Failed";
                    }
                }
                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_RemoveIncomeStream(MAP_TCCRequest_IncomeStream pObjIncomeStream)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();
                MAP_TCCRequest_IncomeStream mObjRecipient = _db.MAP_TCCRequest_IncomeStream.Find(pObjIncomeStream.TRISID);

                if (mObjFuncResponse == null)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Income Stream Already Removed";
                }

                else
                {
                    _db.MAP_TCCRequest_IncomeStream.Remove(mObjRecipient);

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = Ex.Message;
                    }
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_UpdateServiceBillInRequest(TCC_Request pObjRequest)
        {
            using (_db = new EIRSEntities())
            {
                using (_db = new EIRSEntities())
                {
                    FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object
                    TCC_Request mObjUpdateTCCRequest = _db.TCC_Request.Find(pObjRequest.TCCRequestID);

                    if (mObjUpdateTCCRequest != null)
                    {
                        mObjUpdateTCCRequest.ServiceBillID = pObjRequest.ServiceBillID;
                        mObjUpdateTCCRequest.StatusID = pObjRequest.StatusID;

                        //Insert Stages
                        var vStageList = _db.MST_TCCStage.ToList();
                        MAP_TCCRequest_Stages mObjRequestStage;
                        foreach (var item in vStageList)
                        {
                            var vStageData = (from os in _db.MAP_TCCRequest_Stages
                                              where os.RequestID == pObjRequest.TCCRequestID
                                              && os.StageID == item.TCCStageID
                                              select os).FirstOrDefault();

                            if (vStageData == null)
                            {

                                mObjRequestStage = new MAP_TCCRequest_Stages()
                                {
                                    RequestID = pObjRequest.TCCRequestID,
                                    StageID = item.TCCStageID
                                };

                                _db.MAP_TCCRequest_Stages.Add(mObjRequestStage);
                            }
                        }
                    }
                    else
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = "Request Not Found";
                        return mObjFuncResponse;
                    }

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                        mObjFuncResponse.Message = "TCC Request Added Successfully";


                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Exception = Ex;
                        mObjFuncResponse.Message = "TCC Request Addition Failed";
                    }

                    return mObjFuncResponse;
                }
            }
        }

        public IDictionary<string, object> REP_SearchTCCRequest(TCC_Request pObjRequest)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>
                {
                    ["RequestList"] = _db.usp_SearchTCCRequest(pObjRequest.WhereCondition, pObjRequest.OrderBy, pObjRequest.OrderByDirection, pObjRequest.PageNumber, pObjRequest.PageSize, pObjRequest.MainFilter,
                                                                        pObjRequest.StatusID, pObjRequest.TaxOfficeID).ToList()
                };

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(treq.TCCRequestID) FROM TCC_Request treq").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(treq.TCCRequestID) ");
                sbFilteredCountQuery.Append(" FROM TCC_Request treq ");
                sbFilteredCountQuery.Append(" INNER JOIN Individual ind ON treq.TaxPayerID = ind.IndividualID ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN ServiceBill sb ON treq.ServiceBillID = sb.ServiceBillID ");
                sbFilteredCountQuery.Append(" LEFT OUTER JOIN Settlement_Status sb_stat ON sb.SettlementStatusID =  sb_stat.SettlementStatusID ");
                sbFilteredCountQuery.Append(" INNER JOIN MST_TCCRequestStatus req_stat ON treq.StatusID = req_stat.StatusID ");
                sbFilteredCountQuery.Append(" INNER JOIN Tax_Offices toff ON ind.TaxOfficeID = toff.TaxOfficeID ");
                sbFilteredCountQuery.Append(" WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjRequest.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                   new SqlParameter("@MainFilter",pObjRequest.MainFilter),
                    new SqlParameter("@StatusID",pObjRequest.StatusID??0),
                    new SqlParameter("@TaxOfficeID",pObjRequest.TaxOfficeID)
                };

                //Get Filtered Count
                int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntFilteredCount;

                return dcData;
            }
        }

        public IList<usp_GetTCCRequestList_Result> REP_GetTCCRequestList(TCC_Request pObjRequest)
        {
            using (_db = new EIRSEntities())
            {

                return _db.usp_GetTCCRequestList(pObjRequest.TaxPayerID, pObjRequest.StatusID, pObjRequest.TaxOfficeID).ToList();
            }
        }

        public usp_GetTCCRequestDetails_Result REP_GetRequestDetails(long plngRequestID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTCCRequestDetails(plngRequestID).FirstOrDefault();
            }
        }

        public TCC_Request REP_GetRequestBasedOnServiceBill(long pIntServiceBillID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.TCC_Request.Where(t => t.ServiceBillID == pIntServiceBillID).FirstOrDefault();
            }
        }

        public IList<usp_GetAdminRequestStageList_Result> REP_GetAdminRequestStageList(long plngRequestID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAdminRequestStageList(plngRequestID).ToList();
            }
        }

        public FuncResponse REP_InsertUpdateValidateInformation(MAP_TCCRequest_ValidateTaxPayerInformation pObjValidateInformation)
        {
            using (_db = new EIRSEntities())
            {
                MAP_TCCRequest_ValidateTaxPayerInformation mObjInsertUpdateValidateInformation;
                FuncResponse mObjFuncResponse = new FuncResponse();

                if (pObjValidateInformation.VTPInformationID != 0)
                {
                    mObjInsertUpdateValidateInformation = (from oa in _db.MAP_TCCRequest_ValidateTaxPayerInformation
                                                           where oa.VTPInformationID == pObjValidateInformation.VTPInformationID
                                                           select oa).FirstOrDefault();

                    if (mObjInsertUpdateValidateInformation != null)
                    {
                        mObjInsertUpdateValidateInformation.ModifiedBy = pObjValidateInformation.ModifiedBy;
                        mObjInsertUpdateValidateInformation.ModifiedDate = pObjValidateInformation.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateValidateInformation = new MAP_TCCRequest_ValidateTaxPayerInformation
                        {
                            CreatedBy = pObjValidateInformation.CreatedBy,
                            CreatedDate = pObjValidateInformation.CreatedDate
                        };
                    }
                }
                else
                {
                    mObjInsertUpdateValidateInformation = new MAP_TCCRequest_ValidateTaxPayerInformation
                    {
                        CreatedBy = pObjValidateInformation.CreatedBy,
                        CreatedDate = pObjValidateInformation.CreatedDate
                    };
                }

                mObjInsertUpdateValidateInformation.RequestID = pObjValidateInformation.RequestID;
                mObjInsertUpdateValidateInformation.Notes = pObjValidateInformation.Notes;


                if (pObjValidateInformation.VTPInformationID == 0)
                {
                    _db.MAP_TCCRequest_ValidateTaxPayerInformation.Add(mObjInsertUpdateValidateInformation);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;

                    if (pObjValidateInformation.VTPInformationID == 0)
                    {
                        mObjFuncResponse.Message = "Validated Tax Payer Information Completed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Validated Tax Payer Information Completed";
                    }
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;

                    mObjFuncResponse.Exception = ex;

                    if (pObjValidateInformation.VTPInformationID == 0)
                    {
                        mObjFuncResponse.Message = "Validation of Tax Payer Information Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Validation of Tax Payer Information Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_UpdateRequestStage(MAP_TCCRequest_Stages pObjRequestStage)
        {
            using (_db = new EIRSEntities())
            {
                MAP_TCCRequest_Stages mObjUpdateRequestStage;
                FuncResponse mObjFuncResponse = new FuncResponse();

                mObjUpdateRequestStage = (from os in _db.MAP_TCCRequest_Stages
                                          where os.RequestID == pObjRequestStage.RequestID
                                          && os.StageID == pObjRequestStage.StageID
                                          select os).FirstOrDefault();

                if (mObjUpdateRequestStage != null)
                {
                    mObjUpdateRequestStage.ApprovalDate = pObjRequestStage.ApprovalDate != null ? pObjRequestStage.ApprovalDate : mObjUpdateRequestStage.ApprovalDate;

                    if (pObjRequestStage.ApprovalDate != null)
                    {
                        TCC_Request mObjRequest = _db.TCC_Request.Where(t => t.TCCRequestID == pObjRequestStage.RequestID).FirstOrDefault();

                        if (mObjRequest != null)
                        {
                            mObjRequest.StatusID = pObjRequestStage.StatusID;
                        }
                    }

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                        mObjFuncResponse.Message = "Request Stage Updated Successfully";
                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Exception = Ex;
                        mObjFuncResponse.Message = "Request Stage Updation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public MAP_TCCRequest_ValidateTaxPayerInformation REP_GetValidateInformationDetails(long plngRequestID)
        {
            using (_db = new EIRSEntities())
            {
                var vData = (from oa in _db.MAP_TCCRequest_ValidateTaxPayerInformation
                             where oa.RequestID == plngRequestID
                             select oa).FirstOrDefault();

                return vData;
            }
        }

        public FuncResponse REP_InsertUpdateValidateIncome(MAP_TCCRequest_ValidateTaxPayerIncome pObjValidateIncome)
        {
            using (_db = new EIRSEntities())
            {
                MAP_TCCRequest_ValidateTaxPayerIncome mObjInsertUpdateValidateIncome;
                FuncResponse mObjFuncResponse = new FuncResponse();

                if (pObjValidateIncome.VTPIncomeID != 0)
                {
                    mObjInsertUpdateValidateIncome = (from oa in _db.MAP_TCCRequest_ValidateTaxPayerIncome
                                                      where oa.VTPIncomeID == pObjValidateIncome.VTPIncomeID
                                                      select oa).FirstOrDefault();

                    if (mObjInsertUpdateValidateIncome != null)
                    {
                        mObjInsertUpdateValidateIncome.ModifiedBy = pObjValidateIncome.ModifiedBy;
                        mObjInsertUpdateValidateIncome.ModifiedDate = pObjValidateIncome.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateValidateIncome = new MAP_TCCRequest_ValidateTaxPayerIncome
                        {
                            CreatedBy = pObjValidateIncome.CreatedBy,
                            CreatedDate = pObjValidateIncome.CreatedDate
                        };
                    }
                }
                else
                {
                    mObjInsertUpdateValidateIncome = new MAP_TCCRequest_ValidateTaxPayerIncome
                    {
                        CreatedBy = pObjValidateIncome.CreatedBy,
                        CreatedDate = pObjValidateIncome.CreatedDate
                    };
                }

                mObjInsertUpdateValidateIncome.RequestID = pObjValidateIncome.RequestID;
                mObjInsertUpdateValidateIncome.Notes = pObjValidateIncome.Notes;


                if (pObjValidateIncome.VTPIncomeID == 0)
                {
                    _db.MAP_TCCRequest_ValidateTaxPayerIncome.Add(mObjInsertUpdateValidateIncome);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;

                    if (pObjValidateIncome.VTPIncomeID == 0)
                    {
                        mObjFuncResponse.Message = "Validated Tax Payer Income Completed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Validated Tax Payer Income Completed";
                    }
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;

                    mObjFuncResponse.Exception = ex;

                    if (pObjValidateIncome.VTPIncomeID == 0)
                    {
                        mObjFuncResponse.Message = "Validation of Tax Payer Income Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Validation of Tax Payer Income Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public MAP_TCCRequest_ValidateTaxPayerIncome REP_GetValidateIncomeDetails(long plngRequestID)
        {
            using (_db = new EIRSEntities())
            {
                var vData = (from oa in _db.MAP_TCCRequest_ValidateTaxPayerIncome
                             where oa.RequestID == plngRequestID
                             select oa).FirstOrDefault();

                return vData;
            }
        }

        public FuncResponse REP_InsertUpdateGenerateTCCDetail(MAP_TCCRequest_GenerateTCCDetail pObjGenerateTCCDetail)
        {
            using (_db = new EIRSEntities())
            {
                MAP_TCCRequest_GenerateTCCDetail mObjInsertUpdateGenerateTCCDetail;
                FuncResponse mObjFuncResponse = new FuncResponse();

                if (pObjGenerateTCCDetail.GTCCDetailID != 0)
                {
                    mObjInsertUpdateGenerateTCCDetail = (from oa in _db.MAP_TCCRequest_GenerateTCCDetail
                                                         where oa.GTCCDetailID == pObjGenerateTCCDetail.GTCCDetailID
                                                         select oa).FirstOrDefault();

                    if (mObjInsertUpdateGenerateTCCDetail != null)
                    {
                        mObjInsertUpdateGenerateTCCDetail.ModifiedBy = pObjGenerateTCCDetail.ModifiedBy;
                        mObjInsertUpdateGenerateTCCDetail.ModifiedDate = pObjGenerateTCCDetail.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateGenerateTCCDetail = new MAP_TCCRequest_GenerateTCCDetail
                        {
                            CreatedBy = pObjGenerateTCCDetail.CreatedBy,
                            CreatedDate = pObjGenerateTCCDetail.CreatedDate
                        };
                    }
                }
                else
                {
                    mObjInsertUpdateGenerateTCCDetail = new MAP_TCCRequest_GenerateTCCDetail
                    {
                        CreatedBy = pObjGenerateTCCDetail.CreatedBy,
                        CreatedDate = pObjGenerateTCCDetail.CreatedDate
                    };
                }

                mObjInsertUpdateGenerateTCCDetail.RequestID = pObjGenerateTCCDetail.RequestID;
                mObjInsertUpdateGenerateTCCDetail.Notes = pObjGenerateTCCDetail.Notes;


                if (pObjGenerateTCCDetail.GTCCDetailID == 0)
                {
                    _db.MAP_TCCRequest_GenerateTCCDetail.Add(mObjInsertUpdateGenerateTCCDetail);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;

                    if (pObjGenerateTCCDetail.GTCCDetailID == 0)
                    {
                        mObjFuncResponse.Message = "Generated TCC Details Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Generated TCC Details Successfully";
                    }
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;

                    mObjFuncResponse.Exception = ex;

                    if (pObjGenerateTCCDetail.GTCCDetailID == 0)
                    {
                        mObjFuncResponse.Message = "Generating of TCC Details Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Generating of TCC Details Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public MAP_TCCRequest_GenerateTCCDetail REP_GetGenerateTCCDetailDetails(long plngRequestID)
        {
            using (_db = new EIRSEntities())
            {
                var vData = (from oa in _db.MAP_TCCRequest_GenerateTCCDetail
                             where oa.RequestID == plngRequestID
                             select oa).FirstOrDefault();

                return vData;
            }
        }

        public FuncResponse REP_InsertUpdatePrepareTCCDraft(MAP_TCCRequest_PrepareTCCDraft pObjPrepareTCCDraft)
        {
            using (_db = new EIRSEntities())
            {
                MAP_TCCRequest_PrepareTCCDraft mObjInsertUpdatePrepareTCCDraft;
                FuncResponse mObjFuncResponse = new FuncResponse();

                if (pObjPrepareTCCDraft.PTCCDraftID != 0)
                {
                    mObjInsertUpdatePrepareTCCDraft = (from oa in _db.MAP_TCCRequest_PrepareTCCDraft
                                                       where oa.PTCCDraftID == pObjPrepareTCCDraft.PTCCDraftID
                                                       select oa).FirstOrDefault();

                    if (mObjInsertUpdatePrepareTCCDraft != null)
                    {
                        mObjInsertUpdatePrepareTCCDraft.ModifiedBy = pObjPrepareTCCDraft.ModifiedBy;
                        mObjInsertUpdatePrepareTCCDraft.ModifiedDate = pObjPrepareTCCDraft.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdatePrepareTCCDraft = new MAP_TCCRequest_PrepareTCCDraft
                        {
                            CreatedBy = pObjPrepareTCCDraft.CreatedBy,
                            CreatedDate = pObjPrepareTCCDraft.CreatedDate
                        };
                    }
                }
                else
                {
                    mObjInsertUpdatePrepareTCCDraft = new MAP_TCCRequest_PrepareTCCDraft
                    {
                        CreatedBy = pObjPrepareTCCDraft.CreatedBy,
                        CreatedDate = pObjPrepareTCCDraft.CreatedDate
                    };
                }

                mObjInsertUpdatePrepareTCCDraft.RequestID = pObjPrepareTCCDraft.RequestID;
                mObjInsertUpdatePrepareTCCDraft.Notes = pObjPrepareTCCDraft.Notes;


                if (pObjPrepareTCCDraft.PTCCDraftID == 0)
                {
                    _db.MAP_TCCRequest_PrepareTCCDraft.Add(mObjInsertUpdatePrepareTCCDraft);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;

                    if (pObjPrepareTCCDraft.PTCCDraftID == 0)
                    {
                        mObjFuncResponse.Message = "Prepared TCC Draft Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Prepared TCC Draft Successfully";
                    }
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;

                    mObjFuncResponse.Exception = ex;

                    if (pObjPrepareTCCDraft.PTCCDraftID == 0)
                    {
                        mObjFuncResponse.Message = "Preparing of TCC Draft Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Preparing of TCC Draft Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public MAP_TCCRequest_PrepareTCCDraft REP_GetPrepareTCCDraftDetails(long plngRequestID)
        {
            using (_db = new EIRSEntities())
            {
                var vData = (from oa in _db.MAP_TCCRequest_PrepareTCCDraft
                             where oa.RequestID == plngRequestID
                             select oa).FirstOrDefault();

                return vData;
            }
        }

        public MAP_TCCRequest_Generate REP_GetTCCRequestGenerateDetails(long plngTCCRequestID)
        {
            using (_db = new EIRSEntities())
            {
                var vData = (from or in _db.MAP_TCCRequest_Generate
                             where or.RequestID == plngTCCRequestID
                             select or).FirstOrDefault();

                return vData;
            }
        }

        public usp_GetTCCDetailForGenerate_Result REP_GetTCCDetailForGenerateProcess(TaxClearanceCertificate pObjTaxClearanceCertificate)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTCCDetailForGenerate(pObjTaxClearanceCertificate.TaxPayerID, pObjTaxClearanceCertificate.TaxPayerTypeID, pObjTaxClearanceCertificate.TaxYear, pObjTaxClearanceCertificate.RequestRefNo).FirstOrDefault();
            }
        }

        public FuncResponse<MAP_TCCRequest_Generate> REP_InsertUpdateTCCRequestGenerate(MAP_TCCRequest_Generate pObjTCCRequestGenerate)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<MAP_TCCRequest_Generate> mObjFuncResponse = new FuncResponse<MAP_TCCRequest_Generate>();
                TCC_Request mObjUpdateTCCRequest;
                MAP_TCCRequest_Generate mObjUpdateTCCRequestGenerate;


                if (pObjTCCRequestGenerate.RGID != 0)
                {
                    mObjUpdateTCCRequestGenerate = (from mog in _db.MAP_TCCRequest_Generate
                                                    where mog.RGID == pObjTCCRequestGenerate.RGID
                                                    select mog).FirstOrDefault();

                    if (mObjUpdateTCCRequestGenerate != null)
                    {
                        mObjUpdateTCCRequestGenerate.ModifiedBy = pObjTCCRequestGenerate.ModifiedBy;
                        mObjUpdateTCCRequestGenerate.ModifiedDate = pObjTCCRequestGenerate.ModifiedDate;
                    }
                    else
                    {
                        mObjUpdateTCCRequestGenerate = new MAP_TCCRequest_Generate();
                        mObjUpdateTCCRequestGenerate.CreatedBy = pObjTCCRequestGenerate.CreatedBy;
                        mObjUpdateTCCRequestGenerate.CreatedDate = pObjTCCRequestGenerate.CreatedDate;
                    }
                }
                else
                {
                    mObjUpdateTCCRequestGenerate = new MAP_TCCRequest_Generate();
                    mObjUpdateTCCRequestGenerate.CreatedBy = pObjTCCRequestGenerate.CreatedBy;
                    mObjUpdateTCCRequestGenerate.CreatedDate = pObjTCCRequestGenerate.CreatedDate;
                }

                mObjUpdateTCCRequestGenerate.RequestID = pObjTCCRequestGenerate.RequestID;
                mObjUpdateTCCRequestGenerate.Reason = pObjTCCRequestGenerate.Reason ?? mObjUpdateTCCRequestGenerate.Reason;
                mObjUpdateTCCRequestGenerate.Location = pObjTCCRequestGenerate.Location ?? mObjUpdateTCCRequestGenerate.Location;
                mObjUpdateTCCRequestGenerate.ExpiryDate = pObjTCCRequestGenerate.ExpiryDate ?? mObjUpdateTCCRequestGenerate.ExpiryDate;
                mObjUpdateTCCRequestGenerate.IsExpirable = pObjTCCRequestGenerate.IsExpirable ?? mObjUpdateTCCRequestGenerate.IsExpirable;

                if (pObjTCCRequestGenerate.RGID == 0)
                {
                    _db.MAP_TCCRequest_Generate.Add(mObjUpdateTCCRequestGenerate);
                }
                try
                {
                    _db.SaveChanges();

                    mObjUpdateTCCRequest = (from or in _db.TCC_Request where (or.TCCRequestID == pObjTCCRequestGenerate.RequestID) select or).FirstOrDefault();
                    if (pObjTCCRequestGenerate.IsAction)
                    {
                        mObjUpdateTCCRequest.StatusID = (int)EnumList.TCCRequestStatus.Generated_eTCC;
                        mObjUpdateTCCRequest.SEDE_DocumentID = pObjTCCRequestGenerate.SEDE_DocumentID;
                        mObjUpdateTCCRequest.GeneratedPath = pObjTCCRequestGenerate.GeneratedPath;

                        MAP_TCCRequest_Stages mObjUpdateRequestStage = (from os in _db.MAP_TCCRequest_Stages
                                                                        where os.RequestID == pObjTCCRequestGenerate.RequestID
                                                                        && os.StageID == pObjTCCRequestGenerate.StageID
                                                                        select os).FirstOrDefault();

                        if (mObjUpdateRequestStage != null)
                        {
                            mObjUpdateRequestStage.ApprovalDate = pObjTCCRequestGenerate.ApprovalDate;
                        }

                        _db.SaveChanges();


                    }
                    else //Update PDF Template ID
                    {
                        mObjUpdateTCCRequest.PDFTemplateID = pObjTCCRequestGenerate.PDFTemplateID ?? mObjUpdateTCCRequest.PDFTemplateID;
                        _db.SaveChanges();
                    }

                    mObjFuncResponse.Success = true;

                    if (pObjTCCRequestGenerate.RGID == 0)
                    {
                        mObjFuncResponse.Message = "eTCC Generated Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "eTCC Updated Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjUpdateTCCRequestGenerate;
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;

                    mObjFuncResponse.Exception = ex;

                    if (pObjTCCRequestGenerate.RGID == 0)
                    {
                        mObjFuncResponse.Message = "eTCC Generation Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "eTCC Updation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_InsertUpdateGenerateField(MAP_TCCRequest_Generate_Field pObjGenerateField)
        {
            using (_db = new EIRSEntities())
            {
                bool isNewRecord = false;
                FuncResponse mObjFuncResponse = new FuncResponse();
                MAP_TCCRequest_Generate_Field mObjInsertUpdateGenerateField;
                var vExist = (from gf in _db.MAP_TCCRequest_Generate_Field
                              where gf.FieldID == pObjGenerateField.FieldID
                              && gf.RGID == pObjGenerateField.RGID
                              select gf);

                if (vExist.Count() > 0)
                {
                    mObjInsertUpdateGenerateField = vExist.First();
                }
                else
                {
                    isNewRecord = true;
                    mObjInsertUpdateGenerateField = new MAP_TCCRequest_Generate_Field();
                }

                mObjInsertUpdateGenerateField.RGID = pObjGenerateField.RGID;
                mObjInsertUpdateGenerateField.FieldID = pObjGenerateField.FieldID;
                mObjInsertUpdateGenerateField.PFID = pObjGenerateField.PFID;
                mObjInsertUpdateGenerateField.FieldValue = pObjGenerateField.FieldValue;
                mObjInsertUpdateGenerateField.Active = pObjGenerateField.Active;
                mObjInsertUpdateGenerateField.CreatedBy = pObjGenerateField.CreatedBy;
                mObjInsertUpdateGenerateField.CreatedDate = pObjGenerateField.CreatedDate;

                if (isNewRecord)
                {
                    _db.MAP_TCCRequest_Generate_Field.Add(mObjInsertUpdateGenerateField);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                }
                catch (Exception e)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = e.Message;
                }

                return mObjFuncResponse;
            }
        }

        public MAP_TCCRequest_Validate REP_GetTCCRequestValidateDetails(long plngTCCRequestID)
        {
            using (_db = new EIRSEntities())
            {
                var vData = (from ov in _db.MAP_TCCRequest_Validate
                             where ov.RequestID == plngTCCRequestID
                             select ov).FirstOrDefault();

                return vData;
            }
        }

        public FuncResponse<MAP_TCCRequest_Validate> REP_InsertUpdateTCCRequestValidate(MAP_TCCRequest_Validate pObjTCCRequestValidate)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<MAP_TCCRequest_Validate> mObjFuncResponse = new FuncResponse<MAP_TCCRequest_Validate>();
                TCC_Request mObjUpdateTCCRequest;
                MAP_TCCRequest_Validate mObjUpdateTCCRequestValidate;


                if (pObjTCCRequestValidate.RVID != 0)
                {
                    mObjUpdateTCCRequestValidate = (from mog in _db.MAP_TCCRequest_Validate
                                                    where mog.RVID == pObjTCCRequestValidate.RVID
                                                    select mog).FirstOrDefault();

                    if (mObjUpdateTCCRequestValidate != null)
                    {
                        mObjUpdateTCCRequestValidate.ModifiedBy = pObjTCCRequestValidate.ModifiedBy;
                        mObjUpdateTCCRequestValidate.ModifiedDate = pObjTCCRequestValidate.ModifiedDate;
                    }
                    else
                    {
                        mObjUpdateTCCRequestValidate = new MAP_TCCRequest_Validate();
                        mObjUpdateTCCRequestValidate.CreatedBy = pObjTCCRequestValidate.CreatedBy;
                        mObjUpdateTCCRequestValidate.CreatedDate = pObjTCCRequestValidate.CreatedDate;
                    }
                }
                else
                {
                    mObjUpdateTCCRequestValidate = new MAP_TCCRequest_Validate();
                    mObjUpdateTCCRequestValidate.CreatedBy = pObjTCCRequestValidate.CreatedBy;
                    mObjUpdateTCCRequestValidate.CreatedDate = pObjTCCRequestValidate.CreatedDate;
                }


                mObjUpdateTCCRequestValidate.RequestID = pObjTCCRequestValidate.RequestID;
                mObjUpdateTCCRequestValidate.Notes = pObjTCCRequestValidate.Notes ?? mObjUpdateTCCRequestValidate.Notes;


                if (pObjTCCRequestValidate.RVID == 0)
                {
                    _db.MAP_TCCRequest_Validate.Add(mObjUpdateTCCRequestValidate);
                }
                try
                {
                    _db.SaveChanges();

                    if (pObjTCCRequestValidate.IsAction)
                    {
                        mObjUpdateTCCRequest = (from or in _db.TCC_Request where (or.TCCRequestID == pObjTCCRequestValidate.RequestID) select or).FirstOrDefault();

                        if (mObjUpdateTCCRequest != null)
                        {
                            mObjUpdateTCCRequest.StatusID = (int)EnumList.TCCRequestStatus.Validated_eTCC;
                            mObjUpdateTCCRequest.SEDE_OrderID = pObjTCCRequestValidate.SEDE_OrderID;
                            mObjUpdateTCCRequest.ValidatedPath = pObjTCCRequestValidate.ValidatedPath;

                        }




                        MAP_TCCRequest_Stages mObjUpdateRequestStage = (from os in _db.MAP_TCCRequest_Stages
                                                                        where os.RequestID == pObjTCCRequestValidate.RequestID
                                                                        && os.StageID == pObjTCCRequestValidate.StageID
                                                                        select os).FirstOrDefault();

                        if (mObjUpdateRequestStage != null)
                        {
                            mObjUpdateRequestStage.ApprovalDate = pObjTCCRequestValidate.ApprovalDate;
                        }

                        _db.SaveChanges();

                    }

                    mObjFuncResponse.Success = true;

                    if (pObjTCCRequestValidate.RVID == 0)
                    {
                        mObjFuncResponse.Message = "eTCC Validated Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "eTCC Updated Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjUpdateTCCRequestValidate;
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;

                    mObjFuncResponse.Exception = ex;

                    if (pObjTCCRequestValidate.RVID == 0)
                    {
                        mObjFuncResponse.Message = "eTCC Validation Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "eTCC Validation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public MAP_TCCRequest_SignVisible REP_GetTCCRequestSignVisibleDetails(long plngTCCRequestID, int UserID, int StageID = 0)
        {
            using (_db = new EIRSEntities())
            {
                var vData = (from ov in _db.MAP_TCCRequest_SignVisible
                             where ov.RequestID == plngTCCRequestID
                             && ov.UserID == UserID
                             && ov.StageID == StageID
                             select ov).FirstOrDefault();

                return vData;
            }
        }

        public IList<MAP_TCCRequest_SignVisible> REP_GetTCCRequestSignVisibleList(long plngTCCRequestID)
        {
            using (_db = new EIRSEntities())
            {
                var vData = (from ov in _db.MAP_TCCRequest_SignVisible
                             where ov.RequestID == plngTCCRequestID
                             select ov).ToList();

                return vData;
            }
        }

        public FuncResponse<MAP_TCCRequest_SignVisible> REP_InsertUpdateTCCRequestSignVisible(MAP_TCCRequest_SignVisible pObjTCCRequestSignVisible)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<MAP_TCCRequest_SignVisible> mObjFuncResponse = new FuncResponse<MAP_TCCRequest_SignVisible>();
                TCC_Request mObjUpdateTCCRequest;
                MAP_TCCRequest_SignVisible mObjUpdateTCCRequestSignVisible;


                if (pObjTCCRequestSignVisible.RSVID != 0)
                {
                    mObjUpdateTCCRequestSignVisible = (from mog in _db.MAP_TCCRequest_SignVisible
                                                       where mog.RSVID == pObjTCCRequestSignVisible.RSVID
                                                       select mog).FirstOrDefault();

                    if (mObjUpdateTCCRequestSignVisible != null)
                    {
                        mObjUpdateTCCRequestSignVisible.ModifiedBy = pObjTCCRequestSignVisible.ModifiedBy;
                        mObjUpdateTCCRequestSignVisible.ModifiedDate = pObjTCCRequestSignVisible.ModifiedDate;
                    }
                    else
                    {
                        mObjUpdateTCCRequestSignVisible = new MAP_TCCRequest_SignVisible();
                        mObjUpdateTCCRequestSignVisible.CreatedBy = pObjTCCRequestSignVisible.CreatedBy;
                        mObjUpdateTCCRequestSignVisible.CreatedDate = pObjTCCRequestSignVisible.CreatedDate;
                    }
                }
                else
                {
                    mObjUpdateTCCRequestSignVisible = new MAP_TCCRequest_SignVisible();
                    mObjUpdateTCCRequestSignVisible.CreatedBy = pObjTCCRequestSignVisible.CreatedBy;
                    mObjUpdateTCCRequestSignVisible.CreatedDate = pObjTCCRequestSignVisible.CreatedDate;
                }


                mObjUpdateTCCRequestSignVisible.RequestID = pObjTCCRequestSignVisible.RequestID;
                mObjUpdateTCCRequestSignVisible.UserID = pObjTCCRequestSignVisible.UserID;
                mObjUpdateTCCRequestSignVisible.SignDate = pObjTCCRequestSignVisible.SignDate;
                mObjUpdateTCCRequestSignVisible.Notes = pObjTCCRequestSignVisible.Notes ?? mObjUpdateTCCRequestSignVisible.Notes;
                mObjUpdateTCCRequestSignVisible.SignSourceID = pObjTCCRequestSignVisible.SignSourceID;
                mObjUpdateTCCRequestSignVisible.DocumentWidth = pObjTCCRequestSignVisible.DocumentWidth;
                mObjUpdateTCCRequestSignVisible.AdditionalSignatureLocation = pObjTCCRequestSignVisible.AdditionalSignatureLocation;

                if (pObjTCCRequestSignVisible.RSVID == 0)
                {
                    _db.MAP_TCCRequest_SignVisible.Add(mObjUpdateTCCRequestSignVisible);
                }
                try
                {
                    _db.SaveChanges();

                    if (pObjTCCRequestSignVisible.IsAction)
                    {
                        mObjUpdateTCCRequest = (from or in _db.TCC_Request where (or.TCCRequestID == pObjTCCRequestSignVisible.RequestID) select or).FirstOrDefault();

                        if (mObjUpdateTCCRequest != null)
                        {

                            if (mObjUpdateTCCRequest.VisibleSignStatusID == 2)
                            {
                                mObjUpdateTCCRequest.StatusID = (int)EnumList.TCCRequestStatus.Signed_eTCC_Visible;


                                MAP_TCCRequest_Stages mObjUpdateRequestStage = (from os in _db.MAP_TCCRequest_Stages
                                                                                where os.RequestID == pObjTCCRequestSignVisible.RequestID
                                                                                && os.StageID == pObjTCCRequestSignVisible.Request_StageID
                                                                                select os).FirstOrDefault();

                                if (mObjUpdateRequestStage != null)
                                {
                                    mObjUpdateRequestStage.ApprovalDate = pObjTCCRequestSignVisible.ApprovalDate;
                                }
                            }

                            mObjUpdateTCCRequest.VisibleSignStatusID = mObjUpdateTCCRequest.VisibleSignStatusID.GetValueOrDefault() + 1;
                            mObjUpdateTCCRequest.SignedVisiblePath = pObjTCCRequestSignVisible.SignedVisiblePath;
                            mObjUpdateTCCRequestSignVisible.StageID = mObjUpdateTCCRequest.VisibleSignStatusID;

                            _db.SaveChanges();
                        }
                    }

                    mObjFuncResponse.Success = true;

                    if (pObjTCCRequestSignVisible.RSVID == 0)
                    {
                        mObjFuncResponse.Message = "eTCC Signed Visible Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "eTCC Signed Visible Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjUpdateTCCRequestSignVisible;
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;

                    mObjFuncResponse.Exception = ex;

                    if (pObjTCCRequestSignVisible.RSVID == 0)
                    {
                        mObjFuncResponse.Message = "eTCC Signed Visible Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "eTCC Signed Visible Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public MAP_TCCRequest_SignDigital REP_GetTCCRequestSignDigitalDetails(long plngTCCRequestID)
        {
            using (_db = new EIRSEntities())
            {
                var vData = (from ov in _db.MAP_TCCRequest_SignDigital
                             where ov.RequestID == plngTCCRequestID
                             select ov).FirstOrDefault();

                return vData;
            }
        }

        public FuncResponse<MAP_TCCRequest_SignDigital> REP_InsertUpdateTCCRequestSignDigital(MAP_TCCRequest_SignDigital pObjTCCRequestSignDigital)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<MAP_TCCRequest_SignDigital> mObjFuncResponse = new FuncResponse<MAP_TCCRequest_SignDigital>();
                TCC_Request mObjUpdateTCCRequest;
                MAP_TCCRequest_SignDigital mObjUpdateTCCRequestSignDigital;


                if (pObjTCCRequestSignDigital.RSDID != 0)
                {
                    mObjUpdateTCCRequestSignDigital = (from mog in _db.MAP_TCCRequest_SignDigital
                                                       where mog.RSDID == pObjTCCRequestSignDigital.RSDID
                                                       select mog).FirstOrDefault();

                    if (mObjUpdateTCCRequestSignDigital != null)
                    {
                        mObjUpdateTCCRequestSignDigital.ModifiedBy = pObjTCCRequestSignDigital.ModifiedBy;
                        mObjUpdateTCCRequestSignDigital.ModifiedDate = pObjTCCRequestSignDigital.ModifiedDate;
                    }
                    else
                    {
                        mObjUpdateTCCRequestSignDigital = new MAP_TCCRequest_SignDigital();
                        mObjUpdateTCCRequestSignDigital.CreatedBy = pObjTCCRequestSignDigital.CreatedBy;
                        mObjUpdateTCCRequestSignDigital.CreatedDate = pObjTCCRequestSignDigital.CreatedDate;
                    }
                }
                else
                {
                    mObjUpdateTCCRequestSignDigital = new MAP_TCCRequest_SignDigital();
                    mObjUpdateTCCRequestSignDigital.CreatedBy = pObjTCCRequestSignDigital.CreatedBy;
                    mObjUpdateTCCRequestSignDigital.CreatedDate = pObjTCCRequestSignDigital.CreatedDate;
                }


                mObjUpdateTCCRequestSignDigital.RequestID = pObjTCCRequestSignDigital.RequestID;
                mObjUpdateTCCRequestSignDigital.Notes = pObjTCCRequestSignDigital.Notes ?? mObjUpdateTCCRequestSignDigital.Notes;


                if (pObjTCCRequestSignDigital.RSDID == 0)
                {
                    _db.MAP_TCCRequest_SignDigital.Add(mObjUpdateTCCRequestSignDigital);
                }
                try
                {
                    _db.SaveChanges();

                    if (pObjTCCRequestSignDigital.IsAction)
                    {
                        mObjUpdateTCCRequest = (from or in _db.TCC_Request where (or.TCCRequestID == pObjTCCRequestSignDigital.RequestID) select or).FirstOrDefault();

                        if (mObjUpdateTCCRequest != null)
                        {
                            mObjUpdateTCCRequest.StatusID = (int)EnumList.TCCRequestStatus.Signed_eTCC_Digital;
                            mObjUpdateTCCRequest.SignedDigitalPath = pObjTCCRequestSignDigital.SignedDigitalPath;

                        }

                        MAP_TCCRequest_Stages mObjUpdateRequestStage = (from os in _db.MAP_TCCRequest_Stages
                                                                        where os.RequestID == pObjTCCRequestSignDigital.RequestID
                                                                        && os.StageID == pObjTCCRequestSignDigital.StageID
                                                                        select os).FirstOrDefault();

                        if (mObjUpdateRequestStage != null)
                        {
                            mObjUpdateRequestStage.ApprovalDate = pObjTCCRequestSignDigital.ApprovalDate;
                        }

                        _db.SaveChanges();

                    }

                    mObjFuncResponse.Success = true;

                    if (pObjTCCRequestSignDigital.RSDID == 0)
                    {
                        mObjFuncResponse.Message = "eTCC Signed Digital Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "eTCC Signed Digital Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjUpdateTCCRequestSignDigital;
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;

                    mObjFuncResponse.Exception = ex;

                    if (pObjTCCRequestSignDigital.RSDID == 0)
                    {
                        mObjFuncResponse.Message = "eTCC Siging Digital Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "eTCC Siging Digital Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public MAP_TCCRequest_Seal REP_GetTCCRequestSealDetails(long plngTCCRequestID)
        {
            using (_db = new EIRSEntities())
            {
                var vData = (from os in _db.MAP_TCCRequest_Seal
                             where os.RequestID == plngTCCRequestID
                             select os).FirstOrDefault();

                return vData;
            }
        }

        public FuncResponse<MAP_TCCRequest_Seal> REP_InsertUpdateTCCRequestSeal(MAP_TCCRequest_Seal pObjTCCRequestSeal)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<MAP_TCCRequest_Seal> mObjFuncResponse = new FuncResponse<MAP_TCCRequest_Seal>();
                TCC_Request mObjUpdateTCCRequest;
                MAP_TCCRequest_Seal mObjUpdateTCCRequestSeal;


                if (pObjTCCRequestSeal.RSID != 0)
                {
                    mObjUpdateTCCRequestSeal = (from mos in _db.MAP_TCCRequest_Seal
                                                where mos.RSID == pObjTCCRequestSeal.RSID
                                                select mos).FirstOrDefault();

                    if (mObjUpdateTCCRequestSeal != null)
                    {
                        mObjUpdateTCCRequestSeal.ModifiedBy = pObjTCCRequestSeal.ModifiedBy;
                        mObjUpdateTCCRequestSeal.ModifiedDate = pObjTCCRequestSeal.ModifiedDate;
                    }
                    else
                    {
                        mObjUpdateTCCRequestSeal = new MAP_TCCRequest_Seal();
                        mObjUpdateTCCRequestSeal.CreatedBy = pObjTCCRequestSeal.CreatedBy;
                        mObjUpdateTCCRequestSeal.CreatedDate = pObjTCCRequestSeal.CreatedDate;
                    }
                }
                else
                {
                    mObjUpdateTCCRequestSeal = new MAP_TCCRequest_Seal();
                    mObjUpdateTCCRequestSeal.CreatedBy = pObjTCCRequestSeal.CreatedBy;
                    mObjUpdateTCCRequestSeal.CreatedDate = pObjTCCRequestSeal.CreatedDate;
                }


                mObjUpdateTCCRequestSeal.RequestID = pObjTCCRequestSeal.RequestID;
                mObjUpdateTCCRequestSeal.Notes = pObjTCCRequestSeal.Notes ?? mObjUpdateTCCRequestSeal.Notes;

                if (pObjTCCRequestSeal.RSID == 0)
                {
                    _db.MAP_TCCRequest_Seal.Add(mObjUpdateTCCRequestSeal);
                }
                try
                {
                    _db.SaveChanges();
                    if (pObjTCCRequestSeal.IsAction)
                    {
                        mObjUpdateTCCRequest = (from or in _db.TCC_Request where (or.TCCRequestID == pObjTCCRequestSeal.RequestID) select or).FirstOrDefault();

                        if (mObjUpdateTCCRequest != null)
                        {
                            mObjUpdateTCCRequest.StatusID = (int)EnumList.TCCRequestStatus.Sealed_eTCC;
                            mObjUpdateTCCRequest.SealedPath = pObjTCCRequestSeal.SealedPath;
                        }

                        MAP_TCCRequest_Stages mObjUpdateRequestStage = (from os in _db.MAP_TCCRequest_Stages
                                                                        where os.RequestID == pObjTCCRequestSeal.RequestID
                                                                        && os.StageID == pObjTCCRequestSeal.StageID
                                                                        select os).FirstOrDefault();

                        if (mObjUpdateRequestStage != null)
                        {
                            mObjUpdateRequestStage.ApprovalDate = pObjTCCRequestSeal.ApprovalDate;
                        }

                        _db.SaveChanges();


                    }

                    mObjFuncResponse.Success = true;

                    if (pObjTCCRequestSeal.RSID == 0)
                    {
                        mObjFuncResponse.Message = "eTCC Sealed Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "eTCC Updated Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjUpdateTCCRequestSeal;
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;

                    mObjFuncResponse.Exception = ex;

                    if (pObjTCCRequestSeal.RSID == 0)
                    {
                        mObjFuncResponse.Message = "eTCC Seal Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "eTCC Updation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public MAP_TCCRequest_Issue REP_GetTCCRequestIssueDetails(long plngTCCRequestID)
        {
            using (_db = new EIRSEntities())
            {
                var vData = (from os in _db.MAP_TCCRequest_Issue
                             where os.RequestID == plngTCCRequestID
                             select os).FirstOrDefault();

                return vData;
            }
        }

        public FuncResponse<MAP_TCCRequest_Issue> REP_InsertUpdateTCCRequestIssue(MAP_TCCRequest_Issue pObjTCCRequestIssue)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<MAP_TCCRequest_Issue> mObjFuncResponse = new FuncResponse<MAP_TCCRequest_Issue>();
                TCC_Request mObjUpdateTCCRequest;
                MAP_TCCRequest_Issue mObjUpdateTCCRequestIssue;


                if (pObjTCCRequestIssue.RIID != 0)
                {
                    mObjUpdateTCCRequestIssue = (from mos in _db.MAP_TCCRequest_Issue
                                                 where mos.RIID == pObjTCCRequestIssue.RIID
                                                 select mos).FirstOrDefault();

                    if (mObjUpdateTCCRequestIssue != null)
                    {
                        mObjUpdateTCCRequestIssue.ModifiedBy = pObjTCCRequestIssue.ModifiedBy;
                        mObjUpdateTCCRequestIssue.ModifiedDate = pObjTCCRequestIssue.ModifiedDate;
                    }
                    else
                    {
                        mObjUpdateTCCRequestIssue = new MAP_TCCRequest_Issue();
                        mObjUpdateTCCRequestIssue.CreatedBy = pObjTCCRequestIssue.CreatedBy;
                        mObjUpdateTCCRequestIssue.CreatedDate = pObjTCCRequestIssue.CreatedDate;
                    }
                }
                else
                {
                    mObjUpdateTCCRequestIssue = new MAP_TCCRequest_Issue();
                    mObjUpdateTCCRequestIssue.CreatedBy = pObjTCCRequestIssue.CreatedBy;
                    mObjUpdateTCCRequestIssue.CreatedDate = pObjTCCRequestIssue.CreatedDate;
                }


                mObjUpdateTCCRequestIssue.RequestID = pObjTCCRequestIssue.RequestID;
                mObjUpdateTCCRequestIssue.Notes = pObjTCCRequestIssue.Notes ?? mObjUpdateTCCRequestIssue.Notes;

                if (pObjTCCRequestIssue.RIID == 0)
                {
                    _db.MAP_TCCRequest_Issue.Add(mObjUpdateTCCRequestIssue);
                }
                try
                {
                    _db.SaveChanges();
                    if (pObjTCCRequestIssue.IsAction)
                    {
                        mObjUpdateTCCRequest = (from or in _db.TCC_Request where (or.TCCRequestID == pObjTCCRequestIssue.RequestID) select or).FirstOrDefault();

                        if (mObjUpdateTCCRequest != null)
                        {
                            mObjUpdateTCCRequest.StatusID = (int)EnumList.TCCRequestStatus.Issued_eTCC;
                        }

                        MAP_TCCRequest_Stages mObjUpdateRequestStage = (from os in _db.MAP_TCCRequest_Stages
                                                                        where os.RequestID == pObjTCCRequestIssue.RequestID
                                                                        && os.StageID == pObjTCCRequestIssue.StageID
                                                                        select os).FirstOrDefault();

                        if (mObjUpdateRequestStage != null)
                        {
                            mObjUpdateRequestStage.ApprovalDate = pObjTCCRequestIssue.ApprovalDate;
                        }

                        _db.SaveChanges();


                    }

                    mObjFuncResponse.Success = true;

                    if (pObjTCCRequestIssue.RIID == 0)
                    {
                        mObjFuncResponse.Message = "eTCC Issued Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "eTCC Issued Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjUpdateTCCRequestIssue;
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;

                    mObjFuncResponse.Exception = ex;

                    if (pObjTCCRequestIssue.RIID == 0)
                    {
                        mObjFuncResponse.Message = "eTCC Issue Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "eTCC Issue Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }


        /***** TCC - Tax Payer Details *********************/
        public IList<usp_GetTaxPayerAssetForTCC_Result> REP_GetTaxPayerAssetList(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerAssetForTCC(pIntTaxPayerID, pIntTaxPayerTypeID).ToList();
            }
        }

        public IList<usp_GetTaxPayerProfileInformationForTCC_Result> REP_GetTaxPayerProfileList(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerProfileInformationForTCC(pIntTaxPayerID, pIntTaxPayerTypeID).ToList();
            }
        }

        public IList<usp_GetAssessmentRuleInformationForTCC_Result> REP_GetTaxPayerAssessmentRuleList(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentRuleInformationForTCC(pIntTaxPayerID, pIntTaxPayerTypeID).ToList();
            }
        }

        public IList<usp_GetTaxPayerBillForTCC_Result> REP_GetTaxPayerBillList(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerBillForTCC(pIntTaxPayerID, pIntTaxPayerTypeID).ToList();
            }
        }

        public IList<usp_GetTaxPayerPaymentForTCC_Result> REP_GetTaxPayerPaymentList(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerPaymentForTCC(pIntTaxPayerID, pIntTaxPayerTypeID).ToList();
            }
        }
        public IList<usp_GetTaxPayerPaymentForTCCNEW_Result> REP_GetTaxPayerPaymentListNEW(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerPaymentForTCCNEW(pIntTaxPayerID, pIntTaxPayerTypeID).ToList();
            }
        }

        public IList<usp_GetTaxPayerLiabilityForTCC_Result> REP_GetTaxPayerLiabilityForTCC(int pIntTaxPayerID, int pIntTaxPayerTypeID, int pIntTaxYear)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerLiabilityForTCC(pIntTaxPayerTypeID, pIntTaxPayerID, pIntTaxYear).ToList();
            }
        }

        /******************* Notes ******************************/
        public IList<usp_GetRequestNotesList_Result> REP_GetRequestNotesList(MAP_TCCRequest_Notes pObjRequestNotes)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetRequestNotesList(pObjRequestNotes.RequestID).ToList();
            }
        }

        public FuncResponse<MAP_TCCRequest_Notes> REP_InsertRequestNotes(MAP_TCCRequest_Notes pObjRequestNotes)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<MAP_TCCRequest_Notes> mObjFuncResponse = new FuncResponse<MAP_TCCRequest_Notes>();


                MAP_TCCRequest_Notes mObjInsertUpdateRequestNotes;
                mObjInsertUpdateRequestNotes = new MAP_TCCRequest_Notes();
                mObjInsertUpdateRequestNotes.RequestID = pObjRequestNotes.RequestID;
                mObjInsertUpdateRequestNotes.NotesDate = pObjRequestNotes.NotesDate;
                mObjInsertUpdateRequestNotes.Notes = pObjRequestNotes.Notes;
                mObjInsertUpdateRequestNotes.StaffID = pObjRequestNotes.StaffID;
                mObjInsertUpdateRequestNotes.StageID = pObjRequestNotes.StageID;

                mObjInsertUpdateRequestNotes.CreatedBy = pObjRequestNotes.CreatedBy;
                mObjInsertUpdateRequestNotes.CreatedDate = pObjRequestNotes.CreatedDate;

                _db.MAP_TCCRequest_Notes.Add(mObjInsertUpdateRequestNotes);

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.AdditionalData = mObjInsertUpdateRequestNotes;
                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.Message = "Notes Added Successfully";

                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = ex;
                    mObjFuncResponse.Message = "Notes Addition Failed";

                }

                return mObjFuncResponse;
            }
        }

        public MAP_TCCRequest_Notes_Document REP_AddNotesDocument(MAP_TCCRequest_Notes pObjRequestNotes, MAP_TCCRequest_Notes_Document pObjNotesDocument)
        {
            using (_db = new EIRSEntities())
            {
                MAP_TCCRequest_Notes mObjOrderNotes = _db.MAP_TCCRequest_Notes.Find(pObjRequestNotes.RNID);
                mObjOrderNotes.MAP_TCCRequest_Notes_Document.Add(pObjNotesDocument);
                _db.SaveChanges();

                pObjNotesDocument.DocumentName = pObjNotesDocument.DocumentName;
                string strDocumentName = "ND_" + pObjRequestNotes.RNID.ToString() + "_" + pObjNotesDocument.RNDID + "_" + DateTime.Now.ToString("dd_MM_yyyy_ss") + Path.GetExtension(pObjNotesDocument.DocumentName);
                pObjNotesDocument.DocumentPath = "TCC/" + pObjRequestNotes.RequestID.ToString() + "/Notes/" + strDocumentName;
                _db.SaveChanges();

                return pObjNotesDocument;
            }
        }

        public IList<MAP_TCCRequest_Notes_Document> REP_GetNotesDocumentList(long plngRNID)
        {
            using (_db = new EIRSEntities())
            {
                var vNotesDocument = (from doc in _db.MAP_TCCRequest_Notes_Document
                                      where doc.RNID == plngRNID
                                      select doc).ToList();

                return vNotesDocument;
            }
        }

        /******************* Revoke TCC **************************/
        public FuncResponse REP_RevokeTCC(MAP_TCCRequest_Revoke pObjRevoke)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();

                TCC_Request mObjTCCRequest = _db.TCC_Request.Find(pObjRevoke.RequestID);

                if (mObjTCCRequest != null)
                {
                    mObjTCCRequest.StatusID = (int)EnumList.TCCRequestStatus.Paid;
                    mObjTCCRequest.VisibleSignStatusID = null;
                    mObjTCCRequest.PDFTemplateID = null;
                    mObjTCCRequest.GeneratedPath = null;
                    mObjTCCRequest.ValidatedPath = null;
                    mObjTCCRequest.SignedVisiblePath = null;
                    mObjTCCRequest.SignedDigitalPath = null;
                    mObjTCCRequest.SealedPath = null;
                    mObjTCCRequest.SEDE_DocumentID = null;
                    mObjTCCRequest.SEDE_OrderID = null;

                    _db.MAP_TCCRequest_Revoke.Add(pObjRevoke);

                    //Remove All Related Stage Data
                    var lstRequestIncomeStream = _db.MAP_TCCRequest_IncomeStream.Where(t => t.TCCRequestID == pObjRevoke.RequestID);
                    if (lstRequestIncomeStream != null)
                    {
                        _db.MAP_TCCRequest_IncomeStream.RemoveRange(lstRequestIncomeStream);
                    }

                    MAP_TCCRequest_ValidateTaxPayerInformation validateTaxPayerInformation = _db.MAP_TCCRequest_ValidateTaxPayerInformation.Where(t => t.RequestID == pObjRevoke.RequestID).FirstOrDefault();
                    if (validateTaxPayerInformation != null)
                    {
                        _db.MAP_TCCRequest_ValidateTaxPayerInformation.Remove(validateTaxPayerInformation);
                    }

                    MAP_TCCRequest_ValidateTaxPayerIncome validateTaxPayerIncome = _db.MAP_TCCRequest_ValidateTaxPayerIncome.Where(t => t.RequestID == pObjRevoke.RequestID).FirstOrDefault();
                    if (validateTaxPayerIncome != null)
                    {
                        _db.MAP_TCCRequest_ValidateTaxPayerIncome.Remove(validateTaxPayerIncome);
                    }

                    MAP_TCCRequest_GenerateTCCDetail generateTCCDetail = _db.MAP_TCCRequest_GenerateTCCDetail.Where(t => t.RequestID == pObjRevoke.RequestID).FirstOrDefault();
                    if (generateTCCDetail != null)
                    {
                        _db.MAP_TCCRequest_GenerateTCCDetail.Remove(generateTCCDetail);
                    }

                    MAP_TCCRequest_PrepareTCCDraft prepareTCCDraft = _db.MAP_TCCRequest_PrepareTCCDraft.Where(t => t.RequestID == pObjRevoke.RequestID).FirstOrDefault();
                    if (prepareTCCDraft != null)
                    {
                        _db.MAP_TCCRequest_PrepareTCCDraft.Remove(prepareTCCDraft);
                    }


                    MAP_TCCRequest_Generate mObjRequestGenerate = _db.MAP_TCCRequest_Generate.Where(t => t.RequestID == pObjRevoke.RequestID).FirstOrDefault();
                    if (mObjRequestGenerate != null)
                    {
                        _db.MAP_TCCRequest_Generate.Remove(mObjRequestGenerate);

                        if (!string.IsNullOrWhiteSpace(mObjRequestGenerate.GeneratedPath))
                        {
                            if (File.Exists(GlobalDefaultValues.DocumentLocation + mObjRequestGenerate.GeneratedPath))
                            {
                                File.Delete(GlobalDefaultValues.DocumentLocation + mObjRequestGenerate.GeneratedPath);
                            }
                        }

                        var vGenerateField = _db.MAP_TCCRequest_Generate_Field.Where(t => t.RGID == mObjRequestGenerate.RGID);
                        if (vGenerateField != null)
                        {
                            _db.MAP_TCCRequest_Generate_Field.RemoveRange(vGenerateField);
                        }
                    }

                    MAP_TCCRequest_Validate mObjRequestValidate = _db.MAP_TCCRequest_Validate.Where(t => t.RequestID == pObjRevoke.RequestID).FirstOrDefault();
                    if (mObjRequestValidate != null)
                    {
                        _db.MAP_TCCRequest_Validate.Remove(mObjRequestValidate);
                        //Delete Documents
                        if (!string.IsNullOrWhiteSpace(mObjRequestValidate.ValidatedPath))
                        {
                            if (File.Exists(GlobalDefaultValues.DocumentLocation + mObjRequestValidate.ValidatedPath))
                            {
                                File.Delete(GlobalDefaultValues.DocumentLocation + mObjRequestValidate.ValidatedPath);
                            }
                        }
                    }

                    var lstRequestSignVisible = _db.MAP_TCCRequest_SignVisible.Where(t => t.RequestID == pObjRevoke.RequestID);
                    if (lstRequestSignVisible != null)
                    {
                        _db.MAP_TCCRequest_SignVisible.RemoveRange(lstRequestSignVisible);
                    }

                    MAP_TCCRequest_SignDigital mObjRequestSignDigital = _db.MAP_TCCRequest_SignDigital.Where(t => t.RequestID == pObjRevoke.RequestID).FirstOrDefault();
                    if (mObjRequestSignDigital != null)
                    {
                        _db.MAP_TCCRequest_SignDigital.Remove(mObjRequestSignDigital);

                        //Delete Documents
                        if (!string.IsNullOrWhiteSpace(mObjRequestSignDigital.SignedDigitalPath))
                        {
                            if (File.Exists(GlobalDefaultValues.DocumentLocation + mObjRequestSignDigital.SignedDigitalPath))
                            {
                                File.Delete(GlobalDefaultValues.DocumentLocation + mObjRequestSignDigital.SignedDigitalPath);
                            }
                        }
                    }

                    MAP_TCCRequest_Seal mObjRequestSeal = _db.MAP_TCCRequest_Seal.Where(t => t.RequestID == pObjRevoke.RequestID).FirstOrDefault();
                    if (mObjRequestSeal != null)
                    {
                        _db.MAP_TCCRequest_Seal.Remove(mObjRequestSeal);
                        //Delete Documents
                        if (!string.IsNullOrWhiteSpace(mObjRequestSeal.SealedPath))
                        {
                            if (File.Exists(GlobalDefaultValues.DocumentLocation + mObjRequestSeal.SealedPath))
                            {
                                File.Delete(GlobalDefaultValues.DocumentLocation + mObjRequestSeal.SealedPath);
                            }
                        }
                    }

                    MAP_TCCRequest_Issue mObjRequestIssue = _db.MAP_TCCRequest_Issue.Where(t => t.RequestID == pObjRevoke.RequestID).FirstOrDefault();
                    if (mObjRequestIssue != null)
                    {
                        _db.MAP_TCCRequest_Issue.Remove(mObjRequestIssue);
                    }

                    var vRequestStage = _db.MAP_TCCRequest_Stages.Where(t => t.RequestID == pObjRevoke.RequestID);
                    foreach (var stg in vRequestStage)
                    {
                        stg.ApprovalDate = null;
                    }

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                        mObjFuncResponse.Message = "TCC Request Revoked Successfully";

                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Exception = Ex;
                        mObjFuncResponse.Message = "TCC Request Revoked Failed";
                    }
                }
                else
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Request Not Found";
                }

                return mObjFuncResponse;
            }
        }

        public IList<GetEmployerLiability_Result> REP_GetEmployerLiability(int pIntIndividualID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.GetEmployerLiability(pIntIndividualID).ToList();
            }
        }

        public bool REP_VerifyTCCByReferenceNumber(string pObjReferenceNumber)
        {
            using (_db = new EIRSEntities())
            {
                var vTaxClearanceCertificate = (from tcc in _db.TaxClearanceCertificates
                                                where tcc.TCCNumber + "-" + tcc.SerialNumber == pObjReferenceNumber
                                                select tcc).FirstOrDefault();

                if (vTaxClearanceCertificate != null)
                {
                    var vTCCData = (from tcc in _db.TCC_Request
                                    where tcc.RequestRefNo == vTaxClearanceCertificate.RequestRefNo && tcc.StatusID == (int)EnumList.TCCRequestStage.Issue_eTCC
                                    select tcc).FirstOrDefault();

                    if (vTCCData != null)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

    }
}
