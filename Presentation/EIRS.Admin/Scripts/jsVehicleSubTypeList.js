var vVehicleSubTypeDataTable;

jQuery(document).ready(function () {
    vVehicleSubTypeDataTable = jsfn_ConvertToDatatable($('#tbVehicleSubType'));
});

function jsfn_ChangeStatus(VehicleSubTypeid) {
    var vData = {
        VehicleSubTypeID: VehicleSubTypeid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/VehicleSubType/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vVehicleSubTypeDataTable.destroy();
        $('#tbody').html(data.VehicleSubTypeList);
        vVehicleSubTypeDataTable = jsfn_ConvertToDatatable($('#tbVehicleSubType'));
        $("#tbVehicleSubType thead th input[type=text]").each(function () {
            vVehicleSubTypeDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

