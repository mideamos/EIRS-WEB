using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLMDAService
    {
        IMDAServiceRepository _MDAServiceRepository;

        public BLMDAService()
        {
            _MDAServiceRepository = new MDAServiceRepository();
        }

        public IList<usp_GetMDAServiceList_Result> BL_GetMDAServiceList(MDA_Services pObjMDAService)
        {
            return _MDAServiceRepository.REP_GetMDAServiceList(pObjMDAService);
        }

        public IList<vw_MDAServices> BL_GetMDAServiceList()
        {
            return _MDAServiceRepository.REP_GetMDAServiceList();
        }

        public FuncResponse<MDA_Services> BL_InsertUpdateMDAService(MDA_Services pObjMDAService)
        {
            return _MDAServiceRepository.REP_InsertUpdateMDAService(pObjMDAService);
        }

        public usp_GetMDAServiceList_Result BL_GetMDAServiceDetails(MDA_Services pObjMDAService)
        {
            return _MDAServiceRepository.REP_GetMDAServiceDetails(pObjMDAService);
        }

        public FuncResponse BL_UpdateStatus(MDA_Services pObjMDAService)
        {
            return _MDAServiceRepository.REP_UpdateStatus(pObjMDAService);
        }

        public IList<DropDownListResult> BL_GetMDAServiceDropDownList(MDA_Services pObjMDAService)
        {
            return _MDAServiceRepository.REP_GetMDAServiceDropDownList(pObjMDAService);
        }

        public FuncResponse BL_InsertSettlementMethod(MAP_MDAService_SettlementMethod pObjSettlementMethod)
        {
            return _MDAServiceRepository.REP_InsertSettlementMethod(pObjSettlementMethod);
        }

        public FuncResponse BL_RemoveSettlementMethod(MAP_MDAService_SettlementMethod pObjSettlementMethod)
        {
            return _MDAServiceRepository.REP_RemoveSettlementMethod(pObjSettlementMethod);
        }

        public IList<MAP_MDAService_SettlementMethod> BL_GetSettlementMethod(int pIntSettlementMethodID)
        {
            return _MDAServiceRepository.REP_GetSettlementMethod(pIntSettlementMethodID);
        }

        public FuncResponse BL_InsertMDAServiceItem(MAP_MDAService_MDAServiceItem pObjMDAServiceItem)
        {
            return _MDAServiceRepository.REP_InsertMDAServiceItem(pObjMDAServiceItem);
        }

        public FuncResponse BL_RemoveMDAServiceItem(MAP_MDAService_MDAServiceItem pObjMDAServiceItem)
        {
            return _MDAServiceRepository.REP_RemoveMDAServiceItem(pObjMDAServiceItem);
        }

        public IList<MAP_MDAService_MDAServiceItem> BL_GetMDAServiceItem(int pIntMDAServiceID)
        {
            return _MDAServiceRepository.REP_GetMDAServiceItem(pIntMDAServiceID);
        }

        public IList<DropDownListResult> BL_GetSettlementMethodDropDownList(int pIntMDAServiceID)
        {
            return _MDAServiceRepository.REP_GetSettlementMethodDropDownList(pIntMDAServiceID);
        }

        public IList<usp_GetMDAServiceForServiceBill_Result> BL_GetMDAServiceForServiceBill(int pIntTaxPayerTypeID, int pIntTaxPayerID)
        {
            return _MDAServiceRepository.REP_GetMDAServiceForServiceBill(pIntTaxPayerTypeID, pIntTaxPayerID);
        }

        public IList<usp_GetTaxPayerMDAService_Result> BL_GetTaxPayerMDAService(int pIntTaxPayerTypeID, int pIntTaxPayerID)
        {
            return _MDAServiceRepository.REP_GetTaxPayerMDAService(pIntTaxPayerTypeID, pIntTaxPayerID);
        }

        public IList<usp_GetVehicleInsuranceVerificationMDAServiceForSupplier_Result> BL_GetVehicleInsuranceVerificationMDAServiceForSupplier()
        {
            return _MDAServiceRepository.REP_GetVehicleInsuranceVerificationMDAServiceForSupplier();
        }

        public IList<usp_GetEdoGISMDAServiceForSupplier_Result> BL_GetEdoGISMDAServiceForSupplier()
        {
            return _MDAServiceRepository.REP_GetEdoGISMDAServiceForSupplier();
        }

        public IList<usp_GetVehicleLicenseMDAServiceForSupplier_Result> BL_GetVehicleLicenseMDAServiceForSupplier()
        {
            return _MDAServiceRepository.REP_GetVehicleLicenseMDAServiceForSupplier();
        }

        public IList<usp_SearchMDAServiceForRDMLoad_Result> BL_SearchMDAServiceDetails(MDA_Services pObjMDAService)
        {
            return _MDAServiceRepository.REP_SearchMDAServiceDetails(pObjMDAService);
        }

        public IDictionary<string, object> BL_SearchMDAService(MDA_Services pObjMDAService)
        {
            return _MDAServiceRepository.REP_SearchMDAService(pObjMDAService);
        }

        public IDictionary<string, object> BL_SearchMDAServiceForSideMenu(MDA_Services pObjMDAService)
        {
            return _MDAServiceRepository.REP_SearchMDAServiceForSideMenu(pObjMDAService);
        }
    }
}
