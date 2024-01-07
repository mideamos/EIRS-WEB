using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLLandFunction
    {
        ILandFunctionRepository _LandFunctionRepository;

        public BLLandFunction()
        {
            _LandFunctionRepository = new LandFunctionRepository();
        }

        public IList<usp_GetLandFunctionList_Result> BL_GetLandFunctionList(Land_Function pObjLandFunction)
        {
            return _LandFunctionRepository.REP_GetLandFunctionList(pObjLandFunction);
        }

        public FuncResponse BL_InsertUpdateLandFunction(Land_Function pObjLandFunction)
        {
            return _LandFunctionRepository.REP_InsertUpdateLandFunction(pObjLandFunction);
        }

        public usp_GetLandFunctionList_Result BL_GetLandFunctionDetails(Land_Function pObjLandFunction)
        {
            return _LandFunctionRepository.REP_GetLandFunctionDetails(pObjLandFunction);
        }

        public IList<DropDownListResult> BL_GetLandFunctionDropDownList(Land_Function pObjLandFunction)
        {
            return _LandFunctionRepository.REP_GetLandFunctionDropDownList(pObjLandFunction);
        }

        public FuncResponse BL_UpdateStatus(Land_Function pObjLandFunction)
        {
            return _LandFunctionRepository.REP_UpdateStatus(pObjLandFunction);
        }
    }
}
