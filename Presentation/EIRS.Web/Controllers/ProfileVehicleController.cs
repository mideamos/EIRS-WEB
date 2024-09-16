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
    public class ProfileVehicleController : BaseController
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
                sbWhereCondition.Append(" AND ( ISNULL(VehicleRIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(VehicleRegNumber,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(VehicleSubTypeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(VehiclePurposeName,'') LIKE @MainFilter )");
            }

            Vehicle mObjVehicle = new Vehicle()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };


            IDictionary<string, object> dcData = new BLVehicle().BL_SearchVehicleForSideMenu(mObjVehicle);
            IList<usp_SearchVehicleForSideMenu_Result> lstVehicle = (IList<usp_SearchVehicleForSideMenu_Result>)dcData["VehicleList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstVehicle
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
                sbWhereCondition.Append(" AND ( ISNULL(VehicleRIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(VehicleRegNumber,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(VehicleSubTypeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(VehiclePurposeName,'') LIKE @MainFilter )");
            }

            Vehicle mObjVehicle = new Vehicle()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };


            IDictionary<string, object> dcData = new BLVehicle().BL_SearchVehicleForSideMenu(mObjVehicle);
            IList<usp_SearchVehicleForSideMenu_Result> lstVehicle = (IList<usp_SearchVehicleForSideMenu_Result>)dcData["VehicleList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstVehicle
            }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ExportData()
        {
            IList<usp_GetVehicleList_Result> lstVehicleData = new BLVehicle().BL_GetVehicleList(new Vehicle() { intStatus = 2 });
            string[] strColumns = new string[] { "VehicleRIN",
                                                "VehicleRegNumber",
                                                "VIN",
                                                "VehicleTypeName",
                                                "VehicleSubTypeName",
                                                "LGAName",
                                                "VehiclePurposeName",
                                                "VehicleFunctionName",
                                                "VehicleOwnershipName",
                                                "VehicleDescription",
                                                "ActiveText" };
            return ExportToExcel(lstVehicleData, this.RouteData, strColumns, "Vehicle");
        }


        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Search(FormCollection pObjCollection)
        {
            string mStrRegNumber = pObjCollection.Get("txtRegNumber");
            string mStrVehicleDescription = pObjCollection.Get("txtVehicleDescription");
            string mStrRIN = pObjCollection.Get("txtRIN");

            Vehicle mObjVehicle = new Vehicle()
            {
                VehicleRIN = mStrRIN,
                VehicleRegNumber = mStrRegNumber,
                VehicleDescription = mStrVehicleDescription,
                intStatus = 1
            };

            IList<usp_GetVehicleList_Result> lstVehicle = new BLVehicle().BL_GetVehicleList(mObjVehicle);
            return PartialView("_BindTable", lstVehicle.Take(5).ToList());
        }

        public void UI_FillDropDown(VehicleViewModel pObjVehicleViewModel = null)
        {
            if (pObjVehicleViewModel != null)
                pObjVehicleViewModel.AssetTypeID = (int)EnumList.AssetTypes.Vehicles;
            else if (pObjVehicleViewModel == null)
                pObjVehicleViewModel = new VehicleViewModel();

            UI_FillLGADropDown(new LGA() { intStatus = 1, IncludeLGAIds = pObjVehicleViewModel.LGAID.ToString() });
            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1, IncludeAssetTypeIds = pObjVehicleViewModel.AssetTypeID.ToString() }, (int)EnumList.AssetTypes.Vehicles);
            UI_FillVehicleTypeDropDown(new Vehicle_Types() { intStatus = 1, IncludeVehicleTypeIds = pObjVehicleViewModel.VehicleTypeID.ToString() });
            UI_FillVehicleSubTypeDropDown(new Vehicle_SubTypes() { intStatus = 1, IncludeVehicleSubTypeIds = pObjVehicleViewModel.VehicleSubTypeID.ToString(), VehicleTypeID = pObjVehicleViewModel.VehicleTypeID });
            UI_FillVehiclePurposeDropDown(new Vehicle_Purpose() { intStatus = 1, IncludeVehiclePurposeIds = pObjVehicleViewModel.VehiclePurposeID.ToString() });
            UI_FillVehicleFunctionDropDown(new Vehicle_Function() { intStatus = 1, IncludeVehicleFunctionIds = pObjVehicleViewModel.VehicleFunctionID.ToString(), VehiclePurposeID = pObjVehicleViewModel.VehiclePurposeID });
            UI_FillVehicleOwnershipDropDown(new Vehicle_Ownership() { intStatus = 1, IncludeVehicleOwnershipIds = pObjVehicleViewModel.VehicleOwnershipID.ToString() });
        }


        public ActionResult Add()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]

        [ValidateAntiForgeryToken()]
        public ActionResult Add(VehicleViewModel pObjVehicleModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjVehicleModel);
                return View(pObjVehicleModel);
            }
            else
            {
                Vehicle mObjVehicle = new Vehicle()
                {
                    VehicleID = 0,
                    VehicleRegNumber = pObjVehicleModel.VehicleRegNumber,
                    VIN = pObjVehicleModel.VIN != null ? pObjVehicleModel.VIN.Trim() : pObjVehicleModel.VIN,
                    AssetTypeID = (int)EnumList.AssetTypes.Vehicles,
                    VehicleTypeID = pObjVehicleModel.VehicleTypeID,
                    VehicleSubTypeID = pObjVehicleModel.VehicleSubTypeID,
                    LGAID = pObjVehicleModel.LGAID,
                    VehiclePurposeID = pObjVehicleModel.VehiclePurposeID,
                    VehicleFunctionID = pObjVehicleModel.VehicleFunctionID,
                    VehicleOwnershipID = pObjVehicleModel.VehicleOwnershipID,
                    VehicleDescription = pObjVehicleModel.VehicleDescription,
                    Active = true,
                    CreatedBy = SessionManager.UserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };


                try
                {

                    FuncResponse<Vehicle> mObjResponse = new BLVehicle().BL_InsertUpdateVehicle(mObjVehicle);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("Details", "ProfileVehicle", new { id = mObjResponse.AdditionalData.VehicleID, name = mObjResponse.AdditionalData.VehicleRegNumber.ToSeoUrl() });
                    }
                    else
                    {
                        UI_FillDropDown(pObjVehicleModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjVehicleModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjVehicleModel);
                    ViewBag.Message = "Error occurred while saving vehicle";
                    return View(pObjVehicleModel);
                }
            }
        }


        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Vehicle mObjVehicle = new Vehicle()
                {
                    VehicleID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetVehicleList_Result mObjVehicleData = new BLVehicle().BL_GetVehicleDetails(mObjVehicle);

                if (mObjVehicleData != null)
                {
                    VehicleViewModel mObjVehicleModelView = new VehicleViewModel()
                    {
                        VehicleID = mObjVehicleData.VehicleID.GetValueOrDefault(),
                        VehicleRIN = mObjVehicleData.VehicleRIN,
                        VehicleRegNumber = mObjVehicleData.VehicleRegNumber,
                        VIN = mObjVehicleData.VIN,
                        AssetTypeID = mObjVehicleData.AssetTypeID.GetValueOrDefault(),
                        VehicleTypeID = mObjVehicleData.VehicleTypeID.GetValueOrDefault(),
                        VehicleSubTypeID = mObjVehicleData.VehicleSubTypeID.GetValueOrDefault(),
                        LGAID = mObjVehicleData.LGAID.GetValueOrDefault(),
                        VehiclePurposeID = mObjVehicleData.VehiclePurposeID.GetValueOrDefault(),
                        VehicleFunctionID = mObjVehicleData.VehicleFunctionID.GetValueOrDefault(),
                        VehicleOwnershipID = mObjVehicleData.VehicleOwnershipID.GetValueOrDefault(),
                        VehicleDescription = mObjVehicleData.VehicleDescription,
                        Active = mObjVehicleData.Active.GetValueOrDefault(),
                    };

                    UI_FillDropDown(mObjVehicleModelView);
                    return View(mObjVehicleModelView);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileVehicle");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileVehicle");
            }
        }

        [HttpPost()]

        [ValidateAntiForgeryToken()]
        public ActionResult Edit(VehicleViewModel pObjVehicleModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjVehicleModel);
                return View(pObjVehicleModel);
            }
            else
            {
                Vehicle mObjVehicle = new Vehicle()
                {
                    VehicleID = pObjVehicleModel.VehicleID,
                    VehicleRegNumber = pObjVehicleModel.VehicleRegNumber,
                    VIN = pObjVehicleModel.VIN != null ? pObjVehicleModel.VIN.ToUpper() : pObjVehicleModel.VIN,
                    AssetTypeID = (int)EnumList.AssetTypes.Vehicles,
                    VehicleTypeID = pObjVehicleModel.VehicleTypeID,
                    VehicleSubTypeID = pObjVehicleModel.VehicleSubTypeID,
                    LGAID = pObjVehicleModel.LGAID,
                    VehiclePurposeID = pObjVehicleModel.VehiclePurposeID,
                    VehicleFunctionID = pObjVehicleModel.VehicleFunctionID,
                    VehicleOwnershipID = pObjVehicleModel.VehicleOwnershipID,
                    VehicleDescription = pObjVehicleModel.VehicleDescription,
                    Active = true,
                    ModifiedBy = SessionManager.UserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };


                try
                {

                    FuncResponse<Vehicle> mObjResponse = new BLVehicle().BL_InsertUpdateVehicle(mObjVehicle);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("Details", "ProfileVehicle", new { id = mObjResponse.AdditionalData.VehicleID, name = mObjResponse.AdditionalData.VehicleRIN.ToSeoUrl() });
                    }
                    else
                    {
                        UI_FillDropDown(pObjVehicleModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjVehicleModel);
                    }
                }
                catch (Exception ex)
                {
                    Logger.SendErrorToText(ex);
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjVehicleModel);
                    ViewBag.Message = "Error occurred while saving vehicle";
                    return View(pObjVehicleModel);
                }
            }
        }


        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Vehicle mObjVehicle = new Vehicle()
                {
                    VehicleID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetVehicleList_Result mObjVehicleData = new BLVehicle().BL_GetVehicleDetails(mObjVehicle);

                if (mObjVehicleData != null)
                {
                    VehicleViewModel mObjVehicleModelView = new VehicleViewModel()
                    {
                        VehicleID = mObjVehicleData.VehicleID.GetValueOrDefault(),
                        VehicleRIN = mObjVehicleData.VehicleRIN,
                        VehicleRegNumber = mObjVehicleData.VehicleRegNumber,
                        VIN = mObjVehicleData.VIN,
                        AssetTypeName = mObjVehicleData.AssetTypeName,
                        VehicleTypeName = mObjVehicleData.VehicleTypeName,
                        VehicleSubTypeName = mObjVehicleData.VehicleSubTypeName,
                        LGAName = mObjVehicleData.LGAName,
                        VehiclePurposeName = mObjVehicleData.VehiclePurposeName,
                        VehicleFunctionName = mObjVehicleData.VehicleFunctionName,
                        VehicleOwnershipName = mObjVehicleData.VehicleOwnershipName,
                        VehicleDescription = mObjVehicleData.VehicleDescription,
                        ActiveText = mObjVehicleData.ActiveText
                    };


                    MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                    {
                        AssetID = id.GetValueOrDefault(),
                        AssetTypeID = (int)EnumList.AssetTypes.Vehicles
                    };

                    IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                    ViewBag.AssetList = lstTaxPayerAsset;

                    return View(mObjVehicleModelView);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileVehicle");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileVehicle");
            }
        }


        public ActionResult SearchIndividual(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Vehicle mObjVehicle = new Vehicle()
                {
                    VehicleID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetVehicleList_Result mObjVehicleData = new BLVehicle().BL_GetVehicleDetails(mObjVehicle);

                if (mObjVehicleData != null)
                {
                    VehicleViewModel mObjVehicleModelView = new VehicleViewModel()
                    {
                        VehicleID = mObjVehicleData.VehicleID.GetValueOrDefault(),
                        VehicleRIN = mObjVehicleData.VehicleRIN,
                        VehicleRegNumber = mObjVehicleData.VehicleRegNumber,
                        VIN = mObjVehicleData.VIN,
                        AssetTypeName = mObjVehicleData.AssetTypeName,
                        VehicleTypeName = mObjVehicleData.VehicleTypeName,
                        VehicleSubTypeName = mObjVehicleData.VehicleSubTypeName,
                        LGAName = mObjVehicleData.LGAName,
                        VehiclePurposeName = mObjVehicleData.VehiclePurposeName,
                        VehicleFunctionName = mObjVehicleData.VehicleFunctionName,
                        VehicleOwnershipName = mObjVehicleData.VehicleOwnershipName,
                        VehicleDescription = mObjVehicleData.VehicleDescription,
                        ActiveText = mObjVehicleData.ActiveText
                    };


                    return View(mObjVehicleModelView);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileVehicle");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileVehicle");
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
                Vehicle mObjVehicle = new Vehicle()
                {
                    VehicleID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetVehicleList_Result mObjVehicleData = new BLVehicle().BL_GetVehicleDetails(mObjVehicle);

                if (mObjVehicleData != null)
                {
                    VehicleViewModel mObjVehicleModelView = new VehicleViewModel()
                    {
                        VehicleID = mObjVehicleData.VehicleID.GetValueOrDefault(),
                        VehicleRIN = mObjVehicleData.VehicleRIN,
                        VehicleRegNumber = mObjVehicleData.VehicleRegNumber,
                        VIN = mObjVehicleData.VIN,
                        AssetTypeName = mObjVehicleData.AssetTypeName,
                        VehicleTypeName = mObjVehicleData.VehicleTypeName,
                        VehicleSubTypeName = mObjVehicleData.VehicleSubTypeName,
                        LGAName = mObjVehicleData.LGAName,
                        VehiclePurposeName = mObjVehicleData.VehiclePurposeName,
                        VehicleFunctionName = mObjVehicleData.VehicleFunctionName,
                        VehicleOwnershipName = mObjVehicleData.VehicleOwnershipName,
                        VehicleDescription = mObjVehicleData.VehicleDescription,
                        ActiveText = mObjVehicleData.ActiveText
                    };


                    return View(mObjVehicleModelView);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileVehicle");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileVehicle");
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
                Vehicle mObjVehicle = new Vehicle()
                {
                    VehicleID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetVehicleList_Result mObjVehicleData = new BLVehicle().BL_GetVehicleDetails(mObjVehicle);

                if (mObjVehicleData != null)
                {
                    VehicleViewModel mObjVehicleModelView = new VehicleViewModel()
                    {
                        VehicleID = mObjVehicleData.VehicleID.GetValueOrDefault(),
                        VehicleRIN = mObjVehicleData.VehicleRIN,
                        VehicleRegNumber = mObjVehicleData.VehicleRegNumber,
                        VIN = mObjVehicleData.VIN,
                        AssetTypeName = mObjVehicleData.AssetTypeName,
                        VehicleTypeName = mObjVehicleData.VehicleTypeName,
                        VehicleSubTypeName = mObjVehicleData.VehicleSubTypeName,
                        LGAName = mObjVehicleData.LGAName,
                        VehiclePurposeName = mObjVehicleData.VehiclePurposeName,
                        VehicleFunctionName = mObjVehicleData.VehicleFunctionName,
                        VehicleOwnershipName = mObjVehicleData.VehicleOwnershipName,
                        VehicleDescription = mObjVehicleData.VehicleDescription,
                        ActiveText = mObjVehicleData.ActiveText
                    };


                    return View(mObjVehicleModelView);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileVehicle");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileVehicle");
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
                Vehicle mObjVehicle = new Vehicle()
                {
                    VehicleID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetVehicleList_Result mObjVehicleData = new BLVehicle().BL_GetVehicleDetails(mObjVehicle);

                if (mObjVehicleData != null)
                {
                    VehicleViewModel mObjVehicleModelView = new VehicleViewModel()
                    {
                        VehicleID = mObjVehicleData.VehicleID.GetValueOrDefault(),
                        VehicleRIN = mObjVehicleData.VehicleRIN,
                        VehicleRegNumber = mObjVehicleData.VehicleRegNumber,
                        VIN = mObjVehicleData.VIN,
                        AssetTypeName = mObjVehicleData.AssetTypeName,
                        VehicleTypeName = mObjVehicleData.VehicleTypeName,
                        VehicleSubTypeName = mObjVehicleData.VehicleSubTypeName,
                        LGAName = mObjVehicleData.LGAName,
                        VehiclePurposeName = mObjVehicleData.VehiclePurposeName,
                        VehicleFunctionName = mObjVehicleData.VehicleFunctionName,
                        VehicleOwnershipName = mObjVehicleData.VehicleOwnershipName,
                        VehicleDescription = mObjVehicleData.VehicleDescription,
                        ActiveText = mObjVehicleData.ActiveText
                    };


                    return View(mObjVehicleModelView);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileVehicle");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileVehicle");
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

            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual, AssetTypeID = (int)EnumList.AssetTypes.Vehicles });
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
                Vehicle mObjVehicle = new Vehicle()
                {
                    VehicleID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetVehicleList_Result mObjVehicleData = new BLVehicle().BL_GetVehicleDetails(mObjVehicle);

                if (mObjVehicleData != null)
                {
                    TPIndividualViewModel mObjIndividualModel = new TPIndividualViewModel()
                    {
                        AssetID = mObjVehicleData.VehicleID.GetValueOrDefault(),
                        AssetName = mObjVehicleData.VehicleRegNumber,
                        AssetRIN = mObjVehicleData.VehicleRIN,
                        AssetLGAName = mObjVehicleData.LGAName,
                        AssetTypeID = (int)EnumList.AssetTypes.Vehicles,
                        AssetTypeName = mObjVehicleData.AssetTypeName,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                    };

                    UI_FillIndividualDropDown();
                    return View(mObjIndividualModel);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileVehicle");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileVehicle");
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

                            //Creating mapping between individual and Vehicle
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
                                return RedirectToAction("Details", "ProfileVehicle", new { id = pObjIndividualModel.AssetID, name = pObjIndividualModel.AssetRIN });
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

            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies, AssetTypeID = (int)EnumList.AssetTypes.Vehicles });
            UI_FillTaxOfficeDropDown(new Tax_Offices() { intStatus = 1, IncludeTaxOfficeIds = pObjCompanyViewModel.TaxOfficeID.ToString() });
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1, IncludeTaxPayerTypeIds = pObjCompanyViewModel.TaxPayerTypeID.ToString() }, (int)EnumList.TaxPayerType.Companies);
            UI_FillEconomicActivitiesDropDown(new Economic_Activities() { intStatus = 1, IncludeEconomicActivitiesIds = pObjCompanyViewModel.EconomicActivitiesID.ToString(), TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies });
            UI_FillNotificationMethodDropDown(new Notification_Method() { intStatus = 1, IncludeNotificationMethodIds = pObjCompanyViewModel.NotificationMethodID.ToString() });
        }


        public ActionResult AddCorporate(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Vehicle mObjVehicle = new Vehicle()
                {
                    VehicleID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetVehicleList_Result mObjVehicleData = new BLVehicle().BL_GetVehicleDetails(mObjVehicle);

                if (mObjVehicleData != null)
                {
                    TPCompanyViewModel mObjCompanyModel = new TPCompanyViewModel()
                    {
                        AssetID = mObjVehicleData.VehicleID.GetValueOrDefault(),
                        AssetName = mObjVehicleData.VehicleRegNumber,
                        AssetRIN = mObjVehicleData.VehicleRIN,
                        AssetLGAName = mObjVehicleData.LGAName,
                        AssetTypeID = (int)EnumList.AssetTypes.Vehicles,
                        AssetTypeName = mObjVehicleData.AssetTypeName,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                    };

                    UI_FillCompanyDropDown();
                    return View(mObjCompanyModel);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileVehicle");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileVehicle");
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
                            //Creating mapping between individual and Vehicle
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
                                return RedirectToAction("Details", "ProfileVehicle", new { id = pObjCompanyModel.AssetID, name = pObjCompanyModel.AssetRIN });
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
            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual, AssetTypeID = (int)EnumList.AssetTypes.Vehicles });
        }


        public ActionResult AddGovernment(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Vehicle mObjVehicle = new Vehicle()
                {
                    VehicleID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetVehicleList_Result mObjVehicleData = new BLVehicle().BL_GetVehicleDetails(mObjVehicle);

                if (mObjVehicleData != null)
                {
                    TPGovernmentViewModel mObjGovernmentModel = new TPGovernmentViewModel()
                    {
                        AssetID = mObjVehicleData.VehicleID.GetValueOrDefault(),
                        AssetName = mObjVehicleData.VehicleRegNumber,
                        AssetRIN = mObjVehicleData.VehicleRIN,
                        AssetLGAName = mObjVehicleData.LGAName,
                        AssetTypeID = (int)EnumList.AssetTypes.Vehicles,
                        AssetTypeName = mObjVehicleData.AssetTypeName,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                    };

                    UI_FillGovernmentDropDown();
                    return View(mObjGovernmentModel);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileVehicle");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileVehicle");
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
                            //Creating mapping between individual and Vehicle
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
                                return RedirectToAction("Details", "ProfileVehicle", new { id = pObjGovernmentModel.AssetID, name = pObjGovernmentModel.AssetRIN });
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
            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual, AssetTypeID = (int)EnumList.AssetTypes.Vehicles });
        }


        public ActionResult AddSpecial(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Vehicle mObjVehicle = new Vehicle()
                {
                    VehicleID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetVehicleList_Result mObjVehicleData = new BLVehicle().BL_GetVehicleDetails(mObjVehicle);

                if (mObjVehicleData != null)
                {
                    TPSpecialViewModel mObjSpecialModel = new TPSpecialViewModel()
                    {
                        AssetID = mObjVehicleData.VehicleID.GetValueOrDefault(),
                        AssetName = mObjVehicleData.VehicleRegNumber,
                        AssetRIN = mObjVehicleData.VehicleRIN,
                        AssetLGAName = mObjVehicleData.LGAName,
                        AssetTypeID = (int)EnumList.AssetTypes.Vehicles,
                        AssetTypeName = mObjVehicleData.AssetTypeName,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Special,
                    };

                    UI_FillSpecialDropDown();
                    return View(mObjSpecialModel);
                }
                else
                {
                    return RedirectToAction("Search", "ProfileVehicle");
                }
            }
            else
            {
                return RedirectToAction("Search", "ProfileVehicle");
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
                            //Creating mapping between individual and Vehicle
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
                                return RedirectToAction("Details", "ProfileVehicle", new { id = pObjSpecialModel.AssetID, name = pObjSpecialModel.AssetRIN });
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

        public JsonResult UpdateStatus(Vehicle pObjVehicleData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjVehicleData.VehicleID != 0)
            {
                FuncResponse mObjFuncResponse = new BLVehicle().BL_UpdateStatus(pObjVehicleData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["VehicleList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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