$(document).ready(function () {
    $('#tblGovernment').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/CaptureGovernment/LoadExportData",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "GovernmentRIN", "orderable": true, "name": "GovernmentRIN" },
            { "data": "GovernmentName", "orderable": true, "name": "GovernmentName" },
            { "data": "TIN", "orderable": true, "name": "TIN" },
            { "data": "ContactAddress", "orderable": true, "name": "ContactAddress" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, aData) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                        + '</button><ul class="dropdown-menu"><li><a onclick="javascript:jsfn_GovernmentInformation(' + aData["GovernmentID"] + ')">'
                        + 'Quick View</a></li><li><a href="/CaptureGovernment/Details?id=' + aData["GovernmentID"] + '&name=' + aData["GovernmentRIN"] + '">'
                        + 'Tax Payer Details</a></li></ul></div>';
                }
            }],
        "order": [[1, "asc"]]
    });

});
