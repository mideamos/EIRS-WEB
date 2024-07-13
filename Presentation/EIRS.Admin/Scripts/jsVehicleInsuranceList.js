var vVehicleInsuranceDataTable;

jQuery(document).ready(function () {
    vVehicleInsuranceDataTable = jsfn_ConvertToDatatable($('#tbVehicleInsurance'));
});

function jsfn_ChangeStatus(VehicleInsuranceid) {
    var vData = {
        VehicleInsuranceID: VehicleInsuranceid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/VehicleInsurance/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vVehicleInsuranceDataTable.destroy();
        $('#tbody').html(data.VehicleInsuranceList);
        vVehicleInsuranceDataTable = jsfn_ConvertToDatatable($('#tbVehicleInsurance'));
        $("#tbVehicleInsurance thead th input[type=text]").each(function () {
            vVehicleInsuranceDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

