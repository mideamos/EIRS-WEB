using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
   public class BLPAYEInput
    {
        IPAYEInputRepository _PAYEInputRepository;

        public BLPAYEInput()
        {
            _PAYEInputRepository = new PAYEInputRepository();
        }

        public FuncResponse BL_InertUpdatePAYEInput(PAYEInput pObjPAYEInput)
        {
            return _PAYEInputRepository.REP_InertUpdatePAYEInput(pObjPAYEInput);
        }

        public IList<usp_GetPAYEInputList_Result> BL_GetPAYEInputList(PAYEInput pObjPAYEInput)
        {
            return _PAYEInputRepository.REP_GetPAYEInputList(pObjPAYEInput);
        }
    }
}
