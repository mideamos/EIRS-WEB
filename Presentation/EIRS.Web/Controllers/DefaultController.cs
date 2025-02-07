﻿using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using EIRS.Web.Models;
using EIRS.Web.Utility;
using Elmah;
using Grpc.Core;
using Newtonsoft.Json;
using Sharpbrake.Client.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Mvc;
using Twilio.TwiML.Voice;
using static EIRS.Web.Controllers.Filters;

namespace EIRS.Web.Controllers
{
    public class DefaultController : Controller
    {
        EIRSEntities _db = new EIRSEntities();
        // GET: Default
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult VerifyTccRequestStatus()
        {
            ViewBag.sus = 0;
            ViewBag.Rec = "detB";
            return View();
        }
        public ActionResult VerifyTccRequestStatusRes()
        {
            ViewBag.sus = SessionManager.DefualtCher;
            ViewBag.Rec = SessionManager.DefualtRes;
            return View();
        }

        [HttpPost]
        public ActionResult VerifyTccRequestStatus(IndividualViewModelForTCCSTATUS pObjCollection)
        {
            ViewBag.Rec = 0;

            string phoneNumber = pObjCollection.phonenumber;
            int previousYear = DateTime.Now.Year - 1;
            var checker = _db.Individuals.FirstOrDefault(o => o.MobileNumber1 == phoneNumber);

            if (checker == null)
            {
                SessionManager.DefualtCher = 1;

                return RedirectToAction("VerifyTccRequestStatusRes", "Default");
            }

            var query = (from t in _db.TCC_Request
                         join i in _db.Individuals on t.TaxPayerID equals i.IndividualID into ti
                         from i in ti.DefaultIfEmpty()
                         where i != null && i.MobileNumber1 == phoneNumber && t.TaxYear == previousYear
                         select new { t.StatusID }).FirstOrDefault();

            if (query == null)
            {
                SessionManager.DefualtCher = 2;
                return RedirectToAction("VerifyTccRequestStatusRes", "Default");
            }
            else
            {

                SessionManager.DefualtCher = 3;
                NewTCCRequestStage status = (NewTCCRequestStage)Enum.ToObject(typeof(NewTCCRequestStage), query.StatusID);
                var enumDesc = GetEnumDescription(status);
                ViewBag.Rec = enumDesc;
                SessionManager.DefualtRes = enumDesc;
                return RedirectToAction("VerifyTccRequestStatusRes", "Default");
            }
        }

        string GetEnumDescription(NewTCCRequestStage stage)
        {
            switch (stage)
            {
                case NewTCCRequestStage.Validate_Tax_Payer_Information:
                    return "Your request is under information and income validation stage";

                case NewTCCRequestStage.Validate_Tax_Payer_Income:
                    return "Your request is under information and income validation stage";
                case NewTCCRequestStage.Generate_TCC_Detail:
                    return "Your request is under TCC validation stage";
                case NewTCCRequestStage.Prepare_TCC_Draft:
                    return "Your TCC is on approval Stage";
                case NewTCCRequestStage.Generate_eTCC:
                    return "Your request is on approval stage";
                case NewTCCRequestStage.Waiting_For_First_Signature:
                    return "Your TCC is on approval Stage";
                case NewTCCRequestStage.Waiting_For_Second_Signature:
                    return "Your TCC is on approval Stage";
                case NewTCCRequestStage.Waiting_For_Last_Signature:
                    return "Your TCC is approved and ready for collection. Visit closest EIRS office to you.";
                case NewTCCRequestStage.Issue_eTCC:
                    return "Your TCC has been Issued";
                default:
                    throw new ArgumentOutOfRangeException(nameof(stage), stage, null);
            }
        }

        string GetLast10Digits(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                throw new ArgumentException("Phone number cannot be null or empty.", nameof(phoneNumber));
            }

            // Remove any non-numeric characters from the phone number
            string numericPhoneNumber = new string(phoneNumber.Where(char.IsDigit).ToArray());

            if (numericPhoneNumber.Length < 10)
            {
                throw new ArgumentException("Phone number must contain at least 10 digits.", nameof(phoneNumber));
            }

            // Get the last 10 digits
            return numericPhoneNumber.Substring(numericPhoneNumber.Length - 10);
        }


        public ActionResult About()
        {
            MST_Pages mObjPage = new BLPages().BL_GetPageDetails(new MST_Pages() { PageID = (int)EnumList.PageList.About });

            IList<usp_GetMenuList_Result> lstMenuData = new BLMenu().BL_GetMenuList(new MST_Menu() { ParentMenuID = 1, intStatus = 1 });
            ViewBag.MenuList = lstMenuData;

            return View(mObjPage);
        }

        public ActionResult TaxPayer()
        {
            //MST_Pages mObjPage = new BLPages().BL_GetPageDetails(new MST_Pages() { PageID = (int)EnumList.PageList.TaxPayers });

            //IList<usp_GetMenuList_Result> lstMenuData = new BLMenu().BL_GetMenuList(new MST_Menu() { ParentMenuID = 2, intStatus = 1 });
            //ViewBag.MenuList = lstMenuData;

            //return View(mObjPage);
            return View();
        }

        public ActionResult Companies()
        {
            return View();
        }

        public ActionResult Individual()
        {
            return View();
        }

        public ActionResult Government()
        {
            return View();
        }

        public ActionResult Special()
        {
            return View();
        }

        public ActionResult TaxAsset()
        {
            //MST_Pages mObjPage = new BLPages().BL_GetPageDetails(new MST_Pages() { PageID = (int)EnumList.PageList.TaxAssets });

            //IList<usp_GetMenuList_Result> lstMenuData = new BLMenu().BL_GetMenuList(new MST_Menu() { ParentMenuID = 3, intStatus = 1 });
            //ViewBag.MenuList = lstMenuData;

            //return View(mObjPage);
            return View();
        }

        public ActionResult Building()
        {
            return View();
        }

        public ActionResult Business()
        {
            return View();
        }

        public ActionResult Vehicle()
        {
            return View();
        }

        public ActionResult Land()
        {
            return View();
        }

        public ActionResult TaxType()
        {
            MST_Pages mObjPage = new BLPages().BL_GetPageDetails(new MST_Pages() { PageID = (int)EnumList.PageList.TaxTypes });

            IList<usp_GetMenuList_Result> lstMenuData = new BLMenu().BL_GetMenuList(new MST_Menu() { ParentMenuID = 5, intStatus = 1 });
            ViewBag.MenuList = lstMenuData;

            return View(mObjPage);
        }

        public ActionResult Support()
        {
            MST_Pages mObjPage = new BLPages().BL_GetPageDetails(new MST_Pages() { PageID = (int)EnumList.PageList.Support });

            IList<usp_GetMenuList_Result> lstMenuData = new BLMenu().BL_GetMenuList(new MST_Menu() { ParentMenuID = 6, intStatus = 1 });
            ViewBag.MenuList = lstMenuData;

            return View(mObjPage);
        }

        public ActionResult Partnership()
        {
            MST_Pages mObjPage = new BLPages().BL_GetPageDetails(new MST_Pages() { PageID = (int)EnumList.PageList.Partnership });

            IList<usp_GetMenuList_Result> lstMenuData = new BLMenu().BL_GetMenuList(new MST_Menu() { ParentMenuID = 4, intStatus = 1 });
            ViewBag.MenuList = lstMenuData;

            return View(mObjPage);
        }

        public ActionResult Topics(string menu)
        {
            if (!string.IsNullOrWhiteSpace(menu))
            {
                usp_GetMenuList_Result mObjMenuData = new BLMenu().BL_GetMenuDetails(new MST_Menu() { intStatus = 1, MenuURL = Request.RawUrl });

                if (mObjMenuData != null)
                {
                    IList<usp_GetMenuList_Result> lstMenuData = new BLMenu().BL_GetMenuList(new MST_Menu() { ParentMenuID = mObjMenuData.ParentMenuID, intStatus = 1 });
                    ViewBag.MenuList = lstMenuData;
                    return View(mObjMenuData);
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        public ActionResult Awareness()
        {
            IList<usp_GetAwarenessCategoryList_Result> lstAwarenessCategory = new BLAwarenessCategory().BL_GetAwarenessCategoryList(new MST_AwarenessCategory() { intStatus = 1 });
            return View(lstAwarenessCategory);
        }

        public ActionResult FAQ(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                IList<usp_GetFAQList_Result> lstFAQ = new BLFAQ().BL_GetFAQList(new MST_FAQ() { intStatus = 1, AwarenessCategoryIds = id.GetValueOrDefault().ToString() });
                IList<usp_GetAwarenessCategoryList_Result> lstAwarenessCategory = new BLAwarenessCategory().BL_GetAwarenessCategoryList(new MST_AwarenessCategory() { intStatus = 1 });

                ViewBag.SectionData = lstAwarenessCategory.Where(t => t.AwarenessCategoryID == id.GetValueOrDefault()).FirstOrDefault();
                ViewBag.AwarenessCategoryList = lstAwarenessCategory;

                return View(lstFAQ);
            }
            else
            {
                return RedirectToAction("Awareness", "Default");
            }
        }

        public ActionResult SearchBill(FormCollection pObjFormCollection)
        {
            string mStrReferenceNumber = pObjFormCollection.Get("txtReferenceNumber");

            if (!string.IsNullOrWhiteSpace(mStrReferenceNumber))
            {
                return RedirectToAction("BillDisplay", new { refno = mStrReferenceNumber });
            }
            else
            {
                return RedirectToAction("Home");
            }
        }

        public ActionResult BillDisplay(string refno)
        {
            if (!string.IsNullOrWhiteSpace(refno))
            {
                if (refno.StartsWith("SB"))
                {

                    APIResponse mObjResponse = new APIResponse();
                    //do api call
                    try
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(GlobalDefaultValues.APIURL);
                            //HTTP GET
                            var responseTask = client.GetAsync("RevenueData/Assessment/Details/" + refno + "/refno");
                            responseTask.Wait();

                            var result = responseTask.Result;
                            if (result.IsSuccessStatusCode)
                            {
                                //var readTask = result.Content.ReadAsAsync<APIResponse>();
                                //readTask.Wait();

                                //mObjResponse = readTask.Result;
                                var readTask = result.Content.ReadAsStringAsync();
                                // var readTask = result.Content.ReadAsAsync<APIResponse>();
                                readTask.Wait();

                                string res = readTask.Result.ToString();
                                mObjResponse = JsonConvert.DeserializeObject<APIResponse>(res);
                                if (mObjResponse.Success)
                                {
                                    IDictionary<string, object> dcResponse = new Dictionary<string, object>();
                                    dcResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(mObjResponse.Result.ToString());

                                    usp_GetAssessmentList_Result mObjAssessmentDetails = JsonConvert.DeserializeObject<usp_GetAssessmentList_Result>(dcResponse["AssessmentDetails"].ToString());
                                    IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = JsonConvert.DeserializeObject<IList<usp_GetAssessment_AssessmentRuleList_Result>>(dcResponse["AssessmentRuleDetails"].ToString());
                                    IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentItems = JsonConvert.DeserializeObject<IList<usp_GetAssessmentRuleItemList_Result>>(dcResponse["AssessmentRuleItemDetails"].ToString());
                                    IList<DropDownListResult> lstSettlementMethod = JsonConvert.DeserializeObject<IList<DropDownListResult>>(dcResponse["SettlementMethodList"].ToString());
                                    IList<usp_GetAssessmentRuleBasedSettlement_Result> lstAssessmentRuleSettlement = JsonConvert.DeserializeObject<IList<usp_GetAssessmentRuleBasedSettlement_Result>>(dcResponse["SettlementDetails"].ToString());
                                    IList<usp_GetSettlementList_Result> lstSettlement = JsonConvert.DeserializeObject<IList<usp_GetSettlementList_Result>>(dcResponse["Settlements"].ToString());

                                    ViewBag.SettlementMethodList = new SelectList(lstSettlementMethod, "id", "text");
                                    ViewBag.MAPAssessmentRules = lstMAPAssessmentRules;
                                    ViewBag.AssessmentItems = lstAssessmentItems;
                                    ViewBag.AssessmentRuleSettlement = lstAssessmentRuleSettlement;
                                    ViewBag.SettlementList = lstSettlement;

                                    return View("AssessmentBillDisplay", mObjAssessmentDetails);

                                }
                                else
                                {
                                    return RedirectToAction("Home");
                                }
                            }
                            else //web api sent error response 
                            {
                                return RedirectToAction("Home");
                                //log response status here..

                                //students = Enumerable.Empty<StudentViewModel>();

                                //ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        return RedirectToAction("Home");
                    }
                }
                else if (refno.StartsWith("AB"))
                {
                    APIResponse mObjResponse = new APIResponse();
                    //do api call
                    try
                    {
                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(GlobalDefaultValues.APIURL);
                            //HTTP GET
                            var responseTask = client.GetAsync("RevenueData/ServiceBill/Details/" + refno + "/refno");
                            responseTask.Wait();

                            var result = responseTask.Result;
                            if (result.IsSuccessStatusCode)
                            {
                                //var readTask = result.Content.ReadAsAsync<APIResponse>();
                                //readTask.Wait();

                                //mObjResponse = readTask.Result;
                                var readTask = result.Content.ReadAsStringAsync();
                                // var readTask = result.Content.ReadAsAsync<APIResponse>();
                                readTask.Wait();

                                string res = readTask.Result.ToString();
                                mObjResponse = JsonConvert.DeserializeObject<APIResponse>(res);
                                if (mObjResponse.Success)
                                {
                                    IDictionary<string, object> dcResponse = new Dictionary<string, object>();
                                    dcResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(mObjResponse.Result.ToString());

                                    usp_GetServiceBillList_Result mObjServiceBillDetails = JsonConvert.DeserializeObject<usp_GetServiceBillList_Result>(dcResponse["ServiceBillDetails"].ToString());

                                    IList<usp_GetServiceBill_MDAServiceList_Result> lstMAPServiceBillRules = JsonConvert.DeserializeObject<IList<usp_GetServiceBill_MDAServiceList_Result>>(dcResponse["ServiceBillServicesDetails"].ToString());
                                    IList<usp_GetServiceBillItemList_Result> lstServiceBillItems = JsonConvert.DeserializeObject<IList<usp_GetServiceBillItemList_Result>>(dcResponse["ServiceBillItemDetails"].ToString());
                                    IList<DropDownListResult> lstSettlementMethod = JsonConvert.DeserializeObject<IList<DropDownListResult>>(dcResponse["SettlementMethodList"].ToString());
                                    IList<usp_GetMDAServiceBasedSettlement_Result> lstServiceBillRuleSettlement = JsonConvert.DeserializeObject<IList<usp_GetMDAServiceBasedSettlement_Result>>(dcResponse["SettlementDetails"].ToString());
                                    IList<usp_GetSettlementList_Result> lstSettlement = JsonConvert.DeserializeObject<IList<usp_GetSettlementList_Result>>(dcResponse["Settlements"].ToString());

                                    ViewBag.SettlementMethodList = new SelectList(lstSettlementMethod, "id", "text");
                                    ViewBag.MAPServiceBillRules = lstMAPServiceBillRules;
                                    ViewBag.ServiceBillItems = lstServiceBillItems;
                                    ViewBag.ServiceBillRuleSettlement = lstServiceBillRuleSettlement;
                                    ViewBag.SettlementList = lstSettlement;

                                    return View("ServiceBillDisplay", mObjServiceBillDetails);

                                }
                                else
                                {
                                    return RedirectToAction("Home");
                                }
                            }
                            else
                            {
                                return RedirectToAction("Home");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        return RedirectToAction("Home");
                    }
                }
                else
                {
                    return RedirectToAction("Home");
                }
            }
            else
            {
                return RedirectToAction("Home");
            }
        }

        public ActionResult PAYE()
        {
            IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Pay As You Earn", IntSearchType = 1 });
            return View(lstProfileData);
        }

        public ActionResult DirectAssessment()
        {
            IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Direct Assessment", IntSearchType = 1 });
            return View(lstProfileData);
        }

        public ActionResult ConsumptionTax()
        {
            IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Consumption Tax", IntSearchType = 1 });
            return View(lstProfileData);
        }

        public ActionResult BusinessPremises()
        {
            IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Business Premises", IntSearchType = 1 });
            return View(lstProfileData);
        }

        public ActionResult LandUseCharge()
        {
            IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Land Use Charge", IntSearchType = 1 });
            return View(lstProfileData);
        }

        public ActionResult WitholdingTax()
        {
            IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Withholding Tax", IntSearchType = 1 });
            return View(lstProfileData);
        }

        public ActionResult WasteManagement()
        {
            IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Waste Management Fees", IntSearchType = 1 });
            return View(lstProfileData);
        }

        public ActionResult PoolsBetting()
        {
            IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Pools Betting", IntSearchType = 1 });
            return View(lstProfileData);
        }

        public ActionResult VehicleRegistration()
        {
            IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Vehicle Licenses", IntSearchType = 1 });
            return View(lstProfileData);
        }

        public ActionResult CapitalGains()
        {
            IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Capital Gains", IntSearchType = 1 });
            return View(lstProfileData);
        }

        public ActionResult LogProduce()
        {
            IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Log Produce", IntSearchType = 1 });
            return View(lstProfileData);
        }

        public ActionResult PresumptiveTax()
        {
            IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Presumptive Tax", IntSearchType = 1 });
            return View(lstProfileData);
        }

        public ActionResult StampDuty()
        {
            IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Stamp Duty", IntSearchType = 1 });
            return View(lstProfileData);
        }

        public ActionResult SignUp()
        {
            return View();
        }

        public JsonResult GetProfileDetails(int ProfileID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            usp_GetProfileList_Result mObjProfileDetails = new BLProfile().BL_GetProfileDetails(new Profile() { IntStatus = 2, ProfileID = ProfileID });

            if (mObjProfileDetails != null)
            {
                dcResponse["success"] = true;
                dcResponse["ProfileDetails"] = mObjProfileDetails;
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Request";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetHeader()
        {
            return PartialView("_Header");
        }

        [Route("tr/{DocumentUrl}")]
        [HttpGet]
        public ActionResult TreasuryReceiptView(string DocumentUrl)
        {
            if (!string.IsNullOrWhiteSpace(DocumentUrl))
            {
                usp_GetTreasuryReceiptList_Result mObjTreasuryReceiptData = new BLTreasuryReceipt().BL_GetTreasuryReceiptDetails(new Treasury_Receipt() { StatusID = 1, DocumentUrl = DocumentUrl });

                if (mObjTreasuryReceiptData != null)
                {
                    return View(mObjTreasuryReceiptData);
                }
            }

            return RedirectToAction("Home", "Default");
        }

    }
}