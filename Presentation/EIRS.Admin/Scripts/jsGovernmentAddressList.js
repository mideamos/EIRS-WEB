var vAddressDataTable;

jQuery(document).ready(function () {
    vAddressDataTable = jsfn_ConvertToDatatable($('#tbAddressInformation'));

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
        title: "<div class='widget-header'><h4>Remove Address Confirmation</h4></div>",
        buttons: [{
            html: "<i class='fa fa-check'></i>&nbsp; Yes",
            "class": "btn btn-primary",
            click: function () {
                var vData = {
                    GAIID: $("#hdnTBID").val(),
                    GovernmentID: $("#hdnOtherID").val()
                };

                jsfn_ShowLoading();
                jsfn_ajaxPost('/Government/RemoveAddressInformation', vData, jsfn_RemoveAddressResponse);
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

function jsfn_ShowDetails(bldid) {

    var vData = {
        BuildingID: bldid,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Government/GetAddressInformationDetails', vData, jsfn_AddressDetailResponse);
}

function jsfn_AddressDetailResponse(data) {

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

function jsfn_RemoveAddress(caiid, taxpayerid) {
    $('#dvRemoveDialog').dialog('open');
    $("#hdnTBID").val(caiid);
    $("#hdnOtherID").val(taxpayerid);
}

function jsfn_RemoveAddressResponse(data) {
    jsfn_HideLoading();
    $('#dvRemoveDialog').dialog('close');
    if (data.success) {
        $("#hdnTBID").val(0);
        $("#hdnOtherID").val(0);
        jsfn_ShowAlert(data.Message, 'success');
        vAddressDataTable.destroy();
        $('#tbody').html(data.AddressInformationList);
        vAddressDataTable = jsfn_ConvertToDatatable($('#tbAddressInformation'));
        $("#tbAddressInformation thead th input[type=text]").each(function () {
            vAddressDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

