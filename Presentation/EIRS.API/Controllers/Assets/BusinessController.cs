using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using EIRS.API.Utility;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;

namespace EIRS.API.Controllers
{
    /// <summary>
    /// Business Operations
    /// </summary>
    [RoutePrefix("Asset/Business")]
    
    public class BusinessController : BaseController
    {
        /// <summary>
        /// Returns a list of business
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Business mObjBusiness = new Business()
                {
                    intStatus = 1
                };

                IList<usp_GetBusinessListNewTy_Result> lstBusiness = new BLBusiness().BL_GetBusinessList(mObjBusiness);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstBusiness;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        /// <summary>
        /// Find business by id
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
                    Business mObjBusiness = new Business()
                    {
                        BusinessID = id.GetValueOrDefault(),
                        intStatus = 2
                    };

                    usp_GetBusinessListNewTy_Result mObjBusinessData = new BLBusiness().BL_GetBusinessDetails(mObjBusiness);

                    if (mObjBusinessData != null)
                    {
                        mObjAPIResponse.Success = true;
                        mObjAPIResponse.Result = mObjBusinessData;
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
        /// Add New Business
        /// </summary>
        /// <param name="pObjBusinessModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert(BusinessViewModel pObjBusinessModel)
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
                Business mObjBusiness = new Business()
                {
                    BusinessID = 0,
                    AssetTypeID = (int)EnumList.AssetTypes.Business,
                    BusinessTypeID = pObjBusinessModel.BusinessTypeID,
                    BusinessName = pObjBusinessModel.BusinessName,
                    LGAID = pObjBusinessModel.LGAID,
                    BusinessCategoryID = pObjBusinessModel.BusinessCategoryID,
                    BusinessSectorID = pObjBusinessModel.BusinessSectorID,
                    BusinessSubSectorID = pObjBusinessModel.BusinessSubSectorID,
                    BusinessStructureID = pObjBusinessModel.BusinessStructureID,
                    BusinessOperationID = pObjBusinessModel.BusinessOperationID,
                    SizeID = pObjBusinessModel.SizeID,
                    ContactName = pObjBusinessModel.ContactName,
                    BusinessAddress = pObjBusinessModel.BusinessAddress,
                    BusinessNumber = pObjBusinessModel.BusinessNumber,
                    Active = pObjBusinessModel.Active,
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Business> mObjFuncResponse = new BLBusiness().BL_InsertUpdateBusiness(mObjBusiness);

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
                    mObjAPIResponse.Message = "Error occurred while saving business";
                }
            }

            return Ok(mObjAPIResponse);
        }

        /// <summary>
        /// Update Business
        /// </summary>
        /// <param name="pObjBusinessModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Update")]
        public IHttpActionResult Update(BusinessViewModel pObjBusinessModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            if (!ModelState.IsValid)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }
            else
            {
                Business mObjBusiness = new Business()
                {
                    BusinessID = pObjBusinessModel.BusinessID,
                    AssetTypeID = (int)EnumList.AssetTypes.Business,
                    BusinessTypeID = pObjBusinessModel.BusinessTypeID,
                    BusinessName = pObjBusinessModel.BusinessName,
                    LGAID = pObjBusinessModel.LGAID,
                    BusinessCategoryID = pObjBusinessModel.BusinessCategoryID,
                    BusinessSectorID = pObjBusinessModel.BusinessSectorID,
                    BusinessSubSectorID = pObjBusinessModel.BusinessSubSectorID,
                    BusinessStructureID = pObjBusinessModel.BusinessStructureID,
                    BusinessOperationID = pObjBusinessModel.BusinessOperationID,
                    SizeID = pObjBusinessModel.SizeID,
                    ContactName = pObjBusinessModel.ContactName,
                    BusinessAddress = pObjBusinessModel.BusinessAddress,
                    BusinessNumber = pObjBusinessModel.BusinessNumber,
                    Active = pObjBusinessModel.Active,
                    ModifiedBy = -1,
                    ModifiedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Business> mObjFuncResponse = new BLBusiness().BL_InsertUpdateBusiness(mObjBusiness);

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
                    mObjAPIResponse.Message = "Error occurred while updating business";
                }
            }

            return Ok(mObjAPIResponse);
        }

        /// <summary>
        /// Add Tax Payers
        /// </summary>
        /// <param name="pObjAssetModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("AddTaxPayer")]
        public IHttpActionResult AddTaxPayer(TaxPayerAssetViewModel pObjAssetModel)
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
                BLTaxPayerAsset mobjBLTaxPayerAsset = new BLTaxPayerAsset();
                string[] strTaxPayerIds = pObjAssetModel.TaxPayerIds.Split(',');

                foreach (var vTaxPayerID in strTaxPayerIds)
                {
                    if (!string.IsNullOrWhiteSpace(vTaxPayerID))
                    {
                        MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                        {
                            AssetTypeID = (int)EnumList.AssetTypes.Business,
                            AssetID = pObjAssetModel.AssetID,
                            TaxPayerTypeID = pObjAssetModel.TaxPayerTypeID,
                            TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID,
                            TaxPayerID = TrynParse.parseInt(vTaxPayerID),
                            Active = pObjAssetModel.Active,
                           CreatedBy = userId.HasValue ? userId : 22,
                            CreatedDate = CommUtil.GetCurrentDateTime()
                        };

                        FuncResponse mObjResponse = mobjBLTaxPayerAsset.BL_InsertTaxPayerAsset(mObjTaxPayerAsset);
                    }
                }

                mObjAPIResponse.Success = true;

            }

            return Ok(mObjAPIResponse);
        }
    }
}
