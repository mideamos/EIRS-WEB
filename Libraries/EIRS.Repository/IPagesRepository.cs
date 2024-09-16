using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IPagesRepository
    {
        MST_Pages REP_GetPageDetails(MST_Pages pObjPage);
        IList<usp_GetPageList_Result> REP_SearchPages(MST_Pages pObjPage);
        FuncResponse REP_UpdatePages(MST_Pages pObjPage);

        usp_GetPageList_Result REP_GetPageDetails(int mIntPageID);
    }
}