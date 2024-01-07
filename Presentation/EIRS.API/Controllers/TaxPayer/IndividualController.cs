using EIRS.API.Models;
using EIRS.API.Utility;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
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
                if ((token != null) && (userId == 0))
                {
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Message = "Kindly Enter A Valid Token";
                }

                else
                {
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
                        CreatedBy = userId.HasValue ? userId : 22,
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
            int? userId = Utilities.GetUserId(token);
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
