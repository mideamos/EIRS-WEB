var vPAYEOutputAggregationDetailTable;

jQuery(document).ready(function () {
    vPAYEOutputAggregationDetailTable = $("#tblPAYEOutputAggregation").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "responsive": true,
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/OperationManager/PAYEOutputAggregationDetailLoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.TaxYear = $("#hdnTaxYear").val();
                data.EmployerRIN = $("#hdnEmployerRIN").val();
                data.TaxOfficeID = $("#hdnTaxOffice").val();
            }, "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "EmployeeName", "orderable": true, "name": "EmployeeName" },
            { "data": "EmployeeRIN", "orderable": true, "name": "EmployeeRIN" },
            {
                "data": "AmountContributed", "orderable": true, "name": "AmountContributed", "render": function (data, type, st) {
                    return st.AmountContributed.formatMoney();
                }
            }
        ],
        "order": [[1, "asc"]]
    });
});
