using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Repository
{
   public interface  IDAInputRepository
    {
        IList<usp_GetDAInputList_Result> REP_GetDAInputList(DAInput pObjDAInput);
        FuncResponse REP_InertUpdateDAInput(DAInput pObjDAInput);
    }
}
