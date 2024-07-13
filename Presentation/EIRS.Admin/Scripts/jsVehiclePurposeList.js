var vVehiclePurposeDataTable;

jQuery(document).ready(function () {
    vVehiclePurposeDataTable = jsfn_ConvertToDatatable($('#tbVehiclePurpose'));
});

function jsfn_ChangeStatus(VehiclePurposeid) {
    var vData = {
        VehiclePurposeID: VehiclePurposeid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/VehiclePurpose/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vVehiclePurposeDataTable.destroy();
        $('#tbody').html(data.VehiclePurposeList);
        vVehiclePurposeDataTable = jsfn_ConvertToDatatable($('#tbVehiclePurpose'));
        $("#tbVehiclePurpose thead th input[type=text]").each(function () {
            vVehiclePurposeDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

