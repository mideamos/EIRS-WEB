var vVehicleLicenseDataTable;

jQuery(document).ready(function () {
    vVehicleLicenseDataTable = jsfn_ConvertToDatatable($('#tbVehicleLicense'));
});

function jsfn_ChangeStatus(VehicleLicenseid) {
    var vData = {
        VehicleLicenseID: VehicleLicenseid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/VehicleLicense/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vVehicleLicenseDataTable.destroy();
        $('#tbody').html(data.VehicleLicenseList);
        vVehicleLicenseDataTable = jsfn_ConvertToDatatable($('#tbVehicleLicense'));
        $("#tbVehicleLicense thead th input[type=text]").each(function () {
            vVehicleLicenseDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

