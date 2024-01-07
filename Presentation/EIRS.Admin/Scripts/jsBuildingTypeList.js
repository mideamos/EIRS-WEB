var vBuildingTypeDataTable;

jQuery(document).ready(function () {
    vBuildingTypeDataTable = jsfn_ConvertToDatatable($('#tbBuildingType'));
});

function jsfn_ChangeStatus(Buildingtypeid) {
    var vData = {
        BuildingTypeID: Buildingtypeid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/BuildingType/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vBuildingTypeDataTable.destroy();
        $('#tbody').html(data.BuildingTypeList);
        vBuildingTypeDataTable = jsfn_ConvertToDatatable($('#tbBuildingType'));
        $("#tbBuildingType thead th input[type=text]").each(function () {
            vBuildingTypeDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

