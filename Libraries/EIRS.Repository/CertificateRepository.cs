using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;

namespace EIRS.Repository
{
    public class CertificateRepository : ICertificateRepository
    {
        EIRSEntities _db;

        public FuncResponse<Certificate> REP_InsertCertificate(Certificate pObjCertificate)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<Certificate> mObjFuncResponse = new FuncResponse<Certificate>(); //Return Object

                Certificate mObjInsertCertificate; //Certificate Insert Object
                mObjInsertCertificate = new Certificate
                {
                    CreatedBy = pObjCertificate.CreatedBy,
                    CreatedDate = pObjCertificate.CreatedDate
                };

                mObjInsertCertificate.CertificateTypeID = pObjCertificate.CertificateTypeID;
                mObjInsertCertificate.TaxPayerTypeID = pObjCertificate.TaxPayerTypeID;
                mObjInsertCertificate.TaxPayerID = pObjCertificate.TaxPayerID;
                mObjInsertCertificate.ProfileID = pObjCertificate.ProfileID;
                mObjInsertCertificate.AssetID = pObjCertificate.AssetID;
                mObjInsertCertificate.AssetTypeID = pObjCertificate.AssetTypeID;
                mObjInsertCertificate.CertificateDate = pObjCertificate.CertificateDate;
                mObjInsertCertificate.ExpiryDate = pObjCertificate.ExpiryDate;
                mObjInsertCertificate.StatusID = pObjCertificate.StatusID;

                _db.Certificates.Add(mObjInsertCertificate);

                //Insert Stages
                var vStageList = _db.MST_CertificateStage;
                MAP_Certificate_Stages mObjCertificateStage;
                foreach (var item in vStageList)
                {
                    mObjCertificateStage = new MAP_Certificate_Stages()
                    {
                        StageID = item.CertificateStageID
                    };

                    mObjInsertCertificate.MAP_Certificate_Stages.Add(mObjCertificateStage);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.Message = "Certificate Added Successfully";
                    mObjFuncResponse.AdditionalData = mObjInsertCertificate;
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;
                    mObjFuncResponse.Message = "Certificate Addition Failed";
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_UpdateCertificate(Certificate pObjCertificate)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse(); //Return Object

                Certificate mObjUpdateCertificate = _db.Certificates.Find(pObjCertificate.CertificateID);

                if (mObjUpdateCertificate == null)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Certificate Not Found";
                }

                mObjUpdateCertificate.ModifiedBy = pObjCertificate.ModifiedBy;
                mObjUpdateCertificate.ModifiedDate = pObjCertificate.ModifiedDate;

                mObjUpdateCertificate.Notes = pObjCertificate.Notes;
                mObjUpdateCertificate.OtherInformation = pObjCertificate.OtherInformation;


                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.Message = "Certificate Updated Successfully";
                }
                catch (Exception Ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = Ex;
                    mObjFuncResponse.Message = "Certificate Updation Failed";
                }

                return mObjFuncResponse;
            }
        }

        public usp_GetCertificateDetails_Result REP_GetCertificateDetails(long plngCertificateID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetCertificateDetails(plngCertificateID).FirstOrDefault();
            }
        }

        public void REP_UpdateCertificatePath(Certificate pObjCertificate)
        {
            using (_db = new EIRSEntities())
            {
                Certificate mObjUpdateRequest;

                if (pObjCertificate.CertificateID != 0)
                {
                    mObjUpdateRequest = (from crt in _db.Certificates
                                         where crt.CertificateID == pObjCertificate.CertificateID
                                         select crt).FirstOrDefault();

                    if (mObjUpdateRequest != null)
                    {
                        mObjUpdateRequest.CertificatePath = pObjCertificate.CertificatePath;
                        mObjUpdateRequest.StatusID = pObjCertificate.StatusID;
                        mObjUpdateRequest.ModifiedBy = pObjCertificate.ModifiedBy;
                        mObjUpdateRequest.ModifiedDate = pObjCertificate.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                        }
                        catch (Exception Ex)
                        {
                        }
                    }
                }
            }
        }

        public FuncResponse REP_UpdateRequestStatus(Certificate pObjCertificate)
        {
            using (_db = new EIRSEntities())
            {
                Certificate mObjUpdateRequest;
                FuncResponse mObjFuncResponse = new FuncResponse();

                if (pObjCertificate.CertificateID != 0)
                {
                    mObjUpdateRequest = (from crt in _db.Certificates
                                         where crt.CertificateID == pObjCertificate.CertificateID
                                         select crt).FirstOrDefault();

                    if (mObjUpdateRequest != null)
                    {
                        mObjUpdateRequest.StatusID = pObjCertificate.StatusID;
                        mObjUpdateRequest.ModifiedBy = pObjCertificate.ModifiedBy;
                        mObjUpdateRequest.ModifiedDate = pObjCertificate.ModifiedDate;

                        try
                        {
                            _db.SaveChanges();
                            mObjFuncResponse.Success = true;
                            mObjFuncResponse.Message = "Status Updated Successfully";
                        }
                        catch (Exception Ex)
                        {
                            mObjFuncResponse.Success = false;
                            mObjFuncResponse.Exception = Ex;
                            mObjFuncResponse.Message = "Status Updation Failed";
                        }
                    }
                }

                return mObjFuncResponse;
            }
        }

        public IDictionary<string, object> REP_SearchCertificate(Certificate pObjCertificate)
        {
            using (_db = new EIRSEntities())
            {

                IDictionary<string, object> dcData = new Dictionary<string, object>
                {
                    ["CertificateList"] = _db.usp_SearchCertificate(pObjCertificate.WhereCondition, pObjCertificate.OrderBy, pObjCertificate.OrderByDirection, pObjCertificate.PageNumber, pObjCertificate.PageSize, pObjCertificate.MainFilter,
                                                                        pObjCertificate.StatusIds).ToList()
                };

                //Get Total Count
                int mIntTotalCount = _db.Database.SqlQuery<int>("Select COUNT(crt.CertificateID) FROM Certificates crt").FirstOrDefault();

                StringBuilder sbFilteredCountQuery = new StringBuilder();
                sbFilteredCountQuery.Append(" SELECT COUNT(crt.CertificateID) ");
                sbFilteredCountQuery.Append(" FROM Certificates crt ");
                sbFilteredCountQuery.Append(" INNER JOIN Certificate_Types crtype ON crt.CertificateTypeID = crtype.CertificateTypeID ");
                sbFilteredCountQuery.Append(" INNER JOIN MST_CertificateStatus cstat ON crt.StatusID = cstat.CertificateStatusID ");
                sbFilteredCountQuery.Append(" INNER JOIN TaxPayer_Types tptype ON tptype.TaxPayerTypeID = crt.TaxPayerTypeID ");
                sbFilteredCountQuery.Append(" INNER JOIN Tax_Offices toff ON dbo.GetTaxPayerTaxOfficeID(crt.TaxPayerID,crt.TaxPayerTypeID) = toff.TaxOfficeID ");
                sbFilteredCountQuery.Append(" WHERE 1 = 1 ");
                sbFilteredCountQuery.Append(pObjCertificate.WhereCondition);

                SqlParameter[] mObjSqlParameter = new SqlParameter[]
                {
                   new SqlParameter("@MainFilter",pObjCertificate.MainFilter),
                   new SqlParameter("@StatusID",pObjCertificate.StatusIds)
                };

                //Get Filtered Count
                int mIntFilteredCount = _db.Database.SqlQuery<int>(sbFilteredCountQuery.ToString(), mObjSqlParameter).FirstOrDefault();

                dcData["TotalRecords"] = mIntTotalCount;
                dcData["FilteredRecords"] = mIntFilteredCount;

                return dcData;
            }
        }

        public FuncResponse REP_InsertUpdateCertificateField(MAP_Certificate_CustomField pObjCustomField)
        {
            using (_db = new EIRSEntities())
            {
                bool isNewRecord = false;
                FuncResponse mObjFuncResponse = new FuncResponse();
                MAP_Certificate_CustomField mObjInsertUpdateCustomField;
                var vExist = (from gf in _db.MAP_Certificate_CustomField
                              where gf.CTFID == pObjCustomField.CTFID
                              && gf.CertificateID == pObjCustomField.CertificateID
                              select gf);

                if (vExist.Count() > 0)
                {
                    mObjInsertUpdateCustomField = vExist.First();
                }
                else
                {
                    isNewRecord = true;
                    mObjInsertUpdateCustomField = new MAP_Certificate_CustomField();
                }

                mObjInsertUpdateCustomField.CTFID = pObjCustomField.CTFID;
                mObjInsertUpdateCustomField.CertificateID = pObjCustomField.CertificateID;
                mObjInsertUpdateCustomField.FieldValue = pObjCustomField.FieldValue;
                mObjInsertUpdateCustomField.Active = pObjCustomField.Active;
                mObjInsertUpdateCustomField.CreatedBy = pObjCustomField.CreatedBy;
                mObjInsertUpdateCustomField.CreatedDate = pObjCustomField.CreatedDate;

                if (isNewRecord)
                {
                    _db.MAP_Certificate_CustomField.Add(mObjInsertUpdateCustomField);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                }
                catch (Exception e)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = e.Message;
                }

                return mObjFuncResponse;
            }
        }

        public IList<usp_GetCertificateFieldList_Result> REP_GetCertificateField(Certificate pObjCertificate)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetCertificateFieldList(pObjCertificate.CertificateTypeID, pObjCertificate.CertificateID).ToList();
            }
        }

        public IList<usp_GetAssessmentRuleInformationForCertificate_Result> REP_GetAssessmentRuleInformationForCertificate(Certificate pObjCertificate)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAssessmentRuleInformationForCertificate(pObjCertificate.CertificateID).ToList();
            }
        }

        public IList<usp_GetCertificateItemList_Result> REP_GetCertificateItem(Certificate pObjCertificate)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetCertificateItemList(pObjCertificate.CertificateID).ToList();
            }
        }

        public IList<usp_GetAdminCertificateStageList_Result> REP_GetCertificateStageList(long plngCertificateID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetAdminCertificateStageList(plngCertificateID).ToList();
            }
        }

        public usp_GetCertificateDetailForGenerate_Result REP_GetCertificateDetailForGenerateProcess(long plngCertificateID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetCertificateDetailForGenerate(plngCertificateID).FirstOrDefault();
            }
        }

        public MAP_Certificate_Generate REP_GetCertificateGenerateDetails(long plngCertificateID)
        {
            using (_db = new EIRSEntities())
            {
                var vData = (from or in _db.MAP_Certificate_Generate
                             where or.CertificateID == plngCertificateID
                             select or).FirstOrDefault();

                return vData;
            }
        }

        public FuncResponse<MAP_Certificate_Generate> REP_InsertUpdateCertificateGenerate(MAP_Certificate_Generate pObjCertificateGenerate)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<MAP_Certificate_Generate> mObjFuncResponse = new FuncResponse<MAP_Certificate_Generate>();
                Certificate mObjUpdateCertificate;
                MAP_Certificate_Generate mObjUpdateCertificateGenerate;


                if (pObjCertificateGenerate.CGID != 0)
                {
                    mObjUpdateCertificateGenerate = (from mog in _db.MAP_Certificate_Generate
                                                     where mog.CGID == pObjCertificateGenerate.CGID
                                                     select mog).FirstOrDefault();

                    if (mObjUpdateCertificateGenerate != null)
                    {
                        mObjUpdateCertificateGenerate.ModifiedBy = pObjCertificateGenerate.ModifiedBy;
                        mObjUpdateCertificateGenerate.ModifiedDate = pObjCertificateGenerate.ModifiedDate;
                    }
                    else
                    {
                        mObjUpdateCertificateGenerate = new MAP_Certificate_Generate();
                        mObjUpdateCertificateGenerate.CreatedBy = pObjCertificateGenerate.CreatedBy;
                        mObjUpdateCertificateGenerate.CreatedDate = pObjCertificateGenerate.CreatedDate;
                    }
                }
                else
                {
                    mObjUpdateCertificateGenerate = new MAP_Certificate_Generate();
                    mObjUpdateCertificateGenerate.CreatedBy = pObjCertificateGenerate.CreatedBy;
                    mObjUpdateCertificateGenerate.CreatedDate = pObjCertificateGenerate.CreatedDate;
                }

                mObjUpdateCertificateGenerate.CertificateID = pObjCertificateGenerate.CertificateID;
                mObjUpdateCertificateGenerate.Reason = pObjCertificateGenerate.Reason ?? mObjUpdateCertificateGenerate.Reason;
                mObjUpdateCertificateGenerate.Location = pObjCertificateGenerate.Location ?? mObjUpdateCertificateGenerate.Location;
                mObjUpdateCertificateGenerate.ExpiryDate = pObjCertificateGenerate.ExpiryDate ?? mObjUpdateCertificateGenerate.ExpiryDate;
                mObjUpdateCertificateGenerate.IsExpirable = pObjCertificateGenerate.IsExpirable ?? mObjUpdateCertificateGenerate.IsExpirable;

                if (pObjCertificateGenerate.CGID == 0)
                {
                    _db.MAP_Certificate_Generate.Add(mObjUpdateCertificateGenerate);
                }
                try
                {
                    _db.SaveChanges();

                    mObjUpdateCertificate = (from or in _db.Certificates where (or.CertificateID == pObjCertificateGenerate.CertificateID) select or).FirstOrDefault();
                    if (pObjCertificateGenerate.IsAction)
                    {
                        mObjUpdateCertificate.StatusID = (int)EnumList.CertificateStatus.Generated;
                        mObjUpdateCertificate.SEDE_DocumentID = pObjCertificateGenerate.SEDE_DocumentID;
                        mObjUpdateCertificate.GeneratedPath = pObjCertificateGenerate.GeneratedPath;

                        MAP_Certificate_Stages mObjUpdateRequestStage = (from os in _db.MAP_Certificate_Stages
                                                                         where os.CertificateID == pObjCertificateGenerate.CertificateID
                                                                         && os.StageID == pObjCertificateGenerate.StageID
                                                                         select os).FirstOrDefault();

                        if (mObjUpdateRequestStage != null)
                        {
                            mObjUpdateRequestStage.ApprovalDate = pObjCertificateGenerate.ApprovalDate;
                        }

                        _db.SaveChanges();


                    }
                    else //Update PDF Template ID
                    {
                        mObjUpdateCertificate.PDFTemplateID = pObjCertificateGenerate.PDFTemplateID ?? mObjUpdateCertificate.PDFTemplateID;
                        _db.SaveChanges();
                    }

                    mObjFuncResponse.Success = true;

                    if (pObjCertificateGenerate.CGID == 0)
                    {
                        mObjFuncResponse.Message = "Certificate Generated Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Certificate Updated Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjUpdateCertificateGenerate;
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;

                    mObjFuncResponse.Exception = ex;

                    if (pObjCertificateGenerate.CGID == 0)
                    {
                        mObjFuncResponse.Message = "Certificate Generation Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Certificate Updation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_InsertUpdateGenerateField(MAP_Certificate_Generate_Field pObjGenerateField)
        {
            using (_db = new EIRSEntities())
            {
                bool isNewRecord = false;
                FuncResponse mObjFuncResponse = new FuncResponse();
                MAP_Certificate_Generate_Field mObjInsertUpdateGenerateField;
                var vExist = (from gf in _db.MAP_Certificate_Generate_Field
                              where gf.FieldID == pObjGenerateField.FieldID
                              && gf.CGID == pObjGenerateField.CGID
                              select gf);

                if (vExist.Count() > 0)
                {
                    mObjInsertUpdateGenerateField = vExist.First();
                }
                else
                {
                    isNewRecord = true;
                    mObjInsertUpdateGenerateField = new MAP_Certificate_Generate_Field();
                }

                mObjInsertUpdateGenerateField.CGID = pObjGenerateField.CGID;
                mObjInsertUpdateGenerateField.FieldID = pObjGenerateField.FieldID;
                mObjInsertUpdateGenerateField.PFID = pObjGenerateField.PFID;
                mObjInsertUpdateGenerateField.FieldValue = pObjGenerateField.FieldValue;
                mObjInsertUpdateGenerateField.Active = pObjGenerateField.Active;
                mObjInsertUpdateGenerateField.CreatedBy = pObjGenerateField.CreatedBy;
                mObjInsertUpdateGenerateField.CreatedDate = pObjGenerateField.CreatedDate;

                if (isNewRecord)
                {
                    _db.MAP_Certificate_Generate_Field.Add(mObjInsertUpdateGenerateField);
                }

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.Success = true;
                }
                catch (Exception e)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = e.Message;
                }

                return mObjFuncResponse;
            }
        }

        public MAP_Certificate_Validate REP_GetCertificateValidateDetails(long plngCertificateID)
        {
            using (_db = new EIRSEntities())
            {
                var vData = (from ov in _db.MAP_Certificate_Validate
                             where ov.CertificateID == plngCertificateID
                             select ov).FirstOrDefault();

                return vData;
            }
        }

        public FuncResponse<MAP_Certificate_Validate> REP_InsertUpdateCertificateValidate(MAP_Certificate_Validate pObjCertificateValidate)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<MAP_Certificate_Validate> mObjFuncResponse = new FuncResponse<MAP_Certificate_Validate>();
                Certificate mObjUpdateCertificate;
                MAP_Certificate_Validate mObjUpdateCertificateValidate;


                if (pObjCertificateValidate.CVID != 0)
                {
                    mObjUpdateCertificateValidate = (from mog in _db.MAP_Certificate_Validate
                                                     where mog.CVID == pObjCertificateValidate.CVID
                                                     select mog).FirstOrDefault();

                    if (mObjUpdateCertificateValidate != null)
                    {
                        mObjUpdateCertificateValidate.ModifiedBy = pObjCertificateValidate.ModifiedBy;
                        mObjUpdateCertificateValidate.ModifiedDate = pObjCertificateValidate.ModifiedDate;
                    }
                    else
                    {
                        mObjUpdateCertificateValidate = new MAP_Certificate_Validate();
                        mObjUpdateCertificateValidate.CreatedBy = pObjCertificateValidate.CreatedBy;
                        mObjUpdateCertificateValidate.CreatedDate = pObjCertificateValidate.CreatedDate;
                    }
                }
                else
                {
                    mObjUpdateCertificateValidate = new MAP_Certificate_Validate();
                    mObjUpdateCertificateValidate.CreatedBy = pObjCertificateValidate.CreatedBy;
                    mObjUpdateCertificateValidate.CreatedDate = pObjCertificateValidate.CreatedDate;
                }


                mObjUpdateCertificateValidate.CertificateID = pObjCertificateValidate.CertificateID;
                mObjUpdateCertificateValidate.Notes = pObjCertificateValidate.Notes ?? mObjUpdateCertificateValidate.Notes;


                if (pObjCertificateValidate.CVID == 0)
                {
                    _db.MAP_Certificate_Validate.Add(mObjUpdateCertificateValidate);
                }
                try
                {
                    _db.SaveChanges();

                    if (pObjCertificateValidate.IsAction)
                    {
                        mObjUpdateCertificate = (from or in _db.Certificates where (or.CertificateID == pObjCertificateValidate.CertificateID) select or).FirstOrDefault();

                        if (mObjUpdateCertificate != null)
                        {
                            mObjUpdateCertificate.StatusID = (int)EnumList.CertificateStatus.Validated;
                            mObjUpdateCertificate.SEDE_OrderID = pObjCertificateValidate.SEDE_OrderID;
                            mObjUpdateCertificate.ValidatedPath = pObjCertificateValidate.ValidatedPath;

                        }




                        MAP_Certificate_Stages mObjUpdateRequestStage = (from os in _db.MAP_Certificate_Stages
                                                                         where os.CertificateID == pObjCertificateValidate.CertificateID
                                                                         && os.StageID == pObjCertificateValidate.StageID
                                                                         select os).FirstOrDefault();

                        if (mObjUpdateRequestStage != null)
                        {
                            mObjUpdateRequestStage.ApprovalDate = pObjCertificateValidate.ApprovalDate;
                        }

                        _db.SaveChanges();

                    }

                    mObjFuncResponse.Success = true;

                    if (pObjCertificateValidate.CVID == 0)
                    {
                        mObjFuncResponse.Message = "Certificate Validated Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Certificate Updated Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjUpdateCertificateValidate;
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;

                    mObjFuncResponse.Exception = ex;

                    if (pObjCertificateValidate.CVID == 0)
                    {
                        mObjFuncResponse.Message = "Certificate Validation Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Certificate Validation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public MAP_Certificate_SignVisible REP_GetCertificateSignVisibleDetails(long plngCertificateID, int UserID, int StageID = 0)
        {
            using (_db = new EIRSEntities())
            {
                var vData = (from ov in _db.MAP_Certificate_SignVisible
                             where ov.CertificateID == plngCertificateID
                             && ov.UserID == UserID
                             && ov.StageID == StageID
                             select ov).FirstOrDefault();

                return vData;
            }
        }

        public IList<MAP_Certificate_SignVisible> REP_GetCertificateSignVisibleList(long plngCertificateID)
        {
            using (_db = new EIRSEntities())
            {
                var vData = (from ov in _db.MAP_Certificate_SignVisible
                             where ov.CertificateID == plngCertificateID
                             select ov).ToList();

                return vData;
            }
        }

        public FuncResponse<MAP_Certificate_SignVisible> REP_InsertUpdateCertificateSignVisible(MAP_Certificate_SignVisible pObjCertificateSignVisible)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<MAP_Certificate_SignVisible> mObjFuncResponse = new FuncResponse<MAP_Certificate_SignVisible>();
                Certificate mObjUpdateCertificate;
                MAP_Certificate_SignVisible mObjUpdateCertificateSignVisible;


                if (pObjCertificateSignVisible.CSVID != 0)
                {
                    mObjUpdateCertificateSignVisible = (from mog in _db.MAP_Certificate_SignVisible
                                                        where mog.CSVID == pObjCertificateSignVisible.CSVID
                                                        select mog).FirstOrDefault();

                    if (mObjUpdateCertificateSignVisible != null)
                    {
                        mObjUpdateCertificateSignVisible.ModifiedBy = pObjCertificateSignVisible.ModifiedBy;
                        mObjUpdateCertificateSignVisible.ModifiedDate = pObjCertificateSignVisible.ModifiedDate;
                    }
                    else
                    {
                        mObjUpdateCertificateSignVisible = new MAP_Certificate_SignVisible();
                        mObjUpdateCertificateSignVisible.CreatedBy = pObjCertificateSignVisible.CreatedBy;
                        mObjUpdateCertificateSignVisible.CreatedDate = pObjCertificateSignVisible.CreatedDate;
                    }
                }
                else
                {
                    mObjUpdateCertificateSignVisible = new MAP_Certificate_SignVisible();
                    mObjUpdateCertificateSignVisible.CreatedBy = pObjCertificateSignVisible.CreatedBy;
                    mObjUpdateCertificateSignVisible.CreatedDate = pObjCertificateSignVisible.CreatedDate;
                }


                mObjUpdateCertificateSignVisible.CertificateID = pObjCertificateSignVisible.CertificateID;
                mObjUpdateCertificateSignVisible.UserID = pObjCertificateSignVisible.UserID;
                mObjUpdateCertificateSignVisible.SignDate = pObjCertificateSignVisible.SignDate;
                mObjUpdateCertificateSignVisible.Notes = pObjCertificateSignVisible.Notes ?? mObjUpdateCertificateSignVisible.Notes;
                mObjUpdateCertificateSignVisible.SignSourceID = pObjCertificateSignVisible.SignSourceID;
                mObjUpdateCertificateSignVisible.DocumentWidth = pObjCertificateSignVisible.DocumentWidth;
                mObjUpdateCertificateSignVisible.AdditionalSignatureLocation = pObjCertificateSignVisible.AdditionalSignatureLocation;

                if (pObjCertificateSignVisible.CSVID == 0)
                {
                    _db.MAP_Certificate_SignVisible.Add(mObjUpdateCertificateSignVisible);
                }
                try
                {
                    _db.SaveChanges();

                    if (pObjCertificateSignVisible.IsAction)
                    {
                        mObjUpdateCertificate = (from or in _db.Certificates where (or.CertificateID == pObjCertificateSignVisible.CertificateID) select or).FirstOrDefault();

                        if (mObjUpdateCertificate != null)
                        {

                            if (mObjUpdateCertificate.VisibleSignStatusID == 2)
                            {
                                mObjUpdateCertificate.StatusID = (int)EnumList.CertificateStatus.Signed_Visible;


                                MAP_Certificate_Stages mObjUpdateRequestStage = (from os in _db.MAP_Certificate_Stages
                                                                                 where os.CertificateID == pObjCertificateSignVisible.CertificateID
                                                                                 && os.StageID == pObjCertificateSignVisible.Request_StageID
                                                                                 select os).FirstOrDefault();

                                if (mObjUpdateRequestStage != null)
                                {
                                    mObjUpdateRequestStage.ApprovalDate = pObjCertificateSignVisible.ApprovalDate;
                                }
                            }

                            mObjUpdateCertificate.VisibleSignStatusID = mObjUpdateCertificate.VisibleSignStatusID.GetValueOrDefault() + 1;
                            mObjUpdateCertificate.SignedVisiblePath = pObjCertificateSignVisible.SignedVisiblePath;
                            mObjUpdateCertificateSignVisible.StageID = mObjUpdateCertificate.VisibleSignStatusID;

                            _db.SaveChanges();
                        }
                    }

                    mObjFuncResponse.Success = true;

                    if (pObjCertificateSignVisible.CSVID == 0)
                    {
                        mObjFuncResponse.Message = "Certificate Signed Visible Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Certificate Signed Visible Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjUpdateCertificateSignVisible;
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;

                    mObjFuncResponse.Exception = ex;

                    if (pObjCertificateSignVisible.CSVID == 0)
                    {
                        mObjFuncResponse.Message = "Certificate Signed Visible Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Certificate Signed Visible Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public MAP_Certificate_SignDigital REP_GetCertificateSignDigitalDetails(long plngCertificateID)
        {
            using (_db = new EIRSEntities())
            {
                var vData = (from ov in _db.MAP_Certificate_SignDigital
                             where ov.CertificateID == plngCertificateID
                             select ov).FirstOrDefault();

                return vData;
            }
        }

        public FuncResponse<MAP_Certificate_SignDigital> REP_InsertUpdateCertificateSignDigital(MAP_Certificate_SignDigital pObjCertificateSignDigital)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<MAP_Certificate_SignDigital> mObjFuncResponse = new FuncResponse<MAP_Certificate_SignDigital>();
                Certificate mObjUpdateCertificate;
                MAP_Certificate_SignDigital mObjUpdateCertificateSignDigital;


                if (pObjCertificateSignDigital.CSDID != 0)
                {
                    mObjUpdateCertificateSignDigital = (from mog in _db.MAP_Certificate_SignDigital
                                                        where mog.CSDID == pObjCertificateSignDigital.CSDID
                                                        select mog).FirstOrDefault();

                    if (mObjUpdateCertificateSignDigital != null)
                    {
                        mObjUpdateCertificateSignDigital.ModifiedBy = pObjCertificateSignDigital.ModifiedBy;
                        mObjUpdateCertificateSignDigital.ModifiedDate = pObjCertificateSignDigital.ModifiedDate;
                    }
                    else
                    {
                        mObjUpdateCertificateSignDigital = new MAP_Certificate_SignDigital();
                        mObjUpdateCertificateSignDigital.CreatedBy = pObjCertificateSignDigital.CreatedBy;
                        mObjUpdateCertificateSignDigital.CreatedDate = pObjCertificateSignDigital.CreatedDate;
                    }
                }
                else
                {
                    mObjUpdateCertificateSignDigital = new MAP_Certificate_SignDigital();
                    mObjUpdateCertificateSignDigital.CreatedBy = pObjCertificateSignDigital.CreatedBy;
                    mObjUpdateCertificateSignDigital.CreatedDate = pObjCertificateSignDigital.CreatedDate;
                }


                mObjUpdateCertificateSignDigital.CertificateID = pObjCertificateSignDigital.CertificateID;
                mObjUpdateCertificateSignDigital.Notes = pObjCertificateSignDigital.Notes ?? mObjUpdateCertificateSignDigital.Notes;


                if (pObjCertificateSignDigital.CSDID == 0)
                {
                    _db.MAP_Certificate_SignDigital.Add(mObjUpdateCertificateSignDigital);
                }
                try
                {
                    _db.SaveChanges();

                    if (pObjCertificateSignDigital.IsAction)
                    {
                        mObjUpdateCertificate = (from or in _db.Certificates where (or.CertificateID == pObjCertificateSignDigital.CertificateID) select or).FirstOrDefault();

                        if (mObjUpdateCertificate != null)
                        {
                            mObjUpdateCertificate.StatusID = (int)EnumList.CertificateStatus.Signed_Digital;
                            mObjUpdateCertificate.SignedDigitalPath = pObjCertificateSignDigital.SignedDigitalPath;

                        }

                        MAP_Certificate_Stages mObjUpdateRequestStage = (from os in _db.MAP_Certificate_Stages
                                                                         where os.CertificateID == pObjCertificateSignDigital.CertificateID
                                                                         && os.StageID == pObjCertificateSignDigital.StageID
                                                                         select os).FirstOrDefault();

                        if (mObjUpdateRequestStage != null)
                        {
                            mObjUpdateRequestStage.ApprovalDate = pObjCertificateSignDigital.ApprovalDate;
                        }

                        _db.SaveChanges();

                    }

                    mObjFuncResponse.Success = true;

                    if (pObjCertificateSignDigital.CSDID == 0)
                    {
                        mObjFuncResponse.Message = "Certificate Signed Digital Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Certificate Signed Digital Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjUpdateCertificateSignDigital;
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;

                    mObjFuncResponse.Exception = ex;

                    if (pObjCertificateSignDigital.CSDID == 0)
                    {
                        mObjFuncResponse.Message = "Certificate Siging Digital Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Certificate Siging Digital Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public MAP_Certificate_Seal REP_GetCertificateSealDetails(long plngCertificateID)
        {
            using (_db = new EIRSEntities())
            {
                var vData = (from os in _db.MAP_Certificate_Seal
                             where os.CertificateID == plngCertificateID
                             select os).FirstOrDefault();

                return vData;
            }
        }

        public FuncResponse<MAP_Certificate_Seal> REP_InsertUpdateCertificateSeal(MAP_Certificate_Seal pObjCertificateSeal)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<MAP_Certificate_Seal> mObjFuncResponse = new FuncResponse<MAP_Certificate_Seal>();
                Certificate mObjUpdateCertificate;
                MAP_Certificate_Seal mObjUpdateCertificateSeal;


                if (pObjCertificateSeal.CSID != 0)
                {
                    mObjUpdateCertificateSeal = (from mos in _db.MAP_Certificate_Seal
                                                 where mos.CSID == pObjCertificateSeal.CSID
                                                 select mos).FirstOrDefault();

                    if (mObjUpdateCertificateSeal != null)
                    {
                        mObjUpdateCertificateSeal.ModifiedBy = pObjCertificateSeal.ModifiedBy;
                        mObjUpdateCertificateSeal.ModifiedDate = pObjCertificateSeal.ModifiedDate;
                    }
                    else
                    {
                        mObjUpdateCertificateSeal = new MAP_Certificate_Seal();
                        mObjUpdateCertificateSeal.CreatedBy = pObjCertificateSeal.CreatedBy;
                        mObjUpdateCertificateSeal.CreatedDate = pObjCertificateSeal.CreatedDate;
                    }
                }
                else
                {
                    mObjUpdateCertificateSeal = new MAP_Certificate_Seal();
                    mObjUpdateCertificateSeal.CreatedBy = pObjCertificateSeal.CreatedBy;
                    mObjUpdateCertificateSeal.CreatedDate = pObjCertificateSeal.CreatedDate;
                }


                mObjUpdateCertificateSeal.CertificateID = pObjCertificateSeal.CertificateID;
                mObjUpdateCertificateSeal.Notes = pObjCertificateSeal.Notes ?? mObjUpdateCertificateSeal.Notes;

                if (pObjCertificateSeal.CSID == 0)
                {
                    _db.MAP_Certificate_Seal.Add(mObjUpdateCertificateSeal);
                }
                try
                {
                    _db.SaveChanges();
                    if (pObjCertificateSeal.IsAction)
                    {
                        mObjUpdateCertificate = (from or in _db.Certificates where (or.CertificateID == pObjCertificateSeal.CertificateID) select or).FirstOrDefault();

                        if (mObjUpdateCertificate != null)
                        {
                            mObjUpdateCertificate.StatusID = (int)EnumList.CertificateStatus.Sealed;
                            mObjUpdateCertificate.SealedPath = pObjCertificateSeal.SealedPath;
                        }

                        MAP_Certificate_Stages mObjUpdateRequestStage = (from os in _db.MAP_Certificate_Stages
                                                                         where os.CertificateID == pObjCertificateSeal.CertificateID
                                                                         && os.StageID == pObjCertificateSeal.StageID
                                                                         select os).FirstOrDefault();

                        if (mObjUpdateRequestStage != null)
                        {
                            mObjUpdateRequestStage.ApprovalDate = pObjCertificateSeal.ApprovalDate;
                        }

                        _db.SaveChanges();


                    }

                    mObjFuncResponse.Success = true;

                    if (pObjCertificateSeal.CSID == 0)
                    {
                        mObjFuncResponse.Message = "Certificate Sealed Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Certificate Updated Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjUpdateCertificateSeal;
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;

                    mObjFuncResponse.Exception = ex;

                    if (pObjCertificateSeal.CSID == 0)
                    {
                        mObjFuncResponse.Message = "Certificate Seal Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Certificate Updation Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public MAP_Certificate_Issue REP_GetCertificateIssueDetails(long plngCertificateID)
        {
            using (_db = new EIRSEntities())
            {
                var vData = (from os in _db.MAP_Certificate_Issue
                             where os.CertificateID == plngCertificateID
                             select os).FirstOrDefault();

                return vData;
            }
        }

        public FuncResponse<MAP_Certificate_Issue> REP_InsertUpdateCertificateIssue(MAP_Certificate_Issue pObjCertificateIssue)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<MAP_Certificate_Issue> mObjFuncResponse = new FuncResponse<MAP_Certificate_Issue>();
                Certificate mObjUpdateCertificate;
                MAP_Certificate_Issue mObjUpdateCertificateIssue;


                if (pObjCertificateIssue.CIID != 0)
                {
                    mObjUpdateCertificateIssue = (from mos in _db.MAP_Certificate_Issue
                                                  where mos.CIID == pObjCertificateIssue.CIID
                                                  select mos).FirstOrDefault();

                    if (mObjUpdateCertificateIssue != null)
                    {
                        mObjUpdateCertificateIssue.ModifiedBy = pObjCertificateIssue.ModifiedBy;
                        mObjUpdateCertificateIssue.ModifiedDate = pObjCertificateIssue.ModifiedDate;
                    }
                    else
                    {
                        mObjUpdateCertificateIssue = new MAP_Certificate_Issue();
                        mObjUpdateCertificateIssue.CreatedBy = pObjCertificateIssue.CreatedBy;
                        mObjUpdateCertificateIssue.CreatedDate = pObjCertificateIssue.CreatedDate;
                    }
                }
                else
                {
                    mObjUpdateCertificateIssue = new MAP_Certificate_Issue();
                    mObjUpdateCertificateIssue.CreatedBy = pObjCertificateIssue.CreatedBy;
                    mObjUpdateCertificateIssue.CreatedDate = pObjCertificateIssue.CreatedDate;
                }


                mObjUpdateCertificateIssue.CertificateID = pObjCertificateIssue.CertificateID;
                mObjUpdateCertificateIssue.Notes = pObjCertificateIssue.Notes ?? mObjUpdateCertificateIssue.Notes;

                if (pObjCertificateIssue.CIID == 0)
                {
                    _db.MAP_Certificate_Issue.Add(mObjUpdateCertificateIssue);
                }
                try
                {
                    _db.SaveChanges();
                    if (pObjCertificateIssue.IsAction)
                    {
                        mObjUpdateCertificate = (from or in _db.Certificates where (or.CertificateID == pObjCertificateIssue.CertificateID) select or).FirstOrDefault();

                        if (mObjUpdateCertificate != null)
                        {
                            mObjUpdateCertificate.StatusID = (int)EnumList.CertificateStatus.Issued;
                        }

                        MAP_Certificate_Stages mObjUpdateRequestStage = (from os in _db.MAP_Certificate_Stages
                                                                         where os.CertificateID == pObjCertificateIssue.CertificateID
                                                                         && os.StageID == pObjCertificateIssue.StageID
                                                                         select os).FirstOrDefault();

                        if (mObjUpdateRequestStage != null)
                        {
                            mObjUpdateRequestStage.ApprovalDate = pObjCertificateIssue.ApprovalDate;
                        }

                        _db.SaveChanges();


                    }

                    mObjFuncResponse.Success = true;

                    if (pObjCertificateIssue.CIID == 0)
                    {
                        mObjFuncResponse.Message = "Certificate Issued Successfully";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Certificate Issued Successfully";
                    }

                    mObjFuncResponse.AdditionalData = mObjUpdateCertificateIssue;
                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;

                    mObjFuncResponse.Exception = ex;

                    if (pObjCertificateIssue.CIID == 0)
                    {
                        mObjFuncResponse.Message = "Certificate Issue Failed";
                    }
                    else
                    {
                        mObjFuncResponse.Message = "Certificate Issue Failed";
                    }
                }

                return mObjFuncResponse;
            }
        }

        public FuncResponse REP_RevokeCertificate(MAP_Certificate_Revoke pObjRevoke)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjFuncResponse = new FuncResponse();

                Certificate mObjCertificate = _db.Certificates.Find(pObjRevoke.CertificateID);

                if (mObjCertificate != null)
                {
                    mObjCertificate.StatusID = (int)EnumList.CertificateStage.Generate;
                    mObjCertificate.VisibleSignStatusID = null;
                    mObjCertificate.PDFTemplateID = null;
                    mObjCertificate.GeneratedPath = null;
                    mObjCertificate.ValidatedPath = null;
                    mObjCertificate.SignedVisiblePath = null;
                    mObjCertificate.SignedDigitalPath = null;
                    mObjCertificate.SealedPath = null;
                    mObjCertificate.SEDE_DocumentID = null;
                    mObjCertificate.SEDE_OrderID = null;

                    _db.MAP_Certificate_Revoke.Add(pObjRevoke);

                    MAP_Certificate_Generate mObjRequestGenerate = _db.MAP_Certificate_Generate.Where(t => t.CertificateID == pObjRevoke.CertificateID).FirstOrDefault();
                    if (mObjRequestGenerate != null)
                    {
                        _db.MAP_Certificate_Generate.Remove(mObjRequestGenerate);

                        if (!string.IsNullOrWhiteSpace(mObjRequestGenerate.GeneratedPath))
                        {
                            if (File.Exists(GlobalDefaultValues.DocumentLocation + mObjRequestGenerate.GeneratedPath))
                            {
                                File.Delete(GlobalDefaultValues.DocumentLocation + mObjRequestGenerate.GeneratedPath);
                            }
                        }

                        var vGenerateField = _db.MAP_Certificate_Generate_Field.Where(t => t.CGID == mObjRequestGenerate.CGID);
                        if (vGenerateField != null)
                        {
                            _db.MAP_Certificate_Generate_Field.RemoveRange(vGenerateField);
                        }
                    }

                    MAP_Certificate_Validate mObjRequestValidate = _db.MAP_Certificate_Validate.Where(t => t.CertificateID == pObjRevoke.CertificateID).FirstOrDefault();
                    if (mObjRequestValidate != null)
                    {
                        _db.MAP_Certificate_Validate.Remove(mObjRequestValidate);
                        //Delete Documents
                        if (!string.IsNullOrWhiteSpace(mObjRequestValidate.ValidatedPath))
                        {
                            if (File.Exists(GlobalDefaultValues.DocumentLocation + mObjRequestValidate.ValidatedPath))
                            {
                                File.Delete(GlobalDefaultValues.DocumentLocation + mObjRequestValidate.ValidatedPath);
                            }
                        }
                    }

                    var lstRequestSignVisible = _db.MAP_Certificate_SignVisible.Where(t => t.CertificateID == pObjRevoke.CertificateID);
                    if (lstRequestSignVisible != null)
                    {
                        _db.MAP_Certificate_SignVisible.RemoveRange(lstRequestSignVisible);
                    }

                    MAP_Certificate_SignDigital mObjRequestSignDigital = _db.MAP_Certificate_SignDigital.Where(t => t.CertificateID == pObjRevoke.CertificateID).FirstOrDefault();
                    if (mObjRequestSignDigital != null)
                    {
                        _db.MAP_Certificate_SignDigital.Remove(mObjRequestSignDigital);

                        //Delete Documents
                        if (!string.IsNullOrWhiteSpace(mObjRequestSignDigital.SignedDigitalPath))
                        {
                            if (File.Exists(GlobalDefaultValues.DocumentLocation + mObjRequestSignDigital.SignedDigitalPath))
                            {
                                File.Delete(GlobalDefaultValues.DocumentLocation + mObjRequestSignDigital.SignedDigitalPath);
                            }
                        }
                    }

                    MAP_Certificate_Seal mObjRequestSeal = _db.MAP_Certificate_Seal.Where(t => t.CertificateID == pObjRevoke.CertificateID).FirstOrDefault();
                    if (mObjRequestSeal != null)
                    {
                        _db.MAP_Certificate_Seal.Remove(mObjRequestSeal);
                        //Delete Documents
                        if (!string.IsNullOrWhiteSpace(mObjRequestSeal.SealedPath))
                        {
                            if (File.Exists(GlobalDefaultValues.DocumentLocation + mObjRequestSeal.SealedPath))
                            {
                                File.Delete(GlobalDefaultValues.DocumentLocation + mObjRequestSeal.SealedPath);
                            }
                        }
                    }

                    MAP_Certificate_Issue mObjRequestIssue = _db.MAP_Certificate_Issue.Where(t => t.CertificateID == pObjRevoke.CertificateID).FirstOrDefault();
                    if (mObjRequestIssue != null)
                    {
                        _db.MAP_Certificate_Issue.Remove(mObjRequestIssue);
                    }

                    var vRequestStage = _db.MAP_Certificate_Stages.Where(t => t.CertificateID == pObjRevoke.CertificateID);
                    foreach (var stg in vRequestStage)
                    {
                        stg.ApprovalDate = null;
                    }

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                        mObjFuncResponse.Message = "Certificate Revoked Successfully";

                    }
                    catch (Exception Ex)
                    {
                        mObjFuncResponse.Success = false;
                        mObjFuncResponse.Exception = Ex;
                        mObjFuncResponse.Message = "Certificate Revoked Failed";
                    }
                }
                else
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Request Not Found";
                }

                return mObjFuncResponse;
            }
        }

    }
}
