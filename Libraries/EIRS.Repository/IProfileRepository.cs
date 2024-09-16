using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IProfileRepository
    {
        IList<ProfileAttribute> REP_GetProfileAttribute(int pIntProfileID);
        IList<Assessment_Rules> GetAssessment_Rules_Api();
        usp_GetProfileList_Result REP_GetProfileDetails(Profile pObjProfile);
        IList<DropDownListResult> REP_GetProfileDropDownList(Profile pObjProfile);
        IList<ProfileGroup> REP_GetProfileGroup(int pIntProfileID);
        IList<usp_GetProfileList_Result> REP_GetProfileList(Profile pObjProfile);
        IList<ProfileSector> REP_GetProfileSector(int pIntProfileID);
        IList<ProfileSectorElement> REP_GetProfileSectorElement(int pIntProfileID);
        IList<ProfileSectorSubElement> REP_GetProfileSectorSubElement(int pIntProfileID);
        IList<ProfileSubAttribute> REP_GetProfileSubAttribute(int pIntProfileID);
        IList<ProfileSubGroup> REP_GetProfileSubGroup(int pIntProfileID);
        IList<ProfileSubSector> REP_GetProfileSubSector(int pIntProfileID);
        IList<ProfileTaxPayerRole> REP_GetProfileTaxPayerRole(int pIntProfileID);
        IList<ProfileTaxPayerType> REP_GetProfileTaxPayerType(int pIntProfileID);
        FuncResponse REP_InsertProfileAttribute(ProfileAttribute pObjProfileAttribute);
        FuncResponse REP_InsertProfileGroup(ProfileGroup pObjProfileGroup);
        FuncResponse REP_InsertProfileSector(ProfileSector pObjProfileSector);
        FuncResponse REP_InsertProfileSectorElement(ProfileSectorElement pObjProfileSectorElement);
        FuncResponse REP_InsertProfileSectorSubElement(ProfileSectorSubElement pObjProfileSectorSubElement);
        FuncResponse REP_InsertProfileSubAttribute(ProfileSubAttribute pObjProfileSubAttribute);
        FuncResponse REP_InsertProfileSubGroup(ProfileSubGroup pObjProfileSubGroup);
        FuncResponse REP_InsertProfileSubSector(ProfileSubSector pObjProfileSubSector);
        FuncResponse REP_InsertProfileTaxPayerRole(ProfileTaxPayerRole pObjProfileTaxPayerRole);
        FuncResponse REP_InsertProfileTaxPayerType(ProfileTaxPayerType pObjProfileTaxPayerType);
        FuncResponse<Profile> REP_InsertUpdateProfile(Profile pObjProfile);
        FuncResponse REP_RemoveProfileAttribute(ProfileAttribute pObjProfileAttribute);
        FuncResponse REP_RemoveProfileGroup(ProfileGroup pObjProfileGroup);
        FuncResponse REP_RemoveProfileSector(ProfileSector pObjProfileSector);
        FuncResponse REP_RemoveProfileSectorElement(ProfileSectorElement pObjProfileSectorElement);
        FuncResponse REP_RemoveProfileSectorSubElement(ProfileSectorSubElement pObjProfileSectorSubElement);
        FuncResponse REP_RemoveProfileSubAttribute(ProfileSubAttribute pObjProfileSubAttribute);
        FuncResponse REP_RemoveProfileSubGroup(ProfileSubGroup pObjProfileSubGroup);
        FuncResponse REP_RemoveProfileSubSector(ProfileSubSector pObjProfileSubSector);
        FuncResponse REP_RemoveProfileTaxPayerRole(ProfileTaxPayerRole pObjProfileTaxPayerRole);
        FuncResponse REP_RemoveProfileTaxPayerType(ProfileTaxPayerType pObjProfileTaxPayerType);
        FuncResponse REP_UpdateStatus(Profile pObjProfile);

        IList<usp_GetProfileData_Result> REP_GetProfileData(Profile pObjProfile);
        IList<usp_GetProfileTaxPayerData_Result> REP_GetProfileTaxPayerData(Profile pObjProfile);
        IList<usp_GetProfileAssetData_Result> REP_GetProfileAssetData(Profile pObjProfile);

        IList<usp_GetCompanyPAYEAsset_Result> REP_GetCompanyPAYEAssetList(int pIntCompanyID);

        IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> REP_GetTaxPayerBasedOnProfileForSupplier(Profile pObjProfile);
        IList<usp_GetBuildingBasedOnProfileForSupplier_Result> REP_GetBuildingBasedOnProfileForSupplier(Profile pObjProfile);
        IList<usp_GetBusinessBasedOnProfileForSupplier_Result> REP_GetBusinessBasedOnProfileForSupplier(Profile pObjProfile);
        IList<usp_GetLandBasedOnProfileForSupplier_Result> REP_GetLandBasedOnProfileForSupplier(Profile pObjProfile);
        IList<usp_GetVehicleBasedOnProfileForSupplier_Result> REP_GetVehicleBasedOnProfileForSupplier(Profile pObjProfile);
        IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> REP_GetTaxPayerAssetProfileBasedOnProfileForSupplier(Profile pObjProfile);
        IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> REP_GetAssessmentRuleBasedOnProfileForSupplier(Profile pObjProfile);
        IList<usp_GetAssessmentRuleBasedOnProfileForSupplierNew_Result> REP_GetAssessmentRuleBasedOnProfileForSupplierNew(Profile pObjProfile,int year);
        IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> REP_GetAssessmentItemBasedOnProfileForSupplier(Profile pObjProfile);
        IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> REP_GetAssessmentBasedOnProfileForSupplier(Profile pObjProfile);


        IList<usp_SearchProfileForRDMLoad_Result> REP_SearchProfileDetails(Profile pObjProfile);



    }
}