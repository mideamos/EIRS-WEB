using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IUnitOccupancyRepository
    {
        usp_GetUnitOccupancyList_Result REP_GetUnitOccupancyDetails(Unit_Occupancy pObjUnitOccupancy);
        IList<DropDownListResult> REP_GetUnitOccupancyDropDownList(Unit_Occupancy pObjUnitOccupancy);
        IList<usp_GetUnitOccupancyList_Result> REP_GetUnitOccupancyList(Unit_Occupancy pObjUnitOccupancy);
        FuncResponse REP_InsertUpdateUnitOccupancy(Unit_Occupancy pObjUnitOccupancy);
        FuncResponse REP_UpdateStatus(Unit_Occupancy pObjUnitOccupancy);
    }
}