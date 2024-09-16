using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLProfile
    {
        IProfileRepository _ProfileRepository;

        public BLProfile()
        {
            _ProfileRepository = new ProfileRepository();
        }

        public IList<usp_GetProfileList_Result> BL_GetProfileList(Profile pObjProfile)
        {
            return _ProfileRepository.REP_GetProfileList(pObjProfile);
        }

        public FuncResponse<Profile> BL_InsertUpdateProfile(Profile pObjProfile)
        {
            return _ProfileRepository.REP_InsertUpdateProfile(pObjProfile);
        }

        public usp_GetProfileList_Result BL_GetProfileDetails(Profile pObjProfile)
        {
            return _ProfileRepository.REP_GetProfileDetails(pObjProfile);
        }

        public IList<DropDownListResult> BL_GetProfileDropDownList(Profile pObjProfile)
        {
            return _ProfileRepository.REP_GetProfileDropDownList(pObjProfile);
        }

        public FuncResponse BL_UpdateStatus(Profile pObjProfile)
        {
            return _ProfileRepository.REP_UpdateStatus(pObjProfile);
        }

        public FuncResponse BL_InsertProfileSector(ProfileSector pObjProfileSector)
        {
            return _ProfileRepository.REP_InsertProfileSector(pObjProfileSector);
        }

        public FuncResponse BL_RemoveProfileSector(ProfileSector pObjProfileSector)
        {
            return _ProfileRepository.REP_RemoveProfileSector(pObjProfileSector);
        }

        public IList<ProfileSector> BL_GetProfileSector(int pIntProfileID)
        {
            return _ProfileRepository.REP_GetProfileSector(pIntProfileID);
        }


        public FuncResponse BL_InsertProfileSubSector(ProfileSubSector pObjProfileSubSector)
        {
            return _ProfileRepository.REP_InsertProfileSubSector(pObjProfileSubSector);
        }

        public FuncResponse BL_RemoveProfileSubSector(ProfileSubSector pObjProfileSubSector)
        {
            return _ProfileRepository.REP_RemoveProfileSubSector(pObjProfileSubSector);
        }

        public IList<ProfileSubSector> BL_GetProfileSubSector(int pIntProfileID)
        {
            return _ProfileRepository.REP_GetProfileSubSector(pIntProfileID);
        }

        public FuncResponse BL_InsertProfileGroup(ProfileGroup pObjProfileGroup)
        {
            return _ProfileRepository.REP_InsertProfileGroup(pObjProfileGroup);
        }

        public FuncResponse BL_RemoveProfileGroup(ProfileGroup pObjProfileGroup)
        {
            return _ProfileRepository.REP_RemoveProfileGroup(pObjProfileGroup);
        }

        public IList<ProfileGroup> BL_GetProfileGroup(int pIntProfileID)
        {
            return _ProfileRepository.REP_GetProfileGroup(pIntProfileID);
        }


        public FuncResponse BL_InsertProfileSubGroup(ProfileSubGroup pObjProfileSubGroup)
        {
            return _ProfileRepository.REP_InsertProfileSubGroup(pObjProfileSubGroup);
        }

        public FuncResponse BL_RemoveProfileSubGroup(ProfileSubGroup pObjProfileSubGroup)
        {
            return _ProfileRepository.REP_RemoveProfileSubGroup(pObjProfileSubGroup);
        }

        public IList<ProfileSubGroup> BL_GetProfileSubGroup(int pIntProfileID)
        {
            return _ProfileRepository.REP_GetProfileSubGroup(pIntProfileID);
        }

        public FuncResponse BL_InsertProfileSectorElement(ProfileSectorElement pObjProfileSectorElement)
        {
            return _ProfileRepository.REP_InsertProfileSectorElement(pObjProfileSectorElement);
        }

        public FuncResponse BL_RemoveProfileSectorElement(ProfileSectorElement pObjProfileSectorElement)
        {
            return _ProfileRepository.REP_RemoveProfileSectorElement(pObjProfileSectorElement);
        }

        public IList<ProfileSectorElement> BL_GetProfileSectorElement(int pIntProfileID)
        {
            return _ProfileRepository.REP_GetProfileSectorElement(pIntProfileID);
        }

        public FuncResponse BL_InsertProfileSectorSubElement(ProfileSectorSubElement pObjProfileSectorSubElement)
        {
            return _ProfileRepository.REP_InsertProfileSectorSubElement(pObjProfileSectorSubElement);
        }

        public FuncResponse BL_RemoveProfileSectorSubElement(ProfileSectorSubElement pObjProfileSectorSubElement)
        {
            return _ProfileRepository.REP_RemoveProfileSectorSubElement(pObjProfileSectorSubElement);
        }

        public IList<ProfileSectorSubElement> BL_GetProfileSectorSubElement(int pIntProfileID)
        {
            return _ProfileRepository.REP_GetProfileSectorSubElement(pIntProfileID);
        }

        public FuncResponse BL_InsertProfileAttribute(ProfileAttribute pObjProfileAttribute)
        {
            return _ProfileRepository.REP_InsertProfileAttribute(pObjProfileAttribute);
        }

        public FuncResponse BL_RemoveProfileAttribute(ProfileAttribute pObjProfileAttribute)
        {
            return _ProfileRepository.REP_RemoveProfileAttribute(pObjProfileAttribute);
        }

        public IList<ProfileAttribute> BL_GetProfileAttribute(int pIntProfileID)
        {
            return _ProfileRepository.REP_GetProfileAttribute(pIntProfileID);
        }

        public FuncResponse BL_InsertProfileSubAttribute(ProfileSubAttribute pObjProfileSubAttribute)
        {
            return _ProfileRepository.REP_InsertProfileSubAttribute(pObjProfileSubAttribute);
        }

        public FuncResponse BL_RemoveProfileSubAttribute(ProfileSubAttribute pObjProfileSubAttribute)
        {
            return _ProfileRepository.REP_RemoveProfileSubAttribute(pObjProfileSubAttribute);
        }

        public IList<ProfileSubAttribute> BL_GetProfileSubAttribute(int pIntProfileID)
        {
            return _ProfileRepository.REP_GetProfileSubAttribute(pIntProfileID);
        }

        public FuncResponse BL_InsertProfileTaxPayerType(ProfileTaxPayerType pObjProfileTaxPayerType)
        {
            return _ProfileRepository.REP_InsertProfileTaxPayerType(pObjProfileTaxPayerType);
        }

        public FuncResponse BL_RemoveProfileTaxPayerType(ProfileTaxPayerType pObjProfileTaxPayerType)
        {
            return _ProfileRepository.REP_RemoveProfileTaxPayerType(pObjProfileTaxPayerType);
        }

        public IList<ProfileTaxPayerType> BL_GetProfileTaxPayerType(int pIntProfileID)
        {
            return _ProfileRepository.REP_GetProfileTaxPayerType(pIntProfileID);
        }

        public FuncResponse BL_InsertProfileTaxPayerRole(ProfileTaxPayerRole pObjProfileTaxPayerRole)
        {
            return _ProfileRepository.REP_InsertProfileTaxPayerRole(pObjProfileTaxPayerRole);
        }

        public FuncResponse BL_RemoveProfileTaxPayerRole(ProfileTaxPayerRole pObjProfileTaxPayerRole)
        {
            return _ProfileRepository.REP_RemoveProfileTaxPayerRole(pObjProfileTaxPayerRole);
        }

        public IList<usp_GetProfileData_Result> BL_GetProfileData(Profile pObjProfile)
        {
            return _ProfileRepository.REP_GetProfileData(pObjProfile);
        }

        public IList<usp_GetProfileTaxPayerData_Result> BL_GetProfileTaxPayerData(Profile pObjProfile)
        {
            return _ProfileRepository.REP_GetProfileTaxPayerData(pObjProfile);
        }

        public IList<usp_GetProfileAssetData_Result> BL_GetProfileAssetData(Profile pObjProfile)
        {
            return _ProfileRepository.REP_GetProfileAssetData(pObjProfile);
        }

        public IList<ProfileTaxPayerRole> BL_GetProfileTaxPayerRole(int pIntProfileID)
        {
            return _ProfileRepository.REP_GetProfileTaxPayerRole(pIntProfileID);
        }

        public IList<usp_GetCompanyPAYEAsset_Result> BL_GetCompanyPAYEAssetList(int pIntCompanyID)
        {
            return _ProfileRepository.REP_GetCompanyPAYEAssetList(pIntCompanyID);
        }


        public IList<usp_GetTaxPayerBasedOnProfileForSupplier_Result> BL_GetTaxPayerBasedOnProfileForSupplier(Profile pObjProfile)
        {
            return _ProfileRepository.REP_GetTaxPayerBasedOnProfileForSupplier(pObjProfile);
        }

        public IList<usp_GetBuildingBasedOnProfileForSupplier_Result> BL_GetBuildingBasedOnProfileForSupplier(Profile pObjProfile)
        {
            return _ProfileRepository.REP_GetBuildingBasedOnProfileForSupplier(pObjProfile);
        }

        public IList<usp_GetBusinessBasedOnProfileForSupplier_Result> BL_GetBusinessBasedOnProfileForSupplier(Profile pObjProfile)
        {
            return _ProfileRepository.REP_GetBusinessBasedOnProfileForSupplier(pObjProfile);
        }

        public IList<usp_GetLandBasedOnProfileForSupplier_Result> BL_GetLandBasedOnProfileForSupplier(Profile pObjProfile)
        {
            return _ProfileRepository.REP_GetLandBasedOnProfileForSupplier(pObjProfile);
        }

        public IList<usp_GetVehicleBasedOnProfileForSupplier_Result> BL_GetVehicleBasedOnProfileForSupplier(Profile pObjProfile)
        {
            return _ProfileRepository.REP_GetVehicleBasedOnProfileForSupplier(pObjProfile);
        }

        public IList<usp_GetTaxPayerAssetProfileBasedOnProfileForSupplier_Result> BL_GetTaxPayerAssetProfileBasedOnProfileForSupplier(Profile pObjProfile)
        {
            return _ProfileRepository.REP_GetTaxPayerAssetProfileBasedOnProfileForSupplier(pObjProfile);
        }

        public IList<usp_GetAssessmentRuleBasedOnProfileForSupplier_Result> BL_GetAssessmentRuleBasedOnProfileForSupplier(Profile pObjProfile)
        {
            return _ProfileRepository.REP_GetAssessmentRuleBasedOnProfileForSupplier(pObjProfile);
        }    
        public IList<usp_GetAssessmentRuleBasedOnProfileForSupplierNew_Result> BL_GetAssessmentRuleBasedOnProfileForSupplierNew(Profile pObjProfile,int year)
        {
            return _ProfileRepository.REP_GetAssessmentRuleBasedOnProfileForSupplierNew(pObjProfile,year);
        }
        public IList<Assessment_Rules> GetAssessment_Rules_Api()
        {
            return _ProfileRepository.GetAssessment_Rules_Api();
        }

        public IList<usp_GetAssessmentItemBasedOnProfileForSupplier_Result> BL_GetAssessmentItemBasedOnProfileForSupplier(Profile pObjProfile)
        {
            return _ProfileRepository.REP_GetAssessmentItemBasedOnProfileForSupplier(pObjProfile);
        }

        public IList<usp_GetAssessmentBasedOnProfileForSupplier_Result> BL_GetAssessmentBasedOnProfileForSupplier(Profile pObjProfile)
        {
            return _ProfileRepository.REP_GetAssessmentBasedOnProfileForSupplier(pObjProfile);
        }


        public IList<usp_SearchProfileForRDMLoad_Result> BL_SearchProfileDetails(Profile pObjProfile)
        {
            return _ProfileRepository.REP_SearchProfileDetails(pObjProfile);
        }
    }
}
