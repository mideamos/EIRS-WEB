using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IMDAServiceRepository
    {
        usp_GetMDAServiceList_Result REP_GetMDAServiceDetails(MDA_Services pObjMDAService);
        IList<MAP_MDAService_MDAServiceItem> REP_GetMDAServiceItem(int pIntMDAServiceID);
        IList<usp_GetMDAServiceList_Result> REP_GetMDAServiceList(MDA_Services pObjMDAService);
        IList<MAP_MDAService_SettlementMethod> REP_GetSettlementMethod(int pIntMDAServiceID);
        IList<DropDownListResult> REP_GetSettlementMethodDropDownList(int pIntMDAServiceID);
        FuncResponse REP_InsertMDAServiceItem(MAP_MDAService_MDAServiceItem pObjMDAServiceItem);
        FuncResponse REP_InsertSettlementMethod(MAP_MDAService_SettlementMethod pObjSettlementMethod);
        FuncResponse<MDA_Services> REP_InsertUpdateMDAService(MDA_Services pObjMDAService);
        FuncResponse REP_RemoveMDAServiceItem(MAP_MDAService_MDAServiceItem pObjMDAServiceItem);
        FuncResponse REP_RemoveSettlementMethod(MAP_MDAService_SettlementMethod pObjSettlementMethod);
        FuncResponse REP_UpdateStatus(MDA_Services pObjMDAService);

        IList<DropDownListResult> REP_GetMDAServiceDropDownList(MDA_Services pObjMDAService);

        IList<usp_GetMDAServiceForServiceBill_Result> REP_GetMDAServiceForServiceBill(int pIntTaxPayerTypeID, int pIntTaxPayerID);

        IList<usp_GetTaxPayerMDAService_Result> REP_GetTaxPayerMDAService(int pIntTaxPayerTypeID, int pIntTaxPayerID);

        IList<vw_MDAServices> REP_GetMDAServiceList();

        IList<usp_GetVehicleInsuranceVerificationMDAServiceForSupplier_Result> REP_GetVehicleInsuranceVerificationMDAServiceForSupplier();
        IList<usp_GetVehicleLicenseMDAServiceForSupplier_Result> REP_GetVehicleLicenseMDAServiceForSupplier();
        IList<usp_GetEdoGISMDAServiceForSupplier_Result> REP_GetEdoGISMDAServiceForSupplier();

        IList<usp_SearchMDAServiceForRDMLoad_Result> REP_SearchMDAServiceDetails(MDA_Services pObjMDAService);
        IDictionary<string, object> REP_SearchMDAService(MDA_Services pObjMDAService);

        IDictionary<string, object> REP_SearchMDAServiceForSideMenu(MDA_Services pObjMDAService);
    }
}