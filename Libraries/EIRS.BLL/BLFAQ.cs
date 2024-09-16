using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLFAQ
    {
        IFAQRepository _FAQRepository;

        public BLFAQ()
        {
            _FAQRepository = new FAQRepository();
        }

        public IList<usp_GetFAQList_Result> BL_GetFAQList(MST_FAQ pObjFAQ)
        {
            return _FAQRepository.REP_GetFAQList(pObjFAQ);
        }

        public FuncResponse BL_InsertUpdateFAQ(MST_FAQ pObjFAQ)
        {
            return _FAQRepository.REP_InsertUpdateFAQ(pObjFAQ);
        }

        public usp_GetFAQList_Result BL_GetFAQDetails(MST_FAQ pObjFAQ)
        {
            return _FAQRepository.REP_GetFAQDetails(pObjFAQ);
        }
    }
}
