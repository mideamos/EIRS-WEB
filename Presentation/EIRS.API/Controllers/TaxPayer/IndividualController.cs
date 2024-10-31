using EIRS.API.Models;
using EIRS.API.Utility;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using EIRS.Repository;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace EIRS.API.Controllers
{
    /// <summary>
    /// Individual Operations
    /// </summary>

    [RoutePrefix("TaxPayer/Individual")]

    public class IndividualController : BaseController
    {
        /// <summary>
        /// Returns a list of Individual
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Individual mObjIndividual = new Individual()
                {
                    intStatus = 1
                };

                IList<usp_GetIndividualList_Result> lstIndividual = new BLIndividual().BL_GetIndividualList(mObjIndividual);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstIndividual;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        /// <summary>
        /// Find Individual by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// 

        [HttpPost]
        [Route("InsertMinimal")]
        public IHttpActionResult InsertMinimal(IndividualMinimalModel model)
        {
            APIResponse response = new APIResponse();

            // Check if the Authorization header is present
            if (Request.Headers.Authorization == null)
            {
                response.Success = false;
                response.Message = "Authorization header is missing.";
                return Content(HttpStatusCode.Unauthorized, response);
            }

            // Validate the token
            String token = Request.Headers.Authorization.Parameter;
            if (token == null)
            {
                response.Success = false;
                response.Message = "Invalid token.";
                return Content(HttpStatusCode.Unauthorized, response);
            }

            // Check for required fields
            if (model == null || model.GenderID == null || model.TitleID == null || string.IsNullOrEmpty(model.FirstName)
                || string.IsNullOrEmpty(model.LastName) || model.TaxOfficeID == null || model.NationalityID == null
                || model.EconomicActivitiesID == null || model.NotificationMethodID == null || string.IsNullOrEmpty(model.ContactAddress))
            {
                response.Success = false;
                response.Message = "One or more required fields are missing.";
                return Content(HttpStatusCode.BadRequest, response);
            }

            // Populate the Individual entity
            Individual individual = new Individual
            {
                GenderID = model.GenderID.Value,
                TitleID = model.TitleID.Value,
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                DOB = TrynParse.parseDatetime(model.DOB),
                TIN = model.TIN,
                NIN = model.NIN,
                MobileNumber1 = model.MobileNumber1,
                MobileNumber2 = model.MobileNumber2,
                EmailAddress1 = model.EmailAddress1,
                EmailAddress2 = model.EmailAddress2,
                BiometricDetails = model.BiometricDetails,
                TaxOfficeID = model.TaxOfficeID.Value,
                MaritalStatusID = model.MaritalStatusID,
                NationalityID = model.NationalityID.Value,
                EconomicActivitiesID = model.EconomicActivitiesID.Value,
                NotificationMethodID = model.NotificationMethodID.Value,
                ContactAddress = model.ContactAddress,
                Active = true,
                CreatedBy = 22, // Replace with dynamic user ID if available
                CreatedDate = CommUtil.GetCurrentDateTime()
            };

            // Call business logic layer to save
            try
            {
                FuncResponse<Individual> funcResponse = new BLIndividual().BL_InsertUpdateIndividual(individual);

                if (funcResponse.Success)
                {
                    response.Success = true;
                    response.Message = funcResponse.Message;
                    response.Result = funcResponse.AdditionalData;
                }
                else
                {
                    response.Success = false;
                    response.Message = funcResponse.Message;
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = $"Error occurred while saving Individual - {ex.Message}";
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("Details/{id}")]
        public IHttpActionResult Details(int? id)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                if (id.GetValueOrDefault() > 0)
                {
                    Individual mObjIndividual = new Individual()
                    {
                        IndividualID = id.GetValueOrDefault(),
                        intStatus = 2
                    };

                    usp_GetIndividualList_Result mObjIndividualData = new BLIndividual().BL_GetIndividualDetails(mObjIndividual);

                    if (mObjIndividualData != null)
                    {
                        mObjAPIResponse.Success = true;
                        mObjAPIResponse.Result = mObjIndividualData;
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

        /// <summary>
        /// Add New Individual
        /// </summary>
        /// <param name="pObjIndividualModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert(IndividualViewModel pObjIndividualModel)
        {

            NewErrorLog.WriteFormModel("I got here in the controller 1", "SettlementResponse");
            APIResponse mObjAPIResponse = new APIResponse();
            NewErrorLog.WriteFormModel("I got here in the controller 1a", "SettlementResponse");
            // Check if the Authorization header is present
            if (Request.Headers.Authorization == null)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = "Authorization header is missing.";
                return Content(HttpStatusCode.Unauthorized, mObjAPIResponse);
            }
            String token = Request.Headers.Authorization.Parameter;
            NewErrorLog.WriteFormModel("I got here in the controller 1b", "SettlementResponse");
            int? usId = Utilities.GetUserId(token);
            int? userId = usId.HasValue ? usId : 0;
            // int? userId = 0;
            // Validate the token and get the user ID
            if (userId == null || userId <= 0)
            {
                // User ID is not valid or token is missing/invalid
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = "Unauthorized access. Please provide a valid token.";
                // return Ok(mObjAPIResponse);
                return Content(HttpStatusCode.Unauthorized, mObjAPIResponse);
            }
            NewErrorLog.WriteFormModel("I got here in the controller 1c", "SettlementResponse");

            if (!ModelState.IsValid)
            {

                NewErrorLog.WriteFormModel("I got here in the controller 2", "SettlementResponse");
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }
            else
            {
                //Redundant validation, remove validation in future versions
                if ((token != null) && (userId == 0))
                {
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Message = "Kindly Enter A Valid Token";
                }

                else
                {

                    NewErrorLog.WriteFormModel("I got here in the controller 3", "SettlementResponse");
                    HttpContext currentContext = HttpContext.Current;
                    string userName = currentContext.User.Identity.Name;
                    Individual mObjIndividual = new Individual()
                    {
                        IndividualID = 0,
                        GenderID = pObjIndividualModel.GenderID,
                        TitleID = pObjIndividualModel.TitleID,
                        FirstName = pObjIndividualModel.FirstName.Trim(),
                        LastName = pObjIndividualModel.LastName.Trim(),
                        MiddleName = pObjIndividualModel.MiddleName,
                        DOB = TrynParse.parseDatetime(pObjIndividualModel.DOB),
                        TIN = pObjIndividualModel.TIN,
                        NIN = pObjIndividualModel.NIN,
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
                        // CreatedBy = userId.HasValue ? userId : 22,
                        CreatedBy = userId,
                        CreatedDate = CommUtil.GetCurrentDateTime()
                    };

                    try
                    {

                        NewErrorLog.WriteFormModel("I got here in the controller 4", "SettlementResponse");
                        FuncResponse<Individual> mObjFuncResponse = new BLIndividual().BL_InsertUpdateIndividual(mObjIndividual);

                        if (mObjFuncResponse.Success)
                        {
                            mObjAPIResponse.Success = true;
                            mObjAPIResponse.Message = mObjFuncResponse.Message;
                            mObjAPIResponse.Result = mObjFuncResponse.AdditionalData;

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
                        mObjAPIResponse.Message = $"Error occurred while saving Individual - {ex.Message}";
                    }
                }
            }

            return Ok(mObjAPIResponse);
        }
        /// <summary>
        /// Add New Individual
        /// </summary>
        /// <param name="pObjIndividualModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("PayeInsert")]
        public IHttpActionResult PayeInsert(IndividualViewModel pObjIndividualModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();
            String token = Request.Headers.Authorization.Parameter;
            int? userId = 100;
            //int ctd =ClaimsIdentityExtensions.GetUserID(User.Identity);
            //if(ctd == 0)
            //{
            //    ctd = 100;
            //}
            Individual mObjIndividual = new Individual()
            {
                IndividualID = 0,
                GenderID = pObjIndividualModel.GenderID,
                TitleID = pObjIndividualModel.TitleID,
                FirstName = pObjIndividualModel.FirstName.Trim(),
                LastName = pObjIndividualModel.LastName.Trim(),
                MiddleName = pObjIndividualModel.MiddleName,
                DOB = TrynParse.parseDatetime(pObjIndividualModel.DOB),
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
                CreatedBy = userId.HasValue ? userId : 100,
                CreatedDate = CommUtil.GetCurrentDateTime()
            };

            try
            {
                FuncResponse<Individual> mObjFuncResponse = new BLIndividual().BL_InsertUpdateIndividual(mObjIndividual);

                if (mObjFuncResponse.Success)
                {
                    mObjAPIResponse.Success = true;
                    mObjAPIResponse.Message = mObjFuncResponse.Message;
                    mObjAPIResponse.Result = mObjFuncResponse.AdditionalData;

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
                mObjAPIResponse.Message = "Error occurred while saving Individual";
            }
            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("InsertWithoutNumber")]
        public IHttpActionResult InsertWithoutNumber(IndividualViewModel pObjIndividualModel)
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
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = 0,
                    GenderID = pObjIndividualModel.GenderID,
                    TitleID = pObjIndividualModel.TitleID,
                    FirstName = pObjIndividualModel.FirstName.Trim(),
                    LastName = pObjIndividualModel.LastName.Trim(),
                    MiddleName = pObjIndividualModel.MiddleName,
                    DOB = TrynParse.parseDatetime(pObjIndividualModel.DOB),
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
                    CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Individual> mObjFuncResponse = new BLIndividual().BL_InsertUpdateIndividual(mObjIndividual, true, true);

                    if (mObjFuncResponse.Success)
                    {
                        mObjAPIResponse.Success = true;
                        mObjAPIResponse.Message = mObjFuncResponse.Message;
                        mObjAPIResponse.Result = mObjFuncResponse.AdditionalData;

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
                    mObjAPIResponse.Message = "Error occurred while saving Individual";
                }
            }

            return Ok(mObjAPIResponse);
        }

        /// <summary>
        /// Update Individual
        /// </summary>
        /// <param name="pObjIndividualModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult Update(IndividualViewModel pObjIndividualModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            if (!ModelState.IsValid)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }
            else
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = pObjIndividualModel.IndividualID,
                    GenderID = pObjIndividualModel.GenderID,
                    TitleID = pObjIndividualModel.TitleID,
                    FirstName = pObjIndividualModel.FirstName.Trim(),
                    LastName = pObjIndividualModel.LastName.Trim(),
                    MiddleName = pObjIndividualModel.MiddleName,
                    DOB = TrynParse.parseDatetime(pObjIndividualModel.DOB),
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
                    Active = pObjIndividualModel.Active,
                    ModifiedBy = -1,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Individual> mObjFuncResponse = new BLIndividual().BL_InsertUpdateIndividual(mObjIndividual);

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
                    mObjAPIResponse.Message = "Error occurred while updating Individual";
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("SavePassword")]
        public IHttpActionResult SavePassword(TaxPayerPasswordViewModel pObjTaxPayerPasswordModel)
        {

            APIResponse mObjAPIResponse = new APIResponse();

            if (!ModelState.IsValid)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }
            else
            {
                Individual mObjIndividual = new Individual()
                {
                    IndividualID = pObjTaxPayerPasswordModel.TaxPayerID,
                    Password = EncryptDecrypt.Encrypt(pObjTaxPayerPasswordModel.Password),
                    RegisterationStatusID = (int)EnumList.RegisterationStatus.Completed,
                    RegisterationDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjFuncResponse = new BLIndividual().BL_UpdatePassword(mObjIndividual);

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
                    mObjAPIResponse.Message = "Error occurred while updating password";
                }
            }

            return Ok(mObjAPIResponse);
        }
    }
}
