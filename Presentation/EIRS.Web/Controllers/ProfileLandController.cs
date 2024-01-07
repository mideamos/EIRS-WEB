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
    public class ProfileLandController : BaseController
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
                sbWhereCondition.Append(" AND ( ISNULL(LandRIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(PlotNumber,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(LandFunctionName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(LandOccupier,'') LIKE @MainFilter )");
            }

            Land mObjLand = new Land()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };


            IDictionary<string, object> dcData = new BLLand().BL_SearchLandForSideMenu(mObjLand);
            IList<usp_SearchLandForSideMenu_Result> lstLand = (IList<usp_SearchLandForSideMenu_Result>)dcData["LandList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstLand
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
                sbWhereCondition.Append(" AND ( ISNULL(LandRIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(PlotNumber,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(LandFunctionName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(LandOccupier,'') LIKE @MainFilter )");
            }

            Land mObjLand = new Land()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };


            IDictionary<string, object> dcData = new BLLand().BL_SearchLandForSideMenu(mObjLand);
            IList<usp_SearchLandForSideMenu_Result> lstLand = (IList<usp_SearchLandForSideMenu_Result>)dcData["LandList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstLand
            }, JsonRequestBehavior.AllowGet);
        }

        
        [HttpGet]
        public ActionResult ExportData()
        {
            IList<usp_GetLandList_Result> lstLandData = new BLLand().BL_GetLandList(new Land() { intStatus = 2 });
            string[] strColumns = new string[] { "LandRIN",
                                                "PlotNumber",
                                                "StreetName",
                                                "TownName",
                                                "LGAName",
                                                "WardName",
                                                "LandSize_Length",
                                                "LandSize_Width",
                                                "C_OF_O_Ref",
                                                "LandPurposeName",
                                                "LandFunctionName",
                                                "LandOwnershipName",
                                                "LandDevelopmentName",
                                                "Latitude",
                                                "Longitude",
                                                "ValueOfLand",
                                                "LandStreetConditionName",
                                                "LandOccupier",
                                                "Neighborhood",
                                                "ActiveText" };
            return ExportToExcel(lstLandData, this.RouteData, strColumns, "Land");
        }

        
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        
        public ActionResult Search(FormCollection pObjCollection)
        {
            string mStrPlotNumber = pObjCollection.Get("txtPlotNumber");
            string mStrOccupierName = pObjCollection.Get("txtLandOccupier");
            string mStrRIN = pObjCollection.Get("txtRIN");

            Land mObjLand = new Land()
            {
                LandRIN = mStrRIN,
                PlotNumber = mStrPlotNumber,
                LandOccupier = mStrOccupierName,
                intStatus = 1
            };

            IList<usp_GetLandList_Result> lstLand = new BLLand().BL_GetLandList(mObjLand);
            return PartialView("_BindTable", lstLand.Take(5).ToList());
        }

        public void UI_FillDropDown(LandViewModel pObjLandViewModel = null)
        {
            if (pObjLandViewModel != null)
                pObjLandViewModel.AssetTypeID = (int)EnumList.AssetTypes.Land;
            else if (pObjLandViewModel == null)
                pObjLandViewModel = new LandViewModel();

            UI_FillTownDropDown(new Town() { intStatus = 1, IncludeTownIds = pObjLandViewModel.TownID.ToString() });
            UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = pObjLandViewModel.LGAID.ToString() });
            UI_FillWardDropDown(new Ward() { intStatus = 1, LGAID = pObjLandViewModel.LGAID, IncludeWardIds = pObjLandViewModel.WardID.ToString() });
            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjLandViewModel.AssetTypeID.ToString() }, (int)EnumList.AssetTypes.Land);
            UI_FillLandPurposeDropDown(new Land_Purpose() { intStatus = 1, IncludeLandPurposeIds = pObjLandViewModel.LandPurposeID.ToString() });
            UI_FillLandFunctionDropDown(new Land_Function() { intStatus = 1, IncludeLandFunctionIds = pObjLandViewModel.LandFunctionID.ToString(), LandPurposeID = pObjLandViewModel.LandPurposeID });
            UI_FillLandDevelopmentDropDown(new Land_Development() { intStatus = 1, IncludeLandDevelopmentIds = pObjLandViewModel.LandDevelopmentID.ToString() });
            UI_FillLandOwnershipDropDown(new Land_Ownership() { intStatus = 1, IncludeLandOwnershipIds = pObjLandViewModel.LandOwnershipID.ToString() });
            UI_FillLandStreetConditionDropDown(new Land_StreetCondition() { intStatus = 1, IncludeLandStreetConditionIds = pObjLandViewModel.LandStreetConditionID.ToString() });
        }

        
        public ActionResult Add()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]
        
        [ValidateAntiForgeryToken()]
        public ActionResult Add(LandViewModel pObjLandModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjLandModel);
                return View(pObjLandModel);
            }
            else
            {
                Land mObjLand = new Land()
                {
                    LandID = 0,
                    PlotNumber = pObjLandModel.PlotNumber,
                    StreetName = pObjLandModel.StreetName,
                    TownID = pObjLandModel.TownID,
                    LGAID = pObjLandModel.LGAID,
                    WardID = pObjLandModel.WardID,
                    AssetTypeID = (int)EnumList.AssetTypes.Land,
                    LandSize_Length = pObjLandModel.LandSize_Length,
                    LandSize_Width = pObjLandModel.LandSize_Width,
                    C_OF_O_Ref = pObjLandModel.C_OF_O_Ref,
                    LandPurposeID = pObjLandModel.LandPurposeID,
                    LandFunctionID = pObjLandModel.LandFunctionID,
                    LandOwnershipID = pObjLandModel.LandOwnershipID,
                    LandDevelopmentID = pObjLandModel.LandDevelopmentID,
                    Latitude = pObjLandModel.Latitude,
                    Longitude = pObjLandModel.Longitude,
                    ValueOfLand = pObjLandModel.ValueOfLand,
                    LandStreetConditionID = pObjLandModel.LandStreetConditionID,
                    Neighborhood = pObjLandModel.Neighborhood,
                    Active = true,
                    CreatedBy = SessionManager.UserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };


                try
                {

                    FuncResponse<Land> mObjResponse = new BLLand().BL_InsertUpdateLand(mObjLand);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("Details", "ProfileLand", new { id = mObjResponse.AdditionalData.LandID, name = mObjResponse.AdditionalData.Neighborhood.ToSeoUrl() });
                    }
                    else
                    {
                        UI_FillDropDown(pObjLandModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjLandModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjLandModel);
                    ViewBag.Message = "Error occurred while saving land";
                    return View(pObjLandModel);
                }
            }
        }

        
        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Land mObjLand = new Land()
                {
                    LandID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLandList_Result mObjLandData = new BLLand().BL_GetLandDetails(mObjLand);

                if (mObjLandData != null)
                {
                    LandViewModel mObjLandModelView = new LandViewModel()
                    {
                        LandID = mObjLandData.LandID.GetValueOrDefault(),
                        LandRIN = mObjLandData.LandRIN,
                        PlotNumber = mObjLandData.PlotNumber,
                        StreetName = mObjLandData.StreetName,
                        TownID = mObjLandData.TownID.GetValueOrDefault(),
                        LGAID = mObjLandData.LGAID.GetValueOrDefault(),
                        WardID = mObjLandData.WardID.GetValueOrDefault(),
                        AssetTypeID = mObjLandData.AssetTypeID.GetValueOrDefault(),
                        LandSize_Length = mObjLandData.LandSize_Length,
                        LandSize_Width = mObjLandData.LandSize_Width,
                        C_OF_O_Ref = mObjLandData.C_OF_O_Ref,
                        LandPurposeID = mObjLandData.LandPurposeID.GetValueOrDefault(),
                        LandFunctionID = mObjLandData.LandFunctionID.GetValueOrDefault(),
                        LandOwnershipID = mObjLandData.LandOwnershipID.GetValueOrDefault(),
                        LandDevelopmentID = mObjLandData.LandDevelopmentID.GetValueOrDefault(),
                        Latitude = mObjLandData.Latitude,
                        Longitude = mObjLandData.Longitude,
                        ValueOfLand = mObjLandData.ValueOfLand.GetValueOrDefault(),
                        LandStreetConditionID = mObjLandData.LandStreetConditionID.GetValueOrDefault(),
                        Neighborhood = mObjLandData.Neighborhood,
                        Active = mObjLandData.Active.GetValueOrDefault(),
                    };

                    UI_FillDropDown(mObjLandModelView);
                    return View(mObjLandModelView);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileLand");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileLand");
            }
        }

        [HttpPost()]
        
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(LandViewModel pObjLandModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjLandModel);
                return View(pObjLandModel);
            }
            else
            {
                Land mObjLand = new Land()
                {
                    LandID = pObjLandModel.LandID,
                    PlotNumber = pObjLandModel.PlotNumber,
                    StreetName = pObjLandModel.StreetName,
                    TownID = pObjLandModel.TownID,
                    LGAID = pObjLandModel.LGAID,
                    WardID = pObjLandModel.WardID,
                    AssetTypeID = (int)EnumList.AssetTypes.Land,
                    LandSize_Length = pObjLandModel.LandSize_Length,
                    LandSize_Width = pObjLandModel.LandSize_Width,
                    C_OF_O_Ref = pObjLandModel.C_OF_O_Ref,
                    LandPurposeID = pObjLandModel.LandPurposeID,
                    LandFunctionID = pObjLandModel.LandFunctionID,
                    LandOwnershipID = pObjLandModel.LandOwnershipID,
                    LandDevelopmentID = pObjLandModel.LandDevelopmentID,
                    Latitude = pObjLandModel.Latitude,
                    Longitude = pObjLandModel.Longitude,
                    ValueOfLand = pObjLandModel.ValueOfLand,
                    LandStreetConditionID = pObjLandModel.LandStreetConditionID,
                    Neighborhood = pObjLandModel.Neighborhood,
                    Active = true,
                    ModifiedBy = SessionManager.UserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };


                try
                {

                    FuncResponse<Land> mObjResponse = new BLLand().BL_InsertUpdateLand(mObjLand);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("Details", "ProfileLand", new { id = mObjResponse.AdditionalData.LandID, name = mObjResponse.AdditionalData.LandRIN.ToSeoUrl() });
                    }
                    else
                    {
                        UI_FillDropDown(pObjLandModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjLandModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjLandModel);
                    ViewBag.Message = "Error occurred while saving land";
                    return View(pObjLandModel);
                }
            }
        }

        
        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Land mObjLand = new Land()
                {
                    LandID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLandList_Result mObjLandData = new BLLand().BL_GetLandDetails(mObjLand);

                if (mObjLandData != null)
                {
                    LandViewModel mObjLandModelView = new LandViewModel()
                    {
                        LandID = mObjLandData.LandID.GetValueOrDefault(),
                        LandRIN = mObjLandData.LandRIN,
                        PlotNumber = mObjLandData.PlotNumber,
                        StreetName = mObjLandData.StreetName,
                        TownName = mObjLandData.TownName,
                        LGAName = mObjLandData.LGAName,
                        WardName = mObjLandData.WardName,
                        AssetTypeName = mObjLandData.AssetTypeName,
                        LandSize_Length = mObjLandData.LandSize_Length,
                        LandSize_Width = mObjLandData.LandSize_Width,
                        C_OF_O_Ref = mObjLandData.C_OF_O_Ref,
                        LandPurposeName = mObjLandData.LandPurposeName,
                        LandFunctionName = mObjLandData.LandFunctionName,
                        LandOwnershipName = mObjLandData.LandOwnershipName,
                        LandDevelopmentName = mObjLandData.LandDevelopmentName,
                        Latitude = mObjLandData.Latitude,
                        Longitude = mObjLandData.Longitude,
                        ValueOfLand = mObjLandData.ValueOfLand.GetValueOrDefault(),
                        LandStreetConditionName = mObjLandData.LandStreetConditionName,
                        Neighborhood = mObjLandData.Neighborhood,
                        ActiveText = mObjLandData.ActiveText
                    };


                    MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                    {
                        AssetID = id.GetValueOrDefault(),
                        AssetTypeID = (int)EnumList.AssetTypes.Land
                    };

                    IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                    ViewBag.AssetList = lstTaxPayerAsset;

                    return View(mObjLandModelView);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileLand");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileLand");
            }
        }

        
        public ActionResult SearchIndividual(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Land mObjLand = new Land()
                {
                    LandID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLandList_Result mObjLandData = new BLLand().BL_GetLandDetails(mObjLand);

                if (mObjLandData != null)
                {
                    LandViewModel mObjLandModelView = new LandViewModel()
                    {
                        LandID = mObjLandData.LandID.GetValueOrDefault(),
                        LandRIN = mObjLandData.LandRIN,
                        PlotNumber = mObjLandData.PlotNumber,
                        StreetName = mObjLandData.StreetName,
                        TownName = mObjLandData.TownName,
                        LGAName = mObjLandData.LGAName,
                        WardName = mObjLandData.WardName,
                        AssetTypeName = mObjLandData.AssetTypeName,
                        LandSize_Length = mObjLandData.LandSize_Length,
                        LandSize_Width = mObjLandData.LandSize_Width,
                        C_OF_O_Ref = mObjLandData.C_OF_O_Ref,
                        LandPurposeName = mObjLandData.LandPurposeName,
                        LandFunctionName = mObjLandData.LandFunctionName,
                        LandOwnershipName = mObjLandData.LandOwnershipName,
                        LandDevelopmentName = mObjLandData.LandDevelopmentName,
                        Latitude = mObjLandData.Latitude,
                        Longitude = mObjLandData.Longitude,
                        ValueOfLand = mObjLandData.ValueOfLand.GetValueOrDefault(),
                        LandStreetConditionName = mObjLandData.LandStreetConditionName,
                        Neighborhood = mObjLandData.Neighborhood,
                        ActiveText = mObjLandData.ActiveText
                    };


                    return View(mObjLandModelView);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileLand");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileLand");
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
                Land mObjLand = new Land()
                {
                    LandID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLandList_Result mObjLandData = new BLLand().BL_GetLandDetails(mObjLand);

                if (mObjLandData != null)
                {
                    LandViewModel mObjLandModelView = new LandViewModel()
                    {
                        LandID = mObjLandData.LandID.GetValueOrDefault(),
                        LandRIN = mObjLandData.LandRIN,
                        PlotNumber = mObjLandData.PlotNumber,
                        StreetName = mObjLandData.StreetName,
                        TownName = mObjLandData.TownName,
                        LGAName = mObjLandData.LGAName,
                        WardName = mObjLandData.WardName,
                        AssetTypeName = mObjLandData.AssetTypeName,
                        LandSize_Length = mObjLandData.LandSize_Length,
                        LandSize_Width = mObjLandData.LandSize_Width,
                        C_OF_O_Ref = mObjLandData.C_OF_O_Ref,
                        LandPurposeName = mObjLandData.LandPurposeName,
                        LandFunctionName = mObjLandData.LandFunctionName,
                        LandOwnershipName = mObjLandData.LandOwnershipName,
                        LandDevelopmentName = mObjLandData.LandDevelopmentName,
                        Latitude = mObjLandData.Latitude,
                        Longitude = mObjLandData.Longitude,
                        ValueOfLand = mObjLandData.ValueOfLand.GetValueOrDefault(),
                        LandStreetConditionName = mObjLandData.LandStreetConditionName,
                        Neighborhood = mObjLandData.Neighborhood,
                        ActiveText = mObjLandData.ActiveText
                    };


                    return View(mObjLandModelView);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileLand");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileLand");
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
                Land mObjLand = new Land()
                {
                    LandID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLandList_Result mObjLandData = new BLLand().BL_GetLandDetails(mObjLand);

                if (mObjLandData != null)
                {
                    LandViewModel mObjLandModelView = new LandViewModel()
                    {
                        LandID = mObjLandData.LandID.GetValueOrDefault(),
                        LandRIN = mObjLandData.LandRIN,
                        PlotNumber = mObjLandData.PlotNumber,
                        StreetName = mObjLandData.StreetName,
                        TownName = mObjLandData.TownName,
                        LGAName = mObjLandData.LGAName,
                        WardName = mObjLandData.WardName,
                        AssetTypeName = mObjLandData.AssetTypeName,
                        LandSize_Length = mObjLandData.LandSize_Length,
                        LandSize_Width = mObjLandData.LandSize_Width,
                        C_OF_O_Ref = mObjLandData.C_OF_O_Ref,
                        LandPurposeName = mObjLandData.LandPurposeName,
                        LandFunctionName = mObjLandData.LandFunctionName,
                        LandOwnershipName = mObjLandData.LandOwnershipName,
                        LandDevelopmentName = mObjLandData.LandDevelopmentName,
                        Latitude = mObjLandData.Latitude,
                        Longitude = mObjLandData.Longitude,
                        ValueOfLand = mObjLandData.ValueOfLand.GetValueOrDefault(),
                        LandStreetConditionName = mObjLandData.LandStreetConditionName,
                        Neighborhood = mObjLandData.Neighborhood,
                        ActiveText = mObjLandData.ActiveText
                    };


                    return View(mObjLandModelView);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileLand");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileLand");
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
                Land mObjLand = new Land()
                {
                    LandID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLandList_Result mObjLandData = new BLLand().BL_GetLandDetails(mObjLand);

                if (mObjLandData != null)
                {
                    LandViewModel mObjLandModelView = new LandViewModel()
                    {
                        LandID = mObjLandData.LandID.GetValueOrDefault(),
                        LandRIN = mObjLandData.LandRIN,
                        PlotNumber = mObjLandData.PlotNumber,
                        StreetName = mObjLandData.StreetName,
                        TownName = mObjLandData.TownName,
                        LGAName = mObjLandData.LGAName,
                        WardName = mObjLandData.WardName,
                        AssetTypeName = mObjLandData.AssetTypeName,
                        LandSize_Length = mObjLandData.LandSize_Length,
                        LandSize_Width = mObjLandData.LandSize_Width,
                        C_OF_O_Ref = mObjLandData.C_OF_O_Ref,
                        LandPurposeName = mObjLandData.LandPurposeName,
                        LandFunctionName = mObjLandData.LandFunctionName,
                        LandOwnershipName = mObjLandData.LandOwnershipName,
                        LandDevelopmentName = mObjLandData.LandDevelopmentName,
                        Latitude = mObjLandData.Latitude,
                        Longitude = mObjLandData.Longitude,
                        ValueOfLand = mObjLandData.ValueOfLand.GetValueOrDefault(),
                        LandStreetConditionName = mObjLandData.LandStreetConditionName,
                        Neighborhood = mObjLandData.Neighborhood,
                        ActiveText = mObjLandData.ActiveText
                    };


                    return View(mObjLandModelView);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileLand");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileLand");
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

            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual, AssetTypeID = (int)EnumList.AssetTypes.Land });
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
                Land mObjLand = new Land()
                {
                    LandID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLandList_Result mObjLandData = new BLLand().BL_GetLandDetails(mObjLand);

                if (mObjLandData != null)
                {
                    TPIndividualViewModel mObjIndividualModel = new TPIndividualViewModel()
                    {
                        AssetID = mObjLandData.LandID.GetValueOrDefault(),
                        AssetName = mObjLandData.C_OF_O_Ref,
                        AssetRIN = mObjLandData.LandRIN,
                        AssetLGAName = mObjLandData.LGAName,
                        AssetTypeID = (int)EnumList.AssetTypes.Land,
                        AssetTypeName = mObjLandData.AssetTypeName,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                    };

                    UI_FillIndividualDropDown();
                    return View(mObjIndividualModel);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileLand");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileLand");
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

                            //Creating mapping between individual and Land
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
                                return RedirectToAction("Details", "ProfileLand", new { id = pObjIndividualModel.AssetID, name = pObjIndividualModel.AssetRIN });
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

            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies, AssetTypeID = (int)EnumList.AssetTypes.Land });
            UI_FillTaxOfficeDropDown(new Tax_Offices() { intStatus = 1, IncludeTaxOfficeIds = pObjCompanyViewModel.TaxOfficeID.ToString() });
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1, IncludeTaxPayerTypeIds = pObjCompanyViewModel.TaxPayerTypeID.ToString() }, (int)EnumList.TaxPayerType.Companies);
            UI_FillEconomicActivitiesDropDown(new Economic_Activities() { intStatus = 1, IncludeEconomicActivitiesIds = pObjCompanyViewModel.EconomicActivitiesID.ToString(), TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies });
            UI_FillNotificationMethodDropDown(new Notification_Method() { intStatus = 1, IncludeNotificationMethodIds = pObjCompanyViewModel.NotificationMethodID.ToString() });
        }

        
        public ActionResult AddCorporate(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Land mObjLand = new Land()
                {
                    LandID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLandList_Result mObjLandData = new BLLand().BL_GetLandDetails(mObjLand);

                if (mObjLandData != null)
                {
                    TPCompanyViewModel mObjCompanyModel = new TPCompanyViewModel()
                    {
                        AssetID = mObjLandData.LandID.GetValueOrDefault(),
                        AssetName = mObjLandData.C_OF_O_Ref,
                        AssetRIN = mObjLandData.LandRIN,
                        AssetLGAName = mObjLandData.LGAName,
                        AssetTypeID = (int)EnumList.AssetTypes.Land,
                        AssetTypeName = mObjLandData.AssetTypeName,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                    };

                    UI_FillCompanyDropDown();
                    return View(mObjCompanyModel);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileLand");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileLand");
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
                            //Creating mapping between individual and Land
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
                                return RedirectToAction("Details", "ProfileLand", new { id = pObjCompanyModel.AssetID, name = pObjCompanyModel.AssetRIN });
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
            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual, AssetTypeID = (int)EnumList.AssetTypes.Land });
        }

        
        public ActionResult AddGovernment(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Land mObjLand = new Land()
                {
                    LandID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLandList_Result mObjLandData = new BLLand().BL_GetLandDetails(mObjLand);

                if (mObjLandData != null)
                {
                    TPGovernmentViewModel mObjGovernmentModel = new TPGovernmentViewModel()
                    {
                        AssetID = mObjLandData.LandID.GetValueOrDefault(),
                        AssetName = mObjLandData.C_OF_O_Ref,
                        AssetRIN = mObjLandData.LandRIN,
                        AssetLGAName = mObjLandData.LGAName,
                        AssetTypeID = (int)EnumList.AssetTypes.Land,
                        AssetTypeName = mObjLandData.AssetTypeName,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                    };

                    UI_FillGovernmentDropDown();
                    return View(mObjGovernmentModel);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileLand");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileLand");
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
                            //Creating mapping between individual and Land
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
                                return RedirectToAction("Details", "ProfileLand", new { id = pObjGovernmentModel.AssetID, name = pObjGovernmentModel.AssetRIN });
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
            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual, AssetTypeID = (int)EnumList.AssetTypes.Land });
        }

        
        public ActionResult AddSpecial(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Land mObjLand = new Land()
                {
                    LandID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetLandList_Result mObjLandData = new BLLand().BL_GetLandDetails(mObjLand);

                if (mObjLandData != null)
                {
                    TPSpecialViewModel mObjSpecialModel = new TPSpecialViewModel()
                    {
                        AssetID = mObjLandData.LandID.GetValueOrDefault(),
                        AssetName = mObjLandData.C_OF_O_Ref,
                        AssetRIN = mObjLandData.LandRIN,
                        AssetLGAName = mObjLandData.LGAName,
                        AssetTypeID = (int)EnumList.AssetTypes.Land,
                        AssetTypeName = mObjLandData.AssetTypeName,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Special,
                    };

                    UI_FillSpecialDropDown();
                    return View(mObjSpecialModel);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileLand");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileLand");
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
                            //Creating mapping between individual and Land
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
                                return RedirectToAction("Details", "ProfileLand", new { id = pObjSpecialModel.AssetID, name = pObjSpecialModel.AssetRIN });
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

        public JsonResult UpdateStatus(Land pObjLandData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjLandData.LandID != 0)
            {
                FuncResponse mObjFuncResponse = new BLLand().BL_UpdateStatus(pObjLandData);
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