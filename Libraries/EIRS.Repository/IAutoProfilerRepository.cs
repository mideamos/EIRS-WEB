using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIRS.Repository
{
   public interface IAutoProfilerRepository
    {
        FuncResponse REP_UpdateBuildingDefaultValue(AP_Building pObjApBulding);
        AP_Building REP_GetBuildingDefaultValue(AP_Building pObjApBulding);
        FuncResponse REP_UpdateLandDefaultValue(AP_Land pObjApLand);
        AP_Land REP_GetLandDefaultValue(AP_Land pObjApLand);
        FuncResponse REP_UpdateVehicleDefaultValue(AP_Vehicle pObjApVehicle);
        AP_Vehicle REP_GetVehicleDefaultValue(AP_Vehicle pObjApVehicle);
      FuncResponse REP_UpdateBusinessDefaultValue(AP_Business pObjApBusiness);
        AP_Business REP_GetBusinessDefaultValue(AP_Business pObjApBusiness);
        FuncResponse REP_UpdateBuildingUnitDefaultValue(AP_Building_Unit pObjApBuildingUnit);
        AP_Building_Unit REP_GetBuildingUnitDefaultValue(AP_Building_Unit pObjApBuildingUnit);
    }
}
