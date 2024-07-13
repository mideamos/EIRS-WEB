var vBuildingDataTable;

jQuery(document).ready(function () {
    vBuildingDataTable = jsfn_ConvertToDatatable($('#tbBuilding'));

    $('#dvBuildingDetails').dialog({
        autoOpen: false,
        width: 800,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Building Details</h4></div>",
        buttons: [{
            html: "<i class='fa fa-times'></i>&nbsp; Close",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    $('#dvRemoveDialog').removeClass('hide');
    $('#dvRemoveDialog').dialog({
        autoOpen: false,
        width: 600,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Remove Record Confirmation</h4></div>",
        buttons: [{
            html: "<i class='fa fa-check'></i>&nbsp; Yes",
            "class": "btn btn-primary",
            click: function () {
                var vData = {
                    BBID: $("#hdnTBID").val(),
                    BusinessID: $("#hdnOtherID").val()
                };

                jsfn_ShowLoading();
                jsfn_ajaxPost('/Business/RemoveBuilding', vData, jsfn_RemoveLinkResponse);
            }
        }, {
            html: "<i class='fa fa-times'></i>&nbsp; No",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

});

function jsfn_ShowDetails(bbid) {

    var vData = {
        BBID: bbid,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Business/GetBuildingInformation', vData, jsfn_BuildingResponse);
}

function jsfn_BuildingResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvBuildingDetails').dialog('open');
        $('#dvBuildingRIN').html(data.BuildingDetails.BuildingRIN);
        $('#dvBuildingTagNumber').html(data.BuildingDetails.BuildingTagNumber);
        $('#dvBuildingName').html(data.BuildingDetails.BuildingName == null ? "-" : data.BuildingDetails.BuildingName);
        $('#dvBuildingNumber').html(data.BuildingDetails.BuildingNumber);
        $('#dvBuildStreetName').html(data.BuildingDetails.StreetName);
        $('#dvBuildOffStreetName').html(data.BuildingDetails.OffStreetName == null ? "-" : data.BuildingDetails.OffStreetName);
        $('#dvBuildTown').html(data.BuildingDetails.TownName);
        $('#dvBuildLGA').html(data.BuildingDetails.LGAName);
        $('#dvBuildWard').html(data.BuildingDetails.WardName);
        $('#dvBuildingType').html(data.BuildingDetails.BuildingTypeName);
        $('#dvBuildingCompletion').html(data.BuildingDetails.BuildingCompletionName);
        $('#dvBuildingPurpose').html(data.BuildingDetails.BuildingPurposeName);
        $('#dvBuildingFunction').html(data.BuildingDetails.BuildingFunctionNames);
        $('#dvBuildingOwnership').html(data.BuildingDetails.BuildingOwnershipName == null ? "-" : data.BuildingDetails.BuildingOwnershipName);
        $('#dvBuildingOccupancy').html(data.BuildingDetails.BuildingOccupancyName);
        $('#dvBuildingOccupancyType').html(data.BuildingDetails.BuildingOccupancyTypeName);
        $('#dvBuildLatitude').html(data.BuildingDetails.Latitude == null ? "-" : data.BuildingDetails.Latitude);
        $('#dvBuildLongitude').html(data.BuildingDetails.Longitude == null ? "-" : data.BuildingDetails.Longitude);
        $('#dvBuildStatus').html(data.BuildingDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_RemoveLink(bbid,busid) {
    $('#dvRemoveDialog').dialog('open');
    $("#hdnTBID").val(bbid);
    $("#hdnOtherID").val(busid);
}

function jsfn_RemoveLinkResponse(data) {
    jsfn_HideLoading();
    $('#dvRemoveDialog').dialog('close');
    if (data.success) {

        $("#hdnTBID").val(0);
        $("#hdnOtherID").val(0);

        jsfn_ShowAlert(data.Message, 'success');
        vBuildingDataTable.destroy();
        $('#tbody').html(data.BuildingList);
        vBuildingDataTable = jsfn_ConvertToDatatable($('#tbBuilding'));
        $("#tbBuilding thead th input[type=text]").each(function () {
            vBuildingDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}
