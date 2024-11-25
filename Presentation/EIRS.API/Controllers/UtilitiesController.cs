using EIRS.API.Models;
using EIRS.API.Utility;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using Elmah;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web.Http;

namespace EIRS.API.Controllers
{


    /// <summary>
    /// Utilities Operations
    /// </summary>
    [RoutePrefix("Utilities")]

    public class UtilitiesController : BaseController
    {
        EIRSEntities _db = new EIRSEntities();
    
        /// <summary>
        /// Returns a list of assessment
        /// </summary>
        /// <returns></returns>



        [HttpGet]
        [Route("GetAllTaxOffice")]
        public IHttpActionResult GetAllTaxOffice()
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

    }
}




