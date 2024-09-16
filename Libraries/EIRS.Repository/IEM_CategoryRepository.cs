using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IEM_CategoryRepository
    {
        usp_EM_GetCategoryList_Result REP_GetCategoryDetails(EM_Category pObjCategory);
        IList<DropDownListResult> REP_GetCategoryDropDownList(EM_Category pObjCategory);
        IList<usp_EM_GetCategoryList_Result> REP_GetCategoryList(EM_Category pObjCategory);
        FuncResponse REP_InsertUpdateCategory(EM_Category pObjCategory);
        FuncResponse REP_UpdateStatus(EM_Category pObjCategory);
    }
}