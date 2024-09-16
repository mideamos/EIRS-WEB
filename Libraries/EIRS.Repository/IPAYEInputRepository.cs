using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IPAYEInputRepository
    {
        IList<usp_GetPAYEInputList_Result> REP_GetPAYEInputList(PAYEInput pObjPAYEInput);
        FuncResponse REP_InertUpdatePAYEInput(PAYEInput pObjPAYEInput);
    }
}