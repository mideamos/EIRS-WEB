var vVehicleFunctionDataTable;

jQuery(document).ready(function () {
    vVehicleFunctionDataTable = jsfn_ConvertToDatatable($('#tbVehicleFunction'));
});

function jsfn_ChangeStatus(VehicleFunctionid) {
    var vData = {
        VehicleFunctionID: VehicleFunctionid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/VehicleFunction/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vVehicleFunctionDataTable.destroy();
        $('#tbody').html(data.VehicleFunctionList);
        vVehicleFunctionDataTable = jsfn_ConvertToDatatable($('#tbVehicleFunction'));
        $("#tbVehicleFunction thead th input[type=text]").each(function () {
            vVehicleFunctionDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

