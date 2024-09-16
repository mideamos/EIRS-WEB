using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLTown
    {
        ITownRepository _TownRepository;

        public BLTown()
        {
            _TownRepository = new TownRepository();
        }

        public IList<usp_GetTownList_Result> BL_GetTownList(Town pObjTown)
        {
            return _TownRepository.REP_GetTownList(pObjTown);
        }

        public FuncResponse BL_InsertUpdateTown(Town pObjTown)
        {
            return _TownRepository.REP_InsertUpdateTown(pObjTown);
        }

        public usp_GetTownList_Result BL_GetTownDetails(Town pObjTown)
        {
            return _TownRepository.REP_GetTownDetails(pObjTown);
        }

        public IList<DropDownListResult> BL_GetTownDropDownList(Town pObjTown)
        {
            return _TownRepository.REP_GetTownDropDownList(pObjTown);
        }

        public FuncResponse BL_UpdateStatus(Town pObjTown)
        {
            return _TownRepository.REP_UpdateStatus(pObjTown);
        }
    }
}
