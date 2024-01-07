using EIRS.BLL;
using EIRS.BOL;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using EIRS.Models;
using Elmah;
using EIRS.Common;
using Vereyon.Web;
using System;
using System.Transactions;
using System.Reflection;
using System.Text;
using static EIRS.Web.Controllers.Filters;
using EIRS.Web.Utility;

namespace EIRS.Web.Controllers
{
    [SessionTimeout]
    public class ProfileBusinessController : BaseController
    {
        EIRSEntities _db;

        [HttpGet]
        public ActionResult List()
        {
            return View();
        }


        [HttpPost]
        public JsonResult LoadData()
        {
            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var vFilter = Request.Form.GetValues("search[value]").FirstOrDefault();

            StringBuilder sbWhereCondition = new StringBuilder();

            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( ISNULL(BusinessRIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BusinessName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BusinessSubSectorName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BusinessAddress,'') LIKE @MainFilter )");
            }

            Business mObjBusiness = new Business()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };


            IDictionary<string, object> dcData = new BLBusiness().BL_SearchBusinessForSideMenu(mObjBusiness);
            IList<usp_SearchBusinessForSideMenu_Result> lstBusiness = (IList<usp_SearchBusinessForSideMenu_Result>)dcData["BusinessList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstBusiness
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ListWithExport()
        {
            return View();
        }


        [HttpPost]
        public JsonResult LoadExportData()
        {
            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var vFilter = Request.Form.GetValues("search[value]").FirstOrDefault();

            StringBuilder sbWhereCondition = new StringBuilder();

            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( ISNULL(BusinessRIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BusinessName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BusinessSubSectorName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BusinessAddress,'') LIKE @MainFilter )");
            }

            Business mObjBusiness = new Business()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };


            IDictionary<string, object> dcData = new BLBusiness().BL_SearchBusinessForSideMenu(mObjBusiness);
            IList<usp_SearchBusinessForSideMenu_Result> lstBusiness = (IList<usp_SearchBusinessForSideMenu_Result>)dcData["BusinessList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstBusiness
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ExportData()
        {
            IList<usp_GetBusinessListNewTy_Result> lstBusinessData = new BLBusiness().BL_GetBusinessList(new Business() { intStatus = 2 });
            string[] strColumns = new string[] {
                                                "BusinessRIN",
                                                "BusinessName",
                                                "BusinessTypeName",
                                                "LGAName",
                                                "BusinessCategoryName",
                                                "BusinessSectorName",
                                                "BusinessSubSectorName",
                                                "BusinessStructureName",
                                                "BusinessOperationName",
                                                "SizeName",
                                                "ContactName",
                                                "BusinessNumber",
                                                "BusinessAddress",
                                                "ActiveText",

                                                     };

            return ExportToExcel(lstBusinessData, this.RouteData, strColumns, "Business");
        }


        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Search(FormCollection pObjCollection)
        {
            string mStrBusinessName = pObjCollection.Get("txtBusinessName");
            string mStrBusinessAddress = pObjCollection.Get("txtBusinessAddress");
            string mStrRIN = pObjCollection.Get("txtRIN");

            Business mObjBusiness = new Business()
            {
                BusinessRIN = mStrRIN,
                BusinessName = mStrBusinessName,
                BusinessAddress = mStrBusinessAddress,
                intStatus = 1
            };

            IList<usp_GetBusinessListNewTy_Result> lstBusiness = new BLBusiness().BL_GetBusinessList(mObjBusiness);
            return PartialView("_BindTable", lstBusiness.Take(5).ToList());
        }

        public void UI_FillDropDown(BusinessViewModel pObjBusinessViewModel = null)
        {
            if (pObjBusinessViewModel != null)
                pObjBusinessViewModel.AssetTypeID = (int)EnumList.AssetTypes.Business;
            else if (pObjBusinessViewModel == null)
                pObjBusinessViewModel = new BusinessViewModel();

            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjBusinessViewModel.AssetTypeID.ToString() }, (int)EnumList.AssetTypes.Business);
            UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1, IncludeBusinessTypeIds = pObjBusinessViewModel.BusinessTypeID.ToString() });
            UI_FillZoneDropDown(0);
            UI_FillTaxOfficeDropDown(0);
            UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = pObjBusinessViewModel.LGAID.ToString() });
            UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = pObjBusinessViewModel.LGAID.ToString() });
            UI_FillBusinessCategoryDropDown(new Business_Category() { intStatus = 1, IncludeBusinessCategoryIds = pObjBusinessViewModel.BusinessCategoryID.ToString(), BusinessTypeID = pObjBusinessViewModel.BusinessTypeID });
            UI_FillBusinessSectorDropDown(new Business_Sector() { intStatus = 1, IncludeBusinessSectorIds = pObjBusinessViewModel.BusinessSectorID.ToString(), BusinessTypeID = pObjBusinessViewModel.BusinessTypeID, BusinessCategoryID = pObjBusinessViewModel.BusinessCategoryID });
            UI_FillBusinessSubSectorDropDown(new Business_SubSector() { intStatus = 1, IncludeBusinessSubSectorIds = pObjBusinessViewModel.BusinessSubSectorID.ToString(), BusinessSectorID = pObjBusinessViewModel.BusinessSectorID });
            UI_FillBusinessStructureDropDown(new Business_Structure() { intStatus = 1, IncludeBusinessStructureIds = pObjBusinessViewModel.BusinessStructureID.ToString(), BusinessTypeID = pObjBusinessViewModel.BusinessTypeID });
            UI_FillBusinessOperationDropDown(new Business_Operation() { intStatus = 1, IncludeBusinessOperationIds = pObjBusinessViewModel.BusinessOperationID.ToString(), BusinessTypeID = pObjBusinessViewModel.BusinessTypeID });
            UI_FillSizeDropDown(new Size() { intStatus = 1, IncludeSizeIds = pObjBusinessViewModel.SizeID.ToString() });
        }


        public ActionResult Add()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]

        [ValidateAntiForgeryToken()]
        public ActionResult Add(BusinessViewModel pObjBusinessModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjBusinessModel);
                return View(pObjBusinessModel);
            }
            else
            {
                Business mObjBusiness = new Business()
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
                    ZoneId = pObjBusinessModel.ZoneId,
                    TaxOfficeID = pObjBusinessModel.TaxOfficeId,
                    SizeID = pObjBusinessModel.SizeID,
                    ContactName = pObjBusinessModel.ContactName,
                    BusinessAddress = pObjBusinessModel.BusinessAddress,
                    BusinessNumber = pObjBusinessModel.BusinessNumber,
                    Active = true,
                    CreatedBy = SessionManager.UserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Business> mObjResponse = new BLBusiness().BL_InsertUpdateBusiness(mObjBusiness);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("Details", "ProfileBusiness", new { id = mObjResponse.AdditionalData.BusinessID, name = mObjResponse.AdditionalData.BusinessName.ToSeoUrl() });
                    }
                    else
                    {
                        UI_FillDropDown(pObjBusinessModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBusinessModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjBusinessModel);
                    ViewBag.Message = "Error occurred while saving business";
                    return View(pObjBusinessModel);
                }
            }
        }


        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Business mObjBusiness = new Business()
                {
                    BusinessID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBusinessListNewTy_Result mObjBusinessData = new BLBusiness().BL_GetBusinessDetails(mObjBusiness);

                if (mObjBusinessData != null)
                {
                    BusinessViewModel mObjBusinessModelView = new BusinessViewModel()
                    {
                        BusinessID = mObjBusinessData.BusinessID.GetValueOrDefault(),
                        BusinessRIN = mObjBusinessData.BusinessRIN,
                        AssetTypeID = mObjBusinessData.AssetTypeID.GetValueOrDefault(),
                        BusinessTypeID = mObjBusinessData.BusinessTypeID.GetValueOrDefault(),
                        BusinessName = mObjBusinessData.BusinessName,
                        LGAID = mObjBusinessData.LGAID.GetValueOrDefault(),
                        BusinessCategoryID = mObjBusinessData.BusinessCategoryID.GetValueOrDefault(),
                        BusinessSectorID = mObjBusinessData.BusinessSectorID.GetValueOrDefault(),
                        BusinessSubSectorID = mObjBusinessData.BusinessSubSectorID.GetValueOrDefault(),
                        BusinessStructureID = mObjBusinessData.BusinessStructureID.GetValueOrDefault(),
                        BusinessOperationID = mObjBusinessData.BusinessOperationID.GetValueOrDefault(),
                        SizeID = mObjBusinessData.SizeID.GetValueOrDefault(),
                        Active = mObjBusinessData.Active.GetValueOrDefault(),
                        ContactName = mObjBusinessData.ContactName,
                        BusinessAddress = mObjBusinessData.BusinessAddress,
                        BusinessNumber = mObjBusinessData.BusinessNumber,
                    };

                    UI_FillDropDown(mObjBusinessModelView);
                    return View(mObjBusinessModelView);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileBusiness");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileBusiness");
            }
        }

        [HttpPost()]

        [ValidateAntiForgeryToken()]
        public ActionResult Edit(BusinessViewModel pObjBusinessModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjBusinessModel);
                return View(pObjBusinessModel);
            }
            else
            {
                Business mObjBusiness = new Business()
                {
                    BusinessID = pObjBusinessModel.BusinessID,
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
                    ContactName = pObjBusinessModel.ContactName,
                    BusinessAddress = pObjBusinessModel.BusinessAddress,
                    BusinessNumber = pObjBusinessModel.BusinessNumber,
                    Active = true,
                    ModifiedBy = SessionManager.UserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Business> mObjResponse = new BLBusiness().BL_InsertUpdateBusiness(mObjBusiness);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("Details", "ProfileBusiness", new { id = mObjResponse.AdditionalData.BusinessID, name = mObjResponse.AdditionalData.BusinessRIN.ToSeoUrl() });
                    }
                    else
                    {
                        UI_FillDropDown(pObjBusinessModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjBusinessModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjBusinessModel);
                    ViewBag.Message = "Error occurred while saving business";
                    return View(pObjBusinessModel);
                }
            }
        }


        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Business mObjBusiness = new Business()
                {
                    BusinessID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBusinessListNewTy_Result mObjBusinessData = new BLBusiness().BL_GetBusinessDetails(mObjBusiness);

                if (mObjBusinessData != null)
                {
                    BusinessViewModel mObjBusinessModelView = new BusinessViewModel()
                    {
                        BusinessID = mObjBusinessData.BusinessID.GetValueOrDefault(),
                        BusinessRIN = mObjBusinessData.BusinessRIN,
                        AssetTypeName = mObjBusinessData.AssetTypeName,
                        BusinessTypeName = mObjBusinessData.BusinessTypeName,
                        BusinessName = mObjBusinessData.BusinessName,
                        LGAName = mObjBusinessData.LGAName,
                        BusinessCategoryName = mObjBusinessData.BusinessCategoryName,
                        BusinessSectorName = mObjBusinessData.BusinessSectorName,
                        BusinessSubSectorName = mObjBusinessData.BusinessSubSectorName,
                        BusinessStructureName = mObjBusinessData.BusinessStructureName,
                        BusinessOperationName = mObjBusinessData.BusinessOperationName,
                        SizeName = mObjBusinessData.SizeName,
                        ContactName = mObjBusinessData.ContactName,
                        BusinessAddress = mObjBusinessData.BusinessAddress,
                        BusinessNumber = mObjBusinessData.BusinessNumber,
                        ActiveText = mObjBusinessData.ActiveText
                    };

                    MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                    {
                        AssetID = id.GetValueOrDefault(),
                        AssetTypeID = (int)EnumList.AssetTypes.Business
                    };

                    IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                    ViewBag.AssetList = lstTaxPayerAsset;

                    return View(mObjBusinessModelView);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileBusiness");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileBusiness");
            }
        }


        public ActionResult SearchIndividual(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Business mObjBusiness = new Business()
                {
                    BusinessID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBusinessListNewTy_Result mObjBusinessData = new BLBusiness().BL_GetBusinessDetails(mObjBusiness);

                if (mObjBusinessData != null)
                {
                    BusinessViewModel mObjBusinessModelView = new BusinessViewModel()
                    {
                        BusinessID = mObjBusinessData.BusinessID.GetValueOrDefault(),
                        BusinessRIN = mObjBusinessData.BusinessRIN,
                        AssetTypeName = mObjBusinessData.AssetTypeName,
                        BusinessTypeName = mObjBusinessData.BusinessTypeName,
                        BusinessName = mObjBusinessData.BusinessName,
                        LGAName = mObjBusinessData.LGAName,
                        BusinessCategoryName = mObjBusinessData.BusinessCategoryName,
                        BusinessSectorName = mObjBusinessData.BusinessSectorName,
                        BusinessSubSectorName = mObjBusinessData.BusinessSubSectorName,
                        BusinessStructureName = mObjBusinessData.BusinessStructureName,
                        BusinessOperationName = mObjBusinessData.BusinessOperationName,
                        SizeName = mObjBusinessData.SizeName,
                        ContactName = mObjBusinessData.ContactName,
                        BusinessAddress = mObjBusinessData.BusinessAddress,
                        BusinessNumber = mObjBusinessData.BusinessNumber,
                        ActiveText = mObjBusinessData.ActiveText
                    };

                    return View(mObjBusinessModelView);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileBusiness");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileBusiness");
            }
        }

        [HttpPost]

        public ActionResult SearchIndividual(FormCollection pObjCollection)
        {
            string mStrName = pObjCollection.Get("txtName");
            string mStrMobileNumber = pObjCollection.Get("txtMobileNumber");
            string mStrRIN = pObjCollection.Get("txtRIN");

            Individual mObjIndividual = new Individual()
            {
                IndividualName = mStrName,
                MobileNumber1 = mStrMobileNumber,
                IndividualRIN = mStrRIN,
                intStatus = 1
            };

            IList<usp_GetIndividualList_Result> lstIndividual = new BLIndividual().BL_GetIndividualList(mObjIndividual);
            return PartialView("_BindIndividualTable_SingleSelect", lstIndividual.Take(5).ToList());
        }


        public ActionResult SearchCorporate(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Business mObjBusiness = new Business()
                {
                    BusinessID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBusinessListNewTy_Result mObjBusinessData = new BLBusiness().BL_GetBusinessDetails(mObjBusiness);

                if (mObjBusinessData != null)
                {
                    BusinessViewModel mObjBusinessModelView = new BusinessViewModel()
                    {
                        BusinessID = mObjBusinessData.BusinessID.GetValueOrDefault(),
                        BusinessRIN = mObjBusinessData.BusinessRIN,
                        AssetTypeName = mObjBusinessData.AssetTypeName,
                        BusinessTypeName = mObjBusinessData.BusinessTypeName,
                        BusinessName = mObjBusinessData.BusinessName,
                        LGAName = mObjBusinessData.LGAName,
                        BusinessCategoryName = mObjBusinessData.BusinessCategoryName,
                        BusinessSectorName = mObjBusinessData.BusinessSectorName,
                        BusinessSubSectorName = mObjBusinessData.BusinessSubSectorName,
                        BusinessStructureName = mObjBusinessData.BusinessStructureName,
                        BusinessOperationName = mObjBusinessData.BusinessOperationName,
                        SizeName = mObjBusinessData.SizeName,
                        ContactName = mObjBusinessData.ContactName,
                        BusinessAddress = mObjBusinessData.BusinessAddress,
                        BusinessNumber = mObjBusinessData.BusinessNumber,
                        ActiveText = mObjBusinessData.ActiveText
                    };

                    return View(mObjBusinessModelView);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileBusiness");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileBusiness");
            }
        }

        [HttpPost]

        public ActionResult SearchCorporate(FormCollection pObjCollection)
        {
            string mStrCompanyName = pObjCollection.Get("txtCompanyName");
            string mStrMobileNumber = pObjCollection.Get("txtMobileNumber");
            string mStrRIN = pObjCollection.Get("txtRIN");

            Company mObjCompany = new Company()
            {
                CompanyName = mStrCompanyName,
                MobileNumber1 = mStrMobileNumber,
                CompanyRIN = mStrRIN,
                intStatus = 1
            };

            IList<usp_GetCompanyList_Result> lstCompany = new BLCompany().BL_GetCompanyList(mObjCompany);
            return PartialView("_BindCompanyTable_SingleSelect", lstCompany.Take(5).ToList());
        }


        public ActionResult SearchGovernment(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Business mObjBusiness = new Business()
                {
                    BusinessID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBusinessListNewTy_Result mObjBusinessData = new BLBusiness().BL_GetBusinessDetails(mObjBusiness);

                if (mObjBusinessData != null)
                {
                    BusinessViewModel mObjBusinessModelView = new BusinessViewModel()
                    {
                        BusinessID = mObjBusinessData.BusinessID.GetValueOrDefault(),
                        BusinessRIN = mObjBusinessData.BusinessRIN,
                        AssetTypeName = mObjBusinessData.AssetTypeName,
                        BusinessTypeName = mObjBusinessData.BusinessTypeName,
                        BusinessName = mObjBusinessData.BusinessName,
                        LGAName = mObjBusinessData.LGAName,
                        BusinessCategoryName = mObjBusinessData.BusinessCategoryName,
                        BusinessSectorName = mObjBusinessData.BusinessSectorName,
                        BusinessSubSectorName = mObjBusinessData.BusinessSubSectorName,
                        BusinessStructureName = mObjBusinessData.BusinessStructureName,
                        BusinessOperationName = mObjBusinessData.BusinessOperationName,
                        SizeName = mObjBusinessData.SizeName,
                        ContactName = mObjBusinessData.ContactName,
                        BusinessAddress = mObjBusinessData.BusinessAddress,
                        BusinessNumber = mObjBusinessData.BusinessNumber,
                        ActiveText = mObjBusinessData.ActiveText
                    };

                    return View(mObjBusinessModelView);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileBusiness");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileBusiness");
            }
        }

        [HttpPost]

        public ActionResult SearchGovernment(FormCollection pObjCollection)
        {
            string mStrGovernmentName = pObjCollection.Get("txtGovernmentName");
            string mStrMobileNumber = pObjCollection.Get("txtMobileNumber");
            string mStrRIN = pObjCollection.Get("txtRIN");

            Government mObjGovernment = new Government()
            {
                GovernmentName = mStrGovernmentName,
                ContactNumber = mStrMobileNumber,
                GovernmentRIN = mStrRIN,
                intStatus = 1
            };

            IList<usp_GetGovernmentList_Result> lstGovernment = new BLGovernment().BL_GetGovernmentList(mObjGovernment);
            return PartialView("_BindGovernmentTable_SingleSelect", lstGovernment.Take(5).ToList());
        }


        public ActionResult SearchSpecial(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Business mObjBusiness = new Business()
                {
                    BusinessID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBusinessListNewTy_Result mObjBusinessData = new BLBusiness().BL_GetBusinessDetails(mObjBusiness);

                if (mObjBusinessData != null)
                {
                    BusinessViewModel mObjBusinessModelView = new BusinessViewModel()
                    {
                        BusinessID = mObjBusinessData.BusinessID.GetValueOrDefault(),
                        BusinessRIN = mObjBusinessData.BusinessRIN,
                        AssetTypeName = mObjBusinessData.AssetTypeName,
                        BusinessTypeName = mObjBusinessData.BusinessTypeName,
                        BusinessName = mObjBusinessData.BusinessName,
                        LGAName = mObjBusinessData.LGAName,
                        BusinessCategoryName = mObjBusinessData.BusinessCategoryName,
                        BusinessSectorName = mObjBusinessData.BusinessSectorName,
                        BusinessSubSectorName = mObjBusinessData.BusinessSubSectorName,
                        BusinessStructureName = mObjBusinessData.BusinessStructureName,
                        BusinessOperationName = mObjBusinessData.BusinessOperationName,
                        SizeName = mObjBusinessData.SizeName,
                        ContactName = mObjBusinessData.ContactName,
                        BusinessAddress = mObjBusinessData.BusinessAddress,
                        BusinessNumber = mObjBusinessData.BusinessNumber,
                        ActiveText = mObjBusinessData.ActiveText
                    };

                    return View(mObjBusinessModelView);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileBusiness");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileBusiness");
            }
        }

        [HttpPost]

        public ActionResult SearchSpecial(FormCollection pObjCollection)
        {
            string mStrSpecialName = pObjCollection.Get("txtSpecialName");
            string mStrMobileNumber = pObjCollection.Get("txtMobileNumber");
            string mStrRIN = pObjCollection.Get("txtRIN");

            Special mObjSpecial = new Special()
            {
                SpecialTaxPayerName = mStrSpecialName,
                ContactNumber = mStrMobileNumber,
                SpecialRIN = mStrRIN,
                intStatus = 1
            };

            IList<usp_GetSpecialList_Result> lstSpecial = new BLSpecial().BL_GetSpecialList(mObjSpecial);
            return PartialView("_BindSpecialTable_SingleSelect", lstSpecial.Take(5).ToList());
        }

        public void UI_FillIndividualDropDown(IndividualViewModel pObjIndividualViewModel = null)
        {
            if (pObjIndividualViewModel != null)
                pObjIndividualViewModel.TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual;
            else if (pObjIndividualViewModel == null)
                pObjIndividualViewModel = new IndividualViewModel();

            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual, AssetTypeID = (int)EnumList.AssetTypes.Business });
            UI_FillGender();
            UI_FillTitleDropDown(new Title() { intStatus = 1, IncludeTitleIds = pObjIndividualViewModel.TitleID.ToString(), GenderID = pObjIndividualViewModel.GenderID });
            UI_FillTaxOfficeDropDown(new Tax_Offices() { intStatus = 1, IncludeTaxOfficeIds = pObjIndividualViewModel.TaxOfficeID.ToString() });
            UI_FillMaritalStatus();
            UI_FillNationality();
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1, IncludeTaxPayerTypeIds = pObjIndividualViewModel.TaxPayerTypeID.ToString() }, (int)EnumList.TaxPayerType.Individual);
            UI_FillEconomicActivitiesDropDown(new Economic_Activities() { intStatus = 1, IncludeEconomicActivitiesIds = pObjIndividualViewModel.EconomicActivitiesID.ToString(), TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual });
            UI_FillNotificationMethodDropDown(new Notification_Method() { intStatus = 1, IncludeNotificationMethodIds = pObjIndividualViewModel.NotificationMethodID.ToString() });
        }


        public ActionResult AddIndividual(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Business mObjBusiness = new Business()
                {
                    BusinessID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBusinessListNewTy_Result mObjBusinessData = new BLBusiness().BL_GetBusinessDetails(mObjBusiness);

                if (mObjBusinessData != null)
                {
                    TPIndividualViewModel mObjIndividualModel = new TPIndividualViewModel()
                    {
                        AssetID = mObjBusinessData.BusinessID.GetValueOrDefault(),
                        AssetName = mObjBusinessData.BusinessName,
                        AssetRIN = mObjBusinessData.BusinessRIN,
                        AssetLGAName = mObjBusinessData.LGAName,
                        AssetTypeID = (int)EnumList.AssetTypes.Business,
                        AssetTypeName = mObjBusinessData.AssetTypeName,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                    };

                    UI_FillIndividualDropDown();
                    return View(mObjIndividualModel);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileBusiness");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileBusiness");
            }
        }

        [HttpPost]

        [ValidateAntiForgeryToken()]
        public ActionResult AddIndividual(TPIndividualViewModel pObjIndividualModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillIndividualDropDown(pObjIndividualModel);
                return View(pObjIndividualModel);
            }
            else
            {
                using (TransactionScope mObjScope = new TransactionScope())
                {
                    try
                    {
                        Individual mObjIndividual = new Individual()
                        {
                            IndividualID = 0,
                            GenderID = pObjIndividualModel.GenderID,
                            TitleID = pObjIndividualModel.TitleID,
                            FirstName = pObjIndividualModel.FirstName,
                            LastName = pObjIndividualModel.LastName,
                            MiddleName = pObjIndividualModel.MiddleName,
                            DOB = TrynParse.parseNullableDate(pObjIndividualModel.DOB),
                            TIN = pObjIndividualModel.TIN,
                            MobileNumber1 = pObjIndividualModel.MobileNumber1,
                            MobileNumber2 = pObjIndividualModel.MobileNumber2,
                            EmailAddress1 = pObjIndividualModel.EmailAddress1,
                            EmailAddress2 = pObjIndividualModel.EmailAddress2,
                            BiometricDetails = pObjIndividualModel.BiometricDetails,
                            TaxOfficeID = pObjIndividualModel.TaxOfficeID,
                            MaritalStatusID = pObjIndividualModel.MaritalStatusID,
                            NationalityID = pObjIndividualModel.NationalityID,
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                            EconomicActivitiesID = pObjIndividualModel.EconomicActivitiesID,
                            NotificationMethodID = pObjIndividualModel.NotificationMethodID,
                            ContactAddress = pObjIndividualModel.ContactAddress,
                            Active = true,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };
                        FuncResponse<Individual> mObjResponse = new BLIndividual().BL_InsertUpdateIndividual(mObjIndividual);

                        if (mObjResponse.Success)
                        {
                            if (GlobalDefaultValues.SendNotification)
                            {
                                //Send Notification
                                EmailDetails mObjEmailDetails = new EmailDetails()
                                {
                                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                                    TaxPayerTypeName = "Individual",
                                    TaxPayerID = mObjIndividual.IndividualID,
                                    TaxPayerName = mObjIndividual.FirstName + " " + mObjIndividual.LastName,
                                    TaxPayerRIN = mObjIndividual.IndividualRIN,
                                    TaxPayerMobileNumber = mObjIndividual.MobileNumber1,
                                    TaxPayerEmail = mObjIndividual.EmailAddress1,
                                    ContactAddress = mObjIndividual.ContactAddress,
                                    TaxPayerTIN = mObjIndividual.TIN,
                                    LoggedInUserID = SessionManager.UserID,
                                };

                                if (!string.IsNullOrWhiteSpace(mObjIndividual.EmailAddress1))
                                {
                                    BLEmailHandler.BL_TaxPayerCreated(mObjEmailDetails);
                                }

                                if (!string.IsNullOrWhiteSpace(mObjIndividual.MobileNumber1))
                                {
                                    UtilityController.BL_TaxPayerCreated(mObjEmailDetails);
                                }
                            }

                            //Creating mapping between individual and business
                            MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                            {
                                AssetTypeID = pObjIndividualModel.AssetTypeID,
                                AssetID = pObjIndividualModel.AssetID,
                                TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                                TaxPayerRoleID = pObjIndividualModel.TaxPayerRoleID,
                                TaxPayerID = mObjResponse.AdditionalData.IndividualID,
                                Active = true,
                                CreatedBy = SessionManager.UserID,
                                CreatedDate = CommUtil.GetCurrentDateTime()
                            };

                            FuncResponse mObjTPResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

                            if (mObjTPResponse.Success)
                            {
                                var vExists = (from tpa in _db.MAP_TaxPayer_Asset
                                               join aa in _db.Asset_Types on tpa.AssetTypeID equals aa.AssetTypeID
                                               join tp in _db.TaxPayer_Roles on tpa.TaxPayerRoleID equals tp.TaxPayerRoleID
                                               join tpx in _db.TaxPayer_Types on tpa.TaxPayerTypeID equals tpx.TaxPayerTypeID
                                               join idd in _db.Individuals on tpa.TaxPayerID equals idd.IndividualID
                                               where tpa.AssetTypeID == mObjTaxPayerAsset.AssetTypeID && tpa.AssetID == mObjTaxPayerAsset.AssetID
                                                  && tpa.TaxPayerTypeID == mObjTaxPayerAsset.TaxPayerTypeID && tpa.TaxPayerID == mObjTaxPayerAsset.TaxPayerID
                                                  && tpa.TaxPayerRoleID == mObjTaxPayerAsset.TaxPayerRoleID && tpa.Active == true

                                               select new { Post = tpa, Meta = tp, All = tpx, Idd = idd, Aa = aa }).FirstOrDefault();

                                if (GlobalDefaultValues.SendNotification)
                                {
                                    //Send Notification
                                    EmailDetails mObjEmailDetails = new EmailDetails()
                                    {
                                        TaxPayerTypeID = vExists.Post.TaxPayerTypeID.GetValueOrDefault(),
                                        TaxPayerTypeName = vExists.All.TaxPayerTypeName,
                                        TaxPayerID = vExists.Idd.IndividualID,
                                        TaxPayerName = vExists.Idd.FirstName + " " + vExists.Idd.LastName,
                                        TaxPayerRIN = vExists.Idd.IndividualRIN,
                                        TaxPayerRoleName = vExists.Meta.TaxPayerRoleName,
                                        AssetName = vExists.Aa.AssetTypeName,
                                        LoggedInUserID = SessionManager.UserID,
                                    };

                                    if (!string.IsNullOrWhiteSpace(vExists.Idd.EmailAddress1))
                                    {
                                        BLEmailHandler.BL_AssetProfileLinked(mObjEmailDetails);
                                    }

                                    if (!string.IsNullOrWhiteSpace(vExists.Idd.MobileNumber1))
                                    {
                                        UtilityController.BL_AssetProfileLinked(mObjEmailDetails);
                                    }
                                }
                                mObjScope.Complete();
                                FlashMessage.Info("Individual Created Successfully and Linked to Asset");
                                return RedirectToAction("Details", "ProfileBusiness", new { id = pObjIndividualModel.AssetID, name = pObjIndividualModel.AssetRIN });
                            }
                            else
                            {
                                throw new Exception(mObjResponse.Message);
                            }
                        }
                        else
                        {
                            UI_FillIndividualDropDown(pObjIndividualModel);
                            ViewBag.Message = mObjResponse.Message;
                            return View(pObjIndividualModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        UI_FillIndividualDropDown(pObjIndividualModel);
                        Transaction.Current.Rollback();
                        ViewBag.Message = "Error occurred while saving individual";
                        return View(pObjIndividualModel);
                    }
                }
            }

        }

        public void UI_FillCompanyDropDown(CompanyViewModel pObjCompanyViewModel = null)
        {
            if (pObjCompanyViewModel != null)
                pObjCompanyViewModel.TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies;
            else if (pObjCompanyViewModel == null)
                pObjCompanyViewModel = new CompanyViewModel();

            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies, AssetTypeID = (int)EnumList.AssetTypes.Business });
            UI_FillTaxOfficeDropDown(new Tax_Offices() { intStatus = 1, IncludeTaxOfficeIds = pObjCompanyViewModel.TaxOfficeID.ToString() });
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1, IncludeTaxPayerTypeIds = pObjCompanyViewModel.TaxPayerTypeID.ToString() }, (int)EnumList.TaxPayerType.Companies);
            UI_FillEconomicActivitiesDropDown(new Economic_Activities() { intStatus = 1, IncludeEconomicActivitiesIds = pObjCompanyViewModel.EconomicActivitiesID.ToString(), TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies });
            UI_FillNotificationMethodDropDown(new Notification_Method() { intStatus = 1, IncludeNotificationMethodIds = pObjCompanyViewModel.NotificationMethodID.ToString() });
        }


        public ActionResult AddCorporate(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Business mObjBusiness = new Business()
                {
                    BusinessID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBusinessListNewTy_Result mObjBusinessData = new BLBusiness().BL_GetBusinessDetails(mObjBusiness);

                if (mObjBusinessData != null)
                {
                    TPCompanyViewModel mObjCompanyModel = new TPCompanyViewModel()
                    {
                        AssetID = mObjBusinessData.BusinessID.GetValueOrDefault(),
                        AssetName = mObjBusinessData.BusinessName,
                        AssetRIN = mObjBusinessData.BusinessRIN,
                        AssetLGAName = mObjBusinessData.LGAName,
                        AssetTypeID = (int)EnumList.AssetTypes.Business,
                        AssetTypeName = mObjBusinessData.AssetTypeName,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                    };

                    UI_FillCompanyDropDown();
                    return View(mObjCompanyModel);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileBusiness");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileBusiness");
            }
        }

        [HttpPost]

        [ValidateAntiForgeryToken()]
        public ActionResult AddCorporate(TPCompanyViewModel pObjCompanyModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillCompanyDropDown(pObjCompanyModel);
                return View(pObjCompanyModel);
            }
            else
            {
                using (TransactionScope mObjScope = new TransactionScope())
                {
                    try
                    {
                        Company mObjCompany = new Company()
                        {
                            CompanyID = 0,
                            CompanyName = pObjCompanyModel.CompanyName,
                            TIN = pObjCompanyModel.TIN,
                            MobileNumber1 = pObjCompanyModel.MobileNumber1,
                            MobileNumber2 = pObjCompanyModel.MobileNumber2,
                            EmailAddress1 = pObjCompanyModel.EmailAddress1,
                            EmailAddress2 = pObjCompanyModel.EmailAddress2,
                            TaxOfficeID = pObjCompanyModel.TaxOfficeID,
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                            EconomicActivitiesID = pObjCompanyModel.EconomicActivitiesID,
                            NotificationMethodID = pObjCompanyModel.NotificationMethodID,
                            ContactAddress = pObjCompanyModel.ContactAddress,
                            CACRegistrationNumber = pObjCompanyModel.CACRegistrationNumber,
                            Active = true,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse<Company> mObjResponse = new BLCompany().BL_InsertUpdateCompany(mObjCompany);

                        if (mObjResponse.Success)
                        {
                            if (GlobalDefaultValues.SendNotification)
                            {
                                //Send Notification
                                EmailDetails mObjEmailDetails = new EmailDetails()
                                {
                                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                                    TaxPayerTypeName = "Company",
                                    TaxPayerID = pObjCompanyModel.CompanyID,
                                    TaxPayerName = pObjCompanyModel.CompanyName,
                                    TaxPayerRIN = pObjCompanyModel.CompanyRIN,
                                    TaxPayerMobileNumber = pObjCompanyModel.MobileNumber1,
                                    TaxPayerEmail = pObjCompanyModel.EmailAddress1,
                                    ContactAddress = pObjCompanyModel.ContactAddress,
                                    TaxPayerTIN = pObjCompanyModel.TIN,
                                    LoggedInUserID = SessionManager.UserID,
                                };

                                if (!string.IsNullOrWhiteSpace(pObjCompanyModel.EmailAddress1))
                                {
                                    BLEmailHandler.BL_TaxPayerCreated(mObjEmailDetails);
                                }

                                if (!string.IsNullOrWhiteSpace(pObjCompanyModel.MobileNumber1))
                                {
                                    UtilityController.BL_TaxPayerCreated(mObjEmailDetails);
                                }
                            }
                            //Creating mapping between individual and business
                            MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                            {
                                AssetTypeID = pObjCompanyModel.AssetTypeID,
                                AssetID = pObjCompanyModel.AssetID,
                                TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                                TaxPayerRoleID = pObjCompanyModel.TaxPayerRoleID,
                                TaxPayerID = mObjResponse.AdditionalData.CompanyID,
                                Active = true,
                                CreatedBy = SessionManager.UserID,
                                CreatedDate = CommUtil.GetCurrentDateTime()
                            };

                            FuncResponse mObjTPResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

                            if (mObjTPResponse.Success)
                            {
                                var vExists = (from tpa in _db.MAP_TaxPayer_Asset
                                               join aa in _db.Asset_Types on tpa.AssetTypeID equals aa.AssetTypeID
                                               join tp in _db.TaxPayer_Roles on tpa.TaxPayerRoleID equals tp.TaxPayerRoleID
                                               join tpx in _db.TaxPayer_Types on tpa.TaxPayerTypeID equals tpx.TaxPayerTypeID
                                               join idd in _db.Individuals on tpa.TaxPayerID equals idd.IndividualID
                                               where tpa.AssetTypeID == mObjTaxPayerAsset.AssetTypeID && tpa.AssetID == mObjTaxPayerAsset.AssetID
                                                  && tpa.TaxPayerTypeID == mObjTaxPayerAsset.TaxPayerTypeID && tpa.TaxPayerID == mObjTaxPayerAsset.TaxPayerID
                                                  && tpa.TaxPayerRoleID == mObjTaxPayerAsset.TaxPayerRoleID && tpa.Active == true

                                               select new { Post = tpa, Meta = tp, All = tpx, Idd = idd, Aa = aa }).FirstOrDefault();

                                if (GlobalDefaultValues.SendNotification)
                                {
                                    //Send Notification
                                    EmailDetails mObjEmailDetails = new EmailDetails()
                                    {
                                        TaxPayerTypeID = vExists.Post.TaxPayerTypeID.GetValueOrDefault(),
                                        TaxPayerTypeName = vExists.All.TaxPayerTypeName,
                                        TaxPayerID = vExists.Idd.IndividualID,
                                        TaxPayerName = vExists.Idd.FirstName + " " + vExists.Idd.LastName,
                                        TaxPayerRIN = vExists.Idd.IndividualRIN,
                                        TaxPayerRoleName = vExists.Meta.TaxPayerRoleName,
                                        AssetName = vExists.Aa.AssetTypeName,
                                        LoggedInUserID = SessionManager.UserID,
                                    };

                                    if (!string.IsNullOrWhiteSpace(vExists.Idd.EmailAddress1))
                                    {
                                        BLEmailHandler.BL_AssetProfileLinked(mObjEmailDetails);
                                    }

                                    if (!string.IsNullOrWhiteSpace(vExists.Idd.MobileNumber1))
                                    {
                                        UtilityController.BL_AssetProfileLinked(mObjEmailDetails);
                                    }
                                }
                                mObjScope.Complete();
                                FlashMessage.Info("Corporate Created Successfully and Linked to Asset");
                                return RedirectToAction("Details", "ProfileBusiness", new { id = pObjCompanyModel.AssetID, name = pObjCompanyModel.AssetRIN });
                            }
                            else
                            {
                                throw new Exception(mObjResponse.Message);
                            }
                        }
                        else
                        {
                            UI_FillCompanyDropDown(pObjCompanyModel);
                            ViewBag.Message = mObjResponse.Message;
                            return View(pObjCompanyModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        UI_FillCompanyDropDown(pObjCompanyModel);
                        Transaction.Current.Rollback();
                        ViewBag.Message = "Error occurred while saving corporate";
                        return View(pObjCompanyModel);
                    }
                }
            }

        }

        public void UI_FillGovernmentDropDown(GovernmentViewModel pObjGovernmentViewModel = null)
        {
            if (pObjGovernmentViewModel != null)
                pObjGovernmentViewModel.TaxPayerTypeID = (int)EnumList.TaxPayerType.Government;
            else if (pObjGovernmentViewModel == null)
                pObjGovernmentViewModel = new GovernmentViewModel();

            UI_FillTaxOfficeDropDown(new Tax_Offices() { intStatus = 1, IncludeTaxOfficeIds = pObjGovernmentViewModel.TaxOfficeID.ToString() });
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1, IncludeTaxPayerTypeIds = pObjGovernmentViewModel.TaxPayerTypeID.ToString() }, (int)EnumList.TaxPayerType.Government);
            UI_FillGovernmentTypeDropDown(new Government_Types() { intStatus = 1, IncludeGovernmentTypeIds = pObjGovernmentViewModel.GovernmentTypeID.ToString() });
            UI_FillNotificationMethodDropDown(new Notification_Method() { intStatus = 1, IncludeNotificationMethodIds = pObjGovernmentViewModel.NotificationMethodID.ToString() });
            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual, AssetTypeID = (int)EnumList.AssetTypes.Business });
        }


        public ActionResult AddGovernment(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Business mObjBusiness = new Business()
                {
                    BusinessID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBusinessListNewTy_Result mObjBusinessData = new BLBusiness().BL_GetBusinessDetails(mObjBusiness);

                if (mObjBusinessData != null)
                {
                    TPGovernmentViewModel mObjGovernmentModel = new TPGovernmentViewModel()
                    {
                        AssetID = mObjBusinessData.BusinessID.GetValueOrDefault(),
                        AssetName = mObjBusinessData.BusinessName,
                        AssetRIN = mObjBusinessData.BusinessRIN,
                        AssetLGAName = mObjBusinessData.LGAName,
                        AssetTypeID = (int)EnumList.AssetTypes.Business,
                        AssetTypeName = mObjBusinessData.AssetTypeName,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                    };

                    UI_FillGovernmentDropDown();
                    return View(mObjGovernmentModel);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileBusiness");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileBusiness");
            }
        }

        [HttpPost]

        [ValidateAntiForgeryToken()]
        public ActionResult AddGovernment(TPGovernmentViewModel pObjGovernmentModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillGovernmentDropDown(pObjGovernmentModel);
                return View(pObjGovernmentModel);
            }
            else
            {
                using (TransactionScope mObjScope = new TransactionScope())
                {
                    try
                    {
                        Government mObjGovernment = new Government()
                        {
                            GovernmentID = 0,
                            GovernmentName = pObjGovernmentModel.GovernmentName,
                            GovernmentTypeID = pObjGovernmentModel.GovernmentTypeID,
                            TIN = pObjGovernmentModel.TIN,
                            TaxOfficeID = pObjGovernmentModel.TaxOfficeID,
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                            ContactNumber = pObjGovernmentModel.ContactNumber,
                            ContactEmail = pObjGovernmentModel.ContactEmail,
                            ContactName = pObjGovernmentModel.ContactName,
                            NotificationMethodID = pObjGovernmentModel.NotificationMethodID,
                            ContactAddress = pObjGovernmentModel.ContactAddress,
                            Active = true,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse<Government> mObjResponse = new BLGovernment().BL_InsertUpdateGovernment(mObjGovernment);

                        if (mObjResponse.Success)
                        {
                            if (GlobalDefaultValues.SendNotification)
                            {
                                //Send Notification
                                EmailDetails mObjEmailDetails = new EmailDetails()
                                {
                                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                                    TaxPayerTypeName = "Government",
                                    TaxPayerID = pObjGovernmentModel.GovernmentID,
                                    TaxPayerName = pObjGovernmentModel.GovernmentName,
                                    TaxPayerRIN = pObjGovernmentModel.GovernmentRIN,
                                    TaxPayerMobileNumber = pObjGovernmentModel.ContactNumber,
                                    TaxPayerEmail = pObjGovernmentModel.ContactEmail,
                                    ContactAddress = pObjGovernmentModel.ContactAddress,
                                    TaxPayerTIN = pObjGovernmentModel.TIN,
                                    LoggedInUserID = SessionManager.UserID,
                                };

                                if (!string.IsNullOrWhiteSpace(pObjGovernmentModel.ContactEmail))
                                {
                                    BLEmailHandler.BL_TaxPayerCreated(mObjEmailDetails);
                                }

                                if (!string.IsNullOrWhiteSpace(pObjGovernmentModel.ContactNumber))
                                {
                                    UtilityController.BL_TaxPayerCreated(mObjEmailDetails);
                                }
                            }
                            //Creating mapping between individual and business
                            MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                            {
                                AssetTypeID = pObjGovernmentModel.AssetTypeID,
                                AssetID = pObjGovernmentModel.AssetID,
                                TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                                TaxPayerRoleID = pObjGovernmentModel.TaxPayerRoleID,
                                TaxPayerID = mObjResponse.AdditionalData.GovernmentID,
                                Active = true,
                                CreatedBy = SessionManager.UserID,
                                CreatedDate = CommUtil.GetCurrentDateTime()
                            };

                            FuncResponse mObjTPResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

                            if (mObjTPResponse.Success)
                            {
                                var vExists = (from tpa in _db.MAP_TaxPayer_Asset
                                               join aa in _db.Asset_Types on tpa.AssetTypeID equals aa.AssetTypeID
                                               join tp in _db.TaxPayer_Roles on tpa.TaxPayerRoleID equals tp.TaxPayerRoleID
                                               join tpx in _db.TaxPayer_Types on tpa.TaxPayerTypeID equals tpx.TaxPayerTypeID
                                               join idd in _db.Individuals on tpa.TaxPayerID equals idd.IndividualID
                                               where tpa.AssetTypeID == mObjTaxPayerAsset.AssetTypeID && tpa.AssetID == mObjTaxPayerAsset.AssetID
                                                  && tpa.TaxPayerTypeID == mObjTaxPayerAsset.TaxPayerTypeID && tpa.TaxPayerID == mObjTaxPayerAsset.TaxPayerID
                                                  && tpa.TaxPayerRoleID == mObjTaxPayerAsset.TaxPayerRoleID && tpa.Active == true

                                               select new { Post = tpa, Meta = tp, All = tpx, Idd = idd, Aa = aa }).FirstOrDefault();

                                if (GlobalDefaultValues.SendNotification)
                                {
                                    //Send Notification
                                    EmailDetails mObjEmailDetails = new EmailDetails()
                                    {
                                        TaxPayerTypeID = vExists.Post.TaxPayerTypeID.GetValueOrDefault(),
                                        TaxPayerTypeName = vExists.All.TaxPayerTypeName,
                                        TaxPayerID = vExists.Idd.IndividualID,
                                        TaxPayerName = vExists.Idd.FirstName + " " + vExists.Idd.LastName,
                                        TaxPayerRIN = vExists.Idd.IndividualRIN,
                                        TaxPayerRoleName = vExists.Meta.TaxPayerRoleName,
                                        AssetName = vExists.Aa.AssetTypeName,
                                        LoggedInUserID = SessionManager.UserID,
                                    };

                                    if (!string.IsNullOrWhiteSpace(vExists.Idd.EmailAddress1))
                                    {
                                        BLEmailHandler.BL_AssetProfileLinked(mObjEmailDetails);
                                    }

                                    if (!string.IsNullOrWhiteSpace(vExists.Idd.MobileNumber1))
                                    {
                                        UtilityController.BL_AssetProfileLinked(mObjEmailDetails);
                                    }
                                }
                                mObjScope.Complete();
                                FlashMessage.Info("Government Created Successfully and Linked to Asset");
                                return RedirectToAction("Details", "ProfileBusiness", new { id = pObjGovernmentModel.AssetID, name = pObjGovernmentModel.AssetRIN });
                            }
                            else
                            {
                                throw new Exception(mObjResponse.Message);
                            }
                        }
                        else
                        {
                            UI_FillGovernmentDropDown(pObjGovernmentModel);
                            ViewBag.Message = mObjResponse.Message;
                            return View(pObjGovernmentModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        UI_FillGovernmentDropDown(pObjGovernmentModel);
                        Transaction.Current.Rollback();
                        ViewBag.Message = "Error occurred while saving government";
                        return View(pObjGovernmentModel);
                    }
                }
            }

        }

        public void UI_FillSpecialDropDown(SpecialViewModel pObjSpecialViewModel = null)
        {
            if (pObjSpecialViewModel != null)
                pObjSpecialViewModel.TaxPayerTypeID = (int)EnumList.TaxPayerType.Special;
            else if (pObjSpecialViewModel == null)
                pObjSpecialViewModel = new SpecialViewModel();

            UI_FillTaxOfficeDropDown(new Tax_Offices() { intStatus = 1, IncludeTaxOfficeIds = pObjSpecialViewModel.TaxOfficeID.ToString() });
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1, IncludeTaxPayerTypeIds = pObjSpecialViewModel.TaxPayerTypeID.ToString() }, (int)EnumList.TaxPayerType.Special);
            UI_FillNotificationMethodDropDown(new Notification_Method() { intStatus = 1, IncludeNotificationMethodIds = pObjSpecialViewModel.NotificationMethodID.ToString() });
            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual, AssetTypeID = (int)EnumList.AssetTypes.Business });
        }


        public ActionResult AddSpecial(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Business mObjBusiness = new Business()
                {
                    BusinessID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBusinessListNewTy_Result mObjBusinessData = new BLBusiness().BL_GetBusinessDetails(mObjBusiness);

                if (mObjBusinessData != null)
                {
                    TPSpecialViewModel mObjSpecialModel = new TPSpecialViewModel()
                    {
                        AssetID = mObjBusinessData.BusinessID.GetValueOrDefault(),
                        AssetName = mObjBusinessData.BusinessName,
                        AssetRIN = mObjBusinessData.BusinessRIN,
                        AssetLGAName = mObjBusinessData.LGAName,
                        AssetTypeID = (int)EnumList.AssetTypes.Business,
                        AssetTypeName = mObjBusinessData.AssetTypeName,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Special,
                    };

                    UI_FillSpecialDropDown();
                    return View(mObjSpecialModel);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileBusiness");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileBusiness");
            }
        }

        [HttpPost]

        [ValidateAntiForgeryToken()]
        public ActionResult AddSpecial(TPSpecialViewModel pObjSpecialModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillSpecialDropDown(pObjSpecialModel);
                return View(pObjSpecialModel);
            }
            else
            {
                using (TransactionScope mObjScope = new TransactionScope())
                {
                    try
                    {
                        Special mObjSpecial = new Special()
                        {
                            SpecialID = 0,
                            SpecialTaxPayerName = pObjSpecialModel.SpecialName,
                            TIN = pObjSpecialModel.TIN,
                            TaxOfficeID = pObjSpecialModel.TaxOfficeID,
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Special,
                            ContactNumber = pObjSpecialModel.ContactNumber,
                            ContactEmail = pObjSpecialModel.ContactEmail,
                            ContactName = pObjSpecialModel.ContactName,
                            Description = pObjSpecialModel.Description,
                            NotificationMethodID = pObjSpecialModel.NotificationMethodID,
                            Active = true,
                            CreatedBy = SessionManager.UserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse<Special> mObjResponse = new BLSpecial().BL_InsertUpdateSpecial(mObjSpecial);

                        if (mObjResponse.Success)
                        {
                            //Creating mapping between individual and business
                            MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                            {
                                AssetTypeID = pObjSpecialModel.AssetTypeID,
                                AssetID = pObjSpecialModel.AssetID,
                                TaxPayerTypeID = (int)EnumList.TaxPayerType.Special,
                                TaxPayerRoleID = pObjSpecialModel.TaxPayerRoleID,
                                TaxPayerID = mObjResponse.AdditionalData.SpecialID,
                                Active = true,
                                CreatedBy = SessionManager.UserID,
                                CreatedDate = CommUtil.GetCurrentDateTime()
                            };

                            FuncResponse mObjTPResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

                            if (mObjTPResponse.Success)
                            {
                                var vExists = (from tpa in _db.MAP_TaxPayer_Asset
                                               join aa in _db.Asset_Types on tpa.AssetTypeID equals aa.AssetTypeID
                                               join tp in _db.TaxPayer_Roles on tpa.TaxPayerRoleID equals tp.TaxPayerRoleID
                                               join tpx in _db.TaxPayer_Types on tpa.TaxPayerTypeID equals tpx.TaxPayerTypeID
                                               join idd in _db.Individuals on tpa.TaxPayerID equals idd.IndividualID
                                               where tpa.AssetTypeID == mObjTaxPayerAsset.AssetTypeID && tpa.AssetID == mObjTaxPayerAsset.AssetID
                                                  && tpa.TaxPayerTypeID == mObjTaxPayerAsset.TaxPayerTypeID && tpa.TaxPayerID == mObjTaxPayerAsset.TaxPayerID
                                                  && tpa.TaxPayerRoleID == mObjTaxPayerAsset.TaxPayerRoleID && tpa.Active == true

                                               select new { Post = tpa, Meta = tp, All = tpx, Idd = idd, Aa = aa }).FirstOrDefault();

                                if (GlobalDefaultValues.SendNotification)
                                {
                                    //Send Notification
                                    EmailDetails mObjEmailDetails = new EmailDetails()
                                    {
                                        TaxPayerTypeID = vExists.Post.TaxPayerTypeID.GetValueOrDefault(),
                                        TaxPayerTypeName = vExists.All.TaxPayerTypeName,
                                        TaxPayerID = vExists.Idd.IndividualID,
                                        TaxPayerName = vExists.Idd.FirstName + " " + vExists.Idd.LastName,
                                        TaxPayerRIN = vExists.Idd.IndividualRIN,
                                        TaxPayerRoleName = vExists.Meta.TaxPayerRoleName,
                                        AssetName = vExists.Aa.AssetTypeName,
                                        LoggedInUserID = SessionManager.UserID,
                                    };

                                    if (!string.IsNullOrWhiteSpace(vExists.Idd.EmailAddress1))
                                    {
                                        BLEmailHandler.BL_AssetProfileLinked(mObjEmailDetails);
                                    }

                                    if (!string.IsNullOrWhiteSpace(vExists.Idd.MobileNumber1))
                                    {
                                        UtilityController.BL_AssetProfileLinked(mObjEmailDetails);
                                    }
                                }
                                mObjScope.Complete();
                                FlashMessage.Info("Special Created Successfully and Linked to Asset");
                                return RedirectToAction("Details", "ProfileBusiness", new { id = pObjSpecialModel.AssetID, name = pObjSpecialModel.AssetRIN });
                            }
                            else
                            {
                                throw new Exception(mObjResponse.Message);
                            }
                        }
                        else
                        {
                            UI_FillSpecialDropDown(pObjSpecialModel);
                            ViewBag.Message = mObjResponse.Message;
                            return View(pObjSpecialModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.SendErrorToText(ex);
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        UI_FillSpecialDropDown(pObjSpecialModel);
                        Transaction.Current.Rollback();
                        ViewBag.Message = "Error occurred while saving special";
                        return View(pObjSpecialModel);
                    }
                }
            }

        }

        public JsonResult UpdateStatus(Business pObjBusinessData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjBusinessData.BusinessID != 0)
            {
                FuncResponse mObjFuncResponse = new BLBusiness().BL_UpdateStatus(pObjBusinessData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;
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