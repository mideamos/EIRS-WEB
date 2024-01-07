$(document).ready(function () {
    $("#tblRequest").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/Staff/LoadSignTCCData",
            "type": "POST",
            "datatype": "json",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "RequestRefNo", "orderable": true, "name": "RequestRefNo" },
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName" },
            { "data": "MobileNumber", "orderable": true, "name": "MobileNumber" },
            {
                "data": "RequestDate", "orderable": true, "name": "RequestDate", "render": function (data, type, req) {
                    var vRequestDate = new Date(parseInt(req["RequestDate"].substr(6)));
                    return vRequestDate.format("dd-mmm-yyyy");
                }
            },
            { "data": "TaxYear", "orderable": true, "name": "TaxYear" },
            { "data": "SettlementStatusName", "orderable": true, "name": "SettlementStatusName" },
            {
                "data": "", "orderable": false, "name": "Action",
                "render": function (data, type, req) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span></button>'
                        + '<ul class="dropdown-menu">'
                        + '<li><a href="/Staff/SignTCCDetails?reqId=' + req.TCCRequestID + '">Open Request</a></li>'
                        + '</ul>';
                }
            }
        ],
        "order": [[1, "asc"]],
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        }
    });
});
