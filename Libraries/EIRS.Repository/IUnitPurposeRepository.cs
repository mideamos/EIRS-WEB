using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IUnitPurposeRepository
    {
        usp_GetUnitPurposeList_Result REP_GetUnitPurposeDetails(Unit_Purpose pObjUnitPurpose);
        IList<DropDownListResult> REP_GetUnitPurposeDropDownList(Unit_Purpose pObjUnitPurpose);
        IList<usp_GetUnitPurposeList_Result> REP_GetUnitPurposeList(Unit_Purpose pObjUnitPurpose);
        FuncResponse REP_InsertUpdateUnitPurpose(Unit_Purpose pObjUnitPurpose);
        FuncResponse REP_UpdateStatus(Unit_Purpose pObjUnitPurpose);
    }
}