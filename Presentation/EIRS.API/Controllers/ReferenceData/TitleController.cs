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
    /// Title Operations
    /// </summary>
    [RoutePrefix("ReferenceData/Title")]
    //
    public class TitleController : BaseController
    {
        /// <summary>
        /// Returns a list of title
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List(int? GenderID)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                Title mObjTitle = new Title()
                {
                    intStatus = 1,
                    GenderID = GenderID.GetValueOrDefault()
                };

                IList<usp_GetTitleList_Result> lstTitle = new BLTitle().BL_GetTitleList(mObjTitle);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstTitle;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        ///// <summary>
        ///// Find title by id
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
        //            Title mObjTitle = new Title()
        //            {
        //                TitleID = id.GetValueOrDefault(),
        //                intStatus = 2
        //            };

        //            usp_GetTitleList_Result mObjTitleData = new BLTitle().BL_GetTitleDetails(mObjTitle);

        //            if (mObjTitleData != null)
        //            {
        //                mObjAPIResponse.Success = true;
        //                mObjAPIResponse.Result = mObjTitleData;
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
        ///// <param name="pObjTitleModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Insert")]
        //public IHttpActionResult Insert(TitleViewModel pObjTitleModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Title mObjTitle = new Title()
        //        {
        //            TitleID = 0,
        //            TitleName = pObjTitleModel.TitleName.Trim(),
        //            GenderID = pObjTitleModel.GenderID,
        //            Active = true,
        //           CreatedBy = 22,
        //            CreatedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLTitle().BL_InsertUpdateTitle(mObjTitle);

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
        //            mObjAPIResponse.Message = "Error occurred while saving title";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}

        ///// <summary>
        ///// Update Asset Type
        ///// </summary>
        ///// <param name="pObjTitleModel"></param>
        ///// <returns></returns>
        //[HttpPost]
        //[Route("Update")]
        //public IHttpActionResult Update(TitleViewModel pObjTitleModel)
        //{
        //    APIResponse mObjAPIResponse = new APIResponse();

        //    if (!ModelState.IsValid)
        //    {
        //        mObjAPIResponse.Success = false;
        //        mObjAPIResponse.Message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        //    }
        //    else
        //    {
        //        Title mObjTitle = new Title()
        //        {
        //            TitleID = pObjTitleModel.TitleID,
        //            TitleName = pObjTitleModel.TitleName.Trim(),
        //            GenderID = pObjTitleModel.GenderID,
        //            Active = pObjTitleModel.Active,
        //            ModifiedBy = -1,
        //            ModifiedDate = CommUtil.GetCurrentDateTime()
        //        };

        //        try
        //        {

        //            FuncResponse mObjFuncResponse = new BLTitle().BL_InsertUpdateTitle(mObjTitle);

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
        //            mObjAPIResponse.Message = "Error occurred while updating title";
        //        }
        //    }

        //    return Ok(mObjAPIResponse);
        //}
    }
}