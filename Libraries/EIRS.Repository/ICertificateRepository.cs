using EIRS.BOL;
using EIRS.Common;
using System.Collections.Generic;

namespace EIRS.Repository
{
    public interface ICertificateRepository
    {
        MAP_Certificate_Generate REP_GetCertificateGenerateDetails(long plngCertificateID);
        usp_GetCertificateDetailForGenerate_Result REP_GetCertificateDetailForGenerateProcess(long plngCertificateID);
        IList<usp_GetAssessmentRuleInformationForCertificate_Result> REP_GetAssessmentRuleInformationForCertificate(Certificate pObjCertificate);
        usp_GetCertificateDetails_Result REP_GetCertificateDetails(long plngCertificateID);
        IList<usp_GetCertificateFieldList_Result> REP_GetCertificateField(Certificate pObjCertificate);
        MAP_Certificate_Issue REP_GetCertificateIssueDetails(long plngCertificateID);
        IList<usp_GetCertificateItemList_Result> REP_GetCertificateItem(Certificate pObjCertificate);
        MAP_Certificate_Seal REP_GetCertificateSealDetails(long plngCertificateID);
        MAP_Certificate_SignDigital REP_GetCertificateSignDigitalDetails(long plngCertificateID);
        MAP_Certificate_SignVisible REP_GetCertificateSignVisibleDetails(long plngCertificateID, int UserID, int StageID = 0);
        IList<MAP_Certificate_SignVisible> REP_GetCertificateSignVisibleList(long plngCertificateID);
        IList<usp_GetAdminCertificateStageList_Result> REP_GetCertificateStageList(long plngCertificateID);
        MAP_Certificate_Validate REP_GetCertificateValidateDetails(long plngCertificateID);
        FuncResponse<Certificate> REP_InsertCertificate(Certificate pObjCertificate);
        FuncResponse REP_InsertUpdateCertificateField(MAP_Certificate_CustomField pObjCustomField);
        FuncResponse<MAP_Certificate_Generate> REP_InsertUpdateCertificateGenerate(MAP_Certificate_Generate pObjCertificateGenerate);
        FuncResponse<MAP_Certificate_Issue> REP_InsertUpdateCertificateIssue(MAP_Certificate_Issue pObjCertificateIssue);
        FuncResponse<MAP_Certificate_Seal> REP_InsertUpdateCertificateSeal(MAP_Certificate_Seal pObjCertificateSeal);
        FuncResponse<MAP_Certificate_SignDigital> REP_InsertUpdateCertificateSignDigital(MAP_Certificate_SignDigital pObjCertificateSignDigital);
        FuncResponse<MAP_Certificate_SignVisible> REP_InsertUpdateCertificateSignVisible(MAP_Certificate_SignVisible pObjCertificateSignVisible);
        FuncResponse<MAP_Certificate_Validate> REP_InsertUpdateCertificateValidate(MAP_Certificate_Validate pObjCertificateValidate);
        FuncResponse REP_InsertUpdateGenerateField(MAP_Certificate_Generate_Field pObjGenerateField);
        IDictionary<string, object> REP_SearchCertificate(Certificate pObjCertificate);
        FuncResponse REP_UpdateCertificate(Certificate pObjCertificate);
        void REP_UpdateCertificatePath(Certificate pObjCertificate);
        FuncResponse REP_UpdateRequestStatus(Certificate pObjCertificate);
        FuncResponse REP_RevokeCertificate(MAP_Certificate_Revoke pObjRevoke);
    }
}