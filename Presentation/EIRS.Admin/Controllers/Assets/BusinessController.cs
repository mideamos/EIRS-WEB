using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Mvc;
using Vereyon.Web;
using Elmah;
using System.Linq.Dynamic;
using System.Text;

namespace EIRS.Admin.Controllers
{
    public class BusinessController : BaseController
    {
        public ActionResult List()
        { 
            return View();
        } 

        [HttpPost]
        public JsonResult LoadData()
       {
            //Get parameters

            // get Start (paging start index) and length (page size for paging)
            var vDraw = Request.Form.GetValues("draw").FirstOrDefault();
            var vStart = Request.Form.GetValues("start").FirstOrDefault();
            var vLength = Request.Form.GetValues("length").FirstOrDefault();
            //Get Sort columns value
            var vSortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var vSortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var vFilter = Request.Form.GetValues("search[value]").FirstOrDefault();

            StringBuilder sbWhereCondition = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(Request.Form["BusinessRIN"]))
            {
                sbWhereCondition.Append(" AND ISNULL(BusinessRIN,'') LIKE @BusinessRIN");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["BusinessName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(BusinessName,'') LIKE @BusinessName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["BusinessTypeName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(BusinessTypeName,'') LIKE @BusinessTypeName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["LGAName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(LGAName,'') LIKE @LGAName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["BusinessCategoryName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(BusinessCategoryName,'') LIKE @BusinessCategoryName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["BusinessSectorName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(BusinessSectorName,'') LIKE @BusinessSectorName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["BusinessSubSectorName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(BusinessSubSectorName,'') LIKE @BusinessSubSectorName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["BusinessStructureName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(BusinessStructureName,'') LIKE @BusinessStructureName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["BusinessOperationName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(BusinessOperationName,'') LIKE @BusinessOperationName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["SizeName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(SizeName,'') LIKE @SizeName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ActiveText"]))
            {
                sbWhereCondition.Append(" AND CASE WHEN ISNULL(bus.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @ActiveText");
            }

            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( ISNULL(BusinessRIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BusinessName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BusinessTypeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(LGAName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BusinessCategoryName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BusinessSectorName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BusinessSubSectorName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BusinessStructureName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(BusinessOperationName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(SizeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR CASE WHEN ISNULL(bus.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @MainFilter)");
            }
            Business mObjBusiness = new Business()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),

                BusinessRIN = !string.IsNullOrWhiteSpace(Request.Form["BusinessRIN"]) ? "%" + Request.Form["BusinessRIN"].Trim() + "%" : TrynParse.parseString(Request.Form["BusinessRIN"]),
               
                BusinessName = !string.IsNullOrWhiteSpace(Request.Form["BusinessName"]) ? "%" + Request.Form["BusinessName"].Trim() + "%" : TrynParse.parseString(Request.Form["BusinessName"]),
             
                BusinessTypeName =
                !string.IsNullOrWhiteSpace(Request.Form["BusinessTypeName"]) ? "%" + Request.Form["BusinessTypeName"].Trim() + "%" : TrynParse.parseString(Request.Form["BusinessTypeName"]),
                
                LGAName = !string.IsNullOrWhiteSpace(Request.Form["LGAName"]) ? "%" + Request.Form["LGAName"].Trim() + "%" : TrynParse.parseString(Request.Form["LGAName"]),
               
                BusinessCategoryName = !string.IsNullOrWhiteSpace(Request.Form["BusinessCategoryName"]) ? "%" + Request.Form["BusinessCategoryName"].Trim() + "%" : TrynParse.parseString(Request.Form["BusinessCategoryName"]),

               
                BusinessSectorName = !string.IsNullOrWhiteSpace(Request.Form["BusinessSectorName"]) ? "%" + Request.Form["BusinessSectorName"].Trim() + "%" : TrynParse.parseString(Request.Form["BusinessSectorName"]),
                
                BusinessSubSectorName = !string.IsNullOrWhiteSpace(Request.Form["BusinessSubSectorName"]) ? "%" + Request.Form["BusinessSubSectorName"].Trim() + "%" : TrynParse.parseString(Request.Form["BusinessSubSectorName"]),
              
                BusinessStructureName = !string.IsNullOrWhiteSpace(Request.Form["BusinessStructureName"]) ? "%" + Request.Form["BusinessStructureName"].Trim() + "%" : TrynParse.parseString(Request.Form["BusinessStructureName"]),
            
                BusinessOperationName = !string.IsNullOrWhiteSpace(Request.Form["BusinessOperationName"]) ? "%" + Request.Form["BusinessOperationName"].Trim() + "%" : TrynParse.parseString(Request.Form["BusinessOperationName"]),

                SizeName = !string.IsNullOrWhiteSpace(Request.Form["SizeName"]) ? "%" + Request.Form["SizeName"].Trim() + "%" : TrynParse.parseString(Request.Form["SizeName"]),
                ActiveText = !string.IsNullOrWhiteSpace(Request.Form["ActiveText"]) ? "%" + Request.Form["ActiveText"].Trim() + "%" : TrynParse.parseString(Request.Form["ActiveText"]),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
                
                //intStatus = 2,

            };
            IDictionary<string, object> dcData = new BLBusiness().BL_SearchBusiness(mObjBusiness);
            IList<usp_SearchBusiness_Result> lstBusiness = (IList<usp_SearchBusiness_Result>)dcData["BusinessList"];
            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstBusiness
            }, JsonRequestBehavior.AllowGet);


            }

        public void UI_FillDropDown(BusinessViewModel pObjBusinessViewModel = null)
        {
            if (pObjBusinessViewModel != null)
                pObjBusinessViewModel.AssetTypeID = (int)EnumList.AssetTypes.Business;
            else if (pObjBusinessViewModel == null)
                pObjBusinessViewModel = new BusinessViewModel();

            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjBusinessViewModel.AssetTypeID.ToString() }, (int)EnumList.AssetTypes.Business);
            UI_FillBusinessTypeDropDown(new Business_Types() { intStatus = 1, IncludeBusinessTypeIds = pObjBusinessViewModel.BusinessTypeID.ToString() });
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
                    SizeID = pObjBusinessModel.SizeID,
                    ContactName = pObjBusinessModel.ContactName,
                    BusinessAddress = pObjBusinessModel.BusinessAddress,
                    BusinessNumber = pObjBusinessModel.BusinessNumber,
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Business> mObjResponse = new BLBusiness().BL_InsertUpdateBusiness(mObjBusiness);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "Business");
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

                usp_GetBusinessList_Result mObjBusinessData = new BLBusiness().BL_GetBusinessDetails(mObjBusiness);

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
                    return RedirectToAction("List", "Business");
                }
            }
            else
            {
                return RedirectToAction("List", "Business");
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
                    Active = pObjBusinessModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Business> mObjResponse = new BLBusiness().BL_InsertUpdateBusiness(mObjBusiness);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "Business");
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

                usp_GetBusinessList_Result mObjBusinessData = new BLBusiness().BL_GetBusinessDetails(mObjBusiness);

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
                    return RedirectToAction("List", "Business");
                }
            }
            else
            {
                return RedirectToAction("List", "Business");
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

                if (mObjFuncResponse.Success)
                {
                    dcResponse["BusinessList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BusinessTypeChange(int BusinessTypeID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            IList<DropDownListResult> lstBusinessCategory = new BLBusinessCategory().BL_GetBusinessCategoryDropDownList(new Business_Category() { intStatus = 1, BusinessTypeID = BusinessTypeID });
            dcResponse["BusinessCategory"] = lstBusinessCategory;

            IList<DropDownListResult> lstBusinessStructure = new BLBusinessStructure().BL_GetBusinessStructureDropDownList(new Business_Structure() { intStatus = 1, BusinessTypeID = BusinessTypeID });
            dcResponse["BusinessStructure"] = lstBusinessStructure;

            IList<DropDownListResult> lstBusinessOperation = new BLBusinessOperation().BL_GetBusinessOperationDropDownList(new Business_Operation() { intStatus = 1, BusinessTypeID = BusinessTypeID });
            dcResponse["BusinessOperation"] = lstBusinessOperation;

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }


        public ActionResult TaxPayerList(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetID = id.GetValueOrDefault(),
                    AssetTypeID = (int)EnumList.AssetTypes.Business
                };

                IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                usp_GetBusinessList_Result mObjBusinessDetails = new BLBusiness().BL_GetBusinessDetails(new Business() { intStatus = 2, BusinessID = id.GetValueOrDefault() });
                if (mObjBusinessDetails != null)
                {
                    ViewBag.AssetID = id;
                    ViewBag.AssetRIN = name;
                    ViewBag.AssetName = mObjBusinessDetails.BusinessName;
                    return View(lstTaxPayerAsset);
                }
                else
                {
                    return RedirectToAction("List", "Business");
                }
            }
            else
            {
                return RedirectToAction("List", "Business");
            }
        }

        public void UI_FillDropDown(TaxPayerAssetViewModel pObjTPAModel)
        {
            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1 }, (int)EnumList.AssetTypes.Business);
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1 });
            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { intStatus = 1, AssetTypeID = (int)EnumList.AssetTypes.Business });
        }

        public ActionResult AddTaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                usp_GetBusinessList_Result mObjBusinessDetails = new BLBusiness().BL_GetBusinessDetails(new Business() { intStatus = 2, BusinessID = id.GetValueOrDefault() });
                TaxPayerAssetViewModel mObjTaxPayerAssetModel = new TaxPayerAssetViewModel()
                {
                    AssetID = id.GetValueOrDefault(),
                    AssetRIN = name,
                    AssetName = mObjBusinessDetails.BusinessName,
                    AssetTypeID = (int)EnumList.AssetTypes.Business
                };

                UI_FillDropDown(mObjTaxPayerAssetModel);
                return View(mObjTaxPayerAssetModel);
            }
            else
            {
                return RedirectToAction("TaxPayerList", "Business");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddTaxPayer(TaxPayerAssetViewModel pObjAssetModel)
        {
            if (!ModelState.IsValid)
            {
                pObjAssetModel.AssetTypeID = (int)EnumList.AssetTypes.Business;
                UI_FillDropDown(pObjAssetModel);
                return View(pObjAssetModel);
            }
            else
            {
                BLTaxPayerAsset mobjBLTaxPayerAsset = new BLTaxPayerAsset();
                string[] strTaxPayerIds = pObjAssetModel.TaxPayerIds.Split(',');

                foreach (var vTaxPayerID in strTaxPayerIds)
                {
                    if (!string.IsNullOrWhiteSpace(vTaxPayerID))
                    {
                        MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                        {
                            AssetTypeID = (int)EnumList.AssetTypes.Business,
                            AssetID = pObjAssetModel.AssetID,
                            TaxPayerTypeID = pObjAssetModel.TaxPayerTypeID,
                            TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID,
                            TaxPayerID = TrynParse.parseInt(vTaxPayerID),
                            Active = true,
                            CreatedBy = SessionManager.SystemUserID,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse mObjResponse = mobjBLTaxPayerAsset.BL_InsertTaxPayerAsset(mObjTaxPayerAsset);
                    }
                }

                FlashMessage.Info("Tax Payer Linked Successfully");
                return RedirectToAction("TaxPayerList", "Business", new { id = pObjAssetModel.AssetID, name = pObjAssetModel.AssetName });
            }
        }


        public ActionResult BuildingInformation(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                IList<usp_GetBusinessBuildingList_Result> lstBuildingInformation = new BLBusiness().BL_GetBusinessBuildingList(new MAP_Business_Building() { BusinessID = id.GetValueOrDefault() });
                usp_GetBusinessList_Result mObjBusinessDetails = new BLBusiness().BL_GetBusinessDetails(new Business() { intStatus = 2, BusinessID = id.GetValueOrDefault() });

                if (mObjBusinessDetails != null)
                {
                    ViewBag.BusinessID = id;
                    ViewBag.BusinessRIN = name;
                    ViewBag.BusinessName = mObjBusinessDetails.BusinessName;
                    return View(lstBuildingInformation);
                }
                else
                {
                    return RedirectToAction("List", "Business");
                }
            }
            else
            {
                return RedirectToAction("List", "Business");
            }
        }

        public ActionResult AddBuilding(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                usp_GetBusinessList_Result mObjBusinessDetails = new BLBusiness().BL_GetBusinessDetails(new Business() { intStatus = 2, BusinessID = id.GetValueOrDefault() });

                IList<usp_GetBuildingList_Result> lstBuilding = new BLBuilding().BL_GetBuildingList(new Building() { intStatus = 1 });
                ViewBag.BuildingList = lstBuilding;

                BusinessBuildingViewModel mObjBuildingModel = new BusinessBuildingViewModel()
                {
                    BusinessID = id.GetValueOrDefault(),
                    BusinessRIN = name,
                    BusinessName = mObjBusinessDetails.BusinessName,
                };

                return View(mObjBuildingModel);
            }
            else
            {
                return RedirectToAction("List", "Business");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddBuilding(BusinessBuildingViewModel pObjBuildingModel)
        {
            if (!ModelState.IsValid)
            {
                IList<usp_GetBuildingList_Result> lstBuilding = new BLBuilding().BL_GetBuildingList(new Building() { intStatus = 1 });
                ViewBag.BuildingList = lstBuilding;
                return View(pObjBuildingModel);
            }
            else
            {
                MAP_Business_Building mObjBuilding = new MAP_Business_Building()
                {
                    BusinessID = pObjBuildingModel.BusinessID,
                    BuildingID = pObjBuildingModel.BuildingID,
                    BuildingUnitID = pObjBuildingModel.BuildingUnitID,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                FuncResponse mObjResponse = new BLBusiness().BL_InsertBusinessBuilding(mObjBuilding);

                if (mObjResponse.Success)
                {
                    FlashMessage.Info("Building Information Added Successfully");
                    return RedirectToAction("BuildingInformation", "Business", new { id = pObjBuildingModel.BusinessID, name = pObjBuildingModel.BusinessRIN });
                }
                else
                {
                    IList<usp_GetBuildingList_Result> lstBuilding = new BLBuilding().BL_GetBuildingList(new Building() { intStatus = 1 });
                    ViewBag.BuildingList = lstBuilding;
                    ViewBag.Message = mObjResponse.Message;
                    return View(pObjBuildingModel);
                }
            }
        }

        public JsonResult GetTaxPayerList(MAP_TaxPayer_Asset pObjAssetModel)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            //Get Role Details
            usp_GetTaxPayerRoleList_Result mObjTaxPayerRoleDetails = new BLTaxPayerRole().BL_GetTaxPayerRoleDetails(new TaxPayer_Roles() { intStatus = 2, TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID.GetValueOrDefault() });

            bool blnisAlreadyLinked = false;

            if (!mObjTaxPayerRoleDetails.IsMultiLinkable.GetValueOrDefault())
            {
                FuncResponse mObjFuncResponse = new BLTaxPayerAsset().BL_CheckAssetAlreadyLinked(new MAP_TaxPayer_Asset() { TaxPayerTypeID = pObjAssetModel.TaxPayerTypeID, TaxPayerID = pObjAssetModel.TaxPayerID, AssetTypeID = (int)EnumList.AssetTypes.Business, AssetID = pObjAssetModel.AssetID, TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID });

                if (!mObjFuncResponse.Success)
                {
                    blnisAlreadyLinked = true;
                    dcResponse["success"] = false;
                    dcResponse["Message"] = "Single Role allowed - " + mObjTaxPayerRoleDetails.TaxPayerRoleName + " already exist for this asset";
                }
            }

            if (!blnisAlreadyLinked)
            {
                if (pObjAssetModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
                {
                    Individual mObjIndividual = new Individual()
                    {
                        intStatus = 2
                    };

                    IList<usp_GetIndividualList_Result> lstIndividual = new BLIndividual().BL_GetIndividualList(mObjIndividual);
                    dcResponse["success"] = true;
                    if (!mObjTaxPayerRoleDetails.IsMultiLinkable.GetValueOrDefault())
                    {
                        dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindIndividualTable_SingleSelect", lstIndividual, this.ControllerContext);
                    }
                    else
                    {
                        dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindIndividualTable_MultiSelect", lstIndividual, this.ControllerContext);
                    }
                }
                else if (pObjAssetModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
                {
                    Company mObjCompany = new Company()
                    {
                        intStatus = 2
                    };

                    IList<usp_GetCompanyList_Result> lstCompany = new BLCompany().BL_GetCompanyList(mObjCompany);
                    dcResponse["success"] = true;

                    if (!mObjTaxPayerRoleDetails.IsMultiLinkable.GetValueOrDefault())
                    {
                        dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindCompanyTable_SingleSelect", lstCompany, this.ControllerContext);
                    }
                    else
                    {
                        dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindCompanyTable_MultiSelect", lstCompany, this.ControllerContext);
                    }


                }
                else if (pObjAssetModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
                {
                    Government mObjGovernment = new Government()
                    {
                        intStatus = 2
                    };

                    IList<usp_GetGovernmentList_Result> lstGovernment = new BLGovernment().BL_GetGovernmentList(mObjGovernment);
                    dcResponse["success"] = true;

                    if (!mObjTaxPayerRoleDetails.IsMultiLinkable.GetValueOrDefault())
                    {
                        dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindGovernmentTable_SingleSelect", lstGovernment, this.ControllerContext);
                    }
                    else
                    {
                        dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindGovernmentTable_MultiSelect", lstGovernment, this.ControllerContext);
                    }
                }
                else if (pObjAssetModel.TaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
                {
                    Special mObjSpecial = new Special()
                    {
                        intStatus = 2
                    };

                    IList<usp_GetSpecialList_Result> lstSpecial = new BLSpecial().BL_GetSpecialList(mObjSpecial);
                    dcResponse["success"] = true;

                    if (!mObjTaxPayerRoleDetails.IsMultiLinkable.GetValueOrDefault())
                    {
                        dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindSpecialTable_SingleSelect", lstSpecial, this.ControllerContext);
                    }
                    else
                    {
                        dcResponse["TaxPayerList"] = CommUtil.RenderPartialToString("_BindSpecialTable_MultiSelect", lstSpecial, this.ControllerContext);
                    }
                }
                else
                {
                    dcResponse["success"] = false;
                    dcResponse["Message"] = "Invalid Request";
                }
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateTaxPayerStatus(MAP_TaxPayer_Asset pObjTaxPayerData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjTaxPayerData.TPAID != 0)
            {
                pObjTaxPayerData.AssetTypeID = (int)EnumList.AssetTypes.Business;
                pObjTaxPayerData.IntStatus = 2;
                FuncResponse<IList<usp_GetTaxPayerAssetList_Result>> mObjFuncResponse = new BLTaxPayerAsset().BL_UpdateTaxPayerAssetStatus(pObjTaxPayerData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["AssetList"] = CommUtil.RenderPartialToString("_BindTaxPayerTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveTaxPayer(MAP_TaxPayer_Asset pObjTaxPayerData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjTaxPayerData.TPAID != 0)
            {
                pObjTaxPayerData.AssetTypeID = (int)EnumList.AssetTypes.Business;
                FuncResponse<IList<usp_GetTaxPayerAssetList_Result>> mObjFuncResponse = new BLTaxPayerAsset().BL_RemoveTaxPayerAsset(pObjTaxPayerData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["AssetList"] = CommUtil.RenderPartialToString("_BindTaxPayerTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
                }

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Action";
            }



            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTaxPayerDetails(int TPAID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            MAP_TaxPayer_Asset mObjTaxPayer = new BLTaxPayerAsset().BL_GetTaxPayerAssetDetails(TPAID);

            if (mObjTaxPayer != null)
            {
                dcResponse["TaxPayerTypeID"] = mObjTaxPayer.TaxPayerTypeID;
                dcResponse["success"] = true;
                if (mObjTaxPayer.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
                {
                    dcResponse["TaxPayerDetails"] = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 2, IndividualID = mObjTaxPayer.TaxPayerID.GetValueOrDefault() });
                }
                else if (mObjTaxPayer.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
                {
                    dcResponse["TaxPayerDetails"] = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = mObjTaxPayer.TaxPayerID.GetValueOrDefault() });
                }
                else if (mObjTaxPayer.TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
                {
                    dcResponse["TaxPayerDetails"] = new BLGovernment().BL_GetGovernmentDetails(new Government() { intStatus = 2, GovernmentID = mObjTaxPayer.TaxPayerID.GetValueOrDefault() });
                }
                else if (mObjTaxPayer.TaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
                {
                    dcResponse["TaxPayerDetails"] = new BLSpecial().BL_GetSpecialDetails(new Special() { intStatus = 2, SpecialID = mObjTaxPayer.TaxPayerID.GetValueOrDefault() });
                }
            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Response";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBuildingInformation(int BBID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            MAP_Business_Building mObjBuilding = new BLBusiness().BL_GetBusinessBuildingDetails(BBID);

            if (mObjBuilding != null)
            {
                dcResponse["success"] = true;
                dcResponse["BuildingDetails"] = new BLBuilding().BL_GetBuildingDetails(new Building() { intStatus = 2, BuildingID = mObjBuilding.BuildingID.GetValueOrDefault() });

            }
            else
            {
                dcResponse["success"] = false;
                dcResponse["Message"] = "Invalid Response";
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBuildingList()
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            Building mObjBuilding = new Building()
            {
                intStatus = 1
            };

            IList<usp_GetBuildingList_Result> lstBuilding = new BLBuilding().BL_GetBuildingList(mObjBuilding);
            dcResponse["success"] = true;
            dcResponse["BuildingList"] = CommUtil.RenderPartialToString("_BindBuildingTable_SingleSelect", lstBuilding, this.ControllerContext);
            return Json(dcResponse, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetBuildingUnitList(int BuildingID)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            MAP_Building_BuildingUnit mObjBuildingUnit = new MAP_Building_BuildingUnit()
            {
                BuildingID = BuildingID
            };

            IList<usp_GetBuildingUnitNumberList_Result> lstBuildingUnitNumberList = new BLBuilding().BL_GetBuildingUnitNumberList(mObjBuildingUnit);
            dcResponse["success"] = true;
            dcResponse["BuildingUnitList"] = CommUtil.RenderPartialToString("_BindBuildingUnitTable_SingleSelect", lstBuildingUnitNumberList, this.ControllerContext);
            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult RemoveBuilding(MAP_Business_Building pObjBuildingData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjBuildingData.BBID != 0)
            {
                FuncResponse<IList<usp_GetBusinessBuildingList_Result>> mObjFuncResponse = new BLBusiness().BL_RemoveBusinessBuilding(pObjBuildingData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["BuildingList"] = CommUtil.RenderPartialToString("_BindBuildingTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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