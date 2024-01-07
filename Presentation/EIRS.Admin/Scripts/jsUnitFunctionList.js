var vUnitFunctionDataTable;

jQuery(document).ready(function () {
    vUnitFunctionDataTable = jsfn_ConvertToDatatable($('#tbUnitFunction'));
});

function jsfn_ChangeStatus(UnitFunctionid) {
    var vData = {
        UnitFunctionID: UnitFunctionid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/UnitFunction/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vUnitFunctionDataTable.destroy();
        $('#tbody').html(data.UnitFunctionList);
        vUnitFunctionDataTable = jsfn_ConvertToDatatable($('#tbUnitFunction'));
        $("#tbUnitFunction thead th input[type=text]").each(function () {
            vUnitFunctionDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

