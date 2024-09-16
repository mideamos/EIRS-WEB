using EIRS.API.Models;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace EIRS.API.Controllers
{
    [RoutePrefix("SupplierData")]

    public class SupplierDataController : BaseController
    {
        EIRSEntities _db;
        #region WHT Commercial Banks

        [HttpGet]
        [Route("WHT_Commercial_Banks_TaxPayer")]
        public IHttpActionResult WHT_Commercial_Banks_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Withholding Tax Collections - Commercial Banks" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WHT_Commercial_Banks_Asset")]
        public IHttpActionResult WHT_Commercial_Banks_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Withholding Tax Collections - Commercial Banks" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WHT_Commercial_Banks_Profile")]
        public IHttpActionResult WHT_Commercial_Banks_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Withholding Tax Collections - Commercial Banks" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WHT_Commercial_Banks_AssessmentRule")]
        public IHttpActionResult WHT_Commercial_Banks_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Withholding Tax Collections - Commercial Banks" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WHT_Commercial_Banks_AssessmentItem")]
        public IHttpActionResult WHT_Commercial_Banks_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Withholding Tax Collections - Commercial Banks" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WHT_Commercial_Banks_Assessment")]
        public IHttpActionResult WHT_Commercial_Banks_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Withholding Tax Collections - Commercial Banks" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region WHT Savings & Loans

        [HttpGet]
        [Route("WHT_Savings_Loans_TaxPayer")]
        public IHttpActionResult WHT_Savings_Loans_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Withholding Tax Collections - Savings & Loans" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WHT_Savings_Loans_Asset")]
        public IHttpActionResult WHT_Savings_Loans_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Withholding Tax Collections - Savings & Loans" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WHT_Savings_Loans_Profile")]
        public IHttpActionResult WHT_Savings_Loans_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Withholding Tax Collections - Savings & Loans" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WHT_Savings_Loans_AssessmentRule")]
        public IHttpActionResult WHT_Savings_Loans_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Withholding Tax Collections - Savings & Loans" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WHT_Savings_Loans_AssessmentItem")]
        public IHttpActionResult WHT_Savings_Loans_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Withholding Tax Collections - Savings & Loans" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WHT_Savings_Loans_Assessment")]
        public IHttpActionResult WHT_Savings_Loans_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Withholding Tax Collections - Savings & Loans" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Consumption Tax Collections - Bars

        [HttpGet]
        [Route("CTC_Bars_TaxPayer")]
        public IHttpActionResult CTC_Bars_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Bars" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Bars_Asset")]
        public IHttpActionResult CTC_Bars_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Bars" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Bars_Profile")]
        public IHttpActionResult CTC_Bars_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Bars" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Bars_AssessmentRule")]
        public IHttpActionResult CTC_Bars_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Bars" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Bars_AssessmentItem")]
        public IHttpActionResult CTC_Bars_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Bars" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Bars_Assessment")]
        public IHttpActionResult CTC_Bars_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Bars" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Consumption Tax Collections - Event Centers

        [HttpGet]
        [Route("CTC_Event_Centers_TaxPayer")]
        public IHttpActionResult CTC_Event_Centers_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Event Centres" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Event_Centers_Asset")]
        public IHttpActionResult CTC_Event_Centers_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Event Centres" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Event_Centers_Profile")]
        public IHttpActionResult CTC_Event_Centers_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Event Centres" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Event_Centers_AssessmentRule")]
        public IHttpActionResult CTC_Event_Centers_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Event Centres" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Event_Centers_AssessmentItem")]
        public IHttpActionResult CTC_Event_Centers_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Event Centres" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Event_Centers_Assessment")]
        public IHttpActionResult CTC_Event_Centers_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Event Centres" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Consumption Tax Collections - Guest Houses

        [HttpGet]
        [Route("CTC_Guest_Houses_TaxPayer")]
        public IHttpActionResult CTC_Guest_Houses_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Guest Houses" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Guest_Houses_Asset")]
        public IHttpActionResult CTC_Guest_Houses_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Guest Houses" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Guest_Houses_Profile")]
        public IHttpActionResult CTC_Guest_Houses_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Guest Houses" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Guest_Houses_AssessmentRule")]
        public IHttpActionResult CTC_Guest_Houses_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Guest Houses" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Guest_Houses_AssessmentItem")]
        public IHttpActionResult CTC_Guest_Houses_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Guest Houses" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Guest_Houses_Assessment")]
        public IHttpActionResult CTC_Guest_Houses_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Guest Houses" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Consumption Tax Collections - Hotels

        [HttpGet]
        [Route("CTC_Hotels_TaxPayer")]
        public IHttpActionResult CTC_Hotels_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Hotels" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Hotels_Asset")]
        public IHttpActionResult CTC_Hotels_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Hotels" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Hotels_Profile")]
        public IHttpActionResult CTC_Hotels_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Hotels" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Hotels_AssessmentRule")]
        public IHttpActionResult CTC_Hotels_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Hotels" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Hotels_AssessmentItem")]
        public IHttpActionResult CTC_Hotels_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Hotels" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Hotels_Assessment")]
        public IHttpActionResult CTC_Hotels_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Hotels" });

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstAssessmentData;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Consumption Tax Collections - Motels

        [HttpGet]
        [Route("CTC_Motels_TaxPayer")]
        public IHttpActionResult CTC_Motels_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Motels" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Motels_Asset")]
        public IHttpActionResult CTC_Motels_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Motels" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Motels_Profile")]
        public IHttpActionResult CTC_Motels_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Motels" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Motels_AssessmentRule")]
        public IHttpActionResult CTC_Motels_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Motels" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Motels_AssessmentItem")]
        public IHttpActionResult CTC_Motels_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Motels" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Motels_Assessment")]
        public IHttpActionResult CTC_Motels_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Motels" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Consumption Tax Collections - Online Drinks Trading

        [HttpGet]
        [Route("CTC_Online_Drink_Trading_TaxPayer")]
        public IHttpActionResult CTC_Online_Drink_Trading_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Online Drinks Trading" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Online_Drink_Trading_Asset")]
        public IHttpActionResult CTC_Online_Drink_Trading_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Online Drinks Trading" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Online_Drink_Trading_Profile")]
        public IHttpActionResult CTC_Online_Drink_Trading_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Online Drinks Trading" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Online_Drink_Trading_AssessmentRule")]
        public IHttpActionResult CTC_Online_Drink_Trading_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Online Drinks Trading" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Online_Drink_Trading_AssessmentItem")]
        public IHttpActionResult CTC_Online_Drink_Trading_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Online Drinks Trading" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Online_Drink_Trading_Assessment")]
        public IHttpActionResult CTC_Online_Drink_Trading_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Online Drinks Trading" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Consumption Tax Collections - Restaurants

        [HttpGet]
        [Route("CTC_Restaurants_TaxPayer")]
        public IHttpActionResult CTC_Restaurants_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Restaurants" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Restaurants_Asset")]
        public IHttpActionResult CTC_Restaurants_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Restaurants" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Restaurants_Profile")]
        public IHttpActionResult CTC_Restaurants_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Restaurants" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Restaurants_AssessmentRule")]
        public IHttpActionResult CTC_Restaurants_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Restaurants" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Restaurants_AssessmentItem")]
        public IHttpActionResult CTC_Restaurants_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Restaurants" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("CTC_Restaurants_Assessment")]
        public IHttpActionResult CTC_Restaurants_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Consumption Tax Collections - Restaurants" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Land Use Charge - Commercial - Building

        [HttpGet]
        [Route("LUC_Commercial_Building_TaxPayer")]
        public IHttpActionResult LUC_Commercial_Building_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Commercial - Building" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Commercial_Building_Asset")]
        public IHttpActionResult LUC_Commercial_Building_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetLandBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetLandBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Commercial - Building" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Commercial_Building_Profile")]
        public IHttpActionResult LUC_Commercial_Building_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Commercial - Building" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Commercial_Building_AssessmentRule")]
        public IHttpActionResult LUC_Commercial_Building_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Commercial - Building" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Commercial_Building_AssessmentItem")]
        public IHttpActionResult LUC_Commercial_Building_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Commercial - Building" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Commercial_Building_Assessment")]
        public IHttpActionResult LUC_Commercial_Building_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Commercial - Building" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Land Use Charge - Commercial Residential - Building

        [HttpGet]
        [Route("LUC_Commercial_Residential_Building_TaxPayer")]
        public IHttpActionResult LUC_Commercial_Residential_Building_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Commercial Residential - Building" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Commercial_Residential_Building_Asset")]
        public IHttpActionResult LUC_Commercial_Residential_Building_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetLandBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetLandBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Commercial Residential - Building" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Commercial_Residential_Building_Profile")]
        public IHttpActionResult LUC_Commercial_Residential_Building_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Commercial Residential - Building" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Commercial_Residential_Building_AssessmentRule")]
        public IHttpActionResult LUC_Commercial_Residential_Building_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Commercial Residential - Building" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Commercial_Residential_Building_AssessmentItem")]
        public IHttpActionResult LUC_Commercial_Residential_Building_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Commercial Residential - Building" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Commercial_Residential_Building_Assessment")]
        public IHttpActionResult LUC_Commercial_Residential_Building_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Commercial Residential - Building" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Land Use Charge - Farming - Business

        [HttpGet]
        [Route("LUC_Farming_Business_TaxPayer")]
        public IHttpActionResult LUC_Farming_Business_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Farming - Business" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Farming_Business_Asset")]
        public IHttpActionResult LUC_Farming_Business_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetLandBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetLandBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Farming - Business" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Farming_Business_Profile")]
        public IHttpActionResult LUC_Farming_Business_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Farming - Business" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Farming_Business_AssessmentRule")]
        public IHttpActionResult LUC_Farming_Business_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Farming - Business" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Farming_Business_AssessmentItem")]
        public IHttpActionResult LUC_Farming_Business_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Farming - Business" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Farming_Business_Assessment")]
        public IHttpActionResult LUC_Farming_Business_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Farming - Business" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Land Use Charge - Industrial - Building

        [HttpGet]
        [Route("LUC_Industrial_Building_TaxPayer")]
        public IHttpActionResult LUC_Industrial_Building_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Industrial - Building" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Industrial_Building_Asset")]
        public IHttpActionResult LUC_Industrial_Building_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetLandBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetLandBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Industrial - Building" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Industrial_Building_Profile")]
        public IHttpActionResult LUC_Industrial_Building_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Industrial - Building" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Industrial_Building_AssessmentRule")]
        public IHttpActionResult LUC_Industrial_Building_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Industrial - Building" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Industrial_Building_AssessmentItem")]
        public IHttpActionResult LUC_Industrial_Building_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Industrial - Building" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Industrial_Building_Assessment")]
        public IHttpActionResult LUC_Industrial_Building_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Industrial - Building" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Land Use Charge - Mechanics - Business

        [HttpGet]
        [Route("LUC_Mechanics_Business_TaxPayer")]
        public IHttpActionResult LUC_Mechanics_Business_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Mechanics - Business" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Mechanics_Business_Asset")]
        public IHttpActionResult LUC_Mechanics_Business_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetLandBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetLandBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Mechanics - Business" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Mechanics_Business_Profile")]
        public IHttpActionResult LUC_Mechanics_Business_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Mechanics - Business" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Mechanics_Business_AssessmentRule")]
        public IHttpActionResult LUC_Mechanics_Business_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Mechanics - Business" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Mechanics_Business_AssessmentItem")]
        public IHttpActionResult LUC_Mechanics_Business_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Mechanics - Business" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Mechanics_Business_Assessment")]
        public IHttpActionResult LUC_Mechanics_Business_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Mechanics - Business" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Land Use Charge - Open Trading - Business

        [HttpGet]
        [Route("LUC_Open_Trading_Business_TaxPayer")]
        public IHttpActionResult LUC_Open_Trading_Business_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Open Trading - Business" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Open_Trading_Business_Asset")]
        public IHttpActionResult LUC_Open_Trading_Business_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetLandBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetLandBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Open Trading - Business" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Open_Trading_Business_Profile")]
        public IHttpActionResult LUC_Open_Trading_Business_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Open Trading - Business" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Open_Trading_Business_AssessmentRule")]
        public IHttpActionResult LUC_Open_Trading_Business_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Open Trading - Business" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Open_Trading_Business_AssessmentItem")]
        public IHttpActionResult LUC_Open_Trading_Business_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Open Trading - Business" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Open_Trading_Business_Assessment")]
        public IHttpActionResult LUC_Open_Trading_Business_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Open Trading - Business" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Land Use Charge - Other Informal - Business

        [HttpGet]
        [Route("LUC_Other_Informal_Business_TaxPayer")]
        public IHttpActionResult LUC_Other_Informal_Business_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Other Informal - Business" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Other_Informal_Business_Asset")]
        public IHttpActionResult LUC_Other_Informal_Business_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetLandBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetLandBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Other Informal - Business" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Other_Informal_Business_Profile")]
        public IHttpActionResult LUC_Other_Informal_Business_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Other Informal - Business" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Other_Informal_Business_AssessmentRule")]
        public IHttpActionResult LUC_Other_Informal_Business_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Other Informal - Business" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Other_Informal_Business_AssessmentItem")]
        public IHttpActionResult LUC_Other_Informal_Business_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Other Informal - Business" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Other_Informal_Business_Assessment")]
        public IHttpActionResult LUC_Other_Informal_Business_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Other Informal - Business" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Land Use Charge - Private Residential - Building

        [HttpGet]
        [Route("LUC_Private_Residential_Building_TaxPayer")]
        public IHttpActionResult LUC_Private_Residential_Building_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Private Residential - Building" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Private_Residential_Building_Asset")]
        public IHttpActionResult LUC_Private_Residential_Building_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetLandBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetLandBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Private Residential - Building" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Private_Residential_Building_Profile")]
        public IHttpActionResult LUC_Private_Residential_Building_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Private Residential - Building" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Private_Residential_Building_AssessmentRule")]
        public IHttpActionResult LUC_Private_Residential_Building_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Private Residential - Building" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Private_Residential_Building_AssessmentItem")]
        public IHttpActionResult LUC_Private_Residential_Building_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Private Residential - Building" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("LUC_Private_Residential_Building_Assessment")]
        public IHttpActionResult LUC_Private_Residential_Building_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Land Use Charge - Private Residential - Building" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Waste Management Fees - Commercial

        [HttpGet]
        [Route("WM_Commercial_TaxPayer")]
        public IHttpActionResult WM_Commercial_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Commercial" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Commercial_Asset")]
        public IHttpActionResult WM_Commercial_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBuildingBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBuildingBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Commercial" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Commercial_Profile")]
        public IHttpActionResult WM_Commercial_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Commercial" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Commercial_AssessmentRule")]
        public IHttpActionResult WM_Commercial_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Commercial" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Commercial_AssessmentItem")]
        public IHttpActionResult WM_Commercial_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Commercial" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Commercial_Assessment")]
        public IHttpActionResult WM_Commercial_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Commercial" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Waste Management Fees - Commercial Residential

        [HttpGet]
        [Route("WM_Commercial_Residential_TaxPayer")]
        public IHttpActionResult WM_Commercial_Residential_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Commercial Residential" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Commercial_Residential_Asset")]
        public IHttpActionResult WM_Commercial_Residential_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBuildingBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBuildingBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Commercial Residential" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Commercial_Residential_Profile")]
        public IHttpActionResult WM_Commercial_Residential_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Commercial Residential" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Commercial_Residential_AssessmentRule")]
        public IHttpActionResult WM_Commercial_Residential_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Commercial Residential" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Commercial_Residential_AssessmentItem")]
        public IHttpActionResult WM_Commercial_Residential_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Commercial Residential" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Commercial_Residential_Assessment")]
        public IHttpActionResult WM_Commercial_Residential_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Commercial Residential" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Waste Management Fees - Religious

        [HttpGet]
        [Route("WM_Religious_TaxPayer")]
        public IHttpActionResult WM_Religious_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Religious" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Religious_Asset")]
        public IHttpActionResult WM_Religious_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBuildingBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBuildingBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Religious" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Religious_Profile")]
        public IHttpActionResult WM_Religious_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Religious" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Religious_AssessmentRule")]
        public IHttpActionResult WM_Religious_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Religious" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Religious_AssessmentItem")]
        public IHttpActionResult WM_Religious_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Religious" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Religious_Assessment")]
        public IHttpActionResult WM_Religious_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Religious" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Waste Management Fees - Residential

        [HttpGet]
        [Route("WM_Residential_TaxPayer")]
        public IHttpActionResult WM_Residential_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Residential" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Residential_Asset")]
        public IHttpActionResult WM_Residential_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBuildingBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBuildingBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Residential" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Residential_Profile")]
        public IHttpActionResult WM_Residential_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Residential" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Residential_AssessmentRule")]
        public IHttpActionResult WM_Residential_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Residential" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Residential_AssessmentItem")]
        public IHttpActionResult WM_Residential_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Residential" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Residential_Assessment")]
        public IHttpActionResult WM_Residential_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Residential" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Waste Management Fees - Hotel

        [HttpGet]
        [Route("WM_Hotel_TaxPayer")]
        public IHttpActionResult WM_Hotel_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Hotel" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Hotel_Asset")]
        public IHttpActionResult WM_Hotel_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBuildingBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBuildingBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Hotel" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Hotel_Profile")]
        public IHttpActionResult WM_Hotel_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Hotel" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Hotel_AssessmentRule")]
        public IHttpActionResult WM_Hotel_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Hotel" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Hotel_AssessmentItem")]
        public IHttpActionResult WM_Hotel_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Hotel" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Hotel_Assessment")]
        public IHttpActionResult WM_Hotel_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Hotel" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Waste Management Fees - School

        [HttpGet]
        [Route("WM_School_TaxPayer")]
        public IHttpActionResult WM_School_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - School" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_School_Asset")]
        public IHttpActionResult WM_School_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBuildingBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBuildingBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - School" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_School_Profile")]
        public IHttpActionResult WM_School_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - School" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_School_AssessmentRule")]
        public IHttpActionResult WM_School_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - School" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_School_AssessmentItem")]
        public IHttpActionResult WM_School_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - School" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_School_Assessment")]
        public IHttpActionResult WM_School_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - School" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Waste Management Fees - Event Centre

        [HttpGet]
        [Route("WM_Event_Centre_TaxPayer")]
        public IHttpActionResult WM_Event_Centre_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Event Centre" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Event_Centre_Asset")]
        public IHttpActionResult WM_Event_Centre_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBuildingBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBuildingBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Event Centre" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Event_Centre_Profile")]
        public IHttpActionResult WM_Event_Centre_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Event Centre" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Event_Centre_AssessmentRule")]
        public IHttpActionResult WM_Event_Centre_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Event Centre" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Event_Centre_AssessmentItem")]
        public IHttpActionResult WM_Event_Centre_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Event Centre" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Event_Centre_Assessment")]
        public IHttpActionResult WM_Event_Centre_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Event Centre" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Waste Management Fees - Hospital

        [HttpGet]
        [Route("WM_Hospital_TaxPayer")]
        public IHttpActionResult WM_Hospital_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Hospital" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Hospital_Asset")]
        public IHttpActionResult WM_Hospital_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBuildingBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBuildingBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Hospital" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Hospital_Profile")]
        public IHttpActionResult WM_Hospital_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Hospital" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Hospital_AssessmentRule")]
        public IHttpActionResult WM_Hospital_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Hospital" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Hospital_AssessmentItem")]
        public IHttpActionResult WM_Hospital_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Hospital" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("WM_Hospital_Assessment")]
        public IHttpActionResult WM_Hospital_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Waste Management Fees - Hospital" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Pools Betting - Casino Operators

        [HttpGet]
        [Route("PB_Casino_Operators_TaxPayer")]
        public IHttpActionResult PB_Casino_Operators_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Casino Operators" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Casino_Operators_Asset")]
        public IHttpActionResult PB_Casino_Operators_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Casino Operators" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Casino_Operators_Profile")]
        public IHttpActionResult PB_Casino_Operators_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Casino Operators" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Casino_Operators_AssessmentRule")]
        public IHttpActionResult PB_Casino_Operators_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Casino Operators" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Casino_Operators_AssessmentItem")]
        public IHttpActionResult PB_Casino_Operators_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Casino Operators" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Casino_Operators_Assessment")]
        public IHttpActionResult PB_Casino_Operators_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Casino Operators" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Pools Betting - Pools Promoters

        [HttpGet]
        [Route("PB_Pools_Promoters_TaxPayer")]
        public IHttpActionResult PB_Pools_Promoters_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Pools Promoters" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Pools_Promoters_Asset")]
        public IHttpActionResult PB_Pools_Promoters_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Pools Promoters" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Pools_Promoters_Profile")]
        public IHttpActionResult PB_Pools_Promoters_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Pools Promoters" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Pools_Promoters_AssessmentRule")]
        public IHttpActionResult PB_Pools_Promoters_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Pools Promoters" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Pools_Promoters_AssessmentItem")]
        public IHttpActionResult PB_Pools_Promoters_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Pools Promoters" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Pools_Promoters_Assessment")]
        public IHttpActionResult PB_Pools_Promoters_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Pools Promoters" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Pools Betting - Gaming Machine Operators

        [HttpGet]
        [Route("PB_Gaming_Machine_Operators_TaxPayer")]
        public IHttpActionResult PB_Gaming_Machine_Operators_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Gaming Machine Operators" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Gaming_Machine_Operators_Asset")]
        public IHttpActionResult PB_Gaming_Machine_Operators_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Gaming Machine Operators" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Gaming_Machine_Operators_Profile")]
        public IHttpActionResult PB_Gaming_Machine_Operators_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Gaming Machine Operators" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Gaming_Machine_Operators_AssessmentRule")]
        public IHttpActionResult PB_Gaming_Machine_Operators_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Gaming Machine Operators" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Gaming_Machine_Operators_AssessmentItem")]
        public IHttpActionResult PB_Gaming_Machine_Operators_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Gaming Machine Operators" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Gaming_Machine_Operators_Assessment")]
        public IHttpActionResult PB_Gaming_Machine_Operators_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Gaming Machine Operators" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Pools Betting - Lottery Operators

        [HttpGet]
        [Route("PB_Lottery_Operators_TaxPayer")]
        public IHttpActionResult PB_Lottery_Operators_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lottery Operators" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Lottery_Operators_Asset")]
        public IHttpActionResult PB_Lottery_Operators_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lottery Operators" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Lottery_Operators_Profile")]
        public IHttpActionResult PB_Lottery_Operators_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lottery Operators" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Lottery_Operators_AssessmentRule")]
        public IHttpActionResult PB_Lottery_Operators_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lottery Operators" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Lottery_Operators_AssessmentItem")]
        public IHttpActionResult PB_Lottery_Operators_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lottery Operators" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Lottery_Operators_Assessment")]
        public IHttpActionResult PB_Lottery_Operators_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lottery Operators" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Pools Betting - Lotto Agents

        [HttpGet]
        [Route("PB_Lotto_Agents_TaxPayer")]
        public IHttpActionResult PB_Lotto_Agents_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lotto Agents" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Lotto_Agents_Asset")]
        public IHttpActionResult PB_Lotto_Agents_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lotto Agents" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Lotto_Agents_Profile")]
        public IHttpActionResult PB_Lotto_Agents_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lotto Agents" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Lotto_Agents_AssessmentRule")]
        public IHttpActionResult PB_Lotto_Agents_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lotto Agents" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Lotto_Agents_AssessmentItem")]
        public IHttpActionResult PB_Lotto_Agents_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lotto Agents" }); mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Lotto_Agents_Assessment")]
        public IHttpActionResult PB_Lotto_Agents_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lotto Agents" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Pools Betting - Lottery Agents

        [HttpGet]
        [Route("PB_Lottery_Agents_TaxPayer")]
        public IHttpActionResult PB_Lottery_Agents_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lottery Agents" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Lottery_Agents_Asset")]
        public IHttpActionResult PB_Lottery_Agents_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lottery Agents" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Lottery_Agents_Profile")]
        public IHttpActionResult PB_Lottery_Agents_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lottery Agents" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Lottery_Agents_AssessmentRule")]
        public IHttpActionResult PB_Lottery_Agents_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lottery Agents" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Lottery_Agents_AssessmentItem")]
        public IHttpActionResult PB_Lottery_Agents_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lottery Agents" }); mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Lottery_Agents_Assessment")]
        public IHttpActionResult PB_Lottery_Agents_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lottery Agents" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Pools Betting - Lotto Operators

        [HttpGet]
        [Route("PB_Lotto_Operators_TaxPayer")]
        public IHttpActionResult PB_Lotto_Operators_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lotto Operators" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Lotto_Operators_Asset")]
        public IHttpActionResult PB_Lotto_Operators_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lotto Operators" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Lotto_Operators_Profile")]
        public IHttpActionResult PB_Lotto_Operators_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lotto Operators" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Lotto_Operators_AssessmentRule")]
        public IHttpActionResult PB_Lotto_Operators_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lotto Operators" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Lotto_Operators_AssessmentItem")]
        public IHttpActionResult PB_Lotto_Operators_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lotto Operators" }); mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Lotto_Operators_Assessment")]
        public IHttpActionResult PB_Lotto_Operators_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Lotto Operators" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Pools Betting - Online Sports Betting Agents

        [HttpGet]
        [Route("PB_Online_Sports_Betting_Agents_TaxPayer")]
        public IHttpActionResult PB_Online_Sports_Betting_Agents_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Online Sports Betting Agents" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Online_Sports_Betting_Agents_Asset")]
        public IHttpActionResult PB_Online_Sports_Betting_Agents_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Online Sports Betting Agents" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Online_Sports_Betting_Agents_Profile")]
        public IHttpActionResult PB_Online_Sports_Betting_Agents_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Online Sports Betting Agents" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Online_Sports_Betting_Agents_AssessmentRule")]
        public IHttpActionResult PB_Online_Sports_Betting_Agents_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Online Sports Betting Agents" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Online_Sports_Betting_Agents_AssessmentItem")]
        public IHttpActionResult PB_Online_Sports_Betting_Agents_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Online Sports Betting Agents" }); mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Online_Sports_Betting_Agents_Assessment")]
        public IHttpActionResult PB_Online_Sports_Betting_Agents_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Online Sports Betting Agents" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Pools Betting - Online Sports Betting Operators

        [HttpGet]
        [Route("PB_Online_Sports_Betting_Operators_TaxPayer")]
        public IHttpActionResult PB_Online_Sports_Betting_Operators_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Online Sports Betting Operators" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Online_Sports_Betting_Operators_Asset")]
        public IHttpActionResult PB_Online_Sports_Betting_Operators_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Online Sports Betting Operators" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Online_Sports_Betting_Operators_Profile")]
        public IHttpActionResult PB_Online_Sports_Betting_Operators_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Online Sports Betting Operators" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Online_Sports_Betting_Operators_AssessmentRule")]
        public IHttpActionResult PB_Online_Sports_Betting_Operators_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Online Sports Betting Operators" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Online_Sports_Betting_Operators_AssessmentItem")]
        public IHttpActionResult PB_Online_Sports_Betting_Operators_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Online Sports Betting Operators" }); mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Online_Sports_Betting_Operators_Assessment")]
        public IHttpActionResult PB_Online_Sports_Betting_Operators_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Online Sports Betting Operators" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Pools Betting - Pools Agents

        [HttpGet]
        [Route("PB_Pools_Agents_TaxPayer")]
        public IHttpActionResult PB_Pools_Agents_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Pools Agents" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Pools_Agents_Asset")]
        public IHttpActionResult PB_Pools_Agents_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Pools Agents" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Pools_Agents_Profile")]
        public IHttpActionResult PB_Pools_Agents_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Pools Agents" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Pools_Agents_AssessmentRule")]
        public IHttpActionResult PB_Pools_Agents_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Pools Agents" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Pools_Agents_AssessmentItem")]
        public IHttpActionResult PB_Pools_Agents_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Pools Agents" }); mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PB_Pools_Agents_Assessment")]
        public IHttpActionResult PB_Pools_Agents_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Pools Betting - Pools Agents" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Direct Assessment - Formal Business - Self Employed

        [HttpGet]
        [Route("DA_Formal_Business_Self_Employed_TaxPayer")]
        public IHttpActionResult DA_Formal_Business_Self_Employed_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Direct Assessment - Formal Business - Self Employed" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("DA_Formal_Business_Self_Employed_Asset")]
        public IHttpActionResult DA_Formal_Business_Self_Employed_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Direct Assessment - Formal Business - Self Employed" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("DA_Formal_Business_Self_Employed_Profile")]
        public IHttpActionResult DA_Formal_Business_Self_Employed_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Direct Assessment - Formal Business - Self Employed" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("DA_Formal_Business_Self_Employed_AssessmentRule")]
        public IHttpActionResult DA_Formal_Business_Self_Employed_AssessmentRule([FromUri] NewPagingParameterModelII pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                Profile profile = new Profile() { IntSearchType = 1, ProfileDescription = "Direct Assessment - Formal Business - Self Employed" };
                if (pobjPagingModel.year > 0)
                {
                    IList<usp_GetAssessmentRuleBasedOnProfileForSupplierNew_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplierNew(profile, pobjPagingModel.year);
                    mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
                }
                else
                {
                    IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleDataII = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(profile);
                    mObjAPIResponse = PaginationList(lstAssessmentRuleDataII, pobjPagingModel);
                }
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("DA_Formal_Business_Self_Employed_AssessmentItem")]
        public IHttpActionResult DA_Formal_Business_Self_Employed_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Direct Assessment - Formal Business - Self Employed" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("DA_Formal_Business_Self_Employed_Assessment")]
        public IHttpActionResult DA_Formal_Business_Self_Employed_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Direct Assessment - Formal Business - Self Employed" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Direct Assessment for Multi Employees Business Owners

        [HttpGet]
        [Route("DA_Mutli_Employees_Business_Owners_TaxPayer")]
        public IHttpActionResult DA_Mutli_Employees_Business_Owners_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Direct Assessment for Multi Employees Business Owners" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("DA_Mutli_Employees_Business_Owners_Asset")]
        public IHttpActionResult DA_Mutli_Employees_Business_Owners_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Direct Assessment for Multi Employees Business Owners" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("DA_Mutli_Employees_Business_Owners_Profile")]
        public IHttpActionResult DA_Mutli_Employees_Business_Owners_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Direct Assessment for Multi Employees Business Owners" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("DA_Mutli_Employees_Business_Owners_AssessmentRule")]
        public IHttpActionResult DA_Mutli_Employees_Business_Owners_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Direct Assessment for Multi Employees Business Owners" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }  
        [HttpGet]
        [Route("DA_Mutli_Employees_Business_Owners_AssessmentRule_New")]
        public IHttpActionResult DA_Mutli_Employees_Business_Owners_AssessmentRule_New([FromUri] NewPagingParameterModelII pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplierNew_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplierNew(new Profile() { IntSearchType = 1, ProfileDescription = "Direct Assessment for Multi Employees Business Owners" }, pobjPagingModel.year);
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("DA_Mutli_Employees_Business_Owners_AssessmentItem")]
        public IHttpActionResult DA_Mutli_Employees_Business_Owners_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Direct Assessment for Multi Employees Business Owners" }); mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("DA_Mutli_Employees_Business_Owners_Assessment")]
        public IHttpActionResult DA_Mutli_Employees_Business_Owners_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 1, ProfileDescription = "Direct Assessment for Multi Employees Business Owners" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Vehicle Licenses - Commercial

        [HttpGet]
        [Route("Vehicle_Licenses_Commercial_TaxPayer")]
        public IHttpActionResult Vehicle_Licenses_Commercial_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 2, ProfileDescription = "Vehicle Licenses", VehiclePurposeID = 1 });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Vehicle_Licenses_Commercial_Asset")]
        public IHttpActionResult Vehicle_Licenses_Commercial_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetVehicleBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetVehicleBasedOnProfileForSupplier(new Profile() { IntSearchType = 2, ProfileDescription = "Vehicle Licenses", VehiclePurposeID = 1 });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Vehicle_Licenses_Commercial_Profile")]
        public IHttpActionResult Vehicle_Licenses_Commercial_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 2, ProfileDescription = "Vehicle Licenses", VehiclePurposeID = 1 });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Vehicle_Licenses_Commercial_AssessmentRule")]
        public IHttpActionResult Vehicle_Licenses_Commercial_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 2, ProfileDescription = "Vehicle Licenses", VehiclePurposeID = 1 });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Vehicle_Licenses_Commercial_AssessmentItem")]
        public IHttpActionResult Vehicle_Licenses_Commercial_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 2, ProfileDescription = "Vehicle Licenses", VehiclePurposeID = 1 }); mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Vehicle_Licenses_Commercial_Assessment")]
        public IHttpActionResult Vehicle_Licenses_Commercial_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 2, ProfileDescription = "Vehicle Licenses", VehiclePurposeID = 1 }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Vehicle Licenses - Private

        [HttpGet]
        [Route("Vehicle_Licenses_Private_TaxPayer")]
        public IHttpActionResult Vehicle_Licenses_Private_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 2, ProfileDescription = "Vehicle Licenses", VehiclePurposeID = 2 });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Vehicle_Licenses_Private_Asset")]
        public IHttpActionResult Vehicle_Licenses_Private_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetVehicleBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetVehicleBasedOnProfileForSupplier(new Profile() { IntSearchType = 2, ProfileDescription = "Vehicle Licenses", VehiclePurposeID = 2 });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Vehicle_Licenses_Private_Profile")]
        public IHttpActionResult Vehicle_Licenses_Private_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 2, ProfileDescription = "Vehicle Licenses", VehiclePurposeID = 2 });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Vehicle_Licenses_Private_AssessmentRule")]
        public IHttpActionResult Vehicle_Licenses_Private_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 2, ProfileDescription = "Vehicle Licenses", VehiclePurposeID = 2 });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Vehicle_Licenses_Private_AssessmentItem")]
        public IHttpActionResult Vehicle_Licenses_Private_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 2, ProfileDescription = "Vehicle Licenses", VehiclePurposeID = 2 }); mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Vehicle_Licenses_Private_Assessment")]
        public IHttpActionResult Vehicle_Licenses_Private_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 2, ProfileDescription = "Vehicle Licenses", VehiclePurposeID = 2 }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Pay As You Earn - PAYE Collections - Multiple Employees and Business Sector A - F

        [HttpGet]
        [Route("PAYE_Collection_Multiple_Employees_BS_A_F_TaxPayer")]
        public IHttpActionResult PAYE_Collection_Multiple_Employees_BS_A_F_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();
            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Collections - Multiple Employees", BusinessSector = "A,B,C,D,E,F" });
                var newtTaxPayerData = lstTaxPayerData.GroupBy(p => new { p.TaxPayerID, p.TaxPayerRIN }).Select(g => g.First()).ToList();

                // var gtVmList = Getret(newtTaxPayerData, 1);

                mObjAPIResponse = PaginationList(newtTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Collection_Multiple_Employees_BS_A_F_Asset")]
        public IHttpActionResult PAYE_Collection_Multiple_Employees_BS_A_F_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Collections - Multiple Employees", BusinessSector = "A,B,C,D,E,F" });
                // var newtTaxPayerData = lstTaxPayerAssetData.GroupBy(p => new { p.TaxPayerID, p.TaxPayerRIN }).Select(g => g.First()).ToList();
                var number = lstTaxPayerAssetData.Count();
                //var gtVmList = Getret(newtTaxPayerData, 1);

                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Collection_Multiple_Employees_BS_A_F_Profile")]
        public IHttpActionResult PAYE_Collection_Multiple_Employees_BS_A_F_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Collections - Multiple Employees", BusinessSector = "A,B,C,D,E,F" });
                var newtTaxPayerData = lstProfileData.GroupBy(p => new { p.ProfileID }).Select(g => g.First()).ToList();

                var gtVmList = Getret(newtTaxPayerData, 1);

                mObjAPIResponse = PaginationList(gtVmList, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_General_Rules")]
        public IHttpActionResult PAYE_General_Rules()
        {
            APIResponse mObjAPIResponse = new APIResponse();
            List<Returner> lstAssessmentRuleData = new List<Returner>();
            Returner returner = new Returner();
            try
            {
                using (_db = new EIRSEntities())
                {
                    var retVal = _db.Assessment_Rules.Where(o => o.ProfileID == 1277).ToList();
                    foreach (var re in retVal)
                    {
                        var cos = new Returner()
                        {
                            AssessmentRuleID = re.AssessmentRuleID,
                            AssessmentRuleName = re.AssessmentRuleName,
                            AssessmentRuleCode = re.AssessmentRuleCode,
                            AssessmentAmount = re.AssessmentAmount.Value,
                            Active = re.Active.Value,
                            ProfileID = re.ProfileID.Value,
                            RuleRunID = re.RuleRunID.Value,
                            PaymentOptionID = re.PaymentOptionID.Value,
                            TaxYear = re.TaxYear.Value,
                            PaymentFrequencyID = re.PaymentFrequencyID.Value,
                            TaxMonth = re.TaxMonth.Value,
                        };

                        lstAssessmentRuleData.Add(cos);
                    }
                }
                mObjAPIResponse.Success = true;
                mObjAPIResponse.Message = "Assessment Rule Pulled Successfully";
                mObjAPIResponse.Result = lstAssessmentRuleData;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Collection_Multiple_Employees_BS_A_F_AssessmentRule")]
        public IHttpActionResult PAYE_Collection_Multiple_Employees_BS_A_F_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Collections - Multiple Employees", BusinessSector = "A,B,C,D,E,F" });
                //var newtTaxPayerData = lstAssessmentRuleData.GroupBy(p => new { p.TaxPayerID, p.TaxPayerRIN }).Select(g => g.First()).ToList();

                var gtVmList = Getret(lstAssessmentRuleData, 1);

                mObjAPIResponse = PaginationList(gtVmList, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Collection_Multiple_Employees_BS_A_F_AssessmentItem")]
        public IHttpActionResult PAYE_Collection_Multiple_Employees_BS_A_F_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Collections - Multiple Employees", BusinessSector = "A,B,C,D,E,F" });
                var newtTaxPayerData = lstAssessmentItemData.GroupBy(p => new { p.TaxPayerID, p.TaxPayerRIN }).Select(g => g.First()).ToList();

                //var gtVmList = Getret(newtTaxPayerData, 1);

                mObjAPIResponse = PaginationList(newtTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Collection_Multiple_Employees_BS_A_F_Assessment")]
        public IHttpActionResult PAYE_Collection_Multiple_Employees_BS_A_F_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Collections - Multiple Employees", BusinessSector = "A,B,C,D,E,F" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Pay As You Earn - PAYE Collections - Multiple Employees and Business Sector G

        [HttpGet]
        [Route("PAYE_Collection_Multiple_Employees_BS_G_TaxPayer")]
        public IHttpActionResult PAYE_Collection_Multiple_Employees_BS_G_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Collections - Multiple Employees", BusinessSector = "G" });

                var newtTaxPayerData = lstTaxPayerData.GroupBy(p => new { p.TaxPayerID, p.TaxPayerRIN }).Select(g => g.First()).ToList();

                //var gvList = Getret(newtTaxPayerData, 2);

                mObjAPIResponse = PaginationList(newtTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Collection_Multiple_Employees_BS_G_Asset")]
        public IHttpActionResult PAYE_Collection_Multiple_Employees_BS_G_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Collections - Multiple Employees", BusinessSector = "G" });
                var newtTaxPayerData = lstTaxPayerAssetData.GroupBy(p => new { p.TaxPayerID, p.TaxPayerRIN }).Select(g => g.First()).ToList();

                //var gtVmList = Getret(newtTaxPayerData, 2);

                mObjAPIResponse = PaginationList(newtTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Collection_Multiple_Employees_BS_G_Profile")]
        public IHttpActionResult PAYE_Collection_Multiple_Employees_BS_G_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Collections - Multiple Employees", BusinessSector = "G" });
                var newtTaxPayerData = lstProfileData.GroupBy(p => new { p.TaxPayerID, p.TaxPayerRIN }).Select(g => g.First()).ToList();

                var gtVmList = Getret(newtTaxPayerData, 2);

                mObjAPIResponse = PaginationList(gtVmList, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Collection_Multiple_Employees_BS_G_AssessmentRule")]
        public IHttpActionResult PAYE_Collection_Multiple_Employees_BS_G_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Collections - Multiple Employees", BusinessSector = "G" });
                var newtTaxPayerData = lstAssessmentRuleData.GroupBy(p => new { p.TaxPayerID, p.TaxPayerRIN }).Select(g => g.First()).ToList();

                var gtVmList = Getret(newtTaxPayerData, 2);

                mObjAPIResponse = PaginationList(gtVmList, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Collection_Multiple_Employees_BS_G_AssessmentItem")]
        public IHttpActionResult PAYE_Collection_Multiple_Employees_BS_G_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Collections - Multiple Employees", BusinessSector = "G" });
                var newtTaxPayerData = lstAssessmentItemData.GroupBy(p => new { p.TaxPayerID, p.TaxPayerRIN }).Select(g => g.First()).ToList();

                // var gtVmList = Getret(newtTaxPayerData, 2);

                mObjAPIResponse = PaginationList(newtTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Collection_Multiple_Employees_BS_G_Assessment")]
        public IHttpActionResult PAYE_Collection_Multiple_Employees_BS_G_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Collections - Multiple Employees", BusinessSector = "G" });
                mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Pay As You Earn - PAYE Collections - Multiple Employees and Business Sector H - Z

        [HttpGet]
        [Route("PAYE_Collection_Multiple_Employees_BS_H_Z_TaxPayer")]
        public IHttpActionResult PAYE_Collection_Multiple_Employees_BS_H_Z_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Collections - Multiple Employees", BusinessSector = "H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z" });
                var newtTaxPayerData = lstTaxPayerData.GroupBy(p => new { p.TaxPayerID, p.TaxPayerRIN }).Select(g => g.First()).ToList();
                //var ret = Getret(newtTaxPayerData, 3);
                mObjAPIResponse = PaginationList(newtTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Collection_Multiple_Employees_BS_H_Z_Asset")]
        public IHttpActionResult PAYE_Collection_Multiple_Employees_BS_H_Z_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Collections - Multiple Employees", BusinessSector = "H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z" });
              //  var newtTaxPayerData = lstTaxPayerAssetData.GroupBy(p => new { p.TaxPayerID, p.TaxPayerRIN }).Select(g => g.First()).ToList();

                //var gtVmList = Getret(newtTaxPayerData, 3);

                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Collection_Multiple_Employees_BS_H_Z_Profile")]
        public IHttpActionResult PAYE_Collection_Multiple_Employees_BS_H_Z_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Collections - Multiple Employees", BusinessSector = "H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z" });
                var newtTaxPayerData = lstProfileData.GroupBy(p => new { p.TaxPayerID, p.TaxPayerRIN }).Select(g => g.First()).ToList();

                var gtVmList = Getret(newtTaxPayerData, 3);

                mObjAPIResponse = PaginationList(gtVmList, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Collection_Multiple_Employees_BS_H_Z_AssessmentRule")]
        public IHttpActionResult PAYE_Collection_Multiple_Employees_BS_H_Z_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Collections - Multiple Employees", BusinessSector = "H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z" });
                var newtTaxPayerData = lstAssessmentRuleData.GroupBy(p => new { p.TaxPayerID, p.TaxPayerRIN }).Select(g => g.First()).ToList();

                var gtVmList = Getret(newtTaxPayerData, 3);

                mObjAPIResponse = PaginationList(gtVmList, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Collection_Multiple_Employees_BS_H_Z_AssessmentItem")]
        public IHttpActionResult PAYE_Collection_Multiple_Employees_BS_H_Z_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Collections - Multiple Employees", BusinessSector = "H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z" });
                var newtTaxPayerData = lstAssessmentItemData.GroupBy(p => new { p.TaxPayerID, p.TaxPayerRIN }).Select(g => g.First()).ToList();

                // var gtVmList = Getret(newtTaxPayerData, 3);

                mObjAPIResponse = PaginationList(newtTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Collection_Multiple_Employees_BS_H_Z_Assessment")]
        public IHttpActionResult PAYE_Collection_Multiple_Employees_BS_H_Z_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Collections - Multiple Employees", BusinessSector = "H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Pay As You Earn - PAYE Contribution - Formal Business Employee and Business Sector A - F

        [HttpGet]
        [Route("PAYE_Contribution_Formal_Business_Employee_BS_A_F_TaxPayer")]
        public IHttpActionResult PAYE_Contribution_Formal_Business_Employee_BS_A_F_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Contribution - Formal Business Employee", BusinessSector = "A,B,C,D,E,F" });
                var newtTaxPayerData = lstTaxPayerData.GroupBy(p => new { p.TaxPayerID, p.TaxPayerRIN }).Select(g => g.First()).ToList();

                var gtVmList = Getret(newtTaxPayerData, 1);

                mObjAPIResponse = PaginationList(gtVmList, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Contribution_Formal_Business_Employee_BS_A_F_Asset")]
        public IHttpActionResult PAYE_Contribution_Formal_Business_Employee_BS_A_F_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Contribution - Formal Business Employee", BusinessSector = "A,B,C,D,E,F" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Contribution_Formal_Business_Employee_BS_A_F_Profile")]
        public IHttpActionResult PAYE_Contribution_Formal_Business_Employee_BS_A_F_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Contribution - Formal Business Employee", BusinessSector = "A,B,C,D,E,F" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Contribution_Formal_Business_Employee_BS_A_F_AssessmentRule")]
        public IHttpActionResult PAYE_Contribution_Formal_Business_Employee_BS_A_F_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Contribution - Formal Business Employee", BusinessSector = "A,B,C,D,E,F" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Contribution_Formal_Business_Employee_BS_A_F_AssessmentItem")]
        public IHttpActionResult PAYE_Contribution_Formal_Business_Employee_BS_A_F_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Contribution - Formal Business Employee", BusinessSector = "A,B,C,D,E,F" }); mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Contribution_Formal_Business_Employee_BS_A_F_Assessment")]
        public IHttpActionResult PAYE_Contribution_Formal_Business_Employee_BS_A_F_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Contribution - Formal Business Employee", BusinessSector = "A,B,C,D,E,F" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Pay As You Earn - PAYE Contribution - Formal Business Employee and Business Sector G

        [HttpGet]
        [Route("PAYE_Contribution_Formal_Business_Employee_BS_G_TaxPayer")]
        public IHttpActionResult PAYE_Contribution_Formal_Business_Employee_BS_G_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Contribution - Formal Business Employee", BusinessSector = "G" });
                var newtTaxPayerData = lstTaxPayerData.GroupBy(p => new { p.TaxPayerID, p.TaxPayerRIN }).Select(g => g.First()).ToList();

                var gtVmList = Getret(newtTaxPayerData, 2);

                mObjAPIResponse = PaginationList(gtVmList, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Contribution_Formal_Business_Employee_BS_G_Asset")]
        public IHttpActionResult PAYE_Contribution_Formal_Business_Employee_BS_G_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Contribution - Formal Business Employee", BusinessSector = "G" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Contribution_Formal_Business_Employee_BS_G_Profile")]
        public IHttpActionResult PAYE_Contribution_Formal_Business_Employee_BS_G_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Contribution - Formal Business Employee", BusinessSector = "G" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Contribution_Formal_Business_Employee_BS_G_AssessmentRule")]
        public IHttpActionResult PAYE_Contribution_Formal_Business_Employee_BS_G_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Contribution - Formal Business Employee", BusinessSector = "G" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Contribution_Formal_Business_Employee_BS_G_AssessmentItem")]
        public IHttpActionResult PAYE_Contribution_Formal_Business_Employee_BS_G_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Contribution - Formal Business Employee", BusinessSector = "G" }); mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Contribution_Formal_Business_Employee_BS_G_Assessment")]
        public IHttpActionResult PAYE_Contribution_Formal_Business_Employee_BS_G_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Contribution - Formal Business Employee", BusinessSector = "G" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Pay As You Earn - PAYE Contribution - Formal Business Employee and Business Sector H - Z

        [HttpGet]
        [Route("PAYE_Contribution_Formal_Business_Employee_BS_H_Z_TaxPayer")]
        public IHttpActionResult PAYE_Contribution_Formal_Business_Employee_BS_H_Z_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Contribution - Formal Business Employee", BusinessSector = "H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z" });
                var newtTaxPayerData = lstTaxPayerData.GroupBy(p => new { p.TaxPayerID, p.TaxPayerRIN }).Select(g => g.First()).ToList();

                var gtVmList = Getret(newtTaxPayerData, 3);

                mObjAPIResponse = PaginationList(gtVmList, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Contribution_Formal_Business_Employee_BS_H_Z_Asset")]
        public IHttpActionResult PAYE_Contribution_Formal_Business_Employee_BS_H_Z_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Contribution - Formal Business Employee", BusinessSector = "H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Contribution_Formal_Business_Employee_BS_H_Z_Profile")]
        public IHttpActionResult PAYE_Contribution_Formal_Business_Employee_BS_H_Z_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Contribution - Formal Business Employee", BusinessSector = "H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Contribution_Formal_Business_Employee_BS_H_Z_AssessmentRule")]
        public IHttpActionResult PAYE_Contribution_Formal_Business_Employee_BS_H_Z_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Contribution - Formal Business Employee", BusinessSector = "H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Contribution_Formal_Business_Employee_BS_H_Z_AssessmentItem")]
        public IHttpActionResult PAYE_Contribution_Formal_Business_Employee_BS_H_Z_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Contribution - Formal Business Employee", BusinessSector = "H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z" }); mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("PAYE_Contribution_Formal_Business_Employee_BS_H_Z_Assessment")]
        public IHttpActionResult PAYE_Contribution_Formal_Business_Employee_BS_H_Z_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Pay As You Earn - PAYE Contribution - Formal Business Employee", BusinessSector = "H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Business Premises and Business Sector A - I

        [HttpGet]
        [Route("Business_Premises_BS_A_I_TaxPayer")]
        public IHttpActionResult Business_Premises_BS_A_I_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Business Premises", BusinessSector = "A,B,C,D,E,F,G,H,I" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Business_Premises_BS_A_I_Asset")]
        public IHttpActionResult Business_Premises_BS_A_I_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Business Premises", BusinessSector = "A,B,C,D,E,F,G,H,I" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Business_Premises_BS_A_I_Profile")]
        public IHttpActionResult Business_Premises_BS_A_I_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Business Premises", BusinessSector = "A,B,C,D,E,F,G,H,I" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Business_Premises_BS_A_I_AssessmentRule")]
        public IHttpActionResult Business_Premises_BS_A_I_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Business Premises", BusinessSector = "A,B,C,D,E,F,G,H,I" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Business_Premises_BS_A_I_AssessmentItem")]
        public IHttpActionResult Business_Premises_BS_A_I_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Business Premises", BusinessSector = "A,B,C,D,E,F,G,H,I" }); mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Business_Premises_BS_A_I_Assessment")]
        public IHttpActionResult Business_Premises_BS_A_I_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Business Premises", BusinessSector = "A,B,C,D,E,F,G,H,I" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Business Premises and Business Sector J - R

        [HttpGet]
        [Route("Business_Premises_BS_J_R_TaxPayer")]
        public IHttpActionResult Business_Premises_BS_J_R_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Business Premises", BusinessSector = "J,K,L,M,N,O,P,Q,R" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Business_Premises_BS_J_R_Asset")]
        public IHttpActionResult Business_Premises_BS_J_R_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Business Premises", BusinessSector = "J,K,L,M,N,O,P,Q,R" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Business_Premises_BS_J_R_Profile")]
        public IHttpActionResult Business_Premises_BS_J_R_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Business Premises", BusinessSector = "J,K,L,M,N,O,P,Q,R" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Business_Premises_BS_J_R_AssessmentRule")]
        public IHttpActionResult Business_Premises_BS_J_R_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Business Premises", BusinessSector = "J,K,L,M,N,O,P,Q,R" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Business_Premises_BS_J_R_AssessmentItem")]
        public IHttpActionResult Business_Premises_BS_J_R_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Business Premises", BusinessSector = "J,K,L,M,N,O,P,Q,R" }); mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Business_Premises_BS_J_R_Assessment")]
        public IHttpActionResult Business_Premises_BS_J_R_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Business Premises", BusinessSector = "J,K,L,M,N,O,P,Q,R" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Business Premises and Business Sector S - Z

        [HttpGet]
        [Route("Business_Premises_BS_S_Z_TaxPayer")]
        public IHttpActionResult Business_Premises_BS_S_Z_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Business Premises", BusinessSector = "S,T,U,V,W,X,Y,Z" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Business_Premises_BS_S_Z_Asset")]
        public IHttpActionResult Business_Premises_BS_S_Z_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Business Premises", BusinessSector = "S,T,U,V,W,X,Y,Z" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Business_Premises_BS_S_Z_Profile")]
        public IHttpActionResult Business_Premises_BS_S_Z_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Business Premises", BusinessSector = "S,T,U,V,W,X,Y,Z" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Business_Premises_BS_S_Z_AssessmentRule")]
        public IHttpActionResult Business_Premises_BS_S_Z_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Business Premises", BusinessSector = "S,T,U,V,W,X,Y,Z" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Business_Premises_BS_S_Z_AssessmentItem")]
        public IHttpActionResult Business_Premises_BS_S_Z_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Business Premises", BusinessSector = "S,T,U,V,W,X,Y,Z" }); mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Business_Premises_BS_S_Z_Assessment")]
        public IHttpActionResult Business_Premises_BS_S_Z_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 3, ProfileDescription = "Business Premises", BusinessSector = "S,T,U,V,W,X,Y,Z" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Presumptive Taxes

        [HttpGet]
        [Route("Presumptive_Taxes_TaxPayer")]
        public IHttpActionResult Presumptive_Taxes_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 5, ProfileDescription = "Presumptive Taxes" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Presumptive_Taxes_Asset")]
        public IHttpActionResult Presumptive_Taxes_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 5, ProfileDescription = "Presumptive Taxes" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Presumptive_Taxes_Profile")]
        public IHttpActionResult Presumptive_Taxes_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 5, ProfileDescription = "Presumptive Taxes" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Presumptive_Taxes_AssessmentRule")]
        public IHttpActionResult Presumptive_Taxes_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 5, ProfileDescription = "Presumptive Taxes" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Presumptive_Taxes_AssessmentItem")]
        public IHttpActionResult Presumptive_Taxes_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 5, ProfileDescription = "Presumptive Taxes" }); mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Presumptive_Taxes_Assessment")]
        public IHttpActionResult Presumptive_Taxes_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 5, ProfileDescription = "Presumptive Taxes" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Business Category - Mobile Location

        [HttpGet]
        [Route("BC_Mobile_Location_TaxPayer")]
        public IHttpActionResult BC_Mobile_Location_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 4, BusinessCategory = "Mobile Location" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("BC_Mobile_Location_Asset")]
        public IHttpActionResult BC_Mobile_Location_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 4, BusinessCategory = "Mobile Location" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("BC_Mobile_Location_Profile")]
        public IHttpActionResult BC_Mobile_Location_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 4, BusinessCategory = "Mobile Location" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("BC_Mobile_Location_AssessmentRule")]
        public IHttpActionResult BC_Mobile_Location_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 4, BusinessCategory = "Mobile Location" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("BC_Mobile_Location_AssessmentItem")]
        public IHttpActionResult BC_Mobile_Location_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 4, BusinessCategory = "Mobile Location" }); mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("BC_Mobile_Location_Assessment")]
        public IHttpActionResult BC_Mobile_Location_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 4, BusinessCategory = "Mobile Location" }); mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Vehicle Insurance Verification

        [HttpGet]
        [Route("Vehicle_Insurance_Verification_Services")]
        public IHttpActionResult Vehicle_Insurance_Verification_Services([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetVehicleInsuranceVerificationMDAServiceForSupplier_Result> lstMDAServiceData = new BLMDAService().BL_GetVehicleInsuranceVerificationMDAServiceForSupplier();
                mObjAPIResponse = PaginationList(lstMDAServiceData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Vehicle_Insurance_Verification_Services_Items")]
        public IHttpActionResult Vehicle_Insurance_Verification_Services_Items([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetVehicleInsuranceVerificationMDAServiceItemForSupplier_Result> lstMDAServiceItemData = new BLMDAServiceItem().BL_GetVehicleInsuranceVerificationMDAServiceItemForSupplier();
                mObjAPIResponse = PaginationList(lstMDAServiceItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Vehicle_Insurance_Verification_ServicesBill")]
        public IHttpActionResult Vehicle_Insurance_Verification_ServicesBill([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetVehicleInsuranceVerificationServiceBillForSupplier_Result> lstServiceBill = new BLServiceBill().BL_GetVehicleInsuranceVerificationServiceBillForSupplier();
                mObjAPIResponse = PaginationList(lstServiceBill, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Vehicle Licenses

        [HttpGet]
        [Route("Vehicle_License_Services")]
        public IHttpActionResult Vehicle_License_Services([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetVehicleLicenseMDAServiceForSupplier_Result> lstMDAServiceData = new BLMDAService().BL_GetVehicleLicenseMDAServiceForSupplier();
                mObjAPIResponse = PaginationList(lstMDAServiceData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Vehicle_License_Services_Items")]
        public IHttpActionResult Vehicle_License_Services_Items([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetVehicleLicenseMDAServiceItemForSupplier_Result> lstMDAServiceItemData = new BLMDAServiceItem().BL_GetVehicleLicenseMDAServiceItemForSupplier();
                mObjAPIResponse = PaginationList(lstMDAServiceItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Vehicle_License_ServicesBill")]
        public IHttpActionResult Vehicle_License_ServicesBill([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetVehicleLicenseServiceBillForSupplier_Result> lstServiceBill = new BLServiceBill().BL_GetVehicleLicenseServiceBillForSupplier();
                mObjAPIResponse = PaginationList(lstServiceBill, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region POA API

        [HttpGet]
        [Route("POA_Business_Premises")]
        public IHttpActionResult POA_Business_Premises([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetPaymentAccountList_Result> lstPOAData = new BLPaymentAccount().BL_GetPaymentAccountList(new Payment_Account() { RevenueStreamID = (int)EnumList.ReveneueStream.Business_Premises });
                mObjAPIResponse = PaginationList(lstPOAData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("POA_Capital_Gains_Taxes")]
        public IHttpActionResult POA_Capital_Gains_Taxes([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetPaymentAccountList_Result> lstPOAData = new BLPaymentAccount().BL_GetPaymentAccountList(new Payment_Account() { RevenueStreamID = (int)EnumList.ReveneueStream.Capital_Gains_Taxes });
                mObjAPIResponse = PaginationList(lstPOAData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("POA_Consumption_Taxes")]
        public IHttpActionResult POA_Consumption_Taxes([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetPaymentAccountList_Result> lstPOAData = new BLPaymentAccount().BL_GetPaymentAccountList(new Payment_Account() { RevenueStreamID = (int)EnumList.ReveneueStream.Consumption_Taxes });
                mObjAPIResponse = PaginationList(lstPOAData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("POA_Development_Levy")]
        public IHttpActionResult POA_Development_Levy([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetPaymentAccountList_Result> lstPOAData = new BLPaymentAccount().BL_GetPaymentAccountList(new Payment_Account() { RevenueStreamID = (int)EnumList.ReveneueStream.Development_Levy });
                mObjAPIResponse = PaginationList(lstPOAData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("POA_Direct_Assessment")]
        public IHttpActionResult POA_Direct_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetPaymentAccountList_Result> lstPOAData = new BLPaymentAccount().BL_GetPaymentAccountList(new Payment_Account() { RevenueStreamID = (int)EnumList.ReveneueStream.Direct_Assessment });
                mObjAPIResponse = PaginationList(lstPOAData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("POA_Land_Use_Charge")]
        public IHttpActionResult POA_Land_Use_Charge([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetPaymentAccountList_Result> lstPOAData = new BLPaymentAccount().BL_GetPaymentAccountList(new Payment_Account() { RevenueStreamID = (int)EnumList.ReveneueStream.Land_Use_Charge });
                mObjAPIResponse = PaginationList(lstPOAData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("POA_Lottery_Gaming_Regulation")]
        public IHttpActionResult POA_Lottery_Gaming_Regulation([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetPaymentAccountList_Result> lstPOAData = new BLPaymentAccount().BL_GetPaymentAccountList(new Payment_Account() { RevenueStreamID = (int)EnumList.ReveneueStream.Lottery_Gaming_Regulation });
                mObjAPIResponse = PaginationList(lstPOAData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("POA_MDA_Services")]
        public IHttpActionResult POA_MDA_Services([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetPaymentAccountList_Result> lstPOAData = new BLPaymentAccount().BL_GetPaymentAccountList(new Payment_Account() { RevenueStreamID = (int)EnumList.ReveneueStream.MDA_Services });
                mObjAPIResponse = PaginationList(lstPOAData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("POA_Pay_As_You_Earn")]
        public IHttpActionResult POA_Pay_As_You_Earn([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetPaymentAccountList_Result> lstPOAData = new BLPaymentAccount().BL_GetPaymentAccountList(new Payment_Account() { RevenueStreamID = (int)EnumList.ReveneueStream.Pay_As_You_Earn });
                mObjAPIResponse = PaginationList(lstPOAData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("POA_Stamp_Duty")]
        public IHttpActionResult POA_Stamp_Duty([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetPaymentAccountList_Result> lstPOAData = new BLPaymentAccount().BL_GetPaymentAccountList(new Payment_Account() { RevenueStreamID = (int)EnumList.ReveneueStream.Stamp_Duty });
                mObjAPIResponse = PaginationList(lstPOAData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("POA_State_Government")]
        public IHttpActionResult POA_State_Government([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetPaymentAccountList_Result> lstPOAData = new BLPaymentAccount().BL_GetPaymentAccountList(new Payment_Account() { RevenueStreamID = (int)EnumList.ReveneueStream.State_Government });
                mObjAPIResponse = PaginationList(lstPOAData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("POA_Vehicle_Licenses")]
        public IHttpActionResult POA_Vehicle_Licenses([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetPaymentAccountList_Result> lstPOAData = new BLPaymentAccount().BL_GetPaymentAccountList(new Payment_Account() { RevenueStreamID = (int)EnumList.ReveneueStream.Vehicle_License });
                mObjAPIResponse = PaginationList(lstPOAData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("POA_Waste_Management")]
        public IHttpActionResult POA_Waste_Management([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetPaymentAccountList_Result> lstPOAData = new BLPaymentAccount().BL_GetPaymentAccountList(new Payment_Account() { RevenueStreamID = (int)EnumList.ReveneueStream.Waste_Management });
                mObjAPIResponse = PaginationList(lstPOAData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("POA_Withholding_Taxes")]
        public IHttpActionResult POA_Withholding_Taxes([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetPaymentAccountList_Result> lstPOAData = new BLPaymentAccount().BL_GetPaymentAccountList(new Payment_Account() { RevenueStreamID = (int)EnumList.ReveneueStream.Withholding_Taxes });
                mObjAPIResponse = PaginationList(lstPOAData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region MAS Price Sheet

        [HttpGet]
        [Route("MAS_PS_Presumptive_Tax")]
        public IHttpActionResult MAS_PS_Presumptive_Tax()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetMASPriceSheet_Result> lstMASPriceSheet = new BLAssessmentRule().BL_GetMASPriceSheet(1);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstMASPriceSheet;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("MAS_PS_Vehicle_License_Renewal")]
        public IHttpActionResult MAS_PS_Vehicle_License_Renewal()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetMASPriceSheet_Result> lstMASPriceSheet = new BLAssessmentRule().BL_GetMASPriceSheet(2);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstMASPriceSheet;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("MAS_PS_Vehicle_Insurance_Verification")]
        public IHttpActionResult MAS_PS_Vehicle_Insurance_Verification()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetMASPriceSheet_Result> lstMASPriceSheet = new BLAssessmentRule().BL_GetMASPriceSheet(3);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstMASPriceSheet;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("MAS_PS_Informal_Mobile")]
        public IHttpActionResult MAS_PS_Informal_Mobile()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetMASPriceSheet_Result> lstMASPriceSheet = new BLAssessmentRule().BL_GetMASPriceSheet(4);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstMASPriceSheet;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region EdoGIS MDA Services

        [HttpGet]
        [Route("EdoGIS_MDAServices")]
        public IHttpActionResult EdoGIS_MDAServices([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetEdoGISMDAServiceForSupplier_Result> lstMDAServiceData = new BLMDAService().BL_GetEdoGISMDAServiceForSupplier();
                mObjAPIResponse = PaginationList(lstMDAServiceData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("EdoGIS_MDAServices_Items")]
        public IHttpActionResult EdoGIS_MDAServices_Items([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetEdoGISMDAServiceItemForSupplier_Result> lstMDAServiceItemData = new BLMDAServiceItem().BL_GetEdoGISMDAServiceItemForSupplier();
                mObjAPIResponse = PaginationList(lstMDAServiceItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Direct Assessment - A - M

        [HttpGet]
        [Route("Direct_Assessment_TP_A_M_TaxPayer")]
        public IHttpActionResult Direct_Assessment_TP_A_M_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 6, ProfileDescription = "Direct Assessment", TaxPayerName = "A,B,C,D,E,F,G,H,I,J,K,L,M" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                Elmah.ErrorSignal.FromCurrentContext().Raise(Ex);
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Direct_Assessment_TP_A_M_Asset")]
        public IHttpActionResult Direct_Assessment_TP_A_M_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 6, ProfileDescription = "Direct Assessment", TaxPayerName = "A,B,C,D,E,F,G,H,I,J,K,L,M" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Direct_Assessment_TP_A_M_Profile")]
        public IHttpActionResult Direct_Assessment_TP_A_M_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 6, ProfileDescription = "Direct Assessment", TaxPayerName = "A,B,C,D,E,F,G,H,I,J,K,L,M" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Direct_Assessment_TP_A_M_AssessmentRule")]
        public IHttpActionResult Direct_Assessment_TP_A_M_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 6, ProfileDescription = "Direct Assessment", TaxPayerName = "A,B,C,D,E,F,G,H,I,J,K,L,M" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Direct_Assessment_TP_A_M_AssessmentItem")]
        public IHttpActionResult Direct_Assessment_TP_A_M_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 6, ProfileDescription = "Direct Assessment", TaxPayerName = "A,B,C,D,E,F,G,H,I,J,K,L,M" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Direct_Assessment_TP_A_M_Assessment")]
        public IHttpActionResult Direct_Assessment_TP_A_M_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 6, ProfileDescription = "Direct Assessment", TaxPayerName = "A,B,C,D,E,F,G,H,I,J,K,L,M" });
                mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion

        #region Direct Assessment - N - Z

        [HttpGet]
        [Route("Direct_Assessment_TP_N_Z_TaxPayer")]
        public IHttpActionResult Direct_Assessment_TP_N_Z_TaxPayer([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData = new BLProfile().BL_GetTaxPayerBasedOnProfileForSupplier(new Profile() { IntSearchType = 6, ProfileDescription = "Direct Assessment", TaxPayerName = "N,O,P,Q,R,S,T,U,V,W,X,Y,Z" });
                mObjAPIResponse = PaginationList(lstTaxPayerData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Direct_Assessment_TP_N_Z_Asset")]
        public IHttpActionResult Direct_Assessment_TP_N_Z_Asset([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerAssetData = new BLProfile().BL_GetBusinessBasedOnProfileForSupplier(new Profile() { IntSearchType = 6, ProfileDescription = "Direct Assessment", TaxPayerName = "N,O,P,Q,R,S,T,U,V,W,X,Y,Z" });
                mObjAPIResponse = PaginationList(lstTaxPayerAssetData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Direct_Assessment_TP_N_Z_Profile")]
        public IHttpActionResult Direct_Assessment_TP_N_Z_Profile([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstProfileData = new BLProfile().BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(new Profile() { IntSearchType = 6, ProfileDescription = "Direct Assessment", TaxPayerName = "N,O,P,Q,R,S,T,U,V,W,X,Y,Z" });
                mObjAPIResponse = PaginationList(lstProfileData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Direct_Assessment_TP_N_Z_AssessmentRule")]
        public IHttpActionResult Direct_Assessment_TP_N_Z_AssessmentRule([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstAssessmentRuleData = new BLProfile().BL_GetAssessmentRuleBasedOnProfileForSupplier(new Profile() { IntSearchType = 6, ProfileDescription = "Direct Assessment", TaxPayerName = "N,O,P,Q,R,S,T,U,V,W,X,Y,Z" });
                mObjAPIResponse = PaginationList(lstAssessmentRuleData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Direct_Assessment_TP_N_Z_AssessmentItem")]
        public IHttpActionResult Direct_Assessment_TP_N_Z_AssessmentItem([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstAssessmentItemData = new BLProfile().BL_GetAssessmentItemBasedOnProfileForSupplier(new Profile() { IntSearchType = 6, ProfileDescription = "Direct Assessment", TaxPayerName = "N,O,P,Q,R,S,T,U,V,W,X,Y,Z" });
                mObjAPIResponse = PaginationList(lstAssessmentItemData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("Direct_Assessment_TP_N_Z_Assessment")]
        public IHttpActionResult Direct_Assessment_TP_N_Z_Assessment([FromUri] PagingParameterModel pobjPagingModel)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> lstAssessmentData = new BLProfile().BL_GetAssessmentBasedOnProfileForSupplier(new Profile() { IntSearchType = 6, ProfileDescription = "Direct Assessment", TaxPayerName = "N,O,P,Q,R,S,T,U,V,W,X,Y,Z" });
                mObjAPIResponse = PaginationList(lstAssessmentData, pobjPagingModel);
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        #endregion


        private List<GetTaxpayerViewModel> Getret(IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> lstTaxPayerData, int scrId)
        {
            GetTaxpayerViewModel gtVm = new GetTaxpayerViewModel();
            List<GetTaxpayerViewModel> gtVmList = new List<GetTaxpayerViewModel>();
            foreach (var ret in lstTaxPayerData)
            {
                gtVm.ApiId = scrId;
                gtVm.ContactAddress = ret.ContactAddress;
                gtVm.TaxPayerID = ret.TaxPayerID;
                gtVm.TaxPayerTypeID = ret.TaxPayerTypeID;
                gtVm.TaxPayerTypeName = ret.TaxPayerTypeName;
                gtVm.TaxPayerName = ret.TaxPayerName;
                gtVm.TaxPayerRIN = ret.TaxPayerRIN;
                gtVm.MobileNumber = ret.MobileNumber;
                gtVm.EmailAddress = ret.EmailAddress;
                gtVm.TIN = ret.TIN != null ? ret.TIN : "null";
                gtVm.TaxOffice = ret.TaxOffice;

                gtVmList.Add(gtVm);
            }

            return gtVmList;
        }
        private List<GetProfileViewModel> Getret(IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> lstTaxPayerData, int scrId)
        {
            GetProfileViewModel gtVm = new GetProfileViewModel();
            List<GetProfileViewModel> gtVmList = new List<GetProfileViewModel>();


            foreach (var ret in lstTaxPayerData)
            {
                gtVm.ApiId = scrId;
                gtVm.ProfileID = ret.ProfileID;
                gtVm.ProfileReferenceNo = ret.ProfileReferenceNo;
                gtVm.ProfileDescription = ret.ProfileDescription;
                gtVmList.Add(gtVm);
            }

            return gtVmList;
        }

        private List<GetAssestItemViewModel> Getret(IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> lstTaxPayerData, int scrId)
        {
            GetAssestItemViewModel gtVm = new GetAssestItemViewModel();
            List<GetAssestItemViewModel> gtVmList = new List<GetAssestItemViewModel>();

            foreach (var ret in lstTaxPayerData)
            {
                gtVm.ApiId = scrId;
                gtVm.TaxPayerID = ret.TaxPayerID;
                gtVm.TaxPayerTypeID = ret.TaxPayerTypeID;
                gtVm.TaxPayerTypeName = ret.TaxPayerTypeName;
                gtVm.TaxPayerRIN = ret.TaxPayerRIN;
                gtVm.AssetID = ret.AssetID;
                gtVm.AssetTypeID = ret.AssetTypeID;
                gtVm.AssetTypeName = ret.AssetTypeName;
                gtVm.AssetRIN = ret.AssetRIN;
                gtVm.ProfileID = ret.ProfileID;
                gtVm.ProfileReferenceNo = ret.ProfileReferenceNo;
                gtVm.ProfileDescription = ret.ProfileDescription;
                gtVm.AssessmentRuleID = ret.AssessmentRuleID;
                gtVm.AssessmentRuleCode = ret.AssessmentRuleCode;
                gtVm.AssessmentRuleName = ret.AssessmentRuleName;
                gtVm.AssessmentItemID = ret.AssessmentItemID;
                gtVm.AssessmentItemReferenceNo = ret.AssessmentItemReferenceNo;
                gtVm.AssessmentGroupID = ret.AssessmentGroupID;
                gtVm.AssessmentGroupName = ret.AssessmentGroupName;
                gtVm.AssessmentSubGroupID = ret.AssessmentSubGroupID;
                gtVm.AssessmentSubGroupName = ret.AssessmentSubGroupName;
                gtVm.RevenueStreamID = ret.RevenueStreamID;
                gtVm.RevenueStreamName = ret.RevenueStreamName;
                gtVm.RevenueSubStreamID = ret.RevenueSubStreamID;
                gtVm.RevenueSubStreamName = ret.RevenueSubStreamName;
                gtVm.AssessmentItemCategoryID = ret.AssessmentItemCategoryID;
                gtVm.AssessmentItemCategoryName = ret.AssessmentItemCategoryName;
                gtVm.AssessmentItemSubCategoryID = ret.AssessmentItemSubCategoryID;
                gtVm.AssessmentItemSubCategoryName = ret.AssessmentItemSubCategoryName;
                gtVm.AgencyID = ret.AgencyID;
                gtVm.AgencyName = ret.AgencyName;
                gtVm.AssessmentItemName = ret.AssessmentItemName;
                gtVm.ComputationID = ret.ComputationID;
                gtVm.ComputationName = ret.ComputationName;
                gtVm.TaxBaseAmount = ret.TaxBaseAmount;
                gtVm.Percentage = ret.Percentage;
                gtVm.TaxAmount = ret.TaxAmount;

                gtVmList.Add(gtVm);
            }

            return gtVmList;
        }
        private List<GetAssessmentRuleViewModel> Getret(IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> lstTaxPayerData, int scrId)
        {
            List<GetAssessmentRuleViewModel> gtVmList = new List<GetAssessmentRuleViewModel>();

            foreach (var ret in lstTaxPayerData)
            {
                GetAssessmentRuleViewModel gtVm = new GetAssessmentRuleViewModel();

                gtVm.ApiId = scrId;
                gtVm.TaxPayerID = ret.TaxPayerID;
                gtVm.TaxPayerTypeID = ret.TaxPayerTypeID;
                gtVm.TaxPayerTypeName = ret.TaxPayerTypeName;
                gtVm.TaxPayerRIN = ret.TaxPayerRIN;
                gtVm.AssetID = ret.AssetID;
                gtVm.AssetTypeID = ret.AssetTypeID;
                gtVm.AssetTypeName = ret.AssetTypeName;
                gtVm.AssetRIN = ret.AssetRIN;
                gtVm.ProfileID = ret.ProfileID;
                gtVm.ProfileReferenceNo = ret.ProfileReferenceNo;
                gtVm.ProfileDescription = ret.ProfileDescription;
                gtVm.AssessmentRuleID = ret.AssessmentRuleID;
                gtVm.AssessmentRuleCode = ret.AssessmentRuleCode;
                gtVm.AssessmentRuleName = ret.AssessmentRuleName;
                gtVm.TaxMonth = ret.TaxMonth;
                gtVm.PaymentOptionName = ret.PaymentOptionName;
                gtVm.PaymentOptionID = ret.PaymentOptionID;
                gtVm.TaxYear = ret.TaxYear;
                gtVm.AssessmentAmount = ret.AssessmentAmount;
                gtVm.PaymentFrequencyName = ret.PaymentFrequencyName;
                gtVm.PaymentFrequencyID = ret.PaymentFrequencyID;
                gtVm.RuleRunName = ret.RuleRunName;
                gtVm.RuleRunID = ret.RuleRunID;

                gtVmList.Add(gtVm);
            }

            return gtVmList;
        }
        private List<GetBusinessViewModel> Getret(IList<usp_GetBusinessBasedOnProfileForSupplier_Result> lstTaxPayerData, int scrId)
        {
            GetBusinessViewModel gtVm = new GetBusinessViewModel();
            List<GetBusinessViewModel> gtVmList = new List<GetBusinessViewModel>();


            foreach (var ret in lstTaxPayerData)
            {
                gtVm.ApiId = scrId;
                gtVm.BusinessID = ret.BusinessID;
                gtVm.BusinessRIN = ret.BusinessRIN;
                gtVm.AssetTypeID = ret.AssetTypeID;
                gtVm.AssetTypeName = ret.AssetTypeName;
                gtVm.BusinessTypeID = ret.BusinessTypeID;
                gtVm.BusinessTypeName = ret.BusinessTypeName;
                gtVm.BusinessName = ret.BusinessTypeName;
                gtVm.LGAID = ret.LGAID;
                gtVm.LGAName = ret.LGAName;
                gtVm.BusinessCategoryID = ret.BusinessCategoryID;
                gtVm.BusinessCategoryName = ret.BusinessCategoryName;
                gtVm.BusinessSectorID = ret.BusinessSectorID;
                gtVm.BusinessSectorName = ret.BusinessSectorName;
                gtVm.BusinessSubSectorID = ret.BusinessSubSectorID;
                gtVm.BusinessSubSectorName = ret.BusinessSubSectorName;
                gtVm.BusinessStructureID = ret.BusinessStructureID;
                gtVm.BusinessStructureName = ret.BusinessStructureName;
                gtVm.BusinessOperationID = ret.BusinessOperationID;
                gtVm.BusinessOperationName = ret.BusinessOperationName;
                gtVm.SizeID = ret.SizeID;
                gtVm.SizeName = ret.SizeName;
                gtVm.ContactName = ret.ContactName;
                gtVm.BusinessNumber = ret.BusinessNumber;
                gtVm.BusinessAddress = ret.BusinessAddress;

                gtVmList.Add(gtVm);
            }

            return gtVmList;
        }
        public class Returner
        {

            public int AssessmentRuleID { get; set; }
            public string AssessmentRuleCode { get; set; }
            public int ProfileID { get; set; }
            public string AssessmentRuleName { get; set; }
            public int RuleRunID { get; set; }
            public int PaymentFrequencyID { get; set; }
            public decimal AssessmentAmount { get; set; }
            public int TaxYear { get; set; }
            public int PaymentOptionID { get; set; }
            public bool Active { get; set; }
            public int CreatedBy { get; set; }
            public System.DateTime CreatedDate { get; set; }
            public int ModifiedBy { get; set; }
            public DateTime ModifiedDate { get; set; }
            public int TaxMonth { get; set; }
        }
    }
}
