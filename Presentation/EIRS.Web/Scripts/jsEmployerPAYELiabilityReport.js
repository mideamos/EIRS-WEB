$(document).ready(function () {
    $("#tblEmployerPAYELiabilityReport").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/OperationManager/EmployerPAYELiabilityLoadData",
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
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName" },
            { "data": "MobileNumber", "orderable": true, "name": "MobileNumber" },
            { "data": "AssetName", "orderable": true, "name": "AssetName" },
            { "data": "StaffCount", "orderable": true, "name": "StaffCount" },
            { "data": "BillCount", "orderable": true, "name": "BillCount" },
            {
                "data": "BillAmount", "orderable": true, "name": "BillAmount", "width": "25%", "render": function (data, type, st) {
                    return st.BillAmount.formatMoney();
                }
            },
            {
                "data": "PaymentAmount", "orderable": true, "name": "PaymentAmount", "width": "25%", "render": function (data, type, st) {
                    return st.PaymentAmount.formatMoney();
                }
            },
            {
                "data": "Balance", "orderable": true, "name": "Balance", "width": "25%", "render": function (data, type, st) {
                    return st.Balance.formatMoney();
                }
            },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, st) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                        + '</button><ul class="dropdown-menu"><li><a href="/OperationManager/EmployerPAYELiabilityDetails?tpid=' + st.TaxPayerID + '&aid='+st.AssetID+'">'
                        + 'View Liability Report</a></li></ul></div>';
                }, "width": "10%"
            }
        ],
        "order": [[1, "asc"]]
    });


    $("#btnExportToExcel").click(function () {
        var vUrl = '/OperationManager/EmployerPAYELiabilityExportToExcel';
        var win = window.open(vUrl, '_blank');
        win.focus();
    });

});