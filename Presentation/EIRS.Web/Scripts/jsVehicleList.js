var vVehicleDataTable;
$(document).ready(function () {
    vVehicleDataTable = $('#tblVehicle').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/ProfileVehicle/LoadData",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status === 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "VehicleRIN", "orderable": true, "name": "VehicleRIN" },
            { "data": "VehicleRegNumber", "orderable": true, "name": "VehicleRegNumber" },
            { "data": "VehicleSubTypeName", "orderable": true, "name": "VehicleSubTypeName" },
            { "data": "VehiclePurposeName", "orderable": true, "name": "VehiclePurposeName" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, aData) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                        + '</button><ul class="dropdown-menu"><li><a onclick="javascript:jsfn_VehicleInformation(' + aData["VehicleID"] + ')">'
                        + 'Quick View</a></li><li><a href="/ProfileVehicle/Details?id=' + aData["VehicleID"] + '&name=' + aData["VehicleRIN"] + '">Asset Details</a></li>'
                        + '<li><a onclick="javascript:jsfn_ChangeStatus(' + aData["VehicleID"] + ')">' + (aData["Active"] === 0 ? "Mark Active" : "Mark Inactive") + '</a></li>'
                        + '</ul></div>';
                }
            }],
        "order": [[1, "asc"]]
    });

});

function jsfn_ChangeStatus(Vehicleid) {
    var vData = {
        VehicleID: Vehicleid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/ProfileVehicle/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vVehicleDataTable.draw();
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}
