using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IBusinessCategoryRepository
    {
        usp_GetBusinessCategoryList_Result REP_GetBusinessCategoryDetails(Business_Category pObjBusinessCategory);
        IList<DropDownListResult> REP_GetBusinessCategoryDropDownList(Business_Category pObjBusinessCategory);
        IList<usp_GetBusinessCategoryList_Result> REP_GetBusinessCategoryList(Business_Category pObjBusinessCategory);
        FuncResponse REP_InsertUpdateBusinessCategory(Business_Category pObjBusinessCategory);
        FuncResponse REP_UpdateStatus(Business_Category pObjBusinessCategory);
    }
}