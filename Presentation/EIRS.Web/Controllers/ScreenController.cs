using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Elmah;
using EIRS.Models;
using static EIRS.Web.Controllers.Filters;

namespace EIRS.Web.Controllers
{
    [SessionTimeout]
    public class ScreenController : BaseController
    {

        public ActionResult List()
        {
            IList<SelectListItem> lstStatus = new List<SelectListItem>();
            lstStatus.Add(new SelectListItem() { Value = "2", Text = "All" });
            lstStatus.Add(new SelectListItem() { Value = "1", Text = "Active", Selected = true });
            lstStatus.Add(new SelectListItem() { Value = "0", Text = "In Active" });

            ViewBag.StatusList = lstStatus;

            MST_Screen mObjScreen = new MST_Screen()
            {
                intStatus = 1
            };

            IList<usp_GetScreenList_Result> lstScreen = new BLScreen().BL_GetScreenList(mObjScreen);
            return View(lstScreen);
        }

        [HttpPost]
        public ActionResult List(FormCollection p_ObjFormCollection)
        {
            string strTitleFilter = p_ObjFormCollection.Get("txtFilter");
            int intStatus = TrynParse.parseInt(p_ObjFormCollection.Get("cboStatus"));

            MST_Screen mObjScreen = new MST_Screen()
            {
                intStatus = intStatus,
                ScreenName = strTitleFilter
            };

            IList<usp_GetScreenList_Result> lstScreen = new BLScreen().BL_GetScreenList(mObjScreen);
            return PartialView("SearchData", lstScreen);
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                MST_Screen mObjScreen = new MST_Screen()
                {
                    ScreenID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetScreenList_Result mObjScreenData = new BLScreen().BL_GetScreenDetails(mObjScreen);

                if (mObjScreenData != null)
                {
                    ScreenViewModel mObjScreenModelView = new ScreenViewModel()
                    {
                        ScreenName = mObjScreenData.ScreenName,
                        ScreenUrl = mObjScreenData.ScreenUrl,
                        ActiveText = mObjScreenData.ActiveText,
                    };

                    return View(mObjScreenModelView);
                }
                else
                {
                    return RedirectToAction("List", "Screen");
                }
            }
            else
            {
                return RedirectToAction("List", "Screen");
            }
        }

        public ActionResult UserList(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                MST_Screen mObjScreen = new MST_Screen()
                {
                    ScreenID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetScreenList_Result mObjScreenData = new BLScreen().BL_GetScreenDetails(mObjScreen);

                if (mObjScreenData != null)
                {
                    ScreenViewModel mObjScreenModelView = new ScreenViewModel()
                    {
                        ScreenName = mObjScreenData.ScreenName,
                        ScreenUrl = mObjScreenData.ScreenUrl,
                        ActiveText = mObjScreenData.ActiveText,
                    };

                    //Get User List

                    return View(mObjScreenModelView);
                }
                else
                {
                    return RedirectToAction("List", "Screen");
                }
            }
            else
            {
                return RedirectToAction("List", "Screen");
            }
        }


    }
}