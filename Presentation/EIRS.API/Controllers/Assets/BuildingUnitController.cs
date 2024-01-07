using EIRS.API.Utility;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;
using EIRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace EIRS.API.Controllers.Assets
{
    /// <summary>
    /// Building Operations
    /// </summary>
    [RoutePrefix("Asset/BuildingUnit")]
    
    public class BuildingUnitController : BaseController
    {
        /// <summary>
        /// Add New Building
        /// </summary>
        /// <param name="pObjBuildingUnitModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Insert")]
        public IHttpActionResult Insert(BuildingUnitViewModel pObjBuildingUnitModel)
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
                Building_Unit mObjBuildingUnit = new Building_Unit()
                {
                    BuildingUnitID = 0,
                    UnitNumber = pObjBuildingUnitModel.UnitNumber,
                    UnitPurposeID = pObjBuildingUnitModel.UnitPurposeID,
                    UnitFunctionID = pObjBuildingUnitModel.UnitFunctionID,
                    UnitOccupancyID = pObjBuildingUnitModel.UnitOccupancyID,
                    SizeID = pObjBuildingUnitModel.SizeID,
                    Active = true,
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse<Building_Unit> mObjFuncResponse = new BLBuildingUnit().BL_InsertUpdateBuildingUnit(mObjBuildingUnit);

                    if (mObjFuncResponse.Success)
                    {
                        mObjAPIResponse.Success = true;
                        mObjAPIResponse.Message = mObjFuncResponse.Message;
                        mObjAPIResponse.Result = mObjFuncResponse.AdditionalData;
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
                    mObjAPIResponse.Message = "Error occurred while saving building unit";
                }
            }

            return Ok(mObjAPIResponse);
        }

    }
}
