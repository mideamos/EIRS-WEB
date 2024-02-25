var vAssetTable, vAssessmentRuleTable;

$(document).ready(function () {
    vAssetTable = jsfn_ConvertToDatableWithCustomSort($('#tblAssetList'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Assoicated Assets Found');
    vAssessmentRuleTable = jsfn_ConvertToDatableWithCustomSort($('#tblAssessmentRules'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Assoicated Rules Found');
    jsfn_ConvertToDatableWithCustomSort($('#tblProfileList'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Assoicated Profiles Found');
    jsfn_ConvertToDatableWithCustomSort($('#tblPendingList'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Assoicated Pending Approvals Found');
    jsfn_ConvertToDatableWithCustomSort($('#tblDendingList'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Assoicated Declined Approvals Found');
    vMDAServiceTable = jsfn_ConvertToDatableWithCustomSort($('#tblMDAServiceList'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Assoicated MDA Services Found');
    vBillTable = jsfn_ConvertToDatableWithCustomSort($('#tblBillList'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Assoicated Bills Found');
    vPaymentTable = jsfn_ConvertToDatableWithCustomSort($('#tblPaymentList'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Assoicated Payments Found');
    jsfn_ConvertToDatableWithCustomSort($('#tblTCCRequest'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Assoicated Request Found');
    jsfn_ConvertToDatableWithCustomSort($('#tblDocumentList'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Assoicated Document Found');

    $("#btnAddTCCRequest").on("click", function () {
        $("#dvAddTCCRequestModal").modal('show');
    });

    $("#btnSaveRequest").click(function () {
        var vTaxYear = $("#cboTaxYear").val();

        if (vTaxYear !== '') {
            var vData = {
                TaxPayerID: $("#hdnIndividualID").val(),
                TaxYear: vTaxYear
            };

            jsfn_ShowLoading();
            jsfn_ajaxPost('/CaptureIndividual/AddTCCRequest', vData, jsfn_AddTCCResponse);
        }
        else {
            $("#dvTCCRMessage").html('<div class="alert alert-danger">Select Tax Year</div>');
        }
    });
});

function jsfn_AssessmentRuleDetails(aruleID) {
    var vData = {
        AssessmentRuleID: aruleID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetAssessmentRuleDetails', vData, jsfn_ShowAssessmentRuleDetailResponse);
}
function jsfn_ShowAssessmentRuleDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvAssessmentRuleModal').modal('show');
        $('#dvAssessmentRuleRefNo').html(data.AssessmentRuleDetails.AssessmentRuleCode);
        $('#dvAssessmentRuleName').html(data.AssessmentRuleDetails.AssessmentRuleName);
        $('#dvRuleRun').html(data.AssessmentRuleDetails.RuleRunName);
        $('#dvFrequency').html(data.AssessmentRuleDetails.PaymentFrequencyName);
        $('#dvTaxYear').html(data.AssessmentRuleDetails.TaxYear);
        $('#dvSettlementMethod').html(data.AssessmentRuleDetails.SettlementMethodNames);
        $('#dvPaymentOption').html(data.AssessmentRuleDetails.PaymentOptionName);
        $('#dvStatus').html(data.AssessmentRuleDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_FilterAssessmentRuleTable(vTaxYear) {
    vAssessmentRuleTable.fnFilter(vTaxYear, 1, false, false, false, false);
}

function jsfn_AssessmentRuleItemList(aruleID) {
    var vData = {
        AssessmentRuleID: aruleID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetAssessmentRuleItemList', vData, jsfn_AssessmentRuleItemResponse);
}

function jsfn_AssessmentRuleItemResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#dvAssessmentRuleItemModal').modal('show');
        $('#dvAssessmentRuleItemData').html(data.AssessmentRuleItemList);
        jsfn_ConvertToDatableWithCustomSort($('#tblAssessmentRuleItem'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }], 'No Assessment Items Found');
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ProfileDetails(prfID) {
    var vData = {
        ProfileID: prfID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetProfileDetails', vData, jsfn_ShowProfileDetailResponse);
}

function jsfn_ShowProfileDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvProfileModal').modal('show');
        $('#dvProfileRefNo').html(data.ProfileDetails.ProfileReferenceNo);
        $('#dvAssetType').html(data.ProfileDetails.AssetTypeName);
        $('#dvTaxPayerType').html(data.ProfileDetails.TaxPayerTypeNames);
        $('#dvTaxPayerRole').html(data.ProfileDetails.TaxPayerRoleNames);
        $('#dvProfileDescription').html(data.ProfileDetails.ProfileDescription);
        $('#dvAssetStatus').html(data.ProfileDetails.AssetTypeStatusName);
        $('#dvProfileStatus').html(data.ProfileDetails.ActiveText);
        $('#dvProfileSector').html(data.ProfileDetails.ProfileSectorNames);
        $('#dvProfileSubSector').html(data.ProfileDetails.ProfileSubSectorNames);
        $('#dvProfileGroup').html(data.ProfileDetails.ProfileGroupNames);
        $('#dvProfileSubGroup').html(data.ProfileDetails.ProfileSubGroupNames);
        $('#dvSectorElement').html(data.ProfileDetails.ProfileSectorElementNames);
        $('#dvSectorSubElement').html(data.ProfileDetails.ProfileSectorSubElementNames);
        $('#dvProfileAttribute').html(data.ProfileDetails.ProfileAttributeNames);
        $('#dvProfileSubAttribute').html(data.ProfileDetails.ProfileSubAttributeNames);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_FilterMDAServiceTable(vTaxYear) {
    vMDAServiceTable.fnFilter(vTaxYear, 1, false, false, false, false);
}

function jsfn_MDAServiceDetails(MDAServiceID) {

    var vData = {
        MDAServiceID: MDAServiceID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetMDAServiceDetails', vData, jsfn_MDAServiceDetailResponse)
}

function jsfn_MDAServiceDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvMDAServiceModal').modal('show');
        $('#dvMDAServiceCode').html(data.MDAServiceDetails.MDAServiceCode);
        $('#dvMDAServiceName').html(data.MDAServiceDetails.MDAServiceName);
        $('#dvMDARuleRun').html(data.MDAServiceDetails.RuleRunName);
        $('#dvMDAFrequency').html(data.MDAServiceDetails.PaymentFrequencyName);
        $('#dvMDATaxYear').html(data.MDAServiceDetails.TaxYear);
        $('#dvMDASettlementMethod').html(data.MDAServiceDetails.SettlementMethodNames);
        $('#dvMDAPaymentOption').html(data.MDAServiceDetails.PaymentOptionName);
        $('#dvMDAStatus').html(data.MDAServiceDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_MDAServiceItemList(MDAServiceID) {
    var vData = {
        MDAServiceID: MDAServiceID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetMDAServiceItemList', vData, jsfn_MDAServiceItemResponse);
}

function jsfn_MDAServiceItemResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#dvMDAServiceItemModal').modal('show');
        $('#dvMDAServiceItemData').html(data.MDAServiceItemList);
        jsfn_ConvertToDatableWithCustomSort($('#tblMDAServiceItem'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }], 'No MDA Service Items Found');
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_BillDetails(billid, billtype) {

    if (billtype === 1) {
        var vData = {
            AssessmentID: billid,
        };

        jsfn_ShowLoading();
        jsfn_ajaxPost('/Base/GetAssessmentDetails', vData, jsfn_AssessmentDetailResponse)
    }
    else if (billtype === 2) {
        var vData = {
            ServiceBillID: billid,
        };

        jsfn_ShowLoading();
        jsfn_ajaxPost('/Base/GetServiceBillDetails', vData, jsfn_ServiceBillDetailResponse)
    }


}

function jsfn_AssessmentDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvAssessmentModal').modal('show');
        $('#dvAssessmentRefNo').html(data.AssessmentDetails.AssessmentRefNo);
        $('#dvAssessmentDate').html(jsfn_FormatJsonDate(data.AssessmentDetails.AssessmentDate));
        $('#dvATaxPayerTypeName').html(data.AssessmentDetails.TaxPayerTypeName);
        $('#dvATaxPayerName').html(data.AssessmentDetails.TaxPayerName);
        $('#dvAssessmentAmount').html(data.AssessmentDetails.AssessmentAmount.formatMoney(2, '₦'));
        $('#dvASettlementDueDate').html(jsfn_FormatJsonDate(data.AssessmentDetails.SettlementDueDate));
        $('#dvASettlementStatus').html(data.AssessmentDetails.SettlementStatusName);
        $('#dvASettlementDate').html(data.AssessmentDetails.SettlementDate != null ? jsfn_FormatJsonDate(data.AssessmentDetails.SettlementDate) : "-");
        $('#dvAssessmentNotes').html(data.AssessmentDetails.AssessmentNotes);
        $('#dvAStatus').html(data.AssessmentDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ServiceBillDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvServiceBillModal').modal('show');
        $('#dvServiceBillRefNo').html(data.ServiceBillDetails.ServiceBillRefNo);
        $('#dvServiceBillDate').html(jsfn_FormatJsonDate(data.ServiceBillDetails.ServiceBillDate));
        $('#dvSTaxPayerTypeName').html(data.ServiceBillDetails.TaxPayerTypeName);
        $('#dvSTaxPayerName').html(data.ServiceBillDetails.TaxPayerName);
        $('#dvServiceBillAmount').html(data.ServiceBillDetails.ServiceBillAmount.formatMoney(2, '₦'));
        $('#dvSSettlementDueDate').html(jsfn_FormatJsonDate(data.ServiceBillDetails.SettlementDueDate));
        $('#dvSSettlementStatus').html(data.ServiceBillDetails.SettlementStatusName);
        $('#dvSSettlementDate').html(data.ServiceBillDetails.SettlementDate != null ? jsfn_FormatJsonDate(data.ServiceBillDetails.SettlementDate) : "-");
        $('#dvSStatus').html(data.ServiceBillDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_BillPaymentDetails(billid, billtype) {
    var vData = {
        BillID: billid,
        BillTypeID: billtype
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetBillPaymentList', vData, jsfn_BillPaymentDetailResponse)
}

function jsfn_BillPaymentDetailResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#dvBillPaymentModal').modal('show');
        $('#dvBillPaymentData').html(data.BillPaymentList);
        jsfn_ConvertToDatableWithCustomSort($('#tblBillPayment'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }], 'No Payment History Found', []);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_PaymentDetails(paymentid, paymenttype) {

    if (paymenttype === 1) {
        var vData = {
            SettlementID: paymentid,
        };

        jsfn_ShowLoading();
        jsfn_ajaxPost('/Base/GetSettlementDetails', vData, jsfn_SettlementDetailResponse);
    }
    else if (paymenttype === 2) {
        var vData = {
            PaymentAccountID: paymentid,
        };

        jsfn_ShowLoading();
        jsfn_ajaxPost('/Base/GetPoADetails', vData, jsfn_PaymentAccountDetailResponse);
    }


}

function jsfn_PaymentAccountDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvPaymentAccountModal').modal('show');
        $('#dvPaymentAccountRefNo').html(data.PaymentAccountDetails.PaymentRefNo);
        $('#dvPaymentAccountDate').html(jsfn_FormatJsonDate(data.PaymentAccountDetails.PaymentDate));
        $('#dvPATaxPayerTypeName').html(data.PaymentAccountDetails.TaxPayerTypeName);
        $('#dvPATaxPayerRIN').html(data.PaymentAccountDetails.TaxPayerRIN);
        $('#dvPATaxPayerName').html(data.PaymentAccountDetails.TaxPayerName);
        $('#dvPaymentAccountAmount').html(data.PaymentAccountDetails.Amount.formatMoney(2, '₦'));
        $('#dvPATransactionReferenceNo').html(data.PaymentAccountDetails.TransactionReferenceNo);
        $('#dvRevenueStreamName').html(data.PaymentAccountDetails.RevenueStreamName);
        $('#dvNotes').html(data.PaymentAccountDetails.Notes);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_SettlementDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvSettlementModal').modal('show');
        $('#dvSettlementRefNo').html(data.SettlementDetails.SettlementRefNo);
        $('#dvSettlementDate').html(jsfn_FormatJsonDate(data.SettlementDetails.SettlementDate));
        $('#dvTaxPayerTypeName').html(data.SettlementDetails.TaxPayerTypeName);
        $('#dvTaxPayerRIN').html(data.SettlementDetails.TaxPayerRIN);
        $('#dvTaxPayerName').html(data.SettlementDetails.TaxPayerName);
        $('#dvSettlementAmount').html(data.SettlementDetails.SettlementAmount.formatMoney(2, '₦'));
        $('#dvASBRefNo').html(data.SettlementDetails.ASRefNo);
        $('#dvTransactionReferenceNo').html(data.SettlementDetails.TransactionReferenceNo);
        $('#dvSettlementNotes').html(data.SettlementDetails.SettlementNotes);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ChangeStatus(Individualid) {
    var vData = {
        IndividualID: Individualid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/CaptureIndividual/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_AddTCCResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');

        //Bind Table Content
        $("#dvTCCRequest").html(data.RequestList);
        jsfn_ConvertToDatableWithCustomSort($('#tblTCCRequest'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Assoicated Request Found');

        $("#cboTaxYear").val('').selectpicker('refresh');
        $("#dvAddTCCRequestModal").modal('hide');
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger', true, '#dvTCCRMessage');
    }
}


function jsfn_ChangeAssetLinkStatus(tpaid, tpid, tptid) {
    var vData = {
        TPAID: tpaid,
        TaxPayerID: tpid,
        TaxPayerTypeID: tptid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/CaptureIndividual/UpdateAssetStatus', vData, jsfn_ChangeAssetLinkStatusResponse);
}

function jsfn_ChangeAssetLinkStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vAssetTable.fnDestroy();
        $('#tbAssetList').html(data.AssetList);
        vAssetTable = jsfn_ConvertToDatableWithCustomSort($('#tblAssetList'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }, { "orderable": false }], 'No Assoicated Assets Found');
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}