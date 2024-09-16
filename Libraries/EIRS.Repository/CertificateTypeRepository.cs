using System;
using System.Collections.Generic;
using System.Linq;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ICertificateTypeRepository
    {
        usp_GetCertificateTypeList_Result REP_GetCertificateTypeDetails(Certificate_Types pObjCertificateType);
        IList<usp_GetCertificateTypeFieldList_Result> REP_GetCertificateTypeFieldList(Certificate_Types pObjCertificateType);
        IList<usp_GetCertificateTypeItemList_Result> REP_GetCertificateTypeItemList(Certificate_Types pObjCertificateType);
        IList<usp_GetCertificateTypeList_Result> REP_GetCertificateTypeList(Certificate_Types pObjCertificateType);
        FuncResponse REP_InsertCertificateTypeField(MAP_CertificateType_Field pObjCertificateTypeField);
        FuncResponse REP_InsertCertificateTypeItem(MAP_CertificateType_Items pObjCertificateTypeItem);
        FuncResponse<Certificate_Types> REP_InsertUpdateCertificateType(Certificate_Types pObjCertificateType);
        FuncResponse REP_RemoveCertificateTypeField(MAP_CertificateType_Field pObjCertificateTypeField);
        FuncResponse REP_RemoveCertificateTypeItem(MAP_CertificateType_Items pObjCertificateTypeItem);
        FuncResponse REP_UpdateCertificateTypeField(MAP_CertificateType_Field pObjCertificateTypeField);
        FuncResponse REP_UpdateCertificateTypeItem(MAP_CertificateType_Items pObjCertificateTypeItem);
        FuncResponse REP_UpdateStatus(Certificate_Types pObjCertificateType);

        IList<DropDownListResult> REP_GetCertificateTypeDropDown(Certificate_Types pObjCertificateType);
    }

    public sealed class CertificateTypeRepository : ICertificateTypeRepository
    {
        EIRSEntities _db;

        public FuncResponse<Certificate_Types> REP_InsertUpdateCertificateType(Certificate_Types pObjCertificateType)
        {
            using (_db = new EIRSEntities())
            {
                Certificate_Types mObjInsertUpdateCertificateType;
                FuncResponse<Certificate_Types> mObjFuncResponse = new FuncResponse<Certificate_Types>();


                if (pObjCertificateType.CertificateTypeID != 0)
                {
                    mObjInsertUpdateCertificateType = (from dlo in _db.Certificate_Types
                                                       where dlo.CertificateTypeID == pObjCertificateType.CertificateTypeID
                                                       select dlo).FirstOrDefault();

                    if (mObjInsertUpdateCertificateType != null)
                    {
                        mObjInsertUpdateCertificateType.ModifiedBy = pObjCertificateType.ModifiedBy;
                        mObjInsertUpdateCertificateType.ModifiedDate = pObjCertificateType.ModifiedDate;
                    }
                    else
                    {
                        mObjInsertUpdateCertificateType = new Certificate_Types
                        {
                            CreatedBy = pObjCertificateType.CreatedBy,
                            CreatedDate = pObjCertificateType.CreatedDate,
                            CNSerialNumber = 0
                        };
                    }
                }
                else
                {
                    mObjInsertUpdateCertificateType = new Certificate_Types
                    {
                        CreatedBy = pObjCertificateType.CreatedBy,
                        CreatedDate = pObjCertificateType.CreatedDate,
                        CNSerialNumber = 0
                    };
                }

                mObjInsertUpdateCertificateType.CertificateTypeName = pObjCertificateType.CertificateTypeName;
                mObjInsertUpdateCertificateType.Description = pObjCertificateType.Description;
                mObjInsertUpdateCertificateType.TaxYear = pObjCertificateType.TaxYear;
                mObjInsertUpdateCertificateType.CNPrefix = pObjCertificateType.CNPrefix;
                mObjInsertUpdateCertificateType.AgencyID = pObjCertificateType.AgencyID;
                mObjInsertUpdateCertificateType.SEDE_PDFTemplateID = pObjCertificateType.SEDE_PDFTemplateID;
                mObjInsertUpdateCertificateType.CertificateTemplatePath = pObjCertificateType.CertificateTemplatePath;
                mObjInsertUpdateCertificateType.Active = pObjCertificateType.Active;

                if (pObjCertificateType.CertificateTypeID == 0)
                {
                    _db.Certificate_Types.Add(mObjInsertUpdateCertificateType);
                }
                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.AdditionalData = mObjInsertUpdateCertificateType;

                    mObjFuncResponse.Success = true;
                    if (pObjCertificateType.CertificateTypeID == 0)
                    {
                        mObjFuncResponse.Message = "Certificate Type Added Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Certificate Type Updated Successfully";
                    }
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = ex;
                    if (pObjCertificateType.CertificateTypeID == 0)
                    {
                        mObjFuncResponse.Message = "Certificate Type Addition Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Certificate Type Updation Failed";
                    }
                }
                return mObjFuncResponse;
            }
        }

        public IList<usp_GetCertificateTypeList_Result> REP_GetCertificateTypeList(Certificate_Types pObjCertificateType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetCertificateTypeList(pObjCertificateType.CertificateTypeName, pObjCertificateType.TaxYear, pObjCertificateType.CertificateTypeID, pObjCertificateType.IntStatus).ToList();
            }
        }

        public usp_GetCertificateTypeList_Result REP_GetCertificateTypeDetails(Certificate_Types pObjCertificateType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetCertificateTypeList(pObjCertificateType.CertificateTypeName, pObjCertificateType.TaxYear, pObjCertificateType.CertificateTypeID, pObjCertificateType.IntStatus).FirstOrDefault();
            }
        }

        public FuncResponse REP_UpdateStatus(Certificate_Types pObjCertificateType)
        {
            using (_db = new EIRSEntities())
            {
                Certificate_Types mObjInsertUpdateCertificateTypes;
                FuncResponse mObjFuncResponse = new FuncResponse();

                if (pObjCertificateType.CertificateTypeID != 0)
                {
                    mObjInsertUpdateCertificateTypes = (from dd in _db.Certificate_Types
                                                        where dd.CertificateTypeID == pObjCertificateType.CertificateTypeID
                                                        select dd).FirstOrDefault();

                    if (mObjInsertUpdateCertificateTypes != null)
                    {
                        mObjInsertUpdateCertificateTypes.Active = !mObjInsertUpdateCertificateTypes.Active;
                        mObjInsertUpdateCertificateTypes.ModifiedBy = pObjCertificateType.ModifiedBy;
                        mObjInsertUpdateCertificateTypes.ModifiedDate = pObjCertificateType.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Certificate Type Updated Successfully";
                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Certificate Type Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_InsertCertificateTypeItem(MAP_CertificateType_Items pObjCertificateTypeItem)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();

                var vDuplicateCheck = (from cti in _db.MAP_CertificateType_Items
                                       where (cti.CertificateTypeID == pObjCertificateTypeItem.CertificateTypeID && cti.ItemTypeID == pObjCertificateTypeItem.ItemTypeID && cti.ItemID == pObjCertificateTypeItem.ItemID)
                                       select cti).Count();

                if (vDuplicateCheck > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Item Already Exists";
                }

                _db.MAP_CertificateType_Items.Add(pObjCertificateTypeItem);

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = ex.Message;
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetCertificateTypeItemList_Result> REP_GetCertificateTypeItemList(Certificate_Types pObjCertificateType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetCertificateTypeItemList(pObjCertificateType.CertificateTypeID, pObjCertificateType.IntStatus).ToList();
            }
        }

        public FuncResponse REP_RemoveCertificateTypeItem(MAP_CertificateType_Items pObjCertificateTypeItem)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();
                MAP_CertificateType_Items mObjCertificateTypeItem = _db.MAP_CertificateType_Items.Find(pObjCertificateTypeItem.CTIID);

                if (mObjFuncResponse == null)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Certificate Type Item Already Removed";
                }

                else
                {
                    _db.MAP_CertificateType_Items.Remove(mObjCertificateTypeItem);

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = Ex.Message;
                    }
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_UpdateCertificateTypeItem(MAP_CertificateType_Items pObjCertificateTypeItem)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();
                MAP_CertificateType_Items mObjCertificateTypeItem = _db.MAP_CertificateType_Items.Find(pObjCertificateTypeItem.CTIID);

                if (mObjCertificateTypeItem != null)
                {
                    mObjCertificateTypeItem.ItemTypeID = pObjCertificateTypeItem.ItemTypeID;
                    mObjCertificateTypeItem.ItemID = pObjCertificateTypeItem.ItemID;
                    mObjCertificateTypeItem.ModifiedBy = pObjCertificateTypeItem.ModifiedBy;
                    mObjCertificateTypeItem.ModifiedDate = pObjCertificateTypeItem.ModifiedDate;

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = Ex.Message;
                    }
                }
                else
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Certificate Type Item Not Found";
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_InsertCertificateTypeField(MAP_CertificateType_Field pObjCertificateTypeField)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();

                var vDuplicateCheck = (from ctf in _db.MAP_CertificateType_Field
                                       where (ctf.CertificateTypeID == pObjCertificateTypeField.CertificateTypeID && ctf.FieldName == pObjCertificateTypeField.FieldName)
                                       select ctf).Count();

                if (vDuplicateCheck > 0)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Field Already Exists";
                }

                _db.MAP_CertificateType_Field.Add(pObjCertificateTypeField);

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = ex.Message;
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetCertificateTypeFieldList_Result> REP_GetCertificateTypeFieldList(Certificate_Types pObjCertificateType)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetCertificateTypeFieldList(pObjCertificateType.CertificateTypeID, pObjCertificateType.IntStatus).ToList();
            }
        }

        public FuncResponse REP_RemoveCertificateTypeField(MAP_CertificateType_Field pObjCertificateTypeField)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();
                MAP_CertificateType_Field mObjCertificateTypeField = _db.MAP_CertificateType_Field.Find(pObjCertificateTypeField.CTFID);

                if (mObjFuncResponse == null)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Field Already Removed";
                }

                else
                {
                    _db.MAP_CertificateType_Field.Remove(mObjCertificateTypeField);

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = Ex.Message;
                    }
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_UpdateCertificateTypeField(MAP_CertificateType_Field pObjCertificateTypeField)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();
                MAP_CertificateType_Field mObjCertificateTypeField = _db.MAP_CertificateType_Field.Find(pObjCertificateTypeField.CTFID);

                if (mObjCertificateTypeField != null)
                {

                    mObjCertificateTypeField.FieldName = pObjCertificateTypeField.FieldName;
                    mObjCertificateTypeField.FieldTypeID = pObjCertificateTypeField.FieldTypeID;
                    mObjCertificateTypeField.FieldComboValue = pObjCertificateTypeField.FieldComboValue;
                    mObjCertificateTypeField.IsRequired = pObjCertificateTypeField.IsRequired;
                    mObjCertificateTypeField.Active = pObjCertificateTypeField.Active;
                    mObjCertificateTypeField.ModifiedBy = pObjCertificateTypeField.ModifiedBy;
                    mObjCertificateTypeField.ModifiedDate = pObjCertificateTypeField.ModifiedDate;

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Message = Ex.Message;
                    }
                }
                else
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Certificate Type Field Not Found";
                }

                return mObjFuncResponse;
            }
        }

        public IList<DropDownListResult> REP_GetCertificateTypeDropDown(Certificate_Types pObjCertificateType)
        {
            using(_db = new EIRSEntities())
            {
                var vResult = (from crt in _db.usp_GetCertificateTypeList(pObjCertificateType.CertificateTypeName, pObjCertificateType.TaxYear, pObjCertificateType.CertificateTypeID, pObjCertificateType.IntStatus)
                               where crt.Active == true
                               select new DropDownListResult()
                               {
                                   id = crt.CertificateTypeID.GetValueOrDefault(),
                                   text = crt.CertificateTypeName
                               }).ToList();

                return vResult;
            }
        }

    }
}
