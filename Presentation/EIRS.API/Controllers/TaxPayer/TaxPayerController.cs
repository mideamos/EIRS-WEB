using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace EIRS.API.Controllers.TaxPayer
{
    [RoutePrefix("TaxPayer")]
    
    public class TaxPayerController : BaseController
    {
        [HttpGet]
        [Route("SearchTaxPayer")]
        public IHttpActionResult SearchTaxPayer(string TaxPayerRIN, string MobileNumber)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                SearchTaxPayerFilter mObjTaxPayerFilter = new SearchTaxPayerFilter()
                {
                    TaxPayerRIN = TaxPayerRIN,
                    MobileNumber = MobileNumber,
                    intSearchType = 1
                };

                IList<usp_SearchTaxPayer_Result> lstTaxPayer = new BLTaxPayerAsset().BL_SearchTaxPayer(mObjTaxPayerFilter);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstTaxPayer;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("SearchTaxPayerByRIN")]
        public IHttpActionResult SearchTaxPayerByRIN(string TaxPayerRIN)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                SearchTaxPayerFilter mObjTaxPayerFilter = new SearchTaxPayerFilter()
                {
                    TaxPayerRIN = TaxPayerRIN,
                    intSearchType = 4
                };

                IList<usp_SearchTaxPayer_Result> lstTaxPayer = new BLTaxPayerAsset().BL_SearchTaxPayer(mObjTaxPayerFilter);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstTaxPayer;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("SearchTaxPayerByMobileNumber")]
        public IHttpActionResult SearchTaxPayerByMobileNumber(string MobileNumber)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                SearchTaxPayerFilter mObjTaxPayerFilter = new SearchTaxPayerFilter()
                {
                    MobileNumber = MobileNumber,
                    intSearchType = 1
                };

                IList<usp_SearchTaxPayer_Result> lstTaxPayer = new BLTaxPayerAsset().BL_SearchTaxPayer(mObjTaxPayerFilter);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstTaxPayer;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("SearchMainBusinessOwner")]
        public IHttpActionResult SearchMainBusinessOwner(string BusinessName)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                SearchTaxPayerFilter mObjTaxPayerFilter = new SearchTaxPayerFilter()
                {
                    AssetName = BusinessName,
                    intSearchType = 2
                };

                IList<usp_SearchTaxPayer_Result> lstTaxPayer = new BLTaxPayerAsset().BL_SearchTaxPayer(mObjTaxPayerFilter);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstTaxPayer;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("SearchTaxPayerByName")]
        public IHttpActionResult SearchTaxPayerByName(string TaxPayerName)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                SearchTaxPayerFilter mObjTaxPayerFilter = new SearchTaxPayerFilter()
                {
                    TaxPayerName = TaxPayerName,
                    intSearchType = 3
                };

                IList<usp_SearchTaxPayer_Result> lstTaxPayer = new BLTaxPayerAsset().BL_SearchTaxPayer(mObjTaxPayerFilter);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstTaxPayer;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("GetAssetList")]
        public IHttpActionResult GetAssetList(int TaxPayerTypeID, int TaxPayerID)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    TaxPayerID = TaxPayerID,
                    TaxPayerTypeID = TaxPayerTypeID
                };

                IList<usp_GetTaxPayerAssetList_Result> lstTaxPayerAsset = new BLTaxPayerAsset().BL_GetTaxPayerAssetList(mObjTaxPayerAsset);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstTaxPayerAsset;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("SearchTaxPayerByTaxPayerType")]
        public IHttpActionResult SearchTaxPayerByTaxPayerType(string Name, string TaxPayerRIN, string MobileNumber, int TaxPayerTypeID)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                SearchTaxPayerFilter mObjTaxPayerFilter = new SearchTaxPayerFilter()
                {
                    TaxPayerName = Name,
                    TaxPayerRIN = TaxPayerRIN,
                    MobileNumber = MobileNumber,
                    TaxPayerTypeID = TaxPayerTypeID,
                    intSearchType = 5
                };

                IList<usp_SearchTaxPayer_Result> lstTaxPayer = new BLTaxPayerAsset().BL_SearchTaxPayer(mObjTaxPayerFilter);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstTaxPayer;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("GetLandAssetForTaxPayer")]
        public IHttpActionResult GetLandAssetForTaxPayer(int TaxPayerTypeID, int TaxPayerID)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {

                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    TaxPayerID = TaxPayerID,
                    TaxPayerTypeID = TaxPayerTypeID,
                };

                IList<usp_GetTaxPayerLandList_Result> lstTaxPayerLand = new BLTaxPayerAsset().BL_GetTaxPayerLandList(mObjTaxPayerAsset);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstTaxPayerLand;
            }
            catch (Exception Ex)
            {
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = Ex.Message;
            }

            return Ok(mObjAPIResponse);
        }

        [HttpGet]
        [Route("SearchTaxPayerByTIN")]
        public IHttpActionResult SearchTaxPayerByTIN(string TaxPayerTIN)
        {
            APIResponse mObjAPIResponse = new APIResponse();

            try
            {
                SearchTaxPayerFilter mObjTaxPayerFilter = new SearchTaxPayerFilter()
                {
                    TaxPayerTIN = TaxPayerTIN,
                    intSearchType = 6
                };

                IList<usp_SearchTaxPayer_Result> lstTaxPayer = new BLTaxPayerAsset().BL_SearchTaxPayer(mObjTaxPayerFilter);

                mObjAPIResponse.Success = true;
                mObjAPIResponse.Result = lstTaxPayer;
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
