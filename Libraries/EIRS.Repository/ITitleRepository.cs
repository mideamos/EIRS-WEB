using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ITitleRepository
    {
		FuncResponse REP_InsertUpdateTitle(Title pObjTitle);
		IList<usp_GetTitleList_Result> REP_GetTitleList(Title pObjTitle);
        usp_GetTitleList_Result REP_GetTitleDetails(Title pObjTitle);
        IList<DropDownListResult> REP_GetTitleDropDownList(Title pObjTitle);
        FuncResponse REP_UpdateStatus(Title pObjTitle);
    }
}