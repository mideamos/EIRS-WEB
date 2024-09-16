$(document).ready(function () {
    $("#tblCertificateType").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/CertificateType/LoadData",
            "type": "POST",
            "datatype": "json",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "CertificateTypeName", "orderable": true, "name": "CertificateTypeName" },
            { "data": "TaxYear", "orderable": true, "name": "TaxYear" },
            { "data": "ActiveText", "orderable": true, "name": "ActiveText" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, ct) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                        + '</button><ul class="dropdown-menu"><li><a href="/CertificateType/Edit?id=' + ct.CertificateTypeID + '&name=' + toSeoUrl(ct.CertificateTypeName) +'">'
                        + 'Edit Details</a></li><li><a href="/CertificateType/Details?id=' + ct.CertificateTypeID + '&name=' + toSeoUrl(ct.CertificateTypeName) + '">'
                        + 'View Details</a></li></ul></div>';
                }, "width": "10%"
            },
        ],
        "order": [[0, "asc"]]
    });
});