var vBuildingOccupancyDataTable;

jQuery(document).ready(function () {
    vBuildingOccupancyDataTable = jsfn_ConvertToDatatable($('#tbBuildingOccupancy'));
});

function jsfn_ChangeStatus(BuildingOccupancyid) {
    var vData = {
        BuildingOccupancyID: BuildingOccupancyid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/BuildingOccupancy/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vBuildingOccupancyDataTable.destroy();
        $('#tbody').html(data.BuildingOccupancyList);
        vBuildingOccupancyDataTable = jsfn_ConvertToDatatable($('#tbBuildingOccupancy'));
        $("#tbBuildingOccupancy thead th input[type=text]").each(function () {
            vBuildingOccupancyDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

