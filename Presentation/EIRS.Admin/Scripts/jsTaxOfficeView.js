$(document).ready(function () {
   
    $("#dvTaxOfficeAddressList").hide();
  
    $("#aTaxOfficeAddress").click(function () {
        $("#dvTaxOfficeRecordLink").hide();
        $("#dvTaxOfficeAddressList").show();
    });

    $("#btnTOABack").click(function () {
        $("#dvTaxOfficeRecordLink").show();
        $("#dvTaxOfficeAddressList").hide();
    });

   
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

});

function jsfn_ShowBuildingDetails(BuildingID) {

    var vData = {
        BuildingID: BuildingID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/TaxOffice/GetBuildingDetails', vData, jsfn_ShowBuildingDetailResponse)
}

function jsfn_ShowBuildingDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvBuildingDetails').dialog('open');
        $('#dvBuildingRIN').html(data.BuildingDetails.BuildingRIN);
        $('#dvBuildingTagNumber').html(data.BuildingDetails.BuildingTagNumber);
        $('#dvBuildingName').html(data.BuildingDetails.BuildingName == null ? "-" : data.BuildingDetails.BuildingName);
        $('#dvBuildingNumber').html(data.BuildingDetails.BuildingNumber);
        $('#dvStreetName').html(data.BuildingDetails.StreetName);
        $('#dvOffStreetName').html(data.BuildingDetails.OffStreetName == null ? "-" : data.BuildingDetails.OffStreetName);
        $('#dvTown').html(data.BuildingDetails.TownName);
        $('#dvLGA').html(data.BuildingDetails.LGAName);
        $('#dvWard').html(data.BuildingDetails.WardName);
        $('#dvAssetType').html(data.BuildingDetails.AssetTypeName);
        $('#dvBuildingType').html(data.BuildingDetails.BuildingTypeName);
        $('#dvBuildingCompletion').html(data.BuildingDetails.BuildingCompletionName);
        $('#dvBuildingPurpose').html(data.BuildingDetails.BuildingPurposeName);
        $('#dvBuildingFunction').html(data.BuildingDetails.BuildingFunctionNames);
        $('#dvBuildingOwnership').html(data.BuildingDetails.BuildingOwnershipName == null ? "-" : data.BuildingDetails.BuildingOwnershipName);
        $('#dvBuildingOccupancy').html(data.BuildingDetails.BuildingOccupancyName);
        $('#dvBuildingOccupancyType').html(data.BuildingDetails.BuildingOccupancyTypeName);
        $('#dvLatitude').html(data.BuildingDetails.Latitude == null ? "-" : data.BuildingDetails.Latitude);
        $('#dvLongitude').html(data.BuildingDetails.Longitude == null ? "-" : data.BuildingDetails.Longitude);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}