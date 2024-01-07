using System.Collections.Generic;
using EIRS.BOL;
using EIRS.Common;

namespace EIRS.Repository
{
    public interface ITCCRepository
    {
        MAP_TCCRequest_GenerateTCCDetail REP_GetGenerateTCCDetailDetails(long plngRequestID);

        FuncResponse REP_InsertUpdateTCCDetail(TCCDetail pObjTCCDetail);
        FuncResponse REP_RemoveTCCDetail(TCCDetail pObjTCCDetail);
        IList<usp_GetRequestIncomeStreamList_Result> REP_GetIncomeStreamList(long plngRequestID);
        FuncResponse<TCC_Request> REP_GetIncompleteRequest(TCC_Request pObjRequest);
        IList<DirectAssessmentIncomeStreams_Result> REP_GetTCCDetailNew(int TaxPayerTypeID, int TaxPayerID);
        FuncResponse REP_InsertTaxClearanceCertificate(TaxClearanceCertificate pObjTaxClearanceCertificate);
        FuncResponse REP_InsertUpdateTaxClearanceCertificate(TaxClearanceCertificate pObjTaxClearanceCertificate);

        IDictionary<string, object> REP_SearchTaxClearanceCertificate(TaxClearanceCertificate pObjTaxClearanceCertificate);

        IList<usp_GetTaxClearanceCertificateDetails_Result> REP_GetTaxClearanceCertificateList(TaxClearanceCertificate pObjTaxClearanceCertificate);
        usp_GetTaxClearanceCertificateDetails_Result REP_GetTaxClearanceCertificateDetail(TaxClearanceCertificate pObjTaxClearanceCertificate);

        MAP_TCCRequest_PrepareTCCDraft REP_GetPrepareTCCDraftDetails(long plngRequestID);
        MAP_TCCRequest_Generate REP_GetTCCRequestGenerateDetails(long plngTCCRequestID);

        usp_GetTCCDetailForGenerate_Result REP_GetTCCDetailForGenerateProcess(TaxClearanceCertificate pObjTaxClearanceCertificate);
        MAP_TCCRequest_Seal REP_GetTCCRequestSealDetails(long plngTCCRequestID);

        MAP_TCCRequest_Issue REP_GetTCCRequestIssueDetails(long plngTCCRequestID);

        FuncResponse<MAP_TCCRequest_Issue> REP_InsertUpdateTCCRequestIssue(MAP_TCCRequest_Issue pObjTCCRequestIssue);

        MAP_TCCRequest_Validate REP_GetTCCRequestValidateDetails(long plngTCCRequestID);
        MAP_TCCRequest_ValidateTaxPayerIncome REP_GetValidateIncomeDetails(long plngRequestID);
        MAP_TCCRequest_ValidateTaxPayerInformation REP_GetValidateInformationDetails(long plngRequestID);
        FuncResponse REP_InsertTCCDetail(TCCDetail pObjTCCDetails);
        FuncResponse<TCC_Request> REP_InsertTCCRequest(TCC_Request pObjRequest);
        FuncResponse REP_InsertUpdateGenerateField(MAP_TCCRequest_Generate_Field pObjGenerateField);
        FuncResponse REP_InsertUpdateGenerateTCCDetail(MAP_TCCRequest_GenerateTCCDetail pObjGenerateTCCDetail);
        FuncResponse REP_InsertUpdateIncomeStream(MAP_TCCRequest_IncomeStream pObjIncomeStream);
        FuncResponse REP_InsertUpdatePrepareTCCDraft(MAP_TCCRequest_PrepareTCCDraft pObjPrepareTCCDraft);
        FuncResponse<MAP_TCCRequest_Generate> REP_InsertUpdateTCCRequestGenerate(MAP_TCCRequest_Generate pObjTCCRequestGenerate);
        FuncResponse<MAP_TCCRequest_Seal> REP_InsertUpdateTCCRequestSeal(MAP_TCCRequest_Seal pObjTCCRequestSeal);
        FuncResponse<MAP_TCCRequest_Validate> REP_InsertUpdateTCCRequestValidate(MAP_TCCRequest_Validate pObjTCCRequestValidate);
        FuncResponse REP_InsertUpdateValidateIncome(MAP_TCCRequest_ValidateTaxPayerIncome pObjValidateIncome);
        FuncResponse REP_InsertUpdateValidateInformation(MAP_TCCRequest_ValidateTaxPayerInformation pObjValidateInformation);
        FuncResponse REP_RemoveIncomeStream(MAP_TCCRequest_IncomeStream pObjIncomeStream);
        IDictionary<string, object> REP_SearchTCCDetail(TCCDetail pObjTCCDetails);
        FuncResponse REP_UpdateRequestStatus(TCC_Request pObjRequest);
        FuncResponse REP_UpdateServiceBillInRequest(TCC_Request pObjRequest);
        TCC_Request REP_GetRequestBasedOnServiceBill(long pIntServiceBillID);

        IDictionary<string, object> REP_SearchTCCRequest(TCC_Request pObjRequest);

        usp_GetTCCRequestDetails_Result REP_GetRequestDetails(long plngRequestID);

        IList<usp_GetAdminRequestStageList_Result> REP_GetAdminRequestStageList(long plngRequestID);

        FuncResponse REP_UpdateRequestStage(MAP_TCCRequest_Stages pObjRequestStage);

        IList<usp_GetTCCDetail_Result> REP_GetTCCDetail(int TaxPayerID, int TaxPayerTypeID, int TaxYear);

        MAP_TCCRequest_SignVisible REP_GetTCCRequestSignVisibleDetails(long plngTCCRequestID, int UserID, int StageID = 0);

        IList<MAP_TCCRequest_SignVisible> REP_GetTCCRequestSignVisibleList(long plngTCCRequestID);

        FuncResponse<MAP_TCCRequest_SignVisible> REP_InsertUpdateTCCRequestSignVisible(MAP_TCCRequest_SignVisible pObjTCCRequestSignVisible);

        MAP_TCCRequest_SignDigital REP_GetTCCRequestSignDigitalDetails(long plngTCCRequestID);

        FuncResponse<MAP_TCCRequest_SignDigital> REP_InsertUpdateTCCRequestSignDigital(MAP_TCCRequest_SignDigital pObjTCCRequestSignDigital);
        IList<usp_GetTCCRequestList_Result> REP_GetTCCRequestList(TCC_Request pObjRequest);

        IList<usp_GetTaxPayerAssetForTCC_Result> REP_GetTaxPayerAssetList(int pIntTaxPayerID, int pIntTaxPayerTypeID);
        IList<usp_GetTaxPayerProfileInformationForTCC_Result> REP_GetTaxPayerProfileList(int pIntTaxPayerID, int pIntTaxPayerTypeID);
        IList<usp_GetAssessmentRuleInformationForTCC_Result> REP_GetTaxPayerAssessmentRuleList(int pIntTaxPayerID, int pIntTaxPayerTypeID);
        IList<usp_GetTaxPayerBillForTCC_Result> REP_GetTaxPayerBillList(int pIntTaxPayerID, int pIntTaxPayerTypeID);
        IList<usp_GetTaxPayerPaymentForTCC_Result> REP_GetTaxPayerPaymentList(int pIntTaxPayerID, int pIntTaxPayerTypeID);
        IList<usp_GetTaxPayerPaymentForTCCNEW_Result> REP_GetTaxPayerPaymentListNEW(int pIntTaxPayerID, int pIntTaxPayerTypeID);

        IList<usp_GetTaxPayerLiabilityForTCC_Result> REP_GetTaxPayerLiabilityForTCC(int pIntTaxPayerID, int pIntTaxPayerTypeID, int pIntTaxYear);

        IList<usp_GetRequestNotesList_Result> REP_GetRequestNotesList(MAP_TCCRequest_Notes pObjRequestNotes);
        FuncResponse<MAP_TCCRequest_Notes> REP_InsertRequestNotes(MAP_TCCRequest_Notes pObjRequestNotes);
        MAP_TCCRequest_Notes_Document REP_AddNotesDocument(MAP_TCCRequest_Notes pObjOrderNotes, MAP_TCCRequest_Notes_Document pObjNotesDocument);
        IList<MAP_TCCRequest_Notes_Document> REP_GetNotesDocumentList(long plngRNID);

        FuncResponse REP_RevokeTCC(MAP_TCCRequest_Revoke pObjRevoke);
        IList<GetEmployerLiability_Result> REP_GetEmployerLiability(int pIntIndividualID);

        bool REP_VerifyTCCByReferenceNumber(string pObjReferenceNumber);
    }
}