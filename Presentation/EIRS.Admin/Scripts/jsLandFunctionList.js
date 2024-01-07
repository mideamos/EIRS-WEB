var vLandFunctionDataTable;

jQuery(document).ready(function () {
    vLandFunctionDataTable = jsfn_ConvertToDatatable($('#tbLandFunction'));
});

function jsfn_ChangeStatus(LandFunctionid) {
    var vData = {
        LandFunctionID: LandFunctionid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/LandFunction/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vLandFunctionDataTable.destroy();
        $('#tbody').html(data.LandFunctionList);
        vLandFunctionDataTable = jsfn_ConvertToDatatable($('#tbLandFunction'));
        $("#tbLandFunction thead th input[type=text]").each(function () {
            vLandFunctionDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

