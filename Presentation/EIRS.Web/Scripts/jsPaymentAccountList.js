$(document).ready(function () {
    $('#tblPoA').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/Settle/GetPoAData",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [{
            "data": "PaymentRefNo"
        }, {
            "data": "TaxPayerName"
        }, {
            "data": "PaymentDate"
        }, {
            "data": "Amount"
        }, {
            "data": "SettlementMethodName"
            }, {
                "data": "PaymentAccountID"
        }],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('td:eq(3)', nRow).html(aData["Amount"].formatMoney());
            $('td:eq(5)', nRow).html('<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                + '</button><ul class="dropdown-menu"><li><a onclick="javascript:jsfn_PaymentAccountDetails(' + aData["PaymentAccountID"] + ')">'
                + 'View Details</a></li></ul></div>');
        },
    });

});

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
