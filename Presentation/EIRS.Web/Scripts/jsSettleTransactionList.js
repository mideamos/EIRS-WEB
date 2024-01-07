jQuery(document).ready(function () {
    jsfn_bindTable();

    $.fn.datepicker.defaults.format = "dd/mm/yyyy";

    $("#txtFromDate").datepicker({
        autoclose: true
    }).on('changeDate', function (selected) {
        var minDate = new Date(selected.date.valueOf());
        $('#txtToDate').datepicker('setStartDate', minDate);
    });

    $("#txtToDate").datepicker({
        autoclose: true
    }).on('changeDate', function (selected) {
            var maxDate = new Date(selected.date.valueOf());
            $('#txtFromDate').datepicker('setEndDate', maxDate);
        });

    //jsfn_OnEnterPress($("#frmSettlementTransactionList"), $("#btnSearch"));
    $("#btnSearch").click(function () {
        vSettleTransactionDataTable.draw();
    });
});

var vSettleTransactionDataTable;
function jsfn_bindTable() {
    vSettleTransactionDataTable = $("#tblSettlementTransaction").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/Settle/SettleTransactionLoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.FromDate = $("#txtFromDate").val();
                data.ToDate = $("#txtToDate").val();
                data.TransactionTypeID = $("#cboTransactionType").val();
            }, "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "RefNo", "orderable": true, "name": "RefNo" },
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName" },
            { "data": "TransactionTypeName", "orderable": true, "name": "TransactionTypeName" },
            { "data": "SettlementMethodName", "orderable": true, "name": "SettlementMethodName" },
            {
                "data": "Amount", "orderable": true, "name": "Amount", "render": function (data, type, st) {
                    return st.Amount.formatMoney();
                }
            },
            {
                "data": "TransactionDate", "orderable": true, "name": "TransactionDate", "render": function (data, type, st) {
                    var vBillDate = new Date(parseInt(st.TransactionDate.substr(6)));
                   
                    return vBillDate.format("dd-mmm-yyyy");
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
        "order": [[0, "asc"]]
    });

    $('.collapse').click();
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
