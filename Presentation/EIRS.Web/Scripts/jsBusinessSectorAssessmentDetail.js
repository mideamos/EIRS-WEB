$(document).ready(function () {
    $("#tblTaxPayerList").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/OperationManager/BusinessSectorAssessmentDetailLoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.TaxYear = $("#hdnTaxYear").val();
                data.BusinessSectorID = $("#hdnBusinessSectorID").val();
            },
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
            {
                "data": "BillAmount", "orderable": true, "name": "BillAmount", "width": "25%", "render": function (data, type, st) {
                    return st.BillAmount.formatMoney();
                }
            },
            {
                "data": "SettlementAmount", "orderable": true, "name": "SettlementAmount", "width": "25%", "render": function (data, type, st) {
                    return st.SettlementAmount.formatMoney();
                }
            },
            {
                "data": "PoAAmount", "orderable": true, "name": "PoAAmount", "width": "25%", "render": function (data, type, st) {
                    return st.PoAAmount.formatMoney();
                }
            },
            {
                "data": "OutstandingAmount", "orderable": true, "name": "OutstandingAmount", "width": "25%", "render": function (data, type, st) {
                    return st.OutstandingAmount.formatMoney();
                }
            }
        ],
        "order": [[0, "desc"]]
    });
});
