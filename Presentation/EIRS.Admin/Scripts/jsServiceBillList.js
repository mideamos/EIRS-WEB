var vServiceBillDataTable;

jQuery(document).ready(function () {
    jsfn_BuildDataTable();
});

function jsfn_ChangeStatus(astid) {
    var vData = {
        ServiceBillID: astid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/ServiceBill/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vServiceBillDataTable.draw();
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_BuildDataTable() {
    vServiceBillDataTable = $("#tbServiceBill").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/ServiceBill/LoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.ServiceBillRefNo = $("#txtServiceBillRefNo").val();
                data.ServiceBillDate = $("#txtServiceBillDate").val();
                data.TaxPayerTypeName = $("#txtTaxPayerType").val();
                data.TaxPayerName = $("#txtTaxPayerName").val();
                data.TaxPayerRIN = $("#txtTaxPayerRIN").val();
                data.ServiceBillAmount = $("#txtServiceBillAmount").val();
                data.SettlementDueDate = $("#txtSettlementDueDate").val();
                data.SettlementStatusName = $("#txtSettlementStatus").val();
                data.SettlementDate = $("#txtSettlementDate").val();
                data.ActiveText = $("#txtStatus").val();
                data.ServiceBillNotes = $("#txtServiceBillNotes").val();
            }
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",

        "columns": [
            { "data": "ServiceBillRefNo", "orderable": true, "name": "ServiceBillRefNo" },
            {
                "data": "ServiceBillDate", "orderable": true, "name": "ServiceBillDate", "render": function (data, type, servbill) {
                    if (servbill.ServiceBillDate != undefined || servbill.ServiceBillDate != null) {
                        return jsfn_FormatJsonDate(servbill.ServiceBillDate, "dd-mmm-yyyy");
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
                "data": "ServiceBillAmount", "orderable": true, "name": "ServiceBillAmount", "render": function (data, type, servbill) {
                    return servbill.ServiceBillAmount.formatMoney();
                }
            },
            {
                "data": "SettlementDueDate", "orderable": true, "name": "SettlementDueDate", "render": function (data, type, servbill) {
                    if (servbill.SettlementDueDate != undefined || servbill.SettlementDueDate != null) {
                        return jsfn_FormatJsonDate(servbill.SettlementDueDate, "dd-mmm-yyyy");
                    }
                    else {
                        return '';
                    }
                }
            },
            { "data": "SettlementStatusName", "orderable": true, "name": "SettlementStatusName" },
            {
                "data": "SettlementDate", "orderable": true, "name": "SettlementDate", "render": function (data, type, servbill) {
                    if (servbill.SettlementDate != undefined || servbill.SettlementDate != null) {
                        return jsfn_FormatJsonDate(servbill.SettlementDate, "dd-mmm-yyyy");
                    }
                    else {
                        return '';
                    }
                }
            },
            { "data": "Notes", "orderable": true, "name": "Notes" },
            { "data": "ActiveText", "orderable": true, "name": "ActiveText" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, servbill) {
                    return '<div class="btn-group">' + '<button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="false">' +
                        'Actions  <span class="caret"></span></button><ul class="dropdown-menu" role="menu">'

                        + '<li><a href="/ServiceBill/Edit/' + servbill.ServiceBillID + '/' + servbill.ServiceBillRefNo.toSeoUrl() + '">Edit Details</a></li>'

                        + '<li><a href="/ServiceBill/Details/' + servbill.ServiceBillID + '/' + servbill.ServiceBillRefNo.toSeoUrl() + '">View Details</a></li>'
                        + '<li><a onclick="javascript:jsfn_ChangeStatus(' + servbill.ServiceBillID + ')">' + (servbill.Active == 0 ? "Mark Active" : "Mark Inactive") + '</a></li>'
                        + '</ul></div>';
                }
            },
        ],
        "order": [[1, "asc"]]
    });


    $("#tbServiceBill thead th input[type=text]").on('change', function () {
        vServiceBillDataTable
            .column($(this.data).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });

    ////Hide Columns
    vServiceBillDataTable.column(4).visible(false);
    vServiceBillDataTable.column(5).visible(false);
    vServiceBillDataTable.column(6).visible(false);
    vServiceBillDataTable.column(7).visible(false);
    vServiceBillDataTable.column(8).visible(false);
    vServiceBillDataTable.column(10).visible(false);
    vServiceBillDataTable.column(12).visible(false);
}

