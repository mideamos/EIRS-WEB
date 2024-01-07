var vLateChargeDataTable;

jQuery(document).ready(function () {
    vLateChargeDataTable = jsfn_ConvertToDatatable($('#tbLateCharge'));
});

function jsfn_ChangeStatus(LateChargeid) {
    var vData = {
        LateChargeID: LateChargeid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/LateCharge/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vLateChargeDataTable.destroy();
        $('#tbody').html(data.LateChargeList);
        vLateChargeDataTable = jsfn_ConvertToDatatable($('#tbLateCharge'));
        $("#tbLateCharge thead th input[type=text]").each(function () {
            vLateChargeDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

