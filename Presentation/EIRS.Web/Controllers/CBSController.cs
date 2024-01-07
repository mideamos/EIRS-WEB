using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EIRS.BOL;
using EIRS.BLL;
using EIRS.Web.Models;
using System.Net;
using System.IO;
using EIRS.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System;
using System.Net.Http.Headers;
using EIRS.Models;
using static EIRS.Web.Controllers.Filters;
using EIRS.Repository;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using System.IdentityModel.Tokens.Jwt;

namespace EIRS.Web.Controllers
{
    public class CBSController : Controller
    {
        EIRSEntities _db= new EIRSEntities();
        ITreasuryReceiptRepository _repo;
        public CBSController()
        {
            _repo = new TreasuryReceiptRepository();
        }
        // GET: CBS
        public ActionResult Home()
        {
            return View();
        }

        public ActionResult PayTaxes()
        {
            return View();
        }

        public ActionResult SearchBusiness()
        {
            return View();
        }

        public ActionResult SearchBData(FormCollection pObjFormCollection)
        {
            string strTitleFilter = pObjFormCollection.Get("txtBusinessName");

            MST_Business mObjBusiness = new MST_Business()
            {
                intStatus = 1,
                intClaimed = 0,
                BusinessName = strTitleFilter
            };

            IList<EIRS.BOL.usp_GetClaimBusinessList_Result> lstBusiness = new BLClaimBusiness().BL_GetBusinessList(mObjBusiness);
            lstBusiness = lstBusiness.Take(5).ToList();
            return PartialView(lstBusiness);
        }

        public ActionResult ClaimBusiness(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                EIRS.BOL.usp_GetClaimBusinessList_Result mObjBusinessData = new BLClaimBusiness().BL_GetBusinessDetails(new MST_Business() { BusinessID = id.GetValueOrDefault(), intClaimed = 0, intStatus = 1 });

                if (mObjBusinessData != null)
                {
                    ClaimBusinessViewModel mObjClaimBusiness = new ClaimBusinessViewModel()
                    {
                        BusinessID = mObjBusinessData.BusinessID.GetValueOrDefault(),
                        BusinessName = mObjBusinessData.BusinessName,
                        ContactPersonName = mObjBusinessData.ContactName,
                        MobileNumber = mObjBusinessData.Phone
                    };

                    ViewBag.MessageSent = false;
                    return View(mObjClaimBusiness);
                }
                else
                {
                    return RedirectToAction("Home", "Default");
                }
            }
            else
            {
                return RedirectToAction("Home", "Default");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult ClaimBusiness(ClaimBusinessViewModel pObjBusinessModel)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.MessageSent = false;
                return View(pObjBusinessModel);
            }
            else
            {
                MST_Business mObjBusiness = new MST_Business()
                {
                    BusinessID = pObjBusinessModel.BusinessID,
                    ContactName = pObjBusinessModel.ContactPersonName,
                    Phone = pObjBusinessModel.MobileNumber,
                };

                FuncResponse mObjFuncResponse = new BLClaimBusiness().BL_UpdateBusiness(mObjBusiness);

                if (mObjFuncResponse.Success)
                {
                    string url = "https://api.authy.com/protected/json/phones/verification/start?via=sms&phone_number=" + pObjBusinessModel.MobileNumber + "&country_code=234&locale=en";

                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                    httpWebRequest.Headers["X-Authy-API-Key"] = "QRBzvYZwcjbdeAJgStKOeSLB2anj2E82";
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";

                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var result = streamReader.ReadToEnd();
                        JObject mObjJResponse = JObject.Parse(result);
                        if (TrynParse.parseBool(mObjJResponse["success"]))
                        {
                            ViewBag.MessageSent = true;
                        }
                        else
                        {
                            ViewBag.MessageSent = false;
                            ViewBag.Message = mObjJResponse["message"];
                        }

                    }
                    ViewBag.MessageSent = true;
                    return View(pObjBusinessModel);
                }
                else
                {
                    ViewBag.MessageSent = false;
                    ViewBag.Message = mObjFuncResponse.Message;
                    return View(pObjBusinessModel);
                }
            }
        }

        public ActionResult ValidateOTP(ClaimBusinessViewModel pObjBusinessModel, FormCollection pObjFormCollection)
        {
            string strOTP = pObjFormCollection.Get("txtOTP");

            if (string.IsNullOrWhiteSpace(strOTP))
            {
                ViewBag.MessageSent = true;
                return View("ClaimBusiness", pObjBusinessModel);
            }
            else
            {
                string url = "https://api.authy.com/protected/json/phones/verification/check?country_code=234&phone_number=" + pObjBusinessModel.MobileNumber + "&verification_code=" + strOTP + "&locale=en";

                var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
                httpWebRequest.Headers["X-Authy-API-Key"] = "QRBzvYZwcjbdeAJgStKOeSLB2anj2E82";
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "GET";

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                    JObject mObjJResponse = JObject.Parse(result);
                    if (TrynParse.parseBool(mObjJResponse["success"]))
                    {
                        new BLClaimBusiness().BL_UpdatePhoneVerified(new MST_Business() { BusinessID = pObjBusinessModel.BusinessID });
                        return RedirectToAction("UpdateBusiness", "CBS", new { id = pObjBusinessModel.BusinessID, name = pObjBusinessModel.BusinessName.ToSeoUrl() });
                    }
                    else
                    {
                        ViewBag.OTPMessage = mObjJResponse["message"];
                        ViewBag.MessageSent = true;
                        return View("ClaimBusiness", pObjBusinessModel);
                    }

                }
            }
        }

        public ActionResult UpdateBusiness(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                EIRS.BOL.usp_GetClaimBusinessList_Result mObjBusinessData = new BLClaimBusiness().BL_GetBusinessDetails(new MST_Business() { BusinessID = id.GetValueOrDefault(), intClaimed = 0, intStatus = 1 });

                if (mObjBusinessData != null && mObjBusinessData.PhoneVerified.GetValueOrDefault())
                {
                    UpdateBusinessViewModel mObjBusinessModel = new UpdateBusinessViewModel()
                    {
                        BusinessID = mObjBusinessData.BusinessID.GetValueOrDefault(),
                        BusinessName = mObjBusinessData.BusinessName,
                        AssetTypeID = (int)EnumList.AssetTypes.Business,
                    };

                    UI_FillDropDown(mObjBusinessData, mObjBusinessModel, 1);

                    return View(mObjBusinessModel);
                }
                else
                {
                    return RedirectToAction("Home", "Default");
                }
            }
            else
            {
                return RedirectToAction("Home", "Default");
            }
        }

        private void UI_FillDropDown(EIRS.BOL.usp_GetClaimBusinessList_Result pObjBusinessData, UpdateBusinessViewModel pObjUpdateBusinessModel, int pIntFrom)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalDefaultValues.APIURL);

                //Tax Payer Type
                var responseTask = client.PostAsync("ReferenceData/TaxPayerType/List", null);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    //var readTask = result.Content.ReadAsAsync<APIResponse>();
                    //readTask.Wait();

                    //mObjAPIResponse = readTask.Result;
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);
                    if (mObjAPIResponse.Success)
                    {
                        IList<EIRS.BOL.usp_GetTaxPayerTypeList_Result> lstTaxPayerType = JsonConvert.DeserializeObject<IList<EIRS.BOL.usp_GetTaxPayerTypeList_Result>>(mObjAPIResponse.Result.ToString());
                        ViewBag.TaxPayerTypeList = new SelectList(lstTaxPayerType, "TaxPayerTypeID", "TaxPayerTypeName");
                    }
                }

                //Tax Payer Role

                var mobjTaxPayerRole = JsonConvert.SerializeObject(new TaxPayer_Roles() { AssetTypeID = (int)EnumList.AssetTypes.Business, intStatus = 1 });
                var buffer = System.Text.Encoding.UTF8.GetBytes(mobjTaxPayerRole);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                responseTask = client.PostAsync("ReferenceData/TaxPayerRole/List", byteContent);
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    //var readTask = result.Content.ReadAsAsync<APIResponse>();
                    //readTask.Wait();

                    //mObjAPIResponse = readTask.Result;
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);
                    if (mObjAPIResponse.Success)
                    {
                        IList<usp_GetTaxPayerRoleList_Result> lstTaxPayerRole = JsonConvert.DeserializeObject<IList<usp_GetTaxPayerRoleList_Result>>(mObjAPIResponse.Result.ToString());
                        ViewBag.TaxPayerRoleList = new SelectList(lstTaxPayerRole, "TaxPayerRoleID", "TaxPayerRoleName");
                    }
                }

                //Asset Type
                responseTask = client.PostAsync("ReferenceData/AssetType/List", null);
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    //var readTask = result.Content.ReadAsAsync<APIResponse>();
                    //readTask.Wait();

                    //mObjAPIResponse = readTask.Result;
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);
                    if (mObjAPIResponse.Success)
                    {
                        IList<usp_GetAssetTypeList_Result> lstAssetType = JsonConvert.DeserializeObject<IList<usp_GetAssetTypeList_Result>>(mObjAPIResponse.Result.ToString());
                        ViewBag.AssetTypeList = new SelectList(lstAssetType, "AssetTypeID", "AssetTypeName", (int)EnumList.AssetTypes.Business);
                    }
                }

                //Business Type
                responseTask = client.PostAsync("ReferenceData/BusinessType/List", null);
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    //var readTask = result.Content.ReadAsAsync<APIResponse>();
                    //readTask.Wait();

                    //mObjAPIResponse = readTask.Result;
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);
                    if (mObjAPIResponse.Success)
                    {
                        IList<usp_GetBusinessTypeList_Result> lstBusinessType = JsonConvert.DeserializeObject<IList<usp_GetBusinessTypeList_Result>>(mObjAPIResponse.Result.ToString());

                        if (pIntFrom == 1)
                        {
                            int mIntBusinessTypeID = 0;
                            if (lstBusinessType.Where(t => t.BusinessTypeName.ToLower().Equals(pObjBusinessData.BusinessType.ToLower())).Count() > 0)
                            {
                                mIntBusinessTypeID = lstBusinessType.Where(t => t.BusinessTypeName.ToLower().Equals(pObjBusinessData.BusinessType.ToLower())).FirstOrDefault().BusinessTypeID.GetValueOrDefault();
                            }

                            pObjUpdateBusinessModel.BusinessTypeID = mIntBusinessTypeID;
                            ViewBag.BusinessTypeList = new SelectList(lstBusinessType, "BusinessTypeID", "BusinessTypeName", mIntBusinessTypeID);
                        }
                        else
                        {
                            ViewBag.BusinessTypeList = new SelectList(lstBusinessType, "BusinessTypeID", "BusinessTypeName");
                        }
                    }
                }

                //LGA
                responseTask = client.PostAsync("ReferenceData/LGA/List", null);
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    //var readTask = result.Content.ReadAsAsync<APIResponse>();
                    //readTask.Wait();

                    //mObjAPIResponse = readTask.Result;
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);
                    if (mObjAPIResponse.Success)
                    {
                        IList<usp_GetLGAList_Result> lstLGA = JsonConvert.DeserializeObject<IList<usp_GetLGAList_Result>>(mObjAPIResponse.Result.ToString());

                        if (pIntFrom == 1)
                        {
                            int mIntLGAID = 0;
                            if (lstLGA.Where(t => t.LGAName.ToLower().Equals(pObjBusinessData.LGA.ToLower())).Count() > 0)
                            {
                                mIntLGAID = lstLGA.Where(t => t.LGAName.ToLower().Equals(pObjBusinessData.LGA.ToLower())).FirstOrDefault().LGAID.GetValueOrDefault();
                            }

                            pObjUpdateBusinessModel.LGAID = mIntLGAID;
                            ViewBag.LGAList = new SelectList(lstLGA, "LGAID", "LGAName", mIntLGAID);
                        }
                        else
                        {
                            ViewBag.LGAList = new SelectList(lstLGA, "LGAID", "LGAName");
                        }
                    }
                }

                //BusinessCategory

                var mobjBusinessCategory = JsonConvert.SerializeObject(new Business_Category() { BusinessTypeID = pObjUpdateBusinessModel.BusinessTypeID, intStatus = 1 });
                buffer = System.Text.Encoding.UTF8.GetBytes(mobjBusinessCategory);
                byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                responseTask = client.PostAsync("ReferenceData/BusinessCategory/List", byteContent);
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    //var readTask = result.Content.ReadAsAsync<APIResponse>();
                    //readTask.Wait();

                    //mObjAPIResponse = readTask.Result;
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);
                    if (mObjAPIResponse.Success)
                    {
                        IList<usp_GetBusinessCategoryList_Result> lstBusinessCategory = JsonConvert.DeserializeObject<IList<usp_GetBusinessCategoryList_Result>>(mObjAPIResponse.Result.ToString());

                        if (pIntFrom == 1)
                        {
                            int mIntBusinessCategoryID = 0;
                            if (lstBusinessCategory.Where(t => t.BusinessCategoryName.ToLower().Equals(pObjBusinessData.BusinessCategory.ToLower())).Count() > 0)
                            {
                                mIntBusinessCategoryID = lstBusinessCategory.Where(t => t.BusinessCategoryName.ToLower().Equals(pObjBusinessData.BusinessCategory.ToLower())).FirstOrDefault().BusinessCategoryID.GetValueOrDefault();
                            }

                            pObjUpdateBusinessModel.BusinessCategoryID = mIntBusinessCategoryID;
                            ViewBag.BusinessCategoryList = new SelectList(lstBusinessCategory, "BusinessCategoryID", "BusinessCategoryName", mIntBusinessCategoryID);
                        }
                        else
                        {
                            ViewBag.BusinessCategoryList = new SelectList(lstBusinessCategory, "BusinessCategoryID", "BusinessCategoryName");
                        }

                    }
                }

                //BusinessSector

                var mobjBusinessSector = JsonConvert.SerializeObject(new Business_Sector() { BusinessTypeID = pObjUpdateBusinessModel.BusinessTypeID, BusinessCategoryID = pObjUpdateBusinessModel.BusinessCategoryID, intStatus = 1 });
                buffer = System.Text.Encoding.UTF8.GetBytes(mobjBusinessSector);
                byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                responseTask = client.PostAsync("ReferenceData/BusinessSector/List", byteContent);
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    //var readTask = result.Content.ReadAsAsync<APIResponse>();
                    //readTask.Wait();

                    //mObjAPIResponse = readTask.Result;
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);
                    if (mObjAPIResponse.Success)
                    {
                        IList<usp_GetBusinessSectorList_Result> lstBusinessSector = JsonConvert.DeserializeObject<IList<usp_GetBusinessSectorList_Result>>(mObjAPIResponse.Result.ToString());

                        if (pIntFrom == 1)
                        {
                            int mIntBusinessSectorID = 0;
                            if (lstBusinessSector.Where(t => t.BusinessSectorName.ToLower().Equals(pObjBusinessData.BusinessSector.ToLower())).Count() > 0)
                            {
                                mIntBusinessSectorID = lstBusinessSector.Where(t => t.BusinessSectorName.ToLower().Equals(pObjBusinessData.BusinessSector.ToLower())).FirstOrDefault().BusinessSectorID.GetValueOrDefault();
                            }

                            pObjUpdateBusinessModel.BusinessSectorID = mIntBusinessSectorID;
                            ViewBag.BusinessSectorList = new SelectList(lstBusinessSector, "BusinessSectorID", "BusinessSectorName", mIntBusinessSectorID);
                        }
                        else
                        {
                            ViewBag.BusinessSectorList = new SelectList(lstBusinessSector, "BusinessSectorID", "BusinessSectorName");
                        }
                    }
                }

                //BusinessSubSector

                var mobjBusinessSubSector = JsonConvert.SerializeObject(new Business_SubSector() { BusinessSectorID = pObjUpdateBusinessModel.BusinessSectorID, intStatus = 1 });
                buffer = System.Text.Encoding.UTF8.GetBytes(mobjBusinessSubSector);
                byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                responseTask = client.PostAsync("ReferenceData/BusinessSubSector/List", byteContent);
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);

                    if (mObjAPIResponse.Success)
                    {
                        IList<usp_GetBusinessSubSectorList_Result> lstBusinessSubSector = JsonConvert.DeserializeObject<IList<usp_GetBusinessSubSectorList_Result>>(mObjAPIResponse.Result.ToString());

                        if (pIntFrom == 1)
                        {
                            int mIntBusinessSubSectorID = 0;

                            if (lstBusinessSubSector.Where(t => t.BusinessSubSectorName.ToLower().Equals(pObjBusinessData.BusinessSubSector.ToLower())).Count() > 0)
                            {
                                mIntBusinessSubSectorID = lstBusinessSubSector.Where(t => t.BusinessSubSectorName.ToLower().Equals(pObjBusinessData.BusinessSubSector.ToLower())).FirstOrDefault().BusinessSubSectorID.GetValueOrDefault();
                            }

                            pObjUpdateBusinessModel.BusinessSubSectorID = mIntBusinessSubSectorID;
                            ViewBag.BusinessSubSectorList = new SelectList(lstBusinessSubSector, "BusinessSubSectorID", "BusinessSubSectorName", mIntBusinessSubSectorID);
                        }
                        else
                        {
                            ViewBag.BusinessSubSectorList = new SelectList(lstBusinessSubSector, "BusinessSubSectorID", "BusinessSubSectorName");
                        }
                    }
                }

                //BusinessStructure

                var mobjBusinessStructure = JsonConvert.SerializeObject(new Business_Structure() { BusinessTypeID = pObjUpdateBusinessModel.BusinessTypeID, intStatus = 1 });
                buffer = System.Text.Encoding.UTF8.GetBytes(mobjBusinessStructure);
                byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                responseTask = client.PostAsync("ReferenceData/BusinessStructure/List", byteContent);
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);

                    if (mObjAPIResponse.Success)
                    {
                        IList<usp_GetBusinessStructureList_Result> lstBusinessStructure = JsonConvert.DeserializeObject<IList<usp_GetBusinessStructureList_Result>>(mObjAPIResponse.Result.ToString());
                        if (pIntFrom == 1)
                        {
                            int mIntBusinessStructureID = 0;
                            if (lstBusinessStructure.Where(t => t.BusinessStructureName.ToLower().Equals(pObjBusinessData.BusinessStructure.ToLower())).Count() > 0)
                            {
                                mIntBusinessStructureID = lstBusinessStructure.Where(t => t.BusinessStructureName.ToLower().Equals(pObjBusinessData.BusinessStructure.ToLower())).FirstOrDefault().BusinessStructureID.GetValueOrDefault();
                            }

                            pObjUpdateBusinessModel.BusinessStructureID = mIntBusinessStructureID;
                            ViewBag.BusinessStructureList = new SelectList(lstBusinessStructure, "BusinessStructureID", "BusinessStructureName", mIntBusinessStructureID);
                        }
                        else
                        {
                            ViewBag.BusinessStructureList = new SelectList(lstBusinessStructure, "BusinessStructureID", "BusinessStructureName");
                        }
                    }
                }

                //BusinessOperation
                var mobjBusinessOperation = JsonConvert.SerializeObject(new Business_Operation() { BusinessTypeID = pObjUpdateBusinessModel.BusinessTypeID, intStatus = 1 });
                buffer = System.Text.Encoding.UTF8.GetBytes(mobjBusinessOperation);
                byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                responseTask = client.PostAsync("ReferenceData/BusinessOperation/List", byteContent);
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);
                    if (mObjAPIResponse.Success)
                    {
                        IList<usp_GetBusinessOperationList_Result> lstBusinessOperation = JsonConvert.DeserializeObject<IList<usp_GetBusinessOperationList_Result>>(mObjAPIResponse.Result.ToString());

                        if (pIntFrom == 1)
                        {
                            int mIntBusinessOperationID = 0;
                            if (lstBusinessOperation.Where(t => t.BusinessOperationName.ToLower().Equals(pObjBusinessData.BusinessOperation.ToLower())).Count() > 0)
                            {
                                mIntBusinessOperationID = lstBusinessOperation.Where(t => t.BusinessOperationName.ToLower().Equals(pObjBusinessData.BusinessOperation.ToLower())).FirstOrDefault().BusinessOperationID.GetValueOrDefault();
                            }

                            pObjUpdateBusinessModel.BusinessOperationID = mIntBusinessOperationID;
                            ViewBag.BusinessOperationList = new SelectList(lstBusinessOperation, "BusinessOperationID", "BusinessOperationName", mIntBusinessOperationID);
                        }
                        else
                        {
                            ViewBag.BusinessOperationList = new SelectList(lstBusinessOperation, "BusinessOperationID", "BusinessOperationName");
                        }
                    }
                }

                //Size
                responseTask = client.PostAsync("ReferenceData/Size/List", null);
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);
                    if (mObjAPIResponse.Success)
                    {
                        IList<usp_GetSizeList_Result> lstBusinessSize = JsonConvert.DeserializeObject<IList<usp_GetSizeList_Result>>(mObjAPIResponse.Result.ToString());
                        if (pIntFrom == 1)
                        {
                            int mIntBusinessSizeID = 0;
                            if (lstBusinessSize.Where(t => t.SizeName.ToLower().Equals(pObjBusinessData.Size.ToLower())).Count() > 0)
                            {
                                mIntBusinessSizeID = lstBusinessSize.Where(t => t.SizeName.ToLower().Equals(pObjBusinessData.Size.ToLower())).FirstOrDefault().SizeID.GetValueOrDefault();
                            }

                            pObjUpdateBusinessModel.SizeID = mIntBusinessSizeID;
                            ViewBag.BusinessSizeList = new SelectList(lstBusinessSize, "SizeID", "SizeName", mIntBusinessSizeID);
                        }
                        else
                        {
                            ViewBag.BusinessSizeList = new SelectList(lstBusinessSize, "SizeID", "SizeName");
                        }
                    }
                }

                //TaxOffice
                responseTask = client.PostAsync("ReferenceData/TaxOffice/List", null);
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);

                    if (mObjAPIResponse.Success)
                    {
                        IList<usp_GetTaxOfficeList_Result> lstTaxOffice = JsonConvert.DeserializeObject<IList<usp_GetTaxOfficeList_Result>>(mObjAPIResponse.Result.ToString());
                        ViewBag.TaxOfficeList = new SelectList(lstTaxOffice, "TaxOfficeID", "TaxOfficeName");
                    }
                }

                //EconomicActivities
                responseTask = client.PostAsync("ReferenceData/EconomicActivities/List", null);
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);

                    if (mObjAPIResponse.Success)
                    {
                        IList<usp_GetEconomicActivitiesList_Result> lstEconomicActivities = JsonConvert.DeserializeObject<IList<usp_GetEconomicActivitiesList_Result>>(mObjAPIResponse.Result.ToString());
                        ViewBag.EconomicActivitiesList = new SelectList(lstEconomicActivities, "EconomicActivitiesID", "EconomicActivitiesName");
                    }
                }

                //NotificationMethod
                responseTask = client.PostAsync("ReferenceData/NotificationMethod/List", null);
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);
                    if (mObjAPIResponse.Success)
                    {
                        IList<usp_GetNotificationMethodList_Result> lstNotificationMethod = JsonConvert.DeserializeObject<IList<usp_GetNotificationMethodList_Result>>(mObjAPIResponse.Result.ToString());
                        ViewBag.NotificationMethodList = new SelectList(lstNotificationMethod, "NotificationMethodID", "NotificationMethodName");
                    }
                }

                //Gender
                responseTask = client.PostAsync("ReferenceData/Common/Gender", null);
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);
                    if (mObjAPIResponse.Success)
                    {
                        IList<DropDownListResult> lstGender = JsonConvert.DeserializeObject<IList<DropDownListResult>>(mObjAPIResponse.Result.ToString());
                        ViewBag.GenderList = new SelectList(lstGender, "id", "text");
                    }
                }

                //Title
                responseTask = client.PostAsync("ReferenceData/Title/List", null);
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);
                    if (mObjAPIResponse.Success)
                    {
                        IList<usp_GetTitleList_Result> lstTitle = JsonConvert.DeserializeObject<IList<usp_GetTitleList_Result>>(mObjAPIResponse.Result.ToString());
                        ViewBag.TitleList = new SelectList(lstTitle, "TitleID", "TitleName");
                    }
                }

                //MaritalStatus
                responseTask = client.PostAsync("ReferenceData/Common/MaritalStatus", null);
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);
                    if (mObjAPIResponse.Success)
                    {
                        IList<DropDownListResult> lstMaritalStatus = JsonConvert.DeserializeObject<IList<DropDownListResult>>(mObjAPIResponse.Result.ToString());
                        ViewBag.MaritalStatusList = new SelectList(lstMaritalStatus, "id", "text");
                    }
                }

                //Nationality
                responseTask = client.PostAsync("ReferenceData/Common/Nationality", null);
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);

                    if (mObjAPIResponse.Success)
                    {
                        IList<DropDownListResult> lstNationality = JsonConvert.DeserializeObject<IList<DropDownListResult>>(mObjAPIResponse.Result.ToString());
                        ViewBag.NationalityList = new SelectList(lstNationality, "id", "text");
                    }
                }

                //Government Type
                responseTask = client.PostAsync("ReferenceData/GovernmentType/List", null);
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);
                    if (mObjAPIResponse.Success)
                    {
                        IList<usp_GetGovernmentTypeList_Result> lstGovernmentType = JsonConvert.DeserializeObject<IList<usp_GetGovernmentTypeList_Result>>(mObjAPIResponse.Result.ToString());
                        ViewBag.GovernmentTypeList = new SelectList(lstGovernmentType, "GovernmentTypeID", "GovernmentTypeName");
                    }
                }

            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult UpdateBusiness(UpdateBusinessViewModel pObjBusinessModel)
        {
            EIRS.BOL.usp_GetClaimBusinessList_Result mObjBusinessData = new BLClaimBusiness().BL_GetBusinessDetails(new MST_Business() { BusinessID = pObjBusinessModel.BusinessID, intClaimed = 0, intStatus = 1 });

            if (!ModelState.IsValid)
            {
                UI_FillDropDown(mObjBusinessData, pObjBusinessModel, 2);
                return View(pObjBusinessModel);
            }
            else
            {
                APIResponse mObjTaxPayerResponse = new APIResponse();
                int mIntTaxPayerID = 0;

                if (pObjBusinessModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
                {
                    IndividualViewModel mObjIndividual = new IndividualViewModel()
                    {
                        IndividualID = 0,
                        GenderID = pObjBusinessModel.IND_GenderID.GetValueOrDefault(),
                        TitleID = pObjBusinessModel.IND_TitleID.GetValueOrDefault(),
                        FirstName = pObjBusinessModel.IND_FirstName,
                        LastName = pObjBusinessModel.IND_LastName,
                        MiddleName = pObjBusinessModel.IND_MiddleName,
                        DOB = pObjBusinessModel.IND_DOB,
                        TIN = pObjBusinessModel.IND_TIN,
                        MobileNumber1 = pObjBusinessModel.IND_MobileNumber1,
                        MobileNumber2 = pObjBusinessModel.IND_MobileNumber2,
                        EmailAddress1 = pObjBusinessModel.IND_EmailAddress1,
                        EmailAddress2 = pObjBusinessModel.IND_EmailAddress2,
                        BiometricDetails = pObjBusinessModel.IND_BiometricDetails,
                        TaxOfficeID = pObjBusinessModel.IND_TaxOfficeID,
                        MaritalStatusID = pObjBusinessModel.IND_MaritalStatusID,
                        NationalityID = pObjBusinessModel.IND_NationalityID.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                        EconomicActivitiesID = pObjBusinessModel.IND_EconomicActivitiesID.GetValueOrDefault(),
                        NotificationMethodID = pObjBusinessModel.IND_NotificationMethodID.GetValueOrDefault(),
                        Active = false,
                    };

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(GlobalDefaultValues.APIURL);

                        var myContent = JsonConvert.SerializeObject(mObjIndividual);
                        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                        var byteContent = new ByteArrayContent(buffer);
                        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        var responseTask = client.PostAsync("TaxPayer/Indivdual/Insert", byteContent);
                        responseTask.Wait();

                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var readTask = result.Content.ReadAsStringAsync();
                            // var readTask = result.Content.ReadAsAsync<APIResponse>();
                            readTask.Wait();

                            string res = readTask.Result.ToString();
                            mObjTaxPayerResponse = JsonConvert.DeserializeObject<APIResponse>(res);

                           // mObjTaxPayerResponse = readTask.Result;

                            EIRS.BOL.Individual mObjIndividualData = JsonConvert.DeserializeObject<EIRS.BOL.Individual>(mObjTaxPayerResponse.Result.ToString());
                            mIntTaxPayerID = mObjIndividualData.IndividualID;
                        }
                    }
                }
                else if (pObjBusinessModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
                {
                    CompanyViewModel mObjCompanyModel = new CompanyViewModel()
                    {
                        CompanyID = 0,
                        CompanyName = pObjBusinessModel.COMP_CompanyName,
                        TIN = pObjBusinessModel.COMP_CompanyName,
                        MobileNumber1 = pObjBusinessModel.COMP_MobileNumber1,
                        MobileNumber2 = pObjBusinessModel.COMP_MobileNumber2,
                        EmailAddress1 = pObjBusinessModel.COMP_EmailAddress1,
                        EmailAddress2 = pObjBusinessModel.COMP_EmailAddress2,
                        TaxOfficeID = pObjBusinessModel.COMP_TaxOfficeID,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                        EconomicActivitiesID = pObjBusinessModel.COMP_EconomicActivitiesID.GetValueOrDefault(),
                        NotificationMethodID = pObjBusinessModel.COMP_NotificationMethodID.GetValueOrDefault(),
                        Active = false,
                    };

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(GlobalDefaultValues.APIURL);

                        var myContent = JsonConvert.SerializeObject(mObjCompanyModel);
                        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                        var byteContent = new ByteArrayContent(buffer);
                        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        var responseTask = client.PostAsync("TaxPayer/Company/Insert", byteContent);
                        responseTask.Wait();

                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var readTask = result.Content.ReadAsStringAsync();
                            // var readTask = result.Content.ReadAsAsync<APIResponse>();
                            readTask.Wait();

                            string res = readTask.Result.ToString();
                            mObjTaxPayerResponse = JsonConvert.DeserializeObject<APIResponse>(res);

                            EIRS.BOL.Company mObjCompanyData = JsonConvert.DeserializeObject<EIRS.BOL.Company>(mObjTaxPayerResponse.Result.ToString());
                            mIntTaxPayerID = mObjCompanyData.CompanyID;
                        }
                    }
                }
                else if (pObjBusinessModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
                {
                    SpecialViewModel mObjSpecialModel = new SpecialViewModel()
                    {
                        SpecialName = pObjBusinessModel.SP_SpecialName,
                        TaxOfficeID = pObjBusinessModel.SP_TaxOfficeID,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Special,
                        ContactNumber = pObjBusinessModel.SP_ContactNumber,
                        ContactEmail = pObjBusinessModel.SP_ContactEmail,
                        ContactName = pObjBusinessModel.SP_ContactName,
                        Description = pObjBusinessModel.SP_Description,
                        NotificationMethodID = pObjBusinessModel.SP_NotificationMethodID.GetValueOrDefault(),
                        Active = false,
                    };

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(GlobalDefaultValues.APIURL);

                        var myContent = JsonConvert.SerializeObject(mObjSpecialModel);
                        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                        var byteContent = new ByteArrayContent(buffer);
                        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        var responseTask = client.PostAsync("TaxPayer/Special/Insert", byteContent);
                        responseTask.Wait();

                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var readTask = result.Content.ReadAsStringAsync();
                            // var readTask = result.Content.ReadAsAsync<APIResponse>();
                            readTask.Wait();

                            string res = readTask.Result.ToString();
                            mObjTaxPayerResponse = JsonConvert.DeserializeObject<APIResponse>(res);

                            EIRS.BOL.Special mObjSpecialData = JsonConvert.DeserializeObject<EIRS.BOL.Special>(mObjTaxPayerResponse.Result.ToString());
                            mIntTaxPayerID = mObjSpecialData.SpecialID;
                        }
                    }
                }
                else if (pObjBusinessModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
                {
                    GovernmentViewModel mObjGovernmentModel = new GovernmentViewModel()
                    {
                        GovernmentName = pObjBusinessModel.GOV_GovernmentName,
                        GovernmentTypeID = pObjBusinessModel.GOV_GovernmentTypeID.GetValueOrDefault(),
                        TaxOfficeID = pObjBusinessModel.GOV_TaxOfficeID,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                        ContactNumber = pObjBusinessModel.GOV_ContactNumber,
                        ContactEmail = pObjBusinessModel.GOV_ContactEmail,
                        ContactName = pObjBusinessModel.GOV_ContactName,
                        NotificationMethodID = pObjBusinessModel.GOV_NotificationMethodID.GetValueOrDefault(),
                        Active = false,
                    };

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(GlobalDefaultValues.APIURL);

                        var myContent = JsonConvert.SerializeObject(mObjGovernmentModel);
                        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                        var byteContent = new ByteArrayContent(buffer);
                        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        var responseTask = client.PostAsync("TaxPayer/Government/Insert", byteContent);
                        responseTask.Wait();

                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var readTask = result.Content.ReadAsStringAsync();
                            // var readTask = result.Content.ReadAsAsync<APIResponse>();
                            readTask.Wait();

                            string res = readTask.Result.ToString();
                            mObjTaxPayerResponse = JsonConvert.DeserializeObject<APIResponse>(res);

                            EIRS.BOL.Government mObjGovernmentData = JsonConvert.DeserializeObject<EIRS.BOL.Government>(mObjTaxPayerResponse.Result.ToString());
                            mIntTaxPayerID = mObjGovernmentData.GovernmentID;
                        }
                    }
                }
                else
                {
                    UI_FillDropDown(mObjBusinessData, pObjBusinessModel, 2);
                    return View(pObjBusinessModel);
                }

                if (mObjTaxPayerResponse.Success)
                {
                    BusinessViewModel mObjBusinessModel = new BusinessViewModel()
                    {
                        BusinessID = 0,
                        AssetTypeID = (int)EnumList.AssetTypes.Business,
                        BusinessTypeID = pObjBusinessModel.BusinessTypeID,
                        BusinessName = pObjBusinessModel.BusinessName,
                        LGAID = pObjBusinessModel.LGAID,
                        BusinessCategoryID = pObjBusinessModel.BusinessCategoryID,
                        BusinessSectorID = pObjBusinessModel.BusinessSectorID,
                        BusinessSubSectorID = pObjBusinessModel.BusinessSubSectorID,
                        BusinessStructureID = pObjBusinessModel.BusinessStructureID,
                        BusinessOperationID = pObjBusinessModel.BusinessOperationID,
                        SizeID = pObjBusinessModel.SizeID,
                        ContactName = mObjBusinessData.ContactName,
                        BusinessAddress = mObjBusinessData.BusinessAddress,
                        BusinessNumber = mObjBusinessData.Phone,
                        Active = false,
                    };

                    APIResponse mObjBusinessResponse = new APIResponse();

                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(GlobalDefaultValues.APIURL);

                        var myContent = JsonConvert.SerializeObject(mObjBusinessModel);
                        var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                        var byteContent = new ByteArrayContent(buffer);
                        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                        var responseTask = client.PostAsync("Asset/Business/Insert", byteContent);
                        responseTask.Wait();

                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var readTask = result.Content.ReadAsStringAsync();
                            // var readTask = result.Content.ReadAsAsync<APIResponse>();
                            readTask.Wait();

                            string res = readTask.Result.ToString();
                            mObjBusinessResponse = JsonConvert.DeserializeObject<APIResponse>(res);

                        }
                    }

                    if (mObjBusinessResponse.Success)
                    {
                        Business mObjEBusinessData = JsonConvert.DeserializeObject<Business>(mObjBusinessResponse.Result.ToString());

                        TaxPayerAssetViewModel mObjTPAModel = new TaxPayerAssetViewModel()
                        {
                            TaxPayerRoleID = pObjBusinessModel.TaxPayerRoleID,
                            AssetID = mObjEBusinessData.BusinessID,
                            AssetTypeID = (int)EnumList.AssetTypes.Business,
                            TaxPayerTypeID = pObjBusinessModel.TaxPayerTypeID,
                            TaxPayerIds = mIntTaxPayerID.ToString(),
                            Active = false,

                        };
                        APIResponse mObjTPAResponse = new APIResponse();

                        using (var client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(GlobalDefaultValues.APIURL);

                            var myContent = JsonConvert.SerializeObject(mObjTPAModel);
                            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                            var byteContent = new ByteArrayContent(buffer);
                            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                            var responseTask = client.PostAsync("Asset/Business/AddTaxPayer", byteContent);
                            responseTask.Wait();

                            var result = responseTask.Result;
                            if (result.IsSuccessStatusCode)
                            {
                                var readTask = result.Content.ReadAsStringAsync();
                                // var readTask = result.Content.ReadAsAsync<APIResponse>();
                                readTask.Wait();

                                string res = readTask.Result.ToString();
                                mObjTPAResponse = JsonConvert.DeserializeObject<APIResponse>(res);

                                //mObjTPAResponse = readTask.Result;
                            }
                        }

                        if (mObjTPAResponse.Success)
                        {
                            //Mark as Claimed

                            new BLClaimBusiness().BL_UpdateStatus(new MST_Business() { BusinessID = pObjBusinessModel.BusinessID });

                            return RedirectToAction("ThankYou");
                        }
                        else
                        {
                            UI_FillDropDown(mObjBusinessData, pObjBusinessModel, 2);
                            ViewBag.Message = mObjTPAResponse.Message;
                            return View(pObjBusinessModel);
                        }

                    }
                    else
                    {
                        UI_FillDropDown(mObjBusinessData, pObjBusinessModel, 2);
                        ViewBag.Message = mObjBusinessResponse.Message;
                        return View(pObjBusinessModel);
                    }
                }
                else
                {
                    UI_FillDropDown(mObjBusinessData, pObjBusinessModel, 2);
                    ViewBag.Message = mObjTaxPayerResponse.Message;
                    return View(pObjBusinessModel);
                }
            }
        }

        public ActionResult ThankYou()
        {
            return View();
        }

        public JsonResult GetBusinessDetails(int BusinessID)
        {
            EIRS.BOL.usp_GetClaimBusinessList_Result mObjBusinessData = new BLClaimBusiness().BL_GetBusinessDetails(new MST_Business() { BusinessID = BusinessID, intClaimed = 0, intStatus = 1 });
            return Json(mObjBusinessData, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaxPayerRole(int TaxPayerTypeID, int AssetTypeID)
        {
            APIResponse mObjAPIResponse = new APIResponse();
            IList<usp_GetTaxPayerRoleList_Result> lstTaxPayerRole = new List<usp_GetTaxPayerRoleList_Result>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalDefaultValues.APIURL);

                var mobjTaxPayerRole = JsonConvert.SerializeObject(new TaxPayer_Roles() { AssetTypeID = AssetTypeID, TaxPayerTypeID = TaxPayerTypeID, intStatus = 1 });
                var buffer = System.Text.Encoding.UTF8.GetBytes(mobjTaxPayerRole);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var responseTask = client.PostAsync("ReferenceData/TaxPayerRole/List", byteContent);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);

                   // mObjAPIResponse = readTask.Result;

                    if (mObjAPIResponse.Success)
                    {
                        lstTaxPayerRole = JsonConvert.DeserializeObject<IList<usp_GetTaxPayerRoleList_Result>>(mObjAPIResponse.Result.ToString());
                    }
                }
            }

            return Json(lstTaxPayerRole, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BusinessTypeChange(int BusinessTypeID)
        {
            APIResponse mObjAPIResponse = new APIResponse();
            IDictionary<string, object> dcResponse = new Dictionary<string, object>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalDefaultValues.APIURL);

                var mobjBusinessCategory = JsonConvert.SerializeObject(new Business_Category() { BusinessTypeID = BusinessTypeID, intStatus = 1 });
                var buffer = System.Text.Encoding.UTF8.GetBytes(mobjBusinessCategory);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var responseTask = client.PostAsync("ReferenceData/BusinessCategory/List", byteContent);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);

                   // mObjAPIResponse = readTask.Result;

                    if (mObjAPIResponse.Success)
                    {
                        IList<usp_GetBusinessCategoryList_Result> lstBusinessCategory = JsonConvert.DeserializeObject<IList<usp_GetBusinessCategoryList_Result>>(mObjAPIResponse.Result.ToString());
                        dcResponse["BusinessCategory"] = lstBusinessCategory;
                    }
                }

                //BusinessStructure

                var mobjBusinessStructure = JsonConvert.SerializeObject(new Business_Structure() { BusinessTypeID = BusinessTypeID, intStatus = 1 });
                buffer = System.Text.Encoding.UTF8.GetBytes(mobjBusinessStructure);
                byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                responseTask = client.PostAsync("ReferenceData/BusinessStructure/List", byteContent);
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);


                    if (mObjAPIResponse.Success)
                    {
                        IList<usp_GetBusinessStructureList_Result> lstBusinessStructure = JsonConvert.DeserializeObject<IList<usp_GetBusinessStructureList_Result>>(mObjAPIResponse.Result.ToString());
                        dcResponse["BusinessStructure"] = lstBusinessStructure;
                    }
                }

                //BusinessOperation
                var mobjBusinessOperation = JsonConvert.SerializeObject(new Business_Operation() { BusinessTypeID = BusinessTypeID, intStatus = 1 });
                buffer = System.Text.Encoding.UTF8.GetBytes(mobjBusinessOperation);
                byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                responseTask = client.PostAsync("ReferenceData/BusinessOperation/List", byteContent);
                responseTask.Wait();

                result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);

                    if (mObjAPIResponse.Success)
                    {
                        IList<usp_GetBusinessOperationList_Result> lstBusinessOperation = JsonConvert.DeserializeObject<IList<usp_GetBusinessOperationList_Result>>(mObjAPIResponse.Result.ToString());
                        dcResponse["BusinessOperation"] = lstBusinessOperation;

                    }
                }
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBusinessSector(int BusinessCategoryID)
        {
            APIResponse mObjAPIResponse = new APIResponse();
            IList<usp_GetBusinessSectorList_Result> lstBusinessSector = new List<usp_GetBusinessSectorList_Result>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalDefaultValues.APIURL);

                var mobjBusinessSector = JsonConvert.SerializeObject(new Business_Sector() { BusinessCategoryID = BusinessCategoryID, intStatus = 1 });
                var buffer = System.Text.Encoding.UTF8.GetBytes(mobjBusinessSector);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var responseTask = client.PostAsync("ReferenceData/BusinessSector/List", byteContent);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);

                    if (mObjAPIResponse.Success)
                    {
                        lstBusinessSector = JsonConvert.DeserializeObject<IList<usp_GetBusinessSectorList_Result>>(mObjAPIResponse.Result.ToString());
                    }
                }
            }

            return Json(lstBusinessSector, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBusinessSubSector(int BusinessSectorID)
        {
            APIResponse mObjAPIResponse = new APIResponse();
            IList<usp_GetBusinessSubSectorList_Result> lstBusinessSubSector = new List<usp_GetBusinessSubSectorList_Result>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(GlobalDefaultValues.APIURL);

                var mobjBusinessSubSector = JsonConvert.SerializeObject(new Business_SubSector() { BusinessSectorID = BusinessSectorID, intStatus = 1 });
                var buffer = System.Text.Encoding.UTF8.GetBytes(mobjBusinessSubSector);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var responseTask = client.PostAsync("ReferenceData/BusinessSubSector/List", byteContent);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    // var readTask = result.Content.ReadAsAsync<APIResponse>();
                    readTask.Wait();

                    string res = readTask.Result.ToString();
                    mObjAPIResponse = JsonConvert.DeserializeObject<APIResponse>(res);
                    if (mObjAPIResponse.Success)
                    {
                        lstBusinessSubSector = JsonConvert.DeserializeObject<IList<usp_GetBusinessSubSectorList_Result>>(mObjAPIResponse.Result.ToString());
                    }
                }
            }

            return Json(lstBusinessSubSector, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult VerifyTreasuryReceipt()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult VerifyTreasuryReceipt(VerifyTreasuryReceiptViewModel pObjVerifyTRModel)
        {
            if (!ModelState.IsValid)
            {
                return View(pObjVerifyTRModel);
            }
            else
            {
                var vTreasuryReceiptData =  _repo.REP_VerifyTreasuryReceiptNew(new Treasury_Receipt()
                {
                    BillRefNo = pObjVerifyTRModel.BillNumber,
                    ReceiptRefNo = pObjVerifyTRModel.ReceiptNumber
                });

                if(vTreasuryReceiptData != null)
                {
                    var checker = _db.Assessments.FirstOrDefault(o => o.AssessmentID == vTreasuryReceiptData.AssessmentID);
                    if(checker != null && checker.AssessmentRefNo == pObjVerifyTRModel.BillNumber)
                    {
                        //TreasuryReceipt/240524/RP230255_14092022_Signed.pdf
                        ViewBag.TRValid = true;
                        //ViewBag.TRDocumentUrl = $"{GlobalDefaultValues.SiteLink}/Document/TreasuryReceipt/222823/RP212878_15072022_Generated.pdf";
                        ViewBag.TRDocumentUrl = $"{GlobalDefaultValues.SiteLink}/Document/{vTreasuryReceiptData.SignedPath}";
                    }
                    else
                    {
                        ViewBag.TRValid = false;
                    }
                }
                else
                {
                    ViewBag.TRValid = false;
                }

                ModelState.Clear();
                return View();
            }
        }
    }
}