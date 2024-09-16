using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EIRS.BLL
{
    public class BLTaxPayerMessage
    {
        ITaxPayerMessageRespository _TaxPayerMessageRespository;

        public BLTaxPayerMessage()
        {
            _TaxPayerMessageRespository = new TaxPayerMessageRespository();
        }

        public FuncResponse<MAP_TaxPayer_Message> BL_InsertMessage(MAP_TaxPayer_Message pObjMessage)
        {
            FuncResponse<MAP_TaxPayer_Message> mObjFuncResponse = _TaxPayerMessageRespository.REP_InsertMessage(pObjMessage);

            if (mObjFuncResponse.Success)
            {
                pObjMessage.TPMID = mObjFuncResponse.AdditionalData.TPMID;
                BL_ProcessMessageDocument(pObjMessage);
            }

            return mObjFuncResponse;
        }

        public void BL_ProcessMessageDocument(MAP_TaxPayer_Message pObjMessage)
        {
            IEnumerable<MAP_TaxPayer_Message_Document> lstDocument = (from doc in pObjMessage.MAP_TaxPayer_Message_Document select doc).ToList();

            foreach (MAP_TaxPayer_Message_Document doc in lstDocument)
            {

                string strOldPath = doc.DocumentPath;

                doc.TPMID = pObjMessage.TPMID;
                doc.CreatedDate = pObjMessage.CreatedDate;
                doc.CreatedBy = pObjMessage.CreatedBy;

                var temp = _TaxPayerMessageRespository.REP_AddMessageDocument(pObjMessage, doc);

                string strNewPath = GlobalDefaultValues.DocumentLocation + temp.DocumentPath;
                string strDirectory = GlobalDefaultValues.DocumentLocation + "Messages/" + pObjMessage.TPMID.ToString();

                if (!Directory.Exists(strDirectory))
                {
                    Directory.CreateDirectory(strDirectory);
                }

                if (File.Exists(strOldPath))
                {
                    if (!File.Exists(strNewPath))
                    {
                        File.Move(strOldPath, strNewPath);
                    }
                    else
                    {
                        File.Delete(strNewPath);
                        File.Move(strOldPath, strNewPath);
                    }
                }

            }
        }

        public IList<usp_GetTaxPayerMessageList_Result> BL_GetMessageList(MAP_TaxPayer_Message pObjMessage)
        {
            return _TaxPayerMessageRespository.REP_GetMessageList(pObjMessage);
        }

        public IList<MAP_TaxPayer_Message_Document> BL_GetMessageDocumentList(long plngTPMID)
        {
            return _TaxPayerMessageRespository.REP_GetMessageDocumentList(plngTPMID);
        }

    }
}
