////QC_PD_T-ERAS-12_TaxPayerLiabilityStatusAndTaxPayerLiabilityStatusDetails
jQuery(document).ready(function () {
    jsfn_bindTable();
});

var vTaxPayerLiabilityStatusDataTable;
function jsfn_bindTable() {
    vTaxPayerLiabilityStatusDataTable = $("#tblTaxPayerLiabilityStatusDetails").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/OperationManager/TaxPayerLiabilityStatusPaymentLoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.TaxYear = $("#hdnTaxYear").val();
                data.FromDate = $("#hdnFromDate").val();
                data.ToDate = $("#hdnToDate").val();
                data.TaxPayerID = $("#hdnTaxPayerID").val();
                data.TaxPayerTypeID = $("#hdnTaxPayerTypeID").val();

            }, "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName", "width": "25%" },
            { "data": "PaymentRefNo", "orderable": true, "name": "PaymentRefNo", "width": "25%" },
            {
                "data": "PaymentDate", "orderable": true, "name": "PaymentDate", "width": "25%", "render": function (data, type, st) {
                    return jsfn_FormatJsonDate(st.PaymentDate);
                }

            },
            {
                "data": "Amount", "orderable": true, "name": "Amount", "width": "25%", "render": function (data, type, st) {
                    return st.Amount.formatMoney(2);
                } },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, st) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                        + '</button><ul class="dropdown-menu"><li><a href="' + jsfn_GetTaxPayerDetailUrl($("#hdnTaxPayerID").val(), $("#hdnTaxPayerTypeID").val(), st.TaxPayerName) + '">'
                        + 'Tax Payer Details</a></li></ul></div>';
                }, "width": "10%"
            },
        ],
        "order": [[1, "asc"]]
    });
}


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

