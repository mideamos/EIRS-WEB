using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IMDAServiceItemRepository
    {
        usp_GetMDAServiceItemList_Result REP_GetMDAServiceItemDetails(MDA_Service_Items pObjMDAServiceItem);
        IList<DropDownListResult> REP_GetMDAServiceItemDropDownList(MDA_Service_Items pObjMDAServiceItem);
        IList<usp_GetMDAServiceItemList_Result> REP_GetMDAServiceItemList(MDA_Service_Items pObjMDAServiceItem);
        FuncResponse REP_InsertUpdateMDAServiceItem(MDA_Service_Items pObjMDAServiceItem);
        FuncResponse REP_UpdateStatus(MDA_Service_Items pObjMDAServiceItem);

        IList<usp_GetVehicleInsuranceVerificationMDAServiceItemForSupplier_Result> REP_GetVehicleInsuranceVerificationMDAServiceItemForSupplier();
        IList<usp_GetVehicleLicenseMDAServiceItemForSupplier_Result> REP_GetVehicleLicenseMDAServiceItemForSupplier();
        IList<usp_GetEdoGISMDAServiceItemForSupplier_Result> REP_GetEdoGISMDAServiceItemForSupplier();

        IList<usp_SearchMDAServiceItemForRDMLoad_Result> REP_SearchMDAServiceItemDetails(MDA_Service_Items pObjMDAServiceItem);
        IDictionary<string, object> REP_SearchMDAServiceItem(MDA_Service_Items pObjMDAServiceItem);
    }
}