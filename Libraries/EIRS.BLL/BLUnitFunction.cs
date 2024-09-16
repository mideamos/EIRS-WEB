using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLUnitFunction
    {
        IUnitFunctionRepository _UnitFunctionRepository;

        public BLUnitFunction()
        {
            _UnitFunctionRepository = new UnitFunctionRepository();
        }

        public IList<usp_GetUnitFunctionList_Result> BL_GetUnitFunctionList(Unit_Function pObjUnitFunction)
        {
            return _UnitFunctionRepository.REP_GetUnitFunctionList(pObjUnitFunction);
        }

        public FuncResponse BL_InsertUpdateUnitFunction(Unit_Function pObjUnitFunction)
        {
            return _UnitFunctionRepository.REP_InsertUpdateUnitFunction(pObjUnitFunction);
        }

        public usp_GetUnitFunctionList_Result BL_GetUnitFunctionDetails(Unit_Function pObjUnitFunction)
        {
            return _UnitFunctionRepository.REP_GetUnitFunctionDetails(pObjUnitFunction);
        }

        public IList<DropDownListResult> BL_GetUnitFunctionDropDownList(Unit_Function pObjUnitFunction)
        {
            return _UnitFunctionRepository.REP_GetUnitFunctionDropDownList(pObjUnitFunction);
        }

        public FuncResponse BL_UpdateStatus(Unit_Function pObjUnitFunction)
        {
            return _UnitFunctionRepository.REP_UpdateStatus(pObjUnitFunction);
        }
    }
}
