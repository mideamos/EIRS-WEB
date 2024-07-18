using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using EIRS.Web.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vereyon.Web;
using static EIRS.Web.Controllers.Filters;

namespace EIRS.Web.Controllers
{
    [SessionTimeout]
    public class NotifyController : BaseController
    {

        public ActionResult List()
        {
            return View();
        }

        public JsonResult GetData()
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            try
            {
                string mStrSearchFilter = Request.Form.GetValues("search[value]")[0];
                string mStrDraw = Request.Form.GetValues("draw")[0];
                string mstrOrderBy = Request.Form.GetValues("order[0][column]")[0];
                string mStrOrderByDir = Request.Form.GetValues("order[0][dir]")[0];
                int mIntStartRowNumber = TrynParse.parseInt(Request.Form.GetValues("start")[0]);
                int mIntPageNumber = TrynParse.parseInt(Request.Form.GetValues("length")[0]);

                // Loading. 
                IList<vw_Notifications> lstNotificationData = new BLNotification().BL_GetNotificationList();

                // Total record count.   
                int totalRecords = lstNotificationData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstNotificationData = lstNotificationData.Where(p => p.NotificationRefNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.NotificationDate.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.NotificationMethodName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.NotificationTypeName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerRIN != null && p.TaxPayerRIN.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstNotificationData = this.SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstNotificationData);

                // Filter record count.   
                int recFilter = lstNotificationData.Count;

                // Apply pagination.   
                lstNotificationData = lstNotificationData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstNotificationData;

            }
            catch (Exception ex)
            {
                Logger.SendErrorToText(ex);
                Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        private IList<vw_Notifications> SortByColumnWithOrder(string order, string orderDir, IList<vw_Notifications> data)
        {
            // Initialization.   
            IList<vw_Notifications> lst = new List<vw_Notifications>();
            try
            {
                // Sorting   
                switch (order)
                {
                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.NotificationRefNo).ToList() : data.OrderBy(p => p.NotificationRefNo).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.NotificationDate).ToList() : data.OrderBy(p => p.NotificationDate).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.NotificationMethodName).ToList() : data.OrderBy(p => p.NotificationMethodName).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.NotificationTypeName).ToList() : data.OrderBy(p => p.NotificationTypeName).ToList();
                        break;
                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TaxPayerRIN).ToList() : data.OrderBy(p => p.TaxPayerRIN).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                // info.   
                Console.Write(ex);
                Logger.SendErrorToText(ex);
            }
            // info.   
            return lst;
        }


        public ActionResult NotificationCapture()
        {
            UI_FillTaxPayerTypeDropDown();
            UI_FillNotificationMethodDropDown();
            UI_FillNotificationTypeDropDown();
            return View();
        }

        [HttpPost]

        public ActionResult NotificationCapture(NotificationViewModel pObjNotifyModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillTaxPayerTypeDropDown();
                UI_FillNotificationMethodDropDown();
                UI_FillNotificationTypeDropDown();
                return View(pObjNotifyModel);
            }
            else
            {
                bool SuccessCheck = false;
                foreach (var item in pObjNotifyModel.NotificationMethodId)
                {
                    Notification mObjNotification = new Notification()
                    {
                        NotificationID = 0,
                        TaxPayerTypeID = pObjNotifyModel.TaxPayerTypeID,
                        TaxPayerID = pObjNotifyModel.TaxPayerID,
                        NotificationMethodID = item,
                        NotificationTypeID = pObjNotifyModel.NotificationTypeID,
                        NotificationDate = CommUtil.GetCurrentDateTime(),
                        NotificationModeID = (int)EnumList.NotificationMode.Manual,
                        CreatedBy = SessionManager.UserID,
                        CreatedDate = CommUtil.GetCurrentDateTime()
                    };

                    try
                    {
                        FuncResponse mObjResponse = new BLNotification().BL_InsertUpdateNotification(mObjNotification);
                        if (mObjResponse.Success)
                        {
                            //FlashMessage.Info(mObjResponse.Message);
                            SuccessCheck = true;

                        }
                        else
                        {
                            SuccessCheck = false;
                            break;
                            //ViewBag.Message = mObjResponse.Message;
                            //UI_FillTaxPayerTypeDropDown();
                            //UI_FillNotificationMethodDropDown();
                            //UI_FillNotificationTypeDropDown();
                            //return View(pObjNotifyModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        SuccessCheck = false;
                    }
                }
                if (SuccessCheck)
                {
                    return RedirectToAction("List", "Notify");
                }
                else
                {
                    ViewBag.Message = "Error occurred while saving notification";
                    UI_FillTaxPayerTypeDropDown();
                    UI_FillNotificationMethodDropDown();
                    UI_FillNotificationTypeDropDown();
                    return View(pObjNotifyModel);

                }
            }
        }
    }
}