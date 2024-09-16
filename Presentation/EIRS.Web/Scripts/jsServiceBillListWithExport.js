jQuery(document).ready(function () {

    $('#tblServiceBill').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/ServiceBill/LoadExportData",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "ServiceBillRefNo", "orderable": true, "name": "ServiceBillRefNo" },
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName" },
            {
                "data": "ServiceBillDate", "orderable": true, "name": "ServiceBillDate", "render": function (data, type, servbill) {
                    if (servbill.ServiceBillDate != undefined || servbill.ServiceBillDate != null) {
                        return jsfn_FormatJsonDate(servbill.ServiceBillDate, "dd-mmm-yyyy");
                    }
                    else {
                        return '';
                    }
                }
            },
            {
                "data": "ServiceBillAmount", "orderable": true, "name": "ServiceBillAmount", "render": function (data, type, servbill) {
                    return servbill.ServiceBillAmount.formatMoney();
                }
            },
            { "data": "SettlementStatusName", "orderable": true, "name": "SettlementStatusName" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, aData) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                        + '</button><ul class="dropdown-menu"><li><a onclick="javascript:jsfn_BillDetails(' + aData["ServiceBillID"] + ')">Quick View</a></li><li><a onclick="javascript:jsfn_BillPaymentDetails(' + aData["ServiceBillID"] + ')">View Payments </a></li></ul></div>';
                }
            }],
        "order": [[2, "desc"]]
    });
});

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