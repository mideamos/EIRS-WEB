using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLCertificateType
    {
        ICertificateTypeRepository _CertificateTypeRepository;

        public BLCertificateType()
        {
            _CertificateTypeRepository = new CertificateTypeRepository();
        }

        public FuncResponse<Certificate_Types> BL_InsertUpdateCertificateType(Certificate_Types pObjCertificateType)
        {
            return _CertificateTypeRepository.REP_InsertUpdateCertificateType(pObjCertificateType);
        }

        public IList<usp_GetCertificateTypeList_Result> BL_GetCertificateTypeList(Certificate_Types pObjCertificateType)
        {
            return _CertificateTypeRepository.REP_GetCertificateTypeList(pObjCertificateType);
        }

        public usp_GetCertificateTypeList_Result BL_GetCertificateTypeDetails(Certificate_Types pObjCertificateType)
        {
            return _CertificateTypeRepository.REP_GetCertificateTypeDetails(pObjCertificateType);
        }

        public FuncResponse BL_UpdateStatus(Certificate_Types pObjCertificateType)
        {
            return _CertificateTypeRepository.REP_UpdateStatus(pObjCertificateType);
        }

        public FuncResponse BL_InsertCertificateTypeItem(MAP_CertificateType_Items pObjCertificateTypeItem)
        {
            return _CertificateTypeRepository.REP_InsertCertificateTypeItem(pObjCertificateTypeItem);
        }

        public IList<usp_GetCertificateTypeItemList_Result> BL_GetCertificateTypeItemList(Certificate_Types pObjCertificateType)
        {
            return _CertificateTypeRepository.REP_GetCertificateTypeItemList(pObjCertificateType);
        }

        public FuncResponse BL_RemoveCertificateTypeItem(MAP_CertificateType_Items pObjCertificateTypeItem)
        {
            return _CertificateTypeRepository.REP_RemoveCertificateTypeItem(pObjCertificateTypeItem);
        }

        public FuncResponse BL_UpdateCertificateTypeItem(MAP_CertificateType_Items pObjCertificateTypeItem)
        {
            return _CertificateTypeRepository.REP_UpdateCertificateTypeItem(pObjCertificateTypeItem);
        }

        public FuncResponse BL_InsertCertificateTypeField(MAP_CertificateType_Field pObjCertificateTypeField)
        {
            return _CertificateTypeRepository.REP_InsertCertificateTypeField(pObjCertificateTypeField);
        }

        public IList<usp_GetCertificateTypeFieldList_Result> BL_GetCertificateTypeFieldList(Certificate_Types pObjCertificateType)
        {
            return _CertificateTypeRepository.REP_GetCertificateTypeFieldList(pObjCertificateType);
        }

        public FuncResponse BL_RemoveCertificateTypeField(MAP_CertificateType_Field pObjCertificateTypeField)
        {
            return _CertificateTypeRepository.REP_RemoveCertificateTypeField(pObjCertificateTypeField);
        }

        public FuncResponse BL_UpdateCertificateTypeField(MAP_CertificateType_Field pObjCertificateTypeField)
        {
            return _CertificateTypeRepository.REP_UpdateCertificateTypeField(pObjCertificateTypeField);
        }

        public IList<DropDownListResult> BL_GetCertificateTypeDropDown(Certificate_Types pObjCertificateType)
        {
            return _CertificateTypeRepository.REP_GetCertificateTypeDropDown(pObjCertificateType);
        }


    }
}
