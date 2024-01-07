using System.Collections.Generic;
using EIRS.BOL;
using static EIRS.Repository.DataControlRepository;

namespace EIRS.Repository
{
    public interface IDataControlRepository
    {
        IList<usp_DC_GetAssessmentItemWithoutRule_Result> REP_GetAssessmentItemWithoutRule();
        IList<usp_DC_GetCompanyWithoutAssessment_Result> REP_GetCompanyWithoutAssessment();
        IList<usp_DC_GetIndividualWithoutAssessment_Result> REP_GetIndividualWithoutAssessment();
        IList<usp_DC_GetProfileWithoutRule_Result> REP_GetProfileWithoutRule();
        IList<AssessmentAndItemRollOver> GetAssessmentAndItem();
        IList<AssessmentRuleRollover> GetAssessmentAndRule();
        IList<usp_DC_GetTaxPayerWithoutAsset_Result> REP_GetTaxPayerWithoutAsset(string pStrTaxPayerTypeID, string pStrAssetTypeID);
    }
}