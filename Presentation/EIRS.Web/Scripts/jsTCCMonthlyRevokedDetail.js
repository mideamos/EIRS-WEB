var vTCCRequestTable;
$(document).ready(function () {
    vTCCRequestTable = $("#tblRequest").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/OperationManager/TCCMonthlyRevokedDetailLoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.TaxYear = $("#hdnTaxYear").val();
                data.TaxMonth = $("#hdnTaxMonth").val();
                data.StatusID = $("#hdnStatusID").val();
            }, "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "RequestRefNo", "orderable": true, "name": "RequestRefNo" },
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName" },
            { "data": "TaxPayerRIN", "orderable": true, "name": "TaxPayerRIN" },
            { "data": "MobileNumber", "orderable": true, "name": "MobileNumber" },
            {
                "data": "RequestDate", "orderable": true, "name": "RequestDate", "render": function (data, type, req) {
                    var vRequestDate = new Date(parseInt(req["RequestDate"].substr(6)));
                    return vRequestDate.format("dd-mmm-yyyy");
                }
            },
            { "data": "StatusName", "orderable": true, "name": "StatusName" }
        ],
        "order": [[1, "asc"]],
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        }
    });
});
