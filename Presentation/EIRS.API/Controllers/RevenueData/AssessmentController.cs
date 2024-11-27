using EIRS.API.Models;
using EIRS.API.Utility;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Http;

namespace EIRS.API.Controllers
{


    /// <summary>
    /// Assessment Operations
    /// </summary>
    [RoutePrefix("RevenueData/Assessment")]

    public class AssessmentController : BaseController
    {
        EIRSEntities _db = new EIRSEntities();

        IAssessmentRepository _AssessmentRepository;


        public AssessmentController()
        {
            _AssessmentRepository = new AssessmentRepository();
        }
        /// <summary>
        /// Returns a list of assessment
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Assessment mObjAssessment = new Assessment() { IntStatus = 1 };

                IList<usp_GetAssessmentList_Result> lstAssessment = new BLAssessment().BL_GetAssessmentList(mObjAssessment);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstAssessment;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }


        [HttpGet]
        [Route("DetailByRefNo/{refno}")]
        public IHttpActionResult DetailByRefNo(string refno)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                usp_GetAssessmentList_Result mObjAssessmentDetails = new BLAssessment().BL_GetAssessmentDetails(new Assessment() { AssessmentRefNo = refno, IntStatus = 1 });

                if (mObjAssessmentDetails != null)
                {
                    IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentItems = new BLAssessment().BL_GetAssessmentRuleItem(Convert.ToInt32(mObjAssessmentDetails.AssessmentID));
                    if (lstAssessmentItems.Count > 0)
                        mObjAssessmentDetails.AssessmentAmount = lstAssessmentItems.Sum(o => o.TotalAmount);
                    mObjAPIResponse.Success = true;
                    mObjAPIResponse.Result = mObjAssessmentDetails;
                }
                else
                {
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Result = "No Assessment Bill found with given reference number and mobile number";
                }
            }

            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("RuleDetail/{id}")]
        public IHttpActionResult RuleDetail(int? id)
        {
            APIResponse mObjAPIResponse = new APIResponse();
            try
            {
                if (id.GetValueOrDefault() > 0)
                {
                    List<usp_GetAssessment_AssessmentRuleList_Result> ugrls = new List<usp_GetAssessment_AssessmentRuleList_Result>();
                    IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = new BLAssessment().BL_GetAssessmentRules(id.GetValueOrDefault());
                    IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentItems = new BLAssessment().BL_GetAssessmentRuleItem(Convert.ToInt32(id.Value));
                    if (lstAssessmentItems.Count > 0)
                        foreach (var item in lstMAPAssessmentRules)
                        {
                            usp_GetAssessment_AssessmentRuleList_Result ugrl = new usp_GetAssessment_AssessmentRuleList_Result();
                            ugrl.AssessmentRuleAmount = lstAssessmentItems.Where(x => x.AARID == item.AARID).Sum(x => x.TotalAmount.Value);
                            ugrl.AssessmentRuleName = item.AssessmentRuleName;
                            ugrl.AssetRIN = item.AssetRIN;
                            ugrl.AssetID = item.AssetID;
                            ugrl.AARID = item.AARID;
                            ugrl.AssessmentRuleID = item.AssessmentRuleID;
                            ugrl.AssetTypeID = item.AssetTypeID;
                            ugrl.AssetTypeName = item.AssetTypeName;
                            ugrl.ProfileDescription = item.ProfileDescription;
                            ugrl.ProfileID = item.ProfileID;
                            ugrl.TaxYear = item.TaxYear;
                            ugrl.SettledAmount = item.SettledAmount;
                            ugrls.Add(ugrl);
                        }

                    mObjAPIResponse.Success = true;
                    mObjAPIResponse.Result = ugrls;

                }
                else
                {
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Result = "Invalid Request";
                }
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("ItemDetail/{id}")]
        public IHttpActionResult ItemDetail(int? id)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                if (id.GetValueOrDefault() > 0)
                {

                    IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentItems = new BLAssessment().BL_GetAssessmentRuleItem(id.GetValueOrDefault());

                    mObjAPIResponse.Success = true;
                    mObjAPIResponse.Result = lstAssessmentItems;

                }
                else
                {
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Result = "Invalid Request";
                }
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("RuleSettlementDetail/{id}")]
        public IHttpActionResult RuleSettlementDetail(int? id)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                if (id.GetValueOrDefault() > 0)
                {

                    IList<usp_GetAssessmentRuleBasedSettlement_Result> lstAssessmentRuleSettlement = new BLAssessment().BL_GetAssessmentRuleBasedSettlement(id.GetValueOrDefault());

                    mObjAPIResponse.Success = true;
                    mObjAPIResponse.Result = lstAssessmentRuleSettlement;

                }
                else
                {
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Result = "Invalid Request";
                }
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("SettlementDetail/{id}")]
        public IHttpActionResult SettlementDetail(int? id)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                if (id.GetValueOrDefault() > 0)
                {

                    IList<usp_GetSettlementList_Result> lstSettlement = new BLSettlement().BL_GetSettlementList(new Settlement() { ServiceBillID = -1, AssessmentID = id.GetValueOrDefault() });

                    mObjAPIResponse.Success = true;
                    mObjAPIResponse.Result = lstSettlement;

                }
                else
                {
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Result = "Invalid Request";
                }
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }


        /// <summary>
        /// Find assessment by id or ref no
        /// </summary>
        /// <param name="searchby"></param>
        /// <param name="searchtype"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Details/{searchby}/{searchtype}")]
        public IHttpActionResult Details(string searchby, string searchtype)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                if (!string.IsNullOrWhiteSpace(searchby))
                {
                    BLAssessment mObjBLAssessment = new BLAssessment();

                    usp_GetAssessmentList_Result mObjAssessmentDetails = null;

                    if (searchtype == "id")
                    {
                        mObjAssessmentDetails = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = TrynParse.parseInt(searchby), IntStatus = 2 });
                    }
                    else if (searchtype == "refno")
                    {
                        mObjAssessmentDetails = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentRefNo = searchby, IntStatus = 2 });
                    }

                    if (mObjAssessmentDetails != null)
                    {
                        IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = mObjBLAssessment.BL_GetAssessmentRules(mObjAssessmentDetails.AssessmentID.GetValueOrDefault());
                        IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentItems = mObjBLAssessment.BL_GetAssessmentRuleItem(mObjAssessmentDetails.AssessmentID.GetValueOrDefault());
                        IList<usp_GetAssessmentRuleBasedSettlement_Result> lstAssessmentRuleSettlement = mObjBLAssessment.BL_GetAssessmentRuleBasedSettlement(mObjAssessmentDetails.AssessmentID.GetValueOrDefault());

                        IList<DropDownListResult> lstSettlementMethod = mObjBLAssessment.BL_GetSettlementMethodAssessmentRuleBased(mObjAssessmentDetails.AssessmentID.GetValueOrDefault());

                        IList<usp_GetSettlementList_Result> lstSettlement = new BLSettlement().BL_GetSettlementList(new Settlement() { ServiceBillID = -1, AssessmentID = mObjAssessmentDetails.AssessmentID.GetValueOrDefault() });

                        IDictionary<string, object> dcResponse = new Dictionary<string, object>
                        {
                            { "AssessmentDetails", mObjAssessmentDetails },
                            { "AssessmentRuleDetails", lstMAPAssessmentRules },
                            { "AssessmentRuleItemDetails", lstAssessmentItems },
                            {"SettlementMethodList",lstSettlementMethod },
                            { "SettlementDetails",lstAssessmentRuleSettlement },
                            { "Settlements",lstSettlement }
                        };

                        mObjAPIResponse.Success = true;
                        mObjAPIResponse.Result = dcResponse;
                    }
                    else
                    {
                        mObjAPIResponse.Success = false;
                        mObjAPIResponse.Result = "Invalid Request";
                    }
                }
                else
                {
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Result = "Invalid Request";
                }
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }


        [HttpGet]
        [Route("TaxPayerBill")]
        public IHttpActionResult TaxPayerBill(int TaxPayerTypeID, int TaxPayerID)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBill_Result> lstTaxPayerBill = new BLAssessment().BL_GetTaxPayerBill(TaxPayerID, TaxPayerTypeID, 0);

                foreach (var item in lstTaxPayerBill)
                {
                    string email = null;

                    switch (TaxPayerTypeID)
                    {
                        case 1:
                            var individual = _db.Individuals.FirstOrDefault(x => x.IndividualID == TaxPayerID && x.TaxPayerTypeID == TaxPayerTypeID);
                            email = individual?.EmailAddress1;
                            break;
                        case 2:
                            var company = _db.Companies.FirstOrDefault(x => x.CompanyID == TaxPayerID && x.TaxPayerTypeID == TaxPayerTypeID);
                            email = company?.EmailAddress1;
                            break;
                        case 3:
                            var special = _db.Specials.FirstOrDefault(x => x.SpecialID == TaxPayerID && x.TaxPayerTypeID == TaxPayerTypeID);
                            email = special?.ContactEmail;
                            break;
                        case 4:
                            var government = _db.Governments.FirstOrDefault(x => x.GovernmentID == TaxPayerID && x.TaxPayerTypeID == TaxPayerTypeID);
                            email = government?.ContactEmail;
                            break;
                        default:
                            throw new ArgumentException("Invalid TaxPayerTypeID");
                    }
                    item.Email = email;
                }

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstTaxPayerBill;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }



        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert(AssessmentModel pObjAssessmentModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            String token = Request.Headers.Authorization.Parameter;
            int? userId = Utilities.GetUserId(token);
            if (!ModelState.IsValid)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }
            else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    //Creating Assessment Items
                    BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();
                    usp_GetAssessmentItemList_Result mObjAssessmentItemData = null; MAP_Assessment_AssessmentItem mObjAIDetail = null;
                    IList<MAP_Assessment_AssessmentItem> lstAssessmentItem = new List<MAP_Assessment_AssessmentItem>();

                    IList<MAP_Assessment_AssessmentRule> lstAssessmentRule = new List<MAP_Assessment_AssessmentRule>();
                    MAP_Assessment_AssessmentRule mObjAssessmentRuleDetails = new MAP_Assessment_AssessmentRule()
                    {
                        AARID = 0,
                        TaxPayerID = pObjAssessmentModel.TaxPayerID,
                        TaxPayerTypeID = pObjAssessmentModel.TaxPayerTypeID,
                        AssetTypeID = pObjAssessmentModel.AssetTypeID,
                        AssetID = pObjAssessmentModel.AssetID,
                        ProfileID = pObjAssessmentModel.ProfileID,
                        AssessmentRuleID = pObjAssessmentModel.AssessmentRuleID,
                        AssessmentYear = pObjAssessmentModel.TaxYear,
                        CreatedBy = userId.HasValue ? userId : 22,
                        CreatedDate = CommUtil.GetCurrentDateTime(),
                    };

                    lstAssessmentRule.Add(mObjAssessmentRuleDetails);
                    BLAssessment mObjBLAssessment = new BLAssessment();

                    Assessment mObjAssessment = new Assessment()
                    {
                        AssessmentID = 0,
                        TaxPayerID = pObjAssessmentModel.TaxPayerID,
                        TaxPayerTypeID = pObjAssessmentModel.TaxPayerTypeID,
                        AssessmentAmount = pObjAssessmentModel.LstAssessmentItem.Count > 0 ? pObjAssessmentModel.LstAssessmentItem.Sum(t => t.TaxBaseAmount) : 0,
                        AssessmentDate = CommUtil.GetCurrentDateTime(),
                        SettlementDueDate = CommUtil.GetCurrentDateTime().AddMonths(1),
                        SettlementStatusID = (int)EnumList.SettlementStatus.Assessed,
                        AssessmentNotes = pObjAssessmentModel.Notes,
                        Active = true,
                        CreatedBy = userId.HasValue ? userId : 22,
                        CreatedDate = CommUtil.GetCurrentDateTime(),
                    };
                    //
                    try
                    {
                        FuncResponse<Assessment> mObjAssessmentResponse = _AssessmentRepository.REP_InsertUpdateAssessment(mObjAssessment, pObjAssessmentModel.AssessmentRuleID, pObjAssessmentModel.AssetID);

                        if (mObjAssessmentResponse.Success)
                        {
                            var assessmentData = mObjAssessmentResponse.AdditionalData;
                            mObjAssessmentRuleDetails.AssessmentID = assessmentData.AssessmentID;
                            mObjAssessmentRuleDetails.AssessmentAmount = assessmentData.AssessmentAmount;

                            FuncResponse<MAP_Assessment_AssessmentRule> mObjARResponse = mObjBLAssessment.BL_InsertUpdateAssessmentRule(mObjAssessmentRuleDetails);

                            if (mObjARResponse.Success)
                            {
                                var assessmentRuleData = mObjARResponse.AdditionalData;

                                foreach (var item in pObjAssessmentModel.LstAssessmentItem)
                                {
                                    mObjAssessmentItemData = mObjBLAssessmentItem.BL_GetAssessmentItemDetails(new Assessment_Items() { AssessmentItemID = item.AssessmentItemID, intStatus = (int)EnumList.SettlementStatus.Notified });

                                    if (mObjAssessmentItemData != null)
                                    {
                                        mObjAIDetail = new MAP_Assessment_AssessmentItem()
                                        {
                                            AAIID = 0,
                                            AARID = assessmentRuleData.AARID,
                                            AssessmentItemID = mObjAssessmentItemData.AssessmentItemID,
                                            TaxAmount = item.TaxBaseAmount,
                                            TaxBaseAmount = item.TaxBaseAmount,
                                            Percentage = mObjAssessmentItemData.Percentage,
                                            PaymentStatusID = (int)EnumList.PaymentStatus.Due,
                                            CreatedBy = userId.HasValue ? userId : 22,
                                            CreatedDate = CommUtil.GetCurrentDateTime(),
                                        };

                                        lstAssessmentItem.Add(mObjAIDetail);
                                    }
                                    else
                                    {
                                        mObjAPIResponse.Success = false;
                                        mObjAPIResponse.Message = "Invalid Assessment Details";
                                        break;
                                    }
                                }

                                FuncResponse mObjADResponse = mObjBLAssessment.BL_InsertAssessmentItem(lstAssessmentItem);

                                if (!mObjADResponse.Success)
                                {
                                    mObjAPIResponse.Success = false;
                                    mObjAPIResponse.Message = mObjAssessmentResponse.Message;
                                    Transaction.Current.Rollback();
                                }
                            }
                            else
                            {
                                mObjAPIResponse.Success = false;
                                mObjAPIResponse.Message = mObjAssessmentResponse.Message;
                                Transaction.Current.Rollback();
                            }

                            //Send Notification
                            if (GlobalDefaultValues.SendNotification)
                            {
                                var mObjAssessmentData = mObjAssessmentResponse.AdditionalData;
                                //usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = mObjAssessmentResponse.AdditionalData.AssessmentID, IntStatus = (int)EnumList.SettlementStatus.Assessed });
                                IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = mObjBLAssessment.BL_GetAssessmentRules(mObjAssessmentResponse.AdditionalData.AssessmentID);

                                string AssessmentRuleNames = string.Join(",", lstMAPAssessmentRules.Select(t => t.AssessmentRuleName).ToArray());
                                //string AssessmentRuleNames = string.Join(",", lstAssessmentRule.Select(t => t.n).ToArray());


                                if (mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
                                {
                                    usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = (int)EnumList.SettlementStatus.Notified, IndividualID = mObjAssessmentData.TaxPayerID.GetValueOrDefault() });

                                    if (mObjIndividualData != null && mObjAssessmentData != null)
                                    {
                                        EmailDetails mObjEmailDetails = new EmailDetails()
                                        {
                                            TaxPayerTypeID = mObjIndividualData.TaxPayerTypeID.GetValueOrDefault(),
                                            TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
                                            TaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                                            TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                                            TaxPayerRIN = mObjIndividualData.IndividualRIN,
                                            TaxPayerMobileNumber = mObjIndividualData.MobileNumber1,
                                            TaxPayerEmail = mObjIndividualData.EmailAddress1,
                                            BillRefNo = mObjAssessmentData.AssessmentRefNo,
                                            BillAmount = CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount),
                                            BillTypeName = "Assessment Bill",
                                            LoggedInUserID = -1,
                                            RuleNames = AssessmentRuleNames
                                        };

                                        if (!string.IsNullOrWhiteSpace(mObjIndividualData.EmailAddress1))
                                        {
                                            BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
                                        }

                                        if (!string.IsNullOrWhiteSpace(mObjIndividualData.MobileNumber1))
                                        {
                                            BLSMSHandler.BL_BillGenerated(mObjEmailDetails);
                                        }
                                    }
                                }
                                else if (mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
                                {
                                    usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = (int)EnumList.SettlementStatus.Notified, CompanyID = mObjAssessmentData.TaxPayerID.GetValueOrDefault() });
                                    if (mObjCompanyData != null && mObjAssessmentData != null)
                                    {
                                        EmailDetails mObjEmailDetails = new EmailDetails()
                                        {
                                            TaxPayerTypeID = mObjCompanyData.TaxPayerTypeID.GetValueOrDefault(),
                                            TaxPayerTypeName = mObjCompanyData.TaxPayerTypeName,
                                            TaxPayerID = mObjCompanyData.CompanyID.GetValueOrDefault(),
                                            TaxPayerName = mObjCompanyData.CompanyName,
                                            TaxPayerRIN = mObjCompanyData.CompanyRIN,
                                            TaxPayerMobileNumber = mObjCompanyData.MobileNumber1,
                                            TaxPayerEmail = mObjCompanyData.EmailAddress1,
                                            BillRefNo = mObjAssessmentData.AssessmentRefNo,
                                            BillAmount = CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount),
                                            BillTypeName = "Assessment Bill",
                                            LoggedInUserID = -1,
                                            RuleNames = AssessmentRuleNames
                                        };

                                        if (!string.IsNullOrWhiteSpace(mObjCompanyData.EmailAddress1))
                                        {
                                            BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
                                        }

                                        if (!string.IsNullOrWhiteSpace(mObjCompanyData.MobileNumber1))
                                        {
                                            BLSMSHandler.BL_BillGenerated(mObjEmailDetails);
                                        }
                                    }
                                }
                                else if (mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
                                {
                                    usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(new Government() { intStatus = (int)EnumList.SettlementStatus.Notified, GovernmentID = mObjAssessmentData.TaxPayerID.GetValueOrDefault() });
                                    if (mObjGovernmentData != null && mObjAssessmentData != null)
                                    {
                                        EmailDetails mObjEmailDetails = new EmailDetails()
                                        {
                                            TaxPayerTypeID = mObjGovernmentData.TaxPayerTypeID.GetValueOrDefault(),
                                            TaxPayerTypeName = mObjGovernmentData.TaxPayerTypeName,
                                            TaxPayerID = mObjGovernmentData.GovernmentID.GetValueOrDefault(),
                                            TaxPayerName = mObjGovernmentData.GovernmentName,
                                            TaxPayerRIN = mObjGovernmentData.GovernmentRIN,
                                            TaxPayerMobileNumber = mObjGovernmentData.ContactNumber,
                                            TaxPayerEmail = mObjGovernmentData.ContactEmail,
                                            BillRefNo = mObjAssessmentData.AssessmentRefNo,
                                            BillAmount = CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount),
                                            BillTypeName = "Assessment Bill",
                                            LoggedInUserID = -1,
                                            RuleNames = AssessmentRuleNames
                                        };

                                        if (!string.IsNullOrWhiteSpace(mObjGovernmentData.ContactEmail))
                                        {
                                            BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
                                        }

                                        if (!string.IsNullOrWhiteSpace(mObjGovernmentData.ContactNumber))
                                        {
                                            BLSMSHandler.BL_BillGenerated(mObjEmailDetails);
                                        }
                                    }
                                }
                                else if (mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
                                {
                                    usp_GetSpecialList_Result mObjSpecialData = new BLSpecial().BL_GetSpecialDetails(new Special() { intStatus = (int)EnumList.SettlementStatus.Notified, SpecialID = mObjAssessmentData.TaxPayerID.GetValueOrDefault() });
                                    if (mObjSpecialData != null && mObjAssessmentData != null)
                                    {
                                        EmailDetails mObjEmailDetails = new EmailDetails()
                                        {
                                            TaxPayerTypeID = mObjSpecialData.TaxPayerTypeID.GetValueOrDefault(),
                                            TaxPayerTypeName = mObjSpecialData.TaxPayerTypeName,
                                            TaxPayerID = mObjSpecialData.SpecialID.GetValueOrDefault(),
                                            TaxPayerName = mObjSpecialData.SpecialTaxPayerName,
                                            TaxPayerRIN = mObjSpecialData.SpecialRIN,
                                            TaxPayerMobileNumber = mObjSpecialData.ContactNumber,
                                            TaxPayerEmail = mObjSpecialData.ContactEmail,
                                            BillRefNo = mObjAssessmentData.AssessmentRefNo,
                                            BillAmount = CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount),
                                            BillTypeName = "Assessment Bill",
                                            LoggedInUserID = -1,
                                            RuleNames = AssessmentRuleNames
                                        };

                                        if (!string.IsNullOrWhiteSpace(mObjSpecialData.ContactEmail))
                                        {
                                            BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
                                        }

                                        if (!string.IsNullOrWhiteSpace(mObjSpecialData.ContactNumber))
                                        {
                                            BLSMSHandler.BL_BillGenerated(mObjEmailDetails);
                                        }
                                    }
                                }
                            }

                            scope.Complete();
                            mObjAPIResponse.Success = true;
                            mObjAPIResponse.Message = "Assessment Added Successfully";
                            mObjAPIResponse.Result = mObjAssessmentResponse.AdditionalData.AssessmentRefNo;
                        }
                        else
                        {
                            mObjAPIResponse.Success = false;
                            mObjAPIResponse.Message = mObjAssessmentResponse.Message;
                            mObjAPIResponse.Result = mObjAssessmentResponse.AdditionalData.AssessmentRefNo;
                            Transaction.Current.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        mObjAPIResponse.Success = false;
                        if (ex.Message == "ARALREADY")
                        {
                            mObjAPIResponse.Message = "Assessment rules added multiple times and not valid.";
                        }
                        else if (ex.Message == "ARNOTFOUND")
                        {
                            mObjAPIResponse.Message = "Assessment rules not found in assessment.";
                        }
                        else if (ex.Message == "AINOTFOUND")
                        {
                            mObjAPIResponse.Message = "Assessment items not found in assessment.";
                        }
                        else
                        {
                            mObjAPIResponse.Message = "Error occurred while saving assessment";
                        }

                        Transaction.Current.Rollback();
                    }
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("InsertWithMultipleRule")]
        public IHttpActionResult InsertWithMultipleRule(AssessmentWithMultipleRuleModel pObjAssessmentModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            String token = Request.Headers.Authorization.Parameter;
            int? userId = Utilities.GetUserId(token);
            if (!ModelState.IsValid)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }
            else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    //Creating Assessment Items
                    BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();
                    usp_GetAssessmentItemList_Result mObjAssessmentItemData = null; MAP_Assessment_AssessmentItem mObjAIDetail = null;
                    IList<MAP_Assessment_AssessmentItem> lstAssessmentItem = new List<MAP_Assessment_AssessmentItem>();

                    foreach (var item in pObjAssessmentModel.LstAssessmentRule.FirstOrDefault().LstAssessmentItem)
                    {
                        mObjAssessmentItemData = mObjBLAssessmentItem.BL_GetAssessmentItemDetails(new Assessment_Items() { AssessmentItemID = item.AssessmentItemID, intStatus = 2 });

                        if (mObjAssessmentItemData != null)
                        {
                            mObjAIDetail = new MAP_Assessment_AssessmentItem()
                            {
                                AARID = 0,
                                AAIID = 0,
                                AssessmentItemID = mObjAssessmentItemData.AssessmentItemID,
                                Percentage = mObjAssessmentItemData.Percentage,
                                PaymentStatusID = (int)EnumList.PaymentStatus.Due,
                                CreatedBy = userId.HasValue ? userId :  22,
                                CreatedDate = CommUtil.GetCurrentDateTime(),
                                TaxBaseAmount = item.TaxBaseAmount
                            };

                            lstAssessmentItem.Add(mObjAIDetail);
                        }
                        else
                        {
                            mObjAPIResponse.Success = false;
                            mObjAPIResponse.Message = "Invalid Assessment Details";
                            break;
                        }
                    }

                    var assessmentRule = pObjAssessmentModel.LstAssessmentRule.FirstOrDefault();
                    IList<MAP_Assessment_AssessmentRule> lstAssessmentRule = new List<MAP_Assessment_AssessmentRule>();
                    MAP_Assessment_AssessmentRule mObjAssessmentRuleDetails = new MAP_Assessment_AssessmentRule()
                    {
                        AARID = 0,
                        TaxPayerID = pObjAssessmentModel.TaxPayerID,
                        TaxPayerTypeID = pObjAssessmentModel.TaxPayerTypeID,
                        AssetTypeID = assessmentRule.AssetTypeID,
                        AssetID = assessmentRule.AssetID,
                        ProfileID = assessmentRule.ProfileID,
                        AssessmentRuleID = assessmentRule.AssessmentRuleID,
                        AssessmentYear = assessmentRule.TaxYear,
                        CreatedBy = userId.HasValue ? userId : 22,
                        CreatedDate = CommUtil.GetCurrentDateTime(),
                    };

                    lstAssessmentRule.Add(mObjAssessmentRuleDetails);
                    BLAssessment mObjBLAssessment = new BLAssessment();

                    Assessment mObjAssessment = new Assessment()
                    {
                        AssessmentID = 0,
                        TaxPayerID = pObjAssessmentModel.TaxPayerID,
                        TaxPayerTypeID = pObjAssessmentModel.TaxPayerTypeID,
                        AssessmentAmount = lstAssessmentItem.Count > 0 ? lstAssessmentItem.Sum(t => t.TaxBaseAmount) : 0,
                        AssessmentDate = CommUtil.GetCurrentDateTime(),
                        SettlementDueDate = CommUtil.GetCurrentDateTime().AddMonths(1),
                        SettlementStatusID = (int)EnumList.SettlementStatus.Assessed,
                        AssessmentNotes = pObjAssessmentModel.Notes,
                        Active = true,
                        CreatedBy = userId.HasValue ? userId : 22,
                        CreatedDate = CommUtil.GetCurrentDateTime(),
                    };

                    try
                    {
                        FuncResponse<Assessment> mObjAssessmentResponse = mObjBLAssessment.BL_InsertUpdateAssessment(mObjAssessment);

                        if (mObjAssessmentResponse.Success)
                        {
                            //Send Notification
                            if (GlobalDefaultValues.SendNotification)
                            {
                                usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = mObjAssessmentResponse.AdditionalData.AssessmentID, IntStatus = 2 });
                                IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = mObjBLAssessment.BL_GetAssessmentRules(mObjAssessmentResponse.AdditionalData.AssessmentID);

                                string AssessmentRuleNames = string.Join(",", lstMAPAssessmentRules.Select(t => t.AssessmentRuleName).ToArray());

                                if (mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
                                {
                                    usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 2, IndividualID = mObjAssessmentData.TaxPayerID.GetValueOrDefault() });

                                    if (mObjIndividualData != null && mObjAssessmentData != null)
                                    {
                                        EmailDetails mObjEmailDetails = new EmailDetails()
                                        {
                                            TaxPayerTypeID = mObjIndividualData.TaxPayerTypeID.GetValueOrDefault(),
                                            TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
                                            TaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault(),
                                            TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
                                            TaxPayerRIN = mObjIndividualData.IndividualRIN,
                                            TaxPayerMobileNumber = mObjIndividualData.MobileNumber1,
                                            TaxPayerEmail = mObjIndividualData.EmailAddress1,
                                            BillRefNo = mObjAssessmentData.AssessmentRefNo,
                                            BillAmount = CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount),
                                            BillTypeName = "Assessment Bill",
                                            LoggedInUserID = -1,
                                            RuleNames = AssessmentRuleNames
                                        };

                                        if (!string.IsNullOrWhiteSpace(mObjIndividualData.EmailAddress1))
                                        {
                                            BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
                                        }

                                        if (!string.IsNullOrWhiteSpace(mObjIndividualData.MobileNumber1))
                                        {
                                            BLSMSHandler.BL_BillGenerated(mObjEmailDetails);
                                        }
                                    }
                                }
                                else if (mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
                                {
                                    usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = mObjAssessmentData.TaxPayerID.GetValueOrDefault() });
                                    if (mObjCompanyData != null && mObjAssessmentData != null)
                                    {
                                        EmailDetails mObjEmailDetails = new EmailDetails()
                                        {
                                            TaxPayerTypeID = mObjCompanyData.TaxPayerTypeID.GetValueOrDefault(),
                                            TaxPayerTypeName = mObjCompanyData.TaxPayerTypeName,
                                            TaxPayerID = mObjCompanyData.CompanyID.GetValueOrDefault(),
                                            TaxPayerName = mObjCompanyData.CompanyName,
                                            TaxPayerRIN = mObjCompanyData.CompanyRIN,
                                            TaxPayerMobileNumber = mObjCompanyData.MobileNumber1,
                                            TaxPayerEmail = mObjCompanyData.EmailAddress1,
                                            BillRefNo = mObjAssessmentData.AssessmentRefNo,
                                            BillAmount = CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount),
                                            BillTypeName = "Assessment Bill",
                                            LoggedInUserID = -1,
                                            RuleNames = AssessmentRuleNames
                                        };

                                        if (!string.IsNullOrWhiteSpace(mObjCompanyData.EmailAddress1))
                                        {
                                            BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
                                        }

                                        if (!string.IsNullOrWhiteSpace(mObjCompanyData.MobileNumber1))
                                        {
                                            BLSMSHandler.BL_BillGenerated(mObjEmailDetails);
                                        }
                                    }
                                }
                                else if (mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
                                {
                                    usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(new Government() { intStatus = 2, GovernmentID = mObjAssessmentData.TaxPayerID.GetValueOrDefault() });
                                    if (mObjGovernmentData != null && mObjAssessmentData != null)
                                    {
                                        EmailDetails mObjEmailDetails = new EmailDetails()
                                        {
                                            TaxPayerTypeID = mObjGovernmentData.TaxPayerTypeID.GetValueOrDefault(),
                                            TaxPayerTypeName = mObjGovernmentData.TaxPayerTypeName,
                                            TaxPayerID = mObjGovernmentData.GovernmentID.GetValueOrDefault(),
                                            TaxPayerName = mObjGovernmentData.GovernmentName,
                                            TaxPayerRIN = mObjGovernmentData.GovernmentRIN,
                                            TaxPayerMobileNumber = mObjGovernmentData.ContactNumber,
                                            TaxPayerEmail = mObjGovernmentData.ContactEmail,
                                            BillRefNo = mObjAssessmentData.AssessmentRefNo,
                                            BillAmount = CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount),
                                            BillTypeName = "Assessment Bill",
                                            LoggedInUserID = -1,
                                            RuleNames = AssessmentRuleNames
                                        };

                                        if (!string.IsNullOrWhiteSpace(mObjGovernmentData.ContactEmail))
                                        {
                                            BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
                                        }

                                        if (!string.IsNullOrWhiteSpace(mObjGovernmentData.ContactNumber))
                                        {
                                            BLSMSHandler.BL_BillGenerated(mObjEmailDetails);
                                        }
                                    }
                                }
                                else if (mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
                                {
                                    usp_GetSpecialList_Result mObjSpecialData = new BLSpecial().BL_GetSpecialDetails(new Special() { intStatus = 2, SpecialID = mObjAssessmentData.TaxPayerID.GetValueOrDefault() });
                                    if (mObjSpecialData != null && mObjAssessmentData != null)
                                    {
                                        EmailDetails mObjEmailDetails = new EmailDetails()
                                        {
                                            TaxPayerTypeID = mObjSpecialData.TaxPayerTypeID.GetValueOrDefault(),
                                            TaxPayerTypeName = mObjSpecialData.TaxPayerTypeName,
                                            TaxPayerID = mObjSpecialData.SpecialID.GetValueOrDefault(),
                                            TaxPayerName = mObjSpecialData.SpecialTaxPayerName,
                                            TaxPayerRIN = mObjSpecialData.SpecialRIN,
                                            TaxPayerMobileNumber = mObjSpecialData.ContactNumber,
                                            TaxPayerEmail = mObjSpecialData.ContactEmail,
                                            BillRefNo = mObjAssessmentData.AssessmentRefNo,
                                            BillAmount = CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount),
                                            BillTypeName = "Assessment Bill",
                                            LoggedInUserID = -1,
                                            RuleNames = AssessmentRuleNames
                                        };

                                        if (!string.IsNullOrWhiteSpace(mObjSpecialData.ContactEmail))
                                        {
                                            BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
                                        }

                                        if (!string.IsNullOrWhiteSpace(mObjSpecialData.ContactNumber))
                                        {
                                            BLSMSHandler.BL_BillGenerated(mObjEmailDetails);
                                        }
                                    }
                                }
                            }

                            scope.Complete();
                            mObjAPIResponse.Success = true;
                            mObjAPIResponse.Result = mObjAssessmentResponse.AdditionalData.AssessmentRefNo;
                            mObjAPIResponse.Message = "Assessment Added Successfully";

                        }
                        else
                        {
                            mObjAPIResponse.Success = false;
                            mObjAPIResponse.Message = mObjAssessmentResponse.Message;
                            Transaction.Current.Rollback();

                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        mObjAPIResponse.Success = false;
                        if (ex.Message == "ARALREADY")
                        {
                            mObjAPIResponse.Message = "Assessment rules added multiple times and not valid.";
                        }
                        else if (ex.Message == "ARNOTFOUND")
                        {
                            mObjAPIResponse.Message = "Assessment rules not found in assessment.";
                        }
                        else if (ex.Message == "AINOTFOUND")
                        {
                            mObjAPIResponse.Message = "Assessment items not found in assessment.";
                        }
                        else
                        {
                            mObjAPIResponse.Message = "Error occurred while saving assessment";
                        }

                        Transaction.Current.Rollback();
                    }
                }

            }

            return Ok(mObjAPIResponse);
        }
    }
}

//[HttpPost]
//        [Route("Insert")]
//        public IHttpActionResult Insert(AssessmentModel pObjAssessmentModel)
//        {
//            APIResponse mObjAPIResponse = new APIResponse();

//            if (!ModelState.IsValid)
//            {
//                mObjAPIResponse.Success = false;
//                mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
//            }
//            else
//            {
//               // var identity = User.Identity.GetUserID();
//                using (TransactionScope scope = new TransactionScope())
//                {
//                    //Creating Assessment Items
//                    BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();
//                    usp_GetAssessmentItemList_Result mObjAssessmentItemData = null; MAP_Assessment_AssessmentItem mObjAIDetail = null;
//                    IList<MAP_Assessment_AssessmentItem> lstAssessmentItem = new List<MAP_Assessment_AssessmentItem>();

//                    foreach (var item in pObjAssessmentModel.LstAssessmentItem)
//                    {
//                        mObjAssessmentItemData = mObjBLAssessmentItem.BL_GetAssessmentItemDetails(new Assessment_Items() { AssessmentItemID = item.AssessmentItemID, intStatus = 2 });

//                        if (mObjAssessmentItemData != null)
//                        {
//                            mObjAIDetail = new MAP_Assessment_AssessmentItem()
//                            {
//                                AARID = 0,
//                                AAIID = 0,
//                                AssessmentItemID = mObjAssessmentItemData.AssessmentItemID,
//                                Percentage = mObjAssessmentItemData.Percentage,
//                                PaymentStatusID = (int)EnumList.PaymentStatus.Due,
//                               // CreatedBy = 22,
//                                CreatedBy = 22,
//                                CreatedDate = CommUtil.GetCurrentDateTime(),
//                                TaxBaseAmount = item.TaxBaseAmount
//                            };

//                            lstAssessmentItem.Add(mObjAIDetail);
//                        }
//                        else
//                        {
//                            mObjAPIResponse.Success = false;
//                            mObjAPIResponse.Message = "Invalid Assessment Details";
//                            break;
//                        }
//                    }

//                    IList<MAP_Assessment_AssessmentRule> lstAssessmentRule = new List<MAP_Assessment_AssessmentRule>();
//                    MAP_Assessment_AssessmentRule mObjAssessmentRuleDetails = new MAP_Assessment_AssessmentRule()
//                    {
//                        AARID = 0,
//                        TaxPayerID = pObjAssessmentModel.TaxPayerID,
//                        TaxPayerTypeID = pObjAssessmentModel.TaxPayerTypeID,
//                        AssetTypeID = pObjAssessmentModel.AssetTypeID,
//                        AssetID = pObjAssessmentModel.AssetID,
//                        ProfileID = pObjAssessmentModel.ProfileID,
//                        AssessmentRuleID = pObjAssessmentModel.AssessmentRuleID,
//                        AssessmentYear = pObjAssessmentModel.TaxYear,
//                        CreatedBy = 22,
//                        CreatedDate = CommUtil.GetCurrentDateTime(),
//                    };

//                    lstAssessmentRule.Add(mObjAssessmentRuleDetails);
//                    BLAssessment mObjBLAssessment = new BLAssessment();
//                    Assessment mObjAssessment = new Assessment()
//                    {
//                        AssessmentID = 0,
//                        TaxPayerID = pObjAssessmentModel.TaxPayerID,
//                        TaxPayerTypeID = pObjAssessmentModel.TaxPayerTypeID,
//                        AssessmentAmount = lstAssessmentItem.Count > 0 ? lstAssessmentItem.Sum(t => t.TaxBaseAmount) : 0,
//                        AssessmentDate = CommUtil.GetCurrentDateTime(),
//                        SettlementDueDate = CommUtil.GetCurrentDateTime().AddMonths(1),
//                        SettlementStatusID = (int)EnumList.SettlementStatus.Assessed,
//                        AssessmentNotes = pObjAssessmentModel.Notes,
//                        Active = true,
//                        CreatedBy = 22,
//                        CreatedDate = CommUtil.GetCurrentDateTime(),
//                    };

//                    try
//                    {
//                        FuncResponse<Assessment> mObjAssessmentResponse =  _AssessmentRepository.REP_InsertUpdateAssessment(mObjAssessment, pObjAssessmentModel.AssessmentRuleID, pObjAssessmentModel.AssetID);

//                        if (mObjAssessmentResponse.Success)
//                        {
//                            //Send Notification
//                            if (GlobalDefaultValues.SendNotification)
//                            {
//                                usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = mObjAssessmentResponse.AdditionalData.AssessmentID, IntStatus = 2 });
//                                IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = mObjBLAssessment.BL_GetAssessmentRules(mObjAssessmentResponse.AdditionalData.AssessmentID);

//                                string AssessmentRuleNames = string.Join(",", lstMAPAssessmentRules.Select(t => t.AssessmentRuleName).ToArray());

//                                if (mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
//                                {
//                                    usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 2, IndividualID = mObjAssessmentData.TaxPayerID.GetValueOrDefault() });

//                                    if (mObjIndividualData != null && mObjAssessmentData != null)
//                                    {
//                                        EmailDetails mObjEmailDetails = new EmailDetails()
//                                        {
//                                            TaxPayerTypeID = mObjIndividualData.TaxPayerTypeID.GetValueOrDefault(),
//                                            TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
//                                            TaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault(),
//                                            TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
//                                            TaxPayerRIN = mObjIndividualData.IndividualRIN,
//                                            TaxPayerMobileNumber = mObjIndividualData.MobileNumber1,
//                                            TaxPayerEmail = mObjIndividualData.EmailAddress1,
//                                            BillRefNo = mObjAssessmentData.AssessmentRefNo,
//                                            BillAmount = CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount),
//                                            BillTypeName = "Assessment Bill",
//                                            LoggedInUserID = -1,
//                                            RuleNames = AssessmentRuleNames
//                                        };

//                                        if (!string.IsNullOrWhiteSpace(mObjIndividualData.EmailAddress1))
//                                        {
//                                            BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
//                                        }

//                                        if (!string.IsNullOrWhiteSpace(mObjIndividualData.MobileNumber1))
//                                        {
//                                            BLSMSHandler.BL_BillGenerated(mObjEmailDetails);
//                                        }
//                                    }
//                                }
//                                else if (mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
//                                {
//                                    usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = mObjAssessmentData.TaxPayerID.GetValueOrDefault() });
//                                    if (mObjCompanyData != null && mObjAssessmentData != null)
//                                    {
//                                        EmailDetails mObjEmailDetails = new EmailDetails()
//                                        {
//                                            TaxPayerTypeID = mObjCompanyData.TaxPayerTypeID.GetValueOrDefault(),
//                                            TaxPayerTypeName = mObjCompanyData.TaxPayerTypeName,
//                                            TaxPayerID = mObjCompanyData.CompanyID.GetValueOrDefault(),
//                                            TaxPayerName = mObjCompanyData.CompanyName,
//                                            TaxPayerRIN = mObjCompanyData.CompanyRIN,
//                                            TaxPayerMobileNumber = mObjCompanyData.MobileNumber1,
//                                            TaxPayerEmail = mObjCompanyData.EmailAddress1,
//                                            BillRefNo = mObjAssessmentData.AssessmentRefNo,
//                                            BillAmount = CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount),
//                                            BillTypeName = "Assessment Bill",
//                                            LoggedInUserID = -1,
//                                            RuleNames = AssessmentRuleNames
//                                        };

//                                        if (!string.IsNullOrWhiteSpace(mObjCompanyData.EmailAddress1))
//                                        {
//                                            BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
//                                        }

//                                        if (!string.IsNullOrWhiteSpace(mObjCompanyData.MobileNumber1))
//                                        {
//                                            BLSMSHandler.BL_BillGenerated(mObjEmailDetails);
//                                        }
//                                    }
//                                }
//                                else if (mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
//                                {
//                                    usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(new Government() { intStatus = 2, GovernmentID = mObjAssessmentData.TaxPayerID.GetValueOrDefault() });
//                                    if (mObjGovernmentData != null && mObjAssessmentData != null)
//                                    {
//                                        EmailDetails mObjEmailDetails = new EmailDetails()
//                                        {
//                                            TaxPayerTypeID = mObjGovernmentData.TaxPayerTypeID.GetValueOrDefault(),
//                                            TaxPayerTypeName = mObjGovernmentData.TaxPayerTypeName,
//                                            TaxPayerID = mObjGovernmentData.GovernmentID.GetValueOrDefault(),
//                                            TaxPayerName = mObjGovernmentData.GovernmentName,
//                                            TaxPayerRIN = mObjGovernmentData.GovernmentRIN,
//                                            TaxPayerMobileNumber = mObjGovernmentData.ContactNumber,
//                                            TaxPayerEmail = mObjGovernmentData.ContactEmail,
//                                            BillRefNo = mObjAssessmentData.AssessmentRefNo,
//                                            BillAmount = CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount),
//                                            BillTypeName = "Assessment Bill",
//                                            LoggedInUserID = -1,
//                                            RuleNames = AssessmentRuleNames
//                                        };

//                                        if (!string.IsNullOrWhiteSpace(mObjGovernmentData.ContactEmail))
//                                        {
//                                            BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
//                                        }

//                                        if (!string.IsNullOrWhiteSpace(mObjGovernmentData.ContactNumber))
//                                        {
//                                            BLSMSHandler.BL_BillGenerated(mObjEmailDetails);
//                                        }
//                                    }
//                                }
//                                else if (mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
//                                {
//                                    usp_GetSpecialList_Result mObjSpecialData = new BLSpecial().BL_GetSpecialDetails(new Special() { intStatus = 2, SpecialID = mObjAssessmentData.TaxPayerID.GetValueOrDefault() });
//                                    if (mObjSpecialData != null && mObjAssessmentData != null)
//                                    {
//                                        EmailDetails mObjEmailDetails = new EmailDetails()
//                                        {
//                                            TaxPayerTypeID = mObjSpecialData.TaxPayerTypeID.GetValueOrDefault(),
//                                            TaxPayerTypeName = mObjSpecialData.TaxPayerTypeName,
//                                            TaxPayerID = mObjSpecialData.SpecialID.GetValueOrDefault(),
//                                            TaxPayerName = mObjSpecialData.SpecialTaxPayerName,
//                                            TaxPayerRIN = mObjSpecialData.SpecialRIN,
//                                            TaxPayerMobileNumber = mObjSpecialData.ContactNumber,
//                                            TaxPayerEmail = mObjSpecialData.ContactEmail,
//                                            BillRefNo = mObjAssessmentData.AssessmentRefNo,
//                                            BillAmount = CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount),
//                                            BillTypeName = "Assessment Bill",
//                                            LoggedInUserID = -1,
//                                            RuleNames = AssessmentRuleNames
//                                        };

//                                        if (!string.IsNullOrWhiteSpace(mObjSpecialData.ContactEmail))
//                                        {
//                                            BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
//                                        }

//                                        if (!string.IsNullOrWhiteSpace(mObjSpecialData.ContactNumber))
//                                        {
//                                            BLSMSHandler.BL_BillGenerated(mObjEmailDetails);
//                                        }
//                                    }
//                                }
//                            }

//                            scope.Complete();
//                            mObjAPIResponse.Success = true;
//                            mObjAPIResponse.Result = mObjAssessmentResponse.AdditionalData.AssessmentRefNo;
//                            mObjAPIResponse.Message = "Assessment Added Successfully";

//                        }
//                        else
//                        {
//                            mObjAPIResponse.Success = false;
//                            mObjAPIResponse.Message = mObjAssessmentResponse.Message;
//                            Transaction.Current.Rollback();

//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        ErrorSignal.FromCurrentContext().Raise(ex);
//                        mObjAPIResponse.Success = false;
//                        if (ex.Message == "ARALREADY")
//                        {
//                            mObjAPIResponse.Message = "Assessment rules added multiple times and not valid.";
//                        }
//                        else if (ex.Message == "ARNOTFOUND")
//                        {
//                            mObjAPIResponse.Message = "Assessment rules not found in assessment.";
//                        }
//                        else if (ex.Message == "AINOTFOUND")
//                        {
//                            mObjAPIResponse.Message = "Assessment items not found in assessment.";
//                        }
//                        else
//                        {
//                            mObjAPIResponse.Message = "Error occurred while saving assessment";
//                        }

//                        Transaction.Current.Rollback();
//                    }
//                }
//            }

//            return Ok(mObjAPIResponse);
//        }
//[HttpPost]
//[Route("InsertPaye")]
//public IHttpActionResult InsertPaye(AssessmentModel pObjAssessmentModel)
//{
//    APIResponse mObjAPIResponse = new APIResponse();

//    using (TransactionScope scope = new TransactionScope())
//    {
//        BLAssessmentItem mObjBLAssessmentItem = new BLAssessmentItem();
//        usp_GetAssessmentItemList_Result mObjAssessmentItemData = null; MAP_Assessment_AssessmentItem mObjAIDetail = null;
//        IList<MAP_Assessment_AssessmentItem> lstAssessmentItem = new List<MAP_Assessment_AssessmentItem>();

//        foreach (var item in pObjAssessmentModel.LstAssessmentItem)
//        {
//            mObjAssessmentItemData = mObjBLAssessmentItem.BL_GetAssessmentItemDetails(new Assessment_Items() { AssessmentItemID = item.AssessmentItemID, intStatus = 2 });

//            if (mObjAssessmentItemData != null)
//            {
//                mObjAIDetail = new MAP_Assessment_AssessmentItem()
//                {
//                    AARID = 0,
//                    AAIID = 0,
//                    AssessmentItemID = mObjAssessmentItemData.AssessmentItemID,
//                    Percentage = mObjAssessmentItemData.Percentage,
//                    PaymentStatusID = (int)EnumList.PaymentStatus.Due,
//                    CreatedBy = 100,
//                    CreatedDate = CommUtil.GetCurrentDateTime(),
//                    TaxBaseAmount = item.TaxBaseAmount
//                };

//                lstAssessmentItem.Add(mObjAIDetail);
//            }
//            else
//            {
//                mObjAPIResponse.Success = false;
//                mObjAPIResponse.Message = "Invalid Assessment Details";
//                break;
//            }
//        }

//        IList<MAP_Assessment_AssessmentRule> lstAssessmentRule = new List<MAP_Assessment_AssessmentRule>();
//        MAP_Assessment_AssessmentRule mObjAssessmentRuleDetails = new MAP_Assessment_AssessmentRule()
//        {
//            AARID = 0,
//            TaxPayerID = pObjAssessmentModel.TaxPayerID,
//            TaxPayerTypeID = pObjAssessmentModel.TaxPayerTypeID,
//            AssetTypeID = pObjAssessmentModel.AssetTypeID,
//            AssetID = pObjAssessmentModel.AssetID,
//            ProfileID = pObjAssessmentModel.ProfileID,
//            AssessmentRuleID = pObjAssessmentModel.AssessmentRuleID,
//            AssessmentYear = pObjAssessmentModel.TaxYear,
//            CreatedBy = 100,
//            CreatedDate = CommUtil.GetCurrentDateTime(),
//        };

//        lstAssessmentRule.Add(mObjAssessmentRuleDetails);
//        BLAssessment mObjBLAssessment = new BLAssessment();
//        Assessment mObjAssessment = new Assessment()
//        {
//            AssessmentID = 0,
//            TaxPayerID = pObjAssessmentModel.TaxPayerID,
//            TaxPayerTypeID = pObjAssessmentModel.TaxPayerTypeID,
//            AssessmentAmount = lstAssessmentItem.Count > 0 ? lstAssessmentItem.Sum(t => t.TaxBaseAmount) : 0,
//            AssessmentDate = CommUtil.GetCurrentDateTime(),
//            SettlementDueDate = CommUtil.GetCurrentDateTime().AddMonths(1),
//            SettlementStatusID = (int)EnumList.SettlementStatus.Assessed,
//            AssessmentNotes = pObjAssessmentModel.Notes,
//            Active = true,
//            CreatedBy = 100,
//            CreatedDate = CommUtil.GetCurrentDateTime(),
//        };

//        try
//        {
//            FuncResponse<Assessment> mObjAssessmentResponse = mObjBLAssessment.REP_InsertUpdateAssessment(mObjAssessment, pObjAssessmentModel.AssessmentRuleID, pObjAssessmentModel.LstAssessmentItem.First().AssessmentItemID);

//            if (mObjAssessmentResponse.Success)
//            {
//                //Send Notification
//                if (GlobalDefaultValues.SendNotification)
//                {
//                    usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = mObjAssessmentResponse.AdditionalData.AssessmentID, IntStatus = 2 });
//                    IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = mObjBLAssessment.BL_GetAssessmentRules(mObjAssessmentResponse.AdditionalData.AssessmentID);

//                    string AssessmentRuleNames = string.Join(",", lstMAPAssessmentRules.Select(t => t.AssessmentRuleName).ToArray());

//                    if (mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
//                    {
//                        usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 2, IndividualID = mObjAssessmentData.TaxPayerID.GetValueOrDefault() });

//                        if (mObjIndividualData != null && mObjAssessmentData != null)
//                        {
//                            EmailDetails mObjEmailDetails = new EmailDetails()
//                            {
//                                TaxPayerTypeID = mObjIndividualData.TaxPayerTypeID.GetValueOrDefault(),
//                                TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
//                                TaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault(),
//                                TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
//                                TaxPayerRIN = mObjIndividualData.IndividualRIN,
//                                TaxPayerMobileNumber = mObjIndividualData.MobileNumber1,
//                                TaxPayerEmail = mObjIndividualData.EmailAddress1,
//                                BillRefNo = mObjAssessmentData.AssessmentRefNo,
//                                BillAmount = CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount),
//                                BillTypeName = "Assessment Bill",
//                                LoggedInUserID = -1,
//                                RuleNames = AssessmentRuleNames
//                            };

//                            if (!string.IsNullOrWhiteSpace(mObjIndividualData.EmailAddress1))
//                            {
//                                BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
//                            }

//                            if (!string.IsNullOrWhiteSpace(mObjIndividualData.MobileNumber1))
//                            {
//                                BLSMSHandler.BL_BillGenerated(mObjEmailDetails);
//                            }
//                        }
//                    }
//                    else if (mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
//                    {
//                        usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = mObjAssessmentData.TaxPayerID.GetValueOrDefault() });
//                        if (mObjCompanyData != null && mObjAssessmentData != null)
//                        {
//                            EmailDetails mObjEmailDetails = new EmailDetails()
//                            {
//                                TaxPayerTypeID = mObjCompanyData.TaxPayerTypeID.GetValueOrDefault(),
//                                TaxPayerTypeName = mObjCompanyData.TaxPayerTypeName,
//                                TaxPayerID = mObjCompanyData.CompanyID.GetValueOrDefault(),
//                                TaxPayerName = mObjCompanyData.CompanyName,
//                                TaxPayerRIN = mObjCompanyData.CompanyRIN,
//                                TaxPayerMobileNumber = mObjCompanyData.MobileNumber1,
//                                TaxPayerEmail = mObjCompanyData.EmailAddress1,
//                                BillRefNo = mObjAssessmentData.AssessmentRefNo,
//                                BillAmount = CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount),
//                                BillTypeName = "Assessment Bill",
//                                LoggedInUserID = -1,
//                                RuleNames = AssessmentRuleNames
//                            };

//                            if (!string.IsNullOrWhiteSpace(mObjCompanyData.EmailAddress1))
//                            {
//                                BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
//                            }

//                            if (!string.IsNullOrWhiteSpace(mObjCompanyData.MobileNumber1))
//                            {
//                                BLSMSHandler.BL_BillGenerated(mObjEmailDetails);
//                            }
//                        }
//                    }
//                    else if (mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
//                    {
//                        usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(new Government() { intStatus = 2, GovernmentID = mObjAssessmentData.TaxPayerID.GetValueOrDefault() });
//                        if (mObjGovernmentData != null && mObjAssessmentData != null)
//                        {
//                            EmailDetails mObjEmailDetails = new EmailDetails()
//                            {
//                                TaxPayerTypeID = mObjGovernmentData.TaxPayerTypeID.GetValueOrDefault(),
//                                TaxPayerTypeName = mObjGovernmentData.TaxPayerTypeName,
//                                TaxPayerID = mObjGovernmentData.GovernmentID.GetValueOrDefault(),
//                                TaxPayerName = mObjGovernmentData.GovernmentName,
//                                TaxPayerRIN = mObjGovernmentData.GovernmentRIN,
//                                TaxPayerMobileNumber = mObjGovernmentData.ContactNumber,
//                                TaxPayerEmail = mObjGovernmentData.ContactEmail,
//                                BillRefNo = mObjAssessmentData.AssessmentRefNo,
//                                BillAmount = CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount),
//                                BillTypeName = "Assessment Bill",
//                                LoggedInUserID = -1,
//                                RuleNames = AssessmentRuleNames
//                            };

//                            if (!string.IsNullOrWhiteSpace(mObjGovernmentData.ContactEmail))
//                            {
//                                BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
//                            }

//                            if (!string.IsNullOrWhiteSpace(mObjGovernmentData.ContactNumber))
//                            {
//                                BLSMSHandler.BL_BillGenerated(mObjEmailDetails);
//                            }
//                        }
//                    }
//                    else if (mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
//                    {
//                        usp_GetSpecialList_Result mObjSpecialData = new BLSpecial().BL_GetSpecialDetails(new Special() { intStatus = 2, SpecialID = mObjAssessmentData.TaxPayerID.GetValueOrDefault() });
//                        if (mObjSpecialData != null && mObjAssessmentData != null)
//                        {
//                            EmailDetails mObjEmailDetails = new EmailDetails()
//                            {
//                                TaxPayerTypeID = mObjSpecialData.TaxPayerTypeID.GetValueOrDefault(),
//                                TaxPayerTypeName = mObjSpecialData.TaxPayerTypeName,
//                                TaxPayerID = mObjSpecialData.SpecialID.GetValueOrDefault(),
//                                TaxPayerName = mObjSpecialData.SpecialTaxPayerName,
//                                TaxPayerRIN = mObjSpecialData.SpecialRIN,
//                                TaxPayerMobileNumber = mObjSpecialData.ContactNumber,
//                                TaxPayerEmail = mObjSpecialData.ContactEmail,
//                                BillRefNo = mObjAssessmentData.AssessmentRefNo,
//                                BillAmount = CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount),
//                                BillTypeName = "Assessment Bill",
//                                LoggedInUserID = -1,
//                                RuleNames = AssessmentRuleNames
//                            };

//                            if (!string.IsNullOrWhiteSpace(mObjSpecialData.ContactEmail))
//                            {
//                                BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
//                            }

//                            if (!string.IsNullOrWhiteSpace(mObjSpecialData.ContactNumber))
//                            {
//                                BLSMSHandler.BL_BillGenerated(mObjEmailDetails);
//                            }
//                        }
//                    }
//                }

//                scope.Complete();
//                mObjAPIResponse.Success = true;
//                mObjAPIResponse.Result = mObjAssessmentResponse.AdditionalData.AssessmentRefNo;
//                mObjAPIResponse.Message = "Assessment Added Successfully";

//            }
//            else
//            {
//                mObjAPIResponse.Success = false;
//                mObjAPIResponse.Message = mObjAssessmentResponse.Message;
//                Transaction.Current.Rollback();

//            }
//        }
//        catch (Exception ex)
//        {
//            ErrorSignal.FromCurrentContext().Raise(ex);
//            mObjAPIResponse.Success = false;
//            if (ex.Message == "ARALREADY")
//            {
//                mObjAPIResponse.Message = "Assessment rules added multiple times and not valid.";
//            }
//            else if (ex.Message == "ARNOTFOUND")
//            {
//                mObjAPIResponse.Message = "Assessment rules not found in assessment.";
//            }
//            else if (ex.Message == "AINOTFOUND")
//            {
//                mObjAPIResponse.Message = "Assessment items not found in assessment.";
//            }
//            else
//            {
//                mObjAPIResponse.Message = "Error occurred while saving assessment";
//            }

//            Transaction.Current.Rollback();
//        }
//    }

//    return Ok(mObjAPIResponse);
//}

//    [HttpPost]
//    [Route("InsertWithMultipleRule")]
//    public IHttpActionResult InsertWithMultipleRule(AssessmentWithMultipleRuleModel pObjAssessmentModel)
//    {
//        APIResponse mObjAPIResponse = new APIResponse();

//        if (!ModelState.IsValid)
//        {
//            mObjAPIResponse.Success = false;
//            mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
//        }
//        else
//        {
//            using (TransactionScope scope = new TransactionScope())
//            {
//                BLAssessment mObjBLAssessment = new BLAssessment();

//                Assessment mObjAssessment = new Assessment()
//                {
//                    AssessmentID = 0,
//                    TaxPayerID = pObjAssessmentModel.TaxPayerID,
//                    TaxPayerTypeID = pObjAssessmentModel.TaxPayerTypeID,
//                    AssessmentDate = CommUtil.GetCurrentDateTime(),
//                    SettlementDueDate = CommUtil.GetCurrentDateTime().AddMonths(1),
//                    SettlementStatusID = (int)EnumList.SettlementStatus.Assessed,
//                    AssessmentNotes = pObjAssessmentModel.Notes,
//                    Active = true,
//                    CreatedBy = 22,
//                    CreatedDate = CommUtil.GetCurrentDateTime(),
//                };

//                try
//                {

//                    FuncResponse<Assessment> mObjAssessmentResponse = mObjBLAssessment.BL_InsertUpdateAssessment(mObjAssessment);

//                    if (mObjAssessmentResponse.Success)
//                    {

//                        //Send Notification
//                        if (GlobalDefaultValues.SendNotification)
//                        {
//                            usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = mObjAssessmentResponse.AdditionalData.AssessmentID, IntStatus = 2 });
//                            IList<usp_GetAssessment_AssessmentRuleList_Result> lstMAPAssessmentRules = mObjBLAssessment.BL_GetAssessmentRules(mObjAssessmentResponse.AdditionalData.AssessmentID);

//                            string AssessmentRuleNames = string.Join(",", lstMAPAssessmentRules.Select(t => t.AssessmentRuleName).ToArray());

//                            if (mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
//                            {
//                                usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 2, IndividualID = mObjAssessmentData.TaxPayerID.GetValueOrDefault() });

//                                if (mObjIndividualData != null && mObjAssessmentData != null)
//                                {
//                                    EmailDetails mObjEmailDetails = new EmailDetails()
//                                    {
//                                        TaxPayerTypeID = mObjIndividualData.TaxPayerTypeID.GetValueOrDefault(),
//                                        TaxPayerTypeName = mObjIndividualData.TaxPayerTypeName,
//                                        TaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault(),
//                                        TaxPayerName = mObjIndividualData.FirstName + " " + mObjIndividualData.LastName,
//                                        TaxPayerRIN = mObjIndividualData.IndividualRIN,
//                                        TaxPayerMobileNumber = mObjIndividualData.MobileNumber1,
//                                        TaxPayerEmail = mObjIndividualData.EmailAddress1,
//                                        BillRefNo = mObjAssessmentData.AssessmentRefNo,
//                                        BillAmount = CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount),
//                                        BillTypeName = "Assessment Bill",
//                                        LoggedInUserID = -1,
//                                        RuleNames = AssessmentRuleNames
//                                    };

//                                    if (!string.IsNullOrWhiteSpace(mObjIndividualData.EmailAddress1))
//                                    {
//                                        BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
//                                    }

//                                    if (!string.IsNullOrWhiteSpace(mObjIndividualData.MobileNumber1))
//                                    {
//                                        BLSMSHandler.BL_BillGenerated(mObjEmailDetails);
//                                    }
//                                }
//                            }
//                            else if (mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
//                            {
//                                usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = mObjAssessmentData.TaxPayerID.GetValueOrDefault() });
//                                if (mObjCompanyData != null && mObjAssessmentData != null)
//                                {
//                                    EmailDetails mObjEmailDetails = new EmailDetails()
//                                    {
//                                        TaxPayerTypeID = mObjCompanyData.TaxPayerTypeID.GetValueOrDefault(),
//                                        TaxPayerTypeName = mObjCompanyData.TaxPayerTypeName,
//                                        TaxPayerID = mObjCompanyData.CompanyID.GetValueOrDefault(),
//                                        TaxPayerName = mObjCompanyData.CompanyName,
//                                        TaxPayerRIN = mObjCompanyData.CompanyRIN,
//                                        TaxPayerMobileNumber = mObjCompanyData.MobileNumber1,
//                                        TaxPayerEmail = mObjCompanyData.EmailAddress1,
//                                        BillRefNo = mObjAssessmentData.AssessmentRefNo,
//                                        BillAmount = CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount),
//                                        BillTypeName = "Assessment Bill",
//                                        LoggedInUserID = -1,
//                                        RuleNames = AssessmentRuleNames
//                                    };

//                                    if (!string.IsNullOrWhiteSpace(mObjCompanyData.EmailAddress1))
//                                    {
//                                        BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
//                                    }

//                                    if (!string.IsNullOrWhiteSpace(mObjCompanyData.MobileNumber1))
//                                    {
//                                        BLSMSHandler.BL_BillGenerated(mObjEmailDetails);
//                                    }
//                                }
//                            }
//                            else if (mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
//                            {
//                                usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(new Government() { intStatus = 2, GovernmentID = mObjAssessmentData.TaxPayerID.GetValueOrDefault() });
//                                if (mObjGovernmentData != null && mObjAssessmentData != null)
//                                {
//                                    EmailDetails mObjEmailDetails = new EmailDetails()
//                                    {
//                                        TaxPayerTypeID = mObjGovernmentData.TaxPayerTypeID.GetValueOrDefault(),
//                                        TaxPayerTypeName = mObjGovernmentData.TaxPayerTypeName,
//                                        TaxPayerID = mObjGovernmentData.GovernmentID.GetValueOrDefault(),
//                                        TaxPayerName = mObjGovernmentData.GovernmentName,
//                                        TaxPayerRIN = mObjGovernmentData.GovernmentRIN,
//                                        TaxPayerMobileNumber = mObjGovernmentData.ContactNumber,
//                                        TaxPayerEmail = mObjGovernmentData.ContactEmail,
//                                        BillRefNo = mObjAssessmentData.AssessmentRefNo,
//                                        BillAmount = CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount),
//                                        BillTypeName = "Assessment Bill",
//                                        LoggedInUserID = -1,
//                                        RuleNames = AssessmentRuleNames
//                                    };

//                                    if (!string.IsNullOrWhiteSpace(mObjGovernmentData.ContactEmail))
//                                    {
//                                        BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
//                                    }

//                                    if (!string.IsNullOrWhiteSpace(mObjGovernmentData.ContactNumber))
//                                    {
//                                        BLSMSHandler.BL_BillGenerated(mObjEmailDetails);
//                                    }
//                                }
//                            }
//                            else if (mObjAssessmentData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
//                            {
//                                usp_GetSpecialList_Result mObjSpecialData = new BLSpecial().BL_GetSpecialDetails(new Special() { intStatus = 2, SpecialID = mObjAssessmentData.TaxPayerID.GetValueOrDefault() });
//                                if (mObjSpecialData != null && mObjAssessmentData != null)
//                                {
//                                    EmailDetails mObjEmailDetails = new EmailDetails()
//                                    {
//                                        TaxPayerTypeID = mObjSpecialData.TaxPayerTypeID.GetValueOrDefault(),
//                                        TaxPayerTypeName = mObjSpecialData.TaxPayerTypeName,
//                                        TaxPayerID = mObjSpecialData.SpecialID.GetValueOrDefault(),
//                                        TaxPayerName = mObjSpecialData.SpecialTaxPayerName,
//                                        TaxPayerRIN = mObjSpecialData.SpecialRIN,
//                                        TaxPayerMobileNumber = mObjSpecialData.ContactNumber,
//                                        TaxPayerEmail = mObjSpecialData.ContactEmail,
//                                        BillRefNo = mObjAssessmentData.AssessmentRefNo,
//                                        BillAmount = CommUtil.GetFormatedCurrency(mObjAssessmentData.AssessmentAmount),
//                                        BillTypeName = "Assessment Bill",
//                                        LoggedInUserID = -1,
//                                        RuleNames = AssessmentRuleNames
//                                    };

//                                    if (!string.IsNullOrWhiteSpace(mObjSpecialData.ContactEmail))
//                                    {
//                                        BLEmailHandler.BL_BillGenerated(mObjEmailDetails);
//                                    }

//                                    if (!string.IsNullOrWhiteSpace(mObjSpecialData.ContactNumber))
//                                    {
//                                        BLSMSHandler.BL_BillGenerated(mObjEmailDetails);
//                                    }
//                                }
//                            }
//                        }

//                        scope.Complete();
//                        mObjAPIResponse.Success = true;
//                        mObjAPIResponse.Result = mObjAssessmentResponse.AdditionalData.AssessmentRefNo;
//                        mObjAPIResponse.Message = "Assessment Added Successfully";

//                    }
//                    else
//                    {
//                        mObjAPIResponse.Success = false;
//                        mObjAPIResponse.Message = mObjAssessmentResponse.Message;
//                        Transaction.Current.Rollback();

//                    }
//                }
//                catch (Exception ex)
//                {
//                    ErrorSignal.FromCurrentContext().Raise(ex);
//                    mObjAPIResponse.Success = false;
//                    if (ex.Message == "ARALREADY")
//                    {
//                        mObjAPIResponse.Message = "Assessment rules added multiple times and not valid.";
//                    }
//                    else if (ex.Message == "ARNOTFOUND")
//                    {
//                        mObjAPIResponse.Message = "Assessment rules not found in assessment.";
//                    }
//                    else if (ex.Message == "AINOTFOUND")
//                    {
//                        mObjAPIResponse.Message = "Assessment items not found in assessment.";
//                    }
//                    else
//                    {
//                        mObjAPIResponse.Message = "Error occurred while saving assessment";
//                    }

//                    Transaction.Current.Rollback();
//                }
//            }

//        }

//        return Ok(mObjAPIResponse);
//    }


