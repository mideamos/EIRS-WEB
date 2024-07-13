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
    public class VehicleController : BaseController
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

            if (!string.IsNullOrWhiteSpace(Request.Form["VehicleRIN"]))
            {
                sbWhereCondition.Append(" AND ISNULL(VehicleRIN,'') LIKE @VehicleRIN");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["VehicleRegNumber"]))
            {
                sbWhereCondition.Append(" AND ISNULL(VehicleRegNumber,'') LIKE @VehicleRegNumber");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["VIN"]))
            {
                sbWhereCondition.Append(" AND ISNULL(VIN,'') LIKE @VIN");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["VehicleTypeName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(VehicleTypeName,'') LIKE @VehicleTypeName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["VehicleSubTypeName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(VehicleSubTypeName,'') LIKE @VehicleSubTypeName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["LGAName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(LGAName,'') LIKE @LGAName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["VehiclePurposeName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(VehiclePurposeName,'') LIKE @VehiclePurposeName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["VehicleFunctionName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(VehicleFunctionName,'') LIKE @VehicleFunctionName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["VehicleOwnershipName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(VehicleOwnershipName,'') LIKE @VehicleOwnershipName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["VehicleDescription"]))
            {
                sbWhereCondition.Append(" AND ISNULL(VehicleDescription,'') LIKE @VehicleDescription");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ActiveText"]))
            {
                sbWhereCondition.Append(" AND CASE WHEN ISNULL(veh.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @ActiveText");
            }

            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( ISNULL(VehicleRIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(VehicleRegNumber,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(VIN,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(VehicleTypeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(LGAName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(VehiclePurposeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(VehicleFunctionName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(VehicleOwnershipName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(VehicleDescription,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR CASE WHEN ISNULL(veh.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @MainFilter)");
            }

            Vehicle mObjVehicle = new Vehicle()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),

                VehicleRIN = !string.IsNullOrWhiteSpace(Request.Form["VehicleRIN"]) ? "%" + Request.Form["VehicleRIN"].Trim() + "%" : TrynParse.parseString(Request.Form["VehicleRIN"]),
                VehicleRegNumber = !string.IsNullOrWhiteSpace(Request.Form["VehicleRegNumber"]) ? "%" + Request.Form["VehicleRegNumber"].Trim() + "%" : TrynParse.parseString(Request.Form["VehicleRegNumber"]),
                VIN = !string.IsNullOrWhiteSpace(Request.Form["VIN"]) ? "%" + Request.Form["VIN"].Trim() + "%" : TrynParse.parseString(Request.Form["VIN"]),
                VehicleTypeName = !string.IsNullOrWhiteSpace(Request.Form["VehicleTypeName"]) ? "%" + Request.Form["VehicleTypeName"].Trim() + "%" : TrynParse.parseString(Request.Form["VehicleTypeName"]),
                VehicleSubTypeName = !string.IsNullOrWhiteSpace(Request.Form["VehicleSubTypeName"]) ? "%" + Request.Form["VehicleSubTypeName"].Trim() + "%" : TrynParse.parseString(Request.Form["VehicleSubTypeName"]),
                LGAName = !string.IsNullOrWhiteSpace(Request.Form["LGAName"]) ? "%" + Request.Form["LGAName"].Trim() + "%" : TrynParse.parseString(Request.Form["LGAName"]),
                VehiclePurposeName = !string.IsNullOrWhiteSpace(Request.Form["VehiclePurposeName"]) ? "%" + Request.Form["VehiclePurposeName"].Trim() + "%" : TrynParse.parseString(Request.Form["VehiclePurposeName"]),
                VehicleFunctionName = !string.IsNullOrWhiteSpace(Request.Form["VehicleFunctionName"]) ? "%" + Request.Form["VehicleFunctionName"].Trim() + "%" : TrynParse.parseString(Request.Form["VehicleFunctionName"]),
                VehicleOwnershipName = !string.IsNullOrWhiteSpace(Request.Form["VehicleOwnershipName"]) ? "%" + Request.Form["VehicleOwnershipName"].Trim() + "%" : TrynParse.parseString(Request.Form["VehicleOwnershipName"]),
                VehicleDescription = !string.IsNullOrWhiteSpace(Request.Form["VehicleDescription"]) ? "%" + Request.Form["VehicleDescription"].Trim() + "%" : TrynParse.parseString(Request.Form["VehicleDescription"]),
                ActiveText = !string.IsNullOrWhiteSpace(Request.Form["ActiveText"]) ? "%" + Request.Form["ActiveText"].Trim() + "%" : TrynParse.parseString(Request.Form["ActiveText"]),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };
            
            //>>>Purpose Sorting Data 
            IDictionary<string, object> dcData = new BLVehicle().BL_SearchVehicle(mObjVehicle);
            IList<usp_SearchVehicle_Result> lstVehicle = (IList<usp_SearchVehicle_Result>)dcData["VehicleList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstVehicle
            }, JsonRequestBehavior.AllowGet);
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
                    VehicleDescription=pObjVehicleModel.VehicleDescription,
                    Active = true,
                    CreatedBy = SessionManager.SystemUserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Vehicle> mObjResponse = new BLVehicle().BL_InsertUpdateVehicle(mObjVehicle);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "Vehicle");
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
                        VehicleDescription=mObjVehicleData.VehicleDescription,
                        Active = mObjVehicleData.Active.GetValueOrDefault(),
                    };

                    UI_FillDropDown(mObjVehicleModelView);
                    return View(mObjVehicleModelView);
                }
                else
                {
                    return RedirectToAction("List", "Vehicle");
                }
            }
            else
            {
                return RedirectToAction("List", "Vehicle");
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
                    VIN = pObjVehicleModel.VIN != null ? pObjVehicleModel.VIN.Trim() : pObjVehicleModel.VIN,
                    AssetTypeID = (int)EnumList.AssetTypes.Vehicles,
                    VehicleTypeID = pObjVehicleModel.VehicleTypeID,
                    VehicleSubTypeID = pObjVehicleModel.VehicleSubTypeID,
                    LGAID = pObjVehicleModel.LGAID,
                    VehiclePurposeID = pObjVehicleModel.VehiclePurposeID,
                    VehicleFunctionID = pObjVehicleModel.VehicleFunctionID,
                    VehicleOwnershipID = pObjVehicleModel.VehicleOwnershipID,
                    VehicleDescription=pObjVehicleModel.VehicleDescription,
                    Active = pObjVehicleModel.Active,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Vehicle> mObjResponse = new BLVehicle().BL_InsertUpdateVehicle(mObjVehicle);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "Vehicle");
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
                        VehicleDescription=mObjVehicleData.VehicleDescription,
                        ActiveText = mObjVehicleData.ActiveText
                    };

                    return View(mObjVehicleModelView);
                }
                else
                {
                    return RedirectToAction("List", "Vehicle");
                }
            }
            else
            {
                return RedirectToAction("List", "Vehicle");
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

        public ActionResult TaxPayerList(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetID = id.GetValueOrDefault(),
                    AssetTypeID = (int)EnumList.AssetTypes.Vehicles
                };

                IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                usp_GetVehicleList_Result mObjVehicleData = new BLVehicle().BL_GetVehicleDetails(new Vehicle() { intStatus = 2, VehicleID = id.GetValueOrDefault() });
                if (lstTaxPayerAsset != null)
                {
                    ViewBag.AssetID = id;
                    ViewBag.AssetRIN = name;
                    ViewBag.AssetName = mObjVehicleData.VehicleRegNumber;
                    return View(lstTaxPayerAsset);
                }
                else
                {
                    return RedirectToAction("List", "Vehicle");
                }
            }
            else
            {
                return RedirectToAction("List", "Vehicle");
            }
        }

        public void UI_FillDropDown(TaxPayerAssetViewModel pObjTPAModel)
        {
            UI_FillAssetTypeDropDown(new Asset_Types() { intStatus = 1 }, (int)EnumList.AssetTypes.Vehicles);
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1 });
            UI_FillTaxPayerRoleDropDown(new TaxPayer_Roles() { intStatus = 1, AssetTypeID = (int)EnumList.AssetTypes.Vehicles });
        }

        public ActionResult AddTaxPayer(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                usp_GetVehicleList_Result mObjVehicleData = new BLVehicle().BL_GetVehicleDetails(new Vehicle() { intStatus = 2, VehicleID = id.GetValueOrDefault() });
                TaxPayerAssetViewModel mObjTaxPayerAssetModel = new TaxPayerAssetViewModel()
                {
                    AssetID = id.GetValueOrDefault(),
                    AssetRIN = name,
                    AssetName = mObjVehicleData.VehicleRegNumber,
                    AssetTypeID = (int)EnumList.AssetTypes.Vehicles
                };

                UI_FillDropDown(mObjTaxPayerAssetModel);
                return View(mObjTaxPayerAssetModel);
            }
            else
            {
                return RedirectToAction("TaxPayerList", "Vehicle");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddTaxPayer(TaxPayerAssetViewModel pObjAssetModel)
        {
            if (!ModelState.IsValid)
            {
                pObjAssetModel.AssetTypeID = (int)EnumList.AssetTypes.Vehicles;
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
                            AssetTypeID = (int)EnumList.AssetTypes.Vehicles,
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
                return RedirectToAction("TaxPayerList", "Vehicle", new { id = pObjAssetModel.AssetID, name = pObjAssetModel.AssetName });
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
                FuncResponse mObjFuncResponse = new BLTaxPayerAsset().BL_CheckAssetAlreadyLinked(new MAP_TaxPayer_Asset() { TaxPayerTypeID = pObjAssetModel.TaxPayerTypeID, TaxPayerID = pObjAssetModel.TaxPayerID, AssetTypeID = (int)EnumList.AssetTypes.Vehicles, AssetID = pObjAssetModel.AssetID, TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID });

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
                pObjTaxPayerData.AssetTypeID = (int)EnumList.AssetTypes.Vehicles;
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
                pObjTaxPayerData.AssetTypeID = (int)EnumList.AssetTypes.Vehicles;
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
    }
}