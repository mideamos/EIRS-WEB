var vBuildingUnitDataTable;

jQuery(document).ready(function() {
    jsfn_BuildDataTable();
});

function jsfn_BuildDataTable() {
    vBuildingUnitDataTable = $("#tbBuildingUnit").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/BuildingUnit/LoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.UnitNumber = $("#txtUnitNumber").val();
                data.UnitPurposeName = $("#txtUnitPurpose").val();
                data.UnitFunctionName = $("#txtUnitFunction").val();
                data.UnitOccupancyName = $("#txtUnitOccupancy").val();
                data.SizeName = $("#txtUnitSize").val();
                data.ActiveText = $("#txtStatus").val();
            }
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",

        "columns": [
            { "data": "UnitNumber", "orderable": true, "name": "UnitNumber" },
            { "data": "UnitPurposeName", "orderable": true, "name": "UnitPurposeName" },
            { "data": "UnitFunctionName", "orderable": true, "name": "UnitFunctionName" },
            { "data": "UnitOccupancyName", "orderable": true, "name": "UnitOccupancyName" },
            { "data": "SizeName", "orderable": true, "name": "SizeName" },
            { "data": "ActiveText", "orderable": true, "name": "ActiveText" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, Buiunit) {
                    return '<div class="btn-group">' + '<button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="false">' +
                        'Actions  <span class="caret"></span></button><ul class="dropdown-menu" role="menu">'
                        + '<li><a href="/BuildingUnit/Details/' + Buiunit.BuildingUnitID + '/' + Buiunit.UnitNumber.toSeoUrl() + '">View Details</a></li>'
                        + '<li><a href="/BuildingUnit/Edit/' + Buiunit.BuildingUnitID + '/' + Buiunit.UnitNumber.toSeoUrl() + '">Edit Details</a></li>'
                        + '<li><a onclick="javascript:jsfn_ChangeStatus(' + Buiunit.BuildingUnitID + ')">' + (Buiunit.Active == 0 ? "Mark Active" : "Mark Inactive") + '</a></li>'
                        + '</ul></div>';
                }
            },
        ],
        "order": [[1, "asc"]]
    });


    $("#tbBuildingUnit thead th input[type=text]").on('change', function () {
        vBuildingUnitDataTable
            .column($(this.data).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });
    
}

function jsfn_ChangeStatus(botypeid) {
    var vData = {
        BuildingUnitID: botypeid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/BuildingUnit/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vBuildingUnitDataTable.draw();
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

