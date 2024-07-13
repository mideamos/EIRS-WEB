var vNotificationTypeDataTable;

jQuery(document).ready(function () {
    vNotificationTypeDataTable = jsfn_ConvertToDatatable($('#tbNotificationType'));
});

function jsfn_ChangeStatus(NotificationTypeid) {
    var vData = {
        NotificationTypeID: NotificationTypeid,
        intStatus: 2
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/NotificationType/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vNotificationTypeDataTable.destroy();
        $('#tbody').html(data.NotificationTypeList);
        vNotificationTypeDataTable = jsfn_ConvertToDatatable($('#tbNotificationType'));
        $("#tbNotificationType thead th input[type=text]").each(function () {
            vNotificationTypeDataTable
                .column($(this).parent().index() + ':visible')
                .search(this.value)
                .draw();
        });
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

