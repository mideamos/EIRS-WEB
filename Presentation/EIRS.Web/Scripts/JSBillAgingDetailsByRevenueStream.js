var vBillDetailsAgingByRevenueStreamDataTable;
$(document).ready(function () {
    vBillDetailsAgingByRevenueStreamDataTable = $("#tblBillDetailsAgingByRevenueStream").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/OperationManager/BillAgingDetailsByRevenueStreamLoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.RevenueStreamID = $("#hdnRevenueStreamID").val();
                data.BillType = $("#hdnBillTypeID").val();
                data.StageID = $("#hdnStageID").val();
            },
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName", "width": "25%" },
            { "data": "BillRef", "orderable": true, "name": "BillRef", "width": "25%" },
            {
                "data": "BillDate", "orderable": true, "name": "BillDate", "width": "25%", "render": function (data, type, st) {
                    var vBillDate = new Date(parseInt(st.BillDate.substr(6)));

                    return vBillDate.format("dd-mmm-yyyy");
                }
            },
            {
                "data": "Amount", "orderable": true, "name": "Amount", "width": "25%", "render": function (data, type, st) {
                    return st.Amount.formatMoney();
                }
            },
            { "data": "BillStatusName", "orderable": true, "name": "BillStatusName", "width": "25%" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, st) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                        + '</button><ul class="dropdown-menu"><li><a href="' + jsfn_GetTaxPayerDetailUrl(st.TaxPayerID, st.TaxPayerTypeID, st.TaxPayerName) + '">'
                        + 'Tax Payer Details</a></li><li><a href="' + jsfn_GetBillDetailUrl(st.TaxPayerID, st.TaxPayerTypeID, st.TaxPayerName, st.BillID, st.BillRef) + '">'
                        + 'View Bill</a></li></ul></div>';
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

function jsfn_GetBillDetailUrl(TaxPayerID, TaxPayerTypeID, TaxPayerName, BillID, BillRefNo) {
    if (TaxPayerTypeID == TaxPayerType.Individual) {
        return '/CaptureIndividual/BillDetail?id=' + TaxPayerID + '&name=' + toSeoUrl(TaxPayerName) + '&billid=' + BillID + '&billrefno=' + BillRefNo;
    }
    else if (TaxPayerTypeID == TaxPayerType.Company) {
        return '/CaptureCorporate/BillDetail?id=' + TaxPayerID + '&name=' + toSeoUrl(TaxPayerName) + '&billid=' + BillID + '&billrefno=' + BillRefNo;
    }
    else if (TaxPayerTypeID == TaxPayerType.Government) {
        return '/CaptureGovernment/BillDetail?id=' + TaxPayerID + '&name=' + toSeoUrl(TaxPayerName) + '&billid=' + BillID + '&billrefno=' + BillRefNo;
    }
    else if (TaxPayerTypeID == TaxPayerType.Special) {
        return '/CaptureSpecial/BillDetail?id=' + TaxPayerID + '&name=' + toSeoUrl(TaxPayerName) + '&billid=' + BillID + '&billrefno=' + BillRefNo;
    }
}
