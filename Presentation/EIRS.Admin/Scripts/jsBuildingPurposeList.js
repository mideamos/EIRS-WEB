var vBuildingPurposeDataTable;

jQuery(document).ready(function () {
    vBuildingPurposeDataTable = jsfn_ConvertToDatatable($('#tbBuildingPurpose'));
});

function jsfn_ChangeStatus(BuildingPurposeid) {
    var vData = {
        BuildingPurposeID: BuildingPurposeid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/BuildingPurpose/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vBuildingPurposeDataTable.destroy();
        $('#tbody').html(data.BuildingPurposeList);
        vBuildingPurposeDataTable = jsfn_ConvertToDatatable($('#tbBuildingPurpose'));
        $("#tbBuildingPurpose thead th input[type=text]").each(function () {
            vBuildingPurposeDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

