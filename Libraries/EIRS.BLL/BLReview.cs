using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLReview
    {
        IReviewRepository _ReviewRepository;

        public BLReview()
        {
            _ReviewRepository = new ReviewRepository();
        }

        public IList<usp_GetTaxPayerReviewNotes_Result> BL_GetReviewNotes(MAP_TaxPayer_Review pObjTaxPayerReview)
        {
            return _ReviewRepository.REP_GetReviewNotes(pObjTaxPayerReview);
        }

        public FuncResponse BL_InsertTaxPayerReview(MAP_TaxPayer_Review pObjTaxPayerReview)
        {
            return _ReviewRepository.REP_InsertTaxPayerReview(pObjTaxPayerReview);
        }

    }
}
