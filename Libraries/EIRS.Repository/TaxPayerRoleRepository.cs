using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class TaxPayerRoleRepository : ITaxPayerRoleRepository
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertUpdateTaxPayerRole(TaxPayer_Roles pObjTaxPayerRole)
        {
            using (_db = new EIRSEntities())
            {
                TaxPayer_Roles mObjInsertUpdateTaxPayerRole; //Tax Payer Role Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from tprol in _db.TaxPayer_Roles
                                       where tprol.TaxPayerRoleName == pObjTaxPayerRole.TaxPayerRoleName && tprol.AssetTypeID == pObjTaxPayerRole.AssetTypeID && tprol.TaxPayerTypeID == pObjTaxPayerRole.TaxPayerTypeID && tprol.TaxPayerRoleID != pObjTaxPayerRole.TaxPayerRoleID
                                       select tprol);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Tax Payer Role already exists";
                    return mObjFuncResponse;
                }

                //If Update Load Tax Payer Role
                if (pObjTaxPayerRole.TaxPayerRoleID != 0)
                {
                    mObjInsertUpdateTaxPayerRole = (from tprol in _db.TaxPayer_Roles
                                                 where tprol.TaxPayerRoleID == pObjTaxPayerRole.TaxPayerRoleID
                                                 select tprol).FirstOrDefault();

                    if (mObjInsertUpdateTaxPayerRole != null)
                    {
                        mObjInsertUpdateTaxPayerRole.ModifiedBy = pObjTaxPayerRole.ModifiedBy;
                        mObjInsertUpdateTaxPayerRole.ModifiedDate = pObjTaxPayerRole.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateTaxPayerRole = new TaxPayer_Roles();
                        mObjInsertUpdateTaxPayerRole.CreatedBy = pObjTaxPayerRole.CreatedBy;
                        mObjInsertUpdateTaxPayerRole.CreatedDate = pObjTaxPayerRole.CreatedDate;
                    }
                }
                else // Else Insert Tax Payer Role
                {
                    mObjInsertUpdateTaxPayerRole = new TaxPayer_Roles();
                    mObjInsertUpdateTaxPayerRole.CreatedBy = pObjTaxPayerRole.CreatedBy;
                    mObjInsertUpdateTaxPayerRole.CreatedDate = pObjTaxPayerRole.CreatedDate;
                }

                mObjInsertUpdateTaxPayerRole.AssetTypeID = pObjTaxPayerRole.AssetTypeID;
                mObjInsertUpdateTaxPayerRole.TaxPayerTypeID = pObjTaxPayerRole.TaxPayerTypeID;
                mObjInsertUpdateTaxPayerRole.TaxPayerRoleName = pObjTaxPayerRole.TaxPayerRoleName;
                mObjInsertUpdateTaxPayerRole.IsMultiLinkable = pObjTaxPayerRole.IsMultiLinkable;
                mObjInsertUpdateTaxPayerRole.Active = pObjTaxPayerRole.Active;

                if (pObjTaxPayerRole.TaxPayerRoleID == 0)
                {
                    _db.TaxPayer_Roles.Add(mObjInsertUpdateTaxPayerRole);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjTaxPayerRole.TaxPayerRoleID == 0)
                        mObjFuncResponse.Message = "Tax Payer Role Added Successfully";
                    else
                        mObjFuncResponse.Message = "Tax Payer Role Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;

                    if (pObjTaxPayerRole.TaxPayerRoleID == 0)
                        mObjFuncResponse.Message = "Tax Payer Role Addition Failed";
                    else
                        mObjFuncResponse.Message = "Tax Payer Role Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetTaxPayerRoleList_Result> REP_GetTaxPayerRoleList(TaxPayer_Roles pObjTaxPayerRole)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerRoleList(pObjTaxPayerRole.TaxPayerRoleName, pObjTaxPayerRole.TaxPayerRoleID,pObjTaxPayerRole.AssetTypeID,pObjTaxPayerRole.TaxPayerTypeID, pObjTaxPayerRole.TaxPayerRoleIds, pObjTaxPayerRole.intStatus, pObjTaxPayerRole.IncludeTaxPayerRoleIds, pObjTaxPayerRole.ExcludeTaxPayerRoleIds).ToList();
            }
        }

        public usp_GetTaxPayerRoleList_Result REP_GetTaxPayerRoleDetails(TaxPayer_Roles pObjTaxPayerRole)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerRoleList(pObjTaxPayerRole.TaxPayerRoleName, pObjTaxPayerRole.TaxPayerRoleID, pObjTaxPayerRole.AssetTypeID, pObjTaxPayerRole.TaxPayerTypeID, pObjTaxPayerRole.TaxPayerRoleIds, pObjTaxPayerRole.intStatus, pObjTaxPayerRole.IncludeTaxPayerRoleIds, pObjTaxPayerRole.ExcludeTaxPayerRoleIds).FirstOrDefault();
            }
        }

        public IList<DropDownListResult> REP_GetTaxPayerRoleDropDownList(TaxPayer_Roles pObjTaxPayerRole)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from tprol in _db.usp_GetTaxPayerRoleList(pObjTaxPayerRole.TaxPayerRoleName, pObjTaxPayerRole.TaxPayerRoleID, pObjTaxPayerRole.AssetTypeID, pObjTaxPayerRole.TaxPayerTypeID, pObjTaxPayerRole.TaxPayerRoleIds, pObjTaxPayerRole.intStatus, pObjTaxPayerRole.IncludeTaxPayerRoleIds, pObjTaxPayerRole.ExcludeTaxPayerRoleIds)
                               select new DropDownListResult()
                               {
                                   id = tprol.TaxPayerRoleID.GetValueOrDefault(),
                                   text = tprol.TaxPayerRoleName
                               }).ToList();

                return vResult;
            }
        }

        public FuncResponse REP_UpdateStatus(TaxPayer_Roles pObjTaxPayerRole)
        {
            using (_db = new EIRSEntities())
            {
                TaxPayer_Roles mObjInsertUpdateTaxPayerRole; //Tax Payer Role Insert Update Object
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //If Update Load TaxPayerRole
                if (pObjTaxPayerRole.TaxPayerRoleID != 0)
                {
                    mObjInsertUpdateTaxPayerRole = (from tprol in _db.TaxPayer_Roles
                                                 where tprol.TaxPayerRoleID == pObjTaxPayerRole.TaxPayerRoleID
                                                 select tprol).FirstOrDefault();

                    if (mObjInsertUpdateTaxPayerRole != null)
                    {
                        mObjInsertUpdateTaxPayerRole.Active = !mObjInsertUpdateTaxPayerRole.Active;
                        mObjInsertUpdateTaxPayerRole.ModifiedBy = pObjTaxPayerRole.ModifiedBy;
                        mObjInsertUpdateTaxPayerRole.ModifiedDate = pObjTaxPayerRole.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Tax Payer Role Updated Successfully";
                            mObjFuncResponse.AdditionalData = _db.usp_GetTaxPayerRoleList(pObjTaxPayerRole.TaxPayerRoleName, 0, pObjTaxPayerRole.AssetTypeID, pObjTaxPayerRole.TaxPayerTypeID, pObjTaxPayerRole.TaxPayerRoleIds, pObjTaxPayerRole.intStatus, pObjTaxPayerRole.IncludeTaxPayerRoleIds, pObjTaxPayerRole.ExcludeTaxPayerRoleIds).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Tax Payer Role Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<DropDownListResult> REP_GetAssetTypeDropDownList(TaxPayer_Roles pObjTaxPayerRole)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from tprol in _db.TaxPayer_Roles
                               join atype in _db.Asset_Types on tprol.AssetTypeID equals atype.AssetTypeID
                               where tprol.TaxPayerRoleID == pObjTaxPayerRole.TaxPayerRoleID && tprol.TaxPayerTypeID == pObjTaxPayerRole.TaxPayerTypeID
                               select new DropDownListResult()
                               {
                                   id = atype.AssetTypeID,
                                   text = atype.AssetTypeName
                               }).Distinct().ToList();

                return vResult;
            }
        }
    }
}
