var vTCCRequestTable;
$(document).ready(function () {
    vTCCRequestTable = $("#tblRequest").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/ProcessTCCRequest/LoadData",
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
            { "data": "TaxYear", "orderable": true, "name": "TaxYear" },
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName" },
            { "data": "TaxPayerRIN", "orderable": true, "name": "TaxPayerRIN" },
            { "data": "MobileNumber", "orderable": true, "name": "MobileNumber" },
            {
                "data": "RequestDate", "orderable": true, "name": "RequestDate", "render": function (data, type, req) {
                    var vRequestDate = new Date(parseInt(req["RequestDate"].substr(6)));
                    return vRequestDate.format("dd-mmm-yyyy");
                }
            },
            { "data": "StatusName", "orderable": true, "name": "StatusName" },
            {
                "data": "", "orderable": false, "name": "Action",
                "render": function (data, type, req) {
                    return '<div class="btn-group"><button '+(req.StatusID === 15 ? "disabled" : "")+' type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span></button>'
                        + '<ul class="dropdown-menu">'
                        + '<li><a href="/ProcessTCCRequest/Details?reqid=' + req.TCCRequestID + '">Process Request</a></li>'
                        //+ (req.StatusID < 9 ? '<li><a href="/ProcessTCCRequest/Edit?reqid=' + req.TCCRequestID + '">Edit Request</a></li>' : '')
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
