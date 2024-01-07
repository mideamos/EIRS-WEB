using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;
using static EIRS.Repository.DataControlRepository;

namespace EIRS.BLL
{
    public class BLDataControl
    {
        IDataControlRepository _DataControlRepository;

        public BLDataControl()
        {
            _DataControlRepository = new DataControlRepository();
        }

        public IList<usp_DC_GetTaxPayerWithoutAsset_Result> BL_GetTaxPayerWithoutAsset(string pStrTaxPayerTypeID, string pStrAssetTypeID)
        {
            return _DataControlRepository.REP_GetTaxPayerWithoutAsset(pStrTaxPayerTypeID, pStrAssetTypeID);
        }

        public IList<usp_DC_GetProfileWithoutRule_Result> BL_GetProfileWithoutRule()
        {
            return _DataControlRepository.REP_GetProfileWithoutRule();
        }

        public IList<usp_DC_GetAssessmentItemWithoutRule_Result> BL_GetAssessmentItemWithoutRule()
        {
            return _DataControlRepository.REP_GetAssessmentItemWithoutRule();
        }

        public IList<usp_DC_GetIndividualWithoutAssessment_Result> BL_GetIndividualWithoutAssessment()
        {
            return _DataControlRepository.REP_GetIndividualWithoutAssessment();
        } 
        public IList<AssessmentAndItemRollOver> BL_GetAssessmentAndItem()
        {
            return _DataControlRepository.GetAssessmentAndItem();
        }
        public IList<AssessmentRuleRollover> BL_GetAssessmentAndRule()
        {
            return _DataControlRepository.GetAssessmentAndRule();
        }

        public IList<usp_DC_GetCompanyWithoutAssessment_Result> BL_GetCompanyWithoutAssessment()
        {
            return _DataControlRepository.REP_GetCompanyWithoutAssessment();
        }
    }
}
