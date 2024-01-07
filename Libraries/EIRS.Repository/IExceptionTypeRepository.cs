using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IExceptionTypeRepository
    {
        usp_GetExceptionTypeList_Result REP_GetExceptionTypeDetails(Exception_Type pObjExceptionType);
        IList<usp_GetExceptionTypeList_Result> REP_GetExceptionTypeList(Exception_Type pObjExceptionType);
        FuncResponse REP_InsertUpdateExceptionType(Exception_Type pObjExceptionType);
        FuncResponse REP_UpdateStatus(Exception_Type pObjExceptionType);
    }
}