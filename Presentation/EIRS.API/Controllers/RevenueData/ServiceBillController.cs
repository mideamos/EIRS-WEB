using EIRS.API.Models;
using EIRS.API.Utility;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using Elmah;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Http;
using System.Web.UI.WebControls;

namespace EIRS.API.Controllers
{
    /// <summary>
    /// ServiceBill Operations
    /// </summary>
    [RoutePrefix("RevenueData/ServiceBill")]

    public class ServiceBillController : BaseController
    {
        /// <summary>
        /// Returns a list of service bill
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                ServiceBill mObjServiceBill = new ServiceBill() { IntStatus = 1 };

                IList<usp_GetServiceBillList_Result> lstServiceBill = new BLServiceBill().BL_GetServiceBillList(mObjServiceBill);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstServiceBill;
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
                    BLServiceBill mObjBLServiceBill = new BLServiceBill();

                    usp_GetServiceBillList_Result mObjServiceBillDetails = null;

                    if (searchtype == "id")
                    {
                        mObjServiceBillDetails = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = TrynParse.parseInt(searchby), IntStatus = 2 });
                    }
                    else if (searchtype == "refno")
                    {
                        mObjServiceBillDetails = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillRefNo = searchby, IntStatus = 2 });
                    }

                    if (mObjServiceBillDetails != null)
                    {
                        IList<usp_GetServiceBill_MDAServiceList_Result> lstMAPServiceBillServices = mObjBLServiceBill.BL_GetMDAServiceList(mObjServiceBillDetails.ServiceBillID.GetValueOrDefault());
                        IList<usp_GetServiceBillItemList_Result> lstServiceBillItems = mObjBLServiceBill.BL_GetServiceBillItem(mObjServiceBillDetails.ServiceBillID.GetValueOrDefault());
                        IList<usp_GetMDAServiceBasedSettlement_Result> lstMDAServiceSettlement = mObjBLServiceBill.BL_GetMDAServiceBasedSettlement(mObjServiceBillDetails.ServiceBillID.GetValueOrDefault());

                        IList<DropDownListResult> lstSettlementMethod = mObjBLServiceBill.BL_GetSettlementMethodMDAServiceBased(mObjServiceBillDetails.ServiceBillID.GetValueOrDefault());

                        IList<usp_GetSettlementList_Result> lstSettlement = new BLSettlement().BL_GetSettlementList(new Settlement() { AssessmentID = -1, ServiceBillID = mObjServiceBillDetails.ServiceBillID.GetValueOrDefault() });

                        IDictionary<string, object> dcResponse = new Dictionary<string, object>
                        {
                            { "ServiceBillDetails", mObjServiceBillDetails },
                            { "ServiceBillServicesDetails", lstMAPServiceBillServices },
                            { "ServiceBillItemDetails", lstServiceBillItems },
                            {"SettlementMethodList",lstSettlementMethod },
                            { "SettlementDetails",lstMDAServiceSettlement},
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
        [Route("DetailByRefNo/{refno}")]
        public IHttpActionResult DetailByRefNo(string refno)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                usp_GetServiceBillList_Result mObjServiceBillDetails = new BLServiceBill().BL_GetServiceBillDetails(new ServiceBill() { ServiceBillRefNo = refno, IntStatus = 1 });

                if (mObjServiceBillDetails != null)
                {
                    mObjAPIResponse.Success = true;
                    mObjAPIResponse.Result = mObjServiceBillDetails;
                }
                else
                {
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Result = "No Service Bill found with given reference number and mobile number";
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
        [Route("ServiceDetail/{id}")]
        public IHttpActionResult ServiceDetail(int? id)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                if (id.GetValueOrDefault() > 0)
                {

                    IList<usp_GetServiceBill_MDAServiceList_Result> lstMAPServiceBillServices = new BLServiceBill().BL_GetMDAServiceList(id.GetValueOrDefault());

                    mObjAPIResponse.Success = true;
                    mObjAPIResponse.Result = lstMAPServiceBillServices;

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

                    IList<usp_GetServiceBillItemList_Result> lstServiceBillItems = new BLServiceBill().BL_GetServiceBillItem(id.GetValueOrDefault());

                    mObjAPIResponse.Success = true;
                    mObjAPIResponse.Result = lstServiceBillItems;

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
        [Route("ServiceSettlementDetail/{id}")]
        public IHttpActionResult ServiceSettlementDetail(int? id)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                if (id.GetValueOrDefault() > 0)
                {

                    IList<usp_GetMDAServiceBasedSettlement_Result> lstMDAServiceSettlement = new BLServiceBill().BL_GetMDAServiceBasedSettlement(id.GetValueOrDefault());

                    mObjAPIResponse.Success = true;
                    mObjAPIResponse.Result = lstMDAServiceSettlement;

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

                    IList<usp_GetSettlementList_Result> lstSettlement = new BLSettlement().BL_GetSettlementList(new Settlement() { AssessmentID = -1, ServiceBillID = id.GetValueOrDefault() });

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

        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert(ServiceBillModel pObjServiceBillModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();
            String token = Request.Headers.Authorization.Parameter;
            int? userId = Utilities.GetUserId(token);

            //Microsoft.Owin.Security.AuthenticationTicket t = Startup.OAuthOptions.AccessTokenFormat.Unprotect(token);
            if (!ModelState.IsValid)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }
            else
            {
                using (TransactionScope scope = new TransactionScope())
                {
                    BLMDAServiceItem mObjBLMDAServiceItem = new BLMDAServiceItem();
                    usp_GetMDAServiceItemList_Result mObjMDAServiceItemData = null; MAP_ServiceBill_MDAServiceItem mObjSIDetail = null;
                    IList<MAP_ServiceBill_MDAServiceItem> lstMDAServiceItem = new List<MAP_ServiceBill_MDAServiceItem>();

                    IList<MAP_ServiceBill_MDAService> lstMDAService = new List<MAP_ServiceBill_MDAService>();
                    MAP_ServiceBill_MDAService mObjMDAServiceDetails = new MAP_ServiceBill_MDAService()
                    {
                        SBSID = 0,
                        MDAServiceID = pObjServiceBillModel.MDAServiceID,
                        ServiceBillYear = pObjServiceBillModel.TaxYear,
                        ServiceAmount = pObjServiceBillModel.LstServiceBillItem.Count > 0 ? pObjServiceBillModel.LstServiceBillItem.Sum(t => t.ServiceBaseAmount) : 0,

                        //ServiceAmount = lstMDAServiceItem.Sum(t => t.ServiceAmount),
                        CreatedBy = userId.HasValue ? userId : 22,
                        CreatedDate = CommUtil.GetCurrentDateTime(),
                    };

                    lstMDAService.Add(mObjMDAServiceDetails);
                    BLServiceBill mObjBLServiceBill = new BLServiceBill();

                    ServiceBill mObjServiceBill = new ServiceBill()
                    {
                        ServiceBillID = 0,
                        TaxPayerID = pObjServiceBillModel.TaxPayerID,
                        TaxPayerTypeID = pObjServiceBillModel.TaxPayerTypeID,
                        ServiceBillAmount = pObjServiceBillModel.LstServiceBillItem.Count > 0 ? pObjServiceBillModel.LstServiceBillItem.Sum(t => t.ServiceBaseAmount) : 0,

                        ServiceBillDate = CommUtil.GetCurrentDateTime(),
                        SettlementDueDate = CommUtil.GetCurrentDateTime().AddMonths(1),
                        SettlementStatusID = (int)EnumList.SettlementStatus.Assessed,
                        Notes = pObjServiceBillModel.Notes,
                        Active = true,
                        CreatedBy = userId.HasValue ? userId : 22,
                        CreatedDate = CommUtil.GetCurrentDateTime(),
                    };

                    try
                    {
                        FuncResponse<ServiceBill> mObjServiceBillResponse = mObjBLServiceBill.BL_InsertUpdateServiceBill(mObjServiceBill);

                        if (mObjServiceBillResponse.Success)
                        {
                            var serviceBillData = mObjServiceBillResponse.AdditionalData;
                            mObjMDAServiceDetails.ServiceBillID = serviceBillData.ServiceBillID;
                            mObjMDAServiceDetails.ServiceAmount = serviceBillData.ServiceBillAmount;

                            FuncResponse<MAP_ServiceBill_MDAService> mObjSBSResponse = mObjBLServiceBill.BL_InsertUpdateMDAService(mObjMDAServiceDetails);

                            if (mObjSBSResponse.Success)
                            {
                                var mdaServiceBillData = mObjSBSResponse.AdditionalData;

                                foreach (var item in pObjServiceBillModel.LstServiceBillItem)
                                {
                                    mObjMDAServiceItemData = mObjBLMDAServiceItem.BL_GetMDAServiceItemDetails(new MDA_Service_Items() { MDAServiceItemID = item.MDAServiceItemID, intStatus = 2 });

                                    if (mObjMDAServiceItemData != null)
                                    {
                                        mObjSIDetail = new MAP_ServiceBill_MDAServiceItem()
                                        {
                                            SBSIID = 0,
                                            SBSID = mdaServiceBillData.SBSID,
                                            MDAServiceItemID = mObjMDAServiceItemData.MDAServiceItemID,
                                            ServiceAmount = item.ServiceBaseAmount,
                                            ServiceBaseAmount = item.ServiceBaseAmount,

                                            Percentage = mObjMDAServiceItemData.Percentage,
                                            PaymentStatusID = (int)EnumList.PaymentStatus.Due,
                                            CreatedBy = userId.HasValue ? userId : 22,
                                            CreatedDate = CommUtil.GetCurrentDateTime(),
                                        };

                                        lstMDAServiceItem.Add(mObjSIDetail);
                                    }
                                    else
                                    {
                                        mObjAPIResponse.Success = false;
                                        mObjAPIResponse.Message = "Invalid Service Bill Item Details";
                                        break;
                                    }
                                }

                                var addServiceBillItemsResponse = mObjBLServiceBill.BL_InsertServiceBillItem(lstMDAServiceItem); //BL_InsertServiceBillItem

                                if (!addServiceBillItemsResponse.Success)
                                {
                                    mObjAPIResponse.Success = false;
                                    mObjAPIResponse.Message = addServiceBillItemsResponse.Message;
                                    Transaction.Current.Rollback();
                                }

                            }
                            else
                            {
                                mObjAPIResponse.Success = false;
                                mObjAPIResponse.Message = mObjSBSResponse.Message;
                                Transaction.Current.Rollback();
                            }

                            //Send Notification
                            if (GlobalDefaultValues.SendNotification)
                            {
                                var mObjServiceBillData = mObjServiceBillResponse.AdditionalData;
                                IList<usp_GetServiceBill_MDAServiceList_Result> lstMAPServiceBillServices = mObjBLServiceBill.BL_GetMDAServiceList(mObjServiceBillData.ServiceBillID);
                                string RuleNames = string.Join(",", lstMAPServiceBillServices.Select(t => t.MDAServiceName).ToArray());

                                //usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = mObjServiceBillResponse.AdditionalData.ServiceBillID, IntStatus = 2 });
                                //IList<usp_GetServiceBill_MDAServiceList_Result> lstMAPServiceBillServices = mObjBLServiceBill.BL_GetMDAServiceList(mObjServiceBillData.ServiceBillID.GetValueOrDefault());
                                //string RuleNames = string.Join(",", lstMAPServiceBillServices.Select(t => t.MDAServiceName).ToArray());

                                if (mObjServiceBillData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
                                {
                                    usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = (int)EnumList.SettlementStatus.Notified, IndividualID = mObjServiceBillData.TaxPayerID.GetValueOrDefault() });

                                    if (mObjIndividualData != null && mObjServiceBillData != null)
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
                                            BillRefNo = mObjServiceBillData.ServiceBillRefNo,
                                            BillAmount = CommUtil.GetFormatedCurrency(mObjServiceBillData.ServiceBillAmount),
                                            BillTypeName = "Service Bill",
                                            LoggedInUserID = -1,
                                            RuleNames = RuleNames
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
                                else if (mObjServiceBillData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
                                {
                                    usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = (int)EnumList.SettlementStatus.Notified, CompanyID = mObjServiceBillData.TaxPayerID.GetValueOrDefault() });
                                    if (mObjCompanyData != null && mObjServiceBillData != null)
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
                                            BillRefNo = mObjServiceBillData.ServiceBillRefNo,
                                            BillAmount = CommUtil.GetFormatedCurrency(mObjServiceBillData.ServiceBillAmount),
                                            BillTypeName = "Service Bill",
                                            LoggedInUserID = -1,
                                            RuleNames = RuleNames
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
                                else if (mObjServiceBillData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
                                {
                                    usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(new Government() { intStatus = (int)EnumList.SettlementStatus.Notified, GovernmentID = mObjServiceBillData.TaxPayerID.GetValueOrDefault() });
                                    if (mObjGovernmentData != null && mObjServiceBillData != null)
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
                                            BillRefNo = mObjServiceBillData.ServiceBillRefNo,
                                            BillAmount = CommUtil.GetFormatedCurrency(mObjServiceBillData.ServiceBillAmount),
                                            BillTypeName = "Service Bill",
                                            LoggedInUserID = -1,
                                            RuleNames = RuleNames
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
                                else if (mObjServiceBillData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
                                {
                                    usp_GetSpecialList_Result mObjSpecialData = new BLSpecial().BL_GetSpecialDetails(new Special() { intStatus = (int)EnumList.SettlementStatus.Notified, SpecialID = mObjServiceBillData.TaxPayerID.GetValueOrDefault() });
                                    if (mObjSpecialData != null && mObjServiceBillData != null)
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
                                            BillRefNo = mObjServiceBillData.ServiceBillRefNo,
                                            BillAmount = CommUtil.GetFormatedCurrency(mObjServiceBillData.ServiceBillAmount),
                                            BillTypeName = "Service Bill",
                                            LoggedInUserID = -1,
                                            RuleNames = RuleNames
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
                            mObjAPIResponse.Result = mObjServiceBillResponse.AdditionalData.ServiceBillRefNo;
                            mObjAPIResponse.Message = "Service Bill Added Successfully";
                        }
                        else
                        {
                            mObjAPIResponse.Success = false;
                            mObjAPIResponse.Message = mObjServiceBillResponse.Message;
                            Transaction.Current.Rollback();
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        mObjAPIResponse.Success = false;
                        mObjAPIResponse.Message = "Error occurred while saving service bill";
                        Transaction.Current.Rollback();
                    }
                }

            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("InsertWithMultipleMDAService")]
        public IHttpActionResult InsertWithMultipleMDAService([FromBody] ServiceBillWithMultipleServiceModel pObjServiceBillModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();
            String token = Request.Headers.Authorization.Parameter;
           // int? userId = 0;
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
                    usp_GetMDAServiceItemList_Result mObjMDAServiceItemData = null;

                    BLMDAServiceItem mObjBLMDAServiceItem = new BLMDAServiceItem();
                    BLServiceBill mObjBLServiceBill = new BLServiceBill();

                    ServiceBill mObjServiceBill = new ServiceBill()
                    {
                        ServiceBillID = 0,
                        TaxPayerID = pObjServiceBillModel.TaxPayerID,
                        TaxPayerTypeID = pObjServiceBillModel.TaxPayerTypeID,
                        ServiceBillDate = CommUtil.GetCurrentDateTime(),
                        ServiceBillAmount = pObjServiceBillModel.LstMDAService.Sum(item => item.LstServiceBillItem.Sum(o => o.ServiceBaseAmount)),
                        SettlementDueDate = CommUtil.GetCurrentDateTime().AddMonths(1),
                        SettlementStatusID = (int)EnumList.SettlementStatus.Assessed,
                        Notes = pObjServiceBillModel.Notes,
                        Active = true,
                        CreatedBy = userId.HasValue ? userId : 22,
                        CreatedDate = CommUtil.GetCurrentDateTime(),
                    };

                    try
                    {
                        FuncResponse<ServiceBill> mObjServiceBillResponse = mObjBLServiceBill.BL_InsertUpdateServiceBill(mObjServiceBill);

                        if (mObjServiceBillResponse.Success)
                        {
                            foreach (var item in pObjServiceBillModel.LstMDAService)
                            {
                                MAP_ServiceBill_MDAService mObjMDAServiceDetails = new MAP_ServiceBill_MDAService()
                                {
                                    SBSID = 0,
                                    ServiceBillID = mObjServiceBillResponse.AdditionalData.ServiceBillID,
                                    MDAServiceID = item.MDAServiceID,
                                    ServiceBillYear = item.TaxYear,
                                    ServiceAmount = item.LstServiceBillItem.Sum(t => t.ServiceBaseAmount),
                                    CreatedBy = userId.HasValue ? userId : 22,
                                    CreatedDate = CommUtil.GetCurrentDateTime(),
                                };
                                FuncResponse<MAP_ServiceBill_MDAService> mObjSBSResponse = mObjBLServiceBill.BL_InsertUpdateMDAService(mObjMDAServiceDetails);
                                if (mObjSBSResponse.Success)
                                {
                                    List<MAP_ServiceBill_MDAServiceItem> lstMDAServiceItem = new List<MAP_ServiceBill_MDAServiceItem>();
                                    foreach (var item2 in item.LstServiceBillItem)
                                    {
                                        mObjMDAServiceItemData = mObjBLMDAServiceItem.BL_GetMDAServiceItemDetails(new MDA_Service_Items() { MDAServiceItemID = item2.MDAServiceItemID, intStatus = 2 });

                                        MAP_ServiceBill_MDAServiceItem mObjSIDetail = new MAP_ServiceBill_MDAServiceItem()
                                        {
                                            SBSIID = 0,
                                            SBSID = mObjSBSResponse.AdditionalData.SBSID,
                                            MDAServiceItemID = item2.MDAServiceItemID,
                                            ServiceAmount = item2.ServiceBaseAmount,
                                            ServiceBaseAmount = item2.ServiceBaseAmount,

                                            Percentage = mObjMDAServiceItemData.Percentage,
                                            PaymentStatusID = (int)EnumList.PaymentStatus.Due,
                                            CreatedBy = userId.HasValue ? userId : 22,
                                            CreatedDate = CommUtil.GetCurrentDateTime(),
                                        };
                                        lstMDAServiceItem.Add(mObjSIDetail);
                                    }
                                    var addServiceBillItemsResponse = mObjBLServiceBill.BL_InsertServiceBillItem(lstMDAServiceItem); //BL_InsertServiceBillItem

                                    if (!addServiceBillItemsResponse.Success)
                                    {
                                        mObjAPIResponse.Success = false;
                                        mObjAPIResponse.Message = addServiceBillItemsResponse.Message;
                                        Transaction.Current.Rollback();
                                    }

                                }

                                //Send Notification
                                if (GlobalDefaultValues.SendNotification)
                                {
                                    usp_GetServiceBillList_Result mObjServiceBillData = mObjBLServiceBill.BL_GetServiceBillDetails(new ServiceBill() { ServiceBillID = mObjServiceBillResponse.AdditionalData.ServiceBillID, IntStatus = 2 });
                                    IList<usp_GetServiceBill_MDAServiceList_Result> lstMAPServiceBillServices = mObjBLServiceBill.BL_GetMDAServiceList(mObjServiceBillData.ServiceBillID.GetValueOrDefault());

                                    string RuleNames = string.Join(",", lstMAPServiceBillServices.Select(t => t.MDAServiceName).ToArray());

                                    if (mObjServiceBillData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
                                    {
                                        usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 2, IndividualID = mObjServiceBillData.TaxPayerID.GetValueOrDefault() });

                                        if (mObjIndividualData != null && mObjServiceBillData != null)
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
                                                BillRefNo = mObjServiceBillData.ServiceBillRefNo,
                                                BillAmount = CommUtil.GetFormatedCurrency(mObjServiceBillData.ServiceBillAmount),
                                                BillTypeName = "Service Bill",
                                                LoggedInUserID = -1,
                                                RuleNames = RuleNames
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
                                    else if (mObjServiceBillData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
                                    {
                                        usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 2, CompanyID = mObjServiceBillData.TaxPayerID.GetValueOrDefault() });
                                        if (mObjCompanyData != null && mObjServiceBillData != null)
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
                                                BillRefNo = mObjServiceBillData.ServiceBillRefNo,
                                                BillAmount = CommUtil.GetFormatedCurrency(mObjServiceBillData.ServiceBillAmount),
                                                BillTypeName = "Service Bill",
                                                LoggedInUserID = -1,
                                                RuleNames = RuleNames
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
                                    else if (mObjServiceBillData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
                                    {
                                        usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(new Government() { intStatus = 2, GovernmentID = mObjServiceBillData.TaxPayerID.GetValueOrDefault() });
                                        if (mObjGovernmentData != null && mObjServiceBillData != null)
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
                                                BillRefNo = mObjServiceBillData.ServiceBillRefNo,
                                                BillAmount = CommUtil.GetFormatedCurrency(mObjServiceBillData.ServiceBillAmount),
                                                BillTypeName = "Service Bill",
                                                LoggedInUserID = -1,
                                                RuleNames = RuleNames
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
                                    else if (mObjServiceBillData.TaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
                                    {
                                        usp_GetSpecialList_Result mObjSpecialData = new BLSpecial().BL_GetSpecialDetails(new Special() { intStatus = 2, SpecialID = mObjServiceBillData.TaxPayerID.GetValueOrDefault() });
                                        if (mObjSpecialData != null && mObjServiceBillData != null)
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
                                                BillRefNo = mObjServiceBillData.ServiceBillRefNo,
                                                BillAmount = CommUtil.GetFormatedCurrency(mObjServiceBillData.ServiceBillAmount),
                                                BillTypeName = "Service Bill",
                                                LoggedInUserID = -1,
                                                RuleNames = RuleNames
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
                            }
                            scope.Complete();
                            mObjAPIResponse.Success = true;
                            mObjAPIResponse.Result = mObjServiceBillResponse.AdditionalData.ServiceBillRefNo;
                            mObjAPIResponse.Message = "Service Bill Added Successfully";


                        }
                        else
                        {
                            mObjAPIResponse.Success = false;
                            mObjAPIResponse.Message = mObjServiceBillResponse.Message;
                            Transaction.Current.Rollback();

                        }
                    }
                    catch (Exception ex)
                    {
                        NewErrorLog.ErrorLogging(ex);
                        ErrorSignal.FromCurrentContext().Raise(ex);
                        mObjAPIResponse.Success = false;
                        mObjAPIResponse.Message = "Error occurred while saving service bill";

                        Transaction.Current.Rollback();
                    }
                }


            }

            return Ok(mObjAPIResponse);
        }


    }
}