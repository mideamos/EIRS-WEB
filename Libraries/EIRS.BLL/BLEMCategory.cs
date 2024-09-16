using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLEMCategory
    {
        readonly IEM_CategoryRepository _CategoryRepository;

        public BLEMCategory()
        {
            _CategoryRepository = new EM_CategoryRepository();
        }

        public IList<usp_EM_GetCategoryList_Result> BL_GetCategoryList(EM_Category pObjCategory)
        {
            return _CategoryRepository.REP_GetCategoryList(pObjCategory);
        }

        public FuncResponse BL_InsertUpdateCategory(EM_Category pObjCategory)
        {
            return _CategoryRepository.REP_InsertUpdateCategory(pObjCategory);
        }

        public usp_EM_GetCategoryList_Result BL_GetCategoryDetails(EM_Category pObjCategory)
        {
            return _CategoryRepository.REP_GetCategoryDetails(pObjCategory);
        }

        public IList<DropDownListResult> BL_GetCategoryDropDownList(EM_Category pObjCategory)
        {
            return _CategoryRepository.REP_GetCategoryDropDownList(pObjCategory);
        }

        public FuncResponse BL_UpdateStatus(EM_Category pObjCategory)
        {
            return _CategoryRepository.REP_UpdateStatus(pObjCategory);
        }
    }
}
