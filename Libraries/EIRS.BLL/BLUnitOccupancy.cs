using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLUnitOccupancy
    {
        IUnitOccupancyRepository _UnitOccupancyRepository;

        public BLUnitOccupancy()
        {
            _UnitOccupancyRepository = new UnitOccupancyRepository();
        }

        public IList<usp_GetUnitOccupancyList_Result> BL_GetUnitOccupancyList(Unit_Occupancy pObjUnitOccupancy)
        {
            return _UnitOccupancyRepository.REP_GetUnitOccupancyList(pObjUnitOccupancy);
        }

        public FuncResponse BL_InsertUpdateUnitOccupancy(Unit_Occupancy pObjUnitOccupancy)
        {
            return _UnitOccupancyRepository.REP_InsertUpdateUnitOccupancy(pObjUnitOccupancy);
        }

        public usp_GetUnitOccupancyList_Result BL_GetUnitOccupancyDetails(Unit_Occupancy pObjUnitOccupancy)
        {
            return _UnitOccupancyRepository.REP_GetUnitOccupancyDetails(pObjUnitOccupancy);
        }

        public IList<DropDownListResult> BL_GetUnitOccupancyDropDownList(Unit_Occupancy pObjUnitOccupancy)
        {
            return _UnitOccupancyRepository.REP_GetUnitOccupancyDropDownList(pObjUnitOccupancy);
        }

        public FuncResponse BL_UpdateStatus(Unit_Occupancy pObjUnitOccupancy)
        {
            return _UnitOccupancyRepository.REP_UpdateStatus(pObjUnitOccupancy);
        }
    }
}
