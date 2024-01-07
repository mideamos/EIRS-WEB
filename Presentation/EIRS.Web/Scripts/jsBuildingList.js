var vBuildingDataTable;
$(document).ready(function () {
    vBuildingDataTable = $('#tblBuilding').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/ProfileBuilding/LoadData",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "BuildingRIN", "orderable": true, "name": "BuildingRIN" },
            { "data": "BuildingName", "orderable": true, "name": "BuildingName" },
            { "data": "StreetName", "orderable": true, "name": "StreetName" },
            { "data": "BuildingPurposeName", "orderable": true, "name": "BuildingPurposeName" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, aData) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                        + '</button><ul class="dropdown-menu"><li><a onclick="javascript:jsfn_BuildingInformation(' + aData["BuildingID"] + ')">'
                        + 'Quick View</a></li><li><a href="/ProfileBuilding/Details?id=' + aData["BuildingID"] + '&name=' + aData["BuildingRIN"] + '">Asset Details</a></li>'
                        + '<li><a onclick="javascript:jsfn_ChangeStatus(' + aData["BuildingID"] + ')">' + (aData["Active"] === 0 ? "Mark Active" : "Mark Inactive") + '</a></li>'
                        + '</ul></div>';
                }
            }],
        "order": [[1, "asc"]]
    });
});

function jsfn_ChangeStatus(Buildingid) {
    var vData = {
        BuildingID: Buildingid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/ProfileBuilding/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vBuildingDataTable.draw();
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}