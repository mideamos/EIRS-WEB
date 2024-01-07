using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLBusinessCategory
    {
        IBusinessCategoryRepository _BusinessCategoryRepository;

        public BLBusinessCategory()
        {
            _BusinessCategoryRepository = new BusinessCategoryRepository();
        }

        public IList<usp_GetBusinessCategoryList_Result> BL_GetBusinessCategoryList(Business_Category pObjBusinessCategory)
        {
            return _BusinessCategoryRepository.REP_GetBusinessCategoryList(pObjBusinessCategory);
        }

        public FuncResponse BL_InsertUpdateBusinessCategory(Business_Category pObjBusinessCategory)
        {
            return _BusinessCategoryRepository.REP_InsertUpdateBusinessCategory(pObjBusinessCategory);
        }

        public usp_GetBusinessCategoryList_Result BL_GetBusinessCategoryDetails(Business_Category pObjBusinessCategory)
        {
            return _BusinessCategoryRepository.REP_GetBusinessCategoryDetails(pObjBusinessCategory);
        }

        public IList<DropDownListResult> BL_GetBusinessCategoryDropDownList(Business_Category pObjBusinessCategory)
        {
            return _BusinessCategoryRepository.REP_GetBusinessCategoryDropDownList(pObjBusinessCategory);
        }

        public FuncResponse BL_UpdateStatus(Business_Category pObjBusinessCategory)
        {
            return _BusinessCategoryRepository.REP_UpdateStatus(pObjBusinessCategory);
        }
    }
}
