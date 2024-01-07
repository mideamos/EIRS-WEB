using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Vereyon.Web;
using System.Linq;
using System.Transactions;
using Elmah;
using System.Text;
using EIRS.Admin.Models;

namespace EIRS.Admin.Controllers
{
    public class PaymentAccountController : BaseController
    {
        // GET: PaymentAccount
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

            if (!string.IsNullOrWhiteSpace(Request.Form["PaymentRefNo"]))
            {
                sbWhereCondition.Append(" AND ISNULL(PaymentRefNo,'') LIKE @PaymentRefNo");
            }

            if (!string.IsNullOrWhiteSpace(Request.Form["PaymentDate"]))
            {
                sbWhereCondition.Append(" AND ISNULL(REPLACE(CONVERT(varchar(50),PaymentDate,106),' ','-'),'') LIKE @PaymentDate");
            }

            if (!string.IsNullOrWhiteSpace(Request.Form["TaxPayerName"]))
            {
                sbWhereCondition.Append(" AND dbo.GetTaxPayerName(poa.TaxPayerID,poa.TaxPayerTypeID) LIKE @TaxPayerName");
            }

            if (!string.IsNullOrWhiteSpace(Request.Form["TaxPayerTypeName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(TaxPayerTypeName,'') LIKE @TaxPayerTypeName");
            }

            if (!string.IsNullOrWhiteSpace(Request.Form["TaxPayerRIN"]))
            {
                sbWhereCondition.Append(" AND dbo.GetTaxPayerRIN(poa.TaxPayerID,poa.TaxPayerTypeID) LIKE @TaxPayerRIN");
            }

            if (!string.IsNullOrWhiteSpace(Request.Form["Amount"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Convert(varchar(50),Amount,106),'') LIKE @Amount");
            }

            if (!string.IsNullOrWhiteSpace(Request.Form["RevenueStreamName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(RevenueStreamName,'') LIKE @RevenueStreamName");
            }

            if (!string.IsNullOrWhiteSpace(Request.Form["RevenueSubStreamName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(RevenueSubStreamName,'') LIKE @RevenueSubStreamName");
            }

            if (!string.IsNullOrWhiteSpace(Request.Form["AgencyName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(AgencyName,'') LIKE @AgencyName");
            }

            if (!string.IsNullOrWhiteSpace(Request.Form["SettlementMethodName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(SettlementMethodName,'') LIKE @SettlementMethodName");
            }

            if (!string.IsNullOrWhiteSpace(Request.Form["SettlementStatusName"]))
            {
                sbWhereCondition.Append(" AND ISNULL(SettlementStatusName,'') LIKE @SettlementStatusName");
            }

            if (!string.IsNullOrWhiteSpace(Request.Form["Notes"]))
            {
                sbWhereCondition.Append(" AND ISNULL(Notes,'') LIKE @Notes");
            }

            if (!string.IsNullOrWhiteSpace(Request.Form["TransactionRefNo"]))
            {
                sbWhereCondition.Append(" AND ISNULL(TransactionRefNo,'') LIKE @TransactionRefNo");
            }

            if (!string.IsNullOrEmpty(vFilter))
            {
                sbWhereCondition.Append(" AND ( ISNULL(PaymentRefNo,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),PaymentDate,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TaxPayerTypeName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerName(poa.TaxPayerID,poa.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR dbo.GetTaxPayerRIN(poa.TaxPayerID,poa.TaxPayerTypeID) LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Convert(varchar(50),Amount,106),'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(RevenueStreamName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(RevenueSubStreamName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(AgencyName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(SettlementMethodName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(SettlementStatusName,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(Notes,'') LIKE @MainFilter ");
                sbWhereCondition.Append(" OR ISNULL(TransactionRefNo,'') LIKE @MainFilter)");

            }

            Payment_Account mObjPaymentAccount = new Payment_Account()
            {
                WhereCondition = sbWhereCondition.ToString(),
                OrderBy = vSortColumn,
                OrderByDirection = vSortColumnDir,
                PageNumber = (TrynParse.parseInt(vStart) / TrynParse.parseInt(vLength)) + 1,
                PageSize = TrynParse.parseInt(vLength),

                PaymentRefNo = !string.IsNullOrWhiteSpace(Request.Form["PaymentRefNo"]) ? "%" + Request.Form["PaymentRefNo"].Trim() + "%" : TrynParse.parseString(Request.Form["PaymentRefNo"]),
                strPaymentDate = !string.IsNullOrWhiteSpace(Request.Form["PaymentDate"]) ? "%" + Request.Form["PaymentDate"].Trim() + "%" : TrynParse.parseString(Request.Form["PaymentDate"]),
                TaxPayerTypeName = !string.IsNullOrWhiteSpace(Request.Form["TaxPayerTypeName"]) ? "%" + Request.Form["TaxPayerTypeName"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxPayerTypeName"]),
                TaxPayerName = !string.IsNullOrWhiteSpace(Request.Form["TaxPayerName"]) ? "%" + Request.Form["TaxPayerName"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxPayerName"]),
                TaxPayerRIN = !string.IsNullOrWhiteSpace(Request.Form["TaxPayerRIN"]) ? "%" + Request.Form["TaxPayerRIN"].Trim() + "%" : TrynParse.parseString(Request.Form["TaxPayerRIN"]),
                strAmount = !string.IsNullOrWhiteSpace(Request.Form["Amount"]) ? "%" + Request.Form["Amount"].Trim() + "%" : TrynParse.parseString(Request.Form["Amount"]),
                RevenueStreamName = !string.IsNullOrWhiteSpace(Request.Form["RevenueStreamName"]) ? "%" + Request.Form["RevenueStreamName"].Trim() + "%" : TrynParse.parseString(Request.Form["RevenueStreamName"]),
                RevenueSubStreamName = !string.IsNullOrWhiteSpace(Request.Form["RevenueSubStreamName"]) ? "%" + Request.Form["RevenueSubStreamName"].Trim() + "%" : TrynParse.parseString(Request.Form["RevenueSubStreamName"]),
                AgencyName = !string.IsNullOrWhiteSpace(Request.Form["AgencyName"]) ? "%" + Request.Form["AgencyName"].Trim() + "%" : TrynParse.parseString(Request.Form["AgencyName"]),
                SettlementMethodName = !string.IsNullOrWhiteSpace(Request.Form["SettlementMethodName"]) ? "%" + Request.Form["SettlementMethodName"].Trim() + "%" : TrynParse.parseString(Request.Form["SettlementMethodName"]),
                SettlementStatusName = !string.IsNullOrWhiteSpace(Request.Form["SettlementStatusName"]) ? "%" + Request.Form["SettlementStatusName"].Trim() + "%" : TrynParse.parseString(Request.Form["SettlementStatusName"]),
                Notes = !string.IsNullOrWhiteSpace(Request.Form["Notes"]) ? "%" + Request.Form["Notes"].Trim() + "%" : TrynParse.parseString(Request.Form["Notes"]),
                TransactionRefNo = !string.IsNullOrWhiteSpace(Request.Form["TransactionRefNo"]) ? "%" + Request.Form["TransactionRefNo"].Trim() + "%" : TrynParse.parseString(Request.Form["TransactionRefNo"]),
                MainFilter = !string.IsNullOrWhiteSpace(vFilter) ? "%" + vFilter.Trim() + "%" : vFilter
            };

            IDictionary<string, object> dcData = new BLPaymentAccount().BL_SearchPaymentAccount(mObjPaymentAccount);
            IList<usp_SearchPaymentAccount_Result> lstPaymentAccount = (IList<usp_SearchPaymentAccount_Result>)dcData["PaymentAccountList"];

            return Json(new
            {
                draw = vDraw,
                recordsFiltered = dcData["FilteredRecords"],
                recordsTotal = dcData["TotalRecords"],
                data = lstPaymentAccount
            }, JsonRequestBehavior.AllowGet);
        }

        public void UI_FillDropDown(PaymentAccountViewModel pObjPaymentAccountModel = null)
        {
            if (pObjPaymentAccountModel == null)
                pObjPaymentAccountModel = new PaymentAccountViewModel();

            UI_FillRevenueStreamDropDown(new Revenue_Stream() { intStatus = 1, /*AssetTypeID = pObjAssessmentItemModel.AssetTypeID,*/ IncludeRevenueStreamIds = pObjPaymentAccountModel.RevenueStreamID.ToString() });
            UI_FillRevenueSubStreamDropDown(new Revenue_SubStream() { intStatus = 1, RevenueStreamID = pObjPaymentAccountModel.RevenueStreamID, IncludeRevenueSubStreamIds = pObjPaymentAccountModel.RevenueSubStreamID.ToString() });
            UI_FillAgencyDropDown(new Agency() { intStatus = 1, IncludeAgencyIds = pObjPaymentAccountModel.AgencyID.ToString() });
        }

        public ActionResult Edit(long? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                BLPaymentAccount mObjBLPaymentAccount = new BLPaymentAccount();
                usp_GetPaymentAccountList_Result mObjPaymentAccountDetails = mObjBLPaymentAccount.BL_GetPaymentAccountDetails(new Payment_Account() { PaymentAccountID = id.GetValueOrDefault() });

                if (mObjPaymentAccountDetails != null)
                {
                    PaymentAccountViewModel mObjPaymentAccountModel = new PaymentAccountViewModel()
                    {
                        PaymentAccountID = mObjPaymentAccountDetails.PaymentAccountID.GetValueOrDefault(),
                        RevenueStreamID = mObjPaymentAccountDetails.RevenueStreamID.GetValueOrDefault(),
                        RevenueSubStreamID = mObjPaymentAccountDetails.RevenueSubStreamID.GetValueOrDefault(),
                        AgencyID = mObjPaymentAccountDetails.AgencyID.GetValueOrDefault()
                    };

                    UI_FillDropDown(mObjPaymentAccountModel);
                    ViewBag.PaymentAccountData = mObjPaymentAccountDetails;
                    return View(mObjPaymentAccountModel);
                }
                else
                {
                    return RedirectToAction("List", "PaymentAccount");
                }
            }
            else
            {
                return RedirectToAction("List", "PaymentAccount");
            }
        }

        [HttpPost()]
        [ValidateAntiForgeryToken()]
        public ActionResult Edit(PaymentAccountViewModel pObjPaymentAccountModel)
        {
            BLPaymentAccount mObjBLPaymentAccount = new BLPaymentAccount();
            usp_GetPaymentAccountList_Result mObjPaymentAccountDetails = mObjBLPaymentAccount.BL_GetPaymentAccountDetails(new Payment_Account() { PaymentAccountID = pObjPaymentAccountModel.PaymentAccountID });

            if (!ModelState.IsValid)
            {
                ViewBag.PaymentAccountData = mObjPaymentAccountDetails;
                UI_FillDropDown(pObjPaymentAccountModel);
                return View(pObjPaymentAccountModel);
            }
            else
            {
                Payment_Account mObjPaymentAccount = new Payment_Account()
                {
                    PaymentAccountID = pObjPaymentAccountModel.PaymentAccountID,
                    RevenueStreamID = pObjPaymentAccountModel.RevenueStreamID,
                    RevenueSubStreamID = pObjPaymentAccountModel.RevenueSubStreamID,
                    AgencyID = pObjPaymentAccountModel.AgencyID,
                    ModifiedBy = SessionManager.SystemUserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjResponse = mObjBLPaymentAccount.BL_UpdatePaymentAccountFromRDM(mObjPaymentAccount);

                    if (mObjResponse.Success)
                    {
                        FlashMessage.Info(mObjResponse.Message);
                        return RedirectToAction("List", "PaymentAccount");
                    }
                    else
                    {
                        ViewBag.PaymentAccountData = mObjPaymentAccountDetails;
                        UI_FillDropDown(pObjPaymentAccountModel);
                        ViewBag.Message = mObjResponse.Message;
                        return View(pObjPaymentAccountModel);
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.PaymentAccountData = mObjPaymentAccountDetails;
                    ErrorSignal.FromCurrentContext().Raise(ex);
                    UI_FillDropDown(pObjPaymentAccountModel);
                    ViewBag.Message = "Error occurred while saving payment account";
                    return View(pObjPaymentAccountModel);
                }
            }
        }

        public ActionResult Details(long? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                BLPaymentAccount mObjBLPaymentAccount = new BLPaymentAccount();
                usp_GetPaymentAccountList_Result mObjPaymentAccountDetails = mObjBLPaymentAccount.BL_GetPaymentAccountDetails(new Payment_Account() { PaymentAccountID = id.GetValueOrDefault() });

                if (mObjPaymentAccountDetails != null)
                {
                    return View(mObjPaymentAccountDetails);
                }
                else
                {
                    return RedirectToAction("List", "PaymentAccount");
                }
            }
            else
            {
                return RedirectToAction("List", "PaymentAccount");
            }
        }
    }
}