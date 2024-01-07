var vTaxPayerAssetDataTable;

jQuery(document).ready(function () {
    vTaxPayerAssetDataTable = jsfn_ConvertToDatatable($('#tbTaxPayerAsset'));

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

    $('#dvVehicleDetails').dialog({
        autoOpen: false,
        width: 800,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Vehicle Details</h4></div>",
        buttons: [{
            html: "<i class='fa fa-times'></i>&nbsp; Close",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    $('#dvBusinessDetails').dialog({
        autoOpen: false,
        width: 800,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Business Details</h4></div>",
        buttons: [{
            html: "<i class='fa fa-times'></i>&nbsp; Close",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    $('#dvLandDetails').dialog({
        autoOpen: false,
        width: 800,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Land Details</h4></div>",
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
                    TPAID: $("#hdnTBID").val(),
                    TaxPayerID: $("#hdnOtherID").val()
                };

                jsfn_ShowLoading();
                jsfn_ajaxPost('/Government/RemoveAsset', vData, jsfn_RemoveLinkResponse);
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

function jsfn_ChangeStatus(tpaid, taxpayerid) {
    var vData = {
        TPAID: tpaid,
        TaxPayerID: taxpayerid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Government/UpdateAssetStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vTaxPayerAssetDataTable.destroy();
        $('#tbody').html(data.AssetList);
        vTaxPayerAssetDataTable = jsfn_ConvertToDatatable($('#tbTaxPayerAsset'));
        $("#tbTaxPayerAsset thead th input[type=text]").each(function () {
            vTaxPayerAssetDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_ShowDetails(tpaid) {

    var vData = {
        TPAID: tpaid,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Government/GetAssetDetails', vData, jsfn_AssetDetailResponse);
}

function jsfn_AssetDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        if (data.AssetTypeID == '1') {
            $('#dvBuildingDetails').dialog('open');
            $('#dvBuildingRIN').html(data.AssetDetails.BuildingRIN);
            $('#dvBuildingTagNumber').html(data.AssetDetails.BuildingTagNumber);
            $('#dvBuildingName').html(data.AssetDetails.BuildingName == null ? "-" : data.AssetDetails.BuildingName);
            $('#dvBuildingNumber').html(data.AssetDetails.BuildingNumber);
            $('#dvBuildStreetName').html(data.AssetDetails.StreetName);
            $('#dvBuildOffStreetName').html(data.AssetDetails.OffStreetName == null ? "-" : data.AssetDetails.OffStreetName);
            $('#dvBuildTown').html(data.AssetDetails.TownName);
            $('#dvBuildLGA').html(data.AssetDetails.LGAName);
            $('#dvBuildWard').html(data.AssetDetails.WardName);
            $('#dvBuildingType').html(data.AssetDetails.BuildingTypeName);
            $('#dvBuildingCompletion').html(data.AssetDetails.BuildingCompletionName);
            $('#dvBuildingPurpose').html(data.AssetDetails.BuildingPurposeName);
            $('#dvBuildingFunction').html(data.AssetDetails.BuildingFunctionNames);
            $('#dvBuildingOwnership').html(data.AssetDetails.BuildingOwnershipName == null ? "-" : data.AssetDetails.BuildingOwnershipName);
            $('#dvBuildingOccupancy').html(data.AssetDetails.BuildingOccupancyName);
            $('#dvBuildingOccupancyType').html(data.AssetDetails.BuildingOccupancyTypeName);
            $('#dvBuildLatitude').html(data.AssetDetails.Latitude == null ? "-" : data.AssetDetails.Latitude);
            $('#dvBuildLongitude').html(data.AssetDetails.Longitude == null ? "-" : data.AssetDetails.Longitude);
            $('#dvBuildStatus').html(data.AssetDetails.ActiveText);
        }
        else if (data.AssetTypeID == '2') {
            $('#dvVehicleDetails').dialog('open');
            $('#dvVehicleRIN').html(data.AssetDetails.VehicleRIN);
            $('#dvVehicleRegNumber').html(data.AssetDetails.VehicleRegNumber);
            $('#dvVIN').html(data.AssetDetails.VIN == null ? '-' : data.AssetDetails.VIN);
            $('#dvVehicleType').html(data.AssetDetails.VehicleTypeName);
            $('#dvVehicleSubType').html(data.AssetDetails.VehicleSubTypeName);
            $('#dvVLGA').html(data.AssetDetails.LGAName);
            $('#dvVehiclePurpose').html(data.AssetDetails.VehiclePurposeName);
            $('#dvVehicleFunction').html(data.AssetDetails.VehicleFunctionName);
            $('#dvVehicleOwnership').html(data.AssetDetails.VehicleOwnershipName == null ? "-" : data.AssetDetails.VehicleOwnershipName);
            $('#dvVStatus').html(data.AssetDetails.ActiveText);
        }
        else if (data.AssetTypeID == '3') {
            $('#dvBusinessDetails').dialog('open');
            $('#dvBusinessRIN').html(data.AssetDetails.BusinessRIN);
            $('#dvBusinessName').html(data.AssetDetails.BusinessName);
            $('#dvBusinessType').html(data.AssetDetails.BusinessTypeName);
            $('#dvBusinessLGA').html(data.AssetDetails.LGAName);
            $('#dvBusinessCategory').html(data.AssetDetails.BusinessCategoryName);
            $('#dvBusinessSector').html(data.AssetDetails.BusinessSectorName);
            $('#dvBusinessSubSector').html(data.AssetDetails.BusinessSubSectorName);
            $('#dvBusinessStructure').html(data.AssetDetails.BusinessStructureName);
            $('#dvBusinessOperations').html(data.AssetDetails.BusinessOperationName);
            $('#dvBusinessStatus').html(data.AssetDetails.ActiveText);
        }
        else if (data.AssetTypeID == '4') {
            $('#dvLandDetails').dialog('open');
            $('#dvLandRIN').html(data.AssetDetails.LandRIN);
            $('#dvLandStreetName').html(data.AssetDetails.StreetName);
            $('#dvLandTown').html(data.AssetDetails.TownName);
            $('#dvLandLGA').html(data.AssetDetails.LGAName);
            $('#dvLandWard').html(data.AssetDetails.WardName);
            $('#dvLandSize_Length').html(data.AssetDetails.LandSize_Length == null ? "-" : data.AssetDetails.LandSize_Length);
            $('#dvLandSize_Width').html(data.AssetDetails.LandSize_Width == null ? "-" : data.AssetDetails.LandSize_Width);
            $('#dvC_OF_O_Ref').html(data.AssetDetails.C_OF_O_Ref == null ? "-" : data.AssetDetails.C_OF_O_Ref);
            $('#dvLandPurpose').html(data.AssetDetails.LandPurposeName);
            $('#dvLandOwnership').html(data.AssetDetails.LandOwnershipName == null ? "-" : data.AssetDetails.LandOwnershipName);
            $('#dvLandLatitude').html(data.AssetDetails.Latitude == null ? "-" : data.AssetDetails.Latitude);
            $('#dvLandLongitude').html(data.AssetDetails.Longitude == null ? "-" : data.AssetDetails.Longitude);
            $('#dvLandStatus').html(data.AssetDetails.ActiveText);
        }
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_RemoveLink(tpaid, taxpayerid) {
    $('#dvRemoveDialog').dialog('open');
    $("#hdnTBID").val(tpaid);
    $("#hdnOtherID").val(taxpayerid);
}

function jsfn_RemoveLinkResponse(data) {
    jsfn_HideLoading();
    $('#dvRemoveDialog').dialog('close');
    if (data.success) {
        $("#hdnTBID").val(0);
        $("#hdnOtherID").val(0);
        jsfn_ShowAlert(data.Message, 'success');
        vTaxPayerAssetDataTable.destroy();
        $('#tbody').html(data.AssetList);
        vTaxPayerAssetDataTable = jsfn_ConvertToDatatable($('#tbTaxPayerAsset'));
        $("#tbTaxPayerAsset thead th input[type=text]").each(function () {
            vTaxPayerAssetDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}



