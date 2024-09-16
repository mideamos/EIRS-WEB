jQuery(document).ready(function () {
    $('#tblSettlementList').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/Settle/GetSettleList",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status === 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [{
            "data": "TaxPayerName"
        }, {
            "data": "ASRefNo"
        }, {
            "data": "SettlementDate"
        }, {
            "data": "SettlementAmount"
        }, {
            "data": "SettlementMethodName"
        }, {
            "data": "SettlementID"
        }],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            var vBillDate = new Date(parseInt(aData["SettlementDate"].substr(6)));
            $('td:eq(2)', nRow).html(vBillDate.format("dd-mmm-yyyy"));
            $('td:eq(3)', nRow).html(aData["SettlementAmount"].formatMoney());
            $('td:eq(5)', nRow).html('<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span></button>'
                + '<ul class="dropdown-menu"><li><a onclick="javascript:jsfn_SettlementDetails(' + aData["SettlementID"] + ')">View Details</a></li></ul>'
            );
        }
    });
});

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
