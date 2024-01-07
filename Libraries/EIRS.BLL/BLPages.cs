using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EIRS.BLL
{
    public class BLPages
    {
        IPagesRepository _PagesRepository;

        public BLPages()
        {
            _PagesRepository = new PagesRepository();
        }

        public FuncResponse BL_UpdatePages(MST_Pages pObjPage)
        {
            return _PagesRepository.REP_UpdatePages(pObjPage);
        }

        public IList<usp_GetPageList_Result> BL_SearchPages(MST_Pages pObjPage)
        {
            return _PagesRepository.REP_SearchPages(pObjPage);
        }

        public MST_Pages BL_GetPageDetails(MST_Pages pObjPage)
        {
            return _PagesRepository.REP_GetPageDetails(pObjPage);
        }

        public usp_GetPageList_Result BL_GetPageDetails(int mIntPageID)
        {
            return _PagesRepository.REP_GetPageDetails(mIntPageID);
        }
    }
}
