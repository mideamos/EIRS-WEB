$(document).ready(function () {
    $("#tblTaxPayerList").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/OperationManager/TaxPayerTypeByTaxOfficeDetailLoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.TaxOfficeID = $("#hdnTaxOfficeID").val();
            }, "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "TaxPayerTypeName", "orderable": true, "name": "TaxPayerTypeName", "width": "25%" },
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName", "width": "25%" },
            { "data": "TaxPayerRIN", "orderable": true, "name": "TaxPayerRIN", "width": "25%" },
            { "data": "TaxPayerMobileNumber", "orderable": true, "name": "TaxPayerMobileNumber", "width": "25%" },
            { "data": "TaxPayerAddress", "orderable": true, "name": "TaxPayerAddress", "width": "25%" },
            
        ],
        "order": [[1, "asc"]]
    });
});
