using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace EIRS.Repository
{
    public class TaxPayerAssetRespoistory : ITaxPayerAssetRespoistory
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertTaxPayerAsset(MAP_TaxPayer_Asset pObjTaxPayerAsset)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                var vExists = (from tpa in _db.MAP_TaxPayer_Asset
                               where tpa.AssetTypeID == pObjTaxPayerAsset.AssetTypeID && tpa.AssetID == pObjTaxPayerAsset.AssetID
                                  && tpa.TaxPayerTypeID == pObjTaxPayerAsset.TaxPayerTypeID && tpa.TaxPayerID == pObjTaxPayerAsset.TaxPayerID
                                  && tpa.TaxPayerRoleID == pObjTaxPayerAsset.TaxPayerRoleID && tpa.Active == true
                               select tpa);

                if (vExists.Count() > 0)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Link Already Exists";
                    return mObjResponse;
                }

                _db.MAP_TaxPayer_Asset.Add(pObjTaxPayerAsset);

                try
                {
                    _db.SaveChanges();

                    var context = ((IObjectContextAdapter)_db).ObjectContext;
                    var refreshableObjects = _db.ChangeTracker.Entries().Select(c => c.Entity).ToList();
                    context.Refresh(RefreshMode.StoreWins, refreshableObjects);

                    var vTPAList = (from tpa in _db.MAP_TaxPayer_Asset
                                    where tpa.AssetID == pObjTaxPayerAsset.AssetID && tpa.AssetTypeID == pObjTaxPayerAsset.AssetTypeID
                                    select tpa).ToList();

                    foreach (var item in vTPAList)
                    {

                        //Delete Profile Information
                        var vDeleteTaxPayerAssetProfile = (from tpap in _db.MAP_TaxPayer_Asset_Profile
                                                           where tpap.TPAID == item.TPAID
                                                           select tpap);

                        _db.MAP_TaxPayer_Asset_Profile.RemoveRange(vDeleteTaxPayerAssetProfile);

                        _db.SaveChanges();

                        //Update Profile
                        _db.usp_InsertProfileInformation(item.TPAID);
                    }

                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = Ex.Message;
                }

                return mObjResponse;
            }
        }

        public FuncResponse REP_UpdateTaxPayerAsset(MAP_TaxPayer_Asset pObjTaxPayerAsset)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                MAP_TaxPayer_Asset mObjUpdateTaxPayerAsset = _db.MAP_TaxPayer_Asset.Where(t => t.TPAID == pObjTaxPayerAsset.TPAID).FirstOrDefault();

                if (mObjUpdateTaxPayerAsset != null)
                {
                    mObjUpdateTaxPayerAsset.TaxPayerRoleID = pObjTaxPayerAsset.TaxPayerRoleID;
                    mObjUpdateTaxPayerAsset.ModifiedBy = pObjTaxPayerAsset.ModifiedBy;
                    mObjUpdateTaxPayerAsset.ModifiedDate = pObjTaxPayerAsset.ModifiedDate;

                    //Delete Profile Information

                    var vDeleteTaxPayerAssetProfile = (from tpap in _db.MAP_TaxPayer_Asset_Profile
                                                       where tpap.TPAID == pObjTaxPayerAsset.TPAID
                                                       select tpap);

                    _db.MAP_TaxPayer_Asset_Profile.RemoveRange(vDeleteTaxPayerAssetProfile);

                    try
                    {
                        _db.SaveChanges();

                        mObjResponse.Success = true;
                    }
                    catch (Exception Ex)
                    {
                        mObjResponse.Success = false;
                        mObjResponse.Message = Ex.Message;
                    }

                    return mObjResponse;


                }
                else
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = "Link Record Couldn't Find";
                    return mObjResponse;
                }
            }
        }

        public FuncResponse<IList<usp_GetTaxPayerAssetList_Result>> REP_RemoveTaxPayerAsset(MAP_TaxPayer_Asset pObjTaxPayerAsset)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<IList<usp_GetTaxPayerAssetList_Result>> mObjFuncResponse = new FuncResponse<IList<usp_GetTaxPayerAssetList_Result>>(); //Return Object

                MAP_TaxPayer_Asset mObjDeleteAsset;

                mObjDeleteAsset = _db.MAP_TaxPayer_Asset.Find(pObjTaxPayerAsset.TPAID);

                if (mObjDeleteAsset == null)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Asset Already Removed.";
                }
                else
                {
                    _db.MAP_TaxPayer_Asset.Remove(mObjDeleteAsset);

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                        if (pObjTaxPayerAsset.AssetTypeID != null)
                        {
                            mObjFuncResponse.Message = "Tax Payer Removed Successfully";
                        }
                        else
                        {
                            mObjFuncResponse.Message = "Asset Removed Successfully";
                        }

                        mObjFuncResponse.AdditionalData = _db.usp_GetTaxPayerAssetList(pObjTaxPayerAsset.TaxPayerID, pObjTaxPayerAsset.TaxPayerTypeID, pObjTaxPayerAsset.AssetTypeID, pObjTaxPayerAsset.AssetID, pObjTaxPayerAsset.TPAID, 0, 2).ToList();
                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = Ex.Message;
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetTaxPayerAssetList_Result> REP_GetTaxPayerAssetList(MAP_TaxPayer_Asset pObjTaxPayerAsset)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerAssetList(pObjTaxPayerAsset.TaxPayerID, pObjTaxPayerAsset.TaxPayerTypeID, pObjTaxPayerAsset.AssetTypeID, pObjTaxPayerAsset.AssetID, pObjTaxPayerAsset.TPAID, pObjTaxPayerAsset.ProfileID, 2).ToList();
            }
        }

        public IList<DropDownListResult> REP_GetTaxPayerAssetDropDownList(MAP_TaxPayer_Asset pObjTaxPayerAsset)
        {
            using (_db = new EIRSEntities())
            {

                var vResult = (from tpa in _db.usp_GetTaxPayerAssetList(pObjTaxPayerAsset.TaxPayerID, pObjTaxPayerAsset.TaxPayerTypeID, pObjTaxPayerAsset.AssetTypeID, pObjTaxPayerAsset.AssetID, pObjTaxPayerAsset.TPAID, pObjTaxPayerAsset.ProfileID, 2)
                               where tpa.Active == true
                               select new DropDownListResult()
                               {
                                   id = tpa.AssetID.GetValueOrDefault(),
                                   text = tpa.AssetRIN + " - " + tpa.AssetName
                               }).ToList();

                return vResult;
            }
        }

        public MAP_TaxPayer_Asset REP_GetTaxPayerAssetDetails(long pIntTPAID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.MAP_TaxPayer_Asset.Find(pIntTPAID);
            }
        }

        public FuncResponse<IList<usp_GetTaxPayerAssetList_Result>> REP_UpdateTaxPayerAssetStatus(MAP_TaxPayer_Asset pObjTaxPayerAsset)
        {
            using (_db = new EIRSEntities())
            {
                MAP_TaxPayer_Asset mObjUpdateAsset; //Individual Insert Update Object
                FuncResponse<IList<usp_GetTaxPayerAssetList_Result>> mObjFuncResponse = new FuncResponse<IList<usp_GetTaxPayerAssetList_Result>>(); //Return Object

                //If Update Load Individual
                if (pObjTaxPayerAsset.TPAID != 0)
                {
                    mObjUpdateAsset = (from tpa in _db.MAP_TaxPayer_Asset
                                       where tpa.TPAID == pObjTaxPayerAsset.TPAID
                                       select tpa).FirstOrDefault();

                    if (mObjUpdateAsset != null)
                    {
                        mObjUpdateAsset.Active = !mObjUpdateAsset.Active;
                        mObjUpdateAsset.ModifiedBy = pObjTaxPayerAsset.ModifiedBy;
                        mObjUpdateAsset.ModifiedDate = pObjTaxPayerAsset.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            if (pObjTaxPayerAsset.AssetTypeID != null)
                            {
                                mObjFuncResponse.Message = "Tax Payer Updated Successfully";
                            }
                            else
                            {
                                mObjFuncResponse.Message = "Asset Updated Successfully";
                            }

                            mObjFuncResponse.AdditionalData = _db.usp_GetTaxPayerAssetList(pObjTaxPayerAsset.TaxPayerID, pObjTaxPayerAsset.TaxPayerTypeID, pObjTaxPayerAsset.AssetTypeID, pObjTaxPayerAsset.AssetID, 0, pObjTaxPayerAsset.ProfileID, pObjTaxPayerAsset.IntStatus).ToList();

                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Asset Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_CheckAssetAlreadyLinked(MAP_TaxPayer_Asset pObjTaxPayerAsset)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();

                var vResult = (from tpa in _db.MAP_TaxPayer_Asset
                               where tpa.TaxPayerTypeID == pObjTaxPayerAsset.TaxPayerTypeID
                               && tpa.AssetTypeID == pObjTaxPayerAsset.AssetTypeID
                               && tpa.TaxPayerRoleID == pObjTaxPayerAsset.TaxPayerRoleID
                               && tpa.AssetID == pObjTaxPayerAsset.AssetID
                               && tpa.Active == true
                               select tpa);

                if (vResult.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                }
                else
                {
                    mObjFuncResponse.Success = true;
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetProfileInformation_Result> REP_GetTaxPayerProfileInformation(int pIntTaxPayerTypeID, int pIntTaxPayerID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetProfileInformation(pIntTaxPayerID, pIntTaxPayerTypeID).ToList();
            }
        }

        public IList<DropDownListResult> REP_GetTaxPayerProfileDropDownList(int pIntTaxPayerTypeID, int pIntTaxPayerID, int pIntAssetID, int pIntAssetTypeID)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from prof in _db.usp_GetProfileInformation(pIntTaxPayerID, pIntTaxPayerTypeID)
                               where prof.AssetID == pIntAssetID && prof.AssetTypeID == pIntAssetTypeID
                               select new DropDownListResult()
                               {
                                   id = prof.ProfileID.GetValueOrDefault(),
                                   text = prof.ProfileRefNo + " - " + prof.ProfileDescription
                               }).ToList();

                return vResult;

            }
        }

        public IList<usp_GetAssessmentRuleInformation_Result> REP_GetTaxPayerAssessmentRuleInformation(int pIntTaxPayerTypeID, int pIntTaxPayerID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentRuleInformation(pIntTaxPayerID, pIntTaxPayerTypeID).ToList();
            }
        }

        public IList<usp_GetAssessmentRuleForAssessment_Result> REP_GetTaxPayerAssessmentRuleForAssessment(int pIntTaxPayerTypeID, int pIntTaxPayerID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentRuleForAssessment(pIntTaxPayerID, pIntTaxPayerTypeID).ToList();
            }
        }

        public IList<DropDownListResult> REP_GetTaxPayerAssessmentRuleDropDownList(int pIntTaxPayerTypeID, int pIntTaxPayerID, int pIntProfileID, int pIntAssetID, int pIntAssetTypeID)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from arule in _db.usp_GetAssessmentRuleInformation(pIntTaxPayerID, pIntTaxPayerTypeID)
                               where arule.ProfileID == pIntProfileID && arule.AssetID == pIntAssetID && arule.AssetTypeID == pIntAssetTypeID
                               select new DropDownListResult()
                               {
                                   id = arule.AssessmentRuleID.GetValueOrDefault(),
                                   text = arule.AssessmentRuleCode + " - " + arule.AssessmentRuleName
                               }).ToList();

                return vResult;
            }
        }

        public IList<usp_SearchTaxPayer_Result> REP_SearchTaxPayer(SearchTaxPayerFilter pObjSearchTaxPayerFilter)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_SearchTaxPayer(pObjSearchTaxPayerFilter.TaxPayerRIN, pObjSearchTaxPayerFilter.MobileNumber, pObjSearchTaxPayerFilter.AssetName, pObjSearchTaxPayerFilter.TaxPayerName, pObjSearchTaxPayerFilter.TaxPayerTypeID, pObjSearchTaxPayerFilter.TaxPayerTIN, pObjSearchTaxPayerFilter.TaxOfficeID, pObjSearchTaxPayerFilter.intSearchType).ToList();
            }
        }

        public usp_GetTaxPayerDetails_Result REP_GetTaxPayerDetails(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerDetails(pIntTaxPayerID, pIntTaxPayerTypeID).FirstOrDefault();
            }
        }

        public IList<usp_GetTaxPayerBuildingList_Result> REP_GetTaxPayerBuildingList(MAP_TaxPayer_Asset pObjTaxPayerAsset)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerBuildingList(pObjTaxPayerAsset.TaxPayerID, pObjTaxPayerAsset.TaxPayerTypeID).ToList();
            }
        }

        public IList<usp_GetTaxPayerBusinessList_Result> REP_GetTaxPayerBusinessList(MAP_TaxPayer_Asset pObjTaxPayerAsset)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerBusinessList(pObjTaxPayerAsset.TaxPayerID, pObjTaxPayerAsset.TaxPayerTypeID).ToList();
            }
        }

        public IList<usp_GetTaxPayerLandList_Result> REP_GetTaxPayerLandList(MAP_TaxPayer_Asset pObjTaxPayerAsset)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerLandList(pObjTaxPayerAsset.TaxPayerID, pObjTaxPayerAsset.TaxPayerTypeID).ToList();
            }
        }

        public IList<usp_GetTaxPayerVehicleList_Result> REP_GetTaxPayerVehicleList(MAP_TaxPayer_Asset pObjTaxPayerAsset)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerVehicleList(pObjTaxPayerAsset.TaxPayerID, pObjTaxPayerAsset.TaxPayerTypeID).ToList();
            }
        }

        public IList<DropDownListResult> REP_GetTaxPayerProfileDropDown(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from tpap in _db.MAP_TaxPayer_Asset_Profile
                               join tpa in _db.MAP_TaxPayer_Asset on tpap.TPAID equals tpa.TPAID
                               join prf in _db.Profiles on tpap.ProfileID equals prf.ProfileID
                               where tpa.TaxPayerID == pIntTaxPayerID && tpa.TaxPayerTypeID == pIntTaxPayerTypeID && tpa.Active == true
                               select new DropDownListResult()
                               {
                                   id = prf.ProfileID,
                                   text = prf.ProfileReferenceNo + " - " + prf.ProfileDescription
                               }).ToList();

                return vResult;
            }
        }

        public IList<DropDownListResult> REP_GetTaxPayerProfileDropDownForCertificate(int pIntTaxPayerID, int pIntTaxPayerTypeID, int pIntCertificateTypeID)
        {
            using (_db = new EIRSEntities())
            {
                var vResult = (from prof in _db.usp_GetProfileDropDownForCertificate(pIntTaxPayerID, pIntTaxPayerTypeID, pIntCertificateTypeID)
                               select new DropDownListResult()
                               {
                                   id = prof.ProfileID,
                                   text = prof.ProfileName
                               }).ToList();

                return vResult;
            }
        }

        public IList<usp_GetPAYEAssessmentRuleInformation_Result> REP_GetPAYEAssessmentRuleInformation(int pIntTaxPayerTypeID, int pIntTaxPayerID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetPAYEAssessmentRuleInformation(pIntTaxPayerID, pIntTaxPayerTypeID).ToList();
            }
        }

        public IList<usp_GetPAYEProfileInformation_Result> REP_GetPAYEProfileInformation(int pIntTaxPayerTypeID, int pIntTaxPayerID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetPAYEProfileInformation(pIntTaxPayerID, pIntTaxPayerTypeID).ToList();
            }
        }

        public IList<usp_GetPAYEEmployerStaff_Result> REP_GetPAYEEmployerStaff(int pIntAssetID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetPAYEEmployerStaff(pIntAssetID).ToList();
            }
        }
    }
}
