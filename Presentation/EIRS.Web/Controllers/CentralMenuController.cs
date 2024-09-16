using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Elmah;
using EIRS.Models;
using Vereyon.Web; 
using EIRS.Web.Utility;

namespace EIRS.Web.Controllers
{

    public class CentralMenuController : BaseController
    {
        public void UI_FillParentMenuDropDown()
        {
            IList<DropDownListResult> lstParentMenu = new BLCentralMenu().BL_GetParentCentralMenuList();
            ViewBag.ParentMenuList = new SelectList(lstParentMenu, "id", "text");
        }

        public ActionResult List()
        {
            IList<SelectListItem> lstStatus = new List<SelectListItem>();
            lstStatus.Add(new SelectListItem() { Value = "2", Text = "All" });
            lstStatus.Add(new SelectListItem() { Value = "1", Text = "Active", Selected = true });
            lstStatus.Add(new SelectListItem() { Value = "0", Text = "In Active" });

            ViewBag.StatusList = lstStatus;

            UI_FillParentMenuDropDown();

            MST_CentralMenu mObjMenu = new MST_CentralMenu()
            {
                intStatus = 1
            };

            IList<usp_GetCentralMenuList_Result> lstMenu = new BLCentralMenu().BL_GetCentralMenuList(mObjMenu).Where(t => t.ParentCentralMenuID != null).ToList();
            return View(lstMenu);
        }

        [HttpPost]
        public ActionResult List(FormCollection p_ObjFormCollection)
        {
            string strTitleFilter = p_ObjFormCollection.Get("txtFilter");
            int intParentMenuID = TrynParse.parseInt(p_ObjFormCollection.Get("cboParentMenu"));
            int intStatus = TrynParse.parseInt(p_ObjFormCollection.Get("cboStatus"));

            MST_CentralMenu mObjMenu = new MST_CentralMenu()
            {
                intStatus = intStatus,
                ParentCentralMenuID = intParentMenuID,
                CentralMenuName = strTitleFilter
            };

            IList<usp_GetCentralMenuList_Result> lstMenu = new BLCentralMenu().BL_GetCentralMenuList(mObjMenu).Where(t => t.ParentCentralMenuID != null).ToList();
            return PartialView("SearchData", lstMenu);
        }

        public ActionResult Add()
        {
            UI_FillParentMenuDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(CentralMenuViewModel pObjMenuModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillParentMenuDropDown();
                return View(pObjMenuModel);
            }
            else
            {
                MST_CentralMenu mObjMenu = new MST_CentralMenu()
                {
                    CentralMenuID = 0,
                    CentralMenuName = pObjMenuModel.CentralMenuName,
                    ParentCentralMenuID = pObjMenuModel.ParentCentralMenuID,
                    SortOrder = pObjMenuModel.SortOrder,
                    Active = true,
                    CreatedBy = SessionManager.UserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLCentralMenu().BL_InsertUpdateMenu(mObjMenu);

                    if (mObjResponse.Success)
                    {
                        Audit_Log mObjAuditLog = new Audit_Log()
                        {
                            LogDate = CommUtil.GetCurrentDateTime(),
                            ASLID = (int)EnumList.ALScreen.Admin_Central_Menu_Add,
                            Comment = $"New Central Menu Added - {pObjMenuModel.CentralMenuName}",
                            IPAddress = CommUtil.GetIPAddress(),
                            StaffID = SessionManager.UserID,
                        };

                        new BLAuditLog().BL_InsertAuditLog(mObjAuditLog);

                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "CentralMenu");
                    }
                    else
                    {
                        UI_FillParentMenuDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjMenuModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillParentMenuDropDown();
                    ViewBag.Message = "Error occurred while saving Menu";
                    return View(pObjMenuModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                MST_CentralMenu mObjMenu = new MST_CentralMenu()
                {
                    CentralMenuID = id.GetValueOrDefault(),
                    intStatus = 2,
                };

                usp_GetCentralMenuList_Result mObjMenuData = new BLCentralMenu().BL_GetCentralMenuDetails(mObjMenu);

                if (mObjMenuData != null)
                {
                    CentralMenuViewModel mObjMenuModelView = new CentralMenuViewModel()
                    {
                        CentralMenuID = mObjMenuData.CentralMenuID.GetValueOrDefault(),
                        CentralMenuName = mObjMenuData.CentralMenuName,
                        ParentCentralMenuID = mObjMenuData.ParentCentralMenuID.GetValueOrDefault(),
                        SortOrder = mObjMenuData.SortOrder.GetValueOrDefault(),
                        Active = mObjMenuData.Active.GetValueOrDefault(),
                    };

                    UI_FillParentMenuDropDown();
                    return View(mObjMenuModelView);
                }
                else
                {
                    return RedirectToAction("List", "Menu");
                }
            }
            else
            {
                return RedirectToAction("List", "Menu");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(CentralMenuViewModel pObjMenuModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillParentMenuDropDown();
                return View(pObjMenuModel);
            }
            else
            {
                MST_CentralMenu mObjMenu = new MST_CentralMenu()
                {
                    CentralMenuID = pObjMenuModel.CentralMenuID,
                    CentralMenuName = pObjMenuModel.CentralMenuName,
                    ParentCentralMenuID = pObjMenuModel.ParentCentralMenuID,
                    SortOrder = pObjMenuModel.SortOrder,
                    Active = pObjMenuModel.Active,
                    ModifiedBy = SessionManager.UserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLCentralMenu().BL_InsertUpdateMenu(mObjMenu);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "CentralMenu");
                    }
                    else
                    {
                        UI_FillParentMenuDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjMenuModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillParentMenuDropDown();
                    ViewBag.Message = "Error occurred while saving Menu";
                    return View(pObjMenuModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                MST_CentralMenu mObjMenu = new MST_CentralMenu()
                {
                    CentralMenuID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetCentralMenuList_Result mObjMenuData = new BLCentralMenu().BL_GetCentralMenuDetails(mObjMenu);

                if (mObjMenuData != null)
                {
                    CentralMenuViewModel mObjMenuModelView = new CentralMenuViewModel()
                    {
                        CentralMenuName = mObjMenuData.CentralMenuName,
                        ParentCentralMenuName = mObjMenuData.ParentCentralMenuName,
                        SortOrder = mObjMenuData.SortOrder.GetValueOrDefault(),
                        ActiveText = mObjMenuData.ActiveText,
                    };

                    return View(mObjMenuModelView);
                }
                else
                {
                    return RedirectToAction("List", "CentralMenu");
                }
            }
            else
            {
                return RedirectToAction("List", "CentralMenu");
            }
        }

        public ActionResult ScreenList(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                MST_CentralMenu mObjMenu = new MST_CentralMenu()
                {
                    CentralMenuID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetCentralMenuList_Result mObjMenuData = new BLCentralMenu().BL_GetCentralMenuDetails(mObjMenu);

                if (mObjMenuData != null)
                {
                    CentralMenuViewModel mObjMenuModelView = new CentralMenuViewModel()
                    {
                        CentralMenuID = mObjMenuData.CentralMenuID.GetValueOrDefault(),
                        CentralMenuName = mObjMenuData.CentralMenuName,
                        ParentCentralMenuName = mObjMenuData.ParentCentralMenuName,
                        SortOrder = mObjMenuData.SortOrder.GetValueOrDefault(),
                        ActiveText = mObjMenuData.ActiveText,
                    };

                    IList<usp_GetScreenMenuList_Result> lstScreens = new BLScreen().BL_GetScreenMenuList(new MST_Screen() { CentralMenuID = id.GetValueOrDefault() });
                    ViewBag.ScreenList = lstScreens;

                    return View(mObjMenuModelView);
                }
                else
                {
                    return RedirectToAction("List", "CentralMenu");
                }
            }
            else
            {
                return RedirectToAction("List", "CentralMenu");
            }
        }

        public ActionResult AddScreen(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                MST_CentralMenu mObjMenu = new MST_CentralMenu()
                {
                    CentralMenuID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetCentralMenuList_Result mObjMenuData = new BLCentralMenu().BL_GetCentralMenuDetails(mObjMenu);

                if (mObjMenuData != null)
                {
                    CentralMenuViewModel mObjMenuModel = new CentralMenuViewModel()
                    {
                        CentralMenuName = mObjMenuData.CentralMenuName,
                        ParentCentralMenuName = mObjMenuData.ParentCentralMenuName,
                        SortOrder = mObjMenuData.SortOrder.GetValueOrDefault(),
                        ActiveText = mObjMenuData.ActiveText,
                    };

                    IList<usp_GetScreenList_Result> lstScreens = new BLScreen().BL_GetScreenList(new MST_Screen() { intStatus = 1 });
                    ViewBag.ScreenList = lstScreens;
                    ViewBag.MenuData = mObjMenuModel;

                    MenuScreenViewModel mObjScreenModel = new MenuScreenViewModel()
                    {
                        MenuID = mObjMenuData.CentralMenuID.GetValueOrDefault()
                    };

                    return View(mObjScreenModel);
                }
                else
                {
                    return RedirectToAction("List", "CentralMenu");
                }
            }
            else
            {
                return RedirectToAction("List", "CentralMenu");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddScreen(MenuScreenViewModel pObjScreenModel)
        {
            if (string.IsNullOrWhiteSpace(pObjScreenModel.ScreenIds))
            {
                MST_CentralMenu mObjMenu = new MST_CentralMenu()
                {
                    CentralMenuID = pObjScreenModel.MenuID,
                    intStatus = 2
                };

                usp_GetCentralMenuList_Result mObjMenuData = new BLCentralMenu().BL_GetCentralMenuDetails(mObjMenu);

                CentralMenuViewModel mObjMenuModel = new CentralMenuViewModel()
                {
                    CentralMenuName = mObjMenuData.CentralMenuName,
                    ParentCentralMenuName = mObjMenuData.ParentCentralMenuName,
                    SortOrder = mObjMenuData.SortOrder.GetValueOrDefault(),
                    ActiveText = mObjMenuData.ActiveText,
                };

                IList<usp_GetScreenList_Result> lstScreens = new BLScreen().BL_GetScreenList(new MST_Screen() { intStatus = 1 });
                ViewBag.ScreenList = lstScreens;
                ViewBag.MenuData = mObjMenuModel;

                return View(pObjScreenModel);
            }
            else
            {
                BLScreen mobjBLScreen = new BLScreen();
                string[] strScreenIds = pObjScreenModel.ScreenIds.Split(',');

                foreach (var vScreenID in strScreenIds)
                {
                    if (!string.IsNullOrWhiteSpace(vScreenID))
                    {
                        MAP_CentralMenu_Screen mObjCentralMenuScreen = new MAP_CentralMenu_Screen()
                        {

                            CentralMenuID = pObjScreenModel.MenuID,
                            ScreenID = TrynParse.parseInt(vScreenID),
                            isMainScreen = TrynParse.parseInt(vScreenID) == pObjScreenModel.MainScreenID ? true : false,
                            Active = true,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime(),
                        };

                        FuncResponse mObjResponse = mobjBLScreen.BL_InsertCentralMenuScreen(mObjCentralMenuScreen);
                    }
                }

                FlashMessage.Info("Screen added Successfully");
                return RedirectToAction("ScreenList", "CentralMenu", new { id = pObjScreenModel.MenuID });
            }
        }


        public JsonResult RemoveScreen(MAP_CentralMenu_Screen pObjMenuScreen)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjMenuScreen.CMSID != 0)
            {
                FuncResponse<IList<usp_GetScreenMenuList_Result>> mObjFuncResponse = new BLScreen().BL_RemoveMenuScreen(pObjMenuScreen);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["ScreenList"] = CommUtil.RenderPartialToString("_ScreenTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
    }
}