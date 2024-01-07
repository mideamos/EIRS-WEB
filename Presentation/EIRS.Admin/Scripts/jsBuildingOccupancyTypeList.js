var vBuildingOccupancyTypeDataTable;

jQuery(document).ready(function() {
    vBuildingOccupancyTypeDataTable = jsfn_ConvertToDatatable($('#tbBuildingOccupancyType'));
});

function jsfn_ChangeStatus(botypeid) {
    var vData = {
        BuildingOccupancyTypeID: botypeid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/BuildingOccupancyType/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vBuildingOccupancyTypeDataTable.destroy();
        $('#tbody').html(data.BuildingOccupancyTypeList);
        vBuildingOccupancyTypeDataTable = jsfn_ConvertToDatatable($('#tbBuildingOccupancyType'));
        $("#tbBuildingOccupancyType thead th input[type=text]").each(function() {
            vBuildingOccupancyTypeDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

