using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.BLL
{
   public  class BLDAOutput
    {
        IDAOutputRepository _IDAOutputRepository;
        public BLDAOutput()
        {
            _IDAOutputRepository = new DAOutputRepository();
        }

       public FuncResponse BL_InsertUpdateDAOutput(DAOutput pObjDAOutput)
        {
            return _IDAOutputRepository.REP_InsertUpdateDAOutput(pObjDAOutput);
        }
        public IList<usp_GetDAOutputList_Result> BL_GetOutputList(DAOutput pObjDAOutput)
        {
            return _IDAOutputRepository.REP_GetOutputList(pObjDAOutput);
        }
    }
}
