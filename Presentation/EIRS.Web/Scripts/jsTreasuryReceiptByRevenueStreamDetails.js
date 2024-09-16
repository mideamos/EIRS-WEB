

var vTreasuryReceiptByRevenueStreamDetailTable;

jQuery(document).ready(function () {
    vTreasuryReceiptByRevenueStreamDetailTable = $("#tblTreasuryReceiptDetailsByRevenueStream").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "responsive": true,
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/OperationManager/TreasuryReceiptByRevenueStreamDetailLoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.TaxYear = $("#hdnTaxYear").val();
                data.FromDate = $("#hdnFromDate").val();
                data.ToDate = $("#hdnToDate").val();
                data.RevenueStreamID = $("#hdnRevenueStream").val();
            }, "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName" },
            { "data": "ReceiptRefNo", "orderable": true, "name": "ReceiptRefNo" },
            {
                "data": "ReceiptDate", "orderable": true, "name": "ReceiptDate", "render": function (data, type, st) {
                    var vReceiptDate = new Date(parseInt(st.ReceiptDate.substr(6)));

                    return vReceiptDate.format("dd-mmm-yyyy");
                }
            },
            {
                "data": "Amount", "orderable": true, "name": "Amount", "render": function (data, type, st) {
                    return st.Amount.formatMoney();
                }
            },
            { "data": "GeneratedBy", "orderable": true, "name": "GeneratedBy" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, st) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                        + '</button><ul class="dropdown-menu"><li><a href="' + jsfn_GetTaxPayerDetailUrl(st.TaxPayerID, st.TaxPayerTypeID, st.TaxPayerName) + '">'
                        + 'Tax Payer Details</a></li>'
                        + '</ul></div>';
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
