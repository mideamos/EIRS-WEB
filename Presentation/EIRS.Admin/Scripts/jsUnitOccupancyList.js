var vUnitOccupancyDataTable;

jQuery(document).ready(function () {
    vUnitOccupancyDataTable = jsfn_ConvertToDatatable($('#tbUnitOccupancy'));
});

function jsfn_ChangeStatus(UnitOccupancyid) {
    var vData = {
        UnitOccupancyID: UnitOccupancyid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/UnitOccupancy/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vUnitOccupancyDataTable.destroy();
        $('#tbody').html(data.UnitOccupancyList);
        vUnitOccupancyDataTable = jsfn_ConvertToDatatable($('#tbUnitOccupancy'));
        $("#tbUnitOccupancy thead th input[type=text]").each(function () {
            vUnitOccupancyDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

