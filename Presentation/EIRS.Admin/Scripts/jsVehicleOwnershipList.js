var vVehicleOwnershipDataTable;

jQuery(document).ready(function () {
    vVehicleOwnershipDataTable = jsfn_ConvertToDatatable($('#tbVehicleOwnership'));
});

function jsfn_ChangeStatus(VehicleOwnershipid) {
    var vData = {
        VehicleOwnershipID: VehicleOwnershipid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/VehicleOwnership/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vVehicleOwnershipDataTable.destroy();
        $('#tbody').html(data.VehicleOwnershipList);
        vVehicleOwnershipDataTable = jsfn_ConvertToDatatable($('#tbVehicleOwnership'));
        $("#tbVehicleOwnership thead th input[type=text]").each(function () {
            vVehicleOwnershipDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

