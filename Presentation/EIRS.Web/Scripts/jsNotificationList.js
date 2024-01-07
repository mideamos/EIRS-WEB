$(document).ready(function () {
    $('#tblNotifications').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/Notify/GetData",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [{
            "data": "NotificationRefNo"
        }, {
            "data": "NotificationDate"
        }, {
            "data": "NotificationMethodName"
        }, {
            "data": "NotificationTypeName"
        }, {
            "data": "TaxPayerRIN"
        }, {
            "data": "NotificationID"
        }],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            var vNotificationDate = new Date(parseInt(aData["NotificationDate"].substr(6)));
            $('td:eq(1)', nRow).html(vNotificationDate.format("dd-mmm-yyyy"));
            $('td:eq(5)', nRow).html('<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action <span class="caret"></span>'
                + '</button><ul class="dropdown-menu"><li><a onclick="javascript:jsfn_NotificationInformation(' + aData["NotificationID"] + ')">'
                + 'View Notification</a></li></ul></div>');

        },
    });

});

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