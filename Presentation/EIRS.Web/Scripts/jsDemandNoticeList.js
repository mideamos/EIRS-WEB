$(document).ready(function () {
    $("#tblDemandNotice").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/Settle/DemandNoticeLoadData",
            "type": "POST",
            "datatype": "json",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "TaxPayerRIN", "orderable": true, "name": "TaxPayerRIN" },
            { "data": "TaxPayerTypeName", "orderable": true, "name": "TaxPayerTypeName" },
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName" },
            {
                "data": "TotalAssessed", "orderable": true, "name": "TotalAssessed", "render": function (data, type, st) {
                    return st.TotalAssessed.formatMoney();
                }
            },
            {
                "data": "TotalPenalty", "orderable": true, "name": "TotalPenalty", "render": function (data, type, st) {
                    return st.TotalPenalty.formatMoney();
                }
            },
            {
                "data": "TotalInterest", "orderable": true, "name": "TotalInterest", "render": function (data, type, st) {
                    return st.TotalInterest.formatMoney();
                }
            },
            {
                "data": "TotalCharge", "orderable": true, "name": "TotalCharge", "render": function (data, type, st) {
                    return st.TotalCharge.formatMoney();
                }
            }, {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, st) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                        + '</button><ul class="dropdown-menu">'
                        + '<li><a href="/Settle/DemandNoticeCharges?tptid=' + st.TaxPayerTypeID + '&tpid=' + st.TaxPayerID + '">View Details</a></li>'
                        + '<li><a href="/Settle/GenerateDemandNoticePDF?tptid=' + st.TaxPayerTypeID + '&tpid=' + st.TaxPayerID + '">Generate PDF</a></li>'
                        + '</ul></div>';
                }, "width": "10%"
            }
        ],
        "order": [[0, "desc"]]
    });
});
