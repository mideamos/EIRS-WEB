var vNotificationMethodDataTable;

jQuery(document).ready(function () {
    vNotificationMethodDataTable = jsfn_ConvertToDatatable($('#tbNotificationMethod'));
});

function jsfn_ChangeStatus(NotificationMethodid) {
    var vData = {
        NotificationMethodID: NotificationMethodid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/NotificationMethod/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vNotificationMethodDataTable.destroy();
        $('#tbody').html(data.NotificationMethodList);
        vNotificationMethodDataTable = jsfn_ConvertToDatatable($('#tbNotificationMethod'));
        $("#tbNotificationMethod thead th input[type=text]").each(function () {
            vNotificationMethodDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

