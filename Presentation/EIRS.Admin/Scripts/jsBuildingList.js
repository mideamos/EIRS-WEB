var vBuildingDataTable;

jQuery(document).ready(function () {
    jsfn_BuildDataTable();
});

function jsfn_ChangeStatus(Buildingid) {
    var vData = {
        BuildingID: Buildingid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Building/UpdateStatus', vData, jsfn_ChangeStatusResponse);
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

function jsfn_BuildDataTable() {
    vBuildingDataTable = $("#tbBuilding").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/Building/LoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.BuildingRIN = $("#txtBuildingRIN").val();
                data.BuildingTagNumber = $("#txtBuildingTagNumber").val();
                data.BuildingName = $("#txtBuildingName").val();
                data.BuildingNumber = $("#txtBuildingNumber").val();
                data.StreetName = $("#txtStreetName").val();
                data.OffStreetName = $("#txtOffStreetName").val();
                data.TownName = $("#txtTownName").val();
                data.LGAName = $("#txtLGAName").val();
                data.WardName = $("#txtWardName").val();
                data.BuildingTypeName = $("#txtBuildingTypeName").val();
                data.BuildingCompletionName = $("#txtBuildingCompletionName").val();
                data.BuildingPurposeName = $("#txtBuildingPurposeName").val();
                data.BuildingOwnershipName = $("#txtBuildingOwnershipName").val();
                data.NoOfUnits = $("#txtNoOfUnits").val();
                data.BuildingSize_Length = $("#txtBuildingLength").val();
                data.BuildingSize_Width = $("#txtBuildingWidth").val();
                data.Latitude = $("#txtLatitude").val();
                data.Longitude = $("#txtLongitude").val();
                data.ActiveText = $("#txtStatus").val();
            }
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",

        "columns": [
            { "data": "BuildingRIN", "orderable": true, "name": "BuildingRIN" },
            { "data": "BuildingTagNumber", "orderable": true, "name": "BuildingTagNumber" },
            { "data": "BuildingName", "orderable": true, "name": "BuildingName" },
            { "data": "BuildingNumber", "orderable": true, "name": "BuildingNumber" },
            { "data": "StreetName", "orderable": true, "name": "StreetName" },
            { "data": "OffStreetName", "orderable": true, "name": "OffStreetName" },
            { "data": "TownName", "orderable": true, "name": "TownName" },
            { "data": "LGAName", "orderable": true, "name": "LGAName" },
            { "data": "WardName", "orderable": true, "name": "WardName" },
            { "data": "BuildingTypeName", "orderable": true, "name": "BuildingTypeName" },
            { "data": "BuildingCompletionName", "orderable": true, "name": "BuildingCompletionName" },
            { "data": "BuildingPurposeName", "orderable": true, "name": "BuildingPurposeName" },
            { "data": "BuildingOwnershipName", "orderable": true, "name": "BuildingOwnershipName" },
            { "data": "NoOfUnits", "orderable": true, "name": "NoOfUnits" },
            { "data": "BuildingSize_Length", "orderable": true, "name": "BuildingSize_Length" },
            { "data": "BuildingSize_Width", "orderable": true, "name": "BuildingSize_Width" },
            { "data": "Latitude", "orderable": true, "name": "Latitude" },
            { "data": "Longitude", "orderable": true, "name": "Longitude" },
            { "data": "ActiveText", "orderable": true, "name": "ActiveText" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, Bui) {
                    return '<div class="btn-group">' + '<button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="false">' +
                        'Actions  <span class="caret"></span></button><ul class="dropdown-menu" role="menu">'
                        + '<li><a href="/Building/Details/' + Bui.BuildingID + '/' + Bui.BuildingRIN.toSeoUrl() + '">View Details</a></li>'
                        + '<li><a href="/Building/Edit/' + Bui.BuildingID + '/' + Bui.BuildingRIN.toSeoUrl() + '">Edit Details</a></li>'
                        + '<li><a href="/Building/TaxPayerList/' + Bui.BuildingID + '/' + Bui.BuildingRIN.toSeoUrl() + '">Tax Payer Information</a></li>'
                        + '<li><a href="/Building/BusinessInformation/' + Bui.BuildingID + '/' + Bui.BuildingRIN.toSeoUrl() + '">Business Information</a></li>'
                        + '<li><a href="/Building/UnitInformation/' + Bui.BuildingID + '/' + Bui.BuildingRIN.toSeoUrl() + '">Unit Information</a></li>'
                        + '<li><a href="/Building/LandInformation/' + Bui.BuildingID + '/' + Bui.BuildingRIN.toSeoUrl() + '">Land Information</a></li>'
                        + '<li><a onclick="javascript:jsfn_ChangeStatus(' + Bui.BuildingID + ')">' + (Bui.Active == 0 ? "Mark Active" : "Mark Inactive") + '</a></li>'
                        + '</ul></div>';
                }
            },
        ],
        "order": [[1, "asc"]]
    });


    $("#tbBuilding thead th input[type=text]").on('change', function () {
        vBuildingDataTable
            .column($(this.data).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });


    //Hide Columns
    vBuildingDataTable.column(2).visible(false);
    vBuildingDataTable.column(3).visible(false);
    vBuildingDataTable.column(5).visible(false);
    vBuildingDataTable.column(6).visible(false);
    vBuildingDataTable.column(8).visible(false);
    vBuildingDataTable.column(10).visible(false);
    vBuildingDataTable.column(11).visible(false);
    vBuildingDataTable.column(12).visible(false);
    vBuildingDataTable.column(13).visible(false);
    vBuildingDataTable.column(14).visible(false);
    vBuildingDataTable.column(15).visible(false);
    vBuildingDataTable.column(16).visible(false);
    vBuildingDataTable.column(17).visible(false);
}

