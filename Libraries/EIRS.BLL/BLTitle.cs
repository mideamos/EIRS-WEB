using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLTitle
    {
        ITitleRepository _TitleRepository;

        public BLTitle()
        {
            _TitleRepository = new TitleRepository();
        }

        public IList<usp_GetTitleList_Result> BL_GetTitleList(Title pObjTitle)
        {
            return _TitleRepository.REP_GetTitleList(pObjTitle);
        }

        public FuncResponse BL_InsertUpdateTitle(Title pObjTitle)
        {
            return _TitleRepository.REP_InsertUpdateTitle(pObjTitle);
        }

        public usp_GetTitleList_Result BL_GetTitleDetails(Title pObjTitle)
        {
            return _TitleRepository.REP_GetTitleDetails(pObjTitle);
        }

        public IList<DropDownListResult> BL_GetTitleDropDownList(Title pObjTitle)
        {
            return _TitleRepository.REP_GetTitleDropDownList(pObjTitle);
        }

        public FuncResponse BL_UpdateStatus(Title pObjTitle)
        {
            return _TitleRepository.REP_UpdateStatus(pObjTitle);
        }
    }
}
