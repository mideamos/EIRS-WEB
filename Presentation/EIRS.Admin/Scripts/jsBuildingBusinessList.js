var vBusinessDataTable;

jQuery(document).ready(function () {
    vBusinessDataTable = jsfn_ConvertToDatatable($('#tbBusiness'));

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
                    BuildingID: $("#hdnOtherID").val()
                };

                jsfn_ShowLoading();
                jsfn_ajaxPost('/Building/RemoveBusiness', vData, jsfn_RemoveLinkResponse);
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
    jsfn_ajaxPost('/Building/GetBusinessInformation', vData, jsfn_BuisnessResponse);
}

function jsfn_BuisnessResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvBusinessDetails').dialog('open');
        $('#dvBusinessRIN').html(data.BusinessDetails.BusinessRIN);
        $('#dvBusinessName').html(data.BusinessDetails.BusinessName);
        $('#dvBusinessType').html(data.BusinessDetails.BusinessTypeName);
        $('#dvBusinessLGA').html(data.BusinessDetails.LGAName);
        $('#dvBusinessCategory').html(data.BusinessDetails.BusinessCategoryName);
        $('#dvBusinessSector').html(data.BusinessDetails.BusinessSectorName);
        $('#dvBusinessSubSector').html(data.BusinessDetails.BusinessSubSectorName);
        $('#dvBusinessStructure').html(data.BusinessDetails.BusinessStructureName);
        $('#dvBusinessOperations').html(data.BusinessDetails.BusinessOperationName);
        $('#dvBusinessStatus').html(data.BusinessDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_RemoveLink(bbid,bldid) {
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
        vBusinessDataTable.destroy();
        $('#tbody').html(data.BusinessList);
        vBusinessDataTable = jsfn_ConvertToDatatable($('#tbBusiness'));
        $("#tbBusiness thead th input[type=text]").each(function () {
            vBusinessDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}
