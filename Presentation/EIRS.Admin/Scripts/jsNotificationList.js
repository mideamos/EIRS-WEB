

jQuery(document).ready(function () {
    jsfn_bindTable();

});

var vNotificationDataTable;
function jsfn_bindTable() {
    vNotificationDataTable = $("#tbNotification").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url":"/Notification/LoadData",
            "type": "POST",
            "datatype": "json",

            "data": function (data) {
                data.NotificationRefNo = $("#txtNotificationRefNo").val();
                data.NotificationMethodName = $("#txtNotificationMethodName").val();
                data.NotificationTypeName = $("#txtNotificationTypeName").val();
                data.EventRefNo = $("#txtEventRefNo").val();
                data.TaxPayerTypeName = $("#txtTaxPayerTypeName").val();
                data.TaxPayerName = $("#txtTaxPayerName").val();
                data.NotificationDate = $("#txtNotificationDate").val();
                //data.NotificationStatus = $("#txtNotificationStatus").val();
                data.StatusName = $("#txtStatus").val();
            },
            
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",
        "columns": [

            { "data": "NotificationRefNo", "orderable": true, "name": "NotificationRefNo" },
            {
                "data": "NotificationDate", "orderable": true, "name": "NotificationDate", "width": "20%", "render": function (data, type, Notification) {
                    if (Notification.NotificationDate != undefined || Notification.NotificationDate != null) {
                        return jsfn_FormatJsonDate(Notification.NotificationDate, "dd-mmm-yyyy");
                    }
                    else {
                        return '';
                    }
                }
            },
            { "data": "NotificationMethodName", "orderable": true, "name": "NotificationMethodName" },
            { "data": "NotificationTypeName", "orderable": true, "name": "NotificationTypeName" },
            { "data": "EventRefNo", "orderable": true, "name": "EventRefNo" },
            { "data": "TaxPayerTypeName", "orderable": true, "name": "TaxPayerTypeName" },
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName" },
            //{ "data": "NotificationStatus", "orderable": true, "name": "NotificationStatus" },
            { "data": "NotificationStatusText", "orderable": true, "name": "NotificationStatusText" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, Notification) {
                    return '<div class="btn-group">' + '<button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="false">' +
                        "Actions  <span class='caret'></span></button><ul class='dropdown-menu' role='menu'>"
                        + '<li><a href="/Notification/Details/' + Notification.NotificationID + '">View Details</a></li>'
                       + '</ul></div>';
                }, "width": "10%"
            },
        ],
        "order": [[0, "asc"]]
    });


    $("#tbNotification thead th input[type=text]").on('change', function () {
        vNotificationDataTable
            .column($(this.data).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });


   
    //vNotificationDataTable.column(2).visible(false);
    //vNotificationDataTable.column(3).visible(false);
    //vNotificationDataTable.column(5).visible(false);
}


