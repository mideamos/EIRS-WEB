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

    jsfn_ConvertToDatableWithCustomSort($('#tblServiceBills'), vColumnsList, 'No Service Bills Found', vSortOrder);
}

function jsfn_BillDetails(billid) {

    var vData = {
        ServiceBillID: billid,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetServiceBillDetails', vData, jsfn_ServiceBillDetailResponse)
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

function jsfn_BillPaymentDetails(billid) {
    var vData = {
        BillID: billid,
        BillTypeID: 2
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