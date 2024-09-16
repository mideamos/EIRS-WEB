using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IServiceBillRepository
    {
        FuncResponse REP_DeleteMDAService(long pIntSBSID);
        IList<MAP_ServiceBill_MDAService> REP_GetMDAServices(long pIntServiceBillID);
        usp_GetServiceBillList_Result REP_GetServiceBillDetails(ServiceBill pObjServiceBill);
        IList<MAP_ServiceBill_MDAServiceItem> REP_GetServiceBillItems(long plngSBSID);
        MAP_ServiceBill_MDAServiceItem GetServiceBillItems(long sBSID);
        IList<usp_GetServiceBillList_Result> REP_GetServiceBillList(ServiceBill pObjServiceBill);
        FuncResponse REP_InsertServiceBillItem(IList<MAP_ServiceBill_MDAServiceItem> plstServiceBillItem);
        FuncResponse<MAP_ServiceBill_MDAService> REP_InsertUpdateMDAService(MAP_ServiceBill_MDAService pObjMDAService);
        FuncResponse<ServiceBill> REP_InsertUpdateServiceBill(ServiceBill pObjServiceBill);
        FuncResponse REP_UpdateMDAServiceItemStatus(MAP_ServiceBill_MDAServiceItem pObjServiceBillItem);
        FuncResponse REP_UpdateServiceBillSettlementStatus(ServiceBill pObjServiceBill);
        FuncResponse REP_UpdateStatus(ServiceBill pObjServiceBill);

        IList<usp_GetServiceBillItemList_Result> REP_GetServiceBillItem(long pIntServiceBillID);

        IList<DropDownListResult>  REP_GetSettlementMethodMDAServiceBased(long pIntServiceBillID);

        IList<usp_GetMDAServiceBasedSettlement_Result> REP_GetMDAServiceBasedSettlement(long pIntServiceBillID);
        IList<usp_GetServiceBill_MDAServiceList_Result> REP_GetMDAServiceList(long pIntServiceBillID);
        IList<vw_ServiceBillNew> REP_GetServiceBillList();

        IList<usp_GetVehicleInsuranceVerificationServiceBillForSupplier_Result> REP_GetVehicleInsuranceVerificationServiceBillForSupplier();
        IList<usp_GetVehicleLicenseServiceBillForSupplier_Result> REP_GetVehicleLicenseServiceBillForSupplier();

        IDictionary<string, object> REP_SearchServiceBill(ServiceBill pObjServiceBill);

        IList<usp_GetServiceBillAdjustmentList_Result> REP_GetServiceBillAdjustment(long pIntServiceBillID);
        IList<usp_GetServiceBillLateChargeList_Result> REP_GetServiceBillLateCharge(long pIntServiceBillID);
        void REP_UpdateServiceBillAmount(long pIntServiceBillID);
        FuncResponse REP_InsertAdjustment(MAP_ServiceBill_Adjustment pObjAdjustment);
        usp_GetServiceBillItemDetails_Result REP_GetServiceBillItemDetails(long plngSBSIID);

        IDictionary<string, object> REP_SearchServiceBillForSideMenu(ServiceBill pObjServiceBill);
    }
}