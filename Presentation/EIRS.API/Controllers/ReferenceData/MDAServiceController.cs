using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EIRS.API.Models;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;

namespace EIRS.API.Controllers
{
    /// <summary>
    /// Assessment Rules Operations
    /// </summary>
    [RoutePrefix("ReferenceData/MDAService")]
    //
    public class MDAServiceController : BaseController
    {
        /// <summary>
        /// Returns a list of mda service
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("List")]
        public IHttpActionResult List()
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                MDA_Services mObjMDAService = new MDA_Services()
                {
                    IntStatus = 1
                };

                IList<usp_GetMDAServiceList_Result> lstMDAService = new BLMDAService().BL_GetMDAServiceList(mObjMDAService);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstMDAService;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }
        [HttpPost]
        [Route("PaginatedList")]
        public IHttpActionResult PaginatedList([FromBody] NewPagingParameterModel pobjPagingModel)
        {

            APIResponse mObjAPIResponse = new APIResponse();
            try
            {
                MDA_Services mObjMDAService = new MDA_Services()
                {
                    IntStatus = 1
                };

                IList<usp_GetMDAServiceList_Result> lstMDAService = new BLMDAService().BL_GetMDAServiceList(mObjMDAService);
                if (pobjPagingModel.year > 0)
                {
                    lstMDAService = lstMDAService.Where(x => x.TaxYear == pobjPagingModel.year).ToList();
                }
                if (!string.IsNullOrEmpty(pobjPagingModel.filter))
                {
                    lstMDAService = lstMDAService.Where(x => x.MDAServiceName.ToLower().Contains(pobjPagingModel.filter.ToLower())).ToList();
                }
                if(pobjPagingModel.pageSize > 0) { 
                mObjAPIResponse = PaginationListII(lstMDAService, pobjPagingModel);
                }
                else
                {
                    mObjAPIResponse.Success = true;
                    mObjAPIResponse.Result = lstMDAService;
                }
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        } 
        [HttpPost]
        [Route("PaginatedList")]
        public IHttpActionResult PaginatedListWith([FromBody] NewModel pobjPagingModel)
        {

            APIResponse mObjAPIResponse = new APIResponse();
            try
            {
                MDA_Services mObjMDAService = new MDA_Services()
                {
                    IntStatus = 1
                };

                IList<usp_GetMDAServiceList_Result> lstMDAService = new BLMDAService().BL_GetMDAServiceList(mObjMDAService);
                if (pobjPagingModel.year > 0)
                {
                    lstMDAService = lstMDAService.Where(x => x.TaxYear == pobjPagingModel.year).ToList();
                }
                if (!string.IsNullOrEmpty(pobjPagingModel.filter))
                {
                    lstMDAService = lstMDAService.Where(x => x.MDAServiceName.ToLower().Contains(pobjPagingModel.filter.ToLower())).ToList();
                }
                if(pobjPagingModel.pageSize > 0) { 
                mObjAPIResponse = PaginationListII(lstMDAService, pobjPagingModel);
                }
                else
                {
                    mObjAPIResponse.Success = true;
                    mObjAPIResponse.Result = lstMDAService;
                }
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

    }
}