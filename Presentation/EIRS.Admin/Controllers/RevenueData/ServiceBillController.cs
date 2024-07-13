using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Vereyon.Web;
using System.Linq;
using System.Transactions;
using Elmah;
using System.Text;

namespace EIRS.Admin.Controllers
{
    public class ServiceBillController : BaseController
    {
        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public JsonResult LoadData()
        {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var vFilter = Request.Form.GetValues("search[value]").FirstOrDefault();

            StringBuilder sbWhereCondition = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(Request.Form["ServiceBillRefNo"]))
            {
                sbWhereCondition.Append(" AND ISNULL(ServiceBillRefNo,'') LIKE @ServiceBillRefNo");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ServiceBillDate"]))
            {
                sbWhereCondition.Append(" AND ISNULL(REPLACE(CONVERT(varchar(50),ServiceBillDate,106),' ','-'),'') LIKE @ServiceBillDate");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TaxPayerTypeName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(TaxPayerTypeName,'') LIKE @TaxPayerTypeName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TaxPayerName"]))
            {
                sbWhereCondition.Append(" AND dbo.GetTaxPayerName(sb.TaxPayerID,sb.TaxPayerTypeID) LIKE @TaxPayerName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TaxPayerRIN"]))
            {
                sbWhereCondition.Append(" AND dbo.GetTaxPayerRIN(sb.TaxPayerID,sb.TaxPayerTypeID) LIKE @TaxPayerRIN");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ServiceBillAmount"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),ServiceBillAmount,106),'') LIKE @ServiceBillAmount");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["SettlementDueDate"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),SettlementDueDate,106),'') LIKE @SettlementDueDate");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["SettlementStatusName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(SettlementStatusName,'') LIKE @SettlementStatus");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["SettlementDate"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),SettlementDate,106),'') LIKE @SettlementDate");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ServiceBillNotes"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Notes,'') LIKE @ServiceBillNotes");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ActiveText"]))
            {
                sbWhereCondition.Append(" AND CASE WHEN ISNULL(sb.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @ActiveText");
            }

            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( ISNULL(ServiceBillRefNo,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),ServiceBillDate,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TaxPayerTypeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerName(sb.TaxPayerID,sb.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerRIN(sb.TaxPayerID,sb.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),ServiceBillAmount,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),SettlementDueDate,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(SettlementStatusName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),SettlementDate,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Notes,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR CASE WHEN ISNULL(sb.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @MainFilter)");

            }

            ServiceBill mObjServiceBill = new ServiceBill()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),

                ServiceBillRefNo = !string.IsNullOrWhiteSpace(Request.Form["ServiceBillRefNo"]) ? "%" + Request.Form["ServiceBillRefNo"].Trim() + "%" : TrynParse.parseString(Request.Form["ServiceBillRefNo"]),
                strServiceBillDate = !string.IsNullOrWhiteSpace(Request.Form["ServiceBillDate"]) ? "%" + Request.Form["ServiceBillDate"].Trim() + "%" : TrynParse.parseString(Request.Form["ServiceBillDate"]),
                TaxPayerTypeName = !string.IsNullOrWhiteSpace(Request.Form["TaxPayerTypeName"]) ? "%" + Request.Form["TaxPayerTypeName"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxPayerTypeName"]),
                TaxPayerName = !string.IsNullOrWhiteSpace(Request.Form["TaxPayerName"]) ? "%" + Request.Form["TaxPayerName"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxPayerName"]),
                TaxPayerRIN = !string.IsNullOrWhiteSpace(Request.Form["TaxPayerRIN"]) ? "%" + Request.Form["TaxPayerRIN"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxPayerRIN"]),
                strServiceBillAmount = !string.IsNullOrWhiteSpace(Request.Form["ServiceBillAmount"]) ? "%" + Request.Form["ServiceBillAmount"].Trim() + "%" : TrynParse.parseString(Request.Form["ServiceBillAmount"]),
                strSettlementDueDate = !string.IsNullOrWhiteSpace(Request.Form["SettlementDueDate"]) ? "%" + Request.Form["SettlementDueDate"].Trim() + "%" : TrynParse.parseString(Request.Form["SettlementDueDate"]),
                SettlementStatusName = !string.IsNullOrWhiteSpace(Request.Form["SettlementStatusName"]) ? "%" + Request.Form["SettlementStatusName"].Trim() + "%" : TrynParse.parseString(Request.Form["SettlementStatusName"]),
                strSettlementDate = !string.IsNullOrWhiteSpace(Request.Form["SettlementDate"]) ? "%" + Request.Form["SettlementDate"].Trim() + "%" : TrynParse.parseString(Request.Form["SettlementDate"]),
                Notes = !string.IsNullOrWhiteSpace(Request.Form["ServiceBillNotes"]) ? "%" + Request.Form["ServiceBillNotes"].Trim() + "%" : TrynParse.parseString(Request.Form["ServiceBillNotes"]),
                ActiveText = !string.IsNullOrWhiteSpace(Request.Form["ActiveText"]) ? "%" + Request.Form["ActiveText"].Trim() + "%" : TrynParse.parseString(Request.Form["ActiveText"]),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };

            IDictionary<string, object> dcData = new BLServiceBill().BL_SearchServiceBill(mObjServiceBill);
            IList<usp_SearchServiceBill_Result> lstServiceBill = (IList<usp_SearchServiceBill_Result>)dcData["ServiceBillList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstServiceBill
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Generate()
        {
            UI_FillTaxPayerTypeDropDown();
            return View();
        }

        public void UI_FillServiceBillDropDown(ServiceBillViewModel pObjServiceBillModel = null)
        {
            IList<DropDownListResult> lstMDAService = new BLMDAService().BL_GetMDAServiceDropDownList(new MDA_Services() { IntStatus = 1 });
            ViewBag.MDAServiceList = new SelectList(lstMDAService, "id", "text");

            ViewBag.MAPServiceBillServiceList = SessionManager.lstServiceBillService;
            ViewBag.MAPServiceBillItemList = SessionManager.lstServiceBillItem.Where(t => t.MDAService_RowID == 0).ToList();
        }

        public ActionResult Add(int? id, string name, int? tptype)
        {
            if (id.GetValueOrDefault() > 0 && tptype.GetValueOrDefault() > 0)
            {

                ServiceBillViewModel mObjServiceBillModel = new ServiceBillViewModel()
                {
                    TaxPayerID = id.GetValueOrDefault(),
                    TaxPayerRIN = name,
                    TaxPayerTypeID = tptype.GetValueOrDefault(),
                    SettlementDuedate = CommUtil.GetCurrentDateTime()
                };

                if (tptype.GetValueOrDefault() == (int)EnumList.TaxPayerType.Individual)
                {
                    usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 2, IndividualID = id.GetValueOrDefault() });
                    mObjServiceBillModel.TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName;
                }
                else if (tptype.GetValueOrDefault() == (int)EnumList.TaxPayerType.Companies)
                {
                    usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = id.GetValueOrDefault() });
                    mObjServiceBillModel.TaxPayerName = mObjCompanyData.CompanyName;
                }
                else if (tptype.GetValueOrDefault() == (int)EnumList.TaxPayerType.Special)
                {
                    usp_GetSpecialList_Result mObjSpecialData = new BLSpecial().BL_GetSpecialDetails(new Special() { intStatus = 2, SpecialID = id.GetValueOrDefault() });
                    mObjServiceBillModel.TaxPayerName = mObjSpecialData.SpecialTaxPayerName;
                }
                else if (tptype.GetValueOrDefault() == (int)EnumList.TaxPayerType.Government)
                {
                    usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(new Government() { intStatus = 2, GovernmentID = id.GetValueOrDefault() });
                    mObjServiceBillModel.TaxPayerName = mObjGovernmentData.GovernmentName;
                }

                SessionManager.lstServiceBillItem = new List<ServiceBill_MDAServiceItem>();
                SessionManager.lstServiceBillService = new List<ServiceBill_MDAService>();
                UI_FillServiceBillDropDown();
                return View(mObjServiceBillModel);
            }
            else
            {
                return RedirectToAction("List", "ServiceBill");
            }


        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(ServiceBillViewModel pObjServiceBillModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillServiceBillDropDown(pObjServiceBillModel);
                return View(pObjServiceBillModel);
            }
            else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    IList<ServiceBill_MDAServiceItem> lstServiceBillItems = SessionManager.lstServiceBillItem ?? new List<ServiceBill_MDAServiceItem>();
                    IList<ServiceBill_MDAService> lstServiceBillServices = SessionManager.lstServiceBillService ?? new List<ServiceBill_MDAService>();

                    int IntServiceBillServiceCount = lstServiceBillServices.Where(t => t.intTrack != EnumList.Track.DELETE).Count();

                    if (IntServiceBillServiceCount == 0)
                    {
                        UI_FillServiceBillDropDown(pObjServiceBillModel);
                        ModelState.AddModelError("ServiceBillService-error", "Please Add Atleast One MDA Service");
                        Transaction.Current.Rollback();
                        return View(pObjServiceBillModel);
                    }
                    else
                    {

                        BLServiceBill mObjBLServiceBill = new BLServiceBill();

                        ServiceBill mObjServiceBill = new ServiceBill()
                        {
                            ServiceBillID = 0,
                            TaxPayerID = pObjServiceBillModel.TaxPayerID,
                            TaxPayerTypeID = pObjServiceBillModel.TaxPayerTypeID,
                            ServiceBillAmount = lstServiceBillItems.Count > 0 ? lstServiceBillItems.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.ServiceAmount) : 0,
                            ServiceBillDate = CommUtil.GetCurrentDateTime(),
                            SettlementDueDate = pObjServiceBillModel.SettlementDuedate,
                            SettlementStatusID = 2,
                            Active = true,
                            CreatedBy = SessionManager.SystemUserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        try
                        {

                            FuncResponse<ServiceBill> mObjServiceBillResponse = mObjBLServiceBill.BL_InsertUpdateServiceBill(mObjServiceBill);

                            if (mObjServiceBillResponse.Success)
                            {
                                //Adding MDA Service

                                foreach (ServiceBill_MDAService mObjSBS in lstServiceBillServices.Where(t => t.intTrack != EnumList.Track.DELETE))
                                {
                                    MAP_ServiceBill_MDAService mObjMDAService = new MAP_ServiceBill_MDAService()
                                    {
                                        ServiceBillID = mObjServiceBillResponse.AdditionalData.ServiceBillID,
                                        MDAServiceID = mObjSBS.MDAServiceID,
                                        ServiceAmount = mObjSBS.ServiceAmount,
                                        ServiceBillYear = mObjSBS.TaxYear,
                                        CreatedBy = SessionManager.SystemUserID,
                                        CreatedDate = CommUtil.GetCurrentDateTime()
                                    };

                                    FuncResponse<MAP_ServiceBill_MDAService> mObjSBSResponse = mObjBLServiceBill.BL_InsertUpdateMDAService(mObjMDAService);

                                    if (mObjSBSResponse.Success)
                                    {
                                        IList<MAP_ServiceBill_MDAServiceItem> lstInsertServiceBillDetail = new List<MAP_ServiceBill_MDAServiceItem>();

                                        foreach (ServiceBill_MDAServiceItem mObjServiceBillItemDetail in lstServiceBillItems.Where(t => t.MDAService_RowID == mObjSBS.RowID))
                                        {
                                            MAP_ServiceBill_MDAServiceItem mObjSIDetail = new MAP_ServiceBill_MDAServiceItem()
                                            {
                                                SBSID = mObjSBSResponse.AdditionalData.SBSID,
                                                MDAServiceItemID = mObjServiceBillItemDetail.MDAServiceItemID,
                                                ServiceBaseAmount = mObjServiceBillItemDetail.ServiceBaseAmount,
                                                ServiceAmount = mObjServiceBillItemDetail.ServiceAmount,
                                                Percentage = mObjServiceBillItemDetail.Percentage,
                                                PaymentStatusID = (int)EnumList.PaymentStatus.Due,
                                                CreatedBy = SessionManager.SystemUserID,
                                                CreatedDate = CommUtil.GetCurrentDateTime(),
                                            };

                                            lstInsertServiceBillDetail.Add(mObjSIDetail);
                                        }

                                        FuncResponse mObjSDResponse = mObjBLServiceBill.BL_InsertServiceBillItem(lstInsertServiceBillDetail);

                                        if (!mObjSDResponse.Success)
                                        {
                                            throw (mObjSDResponse.Exception);
                                        }
                                    }
                                    else
                                    {
                                        throw (mObjSBSResponse.Exception);
                                    }
                                }

                                scope.Complete();
                                FlashMessage.Info(mObjServiceBillResponse.Message);
                                return RedirectToAction("List", "ServiceBill");
                            }
                            else
                            {
                                UI_FillServiceBillDropDown(pObjServiceBillModel);
                                ViewBag.Message = mObjServiceBillResponse.Message;
                                Transaction.Current.Rollback();
                                return View(pObjServiceBillModel);
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorSignal.FromCurrentContext().Raise(ex);
                            UI_FillServiceBillDropDown(pObjServiceBillModel);
                            ViewBag.Message = "Error occurred while saving service bill";
                            Transaction.Current.Rollback();
                            return View(pObjServiceBillModel);
                        }
                    }
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                BLServiceBill mObjBLServiceBill = new BLServiceBill();
                usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjServiceBillData != null)
                {
                    ServiceBillViewModel mObjServiceBillModel = new ServiceBillViewModel()
                    {
                        ServiceBillID = mObjServiceBillData.ServiceBillID.GetValueOrDefault(),
                        TaxPayerName = mObjServiceBillData.TaxPayerName,
                        TaxPayerRIN = mObjServiceBillData.TaxPayerRIN,
                        SettlementDuedate = mObjServiceBillData.SettlementDueDate.Value,
                        TaxPayerID = mObjServiceBillData.TaxPayerID.GetValueOrDefault(),
                        TaxPayerTypeID = mObjServiceBillData.TaxPayerTypeID.GetValueOrDefault(),
                    };

                    IList<ServiceBill_MDAService> lstServiceBillService = new List<ServiceBill_MDAService>();
                    IList<ServiceBill_MDAServiceItem> lstServiceBillItem = new List<ServiceBill_MDAServiceItem>();

                    IList<MAP_ServiceBill_MDAService> lstMAPServiceBillServices = mObjBLServiceBill.BL_GetMDAServices(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                    IList<MAP_ServiceBill_MDAServiceItem> lstMAPServiceBillItems;
                    foreach (var item in lstMAPServiceBillServices)
                    {
                        ServiceBill_MDAService ServiceBill_MDAService = new ServiceBill_MDAService()
                        {
                            RowID = lstServiceBillService.Count + 1,
                            TablePKID = item.SBSID,
                            MDAServiceID = item.MDAServiceID.GetValueOrDefault(),
                            ServiceAmount = item.ServiceAmount.GetValueOrDefault(),
                            MDAServiceName = item.MDA_Services.MDAServiceCode + " - " + item.MDA_Services.MDAServiceName,
                            TaxYear = item.MDA_Services.TaxYear.GetValueOrDefault(),
                            intTrack = EnumList.Track.EXISTING,
                        };

                        lstServiceBillService.Add(ServiceBill_MDAService);

                        lstMAPServiceBillItems = mObjBLServiceBill.BL_GetServiceBillItems(item.SBSID);

                        foreach (var subitem in lstMAPServiceBillItems)
                        {
                            ServiceBill_MDAServiceItem mObjServiceBillItem = new ServiceBill_MDAServiceItem()
                            {
                                RowID = lstServiceBillItem.Count + 1,
                                MDAService_RowID = ServiceBill_MDAService.RowID,
                                TablePKID = subitem.SBSIID,
                                MDAServiceItemID = subitem.MDAServiceItemID.GetValueOrDefault(),
                                MDAServiceItemName = subitem.MDA_Service_Items.MDAServiceItemName,
                                MDAServiceItemReferenceNo = subitem.MDA_Service_Items.MDAServiceItemReferenceNo,
                                ServiceAmount = subitem.ServiceAmount.GetValueOrDefault(),
                                ServiceBaseAmount = subitem.ServiceBaseAmount.GetValueOrDefault(),
                                ComputationID = subitem.MDA_Service_Items.ComputationID,
                                Percentage = subitem.Percentage.GetValueOrDefault(),
                                intTrack = EnumList.Track.EXISTING
                            };

                            lstServiceBillItem.Add(mObjServiceBillItem);
                        }
                    }



                    SessionManager.lstServiceBillItem = lstServiceBillItem;
                    SessionManager.lstServiceBillService = lstServiceBillService;
                    UI_FillServiceBillDropDown();

                    return View(mObjServiceBillModel);
                }
                else
                {
                    return RedirectToAction("List", "ServiceBill");
                }
            }
            else
            {
                return RedirectToAction("List", "ServiceBill");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(ServiceBillViewModel pObjServiceBillModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillServiceBillDropDown(pObjServiceBillModel);
                return View(pObjServiceBillModel);
            }
            else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    IList<ServiceBill_MDAServiceItem> lstServiceBillItems = SessionManager.lstServiceBillItem ?? new List<ServiceBill_MDAServiceItem>();
                    IList<ServiceBill_MDAService> lstServiceBillServices = SessionManager.lstServiceBillService ?? new List<ServiceBill_MDAService>();

                    int IntServiceBillServiceCount = lstServiceBillServices.Where(t => t.intTrack != EnumList.Track.DELETE).Count();

                    if (IntServiceBillServiceCount == 0)
                    {
                        UI_FillServiceBillDropDown(pObjServiceBillModel);
                        ModelState.AddModelError("ServiceBillService-error", "Please Add Atleast One MDA Service");
                        Transaction.Current.Rollback();
                        return View(pObjServiceBillModel);
                    }
                    else
                    {

                        BLServiceBill mObjBLServiceBill = new BLServiceBill();

                        ServiceBill mObjServiceBill = new ServiceBill()
                        {
                            ServiceBillID = pObjServiceBillModel.ServiceBillID,
                            TaxPayerID = pObjServiceBillModel.TaxPayerID,
                            TaxPayerTypeID = pObjServiceBillModel.TaxPayerTypeID,
                            ServiceBillAmount = lstServiceBillItems.Count > 0 ? lstServiceBillItems.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.ServiceAmount) : 0,
                            ServiceBillDate = CommUtil.GetCurrentDateTime(),
                            SettlementDueDate = pObjServiceBillModel.SettlementDuedate,
                            SettlementStatusID = 2,
                            Active = true,
                            CreatedBy = SessionManager.SystemUserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        try
                        {

                            FuncResponse<ServiceBill> mObjServiceBillResponse = mObjBLServiceBill.BL_InsertUpdateServiceBill(mObjServiceBill);

                            if (mObjServiceBillResponse.Success)
                            {
                                //Adding MDA Service

                                foreach (ServiceBill_MDAService mObjSBS in lstServiceBillServices)
                                {
                                    if (mObjSBS.intTrack == EnumList.Track.INSERT)
                                    {
                                        MAP_ServiceBill_MDAService mObjMDAService = new MAP_ServiceBill_MDAService()
                                        {
                                            ServiceBillID = mObjServiceBillResponse.AdditionalData.ServiceBillID,
                                            MDAServiceID = mObjSBS.MDAServiceID,
                                            ServiceAmount = mObjSBS.ServiceAmount,
                                            ServiceBillYear = mObjSBS.TaxYear,
                                            CreatedBy = SessionManager.SystemUserID,
                                            CreatedDate = CommUtil.GetCurrentDateTime()
                                        };

                                        FuncResponse<MAP_ServiceBill_MDAService> mObjSBSResponse = mObjBLServiceBill.BL_InsertUpdateMDAService(mObjMDAService);

                                        if (mObjSBSResponse.Success)
                                        {
                                            IList<MAP_ServiceBill_MDAServiceItem> lstInsertServiceBillDetail = new List<MAP_ServiceBill_MDAServiceItem>();

                                            foreach (ServiceBill_MDAServiceItem mObjServiceBillItemDetail in lstServiceBillItems.Where(t => t.MDAService_RowID == mObjSBS.RowID))
                                            {
                                                MAP_ServiceBill_MDAServiceItem mObjSIDetail = new MAP_ServiceBill_MDAServiceItem()
                                                {
                                                    SBSID = mObjSBSResponse.AdditionalData.SBSID,
                                                    MDAServiceItemID = mObjServiceBillItemDetail.MDAServiceItemID,
                                                    ServiceBaseAmount = mObjServiceBillItemDetail.ServiceBaseAmount,
                                                    ServiceAmount = mObjServiceBillItemDetail.ServiceAmount,
                                                    Percentage = mObjServiceBillItemDetail.Percentage,
                                                    PaymentStatusID = (int)EnumList.PaymentStatus.Due,
                                                    CreatedBy = SessionManager.SystemUserID,
                                                    CreatedDate = CommUtil.GetCurrentDateTime(),
                                                };

                                                lstInsertServiceBillDetail.Add(mObjSIDetail);
                                            }

                                            FuncResponse mObjSDResponse = mObjBLServiceBill.BL_InsertServiceBillItem(lstInsertServiceBillDetail);

                                            if (!mObjSDResponse.Success)
                                            {
                                                throw (mObjSDResponse.Exception);
                                            }
                                        }
                                        else
                                        {
                                            throw (mObjSBSResponse.Exception);
                                        }
                                    }
                                    else if (mObjSBS.intTrack == EnumList.Track.DELETE)
                                    {
                                        FuncResponse mObjMSResponse = mObjBLServiceBill.BL_DeleteMDAService(mObjSBS.TablePKID);

                                        if (!mObjMSResponse.Success)
                                        {
                                            throw (mObjMSResponse.Exception);
                                        }
                                    }
                                }

                                scope.Complete();
                                FlashMessage.Info(mObjServiceBillResponse.Message);
                                return RedirectToAction("List", "ServiceBill");
                            }
                            else
                            {
                                UI_FillServiceBillDropDown(pObjServiceBillModel);
                                ViewBag.Message = mObjServiceBillResponse.Message;
                                Transaction.Current.Rollback();
                                return View(pObjServiceBillModel);
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorSignal.FromCurrentContext().Raise(ex);
                            UI_FillServiceBillDropDown(pObjServiceBillModel);
                            ViewBag.Message = "Error occurred while updating service bill";
                            Transaction.Current.Rollback();
                            return View(pObjServiceBillModel);
                        }
                    }
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                BLServiceBill mObjBLServiceBill = new BLServiceBill();
                usp_GetServiceBillList_Result mObjServiceBillDetails = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjServiceBillDetails != null)
                {
                    IList<usp_GetServiceBillItemList_Result> lstServiceBillItems = mObjBLServiceBill.BL_GetServiceBillItem(mObjServiceBillDetails.ServiceBillID.GetValueOrDefault());

                    ViewBag.ServiceBillItemList = lstServiceBillItems;

                    return View(mObjServiceBillDetails);
                }
                else
                {
                    return RedirectToAction("List", "ServiceBill");
                }
            }
            else
            {
                return RedirectToAction("List", "ServiceBill");
            }
        }

        public JsonResult UpdateStatus(ServiceBill pObjServiceBillData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjServiceBillData.ServiceBillID != 0)
            {
                FuncResponse mObjFuncResponse = new BLServiceBill().BL_UpdateStatus(pObjServiceBillData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["ServiceBillList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaxPayerList(int TaxPayerTypeID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
            {
                Individual mObjIndividual = new Individual()
                {
                    intStatus = 2
                };

                IList<usp_GetIndividualList_Result> lstIndividual = new BLIndividual().BL_GetIndividualList(mObjIndividual);
                dcResponse["success"] = true;

                dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindIndividualTable_SingleSelect", lstIndividual, this.ControllerContext);

            }
            else if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
            {
                Company mObjCompany = new Company()
                {
                    intStatus = 2
                };

                IList<usp_GetCompanyList_Result> lstCompany = new BLCompany().BL_GetCompanyList(mObjCompany);
                dcResponse["success"] = true;

                dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindCompanyTable_SingleSelect", lstCompany, this.ControllerContext);
            }
            else if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
            {
                Government mObjGovernment = new Government()
                {
                    intStatus = 2
                };

                IList<usp_GetGovernmentList_Result> lstGovernment = new BLGovernment().BL_GetGovernmentList(mObjGovernment);
                dcResponse["success"] = true;

                dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindGovernmentTable_SingleSelect", lstGovernment, this.ControllerContext);

            }
            else if (TaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
            {
                Special mObjSpecial = new Special()
                {
                    intStatus = 2
                };

                IList<usp_GetSpecialList_Result> lstSpecial = new BLSpecial().BL_GetSpecialList(mObjSpecial);
                dcResponse["success"] = true;

                dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindSpecialTable_SingleSelect", lstSpecial, this.ControllerContext);
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

    }
}