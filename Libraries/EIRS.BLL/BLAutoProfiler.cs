using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.BLL
{
    public class BLAutoProfiler
    {
        IAutoProfilerRepository _IAutoProfilerRepository;
        public BLAutoProfiler()
        {
            _IAutoProfilerRepository = new AutoProfilerRepository();
        }
        public FuncResponse BL_UpdateBuildingDefaultValue(AP_Building pObjApBulding)
        {
            return _IAutoProfilerRepository.REP_UpdateBuildingDefaultValue(pObjApBulding);
        }
        public AP_Building BL_GetBuildingDefaultValue(AP_Building pObjApBulding)
        {
            return _IAutoProfilerRepository.REP_GetBuildingDefaultValue(pObjApBulding);
        }



        public FuncResponse BL_UpdateLandDefaultValue(AP_Land pObjApLand)
        {
            return _IAutoProfilerRepository.REP_UpdateLandDefaultValue(pObjApLand);
        }
        public AP_Land BL_GetLandDefaultValue(AP_Land pObjApLand)
        {
            return _IAutoProfilerRepository.REP_GetLandDefaultValue(pObjApLand);
        }
        public FuncResponse BL_UpdateVehicleDefaultValue(AP_Vehicle pObjApVehicle)
        {
            return _IAutoProfilerRepository.REP_UpdateVehicleDefaultValue(pObjApVehicle);
        }
        public AP_Vehicle BL_GetVehicleDefaultValue(AP_Vehicle pObjApVehicle)
        {
            return _IAutoProfilerRepository.REP_GetVehicleDefaultValue(pObjApVehicle);
        }
        public FuncResponse BL_UpdateBusinessDefaultValue(AP_Business pObjApBusiness)
        {
            return _IAutoProfilerRepository.REP_UpdateBusinessDefaultValue(pObjApBusiness);
        }
        public AP_Business BL_GetBusinessDefaultValue(AP_Business pObjApBusiness)
        {
            return _IAutoProfilerRepository.REP_GetBusinessDefaultValue(pObjApBusiness);
        }
        public FuncResponse BL_UpdateBuildingUnitDefaultValue(AP_Building_Unit pObjApBuildingUnit)
        {
            return _IAutoProfilerRepository.REP_UpdateBuildingUnitDefaultValue(pObjApBuildingUnit);
        }
        public AP_Building_Unit BL_GetBuildingUnitDefaultValue(AP_Building_Unit pObjApBuildingUnit)
        {
            return _IAutoProfilerRepository.REP_GetBuildingUnitDefaultValue(pObjApBuildingUnit);
        }
    }
}
