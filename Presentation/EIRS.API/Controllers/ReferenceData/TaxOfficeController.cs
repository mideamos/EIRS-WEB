using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;

namespace EIRS.API.Controllers
{
    /// <summary>
    /// Tax Office Operations
    /// </summary>
    [RoutePrefix("ReferenceData/TaxOffice")]
    public class TaxOfficeController : BaseController
    {
        /// <summary>
        /// Returns a list of tax office
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Tax_Offices mObjTaxOffice = new Tax_Offices()
                {
                    intStatus = 1
                };

                IList<usp_GetTaxOfficeList_Result> lstTaxOffice = new BLTaxOffice().BL_GetTaxOfficeList(mObjTaxOffice);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstTaxOffice;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find tax office by id
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
        //            Tax_Offices mObjTaxOffice = new Tax_Offices()
        //            {
        //                TaxOfficeID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetTaxOfficeList_Result mObjTaxOfficeData = new BLTaxOffice().BL_GetTaxOfficeDetails(mObjTaxOffice);

        //            if (mObjTaxOfficeData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjTaxOfficeData;
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
        ///// <param name="pObjTaxOfficeModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(TaxOfficeViewModel pObjTaxOfficeModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Tax_Offices mObjTaxOffice = new Tax_Offices()
        //        {
        //            TaxOfficeID = 0,
        //            TaxOfficeName = pObjTaxOfficeModel.TaxOfficeName.Trim(),
        //            //AddressTypeID = pObjTaxOfficeModel.A
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLTaxOffice().BL_InsertUpdateTaxOffice(mObjTaxOffice);

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
        //            mObjAPIResponse.Message = "Error occurred while saving tax office";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update Asset Type
        ///// </summary>
        ///// <param name="pObjTaxOfficeModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(TaxOfficeViewModel pObjTaxOfficeModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Tax_Offices mObjTaxOffice = new Tax_Offices()
        //        {
        //            TaxOfficeID = pObjTaxOfficeModel.TaxOfficeID,
        //            TaxOfficeName = pObjTaxOfficeModel.TaxOfficeName.Trim(),
        //            Active = pObjTaxOfficeModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLTaxOffice().BL_InsertUpdateTaxOffice(mObjTaxOffice);

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
        //            mObjAPIResponse.Message = "Error occurred while updating tax office";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}