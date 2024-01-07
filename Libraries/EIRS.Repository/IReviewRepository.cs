using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IReviewRepository
    {
        IList<usp_GetTaxPayerReviewNotes_Result> REP_GetReviewNotes(MAP_TaxPayer_Review pObjTaxPayerReview);
        FuncResponse REP_InsertTaxPayerReview(MAP_TaxPayer_Review pObjTaxPayerReview);
    }
}