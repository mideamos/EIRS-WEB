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



            return Json(dcResponse, System.Web.Mvc.JsonRequestBehavior.AllowGet);
        }
        public IList<DropDownListResult> REP_GetLGAZoneDropDownList(string zoneCode)
        {
            using (_db = new EIRSContext())
            {
                var vResult = (from nat in _db.ZoneLga
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
        public IList<DropDownListResult> REP_GetNationalityDropDownList(string zonecode)
        {
            using (_db = new EIRSContext())
            {
                var vResult = (from nat in _db.TaxOffice
                               where nat.ZoneCode == zonecode
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
    //    public class ZoneController : BaseController
    //    {
    //        EIRSContext _db;

    //        // GET: Zone
    //        public ActionResult Index()
    //        {
    //            using (_db = new EIRSContext())
    //            {
    //                IList<Zone> lstSize = _db.Zone.ToList();
    //                foreach (var item in lstSize)
    //                    if (item.Active == true) item.ActiveText = "Active";
    //                return View(lstSize);
    //            }
    //        }


    //        public ActionResult Details(int? id, string name)
    //        {
    //            if (id.GetValueOrDefault() > 0)
    //            {
    //                using (_db = new EIRSContext())
    //                {

    //                    Zone mObjSizeData = _db.Zone.FirstOrDefault(o => o.ZoneId == id || o.ZoneName == name);

    //                    if (mObjSizeData != null)
    //                    {
    //                        mObjSizeData.LgaNames = _db.ZoneLga.Where(o => o.ZoneCode == mObjSizeData.ZoneCode).ToList();
    //                        mObjSizeData.TaxOffices = _db.TaxOffice.Where(o => o.ZoneCode == mObjSizeData.ZoneCode).ToList();
    //                        return View(mObjSizeData);
    //                    }
    //                    else
    //                    {
    //                        return RedirectToAction("List", "Size");
    //                    }
    //                }
    //            }
    //            else
    //            {
    //                return RedirectToAction("List", "Size");
    //            }
    //        }

    //        public JsonResult UpdateStatus(Zone pObjSizeData)
    //        {
    //            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

    //            if (pObjSizeData.ZoneId != 0)
    //            {
    //                using (_db = new EIRSContext())
    //                {
    //                    Zone mObjSizeData = _db.Zone.FirstOrDefault(o => o.ZoneId == pObjSizeData.ZoneId);
    //                    if (mObjSizeData.Active == false)
    //                        mObjSizeData.Active = true;
    //                    else
    //                        mObjSizeData.Active = false;

    //                    _db.SaveChanges();
    //                    FuncResponse mObjFuncResponse = new FuncResponse();
    //                    mObjFuncResponse.AdditionalData = _db.Zone.ToList();
    //                    dcResponse["success"] = mObjFuncResponse.Success;
    //                    dcResponse["Message"] = mObjFuncResponse.Message;

    //                    //dcResponse["SizeList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
    //                    //return RedirectToAction("Index", "Zone");
    //                }

    //            }
    //            else
    //            {
    //                dcResponse["success"] = false;
    //                dcResponse["Message"] = "Invalid Action";
    //            }



    //            return Json(dcResponse, JsonRequestBehavior.AllowGet);
    //        }
    //        public IList<DropDownListResult> REP_GetLGAZoneDropDownList(string zoneCode)
    //        {
    //            using (_db = new EIRSContext())
    //            {
    //                var vResult = (from nat in _db.ZoneLga
    //                               where nat.ZoneCode == zoneCode
    //                               select new DropDownListResult()
    //                               {
    //                                   id = nat.Id,
    //                                   text = nat.LgaName
    //                               }).ToList();

    //                ViewBag.lga = new SelectList(vResult, "id", "text");
    //                return vResult;
    //            }
    //        }
    //        public IList<DropDownListResult> REP_GetNationalityDropDownList(string zoneCode)
    //        {
    //            using (_db = new EIRSContext())
    //            {
    //                var vResult = (from nat in _db.TaxOffice
    //                               where nat.ZoneCode == zoneCode
    //                               select new DropDownListResult()
    //                               {
    //                                   id = nat.TaxOfficeId,
    //                                   text = nat.TaxOfficeName
    //                               }).ToList();

    //                ViewBag.taxoffice = new SelectList(vResult, "id", "text");
    //                return vResult;
    //            }
    //        }

    //        public ActionResult AddZone()
    //        {
    //            UI_FillLGADropDown();
    //            return View();
    //        }


    //        private void UI_FillDropDown(string zoneCode)
    //        {
    //            REP_GetLGAZoneDropDownList(zoneCode);
    //            REP_GetNationalityDropDownList(zoneCode);

    //        }
    //        public ActionResult Edit(int? id, string name)
    //        {
    //            if (id.GetValueOrDefault() > 0)
    //            {

    //                using (_db = new EIRSContext())
    //                {

    //                    Zone mObjSizeData = _db.Zone.FirstOrDefault(o => o.ZoneId == id || o.ZoneName == name);

    //                    if (mObjSizeData != null)
    //                    {
    //                        mObjSizeData.LgaNames = _db.ZoneLga.Where(o => o.ZoneCode == mObjSizeData.ZoneCode).ToList();
    //                        mObjSizeData.TaxOffices = _db.TaxOffice.Where(o => o.ZoneCode == mObjSizeData.ZoneCode).ToList();

    //                        UI_FillDropDown(mObjSizeData.ZoneCode);
    //                        return View(mObjSizeData);
    //                    }
    //                    else
    //                    {
    //                        return RedirectToAction("Index", "Zone");
    //                    }
    //                }

    //            }
    //            else
    //            {
    //                return RedirectToAction("Index", "Zone");
    //            }
    //        }

    //        [HttpPost()]
    //        [ValidateAntiForgeryToken()]
    //        public ActionResult Edit(Zone pObjAssessmentRuleModel)
    //        {
    //            if (!ModelState.IsValid)
    //            {
    //                UI_FillDropDown(pObjAssessmentRuleModel.ZoneCode);
    //                return View(pObjAssessmentRuleModel);
    //            }
    //            else
    //            {
    //                return RedirectToAction("Index", "Zone");
    //            }
    //        }
    //        //AssessmentRule_Profile mObjAssessmentRuleProfile = SessionManager.lstAssessmentRuleProfile != null ? SessionManager.lstAssessmentRuleProfile.Where(t => t.intTrack != EnumList.Track.DELETE).FirstOrDefault() : new AssessmentRule_Profile();
    //        //IList<AssessmentRule_AssessmentItem> lstAssessmentRuleItems = SessionManager.lstAssessmentRuleItem ?? new List<AssessmentRule_AssessmentItem>();

    //        //int IntAssessmentRuleItemCount = lstAssessmentRuleItems.Where(t => t.intTrack != EnumList.Track.DELETE).Count();
    //        //int IntAssessmentRuleProfileCount = mObjAssessmentRuleProfile != null && mObjAssessmentRuleProfile.ProfileID > 0 ? 1 : 0;

    //        //if (IntAssessmentRuleProfileCount == 0 && IntAssessmentRuleItemCount == 0)
    //        //{
    //        //    UI_FillDropDown(pObjAssessmentRuleModel);
    //        //    ModelState.AddModelError("AssessmentRuleProfile-error", "Please Link Assessment Rule Profile");
    //        //    ModelState.AddModelError("AssessmentRuleItem-error", "Please Link Assessment Rule Item");
    //        //    return View(pObjAssessmentRuleModel);
    //        //}
    //        //else if (IntAssessmentRuleProfileCount == 0)
    //        //{
    //        //    UI_FillDropDown(pObjAssessmentRuleModel);
    //        //    ModelState.AddModelError("AssessmentRuleProfile-error", "Please Link Assessment Rule Profile");
    //        //    return View(pObjAssessmentRuleModel);
    //        //}
    //        //else if (IntAssessmentRuleItemCount == 0)
    //        //{
    //        //    UI_FillDropDown(pObjAssessmentRuleModel);
    //        //    ModelState.AddModelError("AssessmentRuleItem-error", "Please Link Assessment Rule Item");
    //        //    return View(pObjAssessmentRuleModel);
    //        //}
    //        //else
    //        //{
    //        //    using (TransactionScope mObjTransactionScope = new TransactionScope())
    //        //    {
    //        //        BLAssessmentRule mObjBLAssessmentRule = new BLAssessmentRule();

    //        //        Assessment_Rules mObjAssessmentRule = new Assessment_Rules()
    //        //        {
    //        //            AssessmentRuleID = pObjAssessmentRuleModel.AssessmentRuleID,
    //        //            AssessmentRuleName = pObjAssessmentRuleModel.AssessmentRuleName.Trim(),
    //        //            RuleRunID = pObjAssessmentRuleModel.RuleRunID,
    //        //            PaymentFrequencyID = pObjAssessmentRuleModel.PaymentFrequencyID,
    //        //            TaxYear = pObjAssessmentRuleModel.TaxYear,
    //        //            PaymentOptionID = pObjAssessmentRuleModel.PaymentOptionID,
    //        //            ProfileID = mObjAssessmentRuleProfile != null ? mObjAssessmentRuleProfile.ProfileID : 0,
    //        //            AssessmentAmount = lstAssessmentRuleItems != null ? lstAssessmentRuleItems.Where(t => t.intTrack != EnumList.Track.DELETE).Sum(t => t.TaxAmount) : 0,
    //        //            Active = pObjAssessmentRuleModel.Active,
    //        //            ModifiedBy = SessionManager.SystemUserID,
    //        //            ModifiedDate = CommUtil.GetCurrentDateTime()
    //        //        };

    //        //        try
    //        //        {

    //        //            FuncResponse<Assessment_Rules> mObjResponse = mObjBLAssessmentRule.BL_InsertUpdateAssessmentRule(mObjAssessmentRule);

    //        //            if (mObjResponse.Success)
    //        //            {

    //        //                IList<MAP_AssessmentRule_SettlementMethod> lstOldSettlementMethod = mObjBLAssessmentRule.BL_GetSettlementMethod(pObjAssessmentRuleModel.AssessmentRuleID);

    //        //                int[] intRemovedSettlementMethod = new int[] { };
    //        //                int[] intAddedSettlementMethod = new int[] { };

    //        //                if (pObjAssessmentRuleModel.SettlementMethodIds == null)
    //        //                {
    //        //                    intRemovedSettlementMethod = lstOldSettlementMethod.Select(t => t.ARSMID).ToArray();
    //        //                }
    //        //                else
    //        //                {
    //        //                    intRemovedSettlementMethod = lstOldSettlementMethod.Where(t => !pObjAssessmentRuleModel.SettlementMethodIds.Contains(t.SettlementMethodID.GetValueOrDefault())).Select(t => t.ARSMID).ToArray();

    //        //                    if (lstOldSettlementMethod == null || lstOldSettlementMethod.Count() == 0)
    //        //                    {
    //        //                        intAddedSettlementMethod = pObjAssessmentRuleModel.SettlementMethodIds;
    //        //                    }
    //        //                    else
    //        //                    {
    //        //                        int[] intSettlementMethodID = lstOldSettlementMethod.Select(t => t.SettlementMethodID.GetValueOrDefault()).ToArray();
    //        //                        intAddedSettlementMethod = pObjAssessmentRuleModel.SettlementMethodIds.Except(intSettlementMethodID).ToArray();
    //        //                    }
    //        //                }

    //        //                foreach (int intARSMID in intRemovedSettlementMethod)
    //        //                {
    //        //                    MAP_AssessmentRule_SettlementMethod mObjSettlementMethod = new MAP_AssessmentRule_SettlementMethod()
    //        //                    {
    //        //                        ARSMID = intARSMID
    //        //                    };

    //        //                    mObjBLAssessmentRule.BL_RemoveSettlementMethod(mObjSettlementMethod);
    //        //                }

    //        //                foreach (int intSettlementMethodID in intAddedSettlementMethod)
    //        //                {
    //        //                    MAP_AssessmentRule_SettlementMethod mObjSettlementMethod = new MAP_AssessmentRule_SettlementMethod()
    //        //                    {
    //        //                        AssessmentRuleID = pObjAssessmentRuleModel.AssessmentRuleID,
    //        //                        SettlementMethodID = intSettlementMethodID,
    //        //                        CreatedBy = SessionManager.SystemUserID,
    //        //                        CreatedDate = DateTime.Now
    //        //                    };

    //        //                    mObjBLAssessmentRule.BL_InsertSettlementMethod(mObjSettlementMethod);
    //        //                }


    //        //                foreach (var assessmentitem in lstAssessmentRuleItems)
    //        //                {
    //        //                    if (assessmentitem.intTrack == EnumList.Track.INSERT)
    //        //                    {
    //        //                        MAP_AssessmentRule_AssessmentItem mObjAssessmenstItem = new MAP_AssessmentRule_AssessmentItem()
    //        //                        {
    //        //                            AssessmentItemID = assessmentitem.AssessmentItemID,
    //        //                            AssessmentRuleID = mObjResponse.AdditionalData.AssessmentRuleID,
    //        //                            CreatedBy = SessionManager.SystemUserID,
    //        //                            CreatedDate = DateTime.Now
    //        //                        };

    //        //                        mObjBLAssessmentRule.BL_InsertAssessmentItem(mObjAssessmenstItem);
    //        //                    }
    //        //                    else if (assessmentitem.intTrack == EnumList.Track.DELETE)
    //        //                    {
    //        //                        MAP_AssessmentRule_AssessmentItem mObjAssessmenstItem = new MAP_AssessmentRule_AssessmentItem()
    //        //                        {
    //        //                            ARAIID = assessmentitem.TablePKID
    //        //                        };

    //        //                        mObjBLAssessmentRule.BL_RemoveAssessmentItem(mObjAssessmenstItem);
    //        //                    }
    //        //                }


    //        //                mObjTransactionScope.Complete();
    //        //                FlashMessage.Info(mObjResponse.Message);
    //        //                return RedirectToAction("List", "AssessmentRule");
    //        //            }
    //        //            else
    //        //            {
    //        //                UI_FillDropDown(pObjAssessmentRuleModel);
    //        //                Transaction.Current.Rollback();
    //        //                ViewBag.Message = mObjResponse.Message;
    //        //                return View(pObjAssessmentRuleModel);
    //        //            }
    //        //        }
    //        //        catch (Exception ex)
    //        //        {
    //        //            ErrorSignal.FromCurrentContext().Raise(ex);
    //        //            UI_FillDropDown(pObjAssessmentRuleModel);
    //        //            Transaction.Current.Rollback();
    //        //            ViewBag.Message = "Error occurred while updating assessment rule";
    //        //            return View(pObjAssessmentRuleModel);
    //        //        }
    //    }
}