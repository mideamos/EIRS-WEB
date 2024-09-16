using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLAwarenessCategory
    {
        IAwarenessCategoryRepository _AwarenessCategoryRepository;

        public BLAwarenessCategory()
        {
            _AwarenessCategoryRepository = new AwarenessCategoryRepository();
        }

        public IList<usp_GetAwarenessCategoryList_Result> BL_GetAwarenessCategoryList(MST_AwarenessCategory pObjAwarenessCategory)
        {
            return _AwarenessCategoryRepository.REP_GetAwarenessCategoryList(pObjAwarenessCategory);
        }

        public FuncResponse BL_InsertUpdateAwarenessCategory(MST_AwarenessCategory pObjAwarenessCategory)
        {
            return _AwarenessCategoryRepository.REP_InsertUpdateAwarenessCategory(pObjAwarenessCategory);
        }

        public usp_GetAwarenessCategoryList_Result BL_GetAwarenessCategoryDetails(MST_AwarenessCategory pObjAwarenessCategory)
        {
            return _AwarenessCategoryRepository.REP_GetAwarenessCategoryDetails(pObjAwarenessCategory);
        }

        public IList<DropDownListResult> BL_GetAwarenessCategoryDropDownList(MST_AwarenessCategory pObjAwarenessCategory)
        {
            return _AwarenessCategoryRepository.REP_GetAwarenessCategoryDropDownList(pObjAwarenessCategory);
        }

    }
}
