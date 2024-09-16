using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IFAQRepository
    {
        usp_GetFAQList_Result REP_GetFAQDetails(MST_FAQ pObjFAQ);
        IList<usp_GetFAQList_Result> REP_GetFAQList(MST_FAQ pObjFAQ);
        FuncResponse REP_InsertUpdateFAQ(MST_FAQ pObjFAQ);
    }
}