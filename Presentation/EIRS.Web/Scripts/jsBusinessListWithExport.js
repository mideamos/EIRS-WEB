var vBusinessDataTable;
$(document).ready(function () {
    vBusinessDataTable = $('#tblBusiness').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/ProfileBusiness/LoadExportData",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status === 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "BusinessRIN", "orderable": true, "name": "BusinessRIN" },
            { "data": "BusinessName", "orderable": true, "name": "BusinessName" },
            { "data": "BusinessAddress", "orderable": true, "name": "BusinessAddress" },
            { "data": "BusinessSubSectorName", "orderable": true, "name": "BusinessSubSectorName" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, aData) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                        + '</button><ul class="dropdown-menu">'
                        + '<li><a onclick="javascript:jsfn_BusinessInformation(' + aData["BusinessID"] + ')">Quick View</a></li>'
                        + '<li><a href="/ProfileBusiness/Details?id=' + aData["BusinessID"] + '&name=' + aData["BusinessRIN"] + '">Asset Details</a></li>'
                        + '<li><a onclick="javascript:jsfn_ChangeStatus(' + aData["BusinessID"] + ')">' + (aData['Active'] === 0 ? "Mark Active" : "Mark Inactive") + '</a></li>'
                        + '</ul></div>';
                }
            }],
        "order": [[1, "asc"]]
    });

});


function jsfn_ChangeStatus(Businessid) {
    var vData = {
        BusinessID: Businessid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/ProfileBusiness/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vBusinessDataTable.draw();
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}
