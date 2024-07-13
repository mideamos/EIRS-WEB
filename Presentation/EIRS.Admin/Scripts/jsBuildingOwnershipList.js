var vBuildingOwnershipDataTable;

jQuery(document).ready(function () {
    vBuildingOwnershipDataTable = jsfn_ConvertToDatatable($('#tbBuildingOwnership'));
});

function jsfn_ChangeStatus(BuildingOwnershipid) {
    var vData = {
        BuildingOwnershipID: BuildingOwnershipid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/BuildingOwnership/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vBuildingOwnershipDataTable.destroy();
        $('#tbody').html(data.BuildingOwnershipList);
        vBuildingOwnershipDataTable = jsfn_ConvertToDatatable($('#tbBuildingOwnership'));
        $("#tbBuildingOwnership thead th input[type=text]").each(function () {
            vBuildingOwnershipDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

