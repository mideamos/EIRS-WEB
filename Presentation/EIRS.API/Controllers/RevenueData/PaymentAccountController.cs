using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web.Http;
//using System.Xml.Linq;
using EIRS.API.Models;
using EIRS.API.Utility;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using Elmah;

namespace EIRS.API.Controllers
{

    [RoutePrefix("RevenueData/POA")]
    
    public class PaymentAccountController : BaseController
    {
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert(PaymentAccountModel pObjPaymentAccount)
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
                Payment_Account mObjPaymentAccount = new Payment_Account()
                {
                    PaymentAccountID = 0,
                    TaxPayerTypeID = pObjPaymentAccount.TaxPayerTypeID,
                    TaxPayerID = pObjPaymentAccount.TaxPayerID,
                    SettlementMethodID = pObjPaymentAccount.PaymentMethodID,
                    SettlementStatusID = (int)EnumList.SettlementStatus.Settled,
                    PaymentDate = pObjPaymentAccount.PaymentDate,
                    TransactionRefNo = pObjPaymentAccount.TransactionRefNo,
                    Notes = pObjPaymentAccount.Notes,
                    RevenueStreamID = pObjPaymentAccount.RevenueStreamID,
                    RevenueSubStreamID = pObjPaymentAccount.RevenueSubStreamID,
                    AgencyID = pObjPaymentAccount.AgencyID,

                    Amount = pObjPaymentAccount.Amount,

                    Active = true,
                    CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {
                    FuncResponse<Payment_Account> mObjFuncResponse = new BLPaymentAccount().BL_InsertUpdatePaymentAccount(mObjPaymentAccount);

                    if (mObjFuncResponse.Success)
                    {
                        mObjAPIResponse.Success = true;
                        mObjAPIResponse.Message = mObjFuncResponse.Message;
                    }
                    else
                    {
                        mObjAPIResponse.Success = false;
                        mObjAPIResponse.Message = mObjFuncResponse.Message;
                    }
                }
                catch (Exception ex)
                {
                    mObjAPIResponse.Success = true;
                    mObjAPIResponse.Message = "Error occurred while saving payment on account";
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("InsertRINBased")]
        public IHttpActionResult InsertRINBased(PaymentAccountRINModel pObjPaymentAccount)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            String token = Request.Headers.Authorization.Parameter;
            int? userId = Utilities.GetUserId(token);

            if (!ModelState.IsValid)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Result = Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

                //mObjAPIResponse.Success = false;
                //mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }
            else
            {
                //Get Tax Payer ID
                int mIntTaxPayerID = 0;

                if (pObjPaymentAccount.TaxPayerTypeID == (int)EnumList.TaxPayerType.Individual)
                {
                    usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(new Individual() { intStatus = 1, IndividualRIN = pObjPaymentAccount.TaxPayerRIN });

                    if (mObjIndividualData != null)
                    {
                        mIntTaxPayerID = mObjIndividualData.IndividualID.GetValueOrDefault();
                    }
                }
                else if (pObjPaymentAccount.TaxPayerTypeID == (int)EnumList.TaxPayerType.Companies)
                {
                    usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(new Company() { intStatus = 1, CompanyRIN = pObjPaymentAccount.TaxPayerRIN });

                    if (mObjCompanyData != null)
                    {
                        mIntTaxPayerID = mObjCompanyData.CompanyID.GetValueOrDefault();
                    }
                }
                else if (pObjPaymentAccount.TaxPayerTypeID == (int)EnumList.TaxPayerType.Government)
                {
                    usp_GetGovernmentList_Result mObjGovernmentData = new BLGovernment().BL_GetGovernmentDetails(new Government() { intStatus = 1, GovernmentRIN = pObjPaymentAccount.TaxPayerRIN });

                    if (mObjGovernmentData != null)
                    {
                        mIntTaxPayerID = mObjGovernmentData.GovernmentID.GetValueOrDefault();
                    }
                }
                else if (pObjPaymentAccount.TaxPayerTypeID == (int)EnumList.TaxPayerType.Special)
                {
                    usp_GetSpecialList_Result mObjSpecialData = new BLSpecial().BL_GetSpecialDetails(new Special() { intStatus = 1, SpecialRIN = pObjPaymentAccount.TaxPayerRIN });

                    if (mObjSpecialData != null)
                    {
                        mIntTaxPayerID = mObjSpecialData.SpecialID.GetValueOrDefault();
                    }
                }


                if (mIntTaxPayerID > 0)
                {

                    Payment_Account mObjPaymentAccount = new Payment_Account()
                    {
                        PaymentAccountID = 0,
                        TaxPayerTypeID = pObjPaymentAccount.TaxPayerTypeID,
                        TaxPayerID = mIntTaxPayerID,
                        SettlementMethodID = pObjPaymentAccount.PaymentMethodID,
                        SettlementStatusID = (int)EnumList.SettlementStatus.Settled,
                        PaymentDate = pObjPaymentAccount.PaymentDate,
                        TransactionRefNo = pObjPaymentAccount.TransactionRefNo,
                        Notes = pObjPaymentAccount.Notes,
                        RevenueStreamID = pObjPaymentAccount.RevenueStreamID,
                        RevenueSubStreamID = pObjPaymentAccount.RevenueSubStreamID,
                        AgencyID = pObjPaymentAccount.AgencyID,

                        Amount = pObjPaymentAccount.Amount,

                        Active = true,
                        CreatedBy = userId.HasValue ? userId : 22,
                        CreatedDate = CommUtil.GetCurrentDateTime()
                    };

                    try
                    {

                        FuncResponse<Payment_Account> mObjFuncResponse = new BLPaymentAccount().BL_InsertUpdatePaymentAccount(mObjPaymentAccount);

                        if (mObjFuncResponse.Success)
                        {
                            mObjAPIResponse.Success = true;
                            mObjAPIResponse.Message = mObjFuncResponse.Message;
                        }
                        else
                        {
                            mObjAPIResponse.Success = false;
                            mObjAPIResponse.Message = mObjFuncResponse.Message;
                        }
                    }
                    catch (Exception ex)
                    {
                        mObjAPIResponse.Success = true;
                        mObjAPIResponse.Message = "Error occurred while saving payment on account";
                    }
                }
                else
                {
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Message = "Invalid RIN";
                }
            }

            return Ok(mObjAPIResponse);
        }


        [HttpPost]
        [Route("AddIndividual")]
        public IHttpActionResult AddIndividual(IndividualModel pObjIndividualModel)
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
                    try
                    {
                        Individual mObjIndividual = new Individual()
                        {
                            IndividualID = 0,
                            GenderID = 3,
                            TitleID = pObjIndividualModel.TitleID,
                            FirstName = pObjIndividualModel.FirstName.Trim(),
                            LastName = pObjIndividualModel.LastName.Trim(),
                            DOB = new DateTime(1900, 01, 01),
                            TIN = "None Listed",
                            MobileNumber1 = pObjIndividualModel.MobileNumber1,
                            EmailAddress1 = pObjIndividualModel.EmailAddress1,
                            TaxOfficeID = pObjIndividualModel.TaxOfficeID,
                            MaritalStatusID = 3,
                            NationalityID = pObjIndividualModel.NationalityID,
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                            EconomicActivitiesID = pObjIndividualModel.EconomicActivitiesID,
                            NotificationMethodID = pObjIndividualModel.NotificationMethodID,
                            ContactAddress = pObjIndividualModel.ContactAddress,
                            Active = true,
                            CreatedBy = userId.HasValue ? userId : 22,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse<Individual> mObjFuncResponse = new BLIndividual().BL_InsertUpdateIndividual(mObjIndividual);

                        if (mObjFuncResponse.Success)
                        {

                            Payment_Account mObjPaymentAccount = new Payment_Account()
                            {
                                PaymentAccountID = 0,
                                TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                                TaxPayerID = mObjFuncResponse.AdditionalData.IndividualID,
                                SettlementMethodID = pObjIndividualModel.PaymentMethodID,
                                SettlementStatusID = (int)EnumList.SettlementStatus.Settled,
                                PaymentDate = pObjIndividualModel.PaymentDate,
                                TransactionRefNo = pObjIndividualModel.TransactionRefNo,
                                Notes = pObjIndividualModel.Notes,
                                RevenueStreamID = pObjIndividualModel.RevenueStreamID,
                                RevenueSubStreamID = pObjIndividualModel.RevenueSubStreamID,
                                AgencyID = pObjIndividualModel.AgencyID,
                                Active = true,
                                CreatedBy = userId.HasValue ? userId : 22,
                                CreatedDate = CommUtil.GetCurrentDateTime()
                            };

                            FuncResponse<Payment_Account> mObjPoAFuncResponse = new BLPaymentAccount().BL_InsertUpdatePaymentAccount(mObjPaymentAccount);

                            if (mObjFuncResponse.Success)
                            {
                                scope.Complete();
                                mObjAPIResponse.Success = true;
                                mObjAPIResponse.Message = "Individual added and amount added in account succesfully";
                            }
                            else
                            {
                                throw (new Exception(mObjPoAFuncResponse.Message));
                            }

                        }
                        else
                        {
                            throw (new Exception(mObjFuncResponse.Message));
                        }


                    }
                    catch (Exception Ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(Ex);
                        mObjAPIResponse.Success = false;
                        mObjAPIResponse.Message = "Error occurred while adding individual";
                        Transaction.Current.Rollback();
                    }
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("AddCompany")]
        public IHttpActionResult AddCompany(CompanyModel pObjCompanyModel)
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
                    try
                    {
                        Company mObjCompany = new Company()
                        {
                            CompanyID = 0,
                            CompanyName = pObjCompanyModel.CompanyName.Trim(),
                            TIN = "None Listed",
                            MobileNumber1 = pObjCompanyModel.MobileNumber1,
                            EmailAddress1 = pObjCompanyModel.EmailAddress1,
                            TaxOfficeID = pObjCompanyModel.TaxOfficeID,
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                            EconomicActivitiesID = pObjCompanyModel.EconomicActivitiesID,
                            NotificationMethodID = pObjCompanyModel.NotificationMethodID,
                            ContactAddress = pObjCompanyModel.ContactAddress,
                            Active = true,
                            CreatedBy = userId.HasValue ? userId : 22,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse<Company> mObjFuncResponse = new BLCompany().BL_InsertUpdateCompany(mObjCompany);

                        if (mObjFuncResponse.Success)
                        {

                            Payment_Account mObjPaymentAccount = new Payment_Account()
                            {
                                PaymentAccountID = 0,
                                TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                                TaxPayerID = mObjFuncResponse.AdditionalData.CompanyID,
                                SettlementMethodID = pObjCompanyModel.PaymentMethodID,
                                SettlementStatusID = (int)EnumList.SettlementStatus.Settled,
                                PaymentDate = pObjCompanyModel.PaymentDate,
                                TransactionRefNo = pObjCompanyModel.TransactionRefNo,
                                Notes = pObjCompanyModel.Notes,
                                RevenueStreamID = pObjCompanyModel.RevenueStreamID,
                                RevenueSubStreamID = pObjCompanyModel.RevenueSubStreamID,
                                AgencyID = pObjCompanyModel.AgencyID,
                                Active = true,
                                CreatedBy = userId.HasValue ? userId : 22,
                                CreatedDate = CommUtil.GetCurrentDateTime()
                            };

                            FuncResponse<Payment_Account> mObjPoAFuncResponse = new BLPaymentAccount().BL_InsertUpdatePaymentAccount(mObjPaymentAccount);

                            if (mObjFuncResponse.Success)
                            {
                                scope.Complete();
                                mObjAPIResponse.Success = true;
                                mObjAPIResponse.Message = "Company added and amount added in account succesfully";
                            }
                            else
                            {
                                throw (new Exception(mObjPoAFuncResponse.Message));
                            }

                        }
                        else
                        {
                            throw (new Exception(mObjFuncResponse.Message));
                        }


                    }
                    catch (Exception Ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(Ex);
                        mObjAPIResponse.Success = false;

                        if (Ex.Message.Equals("Company with same name already exists"))
                        {
                            mObjAPIResponse.Message = Ex.Message;
                        }
                        else
                        {
                            mObjAPIResponse.Message = "Error occurred while adding company";
                        }

                        Transaction.Current.Rollback();
                    }
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("AddGovernment")]
        public IHttpActionResult AddGovernment(GovernmentModel pObjGovernmentModel)
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
                    try
                    {
                        Government mObjGovernment = new Government()
                        {
                            GovernmentID = 0,
                            GovernmentName = pObjGovernmentModel.GovernmentName.Trim(),
                            TIN = "None Listed",
                            GovernmentTypeID = pObjGovernmentModel.GovernmentTypeID,
                            TaxOfficeID = pObjGovernmentModel.TaxOfficeID,
                            TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                            ContactAddress = pObjGovernmentModel.ContactAddress,
                            ContactNumber = pObjGovernmentModel.MobileNumber1,
                            ContactEmail = pObjGovernmentModel.EmailAddress1,
                            NotificationMethodID = pObjGovernmentModel.NotificationMethodID,
                            Active = true,
                            CreatedBy = userId.HasValue ? userId : 22,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse<Government> mObjFuncResponse = new BLGovernment().BL_InsertUpdateGovernment(mObjGovernment);

                        if (mObjFuncResponse.Success)
                        {

                            Payment_Account mObjPaymentAccount = new Payment_Account()
                            {
                                PaymentAccountID = 0,
                                TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                                TaxPayerID = mObjFuncResponse.AdditionalData.GovernmentID,
                                SettlementMethodID = pObjGovernmentModel.PaymentMethodID,
                                SettlementStatusID = (int)EnumList.SettlementStatus.Settled,
                                PaymentDate = pObjGovernmentModel.PaymentDate,
                                TransactionRefNo = pObjGovernmentModel.TransactionRefNo,
                                Notes = pObjGovernmentModel.Notes,
                                RevenueStreamID = pObjGovernmentModel.RevenueStreamID,
                                RevenueSubStreamID = pObjGovernmentModel.RevenueSubStreamID,
                                AgencyID = pObjGovernmentModel.AgencyID,
                                Active = true,
                                CreatedBy = userId.HasValue ? userId : 22,
                                CreatedDate = CommUtil.GetCurrentDateTime()
                            };

                            FuncResponse<Payment_Account> mObjPoAFuncResponse = new BLPaymentAccount().BL_InsertUpdatePaymentAccount(mObjPaymentAccount);

                            if (mObjFuncResponse.Success)
                            {
                                scope.Complete();
                                mObjAPIResponse.Success = true;
                                mObjAPIResponse.Message = "Government added and amount added in account succesfully";
                            }
                            else
                            {
                                throw (new Exception(mObjPoAFuncResponse.Message));
                            }

                        }
                        else
                        {
                            throw (new Exception(mObjFuncResponse.Message));
                        }


                    }
                    catch (Exception Ex)
                    {
                        ErrorSignal.FromCurrentContext().Raise(Ex);
                        mObjAPIResponse.Success = false;
                        mObjAPIResponse.Message = "Error occurred while adding government";
                        Transaction.Current.Rollback();
                    }
                }
            }

            return Ok(mObjAPIResponse);
        }

    }
}
