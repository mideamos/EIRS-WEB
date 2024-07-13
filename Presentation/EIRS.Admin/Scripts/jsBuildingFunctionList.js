var vBuildingFunctionDataTable;

jQuery(document).ready(function () {
    vBuildingFunctionDataTable = jsfn_ConvertToDatatable($('#tbBuildingFunction'));
});

function jsfn_ChangeStatus(BuildingFunctionid) {
    var vData = {
        BuildingFunctionID: BuildingFunctionid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/BuildingFunction/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vBuildingFunctionDataTable.destroy();
        $('#tbody').html(data.BuildingFunctionList);
        vBuildingFunctionDataTable = jsfn_ConvertToDatatable($('#tbBuildingFunction'));
        $("#tbBuildingFunction thead th input[type=text]").each(function () {
            vBuildingFunctionDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

