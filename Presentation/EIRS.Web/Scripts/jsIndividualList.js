$(document).ready(function () {
    $('#tblIndividual').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/CaptureIndividual/LoadData",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "IndividualRIN", "orderable": true, "name": "IndividualRIN" },
            {
                "data": "FirstName", "orderable": true, "name": "FirstName", "render": function (data, type, ind) {
                    return ind.FirstName + ' ' + ind.LastName;
                }
            }, { "data": "TIN", "orderable": true, "name": "TIN" },
            { "data": "ContactAddress", "orderable": true, "name": "ContactAddress" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, ind) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                        + '</button><ul class="dropdown-menu"><li><a onclick="javascript:jsfn_IndividualInformation(' + ind.IndividualID + ')">'
                        + 'Quick View</a></li><li><a href="/CaptureIndividual/Details?id=' + ind.IndividualID + '&name=' + ind.IndividualRIN + '">'
                        + 'Tax Payer Details</a></li></ul></div>';
                }
            }],
        "order": [[1, "asc"]]
    });

});
