using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Transactions;
using System.Web.Mvc;
using Vereyon.Web;
using System.Linq;
using Elmah;

namespace EIRS.Admin.Controllers
{
    public class DirectorateController : BaseController
    {
        public ActionResult List()
        {
            Directorate mObjDirectorate = new Directorate()
            {
                intStatus = 2
            };

            IList<usp_GetDirectorateList_Result> lstDirectorate = new BLDirectorate().BL_GetDirectorateList(mObjDirectorate);
            return View(lstDirectorate);
        }

        public ActionResult Add()
        {
            UI_FillRevenueStreamDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(DirectorateViewModel pObjDirectorateModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillRevenueStreamDropDown();
                return View(pObjDirectorateModel);
            }
            else
            {
                using (TransactionScope mObjTransactionScope = new TransactionScope())
                {
                    BLDirectorate mObjBLDirectorate = new BLDirectorate();

                    Directorate mObjDirectorate = new Directorate()
                    {
                        DirectorateID = 0,
                        DirectorateName = pObjDirectorateModel.DirectorateName.Trim(),
                        Active = true,
                        CreatedBy = SessionManager.SystemUserID,
                        CreatedDate = CommUtil.GetCurrentDateTime()
                    };

                    try
                    {

                        FuncResponse<Directorate> mObjResponse = mObjBLDirectorate.BL_InsertUpdateDirectorate(mObjDirectorate);

                        if (mObjResponse.Success)
                        {
                            if (pObjDirectorateModel.RevenueStreamIds != null && pObjDirectorateModel.RevenueStreamIds.Length > 0)
                            {
                                foreach (int intRevenueStreamID in pObjDirectorateModel.RevenueStreamIds)
                                {
                                    MAP_Directorates_RevenueStream mObjRevenueStream = new MAP_Directorates_RevenueStream()
                                    {
                                        RevenueStreamID = intRevenueStreamID,
                                        DirectorateID = mObjResponse.AdditionalData.DirectorateID,
                                        CreatedBy = SessionManager.SystemUserID,
                                        CreatedDate = DateTime.Now
                                    };

                                    mObjBLDirectorate.BL_InsertRevenueStream(mObjRevenueStream);
                                }
                            }


                            mObjTransactionScope.Complete();
                            FlashMessage.Info(mObjResponse.Message);
                            return RedirectToAction("List", "Directorate");
                        }
                        else
                        {
                            UI_FillRevenueStreamDropDown();
                            Transaction.Current.Rollback();
                            ViewBag.Message = mObjResponse.Message;
                            return View(pObjDirectorateModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        UI_FillRevenueStreamDropDown();
                        Transaction.Current.Rollback();
                        ViewBag.Message = "Error occurred while saving directorate";
                        return View(pObjDirectorateModel);
                    }
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Directorate mObjDirectorate = new Directorate()
                {
                    DirectorateID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetDirectorateList_Result mObjDirectorateData = new BLDirectorate().BL_GetDirectorateDetails(mObjDirectorate);

                if (mObjDirectorateData != null)
                {
                    DirectorateViewModel mObjDirectorateModelView = new DirectorateViewModel()
                    {
                        DirectorateID = mObjDirectorateData.DirectorateID.GetValueOrDefault(),
                        DirectorateName = mObjDirectorateData.DirectorateName,
                        RevenueStreamIds = TrynParse.parseIntArray(mObjDirectorateData.RevenueStreamIds),
                        Active = mObjDirectorateData.Active.GetValueOrDefault(),
                    };

                    UI_FillRevenueStreamDropDown(new Revenue_Stream() { intStatus = 1, IncludeRevenueStreamIds = mObjDirectorateData.RevenueStreamIds });
                    return View(mObjDirectorateModelView);
                }
                else
                {
                    return RedirectToAction("List", "Directorate");
                }
            }
            else
            {
                return RedirectToAction("List", "Directorate");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(DirectorateViewModel pObjDirectorateModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillRevenueStreamDropDown(new Revenue_Stream() { intStatus = 1, IncludeRevenueStreamIds = pObjDirectorateModel.RevenueStreamIds.ToString() });
                return View(pObjDirectorateModel);
            }
            else
            {
                using (TransactionScope mObjTransactionScope = new TransactionScope())
                {
                    BLDirectorate mObjBLDirectorate = new BLDirectorate();

                    Directorate mObjDirectorate = new Directorate()
                    {
                        DirectorateID = pObjDirectorateModel.DirectorateID,
                        DirectorateName = pObjDirectorateModel.DirectorateName.Trim(),
                        Active = pObjDirectorateModel.Active,
                        ModifiedBy = SessionManager.SystemUserID,
                        ModifiedDate = CommUtil.GetCurrentDateTime()
                    };

                    try
                    {

                        FuncResponse<Directorate> mObjResponse = mObjBLDirectorate.BL_InsertUpdateDirectorate(mObjDirectorate);

                        if (mObjResponse.Success)
                        {


                            IList<MAP_Directorates_RevenueStream> lstOldRevenueStream = mObjBLDirectorate.BL_GetRevenueStream(pObjDirectorateModel.DirectorateID);

                            int[] intRemovedRevenueStream = new int[] { };
                            int[] intAddedRevenueStream = new int[] { };

                            if (pObjDirectorateModel.RevenueStreamIds == null)
                            {
                                intRemovedRevenueStream = lstOldRevenueStream.Select(t => t.DRSID).ToArray();
                            }
                            else
                            {
                                intRemovedRevenueStream = lstOldRevenueStream.Where(t => !pObjDirectorateModel.RevenueStreamIds.Contains(t.RevenueStreamID.GetValueOrDefault())).Select(t => t.DRSID).ToArray();

                                if (lstOldRevenueStream == null || lstOldRevenueStream.Count() == 0)
                                {
                                    intAddedRevenueStream = pObjDirectorateModel.RevenueStreamIds;
                                }
                                else
                                {
                                    int[] intRevenueStreamID = lstOldRevenueStream.Select(t => t.RevenueStreamID.GetValueOrDefault()).ToArray();
                                    intAddedRevenueStream = pObjDirectorateModel.RevenueStreamIds.Except(intRevenueStreamID).ToArray();
                                }
                            }

                            foreach (int intDRSID in intRemovedRevenueStream)
                            {
                                MAP_Directorates_RevenueStream mObjRevenueStream = new MAP_Directorates_RevenueStream()
                                {
                                    DRSID = intDRSID
                                };

                                mObjBLDirectorate.BL_RemoveRevenueStream(mObjRevenueStream);
                            }

                            foreach (int intRevenueStreamID in intAddedRevenueStream)
                            {
                                MAP_Directorates_RevenueStream mObjRevenueStream = new MAP_Directorates_RevenueStream()
                                {
                                    DirectorateID = pObjDirectorateModel.DirectorateID,
                                    RevenueStreamID = intRevenueStreamID,
                                    CreatedBy = SessionManager.SystemUserID,
                                    CreatedDate = DateTime.Now
                                };

                                mObjBLDirectorate.BL_InsertRevenueStream(mObjRevenueStream);
                            }

                            mObjTransactionScope.Complete();
                            FlashMessage.Info(mObjResponse.Message);
                            return RedirectToAction("List", "Directorate");
                        }
                        else
                        {
                            UI_FillRevenueStreamDropDown(new Revenue_Stream() { intStatus = 1, IncludeRevenueStreamIds = pObjDirectorateModel.RevenueStreamIds.ToString() });
                            Transaction.Current.Rollback();
                            ViewBag.Message = mObjResponse.Message;
                            return View(pObjDirectorateModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        UI_FillRevenueStreamDropDown(new Revenue_Stream() { intStatus = 1, IncludeRevenueStreamIds = pObjDirectorateModel.RevenueStreamIds.ToString() });
                        Transaction.Current.Rollback();
                        ViewBag.Message = "Error occurred while saving directorate";
                        return View(pObjDirectorateModel);
                    }
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Directorate mObjDirectorate = new Directorate()
                {
                    DirectorateID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetDirectorateList_Result mObjDirectorateData = new BLDirectorate().BL_GetDirectorateDetails(mObjDirectorate);

                if (mObjDirectorateData != null)
                {
                    DirectorateViewModel mObjDirectorateModelView = new DirectorateViewModel()
                    {
                        DirectorateID = mObjDirectorateData.DirectorateID.GetValueOrDefault(),
                        DirectorateName = mObjDirectorateData.DirectorateName,
                        RevenueStreamNames = mObjDirectorateData.RevenueStreamNames,
                        ActiveText = mObjDirectorateData.ActiveText
                    };

                    return View(mObjDirectorateModelView);
                }
                else
                {
                    return RedirectToAction("List", "Directorate");
                }
            }
            else
            {
                return RedirectToAction("List", "Directorate");
            }
        }

        public JsonResult UpdateStatus(Directorate pObjDirectorateData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjDirectorateData.DirectorateID != 0)
            {
                FuncResponse mObjFuncResponse = new BLDirectorate().BL_UpdateStatus(pObjDirectorateData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["DirectorateList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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