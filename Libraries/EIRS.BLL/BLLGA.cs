using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLLGA
    {
        ILGARepository _LGARepository;

        public BLLGA()
        {
            _LGARepository = new LGARepository();
        }

        public IList<usp_GetLGAList_Result> BL_GetLGAList(LGA pObjLGA)
        {
            return _LGARepository.REP_GetLGAList(pObjLGA);
        }

        public FuncResponse BL_InsertUpdateLGA(LGA pObjLGA)
        {
            return _LGARepository.REP_InsertUpdateLGA(pObjLGA);
        }

        public usp_GetLGAList_Result BL_GetLGADetails(LGA pObjLGA)
        {
            return _LGARepository.REP_GetLGADetails(pObjLGA);
        }

        public IList<DropDownListResult> BL_GetLGADropDownList(LGA pObjLGA)
        {
            return _LGARepository.REP_GetLGADropDownList(pObjLGA);
        }

        public IList<DropDownListResult> BL_GetLGAClassDropDownList()
        {
            return _LGARepository.REP_GetLGAClassDropDownList();
        }

        public FuncResponse BL_UpdateStatus(LGA pObjLGA)
        {
            return _LGARepository.REP_UpdateStatus(pObjLGA);
        }
    }
}
