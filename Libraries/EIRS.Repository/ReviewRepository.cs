using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public class ReviewRepository : IReviewRepository
    {
        EIRSEntities _db;

        public IList<usp_GetTaxPayerReviewNotes_Result> REP_GetReviewNotes(MAP_TaxPayer_Review pObjTaxPayerReview)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerReviewNotes(pObjTaxPayerReview.TaxPayerTypeID,pObjTaxPayerReview.TaxPayerID).ToList();
            }
        }

        public FuncResponse REP_InsertTaxPayerReview(MAP_TaxPayer_Review pObjTaxPayerReview)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();


                MAP_TaxPayer_Review mObjInsertUpdateTaxPayerReview;
                mObjInsertUpdateTaxPayerReview = new MAP_TaxPayer_Review();
                mObjInsertUpdateTaxPayerReview.TaxPayerTypeID = pObjTaxPayerReview.TaxPayerTypeID;
                mObjInsertUpdateTaxPayerReview.TaxPayerID = pObjTaxPayerReview.TaxPayerID;
                mObjInsertUpdateTaxPayerReview.ReviewDate = pObjTaxPayerReview.ReviewDate;
                mObjInsertUpdateTaxPayerReview.Notes = pObjTaxPayerReview.Notes;
                mObjInsertUpdateTaxPayerReview.ReviewStatusID = pObjTaxPayerReview.ReviewStatusID;
                mObjInsertUpdateTaxPayerReview.UserID = pObjTaxPayerReview.UserID;

                mObjInsertUpdateTaxPayerReview.CreatedBy = pObjTaxPayerReview.CreatedBy;
                mObjInsertUpdateTaxPayerReview.CreatedDate = pObjTaxPayerReview.CreatedDate;

                _db.MAP_TaxPayer_Review.Add(mObjInsertUpdateTaxPayerReview);

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.AdditionalData = mObjInsertUpdateTaxPayerReview;
                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.Message = "Tax Payer Review Added Successfully";

                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = ex;
                    mObjFuncResponse.Message = "Tax Payer Review Addition Failed";

                }

                return mObjFuncResponse;
            }
        }
    }
}
