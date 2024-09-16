using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ILandStreetConditionRepository
    {
        usp_GetLandStreetConditionList_Result REP_GetLandStreetConditionDetails(Land_StreetCondition pObjLandStreetCondition);
        IList<DropDownListResult> REP_GetLandStreetConditionDropDownList(Land_StreetCondition pObjLandStreetCondition);
        IList<usp_GetLandStreetConditionList_Result> REP_GetLandStreetConditionList(Land_StreetCondition pObjLandStreetCondition);
        FuncResponse REP_InsertUpdateLandStreetCondition(Land_StreetCondition pObjLandStreetCondition);
        FuncResponse REP_UpdateStatus(Land_StreetCondition pObjLandStreetCondition);
    }
}