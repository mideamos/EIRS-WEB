using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using static EIRS.Web.Controllers.Filters;
using EIRS.Web.Utility;

namespace EIRS.Web.Controllers
{
    [SessionTimeout]
    public partial class ProfileController : BaseController
    {
        #region PAYE
        
        public ActionResult PAYE()
        {
            return View();
        }

        
        public ActionResult PAYEPCME()
        {
            return View();
        }

        public JsonResult GetPAYEPCMEData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Pay As You Earn - PAYE Collections - Multiple Employees", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult PAYEPCME_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PAYEPCME");
                }
            }
            else
            {
                return RedirectToAction("PAYEPCME");
            }
        }

        public JsonResult GetPAYEPCMETaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult PAYEPCME_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PAYECME");
                }
            }
            else
            {
                return RedirectToAction("PAYECME");
            }
        }

        public JsonResult GetPAYEPCMEAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult PAYEPCFBE()
        {
            return View();
        }

        
        public ActionResult PAYEPCFBE_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PAYEPCFBE");
                }
            }
            else
            {
                return RedirectToAction("PAYEPCFBE");
            }
        }

        
        public ActionResult PAYEPCFBE_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PAYEPCFBE");
                }
            }
            else
            {
                return RedirectToAction("PAYEPCFBE");
            }
        }

        public JsonResult GetPAYEPCFBEData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Pay As You Earn - PAYE Contribution - Formal Business Employee", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPAYEPCFBETaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPAYEPCFBEAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region DirectAssessment
        
        public ActionResult DirectAssessment()
        {
            return View();
        }

        
        public ActionResult DA()
        {
            return View();
        }

        public JsonResult GetDAData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Direct Assessment", IntSearchType = 1 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult DA_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("DA");
                }
            }
            else
            {
                return RedirectToAction("DA");
            }
        }

        public JsonResult GetDATaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult DA_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("DA");
                }
            }
            else
            {
                return RedirectToAction("DA");
            }
        }

        public JsonResult GetDAAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult DAFBSE()
        {
            return View();
        }

        public JsonResult GetDAFBSEData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Direct Assessment - Formal Business - Self Employed", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                dcResponse["draw"] = 0;
                dcResponse["recordsTotal"] = 0;
                dcResponse["recordsFiltered"] = 0;
                dcResponse["data"] = null;

                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult DAFBSE_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("DAFBSE");
                }
            }
            else
            {
                return RedirectToAction("DAFBSE");
            }
        }

        public JsonResult GetDAFBSETaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult DAFBSE_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("DAFBSE");
                }
            }
            else
            {
                return RedirectToAction("DAFBSE");
            }
        }

        public JsonResult GetDAFBSEAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult DAMEBO()
        {
            return View();
        }

        public JsonResult GetDAMEBOData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Direct Assessment for Multi Employees Business Owners", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult DAMEBO_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("DAMEBO");
                }
            }
            else
            {
                return RedirectToAction("DAMEBO");
            }
        }

        public JsonResult GetDAMEBOTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search
                    lstTaxPayerData = lstTaxPayerData.Where(x => x.TaxPayerRIN != null).ToList();
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult DAMEBO_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("DAMEBO");
                }
            }
            else
            {
                return RedirectToAction("DAMEBO");
            }
        }

        public JsonResult GetDAMEBOAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region WithHoldingTax
        
        public ActionResult WithHoldingTax()
        {
            return View();
        }

        
        public ActionResult WTC()
        {
            return View();
        }

        
        public ActionResult WTC_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("WTC");
                }
            }
            else
            {
                return RedirectToAction("WTC");
            }
        }

        
        public ActionResult WTC_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("DA");
                }
            }
            else
            {
                return RedirectToAction("DA");
            }
        }

        public JsonResult GetWTCData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Withholding Tax Collections", IntSearchType = 1 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWTCTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWTCAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult WTCCB()
        {
            return View();
        }

        
        public ActionResult WTCCB_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("WTCCB");
                }
            }
            else
            {
                return RedirectToAction("WTCCB");
            }
        }

        
        public ActionResult WTCCB_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("WTCCB");
                }
            }
            else
            {
                return RedirectToAction("WTCCB");
            }
        }

         public JsonResult GetWTCCBData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Withholding Tax Collections - Commercial Banks", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWTCCBTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWTCCBAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult WTCSL()
        {
            return View();
        }

        
        public ActionResult WTCSL_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("WTCSL");
                }
            }
            else
            {
                return RedirectToAction("WTCSL");
            }
        }

        
        public ActionResult WTCSL_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("WTCSL");
                }
            }
            else
            {
                return RedirectToAction("WTCSL");
            }
        }

        public JsonResult GetWTCSLData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Withholding Tax Collections - Savings & Loans", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWTCSLTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWTCSLAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Vehicle License Registration 
        
        public ActionResult VehicleLicenseRegistration()
        {
            return View();
        }

        
        public ActionResult VL()
        {
            return View();
        }

        
        public ActionResult VL_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("VL");
                }
            }
            else
            {
                return RedirectToAction("VL");
            }
        }

        
        public ActionResult VL_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("VL");
                }
            }
            else
            {
                return RedirectToAction("VL");
            }
        }

        public JsonResult GetVLData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Vehicle Licenses", IntSearchType = 1 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVLTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });
                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVLAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult VLC()
        {
            return View();
        }

        
        public ActionResult VLC_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("VLC");
                }
            }
            else
            {
                return RedirectToAction("VLC");
            }
        }

        
        public ActionResult VLC_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("VLC");
                }
            }
            else
            {
                return RedirectToAction("VLC");
            }
        }

        public JsonResult GetVLCData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Vehicle Licenses - Commercial", IntSearchType = 1 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVLCTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVLCAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult VLP()
        {
            return View();
        }

        
        public ActionResult VLP_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("VLP");
                }
            }
            else
            {
                return RedirectToAction("VLP");
            }
        }

        
        public ActionResult VLP_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("VLP");
                }
            }
            else
            {
                return RedirectToAction("VLP");
            }
        }

        public JsonResult GetVLPData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Vehicle Licenses - Private", IntSearchType = 1 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVLPTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetVLPAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Presumptive Taxes

        
        public ActionResult PresumptiveTaxes()
        {
            return View();
        }

        
        public ActionResult PT()
        {
            return View();
        }

        
        public ActionResult PT_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PT");
                }
            }
            else
            {
                return RedirectToAction("PT");
            }
        }

        
        public ActionResult PT_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("DA");
                }
            }
            else
            {
                return RedirectToAction("DA");
            }
        }

        public JsonResult GetPTData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Presumptive Taxes", IntSearchType = 1 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPTTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPTAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Business Premises
        
        public ActionResult BusinessPremises()
        {
            return View();
        }

        
        public ActionResult BP()
        {
            return View();
        }

        
        public ActionResult BP_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("BP");
                }
            }
            else
            {
                return RedirectToAction("BP");
            }
        }

        
        public ActionResult BP_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("BP");
                }
            }
            else
            {
                return RedirectToAction("BP");
            }
        }

        public JsonResult GetBPData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Business Premises", IntSearchType = 1 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBPTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBPAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Consumption Tax
        
        public ActionResult ConsumptionTax()
        {
            return View();
        }

        
        public ActionResult CTC()
        {
            return View();
        }

        
        public ActionResult CTC_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("CTC");
                }
            }
            else
            {
                return RedirectToAction("CTC");
            }
        }

        
        public ActionResult CTC_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("CTC");
                }
            }
            else
            {
                return RedirectToAction("CTC");
            }
        }

        public JsonResult GetCTCData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Consumption Tax Collections", IntSearchType = 1 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCTCTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCTCAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult CTCB()
        {
            return View();
        }

        
        public ActionResult CTCB_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("CTCB");
                }
            }
            else
            {
                return RedirectToAction("CTCB");
            }
        }

        
        public ActionResult CTCB_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("CTCB");
                }
            }
            else
            {
                return RedirectToAction("CTCB");
            }
        }

        public JsonResult GetCTCBData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Consumption Tax Collections - Bars", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCTCBTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCTCBAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult CTCR()
        {
            return View();
        }

        
        public ActionResult CTCR_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("CTCR");
                }
            }
            else
            {
                return RedirectToAction("CTCR");
            }
        }

        
        public ActionResult CTCR_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("CTCR");
                }
            }
            else
            {
                return RedirectToAction("CTCR");
            }
        }

        public JsonResult GetCTCRData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Consumption Tax Collections - Restaurants", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCTCRTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCTCRAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult CTCEC()
        {
            return View();
        }

        
        public ActionResult CTCEC_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("CTCEC");
                }
            }
            else
            {
                return RedirectToAction("CTCEC");
            }
        }

        
        public ActionResult CTCEC_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("CTCEC");
                }
            }
            else
            {
                return RedirectToAction("CTCEC");
            }
        }

        public JsonResult GetCTCECData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Consumption Tax Collections - Event Centres", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCTCECTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCTCECAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult CTCGH()
        {
            return View();
        }

        
        public ActionResult CTCGH_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("CTCGH");
                }
            }
            else
            {
                return RedirectToAction("CTCGH");
            }
        }

        
        public ActionResult CTCGH_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("CTCGH");
                }
            }
            else
            {
                return RedirectToAction("CTCGH");
            }
        }

        public JsonResult GetCTCGHData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Consumption Tax Collections - Guest Houses", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCTCGhTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCTCGHAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult CTCH()
        {
            return View();
        }

        
        public ActionResult CTCH_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("CTCH");
                }
            }
            else
            {
                return RedirectToAction("CTCH");
            }
        }

        
        public ActionResult CTCH_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("CTCCH");
                }
            }
            else
            {
                return RedirectToAction("CTCH");
            }
        }

        public JsonResult GetCTCHData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Consumption Tax Collections - Hotels", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCTCHTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCTCHAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult CTCM()
        {
            return View();
        }

        
        public ActionResult CTCM_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("CTCM");
                }
            }
            else
            {
                return RedirectToAction("CTCM");
            }
        }

        
        public ActionResult CTCM_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("CTCM");
                }
            }
            else
            {
                return RedirectToAction("CTCM");
            }
        }

        public JsonResult GetCTCMData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Consumption Tax Collections - Motels", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCTCMTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCTCMAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult CTCODT()
        {
            return View();
        }

        
        public ActionResult CTCODT_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("CTCODT");
                }
            }
            else
            {
                return RedirectToAction("CTCODT");
            }
        }

        
        public ActionResult CTCODT_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("CTCODT");
                }
            }
            else
            {
                return RedirectToAction("CTCODT");
            }
        }

        public JsonResult GetCTCODTData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Consumption Tax Collections - Online Drinks Trading", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCTCODTTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCTCODTAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Land Use Charge
        
        public ActionResult LandUseCharges()
        {
            return View();
        }
        
        public ActionResult LUC()
        {
            return View();
        }

        
        public ActionResult LUC_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("LUC");
                }
            }
            else
            {
                return RedirectToAction("LUC");
            }
        }

        
        public ActionResult LUC_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("LUC");
                }
            }
            else
            {
                return RedirectToAction("LUC");
            }
        }

        public JsonResult GetLUCData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Land Use Charge", IntSearchType = 1 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLUCTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLUCAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Pools Betting
        
        public ActionResult PoolsBetting()
        {
            return View();
        }

        
        public ActionResult PB()
        {
            return View();
        }

        
        public ActionResult PB_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PB");
                }
            }
            else
            {
                return RedirectToAction("PB");
            }
        }

        
        public ActionResult PB_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PB");
                }
            }
            else
            {
                return RedirectToAction("PB");
            }
        }

        public JsonResult GetPBData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Pools Betting", IntSearchType = 1 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPBTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPBAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult PBCO()
        {
            return View();
        }

        
        public ActionResult PBCO_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PBCO");
                }
            }
            else
            {
                return RedirectToAction("PBCO");
            }
        }

        
        public ActionResult PBCO_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PBCO");
                }
            }
            else
            {
                return RedirectToAction("PBCO");
            }
        }

        public JsonResult GetPBCOData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Pools Betting - Casino Operators", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPBCOTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPBCOAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult PBGMO()
        {
            return View();
        }

        
        public ActionResult PBGMO_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PBGMO");
                }
            }
            else
            {
                return RedirectToAction("PBGMO");
            }
        }

        
        public ActionResult PBGMO_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PBGMO");
                }
            }
            else
            {
                return RedirectToAction("PBGMO");
            }
        }

        public JsonResult GetPBGMOData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Pools Betting - Gaming Machine Operators", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPBGMOTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPBGMOAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult PBLO()
        {
            return View();
        }

        
        public ActionResult PBLO_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PBLO");
                }
            }
            else
            {
                return RedirectToAction("PBLO");
            }
        }

        
        public ActionResult PBLO_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PBLO");
                }
            }
            else
            {
                return RedirectToAction("PBLO");
            }
        }

        public JsonResult GetPBLOData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Pools Betting - Lottery Operators", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPBLOTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPBLOAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult PBLTA()
        {
            return View();
        }

        
        public ActionResult PBLTA_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PBLTA");
                }
            }
            else
            {
                return RedirectToAction("PBLTA");
            }
        }

        
        public ActionResult PBLTA_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PBLTA");
                }
            }
            else
            {
                return RedirectToAction("PBLTA");
            }
        }

        public JsonResult GetPBLTAData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Pools Betting - Lotto Agents", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPBLTATaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPBLTAAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult PBLA()
        {
            return View();
        }

        
        public ActionResult PBLA_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PBLA");
                }
            }
            else
            {
                return RedirectToAction("PBLA");
            }
        }

        
        public ActionResult PBLA_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PBLA");
                }
            }
            else
            {
                return RedirectToAction("PBLA");
            }
        }

        public JsonResult GetPBLAData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Pools Betting - Lottery Agents", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPBLATaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPBLAAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult PBLTO()
        {
            return View();
        }

        
        public ActionResult PBLTO_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PBLTO");
                }
            }
            else
            {
                return RedirectToAction("PBLTO");
            }
        }

        
        public ActionResult PBLTO_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PBLTO");
                }
            }
            else
            {
                return RedirectToAction("PBLTO");
            }
        }

        public JsonResult GetPBLTOData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Pools Betting - Lotto Operators", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPBLTOTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPBLTOAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult PBOSBA()
        {
            return View();
        }

        
        public ActionResult PBOSBA_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PBOSBA");
                }
            }
            else
            {
                return RedirectToAction("PBOSBA");
            }
        }

        
        public ActionResult PBOSBA_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PBOSBA");
                }
            }
            else
            {
                return RedirectToAction("PBOSBA");
            }
        }

        public JsonResult GetPBOSBAData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Pools Betting - Online Sports Betting Agents", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPBOSBATaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPBOSBAAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult PBOSBO()
        {
            return View();
        }

        
        public ActionResult PBOSBO_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PBOSBO");
                }
            }
            else
            {
                return RedirectToAction("PBOSBO");
            }
        }

        
        public ActionResult PBOSBO_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PBOSBO");
                }
            }
            else
            {
                return RedirectToAction("PBOSBO");
            }
        }

        public JsonResult GetPBOSBOData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Pools Betting - Online Sports Betting Operators", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPBOSBOTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPBOSBOAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult PBPA()
        {
            return View();
        }

        
        public ActionResult PBPA_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PBPA");
                }
            }
            else
            {
                return RedirectToAction("PBPA");
            }
        }

        
        public ActionResult PBPA_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PBPA");
                }
            }
            else
            {
                return RedirectToAction("PBPA");
            }
        }

        public JsonResult GetPBPAData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Pools Betting - Pools Agents", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPBPATaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPBPAAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult PBPP()
        {
            return View();
        }

        
        public ActionResult PBPP_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PBPP");
                }
            }
            else
            {
                return RedirectToAction("PBPP");
            }
        }

        
        public ActionResult PBPP_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("PBPP");
                }
            }
            else
            {
                return RedirectToAction("PBPP");
            }
        }

        public JsonResult GetPBPPData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Pools Betting - Pools Promoters", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPBPPTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPBPPAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Waste Management
        
        public ActionResult WasteManagement()
        {
            return View();
        }

        
        public ActionResult CWM()
        {
            return View();
        }

        
        public ActionResult CWM_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("CWM");
                }
            }
            else
            {
                return RedirectToAction("CWM");
            }
        }

        
        public ActionResult CWM_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("CWM");
                }
            }
            else
            {
                return RedirectToAction("CWM");
            }
        }

        public JsonResult GetCWMData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Waste Management Fees", IntSearchType = 1 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCWMTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCWMAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult WMC()
        {
            return View();
        }

        
        public ActionResult WMC_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("WMC");
                }
            }
            else
            {
                return RedirectToAction("WMC");
            }
        }

        
        public ActionResult WMC_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("WMC");
                }
            }
            else
            {
                return RedirectToAction("WMC");
            }
        }

        public JsonResult GetWMCData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Waste Management Fees - Commercial", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWMCTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWMCAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult WMCR()
        {
            return View();
        }

        
        public ActionResult WMCR_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("WMCR");
                }
            }
            else
            {
                return RedirectToAction("WMCR");
            }
        }

        
        public ActionResult WMCR_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("WMCR");
                }
            }
            else
            {
                return RedirectToAction("WMCR");
            }
        }

        public JsonResult GetWMCRData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Waste Management Fees - Commercial Residential", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWMCRTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWMCRAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult WMR()
        {
            return View();
        }

        
        public ActionResult WMR_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("WMR");
                }
            }
            else
            {
                return RedirectToAction("WMR");
            }
        }

        
        public ActionResult WMR_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("WMR");
                }
            }
            else
            {
                return RedirectToAction("WMR");
            }
        }

        public JsonResult GetWMRData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Waste Management Fees - Religious", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWMRTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWMRAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult WMRe()
        {
            return View();
        }

        
        public ActionResult WMRe_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("WMRe");
                }
            }
            else
            {
                return RedirectToAction("WMRe");
            }
        }

        
        public ActionResult WMRe_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("WMRe");
                }
            }
            else
            {
                return RedirectToAction("WMRe");
            }
        }

        public JsonResult GetWMReData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Waste Management Fees - Residential", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWMReTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWMReAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult WMH()
        {
            return View();
        }

        
        public ActionResult WMH_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("WMH");
                }
            }
            else
            {
                return RedirectToAction("WMH");
            }
        }

        
        public ActionResult WMH_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("WMH");
                }
            }
            else
            {
                return RedirectToAction("WMH");
            }
        }

        public JsonResult GetWMHData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Waste Management Fees - Hotel", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWMHTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWMHAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult WMS()
        {
            return View();
        }

        
        public ActionResult WMS_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("WMS");
                }
            }
            else
            {
                return RedirectToAction("WMS");
            }
        }

        
        public ActionResult WMS_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("WMS");
                }
            }
            else
            {
                return RedirectToAction("WMS");
            }
        }

        public JsonResult GetWMSData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Waste Management Fees - School", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWMSTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWMSAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult WMEC()
        {
            return View();
        }

        
        public ActionResult WMEC_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("WMEC");
                }
            }
            else
            {
                return RedirectToAction("WMEC");
            }
        }

        
        public ActionResult WMEC_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("WMEC");
                }
            }
            else
            {
                return RedirectToAction("WMEC");
            }
        }

        public JsonResult GetWMECData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Waste Management Fees - Event Centre", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWMECTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWMECAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        
        public ActionResult WMHO()
        {
            return View();
        }

        
        public ActionResult WMHO_TaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("WMHO");
                }
            }
            else
            {
                return RedirectToAction("WMHO");
            }
        }

        
        public ActionResult WMHO_Asset(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                ViewBag.ProfileID = id;

                usp_GetProfileList_Result mObjProfileData = new BLProfile().BL_GetProfileDetails(new BOL.Profile() { ProfileID = id.GetValueOrDefault(), IntStatus = 2 });

                if (mObjProfileData != null)
                {
                    return View(mObjProfileData);
                }
                else
                {
                    return RedirectToAction("WMH");
                }
            }
            else
            {
                return RedirectToAction("WMH");
            }
        }

        public JsonResult GetWMHOData()
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
                IList<usp_GetProfileData_Result> lstProfileData = new BLProfile().BL_GetProfileData(new Profile() { ProfileDescription = "Waste Management Fees - Hospital", IntSearchType = 2 });

                // Total record count.   
                int totalRecords = lstProfileData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstProfileData = lstProfileData.Where(p => p.ProfileReferenceNo.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.ProfileDescription.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.TaxPayerCount != null && p.TaxPayerCount.ToString().ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstProfileData = this.ProfileData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstProfileData);

                // Filter record count.   
                int recFilter = lstProfileData.Count;

                // Apply pagination.   
                lstProfileData = lstProfileData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstProfileData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWMHOTaxPayerData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileTaxPayerData_Result> lstTaxPayerData = new BLProfile().BL_GetProfileTaxPayerData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstTaxPayerData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstTaxPayerData = lstTaxPayerData.Where(p => p.TaxPayerTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.MobileNumber.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        (p.ContactAddress != null && p.ContactAddress.ToLower().Contains(mStrSearchFilter.ToLower()))).ToList();
                }

                // Sorting.   
                lstTaxPayerData = this.ProfileTaxPayerData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstTaxPayerData);

                // Filter record count.   
                int recFilter = lstTaxPayerData.Count;

                // Apply pagination.   
                lstTaxPayerData = lstTaxPayerData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstTaxPayerData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetWMHOAssetData()
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
                int mIntProfileID = TrynParse.parseInt(Request.QueryString["id"]);

                // Loading. 
                IList<usp_GetProfileAssetData_Result> lstAssetData = new BLProfile().BL_GetProfileAssetData(new Profile() { ProfileID = mIntProfileID });

                // Total record count.   
                int totalRecords = lstAssetData.Count;

                // Filtering.   
                if (!string.IsNullOrEmpty(mStrSearchFilter) && !string.IsNullOrWhiteSpace(mStrSearchFilter))
                {
                    // Apply search   
                    lstAssetData = lstAssetData.Where(p => p.AssetTypeName.ToString().ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetName.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.AssetRIN.ToLower().Contains(mStrSearchFilter.ToLower()) ||
                        p.TaxPayerCount.GetValueOrDefault().ToString().ToLower().Contains(mStrSearchFilter.ToLower())).ToList();
                }

                // Sorting.   
                lstAssetData = this.ProfileAssetData_SortByColumnWithOrder(mstrOrderBy, mStrOrderByDir, lstAssetData);

                // Filter record count.   
                int recFilter = lstAssetData.Count;

                // Apply pagination.   
                lstAssetData = lstAssetData.Skip(mIntStartRowNumber).Take(mIntPageNumber).ToList();

                dcResponse["draw"] = TrynParse.parseInt(mStrDraw);
                dcResponse["recordsTotal"] = totalRecords;
                dcResponse["recordsFiltered"] = recFilter;
                dcResponse["data"] = lstAssetData;

            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }
        #endregion


        private IList<usp_GetProfileData_Result> ProfileData_SortByColumnWithOrder(string order, string orderDir, IList<usp_GetProfileData_Result> data)
        {
            // Initialization.   
            IList<usp_GetProfileData_Result> lst = new List<usp_GetProfileData_Result>();
            try
            {
                // Sorting   
                switch (order)
                {
                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ProfileReferenceNo).ToList() : data.OrderBy(p => p.ProfileReferenceNo).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ProfileDescription).ToList() : data.OrderBy(p => p.ProfileDescription).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TaxPayerCount).ToList() : data.OrderBy(p => p.TaxPayerCount).ToList();
                        break;
                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ProfileReferenceNo).ToList() : data.OrderBy(p => p.ProfileReferenceNo).ToList();
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

        private IList<usp_GetProfileTaxPayerData_Result> ProfileTaxPayerData_SortByColumnWithOrder(string order, string orderDir, IList<usp_GetProfileTaxPayerData_Result> data)
        {
            // Initialization.   
            IList<usp_GetProfileTaxPayerData_Result> lst = new List<usp_GetProfileTaxPayerData_Result>();
            try
            {
                // Sorting   
                switch (order)
                {
                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TaxPayerTypeName).ToList() : data.OrderBy(p => p.TaxPayerTypeName).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TaxPayerName).ToList() : data.OrderBy(p => p.TaxPayerName).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TaxPayerRIN).ToList() : data.OrderBy(p => p.TaxPayerRIN).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.MobileNumber).ToList() : data.OrderBy(p => p.MobileNumber).ToList();
                        break;
                    case "4":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.ContactAddress).ToList() : data.OrderBy(p => p.ContactAddress).ToList();
                        break;
                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TaxPayerTypeName).ToList() : data.OrderBy(p => p.TaxPayerTypeName).ToList();
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

        private IList<usp_GetProfileAssetData_Result> ProfileAssetData_SortByColumnWithOrder(string order, string orderDir, IList<usp_GetProfileAssetData_Result> data)
        {
            // Initialization.   
            IList<usp_GetProfileAssetData_Result> lst = new List<usp_GetProfileAssetData_Result>();
            try
            {
                // Sorting   
                switch (order)
                {
                    case "0":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AssetTypeName).ToList() : data.OrderBy(p => p.AssetTypeName).ToList();
                        break;
                    case "1":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AssetName).ToList() : data.OrderBy(p => p.AssetName).ToList();
                        break;
                    case "2":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AssetRIN).ToList() : data.OrderBy(p => p.AssetRIN).ToList();
                        break;
                    case "3":
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.TaxPayerCount).ToList() : data.OrderBy(p => p.TaxPayerCount).ToList();
                        break;
                    default:
                        // Setting.   
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.AssetTypeName).ToList() : data.OrderBy(p => p.AssetTypeName).ToList();
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
    }
}