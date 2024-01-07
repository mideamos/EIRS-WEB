using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EIRS.Web.Controllers
{
    public class ConvertTaxPayerController : BaseController
    {
        // GET: ConvertTaxPayer
        public ActionResult List()
        {
            return View();
        }

        public void UI_FillDropDown(usp_GetIndividualList_Result pObjIndvivdualViewModel = null)
        {
            if (pObjIndvivdualViewModel != null)
                pObjIndvivdualViewModel.TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies;
            else if (pObjIndvivdualViewModel == null)
                pObjIndvivdualViewModel = new usp_GetIndividualList_Result();

            UI_FillNationality();
            UI_FillMaritalStatus();
            UI_FillGender();
            UI_FillTitleDropDown();
            UI_FillGovernmentTypeDropDown(new Government_Types() { intStatus = 1 });
            UI_FillTaxOfficeDropDown(new Tax_Offices() { intStatus = 1, IncludeTaxOfficeIds = pObjIndvivdualViewModel.TaxOfficeID.GetValueOrDefault().ToString() });
            UI_FillTaxPayerTypeDropDown(new TaxPayer_Types() { intStatus = 1, IncludeTaxPayerTypeIds = pObjIndvivdualViewModel.TaxPayerTypeID.GetValueOrDefault().ToString() }, (int)EnumList.TaxPayerType.Companies);
            UI_FillEconomicActivitiesDropDown(new Economic_Activities() { intStatus = 1, IncludeEconomicActivitiesIds = pObjIndvivdualViewModel.EconomicActivitiesID.GetValueOrDefault().ToString(), TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies });
            UI_FillNotificationMethodDropDown(new Notification_Method() { intStatus = 1, IncludeNotificationMethodIds = pObjIndvivdualViewModel.NotificationMethodID.GetValueOrDefault().ToString() });
        }

        public ActionResult IndividualToCorporate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IndividualToCorporate(FormCollection pObjFormCollection)
        {
            string mStrName = pObjFormCollection.Get("txtName");
            string mStrMobileNumber = pObjFormCollection.Get("txtMobileNumber");
            string mStrRIN = pObjFormCollection.Get("txtRIN");

            Individual mObjIndividual = new Individual()
            {
                IndividualName = mStrName,
                MobileNumber1 = mStrMobileNumber,
                IndividualRIN = mStrRIN,
                intStatus = 1
            };
            SessionManager.lstAssetDetails = null;
            IList<usp_GetIndividualList_Result> lstIndividual = new BLIndividual().BL_GetIndividualList(mObjIndividual);
            return PartialView("_BindIndividualToCorporateTable", lstIndividual.Take(5).ToList());
        }

        public ActionResult ConvertIndividualToCorporate(int? id, string Name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                if (mObjIndividualData != null)
                {

                    CompanyViewModel mObjCompanyModelView = new CompanyViewModel()
                    {
                        CompanyRIN = mObjIndividualData.IndividualRIN,
                        CompanyName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                        TIN = mObjIndividualData.TIN,
                        MobileNumber1 = mObjIndividualData.MobileNumber1,
                        MobileNumber2 = mObjIndividualData.MobileNumber2,
                        EmailAddress1 = mObjIndividualData.EmailAddress1,
                        EmailAddress2 = mObjIndividualData.EmailAddress2,
                        TaxOfficeID = mObjIndividualData.TaxOfficeID,
                        TaxOfficeName = mObjIndividualData.TaxOfficeName,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                        EconomicActivitiesID = mObjIndividualData.EconomicActivitiesID.GetValueOrDefault(),
                        EconomicActivitiesName = mObjIndividualData.EconomicActivitiesName,
                        NotificationMethodID = mObjIndividualData.NotificationMethodID.GetValueOrDefault(),
                        NotificationMethodName = mObjIndividualData.NotificationMethodName,
                        ContactAddress = mObjIndividualData.ContactAddress,
                        Active = true,
                    };

                    MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                    {
                        TaxPayerID = id.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual
                    };

                    IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);

                    IList<AssetDetails> lstAssetDetails = new List<AssetDetails>();


                    foreach (var item in lstTaxPayerAsset)
                    {
                        AssetDetails mObjAssetDetails = new AssetDetails();
                        mObjAssetDetails.TPAID = item.TPAID.GetValueOrDefault();
                        mObjAssetDetails.AssetTypeID = item.AssetTypeID.GetValueOrDefault();
                        mObjAssetDetails.AssetTypeName = item.AssetTypeName;
                        mObjAssetDetails.AssetID = item.AssetID.GetValueOrDefault();
                        mObjAssetDetails.AssetLGA = item.AssetLGA;
                        mObjAssetDetails.AssetRIN = item.AssetRIN;
                        mObjAssetDetails.AssetName = item.AssetName;
                        mObjAssetDetails.BuildingUnitID = item.BuildingUnitID.GetValueOrDefault();
                        mObjAssetDetails.UnitNumber = item.UnitNumber;
                        mObjAssetDetails.Active = item.Active.GetValueOrDefault();
                        mObjAssetDetails.ActiveText = item.ActiveText;
                        mObjAssetDetails.RowID = lstAssetDetails.Count() + 1;

                        lstAssetDetails.Add(mObjAssetDetails);
                    }

                    SessionManager.lstAssetDetails = lstAssetDetails;
                    ViewBag.AssetList = lstAssetDetails;

                    UI_FillDropDown(mObjIndividualData);
                    return View(mObjCompanyModelView);
                }
                else
                {
                    return RedirectToAction("IndividualToCorporate", "ConvertTaxPayer");
                }
            }
            else
            {
                return RedirectToAction("IndividualToCorporate", "ConvertTaxPayer");
            }
        }

        public JsonResult AssignAssetRole(AssetDetails pObjAssetDetails)
        {
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (pObjAssetDetails.RowID > 0)
            {
                IList<AssetDetails> lstAssetDetails;
                if (SessionManager.lstAssetDetails != null)
                {
                    lstAssetDetails = SessionManager.lstAssetDetails;
                }
                else
                {
                    lstAssetDetails = new List<AssetDetails>();
                }

                AssetDetails mObjAssetDetails = lstAssetDetails.Where(t => t.RowID == pObjAssetDetails.RowID).FirstOrDefault();

                if (mObjAssetDetails != null)
                {
                    mObjAssetDetails.TaxPayerRoleID = pObjAssetDetails.TaxPayerRoleID;
                    mObjAssetDetails.TaxPayerRoleName = pObjAssetDetails.TaxPayerRoleName;
                    mObjAssetDetails.BuildingUnitID = pObjAssetDetails.BuildingUnitID;
                }

                SessionManager.lstAssetDetails = lstAssetDetails;
                dcResponse["success"] = true;
                dcResponse["Message"] = "Tax Payer Role Added Successfully";
                dcResponse["AssetData"] = CommUtil.RenderPartialToString("_BindAssetTable", lstAssetDetails.ToList(), this.ControllerContext);

            }
            else
            {
                dcResponse["Message"] = "Tax Payer Role Adding Failed";
                dcResponse["success"] = false;
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DisplayData()
        {
            IList<AssetDetails> lstAssetDetails;
            if (SessionManager.lstAssetDetails != null)
            {
                lstAssetDetails = SessionManager.lstAssetDetails;
            }
            else
            {
                lstAssetDetails = new List<AssetDetails>();
            }
            Dictionary<string, object> dcResponse = new Dictionary<string, object>();

            if (lstAssetDetails.Where(t => t.TaxPayerRoleID == 0).Count() > 0)
            {
                dcResponse["success"] = false;
                dcResponse["dvMessage"] = "All tax payer roles Should be assigned";
            }
            else
            {
                dcResponse["AssetData"] = CommUtil.RenderPartialToString("_BindAssetDetails", lstAssetDetails.ToList(), this.ControllerContext);
                dcResponse["success"] = true;
            }

            return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IndividualToGovernment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IndividualToGovernment(FormCollection pObjFormCollection)
        {
            string mStrName = pObjFormCollection.Get("txtName");
            string mStrMobileNumber = pObjFormCollection.Get("txtMobileNumber");
            string mStrRIN = pObjFormCollection.Get("txtRIN");

            Individual mObjIndividual = new Individual()
            {
                IndividualName = mStrName,
                MobileNumber1 = mStrMobileNumber,
                IndividualRIN = mStrRIN,
                intStatus = 1
            };
            SessionManager.lstAssetDetails = null;
            IList<usp_GetIndividualList_Result> lstIndividual = new BLIndividual().BL_GetIndividualList(mObjIndividual);
            return PartialView("_BindIndividualToGovernmentTable", lstIndividual.Take(5).ToList());
        }

        public ActionResult ConvertIndividualToGovernment(int? id, string Name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                UI_FillDropDown(mObjIndividualData);


                if (mObjIndividualData != null)
                {
                    GovernmentViewModel mObjGovernmentModelView = new GovernmentViewModel()
                    {
                        GovernmentRIN = mObjIndividualData.IndividualRIN,
                        TIN = mObjIndividualData.TIN,
                        GovernmentName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                        TaxOfficeID = mObjIndividualData.TaxOfficeID,
                        TaxOfficeName = mObjIndividualData.TaxOfficeName,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                        TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
                        ContactNumber = mObjIndividualData.MobileNumber1,
                        ContactEmail = mObjIndividualData.EmailAddress1,
                        ContactName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                        NotificationMethodID = mObjIndividualData.NotificationMethodID.GetValueOrDefault(),
                        NotificationMethodName = mObjIndividualData.NotificationMethodName,
                        ContactAddress = mObjIndividualData.ContactAddress,
                        Active = true
                    };

                    MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                    {
                        TaxPayerID = id.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual
                    };


                    IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);

                    IList<AssetDetails> lstAssetDetails = new List<AssetDetails>();


                    foreach (var item in lstTaxPayerAsset)
                    {
                        AssetDetails mObjAssetDetails = new AssetDetails();
                        mObjAssetDetails.TPAID = item.TPAID.GetValueOrDefault();
                        mObjAssetDetails.AssetTypeID = item.AssetTypeID.GetValueOrDefault();
                        mObjAssetDetails.AssetTypeName = item.AssetTypeName;
                        mObjAssetDetails.AssetID = item.AssetID.GetValueOrDefault();
                        mObjAssetDetails.AssetLGA = item.AssetLGA;
                        mObjAssetDetails.AssetRIN = item.AssetRIN;
                        mObjAssetDetails.AssetName = item.AssetName;
                        mObjAssetDetails.BuildingUnitID = item.BuildingUnitID.GetValueOrDefault();
                        mObjAssetDetails.UnitNumber = item.UnitNumber;
                        mObjAssetDetails.Active = item.Active.GetValueOrDefault();
                        mObjAssetDetails.ActiveText = item.ActiveText;
                        mObjAssetDetails.RowID = lstAssetDetails.Count() + 1;

                        lstAssetDetails.Add(mObjAssetDetails);
                    }
                    SessionManager.lstAssetDetails = lstAssetDetails;
                    ViewBag.AssetList = lstAssetDetails;

                    return View(mObjGovernmentModelView);
                }
                else
                {
                    return RedirectToAction("IndividualToGovernment", "ConvertTaxPayer");
                }
            }
            else
            {
                return RedirectToAction("IndividualToGovernment", "ConvertTaxPayer");
            }
        }

        public ActionResult CorporateToIndividual()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CorporateToIndividual(FormCollection pObjFormCollection)
        {
            string mStrCompanyName = pObjFormCollection.Get("txtCompanyName");
            string mStrMobileNumber = pObjFormCollection.Get("txtMobileNumber");
            string mStrRIN = pObjFormCollection.Get("txtRIN");

            Company mObjCompany = new Company()
            {
                CompanyName = mStrCompanyName,
                MobileNumber1 = mStrMobileNumber,
                CompanyRIN = mStrRIN,
                intStatus = 1
            };

            IList<usp_GetCompanyList_Result> lstCompany = new BLCompany().BL_GetCompanyList(mObjCompany);
            return PartialView("_BindCorporateToIndividualTable", lstCompany.Take(5).ToList());
        }

        public ActionResult ConvertCorporateToIndividual(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

                if (mObjCompanyData != null)
                {
                    IndividualViewModel mObjIndividualModelView = new IndividualViewModel()
                    {
                        IndividualRIN = mObjCompanyData.CompanyRIN,
                        TIN = mObjCompanyData.TIN,
                        FirstName = mObjCompanyData.CompanyName,
                        MobileNumber1 = mObjCompanyData.MobileNumber1,
                        MobileNumber2 = mObjCompanyData.MobileNumber2,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                        EmailAddress1 = mObjCompanyData.EmailAddress1,
                        EmailAddress2 = mObjCompanyData.EmailAddress2,
                        TaxOfficeName = mObjCompanyData.TaxOfficeName,
                        TaxPayerTypeName = mObjCompanyData.TaxPayerTypeName,
                        EconomicActivitiesName = mObjCompanyData.EconomicActivitiesName,
                        NotificationMethodName = mObjCompanyData.NotificationMethodName,
                        ContactAddress = mObjCompanyData.ContactAddress,
                        ActiveText = mObjCompanyData.ActiveText
                    };


                    MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                    {
                        TaxPayerID = id.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies
                    };

                    IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                    IList<AssetDetails> lstAssetDetails = new List<AssetDetails>();


                    foreach (var item in lstTaxPayerAsset)
                    {
                        AssetDetails mObjAssetDetails = new AssetDetails();
                        mObjAssetDetails.TPAID = item.TPAID.GetValueOrDefault();
                        mObjAssetDetails.AssetTypeID = item.AssetTypeID.GetValueOrDefault();
                        mObjAssetDetails.AssetTypeName = item.AssetTypeName;
                        mObjAssetDetails.AssetID = item.AssetID.GetValueOrDefault();
                        mObjAssetDetails.AssetLGA = item.AssetLGA;
                        mObjAssetDetails.AssetRIN = item.AssetRIN;
                        mObjAssetDetails.AssetName = item.AssetName;
                        mObjAssetDetails.BuildingUnitID = item.BuildingUnitID.GetValueOrDefault();
                        mObjAssetDetails.UnitNumber = item.UnitNumber;
                        mObjAssetDetails.Active = item.Active.GetValueOrDefault();
                        mObjAssetDetails.ActiveText = item.ActiveText;
                        mObjAssetDetails.RowID = lstAssetDetails.Count() + 1;

                        lstAssetDetails.Add(mObjAssetDetails);
                    }

                    SessionManager.lstAssetDetails = lstAssetDetails;
                    ViewBag.AssetList = lstAssetDetails;

                    usp_GetIndividualList_Result mObjIndvivdualData = new usp_GetIndividualList_Result()
                    {
                        TaxOfficeID = mObjCompanyData.TaxOfficeID,
                        TaxPayerTypeID = mObjCompanyData.TaxPayerTypeID,
                        EconomicActivitiesID = mObjCompanyData.EconomicActivitiesID,
                        NotificationMethodID = mObjCompanyData.NotificationMethodID
                    };
                    UI_FillDropDown(mObjIndvivdualData);
                    return View(mObjIndividualModelView);
                }
                else
                {
                    return RedirectToAction("CorporateToIndividual", "ConvertTaxPayer");
                }
            }
            else
            {
                return RedirectToAction("CorporateToIndividual", "ConvertTaxPayer");
            }
        }

        public ActionResult CorporateToGovernment()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CorporateToGovernment(FormCollection pObjFormCollection)
        {
            string mStrCompanyName = pObjFormCollection.Get("txtCompanyName");
            string mStrMobileNumber = pObjFormCollection.Get("txtMobileNumber");
            string mStrRIN = pObjFormCollection.Get("txtRIN");

            Company mObjCompany = new Company()
            {
                CompanyName = mStrCompanyName,
                MobileNumber1 = mStrMobileNumber,
                CompanyRIN = mStrRIN,
                intStatus = 1
            };

            IList<usp_GetCompanyList_Result> lstCompany = new BLCompany().BL_GetCompanyList(mObjCompany);
            return PartialView("_BindCorporateToGovernmentTable", lstCompany.Take(5).ToList());
        }

        public ActionResult ConvertCorporateToGovernment(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

                if (mObjCompanyData != null)
                {
                    GovernmentViewModel mObjGovernmentModelView = new GovernmentViewModel()
                    {
                        GovernmentRIN = mObjCompanyData.CompanyRIN,
                        TIN = mObjCompanyData.TIN,
                        GovernmentName = mObjCompanyData.CompanyName,
                        TaxOfficeID = mObjCompanyData.TaxOfficeID,
                        TaxOfficeName = mObjCompanyData.TaxOfficeName,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                        TaxPayerTypeName = mObjCompanyData.TaxPayerTypeName,
                        ContactNumber = mObjCompanyData.MobileNumber1,
                        ContactEmail = mObjCompanyData.EmailAddress1,
                        ContactName = mObjCompanyData.CompanyName,
                        NotificationMethodID = mObjCompanyData.NotificationMethodID.GetValueOrDefault(),
                        NotificationMethodName = mObjCompanyData.NotificationMethodName,
                        ContactAddress = mObjCompanyData.ContactAddress,
                        Active = true
                    };

                    MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                    {
                        TaxPayerID = id.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual
                    };

                    IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                    IList<AssetDetails> lstAssetDetails = new List<AssetDetails>();


                    foreach (var item in lstTaxPayerAsset)
                    {
                        AssetDetails mObjAssetDetails = new AssetDetails();
                        mObjAssetDetails.TPAID = item.TPAID.GetValueOrDefault();
                        mObjAssetDetails.AssetTypeID = item.AssetTypeID.GetValueOrDefault();
                        mObjAssetDetails.AssetTypeName = item.AssetTypeName;
                        mObjAssetDetails.AssetID = item.AssetID.GetValueOrDefault();
                        mObjAssetDetails.AssetLGA = item.AssetLGA;
                        mObjAssetDetails.AssetRIN = item.AssetRIN;
                        mObjAssetDetails.AssetName = item.AssetName;
                        mObjAssetDetails.BuildingUnitID = item.BuildingUnitID.GetValueOrDefault();
                        mObjAssetDetails.UnitNumber = item.UnitNumber;
                        mObjAssetDetails.Active = item.Active.GetValueOrDefault();
                        mObjAssetDetails.ActiveText = item.ActiveText;
                        mObjAssetDetails.RowID = lstAssetDetails.Count() + 1;

                        lstAssetDetails.Add(mObjAssetDetails);
                    }

                    SessionManager.lstAssetDetails = lstAssetDetails;
                    ViewBag.AssetList = lstAssetDetails;

                    usp_GetIndividualList_Result mObjIndvivdualData = new usp_GetIndividualList_Result()
                    {
                        TaxOfficeID = mObjCompanyData.TaxOfficeID,
                        TaxPayerTypeID = mObjCompanyData.TaxPayerTypeID,
                        EconomicActivitiesID = mObjCompanyData.EconomicActivitiesID,
                        NotificationMethodID = mObjCompanyData.NotificationMethodID
                    };
                    UI_FillDropDown(mObjIndvivdualData);
                    return View(mObjGovernmentModelView);
                }
                else
                {
                    return RedirectToAction("CorporateToGovernment", "ConvertTaxPayer");
                }
            }
            else
            {
                return RedirectToAction("CorporateToIndividual", "ConvertTaxPayer");
            }
        }

        public ActionResult GovernmentToIndividual()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GovernmentToIndividual(FormCollection pObjCollection)
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
            return PartialView("_BindGovernmentToIndividualTable", lstGovernment.Take(5).ToList());
        }

        public ActionResult ConvertGovernmentToIndividual(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Government mObjGovernment = new Government()
                {
                    GovernmentID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(mObjGovernment);

                if (mObjGovernmentData != null)
                {

                    IndividualViewModel mObjIndividualModelView = new IndividualViewModel()
                    {
                        IndividualRIN = mObjGovernmentData.GovernmentRIN,
                        TIN = mObjGovernmentData.TIN,
                        FirstName = mObjGovernmentData.GovernmentName,
                        MobileNumber1 = mObjGovernmentData.ContactNumber,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                        EmailAddress1 = mObjGovernmentData.ContactEmail,
                        TaxOfficeName = mObjGovernmentData.TaxOfficeName,
                        TaxPayerTypeName = mObjGovernmentData.TaxPayerTypeName,
                        NotificationMethodName = mObjGovernmentData.NotificationMethodName,
                        ContactAddress = mObjGovernmentData.ContactAddress,
                        ActiveText = mObjGovernmentData.ActiveText
                    };


                    MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                    {
                        TaxPayerID = id.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual
                    };

                    IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                    IList<AssetDetails> lstAssetDetails = new List<AssetDetails>();


                    foreach (var item in lstTaxPayerAsset)
                    {
                        AssetDetails mObjAssetDetails = new AssetDetails();
                        mObjAssetDetails.TPAID = item.TPAID.GetValueOrDefault();
                        mObjAssetDetails.AssetTypeID = item.AssetTypeID.GetValueOrDefault();
                        mObjAssetDetails.AssetTypeName = item.AssetTypeName;
                        mObjAssetDetails.AssetID = item.AssetID.GetValueOrDefault();
                        mObjAssetDetails.AssetLGA = item.AssetLGA;
                        mObjAssetDetails.AssetRIN = item.AssetRIN;
                        mObjAssetDetails.AssetName = item.AssetName;
                        mObjAssetDetails.BuildingUnitID = item.BuildingUnitID.GetValueOrDefault();
                        mObjAssetDetails.UnitNumber = item.UnitNumber;
                        mObjAssetDetails.Active = item.Active.GetValueOrDefault();
                        mObjAssetDetails.ActiveText = item.ActiveText;
                        mObjAssetDetails.RowID = lstAssetDetails.Count() + 1;

                        lstAssetDetails.Add(mObjAssetDetails);
                    }

                    SessionManager.lstAssetDetails = lstAssetDetails;
                    ViewBag.AssetList = lstAssetDetails;

                    usp_GetIndividualList_Result mObjIndvivdualData = new usp_GetIndividualList_Result()
                    {
                        TaxOfficeID = mObjGovernmentData.TaxOfficeID,
                        TaxPayerTypeID = mObjGovernmentData.TaxPayerTypeID,
                        NotificationMethodID = mObjGovernmentData.NotificationMethodID
                    };
                    UI_FillDropDown(mObjIndvivdualData);
                    return View(mObjIndividualModelView);
                }
                else
                {
                    return RedirectToAction("GovernmentToIndividual", "ConvertTaxPayer");
                }
            }
            else
            {
                return RedirectToAction("GovernmentToIndividual", "ConvertTaxPayer");
            }
        }

        public ActionResult GovernmentToCorporate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GovernmentToCorporate(FormCollection pObjCollection)
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
            return PartialView("_BindGovernmentToCorporateTable", lstGovernment.Take(5).ToList());
        }

        public ActionResult ConvertGovernmentToCorporate(int? id, string name)
        {
            if (id.GetValueOrDefault() > 0)
            {
                Government mObjGovernment = new Government()
                {
                    GovernmentID = id.GetValueOrDefault(),
                    intStatus = 1
                };

                usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(mObjGovernment);

                if (mObjGovernmentData != null)
                {

                    CompanyViewModel mObjCompanyModelView = new CompanyViewModel()
                    {
                        CompanyRIN = mObjGovernmentData.GovernmentRIN,
                        CompanyName = mObjGovernmentData.GovernmentName,
                        TIN = mObjGovernmentData.TIN,
                        MobileNumber1 = mObjGovernmentData.ContactNumber,
                        EmailAddress1 = mObjGovernmentData.ContactEmail,
                        TaxOfficeID = mObjGovernmentData.TaxOfficeID,
                        TaxOfficeName = mObjGovernmentData.TaxOfficeName,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                        NotificationMethodID = mObjGovernmentData.NotificationMethodID.GetValueOrDefault(),
                        NotificationMethodName = mObjGovernmentData.NotificationMethodName,
                        ContactAddress = mObjGovernmentData.ContactAddress,
                        Active = true,
                    };


                    MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                    {
                        TaxPayerID = id.GetValueOrDefault(),
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual
                    };

                    IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);
                    IList<AssetDetails> lstAssetDetails = new List<AssetDetails>();


                    foreach (var item in lstTaxPayerAsset)
                    {
                        AssetDetails mObjAssetDetails = new AssetDetails();
                        mObjAssetDetails.TPAID = item.TPAID.GetValueOrDefault();
                        mObjAssetDetails.AssetTypeID = item.AssetTypeID.GetValueOrDefault();
                        mObjAssetDetails.AssetTypeName = item.AssetTypeName;
                        mObjAssetDetails.AssetID = item.AssetID.GetValueOrDefault();
                        mObjAssetDetails.AssetLGA = item.AssetLGA;
                        mObjAssetDetails.AssetRIN = item.AssetRIN;
                        mObjAssetDetails.AssetName = item.AssetName;
                        mObjAssetDetails.BuildingUnitID = item.BuildingUnitID.GetValueOrDefault();
                        mObjAssetDetails.UnitNumber = item.UnitNumber;
                        mObjAssetDetails.Active = item.Active.GetValueOrDefault();
                        mObjAssetDetails.ActiveText = item.ActiveText;
                        mObjAssetDetails.RowID = lstAssetDetails.Count() + 1;

                        lstAssetDetails.Add(mObjAssetDetails);
                    }

                    SessionManager.lstAssetDetails = lstAssetDetails;
                    ViewBag.AssetList = lstAssetDetails;

                    usp_GetIndividualList_Result mObjIndvivdualData = new usp_GetIndividualList_Result()
                    {
                        TaxOfficeID = mObjGovernmentData.TaxOfficeID,
                        TaxPayerTypeID = mObjGovernmentData.TaxPayerTypeID,
                        NotificationMethodID = mObjGovernmentData.NotificationMethodID
                    };
                    UI_FillDropDown(mObjIndvivdualData);
                    return View(mObjCompanyModelView);
                }
                else
                {
                    return RedirectToAction("GovernmentToCorporate", "ConvertTaxPayer");
                }
            }
            else
            {
                return RedirectToAction("GovernmentToCorporate", "ConvertTaxPayer");
            }
        }


    }
}
