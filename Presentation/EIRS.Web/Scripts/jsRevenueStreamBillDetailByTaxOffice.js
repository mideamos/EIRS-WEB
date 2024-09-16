////QC_PD_T-ERAS-12_RevenueStreamBillByTaxOfficeAndRevenueStreamBillDetailByTaxOffice
jQuery(document).ready(function () {
    jsfn_bindTable();
});

var vRevenueStreamBillDetailByTaxOfficeDataTable;
function jsfn_bindTable() {
    vRevenueStreamBillDetailByTaxOfficeDataTable = $("#tblRevenueStreamBillDetailByTaxOffice").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/OperationManager/RevenueStreamBillDetailByTaxOfficeLoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.RevenueStreamID = $("#hdnRevenueStreamID").val();
                data.TaxOfficeID = $("#hdnTaxOfficeID").val();
                data.TaxYear = $("#hdnTaxYear").val();
                data.FromDate = $("#hdnFromDate").val();
                data.ToDate = $("#hdnToDate").val();
            }, "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName", "width": "25%" },
            { "data": "TaxPayerTypeName", "orderable": true, "name": "TaxPayerTypeName", "width": "25%" },
            { "data": "TaxPayerRINNumber", "orderable": true, "name": "TaxPayerRIN", "width": "25%" },
            { "data": "AssetName", "orderable": true, "name": "AssetName", "width": "25%" },
            { "data": "AssetTypeName", "orderable": true, "name": "AssetTypeName", "width": "25%" },
            { "data": "AssetRIN", "orderable": true, "name": "AssetRIN", "width": "25%" },
            {
                "data": "BillAmount", "orderable": true, "name": "BillAmount", "width": "25%", "render": function (data, type, st) {
                    return st.BillAmount.formatMoney(2);
                }
            },
            {
                "data": "SettlementAmount", "orderable": true, "name": "SettlementAmount", "width": "25%", "render": function (data, type, st) {
                    return st.SettlementAmount.formatMoney(2);
                }
            },
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

