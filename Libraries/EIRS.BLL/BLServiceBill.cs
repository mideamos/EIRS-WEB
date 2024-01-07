using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLServiceBill
    {
        IServiceBillRepository _ServiceBillRepository;

        public BLServiceBill()
        {
            _ServiceBillRepository = new ServiceBillRepository();
        }

        public IList<usp_GetServiceBillList_Result> BL_GetServiceBillList(ServiceBill pObjServiceBill)
        {
            return _ServiceBillRepository.REP_GetServiceBillList(pObjServiceBill);
        }

        public IList<vw_ServiceBillNew> BL_GetServiceBillList()
        {
            return _ServiceBillRepository.REP_GetServiceBillList();
        }

        public FuncResponse<ServiceBill> BL_InsertUpdateServiceBill(ServiceBill pObjServiceBill)
        {
            return _ServiceBillRepository.REP_InsertUpdateServiceBill(pObjServiceBill);
        }

        public FuncResponse<MAP_ServiceBill_MDAService> BL_InsertUpdateMDAService(MAP_ServiceBill_MDAService pObjMDAService)
        {
            return _ServiceBillRepository.REP_InsertUpdateMDAService(pObjMDAService);
        }

        public IList<MAP_ServiceBill_MDAService> BL_GetMDAServices(long pIntServiceBillID)
        {
            return _ServiceBillRepository.REP_GetMDAServices(pIntServiceBillID);
        }

        public usp_GetServiceBillList_Result BL_GetServiceBillDetails(ServiceBill pObjServiceBill)
        {
            return _ServiceBillRepository.REP_GetServiceBillDetails(pObjServiceBill);
        }

        public FuncResponse BL_InsertServiceBillItem(IList<MAP_ServiceBill_MDAServiceItem> plstServiceBillItem)
        {
            return _ServiceBillRepository.REP_InsertServiceBillItem(plstServiceBillItem);
        }

        public IList<MAP_ServiceBill_MDAServiceItem> BL_GetServiceBillItems(long plngSBSID)
        {
            return _ServiceBillRepository.REP_GetServiceBillItems(plngSBSID);
        } 
        public MAP_ServiceBill_MDAServiceItem GetServiceBillItems(long sBSID)
        {
            return _ServiceBillRepository.GetServiceBillItems(sBSID);
        }

        public IList<usp_GetServiceBillItemList_Result> BL_GetServiceBillItem(long pIntServiceBillID)
        {
            return _ServiceBillRepository.REP_GetServiceBillItem(pIntServiceBillID);
        }

        public FuncResponse BL_UpdateMDAServiceItemStatus(MAP_ServiceBill_MDAServiceItem pObjServiceBillItem)
        {
            return _ServiceBillRepository.REP_UpdateMDAServiceItemStatus(pObjServiceBillItem);
        }

        public FuncResponse BL_UpdateStatus(ServiceBill pObjServiceBill)
        {
            return _ServiceBillRepository.REP_UpdateStatus(pObjServiceBill);
        }


        public FuncResponse BL_UpdateServiceBillSettlementStatus(ServiceBill pObjServiceBill)
        {
            return _ServiceBillRepository.REP_UpdateServiceBillSettlementStatus(pObjServiceBill);
        }

        public FuncResponse BL_DeleteMDAService(long pIntAARID)
        {
            return _ServiceBillRepository.REP_DeleteMDAService(pIntAARID);
        }

        public IList<DropDownListResult> BL_GetSettlementMethodMDAServiceBased(long pIntServiceBillID)
        {
            return _ServiceBillRepository.REP_GetSettlementMethodMDAServiceBased(pIntServiceBillID);
        }

        public IList<usp_GetMDAServiceBasedSettlement_Result> BL_GetMDAServiceBasedSettlement(long pIntServiceBillID)
        {
            return _ServiceBillRepository.REP_GetMDAServiceBasedSettlement(pIntServiceBillID);
        }

        public IList<usp_GetServiceBill_MDAServiceList_Result> BL_GetMDAServiceList(long pIntServiceBillID)
        {
            return _ServiceBillRepository.REP_GetMDAServiceList(pIntServiceBillID);
        }

        public IList<usp_GetVehicleLicenseServiceBillForSupplier_Result> BL_GetVehicleLicenseServiceBillForSupplier()
        {
            return _ServiceBillRepository.REP_GetVehicleLicenseServiceBillForSupplier();
        }

        public IList<usp_GetVehicleInsuranceVerificationServiceBillForSupplier_Result> BL_GetVehicleInsuranceVerificationServiceBillForSupplier()
        {
            return _ServiceBillRepository.REP_GetVehicleInsuranceVerificationServiceBillForSupplier();
        }

        public IDictionary<string, object> BL_SearchServiceBill(ServiceBill pObjServiceBill)
        {
            return _ServiceBillRepository.REP_SearchServiceBill(pObjServiceBill);
        }

        public void BL_UpdateServiceBillAmount(long pIntServiceBillID)
        {
            _ServiceBillRepository.REP_UpdateServiceBillAmount(pIntServiceBillID);
        }

        public IList<usp_GetServiceBillAdjustmentList_Result> BL_GetServiceBillAdjustment(long pIntServiceBillID)
        {
            return _ServiceBillRepository.REP_GetServiceBillAdjustment(pIntServiceBillID);
        }

        public IList<usp_GetServiceBillLateChargeList_Result> BL_GetServiceBillLateCharge(long pIntServiceBillID)
        {
            return _ServiceBillRepository.REP_GetServiceBillLateCharge(pIntServiceBillID);
        }

        public FuncResponse BL_InsertAdjustment(MAP_ServiceBill_Adjustment pObjAdjustment)
        {
            return _ServiceBillRepository.REP_InsertAdjustment(pObjAdjustment);
        }

        public usp_GetServiceBillItemDetails_Result BL_GetServiceBillItemDetails(long plngSBSIID)
        {
            return _ServiceBillRepository.REP_GetServiceBillItemDetails(plngSBSIID);
        }

        public IDictionary<string, object> BL_SearchServiceBillForSideMenu(ServiceBill pObjServiceBill)
        {
            return _ServiceBillRepository.REP_SearchServiceBillForSideMenu(pObjServiceBill);
        }
    }
}
