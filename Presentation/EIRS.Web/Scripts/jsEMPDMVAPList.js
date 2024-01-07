var vTable;
$(document).ready(function () {
    
    vTable = $("#tblData").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/EMPDMVAPending/LoadData",
            "type": "POST",
            "datatype": "json",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "PaymentRefNumber", "orderable": true, "name": "PaymentRefNumber", "width": "25%" },
            { "data": "PaymentDateTime", "orderable": true, "name": "PaymentDateTime", "width": "25%" },
            { "data": "CustomerName", "orderable": true, "name": "CustomerName", "width": "25%" },
            { "data": "RIN", "orderable": true, "name": "RIN", "width": "25%" },
            { "data": "Bank", "orderable": true, "name": "Bank", "width": "25%" },
            {
                "data": "Amount", "orderable": true, "name": "Amount", "width": "25%", "render": function (data, type, st) {
                    return st.Amount.formatMoney();
                }
            },
            {
                "data": "", "orderable": false, "name": "Action",
                "render": function (data, type, st) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span></button>'
                        + '<ul class="dropdown-menu">'
                        + '<li><a href="/EMPDMVAPending/Details?id=' + st["PDMVPID"] + '">View Details</a></li>'
                        + '</ul>';
                }
            }
        ],
        "order": [[1, "desc"]],
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        }
    });
});