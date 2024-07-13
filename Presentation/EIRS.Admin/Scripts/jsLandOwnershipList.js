var vLandOwnershipDataTable;

jQuery(document).ready(function () {
    vLandOwnershipDataTable = jsfn_ConvertToDatatable($('#tbLandOwnership'));
});

function jsfn_ChangeStatus(LandOwnershipid) {
    var vData = {
        LandOwnershipID: LandOwnershipid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/LandOwnership/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vLandOwnershipDataTable.destroy();
        $('#tbody').html(data.LandOwnershipList);
        vLandOwnershipDataTable = jsfn_ConvertToDatatable($('#tbLandOwnership'));
        $("#tbLandOwnership thead th input[type=text]").each(function () {
            vLandOwnershipDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

