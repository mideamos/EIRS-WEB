using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Repository
{
   public interface IDAOutputRepository
    {
       FuncResponse REP_InsertUpdateDAOutput(DAOutput pObjDAOutput);
        IList<usp_GetDAOutputList_Result> REP_GetOutputList(DAOutput pObjDAOutput);
    }
}
