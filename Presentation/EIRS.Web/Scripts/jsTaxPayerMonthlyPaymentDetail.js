$(document).ready(function () {
    $("#tblTransactionList").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/OperationManager/TaxPayerMonthlyPaymentDetailLoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.TaxPayerTypeID = $("#hdnTaxPayerTypeID").val();
                data.TaxPayerID = $("#hdnTaxPayerID").val();
                data.Year = $("#hdnTaxYear").val();
                data.Month = $("#hdnTaxMonth").val();
                data.RevenueStreamID = $("#hdnRevenueStreamID").val();
            }, "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            {
                "data": "PaymentDate", "orderable": true, "name": "PaymentDate", "width": "25%", "render": function (data, type, st) {
                    var vPaymentDate = new Date(parseInt(st["PaymentDate"].substr(6)));
                    return vPaymentDate.format("dd-mmm-yyyy");
                }
            },
            { "data": "PaymentTypeName", "orderable": true, "name": "PaymentTypeName", "width": "25%" },
            { "data": "PaymentRefNo", "orderable": true, "name": "PaymentRefNo", "width": "25%" },
            {
                "data": "Amount", "orderable": true, "name": "Amount", "width": "25%", "render": function (data, type, st) {
                    return st.Amount.formatMoney();
                }
            }
        ],
        "order": [[0, "desc"]]
    });
});
