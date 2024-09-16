using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ISizeRepository
    {
        usp_GetSizeList_Result REP_GetSizeDetails(Size pObjSize);
        IList<DropDownListResult> REP_GetSizeDropDownList(Size pObjSize);
        IList<usp_GetSizeList_Result> REP_GetSizeList(Size pObjSize);
        FuncResponse REP_InsertUpdateSize(Size pObjSize);
        FuncResponse REP_UpdateStatus(Size pObjSize);
    }
}