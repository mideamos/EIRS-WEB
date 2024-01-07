using EIRS.BOL;
using EIRS.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EIRS.Repository
{
    public interface ITaxPayerMessageRespository
    {
        MAP_TaxPayer_Message_Document REP_AddMessageDocument(MAP_TaxPayer_Message pObjMessage, MAP_TaxPayer_Message_Document pObjMessageDocument);
        void REP_DeleteMessageDocument(MAP_TaxPayer_Message_Document pObjMessageDocument);
        IList<MAP_TaxPayer_Message_Document> REP_GetMessageDocumentList(long plngTPMID);
        IList<usp_GetTaxPayerMessageList_Result> REP_GetMessageList(MAP_TaxPayer_Message pObjMessage);
        FuncResponse<MAP_TaxPayer_Message> REP_InsertMessage(MAP_TaxPayer_Message pObjMessage);
    }

    public class TaxPayerMessageRespository : ITaxPayerMessageRespository
    {
        EIRSEntities _db;

        public IList<usp_GetTaxPayerMessageList_Result> REP_GetMessageList(MAP_TaxPayer_Message pObjMessage)
        {
            using (_db = new EIRSEntities())
            {
                return _db.usp_GetTaxPayerMessageList(pObjMessage.TaxPayerID, pObjMessage.TaxPayerTypeID).ToList();
            }
        }

        public FuncResponse<MAP_TaxPayer_Message> REP_InsertMessage(MAP_TaxPayer_Message pObjMessage)
        {
            using (_db = new EIRSEntities())
            {
                FuncResponse<MAP_TaxPayer_Message> mObjFuncResponse = new FuncResponse<MAP_TaxPayer_Message>();


                MAP_TaxPayer_Message mObjInsertUpdateMessage;
                mObjInsertUpdateMessage = new MAP_TaxPayer_Message();
                mObjInsertUpdateMessage.MessageDate = pObjMessage.MessageDate;
                mObjInsertUpdateMessage.Message = pObjMessage.Message;

                if (pObjMessage.Sender_TaxPayerID != null)
                {
                    mObjInsertUpdateMessage.Sender_TaxPayerID = pObjMessage.Sender_TaxPayerID;
                }

                if (pObjMessage.Sender_TaxPayerTypeID != null)
                {
                    mObjInsertUpdateMessage.Sender_TaxPayerTypeID = pObjMessage.Sender_TaxPayerTypeID;
                }

                if (pObjMessage.Sender_StaffID != null)
                {
                    mObjInsertUpdateMessage.Sender_StaffID = pObjMessage.Sender_StaffID;
                }


                if (pObjMessage.Receiver_TaxPayerID != null)
                {
                    mObjInsertUpdateMessage.Receiver_TaxPayerID = pObjMessage.Receiver_TaxPayerID;
                }

                if (pObjMessage.Receiver_TaxPayerTypeID != null)
                {
                    mObjInsertUpdateMessage.Receiver_TaxPayerTypeID = pObjMessage.Receiver_TaxPayerTypeID;
                }

                if (pObjMessage.Receiver_StaffID != null)
                {
                    mObjInsertUpdateMessage.Receiver_StaffID = pObjMessage.Receiver_StaffID;
                }


                mObjInsertUpdateMessage.CreatedBy = pObjMessage.CreatedBy;
                mObjInsertUpdateMessage.CreatedDate = pObjMessage.CreatedDate;

                _db.MAP_TaxPayer_Message.Add(mObjInsertUpdateMessage);

                try
                {
                    _db.SaveChanges();
                    mObjFuncResponse.AdditionalData = mObjInsertUpdateMessage;
                    mObjFuncResponse.Success = true;
                    mObjFuncResponse.Message = "Message Added Successfully";

                }
                catch (Exception ex)
                {
                    mObjFuncResponse.Success = false;
                    mObjFuncResponse.Exception = ex;
                    mObjFuncResponse.Message = "Message Addition Failed";

                }

                return mObjFuncResponse;
            }
        }

        public MAP_TaxPayer_Message_Document REP_AddMessageDocument(MAP_TaxPayer_Message pObjMessage, MAP_TaxPayer_Message_Document pObjMessageDocument)
        {
            using (_db = new EIRSEntities())
            {
                MAP_TaxPayer_Message mObjMessage = _db.MAP_TaxPayer_Message.Find(pObjMessage.TPMID);
                mObjMessage.MAP_TaxPayer_Message_Document.Add(pObjMessageDocument);
                _db.SaveChanges();

                pObjMessageDocument.DocumentName = pObjMessageDocument.DocumentName;
                string strDocumentName = "MD_" + pObjMessage.TPMID.ToString() + "_" + pObjMessageDocument.TPMDID + "_" + DateTime.Now.ToString("dd_MM_yyyy_ss") + Path.GetExtension(pObjMessageDocument.DocumentName);
                pObjMessageDocument.DocumentPath = "Messages/" + pObjMessage.TPMID.ToString() + "/" + strDocumentName;
                _db.SaveChanges();

                return pObjMessageDocument;
            }
        }

        public void REP_DeleteMessageDocument(MAP_TaxPayer_Message_Document pObjMessageDocument)
        {
            using (_db = new EIRSEntities())
            {
                MAP_TaxPayer_Message_Document mRemoveDocument = _db.MAP_TaxPayer_Message_Document.Find(pObjMessageDocument.TPMDID);
                if (mRemoveDocument != null)
                {
                    _db.MAP_TaxPayer_Message_Document.Remove(mRemoveDocument);
                    _db.SaveChanges();
                }
            }
        }

        public IList<MAP_TaxPayer_Message_Document> REP_GetMessageDocumentList(long plngTPMID)
        {
            using (_db = new EIRSEntities())
            {
                var vMessageDocument = (from doc in _db.MAP_TaxPayer_Message_Document
                                        where doc.TPMID == plngTPMID
                                        select doc).ToList();

                return vMessageDocument;
            }
        }

    }
}
