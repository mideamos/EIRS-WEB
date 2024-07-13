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
    public class BuildingUnitController : BaseController
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

            if (!string.IsNullOrWhiteSpace(Request.Form["UnitNumber"]))
            {
                sbWhereCondition.Append(" AND ISNULL(UnitNumber,'') LIKE @UnitNumber");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["UnitPurposeName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(UnitPurposeName,'') LIKE @UnitPurposeName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["UnitFunctionName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(UnitFunctionName,'') LIKE @UnitFunctionName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["UnitOccupancyName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(UnitOccupancyName,'') LIKE @UnitOccupancyName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["SizeName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(SizeName,'') LIKE @SizeName");
            }
            if (!string.IsNullOrWhiteSpace(Request.Form["ActiveText"]))
            {
                sbWhereCondition.Append(" AND CASE WHEN ISNULL(bunit.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @ActiveText");
            }


            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( ISNULL(UnitNumber,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(UnitPurposeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(UnitFunctionName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(UnitOccupancyName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(SizeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR CASE WHEN ISNULL(bunit.Active,0) = 0 THEN 'In Active' ELSE 'Active' END LIKE @MainFilter)");
            }

            Building_Unit mObjBuildingUnit = new Building_Unit()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),

                UnitNumber =!string.IsNullOrWhiteSpace(Request.Form["UnitNumber"]) ? "%" + Request.Form["UnitNumber"].Trim() + "%" : TrynParse.parseString(Request.Form["UnitNumber"]),
                UnitPurposeName = !string.IsNullOrWhiteSpace(Request.Form["UnitPurposeName"]) ? "%" + Request.Form["UnitPurposeName"].Trim() + "%" : TrynParse.parseString(Request.Form["UnitPurposeName"]),
                UnitFunctionName = !string.IsNullOrWhiteSpace(Request.Form["UnitFunctionName"]) ? "%" + Request.Form["UnitFunctionName"].Trim() + "%" : TrynParse.parseString(Request.Form["UnitFunctionName"]),
                UnitOccupancyName = !string.IsNullOrWhiteSpace(Request.Form["UnitOccupancyName"]) ? "%" + Request.Form["UnitOccupancyName"].Trim() + "%" : TrynParse.parseString(Request.Form["UnitOccupancyName"]),
                SizeName = !string.IsNullOrWhiteSpace(Request.Form["SizeName"]) ? "%" + Request.Form["SizeName"].Trim() + "%" : TrynParse.parseString(Request.Form["SizeName"]),
                ActiveText = !string.IsNullOrWhiteSpace(Request.Form["ActiveText"]) ? "%" + Request.Form["ActiveText"].Trim() + "%" : TrynParse.parseString(Request.Form["ActiveText"]),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter

            };

            IDictionary<string, object> dcData = new BLBuildingUnit().BL_SearchBuildingUnit(mObjBuildingUnit);
            IList<usp_SearchBuildingUnit_Result> lstBuildingUnit = (IList<usp_SearchBuildingUnit_Result>)dcData["BuildingUnitList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstBuildingUnit
            }, JsonRequestBehavior.AllowGet);
        }

        public void UI_FillDropDown(BuildingUnitViewModel pObjBuildingUnitViewModel = null)
        {
            if (pObjBuildingUnitViewModel == null)
                pObjBuildingUnitViewModel = new BuildingUnitViewModel();

            UI_FillUnitPurposeDropDown(new Unit_Purpose() { intStatus = 1, IncludeUnitPurposeIds = pObjBuildingUnitViewModel.UnitPurposeID.ToString() });
            UI_FillUnitFunctionDropDown(new Unit_Function() { intStatus = 1, UnitPurposeID = pObjBuildingUnitViewModel.UnitPurposeID, IncludeUnitFunctionIds = pObjBuildingUnitViewModel.UnitFunctionID.ToString() });
            UI_FillUnitOccupancyDropDown(new Unit_Occupancy() { intStatus = 1, IncludeUnitOccupancyIds = pObjBuildingUnitViewModel.UnitOccupancyID.ToString() });
            UI_FillSizeDropDown(new Size() { intStatus = 1, IncludeSizeIds = pObjBuildingUnitViewModel.SizeID.ToString() });
        }

        public ActionResult Add()
        {
            UI_FillDropDown();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(BuildingUnitViewModel pObjBuildingUnitModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjBuildingUnitModel);
                return View(pObjBuildingUnitModel);
            }
            else
            {
                using (TransactionScope mObjTransactionScope = new TransactionScope())
                {
                    BLBuildingUnit mObjBLBuildingUnit = new BLBuildingUnit();

                    Building_Unit mObjBuildingUnit = new Building_Unit()
                    {
                        BuildingUnitID = 0,
                        UnitNumber = pObjBuildingUnitModel.UnitNumber,
                        UnitPurposeID = pObjBuildingUnitModel.UnitPurposeID,
                        UnitFunctionID = pObjBuildingUnitModel.UnitFunctionID,
                        UnitOccupancyID = pObjBuildingUnitModel.UnitOccupancyID,
                        SizeID = pObjBuildingUnitModel.SizeID,
                        Active = true,
                        CreatedBy = SessionManager.SystemUserID,
                        CreatedDate = CommUtil.GetCurrentDateTime()
                    };

                    try
                    {

                        FuncResponse<Building_Unit> mObjResponse = mObjBLBuildingUnit.BL_InsertUpdateBuildingUnit(mObjBuildingUnit);

                        if (mObjResponse.Success)
                        {
                            mObjTransactionScope.Complete();
                            FlashMessage.Info(mObjResponse.Message);
                            return RedirectToAction("List", "BuildingUnit");
                        }
                        else
                        {
                            UI_FillDropDown(pObjBuildingUnitModel);
                            Transaction.Current.Rollback();
                            ViewBag.Message = mObjResponse.Message;
                            return View(pObjBuildingUnitModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        UI_FillDropDown(pObjBuildingUnitModel);
                        Transaction.Current.Rollback();
                        ViewBag.Message = "Error occurred while saving building unit";
                        return View(pObjBuildingUnitModel);
                    }
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Building_Unit mObjBuildingUnit = new Building_Unit()
                {
                    BuildingUnitID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBuildingUnitList_Result mObjBuildingUnitData = new BLBuildingUnit().BL_GetBuildingUnitDetails(mObjBuildingUnit);

                if (mObjBuildingUnitData != null)
                {
                    BuildingUnitViewModel mObjBuildingUnitModelView = new BuildingUnitViewModel()
                    {
                        BuildingUnitID = mObjBuildingUnitData.BuildingUnitID.GetValueOrDefault(),
                        UnitNumber = mObjBuildingUnitData.UnitNumber,
                        UnitPurposeID = mObjBuildingUnitData.UnitPurposeID.GetValueOrDefault(),
                        UnitFunctionID = mObjBuildingUnitData.UnitFunctionID.GetValueOrDefault(),
                        UnitOccupancyID = mObjBuildingUnitData.UnitOccupancyID.GetValueOrDefault(),
                        SizeID = mObjBuildingUnitData.SizeID.GetValueOrDefault(),
                        Active = mObjBuildingUnitData.Active.GetValueOrDefault(),
                    };

                    UI_FillDropDown(mObjBuildingUnitModelView);
                    return View(mObjBuildingUnitModelView);
                }
                else
                {
                    return RedirectToAction("List", "BuildingUnit");
                }
            }
            else
            {
                return RedirectToAction("List", "BuildingUnit");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(BuildingUnitViewModel pObjBuildingUnitModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillDropDown(pObjBuildingUnitModel);
                return View(pObjBuildingUnitModel);
            }
            else
            {
                using (TransactionScope mObjTransactionScope = new TransactionScope())
                {
                    BLBuildingUnit mObjBLBuildingUnit = new BLBuildingUnit();

                    Building_Unit mObjBuildingUnit = new Building_Unit()
                    {
                        BuildingUnitID = pObjBuildingUnitModel.BuildingUnitID,
                        UnitNumber = pObjBuildingUnitModel.UnitNumber,
                        UnitPurposeID = pObjBuildingUnitModel.UnitPurposeID,
                        UnitFunctionID = pObjBuildingUnitModel.UnitFunctionID,
                        UnitOccupancyID = pObjBuildingUnitModel.UnitOccupancyID,
                        SizeID = pObjBuildingUnitModel.SizeID,
                        Active = pObjBuildingUnitModel.Active,
                        ModifiedBy = SessionManager.SystemUserID,
                        ModifiedDate = CommUtil.GetCurrentDateTime()
                    };

                    try
                    {

                        FuncResponse<Building_Unit> mObjResponse = mObjBLBuildingUnit.BL_InsertUpdateBuildingUnit(mObjBuildingUnit);

                        if (mObjResponse.Success)
                        {
                            mObjTransactionScope.Complete();
                            FlashMessage.Info(mObjResponse.Message);
                            return RedirectToAction("List", "BuildingUnit");
                        }
                        else
                        {
                            UI_FillDropDown(pObjBuildingUnitModel);
                            Transaction.Current.Rollback();
                            ViewBag.Message = mObjResponse.Message;
                            return View(pObjBuildingUnitModel);
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        UI_FillDropDown(pObjBuildingUnitModel);
                        Transaction.Current.Rollback();
                        ViewBag.Message = "Error occurred while saving building";
                        return View(pObjBuildingUnitModel);
                    }
                }
            }
        }

        public ActionResult Details(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Building_Unit mObjBuildingUnit = new Building_Unit()
                {
                    BuildingUnitID = id.GetValueOrDefault(),
                    intStatus = 2
                };

                usp_GetBuildingUnitList_Result mObjBuildingUnitData = new BLBuildingUnit().BL_GetBuildingUnitDetails(mObjBuildingUnit);

                if (mObjBuildingUnitData != null)
                {
                    BuildingUnitViewModel mObjBuildingUnitModelView = new BuildingUnitViewModel()
                    {
                        BuildingUnitID = mObjBuildingUnitData.BuildingUnitID.GetValueOrDefault(),
                        UnitNumber = mObjBuildingUnitData.UnitNumber,
                        UnitPurposeName = mObjBuildingUnitData.UnitPurposeName,
                        UnitFunctionName = mObjBuildingUnitData.UnitFunctionName,
                        UnitOccupancyName = mObjBuildingUnitData.UnitOccupancyName,
                        SizeName = mObjBuildingUnitData.SizeName,
                        ActiveText = mObjBuildingUnitData.ActiveText
                    };

                    return View(mObjBuildingUnitModelView);
                }
                else
                {
                    return RedirectToAction("List", "BuildingUnit");
                }
            }
            else
            {
                return RedirectToAction("List", "BuildingUnit");
            }
        }

        public JsonResult UpdateStatus(Building_Unit pObjBuildingUnitData)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjBuildingUnitData.BuildingUnitID != 0)
            {
                FuncResponse mObjFuncResponse = new BLBuildingUnit().BL_UpdateStatus(pObjBuildingUnitData);
                dcResponse["success"] = mObjFuncResponse.Success;
                dcResponse["Message"] = mObjFuncResponse.Message;

                if (mObjFuncResponse.Success)
                {
                    dcResponse["BuildingUnitList"] = CommUtil.RenderPartialToString("_BindTable", mObjFuncResponse.AdditionalData, this.ControllerContext);
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