using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ILGARepository
    {
        usp_GetLGAList_Result REP_GetLGADetails(LGA pObjLGA);
        IList<DropDownListResult> REP_GetLGADropDownList(LGA pObjLGA);
        IList<usp_GetLGAList_Result> REP_GetLGAList(LGA pObjLGA);
        FuncResponse REP_InsertUpdateLGA(LGA pObjLGA);
        FuncResponse REP_UpdateStatus(LGA pObjLGA);

        IList<DropDownListResult> REP_GetLGAClassDropDownList();
    }
}