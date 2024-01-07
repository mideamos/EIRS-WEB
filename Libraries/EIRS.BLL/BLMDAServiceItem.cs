using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLMDAServiceItem
    {
        IMDAServiceItemRepository _MDAServiceItemRepository;

        public BLMDAServiceItem()
        {
            _MDAServiceItemRepository = new MDAServiceItemRepository();
        }

        public IList<usp_GetMDAServiceItemList_Result> BL_GetMDAServiceItemList(MDA_Service_Items pObjMDAServiceItem)
        {
            return _MDAServiceItemRepository.REP_GetMDAServiceItemList(pObjMDAServiceItem);
        }

        public FuncResponse BL_InsertUpdateMDAServiceItem(MDA_Service_Items pObjMDAServiceItem)
        {
            return _MDAServiceItemRepository.REP_InsertUpdateMDAServiceItem(pObjMDAServiceItem);
        }

        public usp_GetMDAServiceItemList_Result BL_GetMDAServiceItemDetails(MDA_Service_Items pObjMDAServiceItem)
        {
            return _MDAServiceItemRepository.REP_GetMDAServiceItemDetails(pObjMDAServiceItem);
        }

        public IList<DropDownListResult> BL_GetMDAServiceItemDropDownList(MDA_Service_Items pObjMDAServiceItem)
        {
            return _MDAServiceItemRepository.REP_GetMDAServiceItemDropDownList(pObjMDAServiceItem);
        }

        public FuncResponse BL_UpdateStatus(MDA_Service_Items pObjMDAServiceItem)
        {
            return _MDAServiceItemRepository.REP_UpdateStatus(pObjMDAServiceItem);
        }

        public IList<usp_GetVehicleInsuranceVerificationMDAServiceItemForSupplier_Result> BL_GetVehicleInsuranceVerificationMDAServiceItemForSupplier()
        {
            return _MDAServiceItemRepository.REP_GetVehicleInsuranceVerificationMDAServiceItemForSupplier();
        }

        public IList<usp_GetVehicleLicenseMDAServiceItemForSupplier_Result> BL_GetVehicleLicenseMDAServiceItemForSupplier()
        {
            return _MDAServiceItemRepository.REP_GetVehicleLicenseMDAServiceItemForSupplier();
        }

        public IList<usp_GetEdoGISMDAServiceItemForSupplier_Result> BL_GetEdoGISMDAServiceItemForSupplier()
        {
            return _MDAServiceItemRepository.REP_GetEdoGISMDAServiceItemForSupplier();
        }

        public IList<usp_SearchMDAServiceItemForRDMLoad_Result> BL_SearchMDAServiceItemDetails(MDA_Service_Items pObjMDAServiceItem)
        {
            return _MDAServiceItemRepository.REP_SearchMDAServiceItemDetails(pObjMDAServiceItem);
        }

        public IDictionary<string, object> BL_SearchMDAServiceItem(MDA_Service_Items pObjMDAServiceItem)
        {
            return _MDAServiceItemRepository.REP_SearchMDAServiceItem(pObjMDAServiceItem);
        }
    }
}
