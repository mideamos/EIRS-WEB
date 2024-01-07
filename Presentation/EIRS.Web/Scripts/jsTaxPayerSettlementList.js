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

    jsfn_ConvertToDatableWithCustomSort($('#tblSettlementList'), vColumnsList, 'No Settlement Found', vSortOrder);
}

function jsfn_SettlementDetails(sid) {

    var vData = {
        SettlementID: sid,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetSettlementDetails', vData, jsfn_SettlementDetailResponse);
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
        $('#dvTransactionReferenceNo').html(data.SettlementDetails.TransactionReferenceNo);
        $('#dvASBRefNo').html(data.SettlementDetails.ASRefNo);
        $('#dvSettlementNotes').html(data.SettlementDetails.SettlementNotes);
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
    jsfn_ajaxPost('/GetSettlementDetails/GetBillPaymentList', vData, jsfn_BillPaymentDetailResponse)
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
