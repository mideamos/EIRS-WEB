var vVehicleDataTable;

jQuery(document).ready(function () {
    jsfn_BuildDataTable();
});

function jsfn_ChangeStatus(Vehicleid) {
    var vData = {
        VehicleID: Vehicleid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Vehicle/UpdateStatus', vData, jsfn_ChangeStatusResponse);
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

function jsfn_BuildDataTable() {
    vVehicleDataTable = $("#tbVehicle").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/Vehicle/LoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.VehicleRIN = $("#txtVehicleRIN").val();
                data.VehicleRegNumber = $("#txtVehicleRegNumber").val();
                data.VIN = $("#txtVIN").val();
                data.VehicleTypeName = $("#txtVehicleTypeName").val();
                data.VehicleSubTypeName = $("#txtVehicleSubTypeName").val();
                data.LGAName = $("#txtLGAName").val();
                data.VehiclePurposeName = $("#txtPurposeName").val();
                data.VehicleFunctionName = $("#txtVehicleFunctionName").val();
                data.VehicleOwnershipName = $("#txtVehicleOwnershipName").val();
                data.VehicleDescription = $("#txtVehicleDescription").val();
                data.ActiveText = $("#txtStatus").val();
            }
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",

        "columns": [
            { "data": "VehicleRIN", "orderable": true, "name": "VehicleRIN" },
            { "data": "VehicleRegNumber", "orderable": true, "name": "VehicleRegNumber" },
            { "data": "VIN", "orderable": true, "name": "VIN" },
            { "data": "VehicleTypeName", "orderable": true, "name": "VehicleTypeName" },
            { "data": "VehicleSubTypeName", "orderable": true, "name": "VehicleSubTypeName" },
            { "data": "LGAName", "orderable": true, "name": "LGAName" },
            { "data": "VehiclePurposeName", "orderable": true, "name": "VehiclePurposeName" },
            { "data": "VehicleFunctionName", "orderable": true, "name": "VehicleFunctionName" },
            { "data": "VehicleOwnershipName", "orderable": true, "name": "VehicleOwnershipName" },
            { "data": "VehicleDescription", "orderable": true, "name": "VehicleDescription" },
            { "data": "ActiveText", "orderable": true, "name": "ActiveText" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, veh) {
                    return '<div class="btn-group">' + '<button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="false">' +
                        'Actions  <span class="caret"></span></button><ul class="dropdown-menu" role="menu">'
                        + '<li><a href="/Vehicle/Details/' + veh.VehicleID + '/' + veh.VehicleRIN.toSeoUrl() + '">View Details</a></li>'
                        + '<li><a href="/Vehicle/Edit/' + veh.VehicleID + '/' + veh.VehicleRIN.toSeoUrl() + '">Edit Details</a></li>'
                        + '<li><a href="/Vehicle/TaxPayerList/' + veh.VehicleID + '/' + veh.VehicleRIN.toSeoUrl() + '">Tax Payer Information</a></li>'
                        + '<li><a onclick="javascript:jsfn_ChangeStatus(' + veh.VehicleID + ')">' + (veh.Active == 0 ? "Mark Active" : "Mark Inactive") + '</a></li>'
                        + '</ul></div>';
                }
            },
        ],
        "order": [[1, "asc"]]
    });


    $("#tbVehicle thead th input[type=text]").on('keyup change', function () {
        vVehicleDataTable
            .column($(this.data).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });


    //Hide Columns
    vVehicleDataTable.column(2).visible(false);
    vVehicleDataTable.column(4).visible(false);
    vVehicleDataTable.column(5).visible(false);
    vVehicleDataTable.column(6).visible(false);

}

