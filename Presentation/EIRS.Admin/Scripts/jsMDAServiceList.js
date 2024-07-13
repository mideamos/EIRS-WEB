var vMDAServiceDataTable;

jQuery(document).ready(function () {
    jsfn_BuildDataTable();
});

function jsfn_ChangeStatus(MDAServiceid) {
    var vData = {
        MDAServiceID: MDAServiceid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/MDAService/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vMDAServiceDataTable.draw();
       
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_BuildDataTable() {
    vMDAServiceDataTable = $("#tbMDAService").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/MDAService/LoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.MDAServiceCode = $("#txtMDAServiceCode").val();
                data.MDAServiceName = $("#txtMDAServiceName").val();
                data.RuleRunName = $("#txtRuleRunName").val();
                data.PaymentFrequencyName = $("#txtFrequencyName").val();
                data.MDAServiceItemName = $("#txtMDAServiceItems").val();
                data.ServiceAmount = $("#txtServiceAmount").val();
                data.TaxYear = $("#txtTaxYear").val();
                data.SettlementMethodName = $("#txtSettlementMethodName").val();
                data.PaymentOptionName = $("#txtPaymentOption").val();
                data.ActiveText = $("#txtStatus").val();
            }
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",

        "columns": [
            { "data": "MDAServiceCode", "orderable": true, "name": "MDAServiceCode" },
            { "data": "MDAServiceName", "orderable": true, "name": "MDAServiceName" },
            { "data": "RuleRunName", "orderable": true, "name": "RuleRunName" },
            { "data": "PaymentFrequencyName", "orderable": true, "name": "PaymentFrequencyName" },
            { "data": "MDAServiceItemNames", "orderable": true, "name": "MDAServiceItemNames" },
            {
                "data": "ServiceAmount", "orderable": true, "name": "ServiceAmount"
                , "render": function (data, type, MDAserv) {
                    return MDAserv.ServiceAmount.formatMoney();
                }
            },
            {
                "data": "TaxYear", "orderable": true, "name": "TaxYear"
                
            },
            { "data": "SettlementMethodNames", "orderable": true, "name": "SettlementMethodNames" },
            { "data": "PaymentOptionName", "orderable": true, "name": "PaymentOptionName" },
            { "data": "ActiveText", "orderable": true, "name": "ActiveText" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, MDAserv) {
                    return '<div class="btn-group">' + '<button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="false">' +
                        'Actions  <span class="caret"></span></button><ul class="dropdown-menu" role="menu">'
                        + '<li><a href="/MDAService/Details/' + MDAserv.MDAServiceID + '/' + MDAserv.MDAServiceName.toSeoUrl() + '">View Details</a></li>'
                        + '<li><a href="/MDAService/Edit/' + MDAserv.MDAServiceID + '/' + MDAserv.MDAServiceName.toSeoUrl() + '">Edit Details</a></li>'
                        + '<li><a href="/MDAService/MDAServiceItemDetails/' + MDAserv.MDAServiceID + '/' + MDAserv.MDAServiceName.toSeoUrl() + '">View MDA Service Item Info</a></li>'
                        + '<li><a onclick="javascript:jsfn_ChangeStatus(' + MDAserv.MDAServiceID + ')">' + (MDAserv.Active == 0 ? "Mark Active" : "Mark Inactive") + '</a></li>'
                        + '</ul></div>';
                }
            },
        ],

        "order": [[1, "asc"]]
    });

    $("#tbMDAService thead th input[type=text]").on('change', function () {
        vMDAServiceDataTable
            .column($(this).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });
    //Hide Columns
    vMDAServiceDataTable.column(2).visible(false);
    vMDAServiceDataTable.column(3).visible(false);
    vMDAServiceDataTable.column(6).visible(false);
    vMDAServiceDataTable.column(7).visible(false);
    vMDAServiceDataTable.column(8).visible(false);
}

