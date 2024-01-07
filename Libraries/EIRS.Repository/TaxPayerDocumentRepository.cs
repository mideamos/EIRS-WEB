using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EIRS.Repository
{
    public interface ITaxPayerDocumentRespoistory
    {
        usp_GetTaxPayerDocumentList_Result REP_GetTaxPayerDocumentDetails(long pIntTPDID);
        IList<usp_GetTaxPayerDocumentList_Result> REP_GetTaxPayerDocumentList(MAP_TaxPayer_Document pObjTaxPayerDocument);
        FuncResponse REP_InsertTaxPayerDocument(MAP_TaxPayer_Document pObjTaxPayerDocument);
        FuncResponse<IList<usp_GetTaxPayerDocumentList_Result>> REP_RemoveTaxPayerDocument(MAP_TaxPayer_Document pObjTaxPayerDocument);
    }

    public class TaxPayerDocumentRespoistory : ITaxPayerDocumentRespoistory
    {
        EIRSEntities _db;

        public FuncResponse REP_InsertTaxPayerDocument(MAP_TaxPayer_Document pObjTaxPayerDocument)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse mObjResponse = new FuncResponse();

                _db.MAP_TaxPayer_Document.Add(pObjTaxPayerDocument);

                try
                {
                    _db.SaveChanges();
                    mObjResponse.Success = true;
                }
                catch (Exception Ex)
                {
                    mObjResponse.Success = false;
                    mObjResponse.Message = Ex.Message;
                }

                return mObjResponse;
            }
        }

        public FuncResponse<IList<usp_GetTaxPayerDocumentList_Result>> REP_RemoveTaxPayerDocument(MAP_TaxPayer_Document pObjTaxPayerDocument)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<IList<usp_GetTaxPayerDocumentList_Result>> mObjFuncResponse = new FuncResponse<IList<usp_GetTaxPayerDocumentList_Result>>(); //Return Object

                MAP_TaxPayer_Document mObjDeleteDocument;

                mObjDeleteDocument = _db.MAP_TaxPayer_Document.Find(pObjTaxPayerDocument.TPDID);

                if (mObjDeleteDocument == null)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Message = "Document Already Removed.";
                }
                else
                {
                    _db.MAP_TaxPayer_Document.Remove(mObjDeleteDocument);

                    try
                    {
                        _db.SaveChanges();
                        mObjFuncResponse.Success = true;
                        mObjFuncResponse.Message = "Document Removed Successfully";
                        mObjFuncResponse.AdditionalData = _db.usp_GetTaxPayerDocumentList(pObjTaxPayerDocument.TaxPayerID, pObjTaxPayerDocument.TaxPayerTypeID, 0).ToList();
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

        public IList<usp_GetTaxPayerDocumentList_Result> REP_GetTaxPayerDocumentList(MAP_TaxPayer_Document pObjTaxPayerDocument)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerDocumentList(pObjTaxPayerDocument.TaxPayerID, pObjTaxPayerDocument.TaxPayerTypeID, pObjTaxPayerDocument.TPDID).ToList();
            }
        }

        public usp_GetTaxPayerDocumentList_Result REP_GetTaxPayerDocumentDetails(long pIntTPDID)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerDocumentList(0, 0, pIntTPDID).FirstOrDefault();
            }
        }


    }
}
