using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface IAddressTypeRepository
    {
        usp_GetAddressTypeList_Result REP_GetAddressTypeDetails(Address_Types pObjAddressType);
        IList<DropDownListResult> REP_GetAddressTypeDropDownList(Address_Types pObjAddressType);
        IList<usp_GetAddressTypeList_Result> REP_GetAddressTypeList(Address_Types pObjAddressType);
        FuncResponse REP_InsertUpdateAddressType(Address_Types pObjAddressType);
        FuncResponse REP_UpdateStatus(Address_Types pObjAddressType);
    }
}