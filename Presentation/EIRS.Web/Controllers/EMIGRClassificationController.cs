using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Vereyon.Web;
using Elmah;
using System.Linq;
using System.Linq.Dynamic;
using EIRS.Web.Utility;

namespace EIRS.Web.Controllers
{
    public class EMIGRClassificationController : BaseController
    {
        // GET: EMIGRClassification
        [HttpGet]
        public ActionResult List()
        {
            EM_IGRClassification mObjIGRClassification = new EM_IGRClassification()
            {
                intStatus = 2
            };

            IList<usp_EM_GetIGRClassificationList_Result> lstIGRClassification = new BLEMIGRClassification().BL_GetIGRClassificationList(mObjIGRClassification);
            return View(lstIGRClassification);
        }

        public void UI_FillDropDown(EMIGRClassificationViewModel pObjIGRClassificationModel = null)
        {
            if (pObjIGRClassificationModel == null)
            {
                pObjIGRClassificationModel = new EMIGRClassificationViewModel()
                {
                    CategoryID = -1,
                    RevenueHeadID = -1,
                };
            }

            UI_FillEMCategoryDropDown(new EM_Category() { intStatus = 1, IncludeCategoryIds = pObjIGRClassificationModel.CategoryID.ToString() });
            UI_FillEMRevenueHeadDropDown(new EM_RevenueHead() { intStatus = 1, CategoryID = pObjIGRClassificationModel.CategoryID, IncludeRevenueHeadIds = pObjIGRClassificationModel.RevenueHeadID.ToString() });
            UI_FillMonthDropDown();
        }

        [HttpGet]
        public ActionResult Add()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(EMIGRClassificationViewModel pObjIGRClassificationModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjIGRClassificationModel);
                return View(pObjIGRClassificationModel);
            }
            else
            {
                EM_IGRClassification mObjIGRClassification = new EM_IGRClassification()
                {
                    IGRClassificationID = 0,
                    RevenueHeadID = pObjIGRClassificationModel.RevenueHeadID,
                    TaxMonth = pObjIGRClassificationModel.TaxMonth,
                    Active = true,
                    CreatedBy = SessionManager.UserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLEMIGRClassification().BL_InsertUpdateIGRClassification(mObjIGRClassification);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "EMIGRClassification");
                    }
                    else
                    {
                        UI_FillDropDown(pObjIGRClassificationModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjIGRClassificationModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjIGRClassificationModel);
                    ViewBag.Message = "Error occurred while saving igr classification";
                    return View(pObjIGRClassificationModel);
                }
            }
        }

        [HttpGet]
        public ActionResult Edit(long? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                EM_IGRClassification mObjIGRClassification = new EM_IGRClassification()
                {
                    IGRClassificationID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_EM_GetIGRClassificationList_Result mObjIGRClassificationData = new BLEMIGRClassification().BL_GetIGRClassificationDetails(mObjIGRClassification);

                if (mObjIGRClassificationData != null)
                {
                    EMIGRClassificationViewModel mObjIGRClassificationModelView = new EMIGRClassificationViewModel()
                    {
                        IGRClassificationID = mObjIGRClassificationData.IGRClassificationID.GetValueOrDefault(),
                        CategoryID = mObjIGRClassificationData.CategoryID.GetValueOrDefault(),
                        RevenueHeadID = mObjIGRClassificationData.RevenueHeadID.GetValueOrDefault(),
                        TaxMonth = mObjIGRClassificationData.TaxMonth.GetValueOrDefault(),
                        Active = mObjIGRClassificationData.Active.GetValueOrDefault(),
                    };

                    UI_FillDropDown(mObjIGRClassificationModelView);
                    return View(mObjIGRClassificationModelView);
                }
                else
                {
                    return RedirectToAction("List", "EMIGRClassification");
                }
            }
            else
            {
                return RedirectToAction("List", "EMIGRClassification");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(EMIGRClassificationViewModel pObjIGRClassificationModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjIGRClassificationModel);
                return View(pObjIGRClassificationModel);
            }
            else
            {
                EM_IGRClassification mObjIGRClassification = new EM_IGRClassification()
                {
                    IGRClassificationID = pObjIGRClassificationModel.IGRClassificationID,
                    RevenueHeadID = pObjIGRClassificationModel.RevenueHeadID,
                    TaxMonth = pObjIGRClassificationModel.TaxMonth,
                    Active = pObjIGRClassificationModel.Active,
                    ModifiedBy = SessionManager.UserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLEMIGRClassification().BL_InsertUpdateIGRClassification(mObjIGRClassification);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "EMIGRClassification");
                    }
                    else
                    {
                        UI_FillDropDown(pObjIGRClassificationModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjIGRClassificationModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    UI_FillDropDown(pObjIGRClassificationModel);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    ViewBag.Message = "Error occurred while saving igr classification";
                    return View(pObjIGRClassificationModel);
                }
            }
        }

        [HttpGet]
        public ActionResult Details(long? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                EM_IGRClassification mObjIGRClassification = new EM_IGRClassification()
                {
                    IGRClassificationID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_EM_GetIGRClassificationList_Result mObjIGRClassificationData = new BLEMIGRClassification().BL_GetIGRClassificationDetails(mObjIGRClassification);

                if (mObjIGRClassificationData != null)
                {
                    IList<usp_EM_GetClassificationDataSourceList_Result> lstDataSource = new BLEMIGRClassification().BL_GetClassificationDataSource(mObjIGRClassificationData.IGRClassificationID.GetValueOrDefault());
                    ViewBag.DataSourceList = lstDataSource;

                    return View(mObjIGRClassificationData);
                }
                else
                {
                    return RedirectToAction("List", "EMIGRClassification");
                }
            }
            else
            {
                return RedirectToAction("List", "EMIGRClassification");
            }
        }

        public JsonResult UpdateStatus(EM_IGRClassification pObjIGRClassificationData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjIGRClassificationData.IGRClassificationID != 0)
            {
                FuncResponse mObjFuncResponse = new BLEMIGRClassification().BL_UpdateStatus(pObjIGRClassificationData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["IGRClassificationList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else  
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadBSData()
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            try
            {
                string mStrSearchFilter = Request.Form.GetValues("search[value]")[0];
                string mStrDraw = Request.Form.GetValues("draw")[0];
                string mstrOrderBy = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                string mStrOrderByDir = Request.Form.GetValues("order[0][dir]")[0];
                int mIntStartRowNumber = TrynParse.parseInt(Request.Form.GetValues("start")[0]);
                int mIntPageNumber = TrynParse.parseInt(Request.Form.GetValues("length")[0]);

                long mlngIGRClassification = TrynParse.parseLong(Request.Form["ClassificationID"]);

                // Loading. 
                IList<vw_EM_BankStatement> lstData = new BLEMIGRClassification().BL_GetBankStatementList(mlngIGRClassification);

                // Total record count.   
                int totalRecords = lstData.Count;

                //// Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstData = lstData.Where(p =>
                        p.PaymentDateTime.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.PaymentRefNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.CustomerName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.Category.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.RevenueHead.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.Bank.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.Amount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                //Purpose Sorting Data 
                if (!string.IsNullOrEmpty(mstrOrderBy) && !string.IsNullOrEmpty(mStrOrderByDir))
                {
                    lstData = lstData.OrderBy(mstrOrderBy + " " + mStrOrderByDir).ToList();
                }

                // Filter record count.   
                int recFilter = lstData.Count;

                // Apply pagination.   
                lstData = lstData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstData;

            }
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadPDMainAuthorizedData()
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            try
            {
                string mStrSearchFilter = Request.Form.GetValues("search[value]")[0];
                string mStrDraw = Request.Form.GetValues("draw")[0];
                string mstrOrderBy = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                string mStrOrderByDir = Request.Form.GetValues("order[0][dir]")[0];
                int mIntStartRowNumber = TrynParse.parseInt(Request.Form.GetValues("start")[0]);
                int mIntPageNumber = TrynParse.parseInt(Request.Form.GetValues("length")[0]);

                long mlngIGRClassification = TrynParse.parseLong(Request.Form["ClassificationID"]);

                // Loading. 
                IList<vw_EM_PD_Main_Authorized> lstData = new BLEMIGRClassification().BL_GetPDMainAuthorizedList(mlngIGRClassification);

                // Total record count.   
                int totalRecords = lstData.Count;

                //// Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstData = lstData.Where(p =>
                        p.PaymentDateTime.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.PaymentRefNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.CustomerName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.RIN.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.Bank.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.Amount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                //Purpose Sorting Data 
                if (!string.IsNullOrEmpty(mstrOrderBy) && !string.IsNullOrEmpty(mStrOrderByDir))
                {
                    lstData = lstData.OrderBy(mstrOrderBy + " " + mStrOrderByDir).ToList();
                }

                // Filter record count.   
                int recFilter = lstData.Count;

                // Apply pagination.   
                lstData = lstData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstData;

            }
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadPDMainPendingData()
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            try
            {
                string mStrSearchFilter = Request.Form.GetValues("search[value]")[0];
                string mStrDraw = Request.Form.GetValues("draw")[0];
                string mstrOrderBy = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                string mStrOrderByDir = Request.Form.GetValues("order[0][dir]")[0];
                int mIntStartRowNumber = TrynParse.parseInt(Request.Form.GetValues("start")[0]);
                int mIntPageNumber = TrynParse.parseInt(Request.Form.GetValues("length")[0]);

                long mlngIGRClassification = TrynParse.parseLong(Request.Form["ClassificationID"]);

                // Loading. 
                IList<vw_EM_PD_Main_Pending> lstData = new BLEMIGRClassification().BL_GetPDMainPendingList(mlngIGRClassification);

                // Total record count.   
                int totalRecords = lstData.Count;

                //// Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstData = lstData.Where(p =>
                        p.PaymentDateTime.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.PaymentRefNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.CustomerName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.RIN.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.Bank.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.Amount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                //Purpose Sorting Data 
                if (!string.IsNullOrEmpty(mstrOrderBy) && !string.IsNullOrEmpty(mStrOrderByDir))
                {
                    lstData = lstData.OrderBy(mstrOrderBy + " " + mStrOrderByDir).ToList();
                }

                // Filter record count.   
                int recFilter = lstData.Count;

                // Apply pagination.   
                lstData = lstData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstData;

            }
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadPDMVAAuthorizedData()
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            try
            {
                string mStrSearchFilter = Request.Form.GetValues("search[value]")[0];
                string mStrDraw = Request.Form.GetValues("draw")[0];
                string mstrOrderBy = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                string mStrOrderByDir = Request.Form.GetValues("order[0][dir]")[0];
                int mIntStartRowNumber = TrynParse.parseInt(Request.Form.GetValues("start")[0]);
                int mIntPageNumber = TrynParse.parseInt(Request.Form.GetValues("length")[0]);

                long mlngIGRClassification = TrynParse.parseLong(Request.Form["ClassificationID"]);

                // Loading. 
                IList<vw_EM_PD_MVA_Authorized> lstData = new BLEMIGRClassification().BL_GetPDMVAAuthorizedList(mlngIGRClassification);

                // Total record count.   
                int totalRecords = lstData.Count;

                //// Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstData = lstData.Where(p =>
                        p.PaymentDateTime.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.PaymentRefNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.CustomerName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.RIN.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.Bank.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.Amount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                //Purpose Sorting Data 
                if (!string.IsNullOrEmpty(mstrOrderBy) && !string.IsNullOrEmpty(mStrOrderByDir))
                {
                    lstData = lstData.OrderBy(mstrOrderBy + " " + mStrOrderByDir).ToList();
                }

                // Filter record count.   
                int recFilter = lstData.Count;

                // Apply pagination.   
                lstData = lstData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstData;

            }
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadPDMVAPendingData()
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            try
            {
                string mStrSearchFilter = Request.Form.GetValues("search[value]")[0];
                string mStrDraw = Request.Form.GetValues("draw")[0];
                string mstrOrderBy = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
                string mStrOrderByDir = Request.Form.GetValues("order[0][dir]")[0];
                int mIntStartRowNumber = TrynParse.parseInt(Request.Form.GetValues("start")[0]);
                int mIntPageNumber = TrynParse.parseInt(Request.Form.GetValues("length")[0]);

                long mlngIGRClassification = TrynParse.parseLong(Request.Form["ClassificationID"]);

                // Loading. 
                IList<vw_EM_PD_MVA_Pending> lstData = new BLEMIGRClassification().BL_GetPDMVAPendingList(mlngIGRClassification);

                // Total record count.   
                int totalRecords = lstData.Count;

                //// Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstData = lstData.Where(p =>
                        p.PaymentDateTime.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.PaymentRefNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.CustomerName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.RIN.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.Bank.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.Amount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                //Purpose Sorting Data 
                if (!string.IsNullOrEmpty(mstrOrderBy) && !string.IsNullOrEmpty(mStrOrderByDir))
                {
                    lstData = lstData.OrderBy(mstrOrderBy + " " + mStrOrderByDir).ToList();
                }

                // Filter record count.   
                int recFilter = lstData.Count;

                // Apply pagination.   
                lstData = lstData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstData;

            }
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PDMainAuthorized(long? id)
        {
            if (id.GetValueOrDefault() > 0)
            {
                EM_IGRClassification mObjIGRClassification = new EM_IGRClassification()
                {
                    IGRClassificationID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_EM_GetIGRClassificationList_Result mObjIGRClassificationData = new BLEMIGRClassification().BL_GetIGRClassificationDetails(mObjIGRClassification);

                if (mObjIGRClassificationData != null)
                {
                    ClassificationEntryViewModel mObjEntryModel = new ClassificationEntryViewModel()
                    {
                        ClassificationID = mObjIGRClassificationData.IGRClassificationID.GetValueOrDefault()
                    };

                    ViewBag.ClassificationData = mObjIGRClassificationData;

                    return View(mObjEntryModel);
                }
                else
                {
                    return RedirectToAction("List", "EMIGRClassification");
                }
            }
            else
            {
                return RedirectToAction("List", "EMIGRClassification");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult PDMainAuthorized(ClassificationEntryViewModel pObjEntryModel)
        {
            if (string.IsNullOrWhiteSpace(pObjEntryModel.EntryIds))
            {
                EM_IGRClassification mObjIGRClassification = new EM_IGRClassification()
                {
                    IGRClassificationID = pObjEntryModel.ClassificationID,
                    intStatus = 2
                };

                usp_EM_GetIGRClassificationList_Result mObjIGRClassificationData = new BLEMIGRClassification().BL_GetIGRClassificationDetails(mObjIGRClassification);

                ViewBag.ClassificationData = mObjIGRClassificationData;

                return View(pObjEntryModel);
            }
            else
            {
                BLEMIGRClassification mobjBLClassification = new BLEMIGRClassification();
                string[] strEntryIds = pObjEntryModel.EntryIds.Split(',');

                foreach (var vEntryID in strEntryIds)
                {
                    if (!string.IsNullOrWhiteSpace(vEntryID))
                    {
                        EM_MAP_IGRClassification_Entry mObjEntry = new EM_MAP_IGRClassification_Entry()
                        {
                            DataSourceID = 1,
                            IGRClassificationID = pObjEntryModel.ClassificationID,
                            DSEID = TrynParse.parseInt(vEntryID),
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime(),
                        };

                        FuncResponse mObjResponse = mobjBLClassification.BL_InsertClassificationEntry(mObjEntry);
                    }
                }

                FlashMessage.Info("Entry added Successfully");
                return RedirectToAction("Details", "EMIGRClassification", new { id = pObjEntryModel.ClassificationID });
            }
        }

        [HttpGet]
        public ActionResult PDMainPending(long? id)
        {
            if (id.GetValueOrDefault() > 0)
            {
                EM_IGRClassification mObjIGRClassification = new EM_IGRClassification()
                {
                    IGRClassificationID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_EM_GetIGRClassificationList_Result mObjIGRClassificationData = new BLEMIGRClassification().BL_GetIGRClassificationDetails(mObjIGRClassification);

                if (mObjIGRClassificationData != null)
                {
                    ClassificationEntryViewModel mObjEntryModel = new ClassificationEntryViewModel()
                    {
                        ClassificationID = mObjIGRClassificationData.IGRClassificationID.GetValueOrDefault()
                    };

                    ViewBag.ClassificationData = mObjIGRClassificationData;

                    return View(mObjEntryModel);
                }
                else
                {
                    return RedirectToAction("List", "EMIGRClassification");
                }
            }
            else
            {
                return RedirectToAction("List", "EMIGRClassification");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult PDMainPending(ClassificationEntryViewModel pObjEntryModel)
        {
            if (string.IsNullOrWhiteSpace(pObjEntryModel.EntryIds))
            {
                EM_IGRClassification mObjIGRClassification = new EM_IGRClassification()
                {
                    IGRClassificationID = pObjEntryModel.ClassificationID,
                    intStatus = 2
                };

                usp_EM_GetIGRClassificationList_Result mObjIGRClassificationData = new BLEMIGRClassification().BL_GetIGRClassificationDetails(mObjIGRClassification);

                ViewBag.ClassificationData = mObjIGRClassificationData;

                return View(pObjEntryModel);
            }
            else
            {
                BLEMIGRClassification mobjBLClassification = new BLEMIGRClassification();
                string[] strEntryIds = pObjEntryModel.EntryIds.Split(',');

                foreach (var vEntryID in strEntryIds)
                {
                    if (!string.IsNullOrWhiteSpace(vEntryID))
                    {
                        EM_MAP_IGRClassification_Entry mObjEntry = new EM_MAP_IGRClassification_Entry()
                        {
                            DataSourceID = 2,
                            IGRClassificationID = pObjEntryModel.ClassificationID,
                            DSEID = TrynParse.parseInt(vEntryID),
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime(),
                        };

                        FuncResponse mObjResponse = mobjBLClassification.BL_InsertClassificationEntry(mObjEntry);
                    }
                }

                FlashMessage.Info("Entry added Successfully");
                return RedirectToAction("Details", "EMIGRClassification", new { id = pObjEntryModel.ClassificationID });
            }
        }

        [HttpGet]
        public ActionResult PDMVAAuthorized(long? id)
        {
            if (id.GetValueOrDefault() > 0)
            {
                EM_IGRClassification mObjIGRClassification = new EM_IGRClassification()
                {
                    IGRClassificationID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_EM_GetIGRClassificationList_Result mObjIGRClassificationData = new BLEMIGRClassification().BL_GetIGRClassificationDetails(mObjIGRClassification);

                if (mObjIGRClassificationData != null)
                {
                    ClassificationEntryViewModel mObjEntryModel = new ClassificationEntryViewModel()
                    {
                        ClassificationID = mObjIGRClassificationData.IGRClassificationID.GetValueOrDefault()
                    };

                    ViewBag.ClassificationData = mObjIGRClassificationData;

                    return View(mObjEntryModel);
                }
                else
                {
                    return RedirectToAction("List", "EMIGRClassification");
                }
            }
            else
            {
                return RedirectToAction("List", "EMIGRClassification");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult PDMVAAuthorized(ClassificationEntryViewModel pObjEntryModel)
        {
            if (string.IsNullOrWhiteSpace(pObjEntryModel.EntryIds))
            {
                EM_IGRClassification mObjIGRClassification = new EM_IGRClassification()
                {
                    IGRClassificationID = pObjEntryModel.ClassificationID,
                    intStatus = 2
                };

                usp_EM_GetIGRClassificationList_Result mObjIGRClassificationData = new BLEMIGRClassification().BL_GetIGRClassificationDetails(mObjIGRClassification);

                ViewBag.ClassificationData = mObjIGRClassificationData;

                return View(pObjEntryModel);
            }
            else
            {
                BLEMIGRClassification mobjBLClassification = new BLEMIGRClassification();
                string[] strEntryIds = pObjEntryModel.EntryIds.Split(',');

                foreach (var vEntryID in strEntryIds)
                {
                    if (!string.IsNullOrWhiteSpace(vEntryID))
                    {
                        EM_MAP_IGRClassification_Entry mObjEntry = new EM_MAP_IGRClassification_Entry()
                        {
                            DataSourceID = 3,
                            IGRClassificationID = pObjEntryModel.ClassificationID,
                            DSEID = TrynParse.parseInt(vEntryID),
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime(),
                        };

                        FuncResponse mObjResponse = mobjBLClassification.BL_InsertClassificationEntry(mObjEntry);
                    }
                }

                FlashMessage.Info("Entry added Successfully");
                return RedirectToAction("Details", "EMIGRClassification", new { id = pObjEntryModel.ClassificationID });
            }
        }

        [HttpGet]
        public ActionResult PDMVAPending(long? id)
        {
            if (id.GetValueOrDefault() > 0)
            {
                EM_IGRClassification mObjIGRClassification = new EM_IGRClassification()
                {
                    IGRClassificationID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_EM_GetIGRClassificationList_Result mObjIGRClassificationData = new BLEMIGRClassification().BL_GetIGRClassificationDetails(mObjIGRClassification);

                if (mObjIGRClassificationData != null)
                {
                    ClassificationEntryViewModel mObjEntryModel = new ClassificationEntryViewModel()
                    {
                        ClassificationID = mObjIGRClassificationData.IGRClassificationID.GetValueOrDefault()
                    };

                    ViewBag.ClassificationData = mObjIGRClassificationData;

                    return View(mObjEntryModel);
                }
                else
                {
                    return RedirectToAction("List", "EMIGRClassification");
                }
            }
            else
            {
                return RedirectToAction("List", "EMIGRClassification");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult PDMVAPending(ClassificationEntryViewModel pObjEntryModel)
        {
            if (string.IsNullOrWhiteSpace(pObjEntryModel.EntryIds))
            {
                EM_IGRClassification mObjIGRClassification = new EM_IGRClassification()
                {
                    IGRClassificationID = pObjEntryModel.ClassificationID,
                    intStatus = 2
                };

                usp_EM_GetIGRClassificationList_Result mObjIGRClassificationData = new BLEMIGRClassification().BL_GetIGRClassificationDetails(mObjIGRClassification);

                ViewBag.ClassificationData = mObjIGRClassificationData;

                return View(pObjEntryModel);
            }
            else
            {
                BLEMIGRClassification mobjBLClassification = new BLEMIGRClassification();
                string[] strEntryIds = pObjEntryModel.EntryIds.Split(',');

                foreach (var vEntryID in strEntryIds)
                {
                    if (!string.IsNullOrWhiteSpace(vEntryID))
                    {
                        EM_MAP_IGRClassification_Entry mObjEntry = new EM_MAP_IGRClassification_Entry()
                        {
                            DataSourceID = 4,
                            IGRClassificationID = pObjEntryModel.ClassificationID,
                            DSEID = TrynParse.parseInt(vEntryID),
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime(),
                        };

                        FuncResponse mObjResponse = mobjBLClassification.BL_InsertClassificationEntry(mObjEntry);
                    }
                }

                FlashMessage.Info("Entry added Successfully");
                return RedirectToAction("Details", "EMIGRClassification", new { id = pObjEntryModel.ClassificationID });
            }
        }

        [HttpGet]
        public ActionResult BankStatement(long? id)
        {
            if (id.GetValueOrDefault() > 0)
            {
                EM_IGRClassification mObjIGRClassification = new EM_IGRClassification()
                {
                    IGRClassificationID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_EM_GetIGRClassificationList_Result mObjIGRClassificationData = new BLEMIGRClassification().BL_GetIGRClassificationDetails(mObjIGRClassification);

                if (mObjIGRClassificationData != null)
                {
                    ClassificationEntryViewModel mObjEntryModel = new ClassificationEntryViewModel()
                    {
                        ClassificationID = mObjIGRClassificationData.IGRClassificationID.GetValueOrDefault()
                    };

                    ViewBag.ClassificationData = mObjIGRClassificationData;

                    return View(mObjEntryModel);
                }
                else
                {
                    return RedirectToAction("List", "EMIGRClassification");
                }
            }
            else
            {
                return RedirectToAction("List", "EMIGRClassification");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult BankStatement(ClassificationEntryViewModel pObjEntryModel)
        {
            if (string.IsNullOrWhiteSpace(pObjEntryModel.EntryIds))
            {
                EM_IGRClassification mObjIGRClassification = new EM_IGRClassification()
                {
                    IGRClassificationID = pObjEntryModel.ClassificationID,
                    intStatus = 2
                };

                usp_EM_GetIGRClassificationList_Result mObjIGRClassificationData = new BLEMIGRClassification().BL_GetIGRClassificationDetails(mObjIGRClassification);

                ViewBag.ClassificationData = mObjIGRClassificationData;

                return View(pObjEntryModel);
            }
            else
            {
                BLEMIGRClassification mobjBLClassification = new BLEMIGRClassification();
                string[] strEntryIds = pObjEntryModel.EntryIds.Split(',');

                foreach (var vEntryID in strEntryIds)
                {
                    if (!string.IsNullOrWhiteSpace(vEntryID))
                    {
                        EM_MAP_IGRClassification_Entry mObjEntry = new EM_MAP_IGRClassification_Entry()
                        {
                            DataSourceID = 5,
                            IGRClassificationID = pObjEntryModel.ClassificationID,
                            DSEID = TrynParse.parseInt(vEntryID),
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime(),
                        };

                        FuncResponse mObjResponse = mobjBLClassification.BL_InsertClassificationEntry(mObjEntry);
                    }
                }

                FlashMessage.Info("Entry added Successfully");
                return RedirectToAction("Details", "EMIGRClassification", new { id = pObjEntryModel.ClassificationID });
            }
        }

        public JsonResult GetEntryDetails(int dsid, long entryid)
        {
            if(dsid == 1)
            {
                EM_PD_Main_Authorized mObjDetails = new BLEMDataImport().BL_GetPDMainAuthorizedDetails(entryid);
                return Json(mObjDetails, JsonRequestBehavior.AllowGet);
            }
            else if(dsid == 2)
            {
                EM_PD_Main_Pending mObjDetails = new BLEMDataImport().BL_GetPDMainPendingDetails(entryid);
                return Json(mObjDetails, JsonRequestBehavior.AllowGet);
            }
            else if (dsid == 3)
            {
                EM_PD_MVA_Authorized mObjDetails = new BLEMDataImport().BL_GetPDMVAAuthorizedDetails(entryid);
                return Json(mObjDetails, JsonRequestBehavior.AllowGet);
            }
            else if (dsid == 4)
            {
                EM_PD_MVA_Pending mObjDetails = new BLEMDataImport().BL_GetPDMVAPendingDetails(entryid);
                return Json(mObjDetails, JsonRequestBehavior.AllowGet);
            }
            else if (dsid == 5)
            {
                EM_BankStatement mObjDetails = new BLEMDataImport().BL_GetBankStatementDetails(entryid);
                return Json(mObjDetails, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(null, JsonRequestBehavior.AllowGet);
            }
        }

    }
}