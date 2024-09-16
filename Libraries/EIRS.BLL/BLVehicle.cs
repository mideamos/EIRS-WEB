using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLVehicle
    {
        IVehicleRepository _VehicleRepository;

        public BLVehicle()
        {
            _VehicleRepository = new VehicleRepository();
        }

        public IList<usp_GetVehicleList_Result> BL_GetVehicleList(Vehicle pObjVehicle)
        {
            return _VehicleRepository.REP_GetVehicleList(pObjVehicle);
        }

        public IList<vw_Vehicle> BL_GetVehicleList()
        {
            return _VehicleRepository.REP_GetVehicleList();
        }

        public FuncResponse<Vehicle> BL_InsertUpdateVehicle(Vehicle pObjVehicle)
        {
            return _VehicleRepository.REP_InsertUpdateVehicle(pObjVehicle);
        }

        public usp_GetVehicleList_Result BL_GetVehicleDetails(Vehicle pObjVehicle)
        {
            return _VehicleRepository.REP_GetVehicleDetails(pObjVehicle);
        }

        public IList<DropDownListResult> BL_GetVehicleDropDownList(Vehicle pObjVehicle)
        {
            return _VehicleRepository.REP_GetVehicleDropDownList(pObjVehicle);
        }

        public FuncResponse BL_UpdateStatus(Vehicle pObjVehicle)
        {
            return _VehicleRepository.REP_UpdateStatus(pObjVehicle);
        }

        public IList<usp_SearchVehicleByRegNumber_Result> BL_SearchVehicleByRegNumber(string pStrRegNumber)
        {
            return _VehicleRepository.REP_SearchVehicleByRegNumber(pStrRegNumber);
        }

        public IList<usp_GetVehicleTaxPayerList_Result> BL_GetVehicleTaxPayerList(int pIntVehicleID)
        {
            return _VehicleRepository.REP_GetVehicleTaxPayerList(pIntVehicleID);
        }

        public IList<usp_SearchVehicleForRDMLoad_Result> BL_SearchVehicleDetails(Vehicle pObjVehicle)
        {
            return _VehicleRepository.REP_SearchVehicleDetails(pObjVehicle);
        }

        public IDictionary<string, object> BL_SearchVehicle(Vehicle pObjVehicle)
        {
            return _VehicleRepository.REP_SearchVehicle(pObjVehicle);
        }

        public IDictionary<string, object> BL_SearchVehicleForSideMenu(Vehicle pObjVehicle)
        {
            return _VehicleRepository.REP_SearchVehicleForSideMenu(pObjVehicle);
        }
    }
}
