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

namespace EIRS.API.Controllers.DataWarehouse
{
    [RoutePrefix("DataWarehouse/DAOutput")]
    
    public class DAOutputController : BaseController
    {
        // GET: DAOutput
        /// <summary>
        /// Function Are Used To Gave List OF Data In DAOutput
        /// </summary>
        /// <param name="pobjPagingModel"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List([FromUri]PagingParameterModel pObjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();
            try
            {
                IList<usp_GetDAOutputList_Result> lstDAOutput = new BLDAOutput().BL_GetOutputList(new DAOutput() { });
                mObjAPIResponse = PaginationList(lstDAOutput, pObjPagingModel);
            }
            catch (Exception ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = ex.Message;
            }
            return Ok(mObjAPIResponse);
        }

        /// <summary>
        /// Function are Used To Insert New Data in DAOutput
        /// </summary>
        /// <param name="pObjDAOutputModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert(DAOutputViewModel pObjOutputModel)
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
                DAOutput mObjDAOutput = new DAOutput()
                {
                    DAOutputID = 0,
                    Assessment_Year = pObjOutputModel.Assessment_Year,
                    Business_RIN = pObjOutputModel.Business_RIN,
                    Business_TIN = pObjOutputModel.Business_TIN,
                    Employee_PAYE_Contribution = pObjOutputModel.Employee_PAYE_Contribution,
                    Life_Assurance = pObjOutputModel.Life_Assurance,
                    NHF_Declared = pObjOutputModel.NHF_Declared,
                    NHIS_Declared = pObjOutputModel.NHIS_Declared,
                    PAYE_Income = pObjOutputModel.PAYE_Income,
                    PAYE_NHF = pObjOutputModel.PAYE_NHF,
                    PAYE_NHIS = pObjOutputModel.PAYE_NHIS,
                    PAYE_Pension = pObjOutputModel.PAYE_Pension,
                    Pension_Contribution_Declared = pObjOutputModel.Pension_Contribution_Declared,
                    Share_of_Assessment = pObjOutputModel.Share_of_Assessment,
                    Taxpayer_RIN = pObjOutputModel.Taxpayer_RIN,
                    Taxpayer_TIN = pObjOutputModel.Taxpayer_TIN,
                    tax_office = pObjOutputModel.tax_office,
                    Total_Income = pObjOutputModel.Total_Income,
                    transaction_date = pObjOutputModel.transaction_date,
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };
                try
                {
                    FuncResponse mObjResponse = new BLDAOutput().BL_InsertUpdateDAOutput(mObjDAOutput);
                    if (mObjResponse.Success)
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
                catch (Exception ex)
                {
                    mObjAPIResponse.Success = true;
                    mObjAPIResponse.Message = "Error Occurred While Saving DA Output";
                }
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
                IList<usp_GetDAOutputList_Result> lstDAOutput = new BLDAOutput().BL_GetOutputList(new DAOutput() { });
                mObjAPIResponse = PaginationList(lstDAOutput, pobjPagingModel);
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