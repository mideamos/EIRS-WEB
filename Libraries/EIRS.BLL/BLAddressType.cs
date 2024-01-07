using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLAddressType
    {
        IAddressTypeRepository _AddressTypeRepository;

        public BLAddressType()
        {
            _AddressTypeRepository = new AddressTypeRepository();
        }

        public IList<usp_GetAddressTypeList_Result> BL_GetAddressTypeList(Address_Types pObjAddressType)
        {
            return _AddressTypeRepository.REP_GetAddressTypeList(pObjAddressType);
        }

        public FuncResponse BL_InsertUpdateAddressType(Address_Types pObjAddressType)
        {
            return _AddressTypeRepository.REP_InsertUpdateAddressType(pObjAddressType);
        }

        public usp_GetAddressTypeList_Result BL_GetAddressTypeDetails(Address_Types pObjAddressType)
        {
            return _AddressTypeRepository.REP_GetAddressTypeDetails(pObjAddressType);
        }

        public IList<DropDownListResult> BL_GetAddressTypeDropDownList(Address_Types pObjAddressType)
        {
            return _AddressTypeRepository.REP_GetAddressTypeDropDownList(pObjAddressType);
        }

        public FuncResponse BL_UpdateStatus(Address_Types pObjAddressType)
        {
            return _AddressTypeRepository.REP_UpdateStatus(pObjAddressType);
        }
    }
}
