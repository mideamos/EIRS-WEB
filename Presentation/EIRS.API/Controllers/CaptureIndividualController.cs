using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;


namespace EIRS.API.Controllers
{

    [RoutePrefix("CaptureIndividual")]

    public class CaptureIndividualController : BaseController
    {
        EIRSEntities _db = new EIRSEntities();

        public class TccRequestModel
        {
            public int TaxYear { get; set; }
            public int TaxPayerID { get; set; }
        }

        [HttpPost]
        [Route("AddTCCRequestBatch")]
        public IHttpActionResult AddTCCRequestBatch(List<TccRequestModel> requests)
        {

            // IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            APIResponse mObjAPIResponse = new APIResponse();
            List<object> results = new List<object>();
            BLTCC mObjBLTCC = new BLTCC();
            // bool allSuccess = true;
            bool atLeastOneSuccess = false;

            try
            {

                foreach (var request in requests)
                {
                    var userDet = _db.Individuals.FirstOrDefault(o => o.IndividualID == request.TaxPayerID);

                    // if (userDet != null)
                    // {
                    TCC_Request mObjRequest = new TCC_Request()
                    {
                        // TaxOfficeId = userDet.TaxOfficeID,
                        TaxOfficeId = userDet != null ? userDet.TaxOfficeID : 0,
                        RequestDate = CommUtil.GetCurrentDateTime(),
                        TaxPayerID = request.TaxPayerID,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                        TaxYear = request.TaxYear,
                        StatusID = (int)EnumList.TCCRequestStatus.In_Progess,
                        CreatedBy = 10,//SessionManager.UserID,
                        // CreatedByTypeID = 3,//add column to DB
                        CreatedDate = CommUtil.GetCurrentDateTime(),
                    };

                    FuncResponse<TCC_Request> mObjICFuncResponse = mObjBLTCC.BL_GetIncompleteRequest(mObjRequest);

                    if (mObjICFuncResponse.Success)
                    {
                        FuncResponse<TCC_Request> mObjReqResponse = mObjBLTCC.BL_InsertTCCRequest(mObjRequest);

                        if (mObjReqResponse.Success)
                        {
                            atLeastOneSuccess = true;
                            mObjRequest = new TCC_Request()
                            {
                                TCCRequestID = mObjReqResponse.AdditionalData.TCCRequestID,
                                ServiceBillID = null,
                                StatusID = (int)EnumList.TCCRequestStatus.In_Progess,
                            };

                            new BLTCC().BL_UpdateServiceBillInRequest(mObjRequest);


                            results.Add(new
                            {
                                TaxPayerID = request.TaxPayerID,
                                TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                                TaxPayerRIN = userDet != null ? userDet.IndividualRIN : "",
                                RequestRefNo = mObjReqResponse.AdditionalData.RequestRefNo,
                                TaxYear = request.TaxYear.ToString(),
                                RequestDate = CommUtil.GetCurrentDateTime().ToString("dd-MMM-yyyy"),
                                TaxOfficeId = userDet != null ? userDet.TaxOfficeID : 0,
                                // RequestStatus = "Paid"
                            });
                        }
                        else
                        {
                            // allSuccess = false;
                            // mObjAPIResponse.Message = $"Request failed for TaxPayerID: {request.TaxPayerID}, TaxYear: {request.TaxYear}. Error: {mObjReqResponse.Message}";
                            mObjAPIResponse.Message += $"Request failed for TaxPayerID: {request.TaxPayerID}, TaxYear: {request.TaxYear}. Error: {mObjReqResponse.Message}\n";
                        }
                    }
                    else
                    {
                        // allSuccess = false;
                        // mObjAPIResponse.Message = $"Request already exists for TaxPayerID: {request.TaxPayerID}, TaxYear: {request.TaxYear}.";
                        mObjAPIResponse.Message += $"Request already exists for TaxPayerID: {request.TaxPayerID}, TaxYear: {request.TaxYear}.\n";
                    }
                    // }
                    // else
                    // {
                    //     allSuccess = false;
                    //     mObjAPIResponse.Message = $"TaxPayer not found for TaxPayerID: {request.TaxPayerID}.";
                    // }
                }

                // mObjAPIResponse.Success = allSuccess;
                mObjAPIResponse.Success = atLeastOneSuccess;
                mObjAPIResponse.Result = results;
            }

            catch (Exception ex)
            {
                // Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = $"Error occurred - {ex.Message}";
            }


            return Ok(mObjAPIResponse);
            // return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }


    }
}