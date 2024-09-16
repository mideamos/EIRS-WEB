using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;


namespace EIRS.API.Controllers
{
    /// <summary>
    /// Tax Payer Type Operations
    /// </summary>
    [RoutePrefix("ReferenceData/TaxPayerType")]
    //
    public class TaxPayerTypeController : BaseController
    {
        /// <summary>
        /// Returns a list of tax payer type
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                TaxPayer_Types mObjTaxPayerType = new TaxPayer_Types()
                {
                    intStatus = 1
                };

                IList<usp_GetTaxPayerTypeList_Result> lstTaxPayerType = new BLTaxPayerType().BL_GetTaxPayerTypeList(mObjTaxPayerType);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstTaxPayerType;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find tax payer type by id
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[Route("Details/{id}")]
        //public IHttpActionResult Details(int? id)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    try
        //    {
        //        if (id.GetValueOrDefault() > 0)
        //        {
        //            TaxPayer_Types mObjTaxPayerType = new TaxPayer_Types()
        //            {
        //                TaxPayerTypeID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetTaxPayerTypeList_Result mObjTaxPayerTypeData = new BLTaxPayerType().BL_GetTaxPayerTypeDetails(mObjTaxPayerType);

        //            if (mObjTaxPayerTypeData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjTaxPayerTypeData;
        //            }
        //            else
        //            {
        //                mObjAPIResponse.Success = false;
        //                mObjAPIResponse.Result = "Invalid Request";
        //            }
        //        }
        //        else
        //        {
        //            mObjAPIResponse.Success = false;
        //            mObjAPIResponse.Result = "Invalid Request";
        //        }


        //    }
        //    catch (Exception Ex)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = Ex.Message;
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Add New Asset Type
        ///// </summary>
        ///// <param name="pObjTaxPayerTypeModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(TaxPayerTypeViewModel pObjTaxPayerTypeModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        TaxPayer_Types mObjTaxPayerType = new TaxPayer_Types()
        //        {
        //            TaxPayerTypeID = 0,
        //            TaxPayerTypeName = pObjTaxPayerTypeModel.TaxPayerTypeName.Trim(),
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLTaxPayerType().BL_InsertUpdateTaxPayerType(mObjTaxPayerType);

        //            if (mObjFuncResponse.Success)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Message = mObjFuncResponse.Message;
        //            }
        //            else
        //            {
        //                mObjAPIResponse.Success = false;
        //                mObjAPIResponse.Message = mObjFuncResponse.Message;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            mObjAPIResponse.Success = true;
        //            mObjAPIResponse.Message = "Error occurred while saving tax payer type";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update Asset Type
        ///// </summary>
        ///// <param name="pObjTaxPayerTypeModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(TaxPayerTypeViewModel pObjTaxPayerTypeModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        TaxPayer_Types mObjTaxPayerType = new TaxPayer_Types()
        //        {
        //            TaxPayerTypeID = pObjTaxPayerTypeModel.TaxPayerTypeID,
        //            TaxPayerTypeName = pObjTaxPayerTypeModel.TaxPayerTypeName.Trim(),
        //            Active = pObjTaxPayerTypeModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLTaxPayerType().BL_InsertUpdateTaxPayerType(mObjTaxPayerType);

        //            if (mObjFuncResponse.Success)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Message = mObjFuncResponse.Message;
        //            }
        //            else
        //            {
        //                mObjAPIResponse.Success = false;
        //                mObjAPIResponse.Message = mObjFuncResponse.Message;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            mObjAPIResponse.Success = true;
        //            mObjAPIResponse.Message = "Error occurred while updating tax payer type";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}