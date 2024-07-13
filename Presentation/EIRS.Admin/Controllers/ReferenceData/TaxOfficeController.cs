using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Vereyon.Web;
using System.Linq;
using Elmah;

namespace EIRS.Admin.Controllers
{
    public class TaxOfficeController : BaseController
    {
        public ActionResult List()
        {
            Tax_Offices mObjTaxOffice = new Tax_Offices()
            {
                intStatus = 2
            };

            IList<usp_GetTaxOfficeList_Result> lstTaxOffice = new BLTaxOffice().BL_GetTaxOfficeList(mObjTaxOffice);
            return View(lstTaxOffice);
        }

        private void UI_FillDropDown(int AddressTypeID = 0, int TaxOfficeID = 0)
        {
            Building mObjBuilding = new Building()
            {
                intStatus = 1
            };

            //IList<usp_GetBuildingList_Result> lstBuilding = new BLBuilding().BL_GetBuildingList(mObjBuilding);
            //ViewBag.BuildingList = lstBuilding;

            ViewBag.TaxOfficeAddressList = SessionManager.lstTaxOfficeAddress;

            UI_FillApproverDropDown(new MST_Users() { intStatus = 1, UserTypeID = (int)EnumList.UserType.Staff, TaxOfficeID = TaxOfficeID });

            UI_FillAddressTypeDropDown(new Address_Types() { intStatus = 1, IncludeAddressTypeIds = AddressTypeID.ToString() });
        }

        public ActionResult Add()
        {
            SessionManager.lstTaxOfficeAddress = new List<TaxOfficeAddress>();
            UI_FillDropDown();
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(TaxOfficeViewModel pObjTaxOfficeModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown();
                return View(pObjTaxOfficeModel);
            }
            else
            {
                TaxOfficeAddress mObjTaxOfficeAddress = SessionManager.lstTaxOfficeAddress != null ? SessionManager.lstTaxOfficeAddress.Where(t => t.intTrack != EnumList.Track.DELETE).FirstOrDefault() : new TaxOfficeAddress();

                Tax_Offices mObjTaxOffice = new Tax_Offices()
                {
                    TaxOfficeID = 0,
                    TaxOfficeName = pObjTaxOfficeModel.TaxOfficeName.Trim(),
                    Approver1 = pObjTaxOfficeModel.Approver1,
                    Approver2 = pObjTaxOfficeModel.Approver2,
                    Approver3 = pObjTaxOfficeModel.Approver3,
                    AddressTypeID = mObjTaxOfficeAddress != null ? mObjTaxOfficeAddress.AddressTypeID : 0,
                    BuildingID = mObjTaxOfficeAddress != null ? mObjTaxOfficeAddress.BuildingID : 0,
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLTaxOffice().BL_InsertUpdateTaxOffice(mObjTaxOffice);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "TaxOffice");
                    }
                    else
                    {
                        UI_FillDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjTaxOfficeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown();
                    ViewBag.Message = "Error occurred while saving tax office";
                    return View(pObjTaxOfficeModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Tax_Offices mObjTaxOffice = new Tax_Offices()
                {
                    TaxOfficeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetTaxOfficeList_Result mObjTaxOfficeData = new BLTaxOffice().BL_GetTaxOfficeDetails(mObjTaxOffice);

                if (mObjTaxOfficeData != null)
                {
                    TaxOfficeViewModel mObjTaxOfficeModelView = new TaxOfficeViewModel()
                    {
                        TaxOfficeID = mObjTaxOfficeData.TaxOfficeID.GetValueOrDefault(),
                        TaxOfficeName = mObjTaxOfficeData.TaxOfficeName,
                        Approver1 = mObjTaxOfficeData.Approver1.GetValueOrDefault(),
                        Approver2 = mObjTaxOfficeData.Approver2.GetValueOrDefault(),
                        Approver3 = mObjTaxOfficeData.Approver3.GetValueOrDefault(),
                        Active = mObjTaxOfficeData.Active.GetValueOrDefault(),
                    };

                    SessionManager.lstTaxOfficeAddress = new List<TaxOfficeAddress>();

                    if (mObjTaxOfficeData.AddressTypeID.GetValueOrDefault() > 0 && mObjTaxOfficeData.BuildingID.GetValueOrDefault() > 0)
                    {
                        TaxOfficeAddress mObjTaxOfficeAddress = new TaxOfficeAddress()
                        {
                            RowID = 1,
                            AddressTypeID = mObjTaxOfficeData.AddressTypeID.GetValueOrDefault(),
                            AddressTypeName = mObjTaxOfficeData.AddressTypeName,
                            BuildingID = mObjTaxOfficeData.BuildingID.GetValueOrDefault(),
                            BuildingName = mObjTaxOfficeData.BuildingName,
                            BuildingRIN = mObjTaxOfficeData.BuildingRIN,
                            intTrack = EnumList.Track.EXISTING
                        };

                        IList<TaxOfficeAddress> lstTaxOfficeAddress = new List<TaxOfficeAddress>();
                        lstTaxOfficeAddress.Add(mObjTaxOfficeAddress);

                        SessionManager.lstTaxOfficeAddress = lstTaxOfficeAddress;
                    }

                    UI_FillDropDown(mObjTaxOfficeData.AddressTypeID.GetValueOrDefault(), mObjTaxOfficeData.TaxOfficeID.GetValueOrDefault());
                    return View(mObjTaxOfficeModelView);
                }
                else
                {
                    return RedirectToAction("List", "TaxOffice");
                }
            }
            else
            {
                return RedirectToAction("List", "TaxOffice");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(TaxOfficeViewModel pObjTaxOfficeModel)
        {
            TaxOfficeAddress mObjTaxOfficeAddress = SessionManager.lstTaxOfficeAddress != null ? SessionManager.lstTaxOfficeAddress.Where(t => t.intTrack != EnumList.Track.DELETE).FirstOrDefault() : new TaxOfficeAddress();

            if (!ModelState.IsValid)
            {
                UI_FillDropDown(mObjTaxOfficeAddress != null ? mObjTaxOfficeAddress.AddressTypeID : 0);
                return View(pObjTaxOfficeModel);
            }
            else
            {

                Tax_Offices mObjTaxOffice = new Tax_Offices()
                {
                    TaxOfficeID = pObjTaxOfficeModel.TaxOfficeID,
                    TaxOfficeName = pObjTaxOfficeModel.TaxOfficeName.Trim(),
                    Approver1 = pObjTaxOfficeModel.Approver1,
                    Approver2 = pObjTaxOfficeModel.Approver2,
                    Approver3 = pObjTaxOfficeModel.Approver3,
                    AddressTypeID = mObjTaxOfficeAddress != null ? mObjTaxOfficeAddress.AddressTypeID : 0,
                    BuildingID = mObjTaxOfficeAddress != null ? mObjTaxOfficeAddress.BuildingID : 0,
                    Active = pObjTaxOfficeModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = new BLTaxOffice().BL_InsertUpdateTaxOffice(mObjTaxOffice);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "TaxOffice");
                    }
                    else
                    {
                        UI_FillDropDown(mObjTaxOfficeAddress != null ? mObjTaxOfficeAddress.AddressTypeID : 0);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjTaxOfficeModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(mObjTaxOfficeAddress != null ? mObjTaxOfficeAddress.AddressTypeID : 0);
                    ViewBag.Message = "Error occurred while saving tax office";
                    return View(pObjTaxOfficeModel);
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Tax_Offices mObjTaxOffice = new Tax_Offices()
                {
                    TaxOfficeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetTaxOfficeList_Result mObjTaxOfficeData = new BLTaxOffice().BL_GetTaxOfficeDetails(mObjTaxOffice);

                if (mObjTaxOfficeData != null)
                {
                    TaxOfficeViewModel mObjTaxOfficeModelView = new TaxOfficeViewModel()
                    {
                        TaxOfficeID = mObjTaxOfficeData.TaxOfficeID.GetValueOrDefault(),
                        TaxOfficeName = mObjTaxOfficeData.TaxOfficeName,
                        ActiveText = mObjTaxOfficeData.ActiveText
                    };

                    SessionManager.lstTaxOfficeAddress = new List<TaxOfficeAddress>();

                    if (mObjTaxOfficeData.AddressTypeID.GetValueOrDefault() > 0 && mObjTaxOfficeData.BuildingID.GetValueOrDefault() > 0)
                    {
                        TaxOfficeAddress mObjTaxOfficeAddress = new TaxOfficeAddress()
                        {
                            RowID = 1,
                            AddressTypeID = mObjTaxOfficeData.AddressTypeID.GetValueOrDefault(),
                            AddressTypeName = mObjTaxOfficeData.AddressTypeName,
                            BuildingID = mObjTaxOfficeData.BuildingID.GetValueOrDefault(),
                            BuildingName = mObjTaxOfficeData.BuildingName,
                            BuildingRIN = mObjTaxOfficeData.BuildingRIN,
                            intTrack = EnumList.Track.EXISTING
                        };

                        IList<TaxOfficeAddress> lstTaxOfficeAddress = new List<TaxOfficeAddress>();
                        lstTaxOfficeAddress.Add(mObjTaxOfficeAddress);

                        SessionManager.lstTaxOfficeAddress = lstTaxOfficeAddress;
                    }

                    ViewBag.TaxOfficeAddressList = SessionManager.lstTaxOfficeAddress;

                    return View(mObjTaxOfficeModelView);
                }
                else
                {
                    return RedirectToAction("List", "TaxOffice");
                }
            }
            else
            {
                return RedirectToAction("List", "TaxOffice");
            }
        }


        public ActionResult AddressDetails(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Tax_Offices mObjTaxOffice = new Tax_Offices()
                {
                    TaxOfficeID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetTaxOfficeList_Result mObjTaxOfficeData = new BLTaxOffice().BL_GetTaxOfficeDetails(mObjTaxOffice);

                if (mObjTaxOfficeData != null)
                {
                    SessionManager.lstTaxOfficeAddress = new List<TaxOfficeAddress>();

                    if (mObjTaxOfficeData.AddressTypeID.GetValueOrDefault() > 0 && mObjTaxOfficeData.BuildingID.GetValueOrDefault() > 0)
                    {
                        TaxOfficeAddress mObjTaxOfficeAddress = new TaxOfficeAddress()
                        {
                            RowID = 1,
                            AddressTypeID = mObjTaxOfficeData.AddressTypeID.GetValueOrDefault(),
                            AddressTypeName = mObjTaxOfficeData.AddressTypeName,
                            BuildingID = mObjTaxOfficeData.BuildingID.GetValueOrDefault(),
                            BuildingName = mObjTaxOfficeData.BuildingName,
                            BuildingRIN = mObjTaxOfficeData.BuildingRIN,
                            intTrack = EnumList.Track.EXISTING
                        };

                        IList<TaxOfficeAddress> lstTaxOfficeAddress = new List<TaxOfficeAddress>();
                        lstTaxOfficeAddress.Add(mObjTaxOfficeAddress);

                        SessionManager.lstTaxOfficeAddress = lstTaxOfficeAddress;
                    }

                    ViewBag.TaxOfficeAddressList = SessionManager.lstTaxOfficeAddress;

                    return View();
                }
                else
                {
                    return RedirectToAction("List", "TaxOffice");
                }
            }
            else
            {
                return RedirectToAction("List", "TaxOffice");
            }
        }

        public JsonResult UpdateStatus(Tax_Offices pObjTaxOfficeData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjTaxOfficeData.TaxOfficeID != 0)
            {
                FuncResponse mObjFuncResponse = new BLTaxOffice().BL_UpdateStatus(pObjTaxOfficeData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["TaxOfficeList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }


        public JsonResult AddTaxOfficeAddress(TaxOfficeAddress pObjTaxOfficeAddress)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();
            IList<TaxOfficeAddress> lstTaxOfficeAddress = SessionManager.lstTaxOfficeAddress != null ? SessionManager.lstTaxOfficeAddress : new List<TaxOfficeAddress>();
            TaxOfficeAddress mObjTaxOfficeAddress = lstTaxOfficeAddress.Where(t => t.intTrack != EnumList.Track.DELETE).Count() > 0 ? lstTaxOfficeAddress.Where(t => t.intTrack != EnumList.Track.DELETE).FirstOrDefault() : new TaxOfficeAddress();

            mObjTaxOfficeAddress.RowID = lstTaxOfficeAddress.Count + 1;
            mObjTaxOfficeAddress.AddressTypeID = pObjTaxOfficeAddress.AddressTypeID;
            mObjTaxOfficeAddress.AddressTypeName = pObjTaxOfficeAddress.AddressTypeName;
            mObjTaxOfficeAddress.BuildingID = pObjTaxOfficeAddress.BuildingID;
            mObjTaxOfficeAddress.BuildingName = pObjTaxOfficeAddress.BuildingName;
            mObjTaxOfficeAddress.BuildingRIN = pObjTaxOfficeAddress.BuildingRIN;
            mObjTaxOfficeAddress.intTrack = EnumList.Track.UPDATE;

            if (lstTaxOfficeAddress.Where(t => t.intTrack != EnumList.Track.DELETE).Count() == 0)
                lstTaxOfficeAddress.Add(mObjTaxOfficeAddress);

            SessionManager.lstTaxOfficeAddress = lstTaxOfficeAddress;

            dcResponse["success"] = true;
            dcResponse["TaxOfficeAddressList"] = CommUtil.RenderPartialToString("_BindTaxOfficeAddress", lstTaxOfficeAddress.Where(t => t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
            dcResponse["TaxOfficeAddressCount"] = lstTaxOfficeAddress.Where(t => t.intTrack != EnumList.Track.DELETE).Count();
            dcResponse["Message"] = "Tax Office Address Added";
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveTaxOfficeAddress(int RowID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();
            IList<TaxOfficeAddress> lstTaxOfficeAddress = SessionManager.lstTaxOfficeAddress != null ? SessionManager.lstTaxOfficeAddress : new List<TaxOfficeAddress>();
            TaxOfficeAddress mObjTaxOfficeAddress = lstTaxOfficeAddress.Where(t => t.RowID == RowID).FirstOrDefault();

            if (mObjTaxOfficeAddress != null)
            {
                mObjTaxOfficeAddress.intTrack = EnumList.Track.DELETE;
                SessionManager.lstTaxOfficeAddress = lstTaxOfficeAddress;

                dcResponse["success"] = true;
                dcResponse["TaxOfficeAddressList"] = CommUtil.RenderPartialToString("_BindTaxOfficeAddress", lstTaxOfficeAddress.Where(t => t.intTrack != EnumList.Track.DELETE).ToList(), this.ControllerContext);
                dcResponse["TaxOfficeAddressCount"] = lstTaxOfficeAddress.Where(t => t.intTrack != EnumList.Track.DELETE).Count();
                dcResponse["Message"] = "Tax Office Address Removed";

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }


            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBuildingDetails(int BuildingID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            usp_GetBuildingList_Result mObjBuildingDetails = new BLBuilding().BL_GetBuildingDetails(new Building() { intStatus = 2, BuildingID = BuildingID });

            if (mObjBuildingDetails != null)
            {
                dcResponse["success"] = true;
                dcResponse["BuildingDetails"] = mObjBuildingDetails;
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