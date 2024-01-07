using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLTaxPayerAsset
    {
        ITaxPayerAssetRespoistory _TaxPayerAssetRespoistory;

        public BLTaxPayerAsset()
        {
            _TaxPayerAssetRespoistory = new TaxPayerAssetRespoistory();
        }

        public FuncResponse BL_InsertTaxPayerAsset(MAP_TaxPayer_Asset pObjTaxPayerAsset)
        {
            FuncResponse mObjFuncResponse = _TaxPayerAssetRespoistory.REP_InsertTaxPayerAsset(pObjTaxPayerAsset);

            if (mObjFuncResponse.Success)
            {
                usp_GetTaxPayerAssetList_Result mObjTaxPayerAssetData = (usp_GetTaxPayerAssetList_Result)mObjFuncResponse.AdditionalData;

                if (mObjTaxPayerAssetData != null && GlobalDefaultValues.SendNotification)
                {
                    //Send Notification
                    EmailDetails mObjEmailDetails = new EmailDetails()
                    {
                        TaxPayerTypeID = mObjTaxPayerAssetData.TaxPayerTypeID.GetValueOrDefault(),
                        TaxPayerTypeName = mObjTaxPayerAssetData.TaxPayerTypeName,
                        TaxPayerID = mObjTaxPayerAssetData.TaxPayerID.GetValueOrDefault(),
                        TaxPayerName = mObjTaxPayerAssetData.TaxPayerName,
                        TaxPayerRIN = mObjTaxPayerAssetData.TaxPayerRINNumber,
                        TaxPayerRoleName = mObjTaxPayerAssetData.TaxPayerRoleName,
                        AssetName = mObjTaxPayerAssetData.AssetName,
                        AssetLGA = mObjTaxPayerAssetData.AssetLGA,
                        AssetRIN = mObjTaxPayerAssetData.AssetRIN,
                        AssetTypeName = mObjTaxPayerAssetData.AssetTypeName,
                        TaxPayerMobileNumber = mObjTaxPayerAssetData.TaxPayerMobileNumber,
                        TaxPayerEmail = mObjTaxPayerAssetData.TaxPayerEmailAddress,
                        LoggedInUserID = pObjTaxPayerAsset.CreatedBy,
                    };

                    if (!string.IsNullOrWhiteSpace(mObjTaxPayerAssetData.TaxPayerEmailAddress))
                    {
                        BLEmailHandler.BL_AssetProfileLinked(mObjEmailDetails);
                    }

                    if (!string.IsNullOrWhiteSpace(mObjTaxPayerAssetData.TaxPayerMobileNumber))
                    {
                        BLSMSHandler.BL_AssetProfileLinked(mObjEmailDetails);
                    }
                }

            }

            return mObjFuncResponse;
        }

        public FuncResponse BL_UpdateTaxPayerAsset(MAP_TaxPayer_Asset pObjTaxPayerAsset)
        {
            return _TaxPayerAssetRespoistory.REP_UpdateTaxPayerAsset(pObjTaxPayerAsset);
        }

        public FuncResponse<IList<usp_GetTaxPayerAssetList_Result>> BL_RemoveTaxPayerAsset(MAP_TaxPayer_Asset pObjTaxPayerAsset)
        {
            return _TaxPayerAssetRespoistory.REP_RemoveTaxPayerAsset(pObjTaxPayerAsset);
        }

        public IList<usp_GetTaxPayerAssetList_Result> BL_GetTaxPayerAssetList(MAP_TaxPayer_Asset pObjTaxPayerAsset)
        {
            return _TaxPayerAssetRespoistory.REP_GetTaxPayerAssetList(pObjTaxPayerAsset);
        }

        public MAP_TaxPayer_Asset BL_GetTaxPayerAssetDetails(long pIntTPAID)
        {
            return _TaxPayerAssetRespoistory.REP_GetTaxPayerAssetDetails(pIntTPAID);
        }

        public FuncResponse<IList<usp_GetTaxPayerAssetList_Result>> BL_UpdateTaxPayerAssetStatus(MAP_TaxPayer_Asset pObjTaxPayerAsset)
        {
            return _TaxPayerAssetRespoistory.REP_UpdateTaxPayerAssetStatus(pObjTaxPayerAsset);
        }

        public FuncResponse BL_CheckAssetAlreadyLinked(MAP_TaxPayer_Asset pObjTaxPayerAsset)
        {
            return _TaxPayerAssetRespoistory.REP_CheckAssetAlreadyLinked(pObjTaxPayerAsset);
        }

        public IList<usp_GetProfileInformation_Result> BL_GetTaxPayerProfileInformation(int pIntTaxPayerTypeID, int pIntTaxPayerID)
        {
            return _TaxPayerAssetRespoistory.REP_GetTaxPayerProfileInformation(pIntTaxPayerTypeID, pIntTaxPayerID);
        }

        public IList<usp_GetAssessmentRuleInformation_Result> BL_GetTaxPayerAssessmentRuleInformation(int pIntTaxPayerTypeID, int pIntTaxPayerID)
        {
            return _TaxPayerAssetRespoistory.REP_GetTaxPayerAssessmentRuleInformation(pIntTaxPayerTypeID, pIntTaxPayerID);
        }

        public IList<usp_GetAssessmentRuleForAssessment_Result> BL_GetTaxPayerAssessmentRuleForAssessment(int pIntTaxPayerTypeID, int pIntTaxPayerID)
        {
            return _TaxPayerAssetRespoistory.REP_GetTaxPayerAssessmentRuleForAssessment(pIntTaxPayerTypeID, pIntTaxPayerID);
        }

        public IList<DropDownListResult> BL_GetTaxPayerAssetDropDownList(MAP_TaxPayer_Asset pObjTaxPayerAsset)
        {
            return _TaxPayerAssetRespoistory.REP_GetTaxPayerAssetDropDownList(pObjTaxPayerAsset);
        }

        public IList<DropDownListResult> BL_GetTaxPayerProfileDropDownList(int pIntTaxPayerTypeID, int pIntTaxPayerID, int pIntAssetID, int pIntAssetTypeID)
        {
            return _TaxPayerAssetRespoistory.REP_GetTaxPayerProfileDropDownList(pIntTaxPayerTypeID, pIntTaxPayerID, pIntAssetID, pIntAssetTypeID);
        }

        public IList<DropDownListResult> BL_GetTaxPayerAssessmentRuleDropDownList(int pIntTaxPayerTypeID, int pIntTaxPayerID, int pIntProfileID, int pIntAssetID, int pIntAssetTypeID)
        {
            return _TaxPayerAssetRespoistory.REP_GetTaxPayerAssessmentRuleDropDownList(pIntTaxPayerTypeID, pIntTaxPayerID, pIntProfileID, pIntAssetID, pIntAssetTypeID);
        }

        public IList<usp_SearchTaxPayer_Result> BL_SearchTaxPayer(SearchTaxPayerFilter pObjSearchTaxPayerFilter)
        {
            return _TaxPayerAssetRespoistory.REP_SearchTaxPayer(pObjSearchTaxPayerFilter);
        }

        public IList<usp_GetTaxPayerBuildingList_Result> BL_GetTaxPayerBuildingList(MAP_TaxPayer_Asset pObjTaxPayerAsset)
        {
            return _TaxPayerAssetRespoistory.REP_GetTaxPayerBuildingList(pObjTaxPayerAsset);
        }

        public IList<usp_GetTaxPayerBusinessList_Result> BL_GetTaxPayerBusinessList(MAP_TaxPayer_Asset pObjTaxPayerAsset)
        {
            return _TaxPayerAssetRespoistory.REP_GetTaxPayerBusinessList(pObjTaxPayerAsset);
        }

        public IList<usp_GetTaxPayerLandList_Result> BL_GetTaxPayerLandList(MAP_TaxPayer_Asset pObjTaxPayerAsset)
        {
            return _TaxPayerAssetRespoistory.REP_GetTaxPayerLandList(pObjTaxPayerAsset);
        }

        public IList<usp_GetTaxPayerVehicleList_Result> BL_GetTaxPayerVehicleList(MAP_TaxPayer_Asset pObjTaxPayerAsset)
        {
            return _TaxPayerAssetRespoistory.REP_GetTaxPayerVehicleList(pObjTaxPayerAsset);
        }

        public IList<DropDownListResult> BL_GetTaxPayerProfileDropDown(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            return _TaxPayerAssetRespoistory.REP_GetTaxPayerProfileDropDown(pIntTaxPayerID, pIntTaxPayerTypeID);
        }

        public usp_GetTaxPayerDetails_Result BL_GetTaxPayerDetails(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            return _TaxPayerAssetRespoistory.REP_GetTaxPayerDetails(pIntTaxPayerID, pIntTaxPayerTypeID);
        }

        public IList<DropDownListResult> BL_GetTaxPayerProfileDropDownForCertificate(int pIntTaxPayerID, int pIntTaxPayerTypeID, int pIntCertificateTypeID)
        {
            return _TaxPayerAssetRespoistory.REP_GetTaxPayerProfileDropDownForCertificate(pIntTaxPayerID, pIntTaxPayerTypeID, pIntCertificateTypeID);
        }

        public IList<usp_GetPAYEAssessmentRuleInformation_Result> BL_GetPAYEAssessmentRuleInformation(int pIntTaxPayerTypeID, int pIntTaxPayerID)
        {
            return _TaxPayerAssetRespoistory.REP_GetPAYEAssessmentRuleInformation(pIntTaxPayerTypeID, pIntTaxPayerID);
        }

        public IList<usp_GetPAYEProfileInformation_Result> BL_GetPAYEProfileInformation(int pIntTaxPayerTypeID, int pIntTaxPayerID)
        {
            return _TaxPayerAssetRespoistory.REP_GetPAYEProfileInformation(pIntTaxPayerTypeID, pIntTaxPayerID);
        }

        public IList<usp_GetPAYEEmployerStaff_Result> BL_GetPAYEEmployerStaff(int pIntAssetID)
        {
            return _TaxPayerAssetRespoistory.REP_GetPAYEEmployerStaff(pIntAssetID);
        }
    }
}
