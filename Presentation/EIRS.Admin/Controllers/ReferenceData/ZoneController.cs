using EIRS.Admin.Models;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using EIRS.Repository;
using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;
using Vereyon.Web;
using Zone = EIRS.BOL.Zone;

namespace EIRS.Admin.Controllers.ReferenceData
{
    public class ZoneController : BaseController
    {
        EIRSContext _db;

        // GET: Zone
        //public ActionResult Index()
        //{
        //    using (_db = new EIRSContext())
        //    {
        //        IList<Zone> lstSize = _db.Zones.ToList();
        //        foreach (var item in lstSize)
        //            if (item.Active == true) item.ActiveText = "Active";
        //        return View(lstSize);
        //    }
        //}
        public ActionResult Index()
        {

            IList<spZoneDetailNew_Result> lstAgency = new BLZone().BL_GetZoneList(0);

            return View(lstAgency);
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                IList<spZoneDetailNew_Result> lstAgency = new BLZone().BL_GetZoneList(id.Value);
                spZoneDetailNew_Result zone = lstAgency.FirstOrDefault();
                return View(zone);
            }
            else
            {
                return RedirectToAction("Index", "Zone");
            }
        }

        public JsonResult UpdateStatus(Zone pObjSizeData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjSizeData.ZoneId != 0)
            {
                //using (_db = new EIRSContext())
                //{
                //    Zone mObjSizeData = _db.Zones.FirstOrDefault(o => o.ZoneId == pObjSizeData.ZoneId);
                //    if (mObjSizeData.Active == false)
                //        mObjSizeData.Active = true;
                //    else
                //        mObjSizeData.Active = false;

                //    _db.SaveChanges();
                //    FuncResponse mObjFuncResponse = new FuncResponse();
                //    mObjFuncResponse.AdditionalData = _db.Zones.ToList();
                //    dcResponse["success"] = mObjFuncResponse.Success;
                //    dcResponse["Message"] = mObjFuncResponse.Message;

                //    //dcResponse["SizeList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                //    //return RedirectToAction("Index", "Zone");
                //}

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
        public IList<DropDownListResult> REP_GetLGAZoneDropDownList(string zoneCode)
        {
            using (_db = new EIRSContext())
            {
                var vResult = (from nat in _db.ZoneLgas
                               where nat.ZoneCode == zoneCode
                               select new DropDownListResult()
                               {
                                   id = nat.Id,
                                   text = nat.LgaName
                               }).ToList();

                ViewBag.lga = new SelectList(vResult, "id", "text");
                return vResult;
            }
        }
        public IList<DropDownListResult> REP_GetNationalityDropDownList(int zoneId)
        {
            using (_db = new EIRSContext())
            {
                var vResult = (from nat in _db.TaxOffices
                               where nat.ZoneId == zoneId
                               select new DropDownListResult()
                               {
                                   id = nat.TaxOfficeId,
                                   text = nat.TaxOfficeName
                               }).ToList();

                ViewBag.taxoffice = new SelectList(vResult, "id", "text");
                return vResult;
            }
        }


        private void UI_FillDropDown(string zoneCode)
        {
            REP_GetLGAZoneDropDownList(zoneCode);
        }
        public ActionResult Edit(int? id, string name)
        {
            UI_FillLGADropDown();
            if (id.GetValueOrDefault() > 0)
            {
                IList<spZoneDetailNew_Result> lstAgency = new BLZone().BL_GetZoneList(id.Value);
                spZoneDetailNew_Result zone = lstAgency.FirstOrDefault();
                Zone zone1 = new Zone();
                zone1.ZoneName = zone.ZoneName;
                zone1.LgaId = zone.LgaId.GetValueOrDefault();
                zone1.ZoneCode = zone.ZoneCode;
                zone1.ZoneId = zone.ZoneId.GetValueOrDefault();
                return View(zone1);
            }
            else
            {
                return RedirectToAction("Index", "Zone");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(Zone pObjZoneModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjZoneModel.ZoneCode);
                return View(pObjZoneModel);
            }
            else
            {
                Zone mObjZOne = new Zone()
                {
                    ZoneId = pObjZoneModel.ZoneId,
                    ZoneName = pObjZoneModel.ZoneName.Trim(),
                    LgaId = pObjZoneModel.LgaId,
                    Active = true,
                    ZoneCode = pObjZoneModel.ZoneCode,
                    // ZoneCode = r.ToString(),
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };
                try
                {
                    FuncResponse mObjResponse = new BLZone().BL_InsertUpdateZone(mObjZOne);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("Index", "Zone");
                    }
                    else
                    {
                        UI_FillAgencyTypeDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjZoneModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillLGADropDown();
                    ViewBag.Message = "Error occurred while Updating Zone";
                    return View(pObjZoneModel);
                }

            }
        }

        public ActionResult Add()
        {
            UI_FillLGADropDown();
            return View();
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(ZoneViewModel pObjZoneModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillLGADropDown();
                return View(pObjZoneModel);
            }
            else
            {
                Random generator = new Random();
                long r = generator.Next(0, 1000000);
                Zone mObjZOne = new Zone()
                {
                    ZoneId = 0,
                    ZoneName = pObjZoneModel.ZoneName.Trim(),
                    LgaId = pObjZoneModel.LGAID,
                    Active = true,
                    ZoneCode = r.ToString(),
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };
                try
                {
                    FuncResponse mObjResponse = new BLZone().BL_InsertUpdateZone(mObjZOne);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("Index", "Zone");
                    }
                    else
                    {
                        UI_FillAgencyTypeDropDown();
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjZoneModel);
                    }
                }
                catch (Exception ex)
                {
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillLGADropDown();
                    ViewBag.Message = "Error occurred while saving Zone";
                    return View(pObjZoneModel);
                }
            }
        }
    }
}