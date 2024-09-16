using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Models;
using EIRS.API.Models;
using System.Web.Http.Description;
using EIRS.API.Utility;

namespace EIRS.API.Controllers
{
    [RoutePrefix("DataWarehouse/PAYEInput")]
    
    public class PAYEInputController : BaseController
    {
        //VP_T-ERAS-8_PAYEInput

        /// <summary>
        /// Function Are Used To Gave List OF Data In PAYEInput
        /// </summary>
        /// <param name="pobjPagingModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List([FromUri]PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetPAYEInputList_Result> lstPAYInput = new BLPAYEInput().BL_GetPAYEInputList(new PAYEInput() { });
                mObjAPIResponse = PaginationList(lstPAYInput, pobjPagingModel);
            }
            catch(Exception ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        /// <summary>
        /// Function Are Used to Search Data using RIN
        /// </summary>
        /// <param name="RIN"></param>
        /// <param name="pobjPagingModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("SearchByRIN")]
        public IHttpActionResult SearchByRIN(string RIN, [FromUri]PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetPAYEInputList_Result> lstPAYInput = new BLPAYEInput().BL_GetPAYEInputList(new PAYEInput() { Employer_RIN = RIN });
                mObjAPIResponse = PaginationList(lstPAYInput, pobjPagingModel);
            }
            catch(Exception ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        /// <summary>
        /// Function are Used To Insert New Data in PAYEInput
        /// </summary>
        /// <param name="pObjPAYInputModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert(PAYEInputViewModel pObjPAYInputModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();
            String token = Request.Headers.Authorization.Parameter;
            int? userId = Utilities.GetUserId(token);

            if (!ModelState.IsValid)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = string.Join("|", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }
            else
            {
                PAYEInput mObjPAYInput = new PAYEInput()
                {
                    PAYEinputID = 0,
                    TransactionDate = pObjPAYInputModel.TranscationDate,
                    Employer_RIN = pObjPAYInputModel.Employer_RIN,
                    Employee_RIN = pObjPAYInputModel.Employee_RIN,
                    Assessment_Year = pObjPAYInputModel.Assessment_Year,
                    Start_Month = pObjPAYInputModel.Start_Month,
                    End_Month = pObjPAYInputModel.End_Month,
                    Annual_Basic = pObjPAYInputModel.Annual_Basic,
                    Annual_Rent = pObjPAYInputModel.Annual_Rent,
                    Annual_Transport = pObjPAYInputModel.Annual_Transport,
                    Annual_Utility = pObjPAYInputModel.Annual_Utility,
                    Annual_Meal = pObjPAYInputModel.Annual_Meal,
                    Other_Allowances_Annual = pObjPAYInputModel.Other_Allowances_Annual,
                    Leave_Transport_Grant_Annual = pObjPAYInputModel.Leave_Transport_Grant_Annual,
                    pension_contribution_declared = pObjPAYInputModel.pension_contribution_declared,
                    nhf_contribution_declared = pObjPAYInputModel.nhf_contribution_declared,
                    nhis_contribution_declared = pObjPAYInputModel.nhis_contribution_declared,
                    Tax_Office=pObjPAYInputModel.Tax_Office,
                    CreatedBy = userId.HasValue ? userId : 22,
                    //CreatedBy = ClaimsIdentityExtensions.GetUserID(User.Identity),
                    CreatedDate =CommUtil.GetCurrentDateTime()
                };

                try
                {
                    FuncResponse mObjFuncResponse = new BLPAYEInput().BL_InertUpdatePAYEInput(mObjPAYInput);

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
                catch(Exception ex)
                {
                    mObjAPIResponse.Success = true;
                    mObjAPIResponse.Message = "Error occurred while saving PAY Input";
                }
            }
            return Ok(mObjAPIResponse);
        }

    }
}