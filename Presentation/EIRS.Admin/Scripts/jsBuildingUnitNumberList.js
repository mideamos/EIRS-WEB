var vUnitDataTable;

jQuery(document).ready(function () {
    vUnitDataTable = jsfn_ConvertToDatatable($('#tbUnit'));

    $('#dvBuildingUnitDetails').dialog({
        autoOpen: false,
        width: 800,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Building Unit Details</h4></div>",
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
                    BBUID: $("#hdnTBID").val(),
                    BuildingID: $("#hdnOtherID").val()
                };

                jsfn_ShowLoading();
                jsfn_ajaxPost('/Building/RemoveBuildingUnit', vData, jsfn_RemoveLinkResponse);
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

function jsfn_ShowDetails(bbuid) {

    var vData = {
        BBUID: bbuid,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Building/GetUnitInformation', vData, jsfn_UnitResponse);
}

function jsfn_UnitResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvBuildingUnitDetails').dialog('open');
        $('#dvUnitNumber').html(data.UnitDetails.UnitNumber);
        $('#dvUnitPurpose').html(data.UnitDetails.UnitPurposeName);
        $('#dvUnitFunction').html(data.UnitDetails.UnitFunctionName);
        $('#dvUnitOccupancy').html(data.UnitDetails.UnitOccupancyName);
        $('#dvUnitSize').html(data.UnitDetails.UnitSize);
        $('#dvUnitStatus').html(data.UnitDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_RemoveLink(bbuid, bldid) {
    $('#dvRemoveDialog').dialog('open');
    $("#hdnTBID").val(bbuid);
    $("#hdnOtherID").val(bldid);
}

function jsfn_RemoveLinkResponse(data) {
    jsfn_HideLoading();
    $('#dvRemoveDialog').dialog('close');
    if (data.success) {

        $("#hdnTBID").val(0);
        $("#hdnOtherID").val(0);

        jsfn_ShowAlert(data.Message, 'success');
        vUnitDataTable.destroy();
        $('#tbody').html(data.BuildingUnitList);
        vUnitDataTable = jsfn_ConvertToDatatable($('#tbUnit'));
        $("#tbUnit thead th input[type=text]").each(function () {
            vUnitDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}
