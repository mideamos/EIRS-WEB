var vLandDevelopmentDataTable;

jQuery(document).ready(function () {
    vLandDevelopmentDataTable = jsfn_ConvertToDatatable($('#tbLandDevelopment'));
});

function jsfn_ChangeStatus(LandDevelopmentid) {
    var vData = {
        LandDevelopmentID: LandDevelopmentid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/LandDevelopment/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vLandDevelopmentDataTable.destroy();
        $('#tbody').html(data.LandDevelopmentList);
        vLandDevelopmentDataTable = jsfn_ConvertToDatatable($('#tbLandDevelopment'));
        $("#tbLandDevelopment thead th input[type=text]").each(function () {
            vLandDevelopmentDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

