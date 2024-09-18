using EIRS.API.Utility;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;


namespace EIRS.API.Controllers
{

    [RoutePrefix("Adjustment")]

    public class AdjustmentController : BaseController
    {
        EIRSEntities _db = new EIRSEntities();

        [HttpPost]
        [Route("CreateABAdjustment")]
        public IHttpActionResult CreateABAdjustment(MAP_Assessment_Adjustment pObjAdjustment, int AssessmentID)
        {
            // IDictionary<string, object> dcResponse = new Dictionary<string, object>();
            APIResponse mObjAPIResponse = new APIResponse();
            String token = Request.Headers.Authorization.Parameter;
            int? usId = Utilities.GetUserId(token);
            int? userID = usId.HasValue ? usId : 10;
            try
            {
                decimal? Amountholder = 0;
                List<int?> lstOfProfiles = new List<int?>();
                MAP_Assessment_AssessmentItem retVal2 = new MAP_Assessment_AssessmentItem();
                Assessment retVal = new Assessment();
                using (_db = new EIRSEntities())
                {
                    retVal = _db.Assessments.FirstOrDefault(o => o.AssessmentID == AssessmentID);
                    Amountholder = retVal.AssessmentAmount;
                    retVal2 = _db.MAP_Assessment_AssessmentItem.FirstOrDefault(o => o.AAIID == pObjAdjustment.AAIID);
                    var lstAssessmentRules = _db.MAP_Assessment_AssessmentRule.Where(o => o.AssessmentID == AssessmentID);
                    var lstProfileId = lstAssessmentRules.Select(o => o.ProfileID).ToList();
                    lstOfProfiles = _db.Profiles.Where(o => lstProfileId.Contains(o.ProfileID)).Select(o => o.ProfileTypeID).ToList();
                    lstOfProfiles = lstOfProfiles.Where(o => o.Value.Equals(5) || o.Value.Equals(7)).ToList();
                }
                BLAssessment mObjBLAssessment = new BLAssessment();
                pObjAdjustment.AdjustmentDate = CommUtil.GetCurrentDateTime();
                pObjAdjustment.CreatedDate = CommUtil.GetCurrentDateTime();
                pObjAdjustment.CreatedBy = userID;//SessionManager.UserID;

                FuncResponse mObjResponse = mObjBLAssessment.BL_InsertAdjustment(pObjAdjustment);

                //Check if Assessment Amount is Greater than Paid Amount then mark it parital
                usp_GetAssessmentRuleItemDetails_Result mObjAssessmentRuleItemDetails = mObjBLAssessment.BL_GetAssessmentRuleItemDetails(pObjAdjustment.AAIID.GetValueOrDefault());
                usp_GetAssessmentList_Result mObjAssessmentData = mObjBLAssessment.BL_GetAssessmentDetails(new Assessment() { AssessmentID = AssessmentID, IntStatus = 2 });
                IList<usp_GetAssessmentAdjustmentList_Result> lstAssessmentAdjustment = mObjBLAssessment.BL_GetAssessmentAdjustment(mObjAssessmentData.AssessmentID.GetValueOrDefault());
                //AssessmentRepository assessmentRepository = new AssessmentRepository();
                IList<usp_GetAssessmentRuleItemList_Result> lstAssessmentItems = mObjBLAssessment.BL_GetAssessmentRuleItem(mObjAssessmentData.AssessmentID.GetValueOrDefault());

                MAP_Assessment_AssessmentItem ret = mObjBLAssessment.GetAssessmentItems(pObjAdjustment.AAIID.GetValueOrDefault());
                decimal? newTaxAmount;
                decimal? newTaxBaseAmount;

                decimal? newAmount = 0;
                if (pObjAdjustment.AdjustmentTypeID == 1)
                {
                    // var retVal = lstAssessmentItems.FirstOrDefault(o => o.AAIID == pObjAdjustment.AAIID);
                    newAmount = (lstAssessmentItems.Sum(t => t.TaxAmount) + pObjAdjustment.Amount);
                    ret.TaxAmount = ret.TaxAmount + pObjAdjustment.Amount;
                }
                else
                {

                    // var retVal = lstAssessmentItems.FirstOrDefault(o => o.AAIID == pObjAdjustment.AAIID);
                    newAmount = (lstAssessmentItems.Sum(t => t.TaxAmount) + pObjAdjustment.Amount);
                    ret.TaxAmount = ret.TaxAmount + pObjAdjustment.Amount;

                }
                int statId = 1;
                if (mObjAssessmentData.SettlementAmount >= newAmount)
                    statId = (int)EnumList.SettlementStatus.Settled;
                else if (mObjAssessmentData.SettlementAmount < newAmount && newAmount != 0)
                    statId = (int)EnumList.SettlementStatus.Partial;

                //update item table
                MAP_Assessment_AssessmentItem mObjAAI = new MAP_Assessment_AssessmentItem()
                {
                    AAIID = pObjAdjustment.AAIID.GetValueOrDefault(),
                    ModifiedBy = userID,//SessionManager.UserID,
                    ModifiedDate = CommUtil.GetCurrentDateTime(),
                    PaymentStatusID = statId,
                    TaxAmount = retVal2.TaxAmount,
                    TaxBaseAmount = retVal2.TaxBaseAmount
                };
                mObjBLAssessment.BL_UpdateAssessmentItemStatus(mObjAAI);
                Assessment mObjAssessment = new Assessment();
                //update assessment table
                if (lstOfProfiles.Count > 0)
                {
                    mObjAssessment = new Assessment()
                    {
                        AssessmentID = AssessmentID,
                        SettlementStatusID = retVal.SettlementStatusID == 3 ? 8 : 6,
                        ModifiedDate = CommUtil.GetCurrentDateTime(),
                        ModifiedBy = userID, //SessionManager.UserID,
                        AssessmentAmount = Amountholder
                    };
                }
                else
                {
                    mObjAssessment = new Assessment()
                    {
                        AssessmentID = AssessmentID,
                        SettlementStatusID = statId,
                        ModifiedDate = CommUtil.GetCurrentDateTime(),
                        ModifiedBy = userID,//SessionManager.UserID,
                        AssessmentAmount = Amountholder
                    };
                }

                mObjBLAssessment.BL_UpdateAssessmentSettlementStatus(mObjAssessment);

                // dcResponse["success"] = mObjResponse.Success;
                // dcResponse["Message"] = mObjResponse.Message;

                mObjAPIResponse.Success = mObjResponse.Success;
                mObjAPIResponse.Message = mObjResponse.Message;
                // mObjAPIResponse.Result = results;

            }
            catch (Exception ex)
            {
                // Logger.SendErrorToText(ex);
                mObjAPIResponse.Success = false;
                mObjAPIResponse.Message = $"Error occurred - {ex.Message}";
            }
            return Ok(mObjAPIResponse);
            // return Json(dcResponse, JsonRequestBehavior.AllowGet);
        }

    }
}