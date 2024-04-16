using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace EIRS.Web.Controllers
{
    public class TCCDetailController : BaseController
    {
        // GET: TCCDetail
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

            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( dbo.GetTaxPayerName(tcd.TaxPayerID,tcd.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerRIN(tcd.TaxPayerID,tcd.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerTIN(tcd.TaxPayerID,tcd.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),TCCTaxPaid,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR Convert(varchar(50),TaxYear) LIKE @MainFilter )");

            }

            TCCDetail mObjTCCDetails = new TCCDetail()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter

            };

            IDictionary<string, object> dcData = new BLTCC().BL_SearchTCCDetail(mObjTCCDetails);
            IList<usp_SearchTCCDetails_Result> lstTCCDetails = (IList<usp_SearchTCCDetails_Result>)dcData["TCCDetailList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstTCCDetails
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Add()
        {
            UI_FillYearDropDown();
            UI_FillTaxPayerTypeDropDown();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Add(TCCDetailViewModel pObjTCCDetailModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillYearDropDown();
                UI_FillTaxPayerTypeDropDown();
                return View(pObjTCCDetailModel);
            }
            else
            {
                usp_GetTaxPayerLiabilityByTaxYear_Result mObjLiabilityData = new BLReport().BL_GetTaxPayerLiabilityByTaxYear(pObjTCCDetailModel.TaxPayerTypeID, pObjTCCDetailModel.TaxPayerID, pObjTCCDetailModel.TaxYear);

                TCCDetail mObjTCCDetail = new TCCDetail()
                {
                    TCCDetailID = 0,
                    TaxPayerID = pObjTCCDetailModel.TaxPayerID,
                    TaxPayerTypeID = pObjTCCDetailModel.TaxPayerTypeID,
                    TaxYear = pObjTCCDetailModel.TaxYear,
                    AssessableIncome = pObjTCCDetailModel.AssessableIncome,
                    TCCTaxPaid = pObjTCCDetailModel.TCCTaxPaid,
                    ERASAssessed = mObjLiabilityData.TotalAssessmentAmount,
                    ERASTaxPaid = mObjLiabilityData.TotalPaymentAmount,
                    CreatedBy = SessionManager.UserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                FuncResponse mObjFuncResponse = new BLTCC().BL_InsertTCCDetail(mObjTCCDetail);

                if (mObjFuncResponse.Success)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ViewBag.Message = mObjFuncResponse.Message;
                    UI_FillYearDropDown();
                    UI_FillTaxPayerTypeDropDown();
                    return View(pObjTCCDetailModel);
                }
            }
        }

        public ActionResult Edit(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {


                UI_FillYearDropDown();
                UI_FillTaxPayerTypeDropDown();
                return View();
            }
            else
            {
                return RedirectToAction("List", "TCCDetail");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(TCCDetailViewModel pObjTCCDetailModel)
        {
            if (!ModelState.IsValid)
            {
                UI_FillYearDropDown();
                UI_FillTaxPayerTypeDropDown();
                return View(pObjTCCDetailModel);
            }
            else
            {
                usp_GetTaxPayerLiabilityByTaxYear_Result mObjLiabilityData = new BLReport().BL_GetTaxPayerLiabilityByTaxYear(pObjTCCDetailModel.TaxPayerTypeID, pObjTCCDetailModel.TaxPayerID, pObjTCCDetailModel.TaxYear);

                TCCDetail mObjTCCDetail = new TCCDetail()
                {
                    TCCDetailID = pObjTCCDetailModel.TCCDetailID,
                    TaxPayerID = pObjTCCDetailModel.TaxPayerID,
                    TaxPayerTypeID = pObjTCCDetailModel.TaxPayerTypeID,
                    TaxYear = pObjTCCDetailModel.TaxYear,
                    AssessableIncome = pObjTCCDetailModel.AssessableIncome,
                    TCCTaxPaid = pObjTCCDetailModel.TCCTaxPaid,
                    ERASAssessed = mObjLiabilityData.TotalAssessmentAmount,
                    ERASTaxPaid = mObjLiabilityData.TotalPaymentAmount,
                    CreatedBy = SessionManager.UserID,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                FuncResponse mObjFuncResponse = new BLTCC().BL_InsertTCCDetail(mObjTCCDetail);

                if (mObjFuncResponse.Success)
                {
                    return RedirectToAction("List");
                }
                else
                {
                    ViewBag.Message = mObjFuncResponse.Message;
                    UI_FillYearDropDown();
                    UI_FillTaxPayerTypeDropDown();
                    return View(pObjTCCDetailModel);
                }
            }
        }

        public JsonResult GetTaxPayerLiabilityData(int TaxPayerTypeID, int TaxPayerID, int TaxYear)
        {
            usp_GetTaxPayerLiabilityByTaxYear_Result mObjLiabilityData = new BLReport().BL_GetTaxPayerLiabilityByTaxYear(TaxPayerTypeID, TaxPayerID, TaxYear);
            return Json(mObjLiabilityData, JsonRequestBehavior.AllowGet);
        }
    }
}