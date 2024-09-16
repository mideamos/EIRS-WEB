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
    /// Tax Payer Role Operations
    /// </summary>
    [RoutePrefix("ReferenceData/TaxPayerRole")]
    public class TaxPayerRoleController : ApiController
    {
         /// <summary>
        /// Returns a list of tax payer role
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List(int? taxpayertpyeid = 0, int? assettypeid = 0)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                TaxPayer_Roles mObjTaxPayerRole = new TaxPayer_Roles()
                {
                    intStatus = 1,
                    TaxPayerTypeID = taxpayertpyeid.GetValueOrDefault(),
                    AssetTypeID = assettypeid.GetValueOrDefault()
                };

                IList<usp_GetTaxPayerRoleList_Result> lstTaxPayerRole = new BLTaxPayerRole().BL_GetTaxPayerRoleList(mObjTaxPayerRole);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstTaxPayerRole;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find tax payer role by id
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
        //            TaxPayer_Roles mObjTaxPayerRole = new TaxPayer_Roles()
        //            {
        //                TaxPayerRoleID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetTaxPayerRoleList_Result mObjTaxPayerRoleData = new BLTaxPayerRole().BL_GetTaxPayerRoleDetails(mObjTaxPayerRole);

        //            if (mObjTaxPayerRoleData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjTaxPayerRoleData;
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
        ///// <param name="pObjTaxPayerRoleModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(TaxPayerRoleViewModel pObjTaxPayerRoleModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        TaxPayer_Roles mObjTaxPayerRole = new TaxPayer_Roles()
        //        {
        //            TaxPayerRoleID = 0,
        //            TaxPayerRoleName = pObjTaxPayerRoleModel.TaxPayerRoleName.Trim(),
        //            AssetTypeID = pObjTaxPayerRoleModel.AssetTypeID,
        //            IsMultiLinkable = pObjTaxPayerRoleModel.isMultiLinkable,
        //            TaxPayerTypeID = pObjTaxPayerRoleModel.TaxPayerTypeID,
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLTaxPayerRole().BL_InsertUpdateTaxPayerRole(mObjTaxPayerRole);

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
        //            mObjAPIResponse.Message = "Error occurred while saving tax payer role";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update Asset Type
        ///// </summary>
        ///// <param name="pObjTaxPayerRoleModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(TaxPayerRoleViewModel pObjTaxPayerRoleModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        TaxPayer_Roles mObjTaxPayerRole = new TaxPayer_Roles()
        //        {
        //            TaxPayerRoleID = pObjTaxPayerRoleModel.TaxPayerRoleID,
        //            TaxPayerRoleName = pObjTaxPayerRoleModel.TaxPayerRoleName.Trim(),
        //            AssetTypeID = pObjTaxPayerRoleModel.AssetTypeID,
        //            IsMultiLinkable = pObjTaxPayerRoleModel.isMultiLinkable,
        //            TaxPayerTypeID = pObjTaxPayerRoleModel.TaxPayerTypeID,
        //            Active = pObjTaxPayerRoleModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLTaxPayerRole().BL_InsertUpdateTaxPayerRole(mObjTaxPayerRole);

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
        //            mObjAPIResponse.Message = "Error occurred while updating tax payer role";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}