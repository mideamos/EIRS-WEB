using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EIRS.API.Models;
using EIRS.API.Utility;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;

namespace EIRS.API.Controllers
{
    /// <summary>
    /// Company Operations
    /// </summary>
    [RoutePrefix("TaxPayer/Company")]

    public class CompanyController : BaseController
    {
        /// <summary>
        /// Returns a list of Company
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Company mObjCompany = new Company()
                {
                    intStatus = 1
                };

                IList<usp_GetCompanyList_Result> lstCompany = new BLCompany().BL_GetCompanyList(mObjCompany);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstCompany;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        /// <summary>
        /// Find Company by id
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
                    Company mObjCompany = new Company()
                    {
                        CompanyID = id.GetValueOrDefault(),
                        intStatus = 2
                    };

                    usp_GetCompanyList_Result mObjCompanyData = new BLCompany().BL_GetCompanyDetails(mObjCompany);

                    if (mObjCompanyData != null)
                    {
                        mObjAPIResponse.Success = true;
                        mObjAPIResponse.Result = mObjCompanyData;
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
        /// Add New Company
        /// </summary>
        /// <param name="pObjCompanyModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert(CompanyViewModel pObjCompanyModel)
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
                Company mObjCompany = new Company()
                {
                    CompanyID = 0,
                    CompanyName = pObjCompanyModel.CompanyName.Trim(),
                    TIN = pObjCompanyModel.TIN,
                    MobileNumber1 = pObjCompanyModel.MobileNumber1,
                    MobileNumber2 = pObjCompanyModel.MobileNumber2,
                    EmailAddress1 = pObjCompanyModel.EmailAddress1,
                    EmailAddress2 = pObjCompanyModel.EmailAddress2,
                    TaxOfficeID = pObjCompanyModel.TaxOfficeID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                    EconomicActivitiesID = pObjCompanyModel.EconomicActivitiesID,
                    NotificationMethodID = pObjCompanyModel.NotificationMethodID,
                    ContactAddress = pObjCompanyModel.ContactAddress,
                    CACRegistrationNumber = pObjCompanyModel.CACRegistrationNumber,
                    Active = true,
                    CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Company> mObjFuncResponse = new BLCompany().BL_InsertUpdateCompany(mObjCompany);

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
                    mObjAPIResponse.Message = "Error occurred while saving Company";
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("InsertWithoutNumber")]
        public IHttpActionResult InsertWithoutNumber(CompanyViewModel pObjCompanyModel)
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
                Company mObjCompany = new Company()
                {
                    CompanyID = 0,
                    CompanyName = pObjCompanyModel.CompanyName.Trim(),
                    TIN = pObjCompanyModel.TIN,
                    MobileNumber1 = pObjCompanyModel.MobileNumber1,
                    MobileNumber2 = pObjCompanyModel.MobileNumber2,
                    EmailAddress1 = pObjCompanyModel.EmailAddress1,
                    EmailAddress2 = pObjCompanyModel.EmailAddress2,
                    TaxOfficeID = pObjCompanyModel.TaxOfficeID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                    EconomicActivitiesID = pObjCompanyModel.EconomicActivitiesID,
                    NotificationMethodID = pObjCompanyModel.NotificationMethodID,
                    ContactAddress = pObjCompanyModel.ContactAddress,
                    CACRegistrationNumber = pObjCompanyModel.CACRegistrationNumber,
                    Active = true,
                    CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Company> mObjFuncResponse = new BLCompany().BL_InsertUpdateCompany(mObjCompany, true, true);

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
                    mObjAPIResponse.Message = "Error occurred while saving Company";
                }
            }

            return Ok(mObjAPIResponse);
        }

        /// <summary>
        /// Update Company
        /// </summary>
        /// <param name="pObjCompanyModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult Update(CompanyViewModel pObjCompanyModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            if (!ModelState.IsValid)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }
            else
            {
                Company mObjCompany = new Company()
                {
                    CompanyID = pObjCompanyModel.CompanyID,
                    CompanyName = pObjCompanyModel.CompanyName,
                    TIN = pObjCompanyModel.TIN,
                    MobileNumber1 = pObjCompanyModel.MobileNumber1,
                    MobileNumber2 = pObjCompanyModel.MobileNumber2,
                    EmailAddress1 = pObjCompanyModel.EmailAddress1,
                    EmailAddress2 = pObjCompanyModel.EmailAddress2,
                    TaxOfficeID = pObjCompanyModel.TaxOfficeID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                    EconomicActivitiesID = pObjCompanyModel.EconomicActivitiesID,
                    NotificationMethodID = pObjCompanyModel.NotificationMethodID,
                    ContactAddress = pObjCompanyModel.ContactAddress,
                    CACRegistrationNumber = pObjCompanyModel.CACRegistrationNumber,
                    Active = pObjCompanyModel.Active,
                    ModifiedBy = -1,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Company> mObjFuncResponse = new BLCompany().BL_InsertUpdateCompany(mObjCompany);

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
                    mObjAPIResponse.Message = "Error occurred while updating Company";
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("Search")]
        public IHttpActionResult Search(Company pObjCompany)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                pObjCompany.intStatus = 1;

                IList<usp_GetCompanyList_Result> lstCompany = new BLCompany().BL_GetCompanyList(pObjCompany);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstCompany;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("GetPAYEAsset")]
        public IHttpActionResult GetPAYEAsset(int CompanyID)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetCompanyPAYEAsset_Result> lstAsset = new BLProfile().BL_GetCompanyPAYEAssetList(CompanyID);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstAsset;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("GetAssetTaxPayer")]
        public IHttpActionResult GetAssetTaxPayer(int AssetTypeID, int AssetID)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetID = AssetID,
                    AssetTypeID = AssetTypeID,
                };

                IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstTaxPayerAsset;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
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
                Company mObjCompany = new Company()
                {
                    CompanyID = pObjTaxPayerPasswordModel.TaxPayerID,
                    Password = EncryptDecrypt.Encrypt(pObjTaxPayerPasswordModel.Password),
                    RegisterationStatusID = (int)EnumList.RegisterationStatus.Completed,
                    RegisterationDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjFuncResponse = new BLCompany().BL_UpdatePassword(mObjCompany);

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
