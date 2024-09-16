using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace EIRS.Repository
{
    public class MDAServiceRepository : IMDAServiceRepository
    {
        EIRSEntities _db;

        public FuncResponse<MDA_Services> REP_InsertUpdateMDAService(MDA_Services pObjMDAService)
        {
            using (_db = new EIRSEntities())
            {
                MDA_Services mObjInsertUpdateMDAService; //MDA ServiceInsert Update Object
                FuncResponse<MDA_Services> mObjFuncResponse = new FuncResponse<MDA_Services>(); //Return Object


                //If Update Load MDA_Services
                if (pObjMDAService.MDAServiceID != 0)
                {
                    mObjInsertUpdateMDAService = (from mser in _db.MDA_Services
                                                  where mser.MDAServiceID == pObjMDAService.MDAServiceID
                                                  select mser).FirstOrDefault();

                    if (mObjInsertUpdateMDAService != null)
                    {
                        mObjInsertUpdateMDAService.ModifiedBy = pObjMDAService.ModifiedBy;
                        mObjInsertUpdateMDAService.ModifiedDate = pObjMDAService.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateMDAService = new MDA_Services();
                        mObjInsertUpdateMDAService.CreatedBy = pObjMDAService.CreatedBy;
                        mObjInsertUpdateMDAService.CreatedDate = pObjMDAService.CreatedDate;
                    }
                }
                else // Else Insert MDA_Services
                {
                    mObjInsertUpdateMDAService = new MDA_Services();
                    mObjInsertUpdateMDAService.CreatedBy = pObjMDAService.CreatedBy;
                    mObjInsertUpdateMDAService.CreatedDate = pObjMDAService.CreatedDate;
                }

                mObjInsertUpdateMDAService.MDAServiceName = pObjMDAService.MDAServiceName;
                mObjInsertUpdateMDAService.RuleRunID = pObjMDAService.RuleRunID;
                mObjInsertUpdateMDAService.PaymentFrequencyID = pObjMDAService.PaymentFrequencyID;
                mObjInsertUpdateMDAService.ServiceAmount = pObjMDAService.ServiceAmount;
                mObjInsertUpdateMDAService.TaxYear = pObjMDAService.TaxYear;
                mObjInsertUpdateMDAService.PaymentOptionID = pObjMDAService.PaymentOptionID;
                mObjInsertUpdateMDAService.Active = pObjMDAService.Active;

                if (pObjMDAService.MDAServiceID == 0)
                {
                    _db.MDA_Services.Add(mObjInsertUpdateMDAService);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjMDAService.MDAServiceID == 0)
                    {
                        mObjFuncResponse.Message = "MDA Service Added Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "MDA Service Updated Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjInsertUpdateMDAService;

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjMDAService.MDAServiceID == 0)
                    {
                        mObjFuncResponse.Message = "MDA Service Addition Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "MDA Service Updation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<vw_MDAServices> REP_GetMDAServiceList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.vw_MDAServices.ToList();
            }
        }

        public IList<usp_GetMDAServiceList_Result> REP_GetMDAServiceList(MDA_Services pObjMDAService)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetMDAServiceList(pObjMDAService.MDAServiceID, pObjMDAService.TaxYear, pObjMDAService.MDAServiceName, pObjMDAService.IntStatus).ToList();
            }
        }

        public usp_GetMDAServiceList_Result REP_GetMDAServiceDetails(MDA_Services pObjMDAService)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetMDAServiceList(pObjMDAService.MDAServiceID, pObjMDAService.TaxYear, pObjMDAService.MDAServiceName, pObjMDAService.IntStatus).FirstOrDefault();
            }
        }

        public FuncResponse REP_UpdateStatus(MDA_Services pObjMDAService)
        {
            using (_db = new EIRSEntities())
            {
                MDA_Services mObjInsertUpdateMDAService; //MDA_Services Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load MDA_Services
                if (pObjMDAService.MDAServiceID != 0)
                {
                    mObjInsertUpdateMDAService = (from mser in _db.MDA_Services
                                                  where mser.MDAServiceID == pObjMDAService.MDAServiceID
                                                  select mser).FirstOrDefault();

                    if (mObjInsertUpdateMDAService != null)
                    {
                        mObjInsertUpdateMDAService.Active = !mObjInsertUpdateMDAService.Active;
                        mObjInsertUpdateMDAService.ModifiedBy = pObjMDAService.ModifiedBy;
                        mObjInsertUpdateMDAService.ModifiedDate = pObjMDAService.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "MDA Service Updated Successfully";
                            //  mObjFuncResponse.AdditionalData = _db.usp_GetMDAServiceList(0, pObjMDAService.IntStatus).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "MDA Service Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<DropDownListResult> REP_GetMDAServiceDropDownList(MDA_Services pObjMDAService)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from mser in _db.usp_GetMDAServiceList(pObjMDAService.MDAServiceID, pObjMDAService.TaxYear, pObjMDAService.MDAServiceName, pObjMDAService.IntStatus)
                               select new DropDownListResult()
                               {
                                   id = mser.MDAServiceID.GetValueOrDefault(),
                                   text = mser.MDAServiceCode + " - " + mser.MDAServiceName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_InsertSettlementMethod(MAP_MDAService_SettlementMethod pObjSettlementMethod)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vExists = (from bbf in _db.MAP_MDAService_SettlementMethod
                               where bbf.SettlementMethodID == pObjSettlementMethod.SettlementMethodID && bbf.MDAServiceID == pObjSettlementMethod.MDAServiceID
                               select bbf);

                if (vExists.Count() > 0)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Settlement Method Already Exists";
                }

                _db.MAP_MDAService_SettlementMethod.Add(pObjSettlementMethod);

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = Ex.Message;
                }

                return mObjResponse;
            }
        }

        public FuncResponse REP_RemoveSettlementMethod(MAP_MDAService_SettlementMethod pObjSettlementMethod)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                MAP_MDAService_SettlementMethod mObjDeleteSettlementMethod;

                mObjDeleteSettlementMethod = _db.MAP_MDAService_SettlementMethod.Find(pObjSettlementMethod.ARSMID);

                if (mObjDeleteSettlementMethod == null)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Settlement Method Already Removed.";
                }
                else
                {
                    _db.MAP_MDAService_SettlementMethod.Remove(mObjDeleteSettlementMethod);

                    try
                    {
                        _db.SaveChanges();
                        mObjResponse.Success = true;
                    }
                    catch (Exception Ex)
                    {
                        mObjResponse.Success = false;
                        mObjResponse.Message = Ex.Message;
                    }
                }

                return mObjResponse;
            }
        }

        public IList<MAP_MDAService_SettlementMethod> REP_GetSettlementMethod(int pIntMDAServiceID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.MAP_MDAService_SettlementMethod.Where(t => t.MDAServiceID == pIntMDAServiceID).ToList();
            }
        }

        public FuncResponse REP_InsertMDAServiceItem(MAP_MDAService_MDAServiceItem pObjMDAServiceItem)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vExists = (from bbf in _db.MAP_MDAService_MDAServiceItem
                               where bbf.MDAServiceItemID == pObjMDAServiceItem.MDAServiceItemID && bbf.MDAServiceID == pObjMDAServiceItem.MDAServiceID
                               select bbf);

                if (vExists.Count() > 0)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "MDA Service Item Already Exists";
                }

                _db.MAP_MDAService_MDAServiceItem.Add(pObjMDAServiceItem);

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = Ex.Message;
                }

                return mObjResponse;
            }
        }

        public FuncResponse REP_RemoveMDAServiceItem(MAP_MDAService_MDAServiceItem pObjMDAServiceItem)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                MAP_MDAService_MDAServiceItem mObjDeleteMDAServiceItem;

                mObjDeleteMDAServiceItem = _db.MAP_MDAService_MDAServiceItem.Find(pObjMDAServiceItem.MSMSIID);

                if (mObjDeleteMDAServiceItem == null)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "MDA Service Already Removed.";
                }
                else
                {
                    _db.MAP_MDAService_MDAServiceItem.Remove(mObjDeleteMDAServiceItem);

                    try
                    {
                        _db.SaveChanges();
                        mObjResponse.Success = true;
                    }
                    catch (Exception Ex)
                    {
                        mObjResponse.Success = false;
                        mObjResponse.Message = Ex.Message;
                    }
                }

                return mObjResponse;
            }
        }

        public IList<MAP_MDAService_MDAServiceItem> REP_GetMDAServiceItem(int pIntMDAServiceID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.MAP_MDAService_MDAServiceItem.Where(t => t.MDAServiceID == pIntMDAServiceID).ToList();
            }
        }

        public IList<DropDownListResult> REP_GetSettlementMethodDropDownList(int pIntMDAServiceID)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from arsm in _db.MAP_MDAService_SettlementMethod
                               join smth in _db.Settlement_Method on arsm.SettlementMethodID equals smth.SettlementMethodID
                               where arsm.MDAServiceID == pIntMDAServiceID
                               select new DropDownListResult()
                               {
                                   id = smth.SettlementMethodID,
                                   text = smth.SettlementMethodName
                               });

                return vResult.ToList();
            }
        }

        public IList<usp_GetMDAServiceForServiceBill_Result> REP_GetMDAServiceForServiceBill(int pIntTaxPayerTypeID, int pIntTaxPayerID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetMDAServiceForServiceBill(pIntTaxPayerID, pIntTaxPayerTypeID).ToList();
            }
        }

        public IList<usp_GetTaxPayerMDAService_Result> REP_GetTaxPayerMDAService(int pIntTaxPayerTypeID, int pIntTaxPayerID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerMDAService(pIntTaxPayerID, pIntTaxPayerTypeID).ToList();
            }
        }

        public IList<usp_GetVehicleInsuranceVerificationMDAServiceForSupplier_Result> REP_GetVehicleInsuranceVerificationMDAServiceForSupplier()
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehicleInsuranceVerificationMDAServiceForSupplier().ToList();
            }
        }

        public IList<usp_GetVehicleLicenseMDAServiceForSupplier_Result> REP_GetVehicleLicenseMDAServiceForSupplier()
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehicleLicenseMDAServiceForSupplier().ToList();
            }
        }

        public IList<usp_GetEdoGISMDAServiceForSupplier_Result> REP_GetEdoGISMDAServiceForSupplier()
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetEdoGISMDAServiceForSupplier().ToList();
            }
        }

        public IList<usp_SearchMDAServiceForRDMLoad_Result> REP_SearchMDAServiceDetails(MDA_Services pObjMDAService)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_SearchMDAServiceForRDMLoad(pObjMDAService.MDAServiceCode, pObjMDAService.MDAServiceName, pObjMDAService.RuleRunName, pObjMDAService.PaymentFrequencyName, pObjMDAService.MDAServiceItemName, pObjMDAService.StrServiceAmount, pObjMDAService.StrTaxYear, pObjMDAService.SettlementMethodName, pObjMDAService.PaymentOptionName, pObjMDAService.ActiveText).ToList();
            }
        }

        public IDictionary<string, object> REP_SearchMDAService(MDA_Services pObjMDAService)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>
                {
                    ["MDAServiceList"] = _db.usp_SearchMDAService(pObjMDAService.WhereCondition, pObjMDAService.OrderBy, pObjMDAService.OrderByDirection, pObjMDAService.PageNumber, pObjMDAService.PageSize, pObjMDAService.MainFilter,
                                                                        pObjMDAService.MDAServiceCode, pObjMDAService.MDAServiceName, pObjMDAService.RuleRunName, pObjMDAService.PaymentFrequencyName, pObjMDAService.MDAServiceItemName, pObjMDAService.StrServiceAmount, pObjMDAService.StrTaxYear, pObjMDAService.SettlementMethodName, pObjMDAService.PaymentOptionName, pObjMDAService.ActiveText).ToList()
                };

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(MDAServiceID) FROM MDA_Services").FirstOrDefault();
                string rrn = "", ppn = "", sty = "", snm = "", pon = "";
                rrn = pObjMDAService.RuleRunName;
                ppn = pObjMDAService.PaymentFrequencyName;
                sty = pObjMDAService.StrTaxYear;
                snm = pObjMDAService.SettlementMethodName;
                pon = pObjMDAService.PaymentOptionName;
                if (rrn == null)
                    rrn = "";
                if (ppn == null)
                    ppn = "";
                if (sty == null)
                    sty = ""; 
                if (snm == null)
                    snm = ""; 
                if (pon == null)
                    pon = "";
                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(MDAServiceID) ");
                sbFilteredCountQuery.Append(" FROM MDA_Services mdaser ");
                sbFilteredCountQuery.Append(" INNER JOIN MST_RuleRun rr ON mdaser.RuleRunID = rr.RuleRunID ");
                sbFilteredCountQuery.Append(" INNER JOIN Payment_Frequency pfreq ON mdaser.PaymentFrequencyID = pfreq.PaymentFrequencyID ");
                sbFilteredCountQuery.Append(" INNER JOIN Payment_Options popt ON mdaser.PaymentOptionID = popt.PaymentOptionID  WHERE 1 = 1  ");
                sbFilteredCountQuery.Append(pObjMDAService.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {

                    new SqlParameter("@MainFilter",pObjMDAService.MainFilter),
                    new SqlParameter("@MDAServiceCode",pObjMDAService.MDAServiceCode),
                    new SqlParameter("@MDAServiceName",pObjMDAService.MDAServiceName),
                    new SqlParameter("@RuleRunName",rrn),
                    new SqlParameter("@PaymentFrequencyName",ppn),
                    new SqlParameter("@MDAServiceItemNames",pObjMDAService.MDAServiceItemName),
                    new SqlParameter("@ServiceAmount",pObjMDAService.StrServiceAmount),
                    new SqlParameter("@TaxYear",sty),
                    new SqlParameter("@SettlementMethodNames",snm),
                    new SqlParameter("@PaymentOptionName",pon),
                    new SqlParameter("@ActiveText",pObjMDAService.ActiveText)
                };

                //Get Filtered Count
                int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntFilteredCount;

                return dcData;
            }
        }

        public IDictionary<string, object> REP_SearchMDAServiceForSideMenu(MDA_Services pObjMDAService)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>
                {
                    ["MDAServiceList"] = _db.usp_SearchMDAServiceForSideMenu(pObjMDAService.WhereCondition, pObjMDAService.OrderBy, pObjMDAService.OrderByDirection, pObjMDAService.PageNumber, pObjMDAService.PageSize, pObjMDAService.MainFilter).ToList()
                };

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(MDAServiceID) FROM MDA_Services mdaser INNER JOIN MST_RuleRun rr ON mdaser.RuleRunID = rr.RuleRunID INNER JOIN Payment_Frequency pfreq ON mdaser.PaymentFrequencyID = pfreq.PaymentFrequencyID").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(MDAServiceID) ");
                sbFilteredCountQuery.Append(" FROM MDA_Services mdaser ");
                sbFilteredCountQuery.Append(" INNER JOIN MST_RuleRun rr ON mdaser.RuleRunID = rr.RuleRunID ");
                sbFilteredCountQuery.Append(" INNER JOIN Payment_Frequency pfreq ON mdaser.PaymentFrequencyID = pfreq.PaymentFrequencyID WHERE 1 = 1  ");
                sbFilteredCountQuery.Append(pObjMDAService.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@MainFilter",pObjMDAService.MainFilter)
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
