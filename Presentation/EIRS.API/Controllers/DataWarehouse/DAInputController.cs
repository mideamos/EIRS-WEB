using EIRS.API.Models;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Net.Http;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using EIRS.API.Utility;

namespace EIRS.API.Controllers.DataWarehouse
{
    [RoutePrefix("DataWarehouse/DAInput")]
    
    public class DAInputController : BaseController
    {
        // GET: DAInput
        /// <summary>
        /// Function Are Used To Gave List OF Data In DAInput
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
                IList<usp_GetDAInputList_Result> lstDAInput = new BLDAInput().BL_GetDAInputList(new DAInput() { });
                mObjAPIResponse = PaginationList(lstDAInput, pobjPagingModel);
            }
            catch (Exception ex)
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
                IList<usp_GetDAInputList_Result> lstDAInput = new BLDAInput().BL_GetDAInputList(new DAInput() { Taxpayer_RIN = RIN });
                mObjAPIResponse = PaginationList(lstDAInput, pobjPagingModel);
            }
            catch (Exception ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = ex.Message;
            }

            return Ok(mObjAPIResponse);
        }


        /// <summary>
        /// Function are Used To Insert New Data in DAInput
        /// </summary>
        /// <param name="pObjDAInputModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert(DAInputViewModel pObjDAInputModel)
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
                DAInput mObjDAInput = new DAInput()
                {
                    DAInputID = 0,
                    transaction_date = pObjDAInputModel.transaction_date,
                    Taxpayer_RIN = pObjDAInputModel.Taxpayer_RIN,
                    Taxpayer_TIN = pObjDAInputModel.Taxpayer_TIN,
                    Start_Month = pObjDAInputModel.Start_Month,
                    Share_of_Partnership_Profit = pObjDAInputModel.Share_of_Partnership_Profit,
                    Salaries = pObjDAInputModel.Salaries,
                    Rent = pObjDAInputModel.Rent,
                    Pension_Contribution_Declared = pObjDAInputModel.Pension_Contribution_Declared,
                    PAYE_Pension = pObjDAInputModel.PAYE_Pension,
                    PAYE_NHIS = pObjDAInputModel.PAYE_NHIS,
                    PAYE_NHF = pObjDAInputModel.PAYE_NHF,
                    PAYE_Income = pObjDAInputModel.PAYE_Income,
                    Other_Incomes_Not_Included = pObjDAInputModel.Other_Incomes_Not_Included,
                    NHIS_Declared = pObjDAInputModel.NHIS_Declared,
                    NHF__Declared = pObjDAInputModel.NHF_Declared,
                    Life_Assurance = pObjDAInputModel.Life_Assurance,
                    Interest_on_Discount = pObjDAInputModel.Interest_on_Discount,
                    Gratuities = pObjDAInputModel.Gratuities,
                    Employee_PAYE_Contribution = pObjDAInputModel.Employee_PAYE_Contribution,
                    Commissions_Recieved = pObjDAInputModel.Commissions_Recieved,
                    Bonuses = pObjDAInputModel.Bonuses,
                    Annuity = pObjDAInputModel.Annuity,
                    Assessment_Year = pObjDAInputModel.Assessment_Year,
                    Business_RIN = pObjDAInputModel.Business_RIN,
                    Business_TIN = pObjDAInputModel.Business_TIN,
                    tax_office = pObjDAInputModel.tax_office,
                    Wages = pObjDAInputModel.Wages,
                    Total_Income = pObjDAInputModel.Total_Income,
                    Trade = pObjDAInputModel.Trade,
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };
                try
                {
                    FuncResponse mObjFuncResponse = new BLDAInput().BL_InertUpdateDAInput(mObjDAInput);

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
                    mObjAPIResponse.Message = "Error occurred while saving DA Input";
                }
            }
            return Ok(mObjAPIResponse);
        }
    }
}