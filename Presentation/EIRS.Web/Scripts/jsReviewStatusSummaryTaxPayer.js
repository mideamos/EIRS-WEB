$(document).ready(function () {
    $("#tblTaxPayerList").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/OperationManager/ReviewStatusTaxPayerLoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.TaxOfficeID = $("#hdnTaxOfficeID").val();
                data.TaxPayerTypeID = $("#hdnTaxPayerTypeID").val();
                data.ReviewStatusID = $("#hdnReviewStatusID").val();
            }, "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "TaxPayerRIN", "orderable": true, "name": "TaxPayerRIN", "width": "25%" },
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName", "width": "25%" },
            { "data": "TaxPayerTypeName", "orderable": true, "name": "TaxPayerTypeName", "width": "25%" },
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
            { "data": "ReviewStatusName", "orderable": true, "name": "ReviewStatusName", "width": "25%" },
            { "data": "TaxOfficerName", "orderable": true, "name": "TaxOfficerName", "width": "25%" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, st) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                        + '</button><ul class="dropdown-menu"><li><a href="' + jsfn_GetTaxPayerDetailUrl(st.TaxPayerID, st.TaxPayerTypeID, st.TaxPayerName) + '">'
                        + 'Tax Payer Details</a></li></ul></div>';
                }, "width": "10%"
            },
        ],
        "order": [[1, "asc"]]
    });
});

function jsfn_GetTaxPayerDetailUrl(TaxPayerID, TaxPayerTypeID, TaxPayerName) {
    if (TaxPayerTypeID == TaxPayerType.Individual) {
        return '/CaptureIndividual/Details?id=' + TaxPayerID + '&name=' + toSeoUrl(TaxPayerName);
    }
    else if (TaxPayerTypeID == TaxPayerType.Company) {
        return '/CaptureCorporate/Details?id=' + TaxPayerID + '&name=' + toSeoUrl(TaxPayerName);
    }
    else if (TaxPayerTypeID == TaxPayerType.Government) {
        return '/CaptureGovernment/Details?id=' + TaxPayerID + '&name=' + toSeoUrl(TaxPayerName);
    }
    else if (TaxPayerTypeID == TaxPayerType.Special) {
        return '/CaptureSpecial/Details?id=' + TaxPayerID + '&name=' + toSeoUrl(TaxPayerName);
    }
}