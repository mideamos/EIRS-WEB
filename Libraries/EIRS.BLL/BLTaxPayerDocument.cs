using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;

namespace EIRS.BLL
{
    public class BLTaxPayerDocument
    {
        ITaxPayerDocumentRespoistory _TaxPayerDocumentRespoistory;

        public BLTaxPayerDocument()
        {
            _TaxPayerDocumentRespoistory = new TaxPayerDocumentRespoistory();
        }

        public FuncResponse BL_InsertTaxPayerDocument(MAP_TaxPayer_Document pObjTaxPayerDocument)
        {
            return _TaxPayerDocumentRespoistory.REP_InsertTaxPayerDocument(pObjTaxPayerDocument);
        }

        public FuncResponse<IList<usp_GetTaxPayerDocumentList_Result>> BL_RemoveTaxPayerDocument(MAP_TaxPayer_Document pObjTaxPayerDocument)
        {
            return _TaxPayerDocumentRespoistory.REP_RemoveTaxPayerDocument(pObjTaxPayerDocument);
        }

        public IList<usp_GetTaxPayerDocumentList_Result> BL_GetTaxPayerDocumentList(MAP_TaxPayer_Document pObjTaxPayerDocument)
        {
            return _TaxPayerDocumentRespoistory.REP_GetTaxPayerDocumentList(pObjTaxPayerDocument);
        }

        public usp_GetTaxPayerDocumentList_Result BL_GetTaxPayerDocumentDetails(long pIntTPDID)
        {
            return _TaxPayerDocumentRespoistory.REP_GetTaxPayerDocumentDetails(pIntTPDID);
        }
    }
}
