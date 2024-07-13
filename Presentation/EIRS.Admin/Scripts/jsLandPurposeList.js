var vLandPurposeDataTable;

jQuery(document).ready(function () {
    vLandPurposeDataTable = jsfn_ConvertToDatatable($('#tbLandPurpose'));
});

function jsfn_ChangeStatus(LandPurposeid) {
    var vData = {
        LandPurposeID: LandPurposeid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/LandPurpose/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vLandPurposeDataTable.destroy();
        $('#tbody').html(data.LandPurposeList);
        vLandPurposeDataTable = jsfn_ConvertToDatatable($('#tbLandPurpose'));
        $("#tbLandPurpose thead th input[type=text]").each(function () {
            vLandPurposeDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

