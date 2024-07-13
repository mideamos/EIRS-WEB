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
using System.Linq.Dynamic;
using System.Text;

namespace EIRS.Admin.Controllers
{
    public class MDAServiceController : BaseController
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

            if (!string.IsNullOrWhiteSpace(Request.Form["MDAServiceCode"]))
            {
                sbWhereCondition.Append(" AND ISNULL(MDAServiceCode,'') LIKE @MDAServiceCode");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["MDAServiceName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(MDAServiceName,'') LIKE @MDAServiceName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["RuleRunName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(RuleRunName,'') LIKE @RuleRunName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["PaymentFrequencyName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(PaymentFrequencyName,'') LIKE @PaymentFrequencyName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["TaxYear"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),TaxYear,106),'') LIKE @TaxYear");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["PaymentOptionName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(PaymentOptionName,'') LIKE @PaymentOptionName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ServiceAmount"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),ServiceAmount,106),'') LIKE @ServiceAmount");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["SettlementMethodName"]))
            {
                sbWhereCondition.Append(" AND EXISTS(SELECT * FROM MAP_MDAService_SettlementMethod mssmthd ");
                sbWhereCondition.Append(" INNER JOIN Settlement_Method smthd ");
                sbWhereCondition.Append(" ON mssmthd.SettlementMethodID = smthd.SettlementMethodID ");
                sbWhereCondition.Append(" WHERE mssmthd.MDAServiceID = mdaser.MDAServiceID ");
                sbWhereCondition.Append(" AND ISNULL(smthd.SettlementMethodName, '') LIKE @SettlementMethodNames) ");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["MDAServiceItemName"]))
            {
                sbWhereCondition.Append(" AND EXISTS (SELECT * FROM MAP_MDAService_MDAServiceItem msitem ");
                sbWhereCondition.Append(" INNER JOIN MDA_Service_Items aitem ON msitem.MDAServiceItemID = aitem.MDAServiceItemID ");
                sbWhereCondition.Append(" WHERE msitem.MDAServiceID = mdaser.MDAServiceID ");
                sbWhereCondition.Append(" AND ISNULL(aitem.MDAServiceItemName, '') LIKE @MDAServiceItemNames) ");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ActiveText"]))
            {
                sbWhereCondition.Append(" AND CASE WHEN ISNULL(mdaser.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @ActiveText");
            }

            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( ISNULL(MDAServiceCode,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(MDAServiceName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(RuleRunName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(PaymentFrequencyName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),TaxYear,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(PaymentOptionName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),ServiceAmount,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR EXISTS(SELECT * FROM MAP_MDAService_SettlementMethod mssmthd ");
                sbWhereCondition.Append(" INNER JOIN Settlement_Method smthd ");
                sbWhereCondition.Append(" ON mssmthd.SettlementMethodID = smthd.SettlementMethodID ");
                sbWhereCondition.Append(" WHERE mssmthd.MDAServiceID = mdaser.MDAServiceID ");
                sbWhereCondition.Append(" AND ISNULL(smthd.SettlementMethodName, '') LIKE @MainFilter)  ");
                sbWhereCondition.Append(" OR EXISTS (SELECT * FROM MAP_MDAService_MDAServiceItem msitem ");
                sbWhereCondition.Append(" INNER JOIN MDA_Service_Items aitem ON msitem.MDAServiceItemID = aitem.MDAServiceItemID ");
                sbWhereCondition.Append(" WHERE msitem.MDAServiceID = mdaser.MDAServiceID ");
                sbWhereCondition.Append(" AND ISNULL(aitem.MDAServiceItemName, '') LIKE @MainFilter) ");
                sbWhereCondition.Append(" OR CASE WHEN ISNULL(mdaser.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @MainFilter)");
            }

            MDA_Services mObjMDAServices = new MDA_Services()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),

                MDAServiceCode = !string.IsNullOrWhiteSpace(Request.Form["MDAServiceCode"]) ? "%" + Request.Form["MDAServiceCode"].Trim() + "%" : TrynParse.parseString(Request.Form["MDAServiceCode"]),
                MDAServiceName = !string.IsNullOrWhiteSpace(Request.Form["MDAServiceName"]) ? "%" + Request.Form["MDAServiceName"].Trim() + "%" : TrynParse.parseString(Request.Form["MDAServiceName"]),
                RuleRunName = !string.IsNullOrWhiteSpace(Request.Form["RuleRunName"]) ? "%" + Request.Form["RuleRunName"].Trim() + "%" : TrynParse.parseString(Request.Form["RuleRunName"]),
                PaymentFrequencyName = !string.IsNullOrWhiteSpace(Request.Form["PaymentFrequencyName"]) ? "%" + Request.Form["PaymentFrequencyName"].Trim() + "%" : TrynParse.parseString(Request.Form["PaymentFrequencyName"]),
                MDAServiceItemName = !string.IsNullOrWhiteSpace(Request.Form["MDAServiceItemName"]) ? "%" + Request.Form["MDAServiceItemName"].Trim() + "%" : TrynParse.parseString(Request.Form["MDAServiceItemName"]),
                StrServiceAmount = !string.IsNullOrWhiteSpace(Request.Form["ServiceAmount"]) ? "%" + Request.Form["ServiceAmount"].Trim() + "%" : TrynParse.parseString(Request.Form["ServiceAmount"]),
                StrTaxYear = !string.IsNullOrWhiteSpace(Request.Form["TaxYear"]) ? "%" + Request.Form["TaxYear"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxYear"]),
                SettlementMethodName = !string.IsNullOrWhiteSpace(Request.Form["SettlementMethodName"]) ? "%" + Request.Form["SettlementMethodName"].Trim() + "%" : TrynParse.parseString(Request.Form["SettlementMethodName"]),
                PaymentOptionName = !string.IsNullOrWhiteSpace(Request.Form["PaymentOptionName"]) ? "%" + Request.Form["PaymentOptionName"].Trim() + "%" : TrynParse.parseString(Request.Form["PaymentOptionName"]),
                ActiveText = !string.IsNullOrWhiteSpace(Request.Form["ActiveText"]) ? "%" + Request.Form["ActiveText"].Trim() + "%" : TrynParse.parseString(Request.Form["ActiveText"]),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };


            IDictionary<string, object> dcData = new BLMDAService().BL_SearchMDAService(mObjMDAServices);
            IList<usp_SearchMDAService_Result> lstMDAService = (IList<usp_SearchMDAService_Result>)dcData["MDAServiceList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstMDAService
            }, JsonRequestBehavior.AllowGet);
        }

        private void UI_FillDropDown(MDAServiceViewModel pObjMDAServiceModel = null)
        {
            if (pObjMDAServiceModel == null)
                pObjMDAServiceModel = new MDAServiceViewModel();

            UI_FillRuleRun();
            UI_FillPaymentFrequencyDropDown(new Payment_Frequency() { intStatus = 1, IncludePaymentFrequencyIds = pObjMDAServiceModel.PaymentFrequencyID.ToString() });
            UI_FillPaymentOptionDropDown(new Payment_Options() { intStatus = 1, IncludePaymentOptionIds = pObjMDAServiceModel.PaymentOptionID.ToString() });
            UI_FillSettlementMethodDropDown(new Settlement_Method() { intStatus = 1 });
            UI_FillYearDropDown();


            ViewBag.MSMSItemList = SessionManager.lstMDAServiceItem;

            MDA_Service_Items mObjMDAServiceItem = new MDA_Service_Items()
            {
                intStatus = 1
            };

            IList<usp_GetMDAServiceItemList_Result> lstMDAServiceItem = new BLMDAServiceItem().BL_GetMDAServiceItemList(mObjMDAServiceItem);
            ViewBag.MDAServiceItemList = lstMDAServiceItem;

        }

        public ActionResult Add()
        {
            SessionManager.lstMDAServiceItem = new List<MDAService_MDAServiceItem>();
            UI_FillDropDown();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(MDAServiceViewModel pObjMDAServiceModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjMDAServiceModel);
                return View(pObjMDAServiceModel);
            }
            else
            {
                IList<MDAService_MDAServiceItem> lstMDAServiceItems = SessionManager.lstMDAServiceItem ?? new List<MDAService_MDAServiceItem>();

                int IntMDAServiceItemCount = lstMDAServiceItems.Where(t => t.intTrack != EnumList.Track.DELETE).Count();
                
                if (IntMDAServiceItemCount == 0)
                {
                    UI_FillDropDown(pObjMDAServiceModel);
                    ModelState.AddModelError("MDAServiceItem-error", "Please Link MDA Service Item");
                    return View(pObjMDAServiceModel);
                }
                else
                {
                    using (TransactionScope mObjTransactionScope = new TransactionScope())
                    {
                        BLMDAService mObjBLMDAService = new BLMDAService();

                        MDA_Services mObjMDAService = new MDA_Services()
                        {
                            MDAServiceID = 0,
                            MDAServiceName = pObjMDAServiceModel.MDAServiceName.Trim(),
                            RuleRunID = pObjMDAServiceModel.RuleRunID,
                            PaymentFrequencyID = pObjMDAServiceModel.PaymentFrequencyID,
                            TaxYear = pObjMDAServiceModel.TaxYear,
                            PaymentOptionID = pObjMDAServiceModel.PaymentOptionID,
                            ServiceAmount = lstMDAServiceItems != null ? lstMDAServiceItems.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.ServiceAmount) : 0,
                            Active = true,
                            CreatedBy = SessionManager.SystemUserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        try
                        {

                            FuncResponse<MDA_Services> mObjResponse = mObjBLMDAService.BL_InsertUpdateMDAService(mObjMDAService);

                            if (mObjResponse.Success)
                            {
                                if (pObjMDAServiceModel.SettlementMethodIds != null && pObjMDAServiceModel.SettlementMethodIds.Length > 0)
                                {
                                    foreach (int intSettlementMethodID in pObjMDAServiceModel.SettlementMethodIds)
                                    {
                                        MAP_MDAService_SettlementMethod mObjSettlementMethod = new MAP_MDAService_SettlementMethod()
                                        {
                                            SettlementMethodID = intSettlementMethodID,
                                            MDAServiceID = mObjResponse.AdditionalData.MDAServiceID,
                                            CreatedBy = SessionManager.SystemUserID,
                                            CreatedDate = DateTime.Now
                                        };

                                        mObjBLMDAService.BL_InsertSettlementMethod(mObjSettlementMethod);
                                    }
                                }

                                if (lstMDAServiceItems.Where(t => t.intTrack != EnumList.Track.DELETE).Count() > 0)
                                {
                                    foreach (var MDAServiceitem in lstMDAServiceItems)
                                    {
                                        if (MDAServiceitem.intTrack != EnumList.Track.DELETE)
                                        {
                                            MAP_MDAService_MDAServiceItem mObjAssessmenstItem = new MAP_MDAService_MDAServiceItem()
                                            {
                                                MDAServiceItemID = MDAServiceitem.MDAServiceItemID,
                                                MDAServiceID = mObjResponse.AdditionalData.MDAServiceID,
                                                CreatedBy = SessionManager.SystemUserID,
                                                CreatedDate = DateTime.Now
                                            };

                                            mObjBLMDAService.BL_InsertMDAServiceItem(mObjAssessmenstItem);
                                        }
                                    }
                                }

                                mObjTransactionScope.Complete();
                                FlashMessage.Info(mObjResponse.Message);
                                return RedirectToAction("List", "MDAService");
                            }
                            else
                            {
                                UI_FillDropDown(pObjMDAServiceModel);
                                Transaction.Current.Rollback();
                                ViewBag.Message = mObjResponse.Message;
                                return View(pObjMDAServiceModel);
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorSignal.FromCurrentContext().Raise(ex);
                            UI_FillDropDown(pObjMDAServiceModel);
                            Transaction.Current.Rollback();
                            ViewBag.Message = "Error occurred while saving mda service";
                            return View(pObjMDAServiceModel);
                        }
                    }
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                MDA_Services mObjMDAService = new MDA_Services()
                {
                    MDAServiceID = id.GetValueOrDefault(),
                    IntStatus = 2
                };

                usp_GetMDAServiceList_Result mObjMDAServiceData = new BLMDAService().BL_GetMDAServiceDetails(mObjMDAService);

                if (mObjMDAServiceData != null)
                {
                    MDAServiceViewModel mObjMDAServiceModelView = new MDAServiceViewModel()
                    {
                        MDAServiceID = mObjMDAServiceData.MDAServiceID.GetValueOrDefault(),
                        MDAServiceCode = mObjMDAServiceData.MDAServiceCode,
                        MDAServiceName = mObjMDAServiceData.MDAServiceName,
                        RuleRunID = mObjMDAServiceData.RuleRunID.GetValueOrDefault(),
                        PaymentFrequencyID = mObjMDAServiceData.PaymentFrequencyID.GetValueOrDefault(),
                        TaxYear = mObjMDAServiceData.TaxYear.GetValueOrDefault(),
                        SettlementMethodIds = TrynParse.parseIntArray(mObjMDAServiceData.SettlementMethodIds),
                        PaymentOptionID = mObjMDAServiceData.PaymentOptionID.GetValueOrDefault(),
                        Active = mObjMDAServiceData.Active.GetValueOrDefault(),
                    };

                    SessionManager.lstMDAServiceItem = new List<MDAService_MDAServiceItem>();

                    IList<MAP_MDAService_MDAServiceItem> lstMDAService_MSItem = new BLMDAService().BL_GetMDAServiceItem(mObjMDAServiceData.MDAServiceID.GetValueOrDefault());

                    if (lstMDAService_MSItem != null && lstMDAService_MSItem.Count > 0)
                    {
                        IList<MDAService_MDAServiceItem> lstMDAServiceItems = new List<MDAService_MDAServiceItem>();
                        string[] strArrMDAServiceItemIds = mObjMDAServiceData.MDAServiceItemIds.Split(',');

                        BLMDAServiceItem mObjBLMDAServiceItem = new BLMDAServiceItem();

                        foreach (var vMDAServiceItem in lstMDAService_MSItem)
                        {
                            if (vMDAServiceItem.MDAServiceItemID.GetValueOrDefault() > 0)
                            {
                                usp_GetMDAServiceItemList_Result mObjMDAServiceItem = mObjBLMDAServiceItem.BL_GetMDAServiceItemDetails(new MDA_Service_Items() { intStatus = 2, MDAServiceItemID = vMDAServiceItem.MDAServiceItemID.GetValueOrDefault() });

                                MDAService_MDAServiceItem mObjMSMSItem = new MDAService_MDAServiceItem()
                                {
                                    RowID = lstMDAServiceItems.Count + 1
                                };

                                mObjMSMSItem.TablePKID = vMDAServiceItem.MSMSIID;
                                mObjMSMSItem.MDAServiceItemID = mObjMDAServiceItem.MDAServiceItemID.GetValueOrDefault();
                                mObjMSMSItem.MDAServiceItemReferenceNo = mObjMDAServiceItem.MDAServiceItemReferenceNo;
                                mObjMSMSItem.MDAServiceItemName = mObjMDAServiceItem.MDAServiceItemName;
                                mObjMSMSItem.ServiceAmount = mObjMDAServiceItem.ServiceAmount.GetValueOrDefault();
                                mObjMSMSItem.intTrack = EnumList.Track.EXISTING;

                                lstMDAServiceItems.Add(mObjMSMSItem);
                            }
                        }

                        SessionManager.lstMDAServiceItem = lstMDAServiceItems;

                    }


                    UI_FillDropDown(mObjMDAServiceModelView);
                    return View(mObjMDAServiceModelView);
                }
                else
                {
                    return RedirectToAction("List", "MDAService");
                }
            }
            else
            {
                return RedirectToAction("List", "MDAService");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(MDAServiceViewModel pObjMDAServiceModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjMDAServiceModel);
                return View(pObjMDAServiceModel);
            }
            else
            {
                IList<MDAService_MDAServiceItem> lstMDAServiceItems = SessionManager.lstMDAServiceItem ?? new List<MDAService_MDAServiceItem>();

                int IntMDAServiceItemCount = lstMDAServiceItems.Where(t => t.intTrack != EnumList.Track.DELETE).Count();
             
                if (IntMDAServiceItemCount == 0)
                {
                    UI_FillDropDown(pObjMDAServiceModel);
                    ModelState.AddModelError("MDAServiceItem-error", "Please Link MDA Service Item");
                    return View(pObjMDAServiceModel);
                }
                else
                {
                    using (TransactionScope mObjTransactionScope = new TransactionScope())
                    {
                        BLMDAService mObjBLMDAService = new BLMDAService();

                        MDA_Services mObjMDAService = new MDA_Services()
                        {
                            MDAServiceID = pObjMDAServiceModel.MDAServiceID,
                            MDAServiceName = pObjMDAServiceModel.MDAServiceName.Trim(),
                            RuleRunID = pObjMDAServiceModel.RuleRunID,
                            PaymentFrequencyID = pObjMDAServiceModel.PaymentFrequencyID,
                            TaxYear = pObjMDAServiceModel.TaxYear,
                            PaymentOptionID = pObjMDAServiceModel.PaymentOptionID,
                            ServiceAmount = lstMDAServiceItems != null ? lstMDAServiceItems.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.ServiceAmount) : 0,
                            Active = pObjMDAServiceModel.Active,
                            ModifiedBy = SessionManager.SystemUserID,
                            ModifiedDate = CommUtil.GetCurrentDateTime()
                        };

                        try
                        {

                            FuncResponse<MDA_Services> mObjResponse = mObjBLMDAService.BL_InsertUpdateMDAService(mObjMDAService);

                            if (mObjResponse.Success)
                            {

                                IList<MAP_MDAService_SettlementMethod> lstOldSettlementMethod = mObjBLMDAService.BL_GetSettlementMethod(pObjMDAServiceModel.MDAServiceID);

                                int[] intRemovedSettlementMethod = new int[] { };
                                int[] intAddedSettlementMethod = new int[] { };

                                if (pObjMDAServiceModel.SettlementMethodIds == null)
                                {
                                    intRemovedSettlementMethod = lstOldSettlementMethod.Select(t => t.ARSMID).ToArray();
                                }
                                else
                                {
                                    intRemovedSettlementMethod = lstOldSettlementMethod.Where(t => !pObjMDAServiceModel.SettlementMethodIds.Contains(t.SettlementMethodID.GetValueOrDefault())).Select(t => t.ARSMID).ToArray();

                                    if (lstOldSettlementMethod == null || lstOldSettlementMethod.Count() == 0)
                                    {
                                        intAddedSettlementMethod = pObjMDAServiceModel.SettlementMethodIds;
                                    }
                                    else
                                    {
                                        int[] intSettlementMethodID = lstOldSettlementMethod.Select(t => t.SettlementMethodID.GetValueOrDefault()).ToArray();
                                        intAddedSettlementMethod = pObjMDAServiceModel.SettlementMethodIds.Except(intSettlementMethodID).ToArray();
                                    }
                                }

                                foreach (int intARSMID in intRemovedSettlementMethod)
                                {
                                    MAP_MDAService_SettlementMethod mObjSettlementMethod = new MAP_MDAService_SettlementMethod()
                                    {
                                        ARSMID = intARSMID
                                    };

                                    mObjBLMDAService.BL_RemoveSettlementMethod(mObjSettlementMethod);
                                }

                                foreach (int intSettlementMethodID in intAddedSettlementMethod)
                                {
                                    MAP_MDAService_SettlementMethod mObjSettlementMethod = new MAP_MDAService_SettlementMethod()
                                    {
                                        MDAServiceID = pObjMDAServiceModel.MDAServiceID,
                                        SettlementMethodID = intSettlementMethodID,
                                        CreatedBy = SessionManager.SystemUserID,
                                        CreatedDate = DateTime.Now
                                    };

                                    mObjBLMDAService.BL_InsertSettlementMethod(mObjSettlementMethod);
                                }


                                foreach (var MDAServiceitem in lstMDAServiceItems)
                                {
                                    if (MDAServiceitem.intTrack == EnumList.Track.INSERT)
                                    {
                                        MAP_MDAService_MDAServiceItem mObjAssessmenstItem = new MAP_MDAService_MDAServiceItem()
                                        {
                                            MDAServiceItemID = MDAServiceitem.MDAServiceItemID,
                                            MDAServiceID = mObjResponse.AdditionalData.MDAServiceID,
                                            CreatedBy = SessionManager.SystemUserID,
                                            CreatedDate = DateTime.Now
                                        };

                                        mObjBLMDAService.BL_InsertMDAServiceItem(mObjAssessmenstItem);
                                    }
                                    else if (MDAServiceitem.intTrack == EnumList.Track.DELETE)
                                    {
                                        MAP_MDAService_MDAServiceItem mObjAssessmenstItem = new MAP_MDAService_MDAServiceItem()
                                        {
                                            MSMSIID = MDAServiceitem.TablePKID
                                        };

                                        mObjBLMDAService.BL_RemoveMDAServiceItem(mObjAssessmenstItem);
                                    }
                                }


                                mObjTransactionScope.Complete();
                                FlashMessage.Info(mObjResponse.Message);
                                return RedirectToAction("List", "MDAService");
                            }
                            else
                            {
                                UI_FillDropDown(pObjMDAServiceModel);
                                Transaction.Current.Rollback();
                                ViewBag.Message = mObjResponse.Message;
                                return View(pObjMDAServiceModel);
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrorSignal.FromCurrentContext().Raise(ex);
                            UI_FillDropDown(pObjMDAServiceModel);
                            Transaction.Current.Rollback();
                            ViewBag.Message = "Error occurred while updating mda service";
                            return View(pObjMDAServiceModel);
                        }
                    }
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                MDA_Services mObjMDAService = new MDA_Services()
                {
                    MDAServiceID = id.GetValueOrDefault(),
                    IntStatus = 2
                };

                usp_GetMDAServiceList_Result mObjMDAServiceData = new BLMDAService().BL_GetMDAServiceDetails(mObjMDAService);

                if (mObjMDAServiceData != null)
                {
                    MDAServiceViewModel mObjMDAServiceModelView = new MDAServiceViewModel()
                    {
                        MDAServiceID = mObjMDAServiceData.MDAServiceID.GetValueOrDefault(),
                        MDAServiceCode = mObjMDAServiceData.MDAServiceCode,
                        MDAServiceName = mObjMDAServiceData.MDAServiceName,
                        RuleRunName = mObjMDAServiceData.RuleRunName,
                        PaymentFrequencyName = mObjMDAServiceData.PaymentFrequencyName,
                        TaxYear = mObjMDAServiceData.TaxYear.GetValueOrDefault(),
                        SettlementMethodNames = mObjMDAServiceData.SettlementMethodNames,
                        PaymentOptionName = mObjMDAServiceData.PaymentOptionName,
                        ActiveText = mObjMDAServiceData.ActiveText,
                    };

                    SessionManager.lstMDAServiceItem = new List<MDAService_MDAServiceItem>();

                    IList<MAP_MDAService_MDAServiceItem> lstMDAService_AItem = new BLMDAService().BL_GetMDAServiceItem(mObjMDAServiceData.MDAServiceID.GetValueOrDefault());
                    if (lstMDAService_AItem != null && lstMDAService_AItem.Count > 0)
                    {
                        IList<MDAService_MDAServiceItem> lstMDAServiceItems = new List<MDAService_MDAServiceItem>();

                        BLMDAServiceItem mObjBLMDAServiceItem = new BLMDAServiceItem();

                        foreach (var vMDAServiceItem in lstMDAService_AItem)
                        {
                            if (vMDAServiceItem.MDAServiceItemID.GetValueOrDefault() > 0)
                            {
                                usp_GetMDAServiceItemList_Result mObjMDAServiceItem = mObjBLMDAServiceItem.BL_GetMDAServiceItemDetails(new MDA_Service_Items() { intStatus = 2, MDAServiceItemID = vMDAServiceItem.MDAServiceItemID.GetValueOrDefault() });

                                MDAService_MDAServiceItem mObjMSMSItem = new MDAService_MDAServiceItem()
                                {
                                    RowID = lstMDAServiceItems.Count + 1
                                };

                                mObjMSMSItem.TablePKID = vMDAServiceItem.MSMSIID;
                                mObjMSMSItem.MDAServiceItemID = mObjMDAServiceItem.MDAServiceItemID.GetValueOrDefault();
                                mObjMSMSItem.MDAServiceItemName = mObjMDAServiceItem.MDAServiceItemName;
                                mObjMSMSItem.ServiceAmount = mObjMDAServiceItem.ServiceAmount.GetValueOrDefault();
                                mObjMSMSItem.intTrack = EnumList.Track.EXISTING;

                                lstMDAServiceItems.Add(mObjMSMSItem);
                            }
                        }

                        SessionManager.lstMDAServiceItem = lstMDAServiceItems;

                    }

                    ViewBag.MDAServiceItemList = SessionManager.lstMDAServiceItem;
                    
                    return View(mObjMDAServiceModelView);
                }
                else
                {
                    return RedirectToAction("List", "MDAService");
                }
            }
            else
            {
                return RedirectToAction("List", "MDAService");
            }
        }

        public ActionResult MDAServiceItemDetails(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                IList<MAP_MDAService_MDAServiceItem> lstMDAService_AItem = new BLMDAService().BL_GetMDAServiceItem(id.GetValueOrDefault());
                if (lstMDAService_AItem != null && lstMDAService_AItem.Count > 0)
                {
                    IList<MDAService_MDAServiceItem> lstMDAServiceItems = new List<MDAService_MDAServiceItem>();

                    BLMDAServiceItem mObjBLMDAServiceItem = new BLMDAServiceItem();

                    foreach (var vMDAServiceItem in lstMDAService_AItem)
                    {
                        if (vMDAServiceItem.MDAServiceItemID.GetValueOrDefault() > 0)
                        {
                            usp_GetMDAServiceItemList_Result mObjMDAServiceItem = mObjBLMDAServiceItem.BL_GetMDAServiceItemDetails(new MDA_Service_Items() { intStatus = 2, MDAServiceItemID = vMDAServiceItem.MDAServiceItemID.GetValueOrDefault() });

                            MDAService_MDAServiceItem mObjMSMSItem = new MDAService_MDAServiceItem()
                            {
                                RowID = lstMDAServiceItems.Count + 1
                            };

                            mObjMSMSItem.TablePKID = vMDAServiceItem.MSMSIID;
                            mObjMSMSItem.MDAServiceItemID = mObjMDAServiceItem.MDAServiceItemID.GetValueOrDefault();
                            mObjMSMSItem.MDAServiceItemReferenceNo = mObjMDAServiceItem.MDAServiceItemReferenceNo;
                            mObjMSMSItem.MDAServiceItemName = mObjMDAServiceItem.MDAServiceItemName;
                            mObjMSMSItem.ServiceAmount = mObjMDAServiceItem.ServiceAmount.GetValueOrDefault();
                            mObjMSMSItem.intTrack = EnumList.Track.EXISTING;

                            lstMDAServiceItems.Add(mObjMSMSItem);
                        }
                    }

                    return View(lstMDAServiceItems);

                }
                else
                {
                    return RedirectToAction("List", "MDAService");
                }
            }
            else
            {
                return RedirectToAction("List", "MDAService");
            }
        }

        public JsonResult UpdateStatus(MDA_Services pObjMDAServiceData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjMDAServiceData.MDAServiceID != 0)
            {
                FuncResponse mObjFuncResponse = new BLMDAService().BL_UpdateStatus(pObjMDAServiceData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["MDAServiceList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult AddMDAServiceItem(string MDAServiceItemIds)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();
            IList<MDAService_MDAServiceItem> lstMDAServiceItem = SessionManager.lstMDAServiceItem ?? new List<MDAService_MDAServiceItem>();

            string[] strArrMDAServiceItemIds = MDAServiceItemIds.Split(',');

            if (strArrMDAServiceItemIds.Length > 0)
            {
                BLMDAServiceItem mObjBLMDAServiceItem = new BLMDAServiceItem();
                foreach (string strMDAServiceItem in strArrMDAServiceItemIds)
                {
                    if (!string.IsNullOrEmpty(strMDAServiceItem))
                    {
                        usp_GetMDAServiceItemList_Result mObjMDAServiceItem = mObjBLMDAServiceItem.BL_GetMDAServiceItemDetails(new MDA_Service_Items() { intStatus = 2, MDAServiceItemID = TrynParse.parseInt(strMDAServiceItem) });

                        MDAService_MDAServiceItem mObjMSMSItem;

                        if (lstMDAServiceItem.Where(t => t.intTrack != EnumList.Track.DELETE && t.MDAServiceItemID == mObjMDAServiceItem.MDAServiceItemID).Count() == 0)
                        {
                            mObjMSMSItem = new MDAService_MDAServiceItem()
                            {
                                RowID = lstMDAServiceItem.Count + 1,
                                intTrack = EnumList.Track.INSERT
                            };
                        }
                        else
                        {
                            mObjMSMSItem = lstMDAServiceItem.Where(t => t.intTrack != EnumList.Track.DELETE && t.MDAServiceItemID == mObjMDAServiceItem.MDAServiceItemID).FirstOrDefault();
                            mObjMSMSItem.intTrack = EnumList.Track.UPDATE;
                        }


                        mObjMSMSItem.MDAServiceItemID = mObjMDAServiceItem.MDAServiceItemID.GetValueOrDefault();
                        mObjMSMSItem.MDAServiceItemReferenceNo = mObjMDAServiceItem.MDAServiceItemReferenceNo;
                        mObjMSMSItem.MDAServiceItemName = mObjMDAServiceItem.MDAServiceItemName;
                        mObjMSMSItem.ServiceAmount = mObjMDAServiceItem.ServiceAmount.GetValueOrDefault();

                        if (lstMDAServiceItem.Where(t => t.intTrack != EnumList.Track.DELETE && t.MDAServiceItemID == mObjMDAServiceItem.MDAServiceItemID).Count() == 0)
                            lstMDAServiceItem.Add(mObjMSMSItem);

                    }
                }
            }


            SessionManager.lstMDAServiceItem = lstMDAServiceItem;

            dcResponse["success"] = true;
            dcResponse["MDAServiceItemList"] = CommUtil.RenderPartialToString("_BindMDAServiceItem", lstMDAServiceItem.Where(t => t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
            dcResponse["MDAServiceItemCount"] = lstMDAServiceItem.Where(t => t.intTrack != EnumList.Track.DELETE).Count();
            dcResponse["Message"] = "MDA Service Item Added";
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveMDAServiceItem(int RowID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();
            IList<MDAService_MDAServiceItem> lstMDAServiceItem = SessionManager.lstMDAServiceItem ?? new List<MDAService_MDAServiceItem>();
            MDAService_MDAServiceItem mObjMDAServiceItem = lstMDAServiceItem.Where(t => t.RowID == RowID).FirstOrDefault();

            if (mObjMDAServiceItem != null)
            {
                mObjMDAServiceItem.intTrack = EnumList.Track.DELETE;
                SessionManager.lstMDAServiceItem = lstMDAServiceItem;

                dcResponse["success"] = true;
                dcResponse["MDAServiceItemList"] = CommUtil.RenderPartialToString("_BindMDAServiceItem", lstMDAServiceItem.Where(t => t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
                dcResponse["MDAServiceItemCount"] = lstMDAServiceItem.Where(t => t.intTrack != EnumList.Track.DELETE).Count();
                dcResponse["Message"] = "Profile Removed";

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