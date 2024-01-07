using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IAwarenessCategoryRepository
    {
        usp_GetAwarenessCategoryList_Result REP_GetAwarenessCategoryDetails(MST_AwarenessCategory pObjAwarenessCategory);
        IList<DropDownListResult> REP_GetAwarenessCategoryDropDownList(MST_AwarenessCategory pObjAwarenessCategory);
        IList<usp_GetAwarenessCategoryList_Result> REP_GetAwarenessCategoryList(MST_AwarenessCategory pObjAwarenessCategory);
        FuncResponse REP_InsertUpdateAwarenessCategory(MST_AwarenessCategory pObjAwarenessCategory);
    }
}