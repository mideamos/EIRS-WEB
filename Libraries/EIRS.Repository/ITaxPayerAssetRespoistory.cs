using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ITaxPayerAssetRespoistory
    {
        MAP_TaxPayer_Asset REP_GetTaxPayerAssetDetails(long pIntTPAID);
        IList<usp_GetTaxPayerAssetList_Result> REP_GetTaxPayerAssetList(MAP_TaxPayer_Asset pObjTaxPayerAsset);
        FuncResponse REP_InsertTaxPayerAsset(MAP_TaxPayer_Asset pObjTaxPayerAsset);

        FuncResponse REP_UpdateTaxPayerAsset(MAP_TaxPayer_Asset pObjTaxPayerAsset);
        FuncResponse<IList<usp_GetTaxPayerAssetList_Result>> REP_RemoveTaxPayerAsset(MAP_TaxPayer_Asset pObjTaxPayerAsset);
        FuncResponse<IList<usp_GetTaxPayerAssetList_Result>> REP_UpdateTaxPayerAssetStatus(MAP_TaxPayer_Asset pObjTaxPayerAsset);

        FuncResponse REP_CheckAssetAlreadyLinked(MAP_TaxPayer_Asset pObjTaxPayerAsset);

        IList<usp_GetProfileInformation_Result> REP_GetTaxPayerProfileInformation(int pIntTaxPayerTypeID, int pIntTaxPayerID);

        IList<usp_GetAssessmentRuleInformation_Result> REP_GetTaxPayerAssessmentRuleInformation(int pIntTaxPayerTypeID, int pIntTaxPayerID);

        IList<DropDownListResult> REP_GetTaxPayerAssetDropDownList(MAP_TaxPayer_Asset pObjTaxPayerAsset);

        IList<DropDownListResult> REP_GetTaxPayerProfileDropDownList(int pIntTaxPayerTypeID, int pIntTaxPayerID, int pIntAssetID, int pIntAssetTypeID);

        IList<DropDownListResult> REP_GetTaxPayerAssessmentRuleDropDownList(int pIntTaxPayerTypeID, int pIntTaxPayerID, int pIntProfileID, int pIntAssetID, int pIntAssetTypeID);

        IList<usp_GetAssessmentRuleForAssessment_Result> REP_GetTaxPayerAssessmentRuleForAssessment(int pIntTaxPayerTypeID, int pIntTaxPayerID);

        IList<usp_SearchTaxPayer_Result> REP_SearchTaxPayer(SearchTaxPayerFilter pObjSearchTaxPayerFilter);

        IList<usp_GetTaxPayerBuildingList_Result> REP_GetTaxPayerBuildingList(MAP_TaxPayer_Asset pObjTaxPayerAsset);
        IList<usp_GetTaxPayerBusinessList_Result> REP_GetTaxPayerBusinessList(MAP_TaxPayer_Asset pObjTaxPayerAsset);
        IList<usp_GetTaxPayerLandList_Result> REP_GetTaxPayerLandList(MAP_TaxPayer_Asset pObjTaxPayerAsset);
        IList<usp_GetTaxPayerVehicleList_Result> REP_GetTaxPayerVehicleList(MAP_TaxPayer_Asset pObjTaxPayerAsset);

        IList<DropDownListResult> REP_GetTaxPayerProfileDropDown(int pIntTaxPayerID, int pIntTaxPayerTypeID);
        usp_GetTaxPayerDetails_Result REP_GetTaxPayerDetails(int pIntTaxPayerID, int pIntTaxPayerTypeID);
        IList<DropDownListResult> REP_GetTaxPayerProfileDropDownForCertificate(int pIntTaxPayerID, int pIntTaxPayerTypeID, int pIntCertificateTypeID);

        IList<usp_GetPAYEAssessmentRuleInformation_Result> REP_GetPAYEAssessmentRuleInformation(int pIntTaxPayerTypeID, int pIntTaxPayerID);

        IList<usp_GetPAYEProfileInformation_Result> REP_GetPAYEProfileInformation(int pIntTaxPayerTypeID, int pIntTaxPayerID);
        IList<usp_GetPAYEEmployerStaff_Result> REP_GetPAYEEmployerStaff(int pIntAssetID);
    }
}