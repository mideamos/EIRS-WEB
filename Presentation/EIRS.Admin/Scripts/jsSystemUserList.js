var vSystemUserDataTable;

jQuery(document).ready(function () {
    vSystemUserDataTable = jsfn_ConvertToDatatable($('#tbSystemUser'));
});

function jsfn_ChangeStatus(assettypeid) {
    var vData = {
        SystemUserID: assettypeid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/SystemUser/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vSystemUserDataTable.destroy();
        $('#tbody').html(data.SystemUserList);
        vSystemUserDataTable = jsfn_ConvertToDatatable($('#tbSystemUser'));
        $("#tbSystemUser thead th input[type=text]").each(function () {
            vSystemUserDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

