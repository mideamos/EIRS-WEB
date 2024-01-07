$(document).ready(function () {
    $('#dvSearchData').hide();
    
    $("#btnSearch").click(function () {
            $('#dvSearchData').show();
            jsfn_bindTable();
            return false;
    });

});

var vReviewStatusSummary;
function jsfn_bindTable() {
    if (vReviewStatusSummary !== undefined) {
        vReviewStatusSummary.draw();
    }
    else {
        vReviewStatusSummary = $("#tblReviewStatusSummary").DataTable({
            "processing": true, // for show progress bar
            "serverSide": true, // for process server side
            "filter": true,
            "orderMulti": false, // for disable multiple column at once
            "ajax": {
                "url": "/Staff/ReviewStatusSummaryLoadData",
                "type": "POST",
                "datatype": "json",
                "data": function (data) {
                    data.ReviewStatusID = $("#cboReviewStatus").val() !== '' ? $("#cboReviewStatus").val() : '0';
                }, "error": function (jqXHR, exception) {
                    if (jqXHR.status == 401) {
                        window.location.href = '/Login/Individual';
                    }
                }
            },
            "columns": [
                { "data": "ReviewStatusName", "orderable": true, "name": "ReviewStatusName", "width": "25%" },
                { "data": "TotalTaxPayerCount", "orderable": true, "name": "TotalTaxPayerCount", "width": "25%" },
                {
                    "data": "TotalAssessmentAmount", "orderable": true, "name": "TotalAssessmentAmount", "width": "25%", "render": function (data, type, st) {
                        return st.TotalAssessmentAmount.formatMoney();
                    }
                },
                {
                    "data": "TotalPaymentAmount", "orderable": true, "name": "TotalPaymentAmount", "width": "25%", "render": function (data, type, st) {
                        return st.TotalPaymentAmount.formatMoney();
                    }
                },
                {
                    "data": "OutstandingAmount", "orderable": true, "name": "OutstandingAmount", "width": "25%", "render": function (data, type, st) {
                        return st.OutstandingAmount.formatMoney();
                    }
                },
                {
                    "data": "", "orderable": false, "name": "Action", "render": function (data, type, st) {
                        return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                            + '</button><ul class="dropdown-menu"><li><a href="/Staff/ReviewStatusTaxPayer?rsid=' + st.ReviewStatusID + '">'
                            + 'Tax Payer List</a></li></ul></div>';
                    }, "width": "10%"
                }
            ],
            "order": [[1, "asc"]]
        });
    }
}