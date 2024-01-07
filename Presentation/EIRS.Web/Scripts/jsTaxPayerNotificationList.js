jQuery(document).ready(function () {
    jsfn_bindTable();
});

function jsfn_bindTable() {
    var vColumnsList = [{
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": false
    }];

    var vSortOrder = [];

    jsfn_ConvertToDatableWithCustomSort($('#tblNotifications'), vColumnsList, 'No Notification Found', vSortOrder);
}

function jsfn_NotificationInformation(NotID) {
    var vData = {
        NotificationID: NotID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetNotificationDetails', vData, jsfn_NotificationDetailResponse);
}

function jsfn_NotificationDetailResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#dvNotificationModal').modal('show');
        $('#dvNotificationContent').html(data.NotificationContent);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}