var vVehicleTypeDataTable;

jQuery(document).ready(function () {
    vVehicleTypeDataTable = jsfn_ConvertToDatatable($('#tbVehicleType'));
});

function jsfn_ChangeStatus(VehicleTypeid) {
    var vData = {
        VehicleTypeID: VehicleTypeid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/VehicleType/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vVehicleTypeDataTable.destroy();
        $('#tbody').html(data.VehicleTypeList);
        vVehicleTypeDataTable = jsfn_ConvertToDatatable($('#tbVehicleType'));
        $("#tbVehicleType thead th input[type=text]").each(function () {
            vVehicleTypeDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

