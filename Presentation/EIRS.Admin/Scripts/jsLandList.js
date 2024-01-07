var vLandDataTable;

jQuery(document).ready(function () {
    jsfn_BuildDataTable();
});

function jsfn_ChangeStatus(lndid) {
    var vData = {
        LandID: lndid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Land/UpdateStatus', vData, jsfn_ChangeStatusResponse);
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

function jsfn_BuildDataTable() {
    vLandDataTable = $("#tbLand").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/Land/LoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.LandRIN = $("#txtLandRIN").val();
                data.PlotNumber = $("#txtPlotNumber").val();
                data.StreetName = $("#txtStreetName").val();
                data.TownName = $("#txtTownName").val();
                data.LGAName = $("#txtLGAName").val();
                data.WardName = $("#txtWardName").val();
                data.LandSize_Length = $("#txtLandLength").val();
                data.LandSize_Width = $("#txtLandWidth").val();
                data.C_OF_O_Ref = $("#txtC_Of_O_Ref").val();
                data.LandPurposeName = $("#txtLandPurposeName").val();
                data.LandFunctionName = $("#txtLandFunctionName").val();
                data.LandOwnershipName = $("#txtLandOwnershipName").val();
                data.LandDevelopmentName = $("#txtLandDevelopmentName").val();
                data.Latitude = $("#txtLatitude").val();
                data.Longitude = $("#txtLongitude").val();
                data.LandStreetConditionName = $("#txtStreetCondition").val();
                data.ValueOfLand = $("#txtValueOfLand").val();
                data.Neighborhood = $("#txtNeighborhood").val();
                data.ActiveText = $("#txtStatus").val();
            }
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",

        "columns": [
            { "data": "LandRIN", "orderable": true, "name": "LandRIN" },
            { "data": "PlotNumber", "orderable": true, "name": "PlotNumber" },
            { "data": "StreetName", "orderable": true, "name": "StreetName" },
            { "data": "TownName", "orderable": true, "name": "TownName" },
            { "data": "LGAName", "orderable": true, "name": "LGAName" },
            { "data": "WardName", "orderable": true, "name": "WardName" },
            { "data": "LandSize_Length", "orderable": true, "name": "LandSize_Length" },
            { "data": "LandSize_Width", "orderable": true, "name": "LandSize_Width" },
            { "data": "C_OF_O_Ref", "orderable": true, "name": "C_OF_O_Ref" },
            { "data": "LandPurposeName", "orderable": true, "name": "LandPurposeName" },
            { "data": "LandFunctionName", "orderable": true, "name": "LandFunctionName" },
            { "data": "LandOwnershipName", "orderable": true, "name": "LandOwnershipName" },
            { "data": "LandDevelopmentName", "orderable": true, "name": "LandDevelopmentName" },
            { "data": "Latitude", "orderable": true, "name": "Latitude" },
            { "data": "Longitude", "orderable": true, "name": "Longitude" },
            { "data": "LandStreetConditionName", "orderable": true, "name": "LandStreetConditionName" },
            {
                "data": "ValueOfLand", "orderable": true, "name": "ValueOfLand", "render": function (data, type, lan) {
                    return lan.ValueOfLand.formatMoney();
                }
            },
            { "data": "Neighborhood", "orderable": true, "name": "Neighborhood" },
            { "data": "ActiveText", "orderable": true, "name": "ActiveText" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, lan) {
                    return '<div class="btn-group">' + '<button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="false">' +
                        'Actions  <span class="caret"></span></button><ul class="dropdown-menu" role="menu">'
                        + '<li><a href="/Land/Details/' + lan.LandID + '/' + lan.LandRIN.toSeoUrl() + '">View Details</a></li>'
                        + '<li><a href="/Land/Edit/' + lan.LandID + '/' + lan.LandRIN.toSeoUrl() + '">Edit Details</a></li>'
                        + '<li><a href="/Land/TaxPayerList/' + lan.LandID + '/' + lan.LandRIN.toSeoUrl() + '">Tax Payer Information</a></li>'
                        + '<li><a onclick="javascript:jsfn_ChangeStatus(' + lan.LandID + ')">' + (lan.Active == 0 ? "Mark Active" : "Mark Inactive") + '</a></li>'
                        + '</ul></div>';
                }
            },
        ],
        "order": [[1, "asc"]]
    });


    $("#tbLand thead th input[type=text]").on('change', function () {
        vLandDataTable
            .column($(this.data).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });

    //Hide Columns
    vLandDataTable.column(4).visible(false);
    vLandDataTable.column(5).visible(false);
    vLandDataTable.column(6).visible(false);
    vLandDataTable.column(7).visible(false);
    vLandDataTable.column(8).visible(false);
    vLandDataTable.column(9).visible(false);
    vLandDataTable.column(10).visible(false);
    vLandDataTable.column(11).visible(false);
}

