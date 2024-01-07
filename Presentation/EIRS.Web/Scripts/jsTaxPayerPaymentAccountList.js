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

    jsfn_ConvertToDatableWithCustomSort($('#tblPoA'), vColumnsList, 'No Payment Acccount Found', vSortOrder);
}


function jsfn_PaymentAccountDetails(paid) {

    var vData = {
        PaymentAccountID: paid,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetPoADetails', vData, jsfn_PaymentAccountDetailResponse);
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
