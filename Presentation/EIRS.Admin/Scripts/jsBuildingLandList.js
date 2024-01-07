var vLandDataTable;

jQuery(document).ready(function () {
    vLandDataTable = jsfn_ConvertToDatatable($('#tbLand'));

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
                    BLID: $("#hdnTBID").val(),
                    BuildingID: $("#hdnOtherID").val()
                };

                jsfn_ShowLoading();
                jsfn_ajaxPost('/Building/RemoveLand', vData, jsfn_RemoveLinkResponse);
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

function jsfn_ShowDetails(blid) {

    var vData = {
        BLID: blid,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Building/GetLandInformation', vData, jsfn_LandResponse);
}

function jsfn_LandResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvLandDetails').dialog('open');
        $('#dvLandRIN').html(data.LandDetails.LandRIN);
        $('#dvLandStreetName').html(data.LandDetails.StreetName);
        $('#dvLandTown').html(data.LandDetails.TownName);
        $('#dvLandLGA').html(data.LandDetails.LGAName);
        $('#dvLandWard').html(data.LandDetails.WardName);
        $('#dvLandSize_Length').html(data.LandDetails.LandSize_Length == null ? "-" : data.LandDetails.LandSize_Length);
        $('#dvLandSize_Width').html(data.LandDetails.LandSize_Width == null ? "-" : data.LandDetails.LandSize_Width);
        $('#dvC_OF_O_Ref').html(data.LandDetails.C_OF_O_Ref == null ? "-" : data.LandDetails.C_OF_O_Ref);
        $('#dvLandPurpose').html(data.LandDetails.LandPurposeName);
        $('#dvLandOwnership').html(data.LandDetails.LandOwnershipName == null ? "-" : data.LandDetails.LandOwnershipName);
        $('#dvLandLatitude').html(data.LandDetails.Latitude == null ? "-" : data.LandDetails.Latitude);
        $('#dvLandLongitude').html(data.LandDetails.Longitude == null ? "-" : data.LandDetails.Longitude);
        $('#dvLandStatus').html(data.LandDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_RemoveLink(bbid, bldid) {
    $('#dvRemoveDialog').dialog('open');
    $("#hdnTBID").val(bbid);
    $("#hdnOtherID").val(bldid);
}

function jsfn_RemoveLinkResponse(data) {
    jsfn_HideLoading();
    $('#dvRemoveDialog').dialog('close');
    if (data.success) {

        $("#hdnTBID").val(0);
        $("#hdnOtherID").val(0);

        jsfn_ShowAlert(data.Message, 'success');
        vLandDataTable.destroy();
        $('#tbody').html(data.LandList);
        vLandDataTable = jsfn_ConvertToDatatable($('#tbLand'));
        $("#tbLand thead th input[type=text]").each(function () {
            vLandDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}
