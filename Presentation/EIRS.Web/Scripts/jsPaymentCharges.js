$(document).ready(function () {
    $("#tblPaymentCharges").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/OperationManager/PaymentChargesLoadData",
            "type": "POST",
            "datatype": "json",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "TaxPayerRIN", "orderable": true, "name": "TaxPayerRIN" },
            { "data": "TaxPayerTypeName", "orderable": true, "name": "TaxPayerTypeName" },
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName" },
            { "data": "TaxYear", "orderable": true, "name": "TaxYear" },
            { "data": "BillRefNo", "orderable": true, "name": "BillRefNo" },
            { "data": "RevenueStreamName", "orderable": true, "name": "RevenueStreamName" },
            {
                "data": "Penalty", "orderable": true, "name": "Penalty", "render": function (data, type, st) {
                    return st.Penalty.formatMoney();
                } },
            {
                "data": "Interest", "orderable": true, "name": "Interest","render": function (data, type, st) {
                    return st.Interest.formatMoney();
                } },
            {
                "data": "TotalCharge", "orderable": true, "name": "TotalCharge","render": function (data, type, st) {
                    return st.TotalCharge.formatMoney();
                } },
            {
                "data": "ChargeDate", "orderable": true, "name": "ChargeDate", "render": function (data, type, st) {
                    var vChargeDate = new Date(parseInt(st.ChargeDate.substr(6)));
                    return vChargeDate.format("dd-mmm-yyyy");
                } },
            { "data": "BillStatus", "orderable": true, "name": "BillStatus" },
        ],
        "order": [[9, "desc"]]
    });
});
