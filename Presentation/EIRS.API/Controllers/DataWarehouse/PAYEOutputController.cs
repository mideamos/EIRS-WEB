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

namespace EIRS.API.Controllers.DataWarehouse
{
    [RoutePrefix("DataWarehouse/PAYEOutput")]
    
    public class PAYEOutputController : BaseController
    {
        // GET: PAYEOutput
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List([FromUri]PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();
            try
            {
                PAYEOutput mObjOutput = new PAYEOutput();
                IList<usp_GETPAYEOutput_Result> lstPAYEOutput = new BLPAYEOutput().BL_GetPayeOutputList(mObjOutput);
                mObjAPIResponse = PaginationList(lstPAYEOutput, pobjPagingModel);
            }
            catch(Exception ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = ex.Message;
            }
            return Ok(mObjAPIResponse);
        }


        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert(PAYEOutputViewModel pObjPAYEOutputModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();
            String token = Request.Headers.Authorization.Parameter;
            int? userId = Utilities.GetUserId(token);

            if (!ModelState.IsValid)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message= string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }
            else
            {
                PAYEOutput mObjPAYEOutput = new PAYEOutput()
                {
                    PAYEOutputID = 0,
                    Transaction_Date = pObjPAYEOutputModel.Transaction_Date,
                    Employee_Rin = pObjPAYEOutputModel.Employee_Rin,
                    Employer_Rin = pObjPAYEOutputModel.Employer_Rin,
                    AssessmentYear = pObjPAYEOutputModel.AssessmentYear,
                    Assessment_Month = pObjPAYEOutputModel.Assessment_Month,
                    Monthly_CRA = pObjPAYEOutputModel.Monthly_CRA,
                    Monthly_Gross = pObjPAYEOutputModel.Monthly_Gross,
                    Monthly_ValidatedNHF = pObjPAYEOutputModel.Monthly_ValidatedNHF,
                    Monthly_ValidatedNHIS = pObjPAYEOutputModel.Monthly_ValidatedNHIS,
                    Monthly_ValidatedPension = pObjPAYEOutputModel.Monthly_ValidatedPension,
                    Monthly_ChargeableIncome = pObjPAYEOutputModel.Monthly_ChargeableIncome,
                    Monthly_TaxFreePay = pObjPAYEOutputModel.Monthly_TaxFreePay,
                    Monthly_Tax = pObjPAYEOutputModel.Monthly_Tax,
                    Tax_Office=pObjPAYEOutputModel.Tax_Office,
                    CreatedBy = userId.HasValue ? userId : 22,
                    //CreatedBy = ClaimsIdentityExtensions.GetUserID(User.Identity),
                   CreatedDate = CommUtil.GetCurrentDateTime()
                };
                try
                {
                    FuncResponse mObjResponse = new BLPAYEOutput().BL_InsertUpdatePayeOutput(mObjPAYEOutput);
                    if(mObjResponse.Success)
                    {
                        mObjAPIResponse.Success = true;
                        mObjAPIResponse.Message = mObjResponse.Message;
                    }
                    else
                    {
                        mObjAPIResponse.Success = false;
                        mObjAPIResponse.Message = mObjResponse.Message;
                    }
                }
                catch(Exception ex)
                {
                    mObjAPIResponse.Success = true;
                    mObjAPIResponse.Message = "Error Occurred While Saving PAYE Output";
                }
            }
            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("SearchByRIN")]
        public IHttpActionResult SearchByRIN(string RIN, [FromUri]PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();
            try
            {
                IList<usp_GETPAYEOutput_Result> lstPAYEOutput = new BLPAYEOutput().BL_GetPayeOutputList(new PAYEOutput() {});
                mObjAPIResponse = PaginationList(lstPAYEOutput, pobjPagingModel);
            }
            catch (Exception ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = ex.Message;
            }
            return Ok(mObjAPIResponse);
        }
    }
}