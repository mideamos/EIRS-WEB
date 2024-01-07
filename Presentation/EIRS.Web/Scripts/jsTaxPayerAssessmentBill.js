jQuery(document).ready(function () {
    jsfn_bindTable();
});

function jsfn_bindTable() {
    var vColumnsList = [{
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": false
    }];

    var vSortOrder = [];

    jsfn_ConvertToDatableWithCustomSort($('#tblAssessment'), vColumnsList, 'No Assessment Bill Found', vSortOrder);
}

function jsfn_BillDetails(billid) {

    var vData = {
        AssessmentID: billid,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetAssessmentDetails', vData, jsfn_AssessmentDetailResponse)
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

function jsfn_BillPaymentDetails(billid) {
    var vData = {
        BillID: billid,
        BillTypeID: 1
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

