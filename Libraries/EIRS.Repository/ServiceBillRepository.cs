using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class ServiceBillRepository : IServiceBillRepository
    {
        EIRSEntities _db;

        public FuncResponse<ServiceBill> REP_InsertUpdateServiceBill(ServiceBill pObjServiceBill)
        {
            using (_db = new EIRSEntities())
            {
                ServiceBill mObjInsertUpdateServiceBill; //Service Bill Insert Object
                FuncResponse<ServiceBill> mObjFuncResponse = new FuncResponse<ServiceBill>(); //Return Object


                //Check if Duplicate
                //var vDuplicateCheck = (from sb in _db.ServiceBills
                //                       where sb.TaxPayerID == pObjServiceBill.TaxPayerID
                //                       && sb.TaxPayerTypeID == pObjServiceBill.TaxPayerTypeID
                //                       && sb.ServiceBillID != pObjServiceBill.ServiceBillID
                //                       && sb.ServiceBillAmount == pObjServiceBill.ServiceBillAmount
                //                       && sb.Notes == pObjServiceBill.Notes
                //                       && sb.CreatedBy == sb.CreatedBy
                //                       select sb);

                //if (vDuplicateCheck.Count() > 0)
                //{
                //    mObjFuncResponse.Success = false;
                //    mObjFuncResponse.Message = "Service Bill already exists";
                //    return mObjFuncResponse;
                //}

                //If Update Load Service Bill
                if (pObjServiceBill.ServiceBillID != 0)
                {
                    mObjInsertUpdateServiceBill = (from sb in _db.ServiceBills
                                                   where sb.ServiceBillID == pObjServiceBill.ServiceBillID
                                                   select sb).FirstOrDefault();

                    if (mObjInsertUpdateServiceBill != null)
                    {
                        mObjInsertUpdateServiceBill.ModifiedBy = pObjServiceBill.ModifiedBy;
                        mObjInsertUpdateServiceBill.ModifiedDate = pObjServiceBill.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateServiceBill = new ServiceBill
                        {
                            CreatedBy = pObjServiceBill.CreatedBy,
                            CreatedDate = pObjServiceBill.CreatedDate
                        };
                    }
                }
                else // Else Insert Service Bill
                {
                    mObjInsertUpdateServiceBill = new ServiceBill
                    {
                        CreatedBy = pObjServiceBill.CreatedBy,
                        CreatedDate = pObjServiceBill.CreatedDate
                    };
                }

                mObjInsertUpdateServiceBill.ServiceBillDate = pObjServiceBill.ServiceBillDate != null ? pObjServiceBill.ServiceBillDate : mObjInsertUpdateServiceBill.ServiceBillDate;
                mObjInsertUpdateServiceBill.TaxPayerTypeID = pObjServiceBill.TaxPayerTypeID != null ? pObjServiceBill.TaxPayerTypeID : mObjInsertUpdateServiceBill.TaxPayerTypeID;
                mObjInsertUpdateServiceBill.TaxPayerID = pObjServiceBill.TaxPayerID != null ? pObjServiceBill.TaxPayerID : mObjInsertUpdateServiceBill.TaxPayerID;
                mObjInsertUpdateServiceBill.ServiceBillAmount = pObjServiceBill.ServiceBillAmount;
                mObjInsertUpdateServiceBill.SettlementDate = pObjServiceBill.SettlementDate != null ? pObjServiceBill.SettlementDate : mObjInsertUpdateServiceBill.SettlementDate;
                mObjInsertUpdateServiceBill.SettlementStatusID = pObjServiceBill.SettlementStatusID != null ? pObjServiceBill.SettlementStatusID : mObjInsertUpdateServiceBill.SettlementStatusID;
                mObjInsertUpdateServiceBill.SettlementDueDate = pObjServiceBill.SettlementDueDate;
                mObjInsertUpdateServiceBill.Notes = pObjServiceBill.Notes;
                mObjInsertUpdateServiceBill.Active = pObjServiceBill.Active != null ? pObjServiceBill.Active : mObjInsertUpdateServiceBill.Active;

                if (pObjServiceBill.ServiceBillID == 0)
                {
                    _db.ServiceBills.Add(mObjInsertUpdateServiceBill);
                }

                try
                {
                    _db.SaveChanges();

                    var context = ((IObjectContextAdapter)_db).ObjectContext;
                    var refreshableObjects = _db.ChangeTracker.Entries().Select(c => c.Entity).ToList();
                    context.Refresh(RefreshMode.StoreWins, refreshableObjects);
                    mObjFuncResponse.Success = true;
                    if (pObjServiceBill.ServiceBillID == 0)
                        mObjFuncResponse.Message = "Service Bill Added Successfully";
                    else
                        mObjFuncResponse.Message = "Service Bill Updated Successfully";

                    mObjFuncResponse.AdditionalData = mObjInsertUpdateServiceBill;

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjServiceBill.ServiceBillID == 0)
                        mObjFuncResponse.Message = "Service Bill Addition Failed";
                    else
                        mObjFuncResponse.Message = "Service Bill Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<vw_ServiceBillNew> REP_GetServiceBillList()
        {
            using (_db = new EIRSEntities())
            {
                return _db.vw_ServiceBillNew.ToList();
            }
        }

        public IList<usp_GetServiceBillList_Result> REP_GetServiceBillList(ServiceBill pObjServiceBill)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetServiceBillList(pObjServiceBill.TaxPayerTypeID, pObjServiceBill.TaxPayerID, pObjServiceBill.ServiceBillID, pObjServiceBill.ServiceBillRefNo, pObjServiceBill.IntStatus).ToList();
            }
        }

        public usp_GetServiceBillList_Result REP_GetServiceBillDetails(ServiceBill pObjServiceBill)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetServiceBillList(pObjServiceBill.TaxPayerTypeID, pObjServiceBill.TaxPayerID, pObjServiceBill.ServiceBillID, pObjServiceBill.ServiceBillRefNo, pObjServiceBill.IntStatus).FirstOrDefault();
            }
        }

        public IList<usp_GetServiceBillItemList_Result> REP_GetServiceBillItem(long pIntServiceBillID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetServiceBillItemList(Convert.ToInt32(pIntServiceBillID)).ToList();
            }
        }

        public usp_GetServiceBillItemDetails_Result REP_GetServiceBillItemDetails(long plngSBSIID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetServiceBillItemDetails(plngSBSIID).FirstOrDefault();
            }
        }

        public FuncResponse REP_UpdateStatus(ServiceBill pObjServiceBill)
        {
            using (_db = new EIRSEntities())
            {
                ServiceBill mObjInsertUpdateServiceBill; //Service Bill Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load ServiceBill_Rules
                if (pObjServiceBill.ServiceBillID != 0)
                {
                    mObjInsertUpdateServiceBill = (from ast in _db.ServiceBills
                                                   where ast.ServiceBillID == pObjServiceBill.ServiceBillID
                                                   select ast).FirstOrDefault();

                    if (mObjInsertUpdateServiceBill != null)
                    {
                        mObjInsertUpdateServiceBill.Active = !mObjInsertUpdateServiceBill.Active;
                        mObjInsertUpdateServiceBill.ModifiedBy = pObjServiceBill.ModifiedBy;
                        mObjInsertUpdateServiceBill.ModifiedDate = pObjServiceBill.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Service Bill Updated Successfully";
                           // mObjFuncResponse.AdditionalData = _db.usp_GetServiceBillList(pObjServiceBill.TaxPayerTypeID, pObjServiceBill.TaxPayerID, 0,"", pObjServiceBill.IntStatus).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Service Bill Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public ServiceBill REP_GetServiceBillDetailsById(long serviceBillId)
        {
            using (_db = new EIRSEntities())
            {
                var serviceBill = (from ast in _db.ServiceBills
                                   where ast.ServiceBillID == serviceBillId
                                   select ast).FirstOrDefault();
                return serviceBill;
            }
        }

        public FuncResponse REP_UpdateServiceBillSettlementStatus(ServiceBill pObjServiceBill)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                ServiceBill mObjUpdateServiceBill = _db.ServiceBills.Find(pObjServiceBill.ServiceBillID);

                if (mObjUpdateServiceBill != null)
                {
                    mObjUpdateServiceBill.SettlementDate = pObjServiceBill.SettlementDate == null ? mObjUpdateServiceBill.SettlementDate : pObjServiceBill.SettlementDate;
                    mObjUpdateServiceBill.SettlementStatusID = pObjServiceBill.SettlementStatusID;
                    mObjUpdateServiceBill.ModifiedBy = pObjServiceBill.ModifiedBy;
                    mObjUpdateServiceBill.ModifiedDate = pObjServiceBill.ModifiedDate;
                    mObjUpdateServiceBill.ServiceBillAmount = pObjServiceBill.ServiceBillAmount;
                }

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

        public FuncResponse<MAP_ServiceBill_MDAService> REP_InsertUpdateMDAService(MAP_ServiceBill_MDAService pObjMDAService)
        {
            using (_db = new EIRSEntities())
            {
                MAP_ServiceBill_MDAService mObjInsertUpdateMDAService; //ServiceBill Insert Object
                FuncResponse<MAP_ServiceBill_MDAService> mObjFuncResponse = new FuncResponse<MAP_ServiceBill_MDAService>(); //Return Object

                if (pObjMDAService.SBSID > 0)
                {
                    mObjInsertUpdateMDAService = _db.MAP_ServiceBill_MDAService.Find(pObjMDAService.SBSID);

                    if (mObjInsertUpdateMDAService != null)
                    {
                        mObjInsertUpdateMDAService.ServiceAmount = pObjMDAService.ServiceAmount;
                        mObjInsertUpdateMDAService.ModifiedBy = pObjMDAService.ModifiedBy;
                        mObjInsertUpdateMDAService.ModifiedDate = pObjMDAService.ModifiedDate;
                    }
                    else
                    {
                        throw (new Exception("Not Found"));
                    }
                }
                else
                {
                    mObjInsertUpdateMDAService = new MAP_ServiceBill_MDAService
                    {
                        ServiceBillID = pObjMDAService.ServiceBillID,
                        MDAServiceID = pObjMDAService.MDAServiceID,
                        ServiceAmount = pObjMDAService.ServiceAmount,
                        ServiceBillYear = pObjMDAService.ServiceBillYear,
                        CreatedBy = pObjMDAService.CreatedBy,
                        CreatedDate = pObjMDAService.CreatedDate,
                    };

                    _db.MAP_ServiceBill_MDAService.Add(mObjInsertUpdateMDAService);
                }
                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.Message = "MDA Service Added Successfully";
                    mObjFuncResponse.AdditionalData = mObjInsertUpdateMDAService;
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;
                    mObjFuncResponse.Message = "MDA Service Addition Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<MAP_ServiceBill_MDAService> REP_GetMDAServices(long pIntServiceBillID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.MAP_ServiceBill_MDAService.Include("MDA_Services").Where(t => t.ServiceBillID == pIntServiceBillID).ToList();
            }
        }

        public FuncResponse REP_DeleteMDAService(long pIntSBSID)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                MAP_ServiceBill_MDAService mObjDeleteMDAService = _db.MAP_ServiceBill_MDAService.Find(pIntSBSID);

                if (mObjDeleteMDAService != null)
                {
                    _db.MAP_ServiceBill_MDAService.Remove(mObjDeleteMDAService);

                    // Find MDA Service Item 

                    var vServiceBillItems = _db.MAP_ServiceBill_MDAServiceItem.Where(t => t.SBSID == pIntSBSID);

                    if (vServiceBillItems.Count() > 0)
                    {
                        _db.MAP_ServiceBill_MDAServiceItem.RemoveRange(vServiceBillItems);
                    }

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                        mObjFuncResponse.Message = "MDA Service Removed Successfully";

                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Exception = Ex;
                        mObjFuncResponse.Message = "MDA Service Deletion Failed";

                    }

                }
                else
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Invalid MDA Service";
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_InsertServiceBillItem(IList<MAP_ServiceBill_MDAServiceItem> plstServiceBillItem)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();
                MAP_ServiceBill_MDAServiceItem mObjInsertServiceBillItem;

                foreach (MAP_ServiceBill_MDAServiceItem mObjSIDetail in plstServiceBillItem)
                {
                    if (mObjSIDetail.SBSIID > 0)
                    {
                        mObjInsertServiceBillItem = _db.MAP_ServiceBill_MDAServiceItem.Find(mObjSIDetail.SBSIID);
                        if (mObjInsertServiceBillItem != null)
                        {
                            mObjInsertServiceBillItem.ServiceBaseAmount = mObjSIDetail.ServiceBaseAmount;
                            mObjInsertServiceBillItem.Percentage = mObjSIDetail.Percentage;
                            mObjInsertServiceBillItem.ServiceAmount = mObjSIDetail.ServiceAmount;
                            mObjInsertServiceBillItem.ModifiedBy = mObjSIDetail.ModifiedBy;
                            mObjInsertServiceBillItem.ModifiedDate = mObjSIDetail.ModifiedDate;
                        }
                        else
                        {
                            throw (new Exception("Not Found"));
                        }
                    }
                    else
                    {
                        mObjInsertServiceBillItem = new MAP_ServiceBill_MDAServiceItem()
                        {
                            SBSID = mObjSIDetail.SBSID,
                            MDAServiceItemID = mObjSIDetail.MDAServiceItemID,
                            ServiceBaseAmount = mObjSIDetail.ServiceBaseAmount,
                            Percentage = mObjSIDetail.Percentage,
                            ServiceAmount = mObjSIDetail.ServiceAmount,
                            PaymentStatusID = mObjSIDetail.PaymentStatusID,
                            CreatedBy = mObjSIDetail.CreatedBy,
                            CreatedDate = mObjSIDetail.CreatedDate,
                        };

                        _db.MAP_ServiceBill_MDAServiceItem.Add(mObjInsertServiceBillItem);
                    }
                }

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

        public IList<MAP_ServiceBill_MDAServiceItem> REP_GetServiceBillItems(long plngSBSID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.MAP_ServiceBill_MDAServiceItem.Include("MDA_Service_Items").Where(t => t.SBSID == plngSBSID).ToList();
            }
        }  
        public MAP_ServiceBill_MDAServiceItem GetServiceBillItems(long sBSID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.MAP_ServiceBill_MDAServiceItem.FirstOrDefault(t => t.SBSIID == sBSID);
            }
        }

        public FuncResponse REP_UpdateMDAServiceItemStatus(MAP_ServiceBill_MDAServiceItem pObjServiceBillItem)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();
                decimal? checker = 0;
                MAP_ServiceBill_MDAServiceItem mObjUpdateServiceBillItem = _db.MAP_ServiceBill_MDAServiceItem.Find(pObjServiceBillItem.SBSIID);

                if (mObjUpdateServiceBillItem != null)
                {
                    List<MAP_ServiceBill_MDAServiceItem> zeroValue = _db.MAP_ServiceBill_MDAServiceItem.Where(k => k.SBSIID == mObjUpdateServiceBillItem.SBSIID && k.ServiceAmount == checker).ToList();
                    foreach (var zeroValueItem in zeroValue)
                    {
                        zeroValueItem.PaymentStatusID = 3;
                    }
                    mObjUpdateServiceBillItem.PaymentStatusID = pObjServiceBillItem.PaymentStatusID;
                 //  mObjUpdateServiceBillItem.ServiceBaseAmount = pObjServiceBillItem.ServiceBaseAmount;
                  //  mObjUpdateServiceBillItem.ServiceAmount = pObjServiceBillItem.ServiceAmount;
                    mObjUpdateServiceBillItem.ModifiedBy = pObjServiceBillItem.ModifiedBy;
                    mObjUpdateServiceBillItem.ModifiedDate = pObjServiceBillItem.ModifiedDate;
                }

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


        public IList<DropDownListResult> REP_GetSettlementMethodMDAServiceBased(long pIntServiceBillID)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from settlmthd in _db.Settlement_Method
                               join ss in _db.MAP_MDAService_SettlementMethod on settlmthd.SettlementMethodID equals ss.SettlementMethodID
                               join sbs in _db.MAP_ServiceBill_MDAService on ss.MDAServiceID equals sbs.MDAServiceID
                               where sbs.ServiceBillID == pIntServiceBillID
                               select new DropDownListResult()
                               {
                                   id = settlmthd.SettlementMethodID,
                                   text = settlmthd.SettlementMethodName
                               }).Distinct().ToList();

                return vResult;
            }
        }

        public IList<usp_GetMDAServiceBasedSettlement_Result> REP_GetMDAServiceBasedSettlement(long pIntServiceBillID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetMDAServiceBasedSettlement(Convert.ToInt32(pIntServiceBillID)).ToList();
            }
        }

        public IList<usp_GetServiceBill_MDAServiceList_Result> REP_GetMDAServiceList(long pIntServiceBillID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetServiceBill_MDAServiceList(Convert.ToInt32(pIntServiceBillID)).ToList();
            }
        }

        public IList<usp_GetVehicleInsuranceVerificationServiceBillForSupplier_Result> REP_GetVehicleInsuranceVerificationServiceBillForSupplier()
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehicleInsuranceVerificationServiceBillForSupplier().ToList();
            }
        }

        public IList<usp_GetVehicleLicenseServiceBillForSupplier_Result> REP_GetVehicleLicenseServiceBillForSupplier()
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetVehicleLicenseServiceBillForSupplier().ToList();
            }
        }

        public IDictionary<string, object> REP_SearchServiceBill(ServiceBill pObjServiceBill)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>
                {
                    ["ServiceBillList"] = _db.usp_SearchServiceBill(pObjServiceBill.WhereCondition, pObjServiceBill.OrderBy, pObjServiceBill.OrderByDirection, pObjServiceBill.PageNumber, pObjServiceBill.PageSize, pObjServiceBill.MainFilter,
                                                                        pObjServiceBill.ServiceBillRefNo, pObjServiceBill.strServiceBillDate, pObjServiceBill.TaxPayerTypeName, pObjServiceBill.TaxPayerRIN, pObjServiceBill.TaxPayerName, pObjServiceBill.strServiceBillAmount, pObjServiceBill.strSettlementDueDate,pObjServiceBill.strSettlementDate,pObjServiceBill.SettlementStatusName, pObjServiceBill.Notes, pObjServiceBill.ActiveText).ToList()
                };

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(ServiceBillID) FROM ServiceBill").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(ServiceBillID) ");
                sbFilteredCountQuery.Append(" FROM ServiceBill sb ");
                sbFilteredCountQuery.Append(" INNER JOIN Settlement_Status ss ON sb.SettlementStatusID = ss.SettlementStatusID ");
                sbFilteredCountQuery.Append(" INNER JOIN TaxPayer_Types tptype ON sb.TaxPayerTypeID = tptype.TaxPayerTypeID WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjServiceBill.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@MainFilter",pObjServiceBill.MainFilter),
                    new SqlParameter("@ServiceBillRefNo",pObjServiceBill.ServiceBillRefNo),
                    new SqlParameter("@ServiceBillDate",pObjServiceBill.strServiceBillDate),
                    new SqlParameter("@TaxPayerTypeName",pObjServiceBill.TaxPayerTypeName),
                    new SqlParameter("@TaxPayerRIN",pObjServiceBill.TaxPayerRIN),
                    new SqlParameter("@TaxPayerName",pObjServiceBill.TaxPayerName),
                    new SqlParameter("@ServiceBillAmount",pObjServiceBill.strServiceBillAmount),
                    new SqlParameter("@SettlementDueDate",pObjServiceBill.strSettlementDueDate),
                    new SqlParameter("@SettlementDate",pObjServiceBill.strSettlementDate),
                    new SqlParameter("@SettlementStatus",pObjServiceBill.SettlementStatusName),
                    new SqlParameter("@ServiceBillNotes",pObjServiceBill.Notes),
                    new SqlParameter("@ActiveText",pObjServiceBill.ActiveText),
                };

                //Get Filtered Count
                int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntFilteredCount;

                return dcData;
            }
        }

        public void REP_UpdateServiceBillAmount(long pIntServiceBillID)
        {
            using (_db = new EIRSEntities())
            {
                _db.usp_UpdateServiceBillAmount(Convert.ToInt32(pIntServiceBillID));
            }
        }

        public IList<usp_GetServiceBillAdjustmentList_Result> REP_GetServiceBillAdjustment(long pIntServiceBillID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetServiceBillAdjustmentList(Convert.ToInt32(pIntServiceBillID)).ToList();
            }
        }

        public IList<usp_GetServiceBillLateChargeList_Result> REP_GetServiceBillLateCharge(long pIntServiceBillID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetServiceBillLateChargeList(Convert.ToInt32(pIntServiceBillID)).ToList();
            }
        }

        public FuncResponse REP_InsertAdjustment(MAP_ServiceBill_Adjustment pObjAdjustment)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();
                MAP_ServiceBill_Adjustment mObjInsertAdjustment = new MAP_ServiceBill_Adjustment()
                {
                    SBSIID = pObjAdjustment.SBSIID,
                    AdjustmentDate = pObjAdjustment.AdjustmentDate,
                    AdjustmentLine = pObjAdjustment.AdjustmentLine,
                    AdjustmentTypeID = pObjAdjustment.AdjustmentTypeID,
                    Amount = pObjAdjustment.Amount,
                    CreatedBy = pObjAdjustment.CreatedBy,
                    CreatedDate = pObjAdjustment.CreatedDate,
                };

                _db.MAP_ServiceBill_Adjustment.Add(mObjInsertAdjustment);

                try
                {
                    _db.SaveChanges();

                    //Get Service Bill Id
                    var vResult = (from mditem in _db.MAP_ServiceBill_MDAServiceItem
                                   join mds in _db.MAP_ServiceBill_MDAService on mditem.SBSID equals mds.SBSID
                                   where mditem.SBSIID == pObjAdjustment.SBSIID
                                   select mds.ServiceBillID).FirstOrDefault();

                    mObjResponse.Success = true;
                    mObjResponse.Message = "Adjustment Added Successfully";
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Exception = Ex;
                    mObjResponse.Message = "Adjustment Adding Failed";
                }

                return mObjResponse;
            }
        }

        public IDictionary<string, object> REP_SearchServiceBillForSideMenu(ServiceBill pObjServiceBill)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>
                {
                    ["ServiceBillList"] = _db.usp_SearchServiceBillForSideMenu(pObjServiceBill.WhereCondition, pObjServiceBill.OrderBy, pObjServiceBill.OrderByDirection, pObjServiceBill.PageNumber, pObjServiceBill.PageSize, pObjServiceBill.MainFilter).ToList()
                };

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select Count(ServiceBillID) FROM ServiceBill sb INNER JOIN Settlement_Status ss ON sb.SettlementStatusID = ss.SettlementStatusID").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT Count(ServiceBillID) ");
                sbFilteredCountQuery.Append(" FROM ServiceBill sb ");
                sbFilteredCountQuery.Append(" INNER JOIN Settlement_Status ss ON sb.SettlementStatusID = ss.SettlementStatusID WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjServiceBill.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                    new SqlParameter("@MainFilter",pObjServiceBill.MainFilter)
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
