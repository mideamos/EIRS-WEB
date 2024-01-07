using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.BLL
{
    public class BLCertificate
    {
        ICertificateRepository _CertificateRepository;

        public BLCertificate()
        {
            _CertificateRepository = new CertificateRepository();
        }
        public FuncResponse<Certificate> BL_InsertCertificate(Certificate pObjCertificate)
        {
            return _CertificateRepository.REP_InsertCertificate(pObjCertificate);
        }

        public FuncResponse BL_UpdateCertificate(Certificate pObjCertificate)
        {
            return _CertificateRepository.REP_UpdateCertificate(pObjCertificate);
        }

        public usp_GetCertificateDetails_Result BL_GetCertificateDetails(long plngCertificateID)
        {
            return _CertificateRepository.REP_GetCertificateDetails(plngCertificateID);
        }

        public IDictionary<string, object> BL_SearchCertificate(Certificate pObjCertificate)
        {
            return _CertificateRepository.REP_SearchCertificate(pObjCertificate);
        }

        public FuncResponse BL_InsertUpdateCertificateField(MAP_Certificate_CustomField pObjCustomField)
        {
            return _CertificateRepository.REP_InsertUpdateCertificateField(pObjCustomField);
        }

        public IList<usp_GetCertificateFieldList_Result> BL_GetCertificateField(Certificate pObjCertificate)
        {
            return _CertificateRepository.REP_GetCertificateField(pObjCertificate);
        }

        public IList<usp_GetCertificateItemList_Result> BL_GetCertificateItem(Certificate pObjCertificate)
        {
            return _CertificateRepository.REP_GetCertificateItem(pObjCertificate);
        }

        public void BL_UpdateCertificatePath(Certificate pObjCertificate)
        {
            _CertificateRepository.REP_UpdateCertificatePath(pObjCertificate);
        }

        public IList<usp_GetAssessmentRuleInformationForCertificate_Result> BL_GetAssessmentRuleInformationForCertificate(Certificate pObjCertificate)
        {
            return _CertificateRepository.REP_GetAssessmentRuleInformationForCertificate(pObjCertificate);
        }

        public IList<usp_GetAdminCertificateStageList_Result> BL_GetCertificateStageList(long plngCertificateID)
        {
            return _CertificateRepository.REP_GetCertificateStageList(plngCertificateID);
        }

        public usp_GetCertificateDetailForGenerate_Result BL_GetCertificateDetailForGenerateProcess(long plngCertificateID)
        {
            return _CertificateRepository.REP_GetCertificateDetailForGenerateProcess(plngCertificateID);
        }

        public MAP_Certificate_Generate BL_GetCertificateGenerateDetails(long plngCertificateID)
        {
            return _CertificateRepository.REP_GetCertificateGenerateDetails(plngCertificateID);
        }

        public FuncResponse<MAP_Certificate_Generate> BL_InsertUpdateCertificateGenerate(MAP_Certificate_Generate pObjCertificateGenerate)
        {
            return _CertificateRepository.REP_InsertUpdateCertificateGenerate(pObjCertificateGenerate);
        }

        public FuncResponse BL_InsertUpdateGenerateField(MAP_Certificate_Generate_Field pObjGenerateField)
        {
            return _CertificateRepository.REP_InsertUpdateGenerateField(pObjGenerateField);
        }

        public MAP_Certificate_Validate BL_GetCertificateValidateDetails(long plngCertificateID)
        {
            return _CertificateRepository.REP_GetCertificateValidateDetails(plngCertificateID);
        }

        public FuncResponse<MAP_Certificate_Validate> BL_InsertUpdateCertificateValidate(MAP_Certificate_Validate pObjCertificateValidate)
        {
            return _CertificateRepository.REP_InsertUpdateCertificateValidate(pObjCertificateValidate);
        }

        public MAP_Certificate_Seal BL_GetCertificateSealDetails(long plngCertificateID)
        {
            return _CertificateRepository.REP_GetCertificateSealDetails(plngCertificateID);
        }

        public FuncResponse<MAP_Certificate_Seal> BL_InsertUpdateCertificateSeal(MAP_Certificate_Seal pObjCertificateSeal)
        {
            return _CertificateRepository.REP_InsertUpdateCertificateSeal(pObjCertificateSeal);
        }

        public MAP_Certificate_Issue BL_GetCertificateIssueDetails(long plngCertificateID)
        {
            return _CertificateRepository.REP_GetCertificateIssueDetails(plngCertificateID);
        }

        public FuncResponse<MAP_Certificate_Issue> BL_InsertUpdateCertificateIssue(MAP_Certificate_Issue pObjCertificateIssue)
        {
            return _CertificateRepository.REP_InsertUpdateCertificateIssue(pObjCertificateIssue);
        }

        public MAP_Certificate_SignVisible BL_GetCertificateSignVisibleDetails(long plngCertificateID, int UserID, int StageID = 0)
        {
            return _CertificateRepository.REP_GetCertificateSignVisibleDetails(plngCertificateID, UserID, StageID);
        }

        public FuncResponse<MAP_Certificate_SignVisible> BL_InsertUpdateCertificateSignVisible(MAP_Certificate_SignVisible pObjCertificateSignVisible)
        {
            return _CertificateRepository.REP_InsertUpdateCertificateSignVisible(pObjCertificateSignVisible);
        }

        public MAP_Certificate_SignDigital BL_GetCertificateSignDigitalDetails(long plngCertificateID)
        {
            return _CertificateRepository.REP_GetCertificateSignDigitalDetails(plngCertificateID);
        }

        public FuncResponse<MAP_Certificate_SignDigital> BL_InsertUpdateCertificateSignDigital(MAP_Certificate_SignDigital pObjCertificateSignDigital)
        {
            return _CertificateRepository.REP_InsertUpdateCertificateSignDigital(pObjCertificateSignDigital);
        }

        public IList<MAP_Certificate_SignVisible> BL_GetCertificateSignVisibleList(long plngCertificateID)
        {
            return _CertificateRepository.REP_GetCertificateSignVisibleList(plngCertificateID);
        }

        public FuncResponse BL_RevokeCertificate(MAP_Certificate_Revoke pObjRevoke)
        {
            return _CertificateRepository.REP_RevokeCertificate(pObjRevoke);
        }

    }
}
