using System;
using System.Linq;
using System.Transactions;
using System.Web.Http;
using EIRS.API.Models;
using EIRS.API.Utility;
using EIRS.BLL;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.API.Controllers
{
    
    [RoutePrefix("TaxPayerAsset")]
    public class TaxPayerAssetController : BaseController
    {
        [HttpPost]
        [Route("AddBusinessToIndividual")]
        public IHttpActionResult AddBusinessToIndividual(TaxPayerAssetViewModel pObjAssetModel)
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

                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetTypeID = (int)EnumList.AssetTypes.Business,
                    AssetID = pObjAssetModel.AssetID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                    TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID,
                    TaxPayerID = pObjAssetModel.TaxPayerID,
                    Active = true,
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjFuncResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

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
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Message = "Error occurred while mapping business to individual";
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("AddBusinessToCompany")]
        public IHttpActionResult AddBusinessToCompany(TaxPayerAssetViewModel pObjAssetModel)
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

                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetTypeID = (int)EnumList.AssetTypes.Business,
                    AssetID = pObjAssetModel.AssetID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                    TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID,
                    TaxPayerID = pObjAssetModel.TaxPayerID,
                    Active = true,
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjFuncResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

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
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Message = "Error occurred while mapping business to company";
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("AddBusinessToGovernment")]
        public IHttpActionResult AddBusinessToGovernment(TaxPayerAssetViewModel pObjAssetModel)
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

                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetTypeID = (int)EnumList.AssetTypes.Business,
                    AssetID = pObjAssetModel.AssetID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                    TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID,
                    TaxPayerID = pObjAssetModel.TaxPayerID,
                    Active = true,
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjFuncResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

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
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Message = "Error occurred while mapping business to government";
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("AddBusinessToSpecial")]
        public IHttpActionResult AddBusinessToSpecial(TaxPayerAssetViewModel pObjAssetModel)
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

                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetTypeID = (int)EnumList.AssetTypes.Business,
                    AssetID = pObjAssetModel.AssetID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Special,
                    TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID,
                    TaxPayerID = pObjAssetModel.TaxPayerID,
                    Active = true,
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjFuncResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

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
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Message = "Error occurred while mapping business to Special";
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("AddVehicleToIndividual")]
        public IHttpActionResult AddVehicleToIndividual(TaxPayerAssetViewModel pObjAssetModel)
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
                using (TransactionScope mObjTransactionScope = new TransactionScope())
                {
                    MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                    {
                        AssetTypeID = (int)EnumList.AssetTypes.Vehicles,
                        AssetID = pObjAssetModel.AssetID,
                        TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                        TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID,
                        TaxPayerID = pObjAssetModel.TaxPayerID,
                        Active = true,
                        //CreatedBy = ClaimsIdentityExtensions.GetUserID(User.Identity),
                        CreatedBy = userId.HasValue ? userId : 22,
                        CreatedDate = CommUtil.GetCurrentDateTime()
                    };

                    try
                    {

                        FuncResponse mObjFuncResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

                        if (mObjFuncResponse.Success)
                        {
                            mObjTransactionScope.Complete();
                            mObjAPIResponse.Success = true;
                            mObjAPIResponse.Message = mObjFuncResponse.Message;
                        }
                        else
                        {
                            Transaction.Current.Rollback();
                            mObjAPIResponse.Success = false;
                            mObjAPIResponse.Message = mObjFuncResponse.Message;
                        }
                    }
                    catch (Exception ex)
                    {
                        Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                        Transaction.Current.Rollback();
                        mObjAPIResponse.Success = false;
                        mObjAPIResponse.Message = "Error occurred while mapping vehicle to individual";
                    }
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("AddVehicleToCompany")]
        public IHttpActionResult AddVehicleToCompany(TaxPayerAssetViewModel pObjAssetModel)
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

                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetTypeID = (int)EnumList.AssetTypes.Vehicles,
                    AssetID = pObjAssetModel.AssetID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                    TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID,
                    TaxPayerID = pObjAssetModel.TaxPayerID,
                    Active = true,
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjFuncResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

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
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Message = "Error occurred while mapping vehicle to company";
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("AddVehicleToGovernment")]
        public IHttpActionResult AddVehicleToGovernment(TaxPayerAssetViewModel pObjAssetModel)
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

                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetTypeID = (int)EnumList.AssetTypes.Vehicles,
                    AssetID = pObjAssetModel.AssetID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                    TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID,
                    TaxPayerID = pObjAssetModel.TaxPayerID,
                    Active = true,
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjFuncResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

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
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Message = "Error occurred while mapping vehicle to government";
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("AddVehicleToSpecial")]
        public IHttpActionResult AddVehicleToSpecial(TaxPayerAssetViewModel pObjAssetModel)
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

                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetTypeID = (int)EnumList.AssetTypes.Vehicles,
                    AssetID = pObjAssetModel.AssetID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Special,
                    TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID,
                    TaxPayerID = pObjAssetModel.TaxPayerID,
                    Active = true,
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjFuncResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

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
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Message = "Error occurred while mapping vehicle to Special";
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("AddLandToIndividual")]
        public IHttpActionResult AddLandToIndividual(TaxPayerAssetViewModel pObjAssetModel)
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

                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetTypeID = (int)EnumList.AssetTypes.Land,
                    AssetID = pObjAssetModel.AssetID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                    TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID,
                    TaxPayerID = pObjAssetModel.TaxPayerID,
                    Active = true,
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjFuncResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

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
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Message = "Error occurred while mapping land to individual";
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("AddLandToCompany")]
        public IHttpActionResult AddLandToCompany(TaxPayerAssetViewModel pObjAssetModel)
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

                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetTypeID = (int)EnumList.AssetTypes.Land,
                    AssetID = pObjAssetModel.AssetID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                    TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID,
                    TaxPayerID = pObjAssetModel.TaxPayerID,
                    Active = true,
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjFuncResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

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
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Message = "Error occurred while mapping land to company";
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("AddLandToGovernment")]
        public IHttpActionResult AddLandToGovernment(TaxPayerAssetViewModel pObjAssetModel)
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

                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetTypeID = (int)EnumList.AssetTypes.Land,
                    AssetID = pObjAssetModel.AssetID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                    TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID,
                    TaxPayerID = pObjAssetModel.TaxPayerID,
                    Active = true,
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjFuncResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

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
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Message = "Error occurred while mapping land to government";
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("AddLandToSpecial")]
        public IHttpActionResult AddLandToSpecial(TaxPayerAssetViewModel pObjAssetModel)
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

                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetTypeID = (int)EnumList.AssetTypes.Land,
                    AssetID = pObjAssetModel.AssetID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Special,
                    TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID,
                    TaxPayerID = pObjAssetModel.TaxPayerID,
                    Active = true,
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjFuncResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

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
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Message = "Error occurred while mapping land to Special";
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("AddBuildingToIndividual")]
        public IHttpActionResult AddBuildingToIndividual(TaxPayerAssetViewModel pObjAssetModel)
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

                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetTypeID = (int)EnumList.AssetTypes.Building,
                    AssetID = pObjAssetModel.AssetID,
                    BuildingUnitID = pObjAssetModel.BuildingUnitID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Individual,
                    TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID,
                    TaxPayerID = pObjAssetModel.TaxPayerID,
                    Active = true,
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjFuncResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

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
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Message = "Error occurred while mapping building to individual";
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("AddBuildingToCompany")]
        public IHttpActionResult AddBuildingToCompany(TaxPayerAssetViewModel pObjAssetModel)
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

                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetTypeID = (int)EnumList.AssetTypes.Building,
                    AssetID = pObjAssetModel.AssetID,
                    BuildingUnitID = pObjAssetModel.BuildingUnitID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Companies,
                    TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID,
                    TaxPayerID = pObjAssetModel.TaxPayerID,
                    Active = true,
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjFuncResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

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
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Message = "Error occurred while mapping building to company";
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("AddBuildingToGovernment")]
        public IHttpActionResult AddBuildingToGovernment(TaxPayerAssetViewModel pObjAssetModel)
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

                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetTypeID = (int)EnumList.AssetTypes.Building,
                    AssetID = pObjAssetModel.AssetID,
                    BuildingUnitID = pObjAssetModel.BuildingUnitID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Government,
                    TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID,
                    TaxPayerID = pObjAssetModel.TaxPayerID,
                    Active = true,
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjFuncResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

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
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Message = "Error occurred while mapping building to government";
                }
            }

            return Ok(mObjAPIResponse);
        }

        [HttpPost]
        [Route("AddBuildingToSpecial")]
        public IHttpActionResult AddBuildingToSpecial(TaxPayerAssetViewModel pObjAssetModel)
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

                MAP_TaxPayer_Asset mObjTaxPayerAsset = new MAP_TaxPayer_Asset()
                {
                    AssetTypeID = (int)EnumList.AssetTypes.Building,
                    AssetID = pObjAssetModel.AssetID,
                    BuildingUnitID = pObjAssetModel.BuildingUnitID,
                    TaxPayerTypeID = (int)EnumList.TaxPayerType.Special,
                    TaxPayerRoleID = pObjAssetModel.TaxPayerRoleID,
                    TaxPayerID = pObjAssetModel.TaxPayerID,
                    Active = true,
                   CreatedBy = userId.HasValue ? userId : 22,
                    CreatedDate = CommUtil.GetCurrentDateTime()
                };

                try
                {

                    FuncResponse mObjFuncResponse = new BLTaxPayerAsset().BL_InsertTaxPayerAsset(mObjTaxPayerAsset);

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
                    Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
                    mObjAPIResponse.Success = false;
                    mObjAPIResponse.Message = "Error occurred while mapping building to Special";
                }
            }

            return Ok(mObjAPIResponse);
        }
    }
}
