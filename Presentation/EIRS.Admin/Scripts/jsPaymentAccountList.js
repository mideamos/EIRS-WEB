var vPaymentAccountDataTable;

jQuery(document).ready(function () {
    jsfn_BuildDataTable();
});

function jsfn_BuildDataTable() {
    vPaymentAccountDataTable = $("#tbPaymentAccount").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/PaymentAccount/LoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.PaymentRefNo = $("#txtPaymentRefNo").val();
                data.PaymentDate = $("#txtPaymentDate").val();
                data.TaxPayerTypeName = $("#txtTaxPayerType").val();
                data.TaxPayerName = $("#txtTaxPayerName").val();
                data.TaxPayerRIN = $("#txtTaxPayerRIN").val();
                data.Amount = $("#txtAmount").val();
                data.RevenueStreamName = $("#txtRevenueStream").val();
                data.RevenueSubStreamName = $("#txtRevenueSubStream").val();
                data.AgencyName = $("#txtAgencyName").val();
                data.SettlementMethodName = $("#txtSettlementMethod").val();
                data.SettlementStatusName = $("#txtSettlementStatus").val();
                data.Notes = $("#txtNotes").val();
                data.TransactionRefNo = $("#txtTransactionRefNo").val();
                
            }
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",

        "columns": [
            { "data": "PaymentRefNo", "orderable": true, "name": "PaymentRefNo" },
            {
                "data": "PaymentDate", "orderable": true, "name": "PaymentDate", "render": function (data, type, poa) {
                    if (poa.PaymentDate != undefined || poa.PaymentDate != null) {
                        return jsfn_FormatJsonDate(poa.PaymentDate, "dd-mmm-yyyy");
                    }
                    else {
                        return '';
                    }
                }
            },
            { "data": "TaxPayerTypeName", "orderable": true, "name": "TaxPayerTypeName" },
            { "data": "TaxPayerName", "orderable": true, "name": "TaxPayerName" },
            { "data": "TaxPayerRIN", "orderable": true, "name": "TaxPayerRIN" },
            {
                "data": "Amount", "orderable": true, "name": "Amount", "render": function (data, type, poa) {
                    return poa.Amount.formatMoney();
                }
            },

            { "data": "RevenueStreamName", "orderable": true, "name": "RevenueStreamName" },
            { "data": "RevenueSubStreamName", "orderable": true, "name": "RevenueSubStreamName" },
            { "data": "AgencyName", "orderable": true, "name": "AgencyName" },

            { "data": "SettlementMethodName", "orderable": true, "name": "SettlementMethodName" },
            { "data": "SettlementStatusName", "orderable": true, "name": "SettlementStatusName" },
            { "data": "Notes", "orderable": true, "name": "Notes" },
            { "data": "TransactionRefNo", "orderable": true, "name": "TransactionRefNo" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, poa) {
                    return '<div class="btn-group">' + '<button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="false">' +
                        'Actions  <span class="caret"></span></button><ul class="dropdown-menu" role="menu">'
                        + '<li><a href="/PaymentAccount/Edit/' + poa.PaymentAccountID + '/' + poa.PaymentRefNo.toSeoUrl() + '">Edit Details</a></li>'
                        + '<li><a href="/PaymentAccount/Details/' + poa.PaymentAccountID + '/' + poa.PaymentRefNo.toSeoUrl() + '">View Details</a></li>'
                        + '</ul></div>';
                }
            },
        ],
        "order": [[1, "asc"]]
    });


    $("#tbPaymentAccount thead th input[type=text]").on('change', function () {
        vPaymentAccountDataTable
            .column($(this.data).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });


    ////Hide Columns
    
    vPaymentAccountDataTable.column(6).visible(false);
    vPaymentAccountDataTable.column(7).visible(false);
    vPaymentAccountDataTable.column(8).visible(false);
    vPaymentAccountDataTable.column(9).visible(false);
    vPaymentAccountDataTable.column(10).visible(false);
    vPaymentAccountDataTable.column(11).visible(false);
}

