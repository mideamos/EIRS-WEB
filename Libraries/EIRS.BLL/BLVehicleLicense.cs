using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLVehicleLicense
    {
        IVehicleLicenseRepository _VehicleLicenseRepository;

        public BLVehicleLicense()
        {
            _VehicleLicenseRepository = new VehicleLicenseRepository();
        }

        public IList<usp_GetVehicleLicenseList_Result> BL_GetVehicleLicenseList(Vehicle_Licenses pObjVehicleLicense)
        {
            return _VehicleLicenseRepository.REP_GetVehicleLicenseList(pObjVehicleLicense);
        }

        public FuncResponse BL_InsertUpdateVehicleLicense(Vehicle_Licenses pObjVehicleLicense)
        {
            return _VehicleLicenseRepository.REP_InsertUpdateVehicleLicense(pObjVehicleLicense);
        }

        public usp_GetVehicleLicenseList_Result BL_GetVehicleLicenseDetails(Vehicle_Licenses pObjVehicleLicense)
        {
            return _VehicleLicenseRepository.REP_GetVehicleLicenseDetails(pObjVehicleLicense);
        }

        public IList<DropDownListResult> BL_GetVehicleLicenseDropDownList(Vehicle_Licenses pObjVehicleLicense)
        {
            return _VehicleLicenseRepository.REP_GetVehicleLicenseDropDownList(pObjVehicleLicense);
        }

        public FuncResponse BL_UpdateStatus(Vehicle_Licenses pObjVehicleLicense)
        {
            return _VehicleLicenseRepository.REP_UpdateStatus(pObjVehicleLicense);
        }
    }
}
