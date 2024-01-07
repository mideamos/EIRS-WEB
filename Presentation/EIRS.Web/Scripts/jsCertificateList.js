var vCertificateTable;
$(document).ready(function () {
    vCertificateTable = $("#tblCertificate").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/Certificate/LoadData",
            "type": "POST",
            "datatype": "json",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "CertificateNumber", "orderable": true, "name": "CertificateNumber" },
            { "data": "CertificateTypeName", "orderable": true, "name": "CertificateTypeName" },
            { "data": "TaxYear", "orderable": true, "name": "TaxYear" },
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName" },
            { "data": "TaxPayerRIN", "orderable": true, "name": "TaxPayerRIN" },
            //{
            //    "data": "RequestDate", "orderable": true, "name": "RequestDate", "render": function (data, type, req) {
            //        var vRequestDate = new Date(parseInt(req["RequestDate"].substr(6)));
            //        return vRequestDate.format("dd-mmm-yyyy");
            //    }
            //},
            { "data": "CertificateStatusName", "orderable": true, "name": "CertificateStatusName" },
            {
                "data": "", "orderable": false, "name": "Action",
                "render": function (data, type, crt) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span></button>'
                        + '<ul class="dropdown-menu">'
                        + '<li><a href="/Certificate/Update?id=' + crt.CertificateID + '">Edit Details</a></li>'
                        + (crt.StatusID === 2 ? '<li><a target="_blank" href="/Certificate/GeneratePDF?id=' + crt.CertificateID + '">Generate PDF</a></li>' : '')
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
