using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;


namespace EIRS.Repository
{
    public class FAQRepository : IFAQRepository
    {
        ERASEntities _db;

        public FuncResponse REP_InsertUpdateFAQ(MST_FAQ pObjFAQ)
        {
            using (_db = new ERASEntities())
            {
                MST_FAQ mObjInsertUpdateFAQ; //FAQ Insert Update Object

                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                //Check if Duplicate
                var vDuplicateCheck = (from faq in _db.MST_FAQ
                                       where faq.FAQTitle == pObjFAQ.FAQTitle && faq.AwarenessCategoryID == pObjFAQ.AwarenessCategoryID && faq.FAQID != pObjFAQ.FAQID
                                       select faq);

                if (vDuplicateCheck.Count() > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "FAQ already exists";
                    return mObjFuncResponse;
                }

                //If Update Load FAQ
                if (pObjFAQ.FAQID != 0)
                {
                    mObjInsertUpdateFAQ = (from faq in _db.MST_FAQ
                                            where faq.FAQID == pObjFAQ.FAQID
                                            select faq).FirstOrDefault();

                    if (mObjInsertUpdateFAQ != null)
                    {
                        mObjInsertUpdateFAQ.ModifiedBy = pObjFAQ.ModifiedBy;
                        mObjInsertUpdateFAQ.ModifiedDate = pObjFAQ.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateFAQ = new MST_FAQ();
                        mObjInsertUpdateFAQ.CreatedBy = pObjFAQ.CreatedBy;
                        mObjInsertUpdateFAQ.CreatedDate = pObjFAQ.CreatedDate;
                    }
                }
                else // Else Insert FAQ
                {
                    mObjInsertUpdateFAQ = new MST_FAQ();
                    mObjInsertUpdateFAQ.CreatedBy = pObjFAQ.CreatedBy;
                    mObjInsertUpdateFAQ.CreatedDate = pObjFAQ.CreatedDate;
                }

                mObjInsertUpdateFAQ.AwarenessCategoryID = pObjFAQ.AwarenessCategoryID;
                mObjInsertUpdateFAQ.FAQTitle = pObjFAQ.FAQTitle;
                mObjInsertUpdateFAQ.FAQText = pObjFAQ.FAQText;
                mObjInsertUpdateFAQ.Active = pObjFAQ.Active;

                if (pObjFAQ.FAQID == 0)
                {
                    _db.MST_FAQ.Add(mObjInsertUpdateFAQ);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    if (pObjFAQ.FAQID == 0)
                        mObjFuncResponse.Message = "FAQ Added Successfully";
                    else
                        mObjFuncResponse.Message = "FAQ Updated Successfully";

                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;
                    if (pObjFAQ.FAQID == 0)
                        mObjFuncResponse.Message = "FAQ Addition Failed";
                    else
                        mObjFuncResponse.Message = "FAQ Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetFAQList_Result> REP_GetFAQList(MST_FAQ pObjFAQ)
        {
            using (_db = new ERASEntities())
            {
                return _db.usp_GetFAQList(pObjFAQ.FAQTitle, pObjFAQ.FAQID, pObjFAQ.FAQIds, pObjFAQ.AwarenessCategoryIds, pObjFAQ.intStatus, pObjFAQ.IncludeFAQIds, pObjFAQ.ExcludeFAQIds).ToList();
            }
        }

        public usp_GetFAQList_Result REP_GetFAQDetails(MST_FAQ pObjFAQ)
        {
            using (_db = new ERASEntities())
            {
                return _db.usp_GetFAQList(pObjFAQ.FAQTitle, pObjFAQ.FAQID, pObjFAQ.FAQIds, pObjFAQ.AwarenessCategoryIds, pObjFAQ.intStatus, pObjFAQ.IncludeFAQIds, pObjFAQ.ExcludeFAQIds).FirstOrDefault();
            }
        }
    }
}
