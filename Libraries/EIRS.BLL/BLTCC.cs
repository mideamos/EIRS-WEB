using EIRS.BOL;
using EIRS.Common;
using EIRS.Repository;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EIRS.BLL
{
    public class BLTCC
    {
        ITCCRepository _TCCRepository;

        public BLTCC()
        {
            _TCCRepository = new TCCRepository();
        }

        public FuncResponse BL_InsertTCCDetail(TCCDetail pObjTCCDetails)
        {
            return _TCCRepository.REP_InsertTCCDetail(pObjTCCDetails);
        }

        public FuncResponse BL_InsertUpdateTCCDetail(TCCDetail pObjTCCDetail)
        {
            return _TCCRepository.REP_InsertUpdateTCCDetail(pObjTCCDetail);
        }

        public FuncResponse BL_RemoveTCCDetail(TCCDetail pObjTCCDetail)
        {
            return _TCCRepository.REP_RemoveTCCDetail(pObjTCCDetail);
        }

        public IDictionary<string, object> BL_SearchTCCDetail(TCCDetail pObjTCCDetails)
        {
            return _TCCRepository.REP_SearchTCCDetail(pObjTCCDetails);
        }

        public FuncResponse BL_InsertTaxClearanceCertificate(TaxClearanceCertificate pObjTaxClearanceCertificate)
        {
            return _TCCRepository.REP_InsertTaxClearanceCertificate(pObjTaxClearanceCertificate);
        }

        public FuncResponse BL_InsertUpdateTaxClearanceCertificate(TaxClearanceCertificate pObjTaxClearanceCertificate)
        {
            return _TCCRepository.REP_InsertUpdateTaxClearanceCertificate(pObjTaxClearanceCertificate);
        }

        public IDictionary<string, object> BL_SearchTaxClearanceCertificate(TaxClearanceCertificate pObjTaxClearanceCertificate)
        {
            return _TCCRepository.REP_SearchTaxClearanceCertificate(pObjTaxClearanceCertificate);
        }

        public IList<usp_GetTaxClearanceCertificateDetails_Result> BL_GetTaxClearanceCertificateList(TaxClearanceCertificate pObjTaxClearanceCertificate)
        {
            return _TCCRepository.REP_GetTaxClearanceCertificateList(pObjTaxClearanceCertificate);
        }

        public usp_GetTaxClearanceCertificateDetails_Result BL_GetTaxClearanceCertificateDetail(TaxClearanceCertificate pObjTaxClearanceCertificate)
        {
            return _TCCRepository.REP_GetTaxClearanceCertificateDetail(pObjTaxClearanceCertificate);
        }


        public FuncResponse<TCC_Request> BL_GetIncompleteRequest(TCC_Request pObjRequest)
        {
            return _TCCRepository.REP_GetIncompleteRequest(pObjRequest);
        }

        public FuncResponse<TCC_Request> BL_InsertTCCRequest(TCC_Request pObjRequest)
        {
            return _TCCRepository.REP_InsertTCCRequest(pObjRequest);
        }

        public FuncResponse BL_UpdateRequestStatus(TCC_Request pObjRequest)
        {
            return _TCCRepository.REP_UpdateRequestStatus(pObjRequest);
        }

        public IList<usp_GetRequestIncomeStreamList_Result> BL_GetIncomeStreamList(long plngRequestID)
        {
            return _TCCRepository.REP_GetIncomeStreamList(plngRequestID);
        }

        public FuncResponse BL_InsertUpdateIncomeStream(MAP_TCCRequest_IncomeStream pObjIncomeStream)
        {
            return _TCCRepository.REP_InsertUpdateIncomeStream(pObjIncomeStream);
        }

        public FuncResponse BL_RemoveIncomeStream(MAP_TCCRequest_IncomeStream pObjIncomeStream)
        {
            return _TCCRepository.REP_RemoveIncomeStream(pObjIncomeStream);
        }

        public FuncResponse BL_UpdateServiceBillInRequest(TCC_Request pObjRequest)
        {
            return _TCCRepository.REP_UpdateServiceBillInRequest(pObjRequest);
        }

        public FuncResponse BL_InsertUpdateValidateInformation(MAP_TCCRequest_ValidateTaxPayerInformation pObjValidateInformation)
        {
            return _TCCRepository.REP_InsertUpdateValidateInformation(pObjValidateInformation);
        }

        public MAP_TCCRequest_ValidateTaxPayerInformation BL_GetValidateInformationDetails(long plngRequestID)
        {
            return _TCCRepository.REP_GetValidateInformationDetails(plngRequestID);
        }

        public FuncResponse BL_InsertUpdateValidateIncome(MAP_TCCRequest_ValidateTaxPayerIncome pObjValidateIncome)
        {
            return _TCCRepository.REP_InsertUpdateValidateIncome(pObjValidateIncome);
        }

        public MAP_TCCRequest_ValidateTaxPayerIncome BL_GetValidateIncomeDetails(long plngRequestID)
        {
            return _TCCRepository.REP_GetValidateIncomeDetails(plngRequestID);
        }

        public FuncResponse BL_InsertUpdateGenerateTCCDetail(MAP_TCCRequest_GenerateTCCDetail pObjGenerateTCCDetail)
        {
            return _TCCRepository.REP_InsertUpdateGenerateTCCDetail(pObjGenerateTCCDetail);
        }

        public MAP_TCCRequest_GenerateTCCDetail BL_GetGenerateTCCDetailDetails(long plngRequestID)
        {
            return _TCCRepository.REP_GetGenerateTCCDetailDetails(plngRequestID);
        }

        public FuncResponse BL_InsertUpdatePrepareTCCDraft(MAP_TCCRequest_PrepareTCCDraft pObjPrepareTCCDraft)
        {
            return _TCCRepository.REP_InsertUpdatePrepareTCCDraft(pObjPrepareTCCDraft);
        }

        public MAP_TCCRequest_PrepareTCCDraft BL_GetPrepareTCCDraftDetails(long plngRequestID)
        {
            return _TCCRepository.REP_GetPrepareTCCDraftDetails(plngRequestID);
        }

        public usp_GetTCCDetailForGenerate_Result BL_GetTCCDetailForGenerateProcess(TaxClearanceCertificate pObjTaxClearanceCertificate)
        {
            return _TCCRepository.REP_GetTCCDetailForGenerateProcess(pObjTaxClearanceCertificate);
        }

        public MAP_TCCRequest_Generate BL_GetTCCRequestGenerateDetails(long plngTCCRequestID)
        {
            return _TCCRepository.REP_GetTCCRequestGenerateDetails(plngTCCRequestID);
        }

        public FuncResponse<MAP_TCCRequest_Generate> BL_InsertUpdateTCCRequestGenerate(MAP_TCCRequest_Generate pObjTCCRequestGenerate)
        {
            return _TCCRepository.REP_InsertUpdateTCCRequestGenerate(pObjTCCRequestGenerate);
        }

        public FuncResponse BL_InsertUpdateGenerateField(MAP_TCCRequest_Generate_Field pObjGenerateField)
        {
            return _TCCRepository.REP_InsertUpdateGenerateField(pObjGenerateField);
        }

        public MAP_TCCRequest_Validate BL_GetTCCRequestValidateDetails(long plngTCCRequestID)
        {
            return _TCCRepository.REP_GetTCCRequestValidateDetails(plngTCCRequestID);
        }

        public FuncResponse<MAP_TCCRequest_Validate> BL_InsertUpdateTCCRequestValidate(MAP_TCCRequest_Validate pObjTCCRequestValidate)
        {
            return _TCCRepository.REP_InsertUpdateTCCRequestValidate(pObjTCCRequestValidate);
        }

        public MAP_TCCRequest_Seal BL_GetTCCRequestSealDetails(long plngTCCRequestID)
        {
            return _TCCRepository.REP_GetTCCRequestSealDetails(plngTCCRequestID);
        }

        public FuncResponse<MAP_TCCRequest_Seal> BL_InsertUpdateTCCRequestSeal(MAP_TCCRequest_Seal pObjTCCRequestSeal)
        {
            return _TCCRepository.REP_InsertUpdateTCCRequestSeal(pObjTCCRequestSeal);
        }

        public MAP_TCCRequest_Issue BL_GetTCCRequestIssueDetails(long plngTCCRequestID)
        {
            return _TCCRepository.REP_GetTCCRequestIssueDetails(plngTCCRequestID);
        }

        public FuncResponse<MAP_TCCRequest_Issue> BL_InsertUpdateTCCRequestIssue(MAP_TCCRequest_Issue pObjTCCRequestIssue)
        {
            return _TCCRepository.REP_InsertUpdateTCCRequestIssue(pObjTCCRequestIssue);
        }


        public IDictionary<string, object> BL_SearchTCCRequest(TCC_Request pObjRequest)
        {
            return _TCCRepository.REP_SearchTCCRequest(pObjRequest);
        }

        public usp_GetTCCRequestDetails_Result BL_GetRequestDetails(long plngRequestID)
        {
            return _TCCRepository.REP_GetRequestDetails(plngRequestID);
        }

        public IList<usp_GetAdminRequestStageList_Result> BL_GetAdminRequestStageList(long plngRequestID)
        {
            return _TCCRepository.REP_GetAdminRequestStageList(plngRequestID);
        }

        public FuncResponse BL_UpdateRequestStage(MAP_TCCRequest_Stages pObjRequestStage)
        {
            return _TCCRepository.REP_UpdateRequestStage(pObjRequestStage);
        }

        public IList<usp_GetTCCDetail_Result> BL_GetTCCDetail(int TaxPayerID, int TaxPayerTypeID, int TaxYear)
        {
            return _TCCRepository.REP_GetTCCDetail(TaxPayerID, TaxPayerTypeID, TaxYear);
        }
        public IList<DirectAssessmentIncomeStreams_Result> BL_GetTCCDetailNew(int TaxPayerTypeID, int TaxPayerID)
        {
            return _TCCRepository.REP_GetTCCDetailNew(TaxPayerTypeID, TaxPayerID);
        }

        public MAP_TCCRequest_SignVisible BL_GetTCCRequestSignVisibleDetails(long plngTCCRequestID, int UserID, int StageID = 0)
        {
            return _TCCRepository.REP_GetTCCRequestSignVisibleDetails(plngTCCRequestID, UserID, StageID);
        }

        public FuncResponse<MAP_TCCRequest_SignVisible> BL_InsertUpdateTCCRequestSignVisible(MAP_TCCRequest_SignVisible pObjTCCRequestSignVisible)
        {
            return _TCCRepository.REP_InsertUpdateTCCRequestSignVisible(pObjTCCRequestSignVisible);
        }

        public MAP_TCCRequest_SignDigital BL_GetTCCRequestSignDigitalDetails(long plngTCCRequestID)
        {
            return _TCCRepository.REP_GetTCCRequestSignDigitalDetails(plngTCCRequestID);
        }

        public FuncResponse<MAP_TCCRequest_SignDigital> BL_InsertUpdateTCCRequestSignDigital(MAP_TCCRequest_SignDigital pObjTCCRequestSignDigital)
        {
            return _TCCRepository.REP_InsertUpdateTCCRequestSignDigital(pObjTCCRequestSignDigital);
        }

        public IList<MAP_TCCRequest_SignVisible> BL_GetTCCRequestSignVisibleList(long plngTCCRequestID)
        {
            return _TCCRepository.REP_GetTCCRequestSignVisibleList(plngTCCRequestID);
        }

        public TCC_Request BL_GetRequestBasedOnServiceBill(long pIntServiceBillID)
        {
            return _TCCRepository.REP_GetRequestBasedOnServiceBill(pIntServiceBillID);
        }

        public IList<usp_GetTCCRequestList_Result> BL_GetTCCRequestList(TCC_Request pObjRequest)
        {
            return _TCCRepository.REP_GetTCCRequestList(pObjRequest);
        }


        public IList<usp_GetTaxPayerAssetForTCC_Result> BL_GetTaxPayerAssetList(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            return _TCCRepository.REP_GetTaxPayerAssetList(pIntTaxPayerID, pIntTaxPayerTypeID);
        }

        public IList<usp_GetTaxPayerProfileInformationForTCC_Result> BL_GetTaxPayerProfileList(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            return _TCCRepository.REP_GetTaxPayerProfileList(pIntTaxPayerID, pIntTaxPayerTypeID);
        }

        public IList<usp_GetAssessmentRuleInformationForTCC_Result> BL_GetTaxPayerAssessmentRuleList(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            return _TCCRepository.REP_GetTaxPayerAssessmentRuleList(pIntTaxPayerID, pIntTaxPayerTypeID);
        }

        public IList<usp_GetTaxPayerBillForTCC_Result> BL_GetTaxPayerBillList(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            return _TCCRepository.REP_GetTaxPayerBillList(pIntTaxPayerID, pIntTaxPayerTypeID);
        }

        public IList<usp_GetTaxPayerPaymentForTCC_Result> BL_GetTaxPayerPaymentList(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            return _TCCRepository.REP_GetTaxPayerPaymentList(pIntTaxPayerID, pIntTaxPayerTypeID);
        } 
        public IList<usp_GetTaxPayerPaymentForTCCNEW_Result> BL_GetTaxPayerPaymentListNEW(int pIntTaxPayerID, int pIntTaxPayerTypeID)
        {
            return _TCCRepository.REP_GetTaxPayerPaymentListNEW(pIntTaxPayerID, pIntTaxPayerTypeID);
        }

        public IList<usp_GetTaxPayerLiabilityForTCC_Result> BL_GetTaxPayerLiabilityForTCC(int pIntTaxPayerID, int pIntTaxPayerTypeID, int pIntTaxYear)
        {
            return _TCCRepository.REP_GetTaxPayerLiabilityForTCC(pIntTaxPayerID, pIntTaxPayerTypeID, pIntTaxYear);
        }

        public FuncResponse<MAP_TCCRequest_Notes> BL_InsertRequestNotes(MAP_TCCRequest_Notes pObjRequestNotes)
        {
            FuncResponse<MAP_TCCRequest_Notes> mObjFuncResponse = _TCCRepository.REP_InsertRequestNotes(pObjRequestNotes);

            if (mObjFuncResponse.Success)
            {
                pObjRequestNotes.RNID = mObjFuncResponse.AdditionalData.RNID;
                BL_ProcessNotesDocument(pObjRequestNotes);
            }

            return mObjFuncResponse;
        }

        public void BL_ProcessNotesDocument(MAP_TCCRequest_Notes pObjRequestNotes)
        {
            IEnumerable<MAP_TCCRequest_Notes_Document> lstDocument = (from doc in pObjRequestNotes.MAP_TCCRequest_Notes_Document select doc).ToList();

            foreach (MAP_TCCRequest_Notes_Document doc in lstDocument)
            {

                string strOldPath = doc.DocumentPath;

                doc.RNID = pObjRequestNotes.RNID;
                doc.CreatedDate = pObjRequestNotes.CreatedDate;
                doc.CreatedBy = pObjRequestNotes.CreatedBy;

                var temp = _TCCRepository.REP_AddNotesDocument(pObjRequestNotes, doc);

                string strNewPath = GlobalDefaultValues.DocumentLocation + temp.DocumentPath;
                string strDirectory = GlobalDefaultValues.DocumentLocation + "TCC/" + pObjRequestNotes.RequestID.ToString() + "/Notes/";

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

        public IList<usp_GetRequestNotesList_Result> BL_GetRequestNotesList(MAP_TCCRequest_Notes pObjRequestNotes)
        {
            return _TCCRepository.REP_GetRequestNotesList(pObjRequestNotes);
        }

        public IList<MAP_TCCRequest_Notes_Document> BL_GetNotesDocumentList(long plngRNID)
        {
            return _TCCRepository.REP_GetNotesDocumentList(plngRNID);
        }

        public FuncResponse BL_RevokeTCC(MAP_TCCRequest_Revoke pObjRevoke)
        {
            return _TCCRepository.REP_RevokeTCC(pObjRevoke);
        }

        public IList<GetEmployerLiability_Result> BL_GetEmployerLiability(int pIntIndividualID)
        {
            return _TCCRepository.REP_GetEmployerLiability(pIntIndividualID);
        }

        public bool BL_VerifyTCCByReferenceNumber(string pObjReferenceNumber)
        {
            return _TCCRepository.REP_VerifyTCCByReferenceNumber(pObjReferenceNumber);
        }
    }
}
