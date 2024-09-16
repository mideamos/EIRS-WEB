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
    /// Assessment Rules Operations
    /// </summary>
    [RoutePrefix("ReferenceData/MDAServiceItem")]
   // 
    public class MDAServiceItemController : BaseController
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
                MDA_Service_Items mObjMDAServiceItem = new MDA_Service_Items()
                {
                    intStatus = 1
                };

                IList<usp_GetMDAServiceItemList_Result> lstMDAServiceItem = new BLMDAServiceItem().BL_GetMDAServiceItemList(mObjMDAServiceItem);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstMDAServiceItem;
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