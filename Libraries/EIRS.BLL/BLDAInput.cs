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
   public  class BLDAInput
    {
        IDAInputRepository _IDAInputRepository;
        public BLDAInput()
        {
            _IDAInputRepository = new DAInputRepository();
        }

        public IList<usp_GetDAInputList_Result> BL_GetDAInputList(DAInput pObjDAInput)
        {
            return _IDAInputRepository.REP_GetDAInputList(pObjDAInput);
        }
        public FuncResponse BL_InertUpdateDAInput(DAInput pObjDAInput)
        {
            return _IDAInputRepository.REP_InertUpdateDAInput(pObjDAInput);
        }
        
    }
}
