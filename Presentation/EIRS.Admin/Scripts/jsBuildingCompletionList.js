var vBuildingCompletionDataTable;

jQuery(document).ready(function () {
    vBuildingCompletionDataTable = jsfn_ConvertToDatatable($('#tbBuildingCompletion'));
});

function jsfn_ChangeStatus(BuildingCompletionid) {
    var vData = {
        BuildingCompletionID: BuildingCompletionid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/BuildingCompletion/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vBuildingCompletionDataTable.destroy();
        $('#tbody').html(data.BuildingCompletionList);
        vBuildingCompletionDataTable = jsfn_ConvertToDatatable($('#tbBuildingCompletion'));
        $("#tbBuildingCompletion thead th input[type=text]").each(function () {
            vBuildingCompletionDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

