$(document).ready(function () {
    $("#tblTransactionList").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/OperationManager/PaymentChannelByRevenueStreamDetailLoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.SettlementMethodID = $("#hdnSettlementMethodID").val();
                data.TaxYear = $("#hdnTaxYear").val();
                data.RevenueStreamID = $("#hdnRevenueStreamID").val();
                data.FromDate = $("#hdnFromDate").val();
                data.ToDate = $("#hdnToDate").val();
            }, "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "TaxPayerRIN", "orderable": true, "name": "TaxPayerRIN", "width": "25%" },
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName", "width": "25%" },
            { "data": "TaxPayerTypeName", "orderable": true, "name": "TaxPayerTypeName", "width": "25%" },
            { "data": "RevenueStreamName", "orderable": true, "name": "RevenueStreamName", "width": "25%" },
            {
                "data": "PaymentDate", "orderable": true, "name": "PaymentDate", "width": "25%", "render": function (data, type, st) {
                    var vPaymentDate = new Date(parseInt(st["PaymentDate"].substr(6)));
                    return vPaymentDate.format("dd-mmm-yyyy");
                }
            },
            {
                "data": "Amount", "orderable": true, "name": "Amount", "width": "25%", "render": function (data, type, st) {
                    return st.Amount.formatMoney();
                }
            }
        ],
        "order": [[4, "desc"]]
    });
});
