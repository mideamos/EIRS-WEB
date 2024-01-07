var vLandDataTable;
$(document).ready(function () {
    vLandDataTable = $('#tblLand').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/ProfileLand/LoadExportData",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "LandRIN", "orderable": true, "name": "LandRIN" },
            { "data": "PlotNumber", "orderable": true, "name": "PlotNumber" },
            { "data": "LandOccupier", "orderable": true, "name": "LandOccupier" },
            { "data": "LandFunctionName", "orderable": true, "name": "LandFunctionName" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, aData) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                        + '</button><ul class="dropdown-menu"><li><a onclick="javascript:jsfn_LandInformation(' + aData["LandID"] + ')">'
                        + 'Quick View</a></li><li><a href="/ProfileLand/Details?id=' + aData["LandID"] + '&name=' + aData["LandRIN"] + '">Asset Details</a></li>'
                        + '<li><a onclick="javascript:jsfn_ChangeStatus(' + aData["LandID"] + ')">' + (aData["Active"] === 0 ? "Mark Active" : "Mark Inactive") + '</a></li>'
                        + '</ul></div>';
                }
            }],
        "order": [[1, "asc"]]
    });

});

function jsfn_ChangeStatus(lndid) {
    var vData = {
        LandID: lndid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/ProfileLand/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vLandDataTable.draw();
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

