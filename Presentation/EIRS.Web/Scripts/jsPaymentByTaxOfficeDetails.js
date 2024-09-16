//QC_VP_T-EARS-56/63/64_OperationManagerMakePaymentByTaxOffice

var vPaymentByTaxOfficeDetailTable;

jQuery(document).ready(function () {
    vPaymentByTaxOfficeDetailTable = $("#tblPaymentDetailsByTaxOffice").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "responsive": true,
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/OperationManager/PaymentByTaxOfficeDetailLoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.TaxYear = $("#hdnTaxYear").val();
                data.FromDate = $("#hdnFromDate").val();
                data.ToDate = $("#hdnToDate").val();
                data.PaymentTypeID = $("#hdnPaymentType").val();
                data.TaxOfficeID = $("#hdnTaxOffice").val();
            }, "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName" },
            { "data": "PaymentRef", "orderable": true, "name": "PaymentRef" },
            {
                "data": "PaymentDate", "orderable": true, "name": "PaymentDate", "render": function (data, type, st) {
                    var vPaymentDate = new Date(parseInt(st.PaymentDate.substr(6)));

                    return vPaymentDate.format("dd-mmm-yyyy");
                }
            },
            {
                "data": "Amount", "orderable": true, "name": "Amount", "render": function (data, type, st) {
                    return st.Amount.formatMoney();
                }
            },
            { "data": "PaymentMethodName", "orderable": true, "name": "PaymentMethodName" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, st) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                        + '</button><ul class="dropdown-menu"><li><a href="' + jsfn_GetTaxPayerDetailUrl(st.TaxPayerID, st.TaxPayerTypeID, st.TaxPayerName) + '">'
                        + 'Tax Payer Details</a></li>'
                        + (st.PaymentTypeID === 2 ? '<li><a href="/OperationManager/PoATransfer?taxpayerid=' + st.TaxPayerID + '&taxpayertypeid=' + st.TaxPayerTypeID + '">Transfer PoA</a></li>' : '')
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
