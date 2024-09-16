$(document).ready(function () {
    $("#tblTCC").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/TaxClearanceCertificate/LoadData",
            "type": "POST",
            "datatype": "json",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "TCCNumber", "orderable": true, "name": "TCCNumber" },
            {
                "data": "TCCDate", "orderable": true, "name": "TCCDate", "render": function (data, type, tcc) {
                    var vTCCDate = new Date(parseInt(tcc["TCCDate"].substr(6)));
                    return vTCCDate.format("dd-mmm-yyyy");
                }
            },
            { "data": "TaxYear", "orderable": true, "name": "TaxYear" },
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName" },
            { "data": "StatusName", "orderable": true, "name": "StatusName" },
            {
                "data": "", "orderable": false, "name": "Action",
                "render": function (data, type, tcc) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span></button>'
                        + '<ul class="dropdown-menu">'
                        + '<li><a href="/TaxClearanceCertificate/Details?tccid=' + tcc.TCCID+'">Certificate Details</a></li>'
                        + '<li><a href="/TaxClearanceCertificate/GeneratePDF?tccid=' + tcc.TCCID +'">View PDF TCC</a></li>'
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
