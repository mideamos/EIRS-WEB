
jQuery(document).ready(function () {
    jsfn_bindTable();
});

var vBuildingDataTable;

function jsfn_bindTable() {
    vBuildingDataTable = $("#tbPoAOperation").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/OperationManager/PoAOperationLoadData",
            "type": "POST",
            "datatype": "json",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }

        },
        "columns": [


            {
                "data": "OperationDate", "orderable": true, "name": "OperationDate", "width": "10%", "render": function (data, type, operation) {
                    return jsfn_FormatJsonDate(operation.BillDate, "dd-mmm-yyyy hh:MM TT");
                }
            },
            { "data": "Operation_TypesName", "orderable": true, "name": "Operation_TypesName", "width": "10%" },
            { "data": "FromTaxPayer", "orderable": true, "name": "FromTaxPayer", "width": "20%" },
            { "data": "FromName", "orderable": true, "name": "FromName", "width": "20%" },
           
            { "data": "ToTaxPayer", "orderable": true, "name": "ToTaxPayer", "width": "10%" },
            { "data": "ToName", "orderable": true, "name": "ToName", "width": "10%" },
            {
                "data": "Amount", "orderable": true, "name": "Amount", "width": "10%", "render": function (data, type, operation) {
                    return operation.BillAmount.formatMoney(2, '₹');
                }
            },

            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, operation) {
                    return '<div class="btn-group">' + '<button class="btn btn-theme dropdown-toggle md-skip btn-xs" type="button" data-toggle="dropdown" aria-expanded="false">' +
                        "Actions" +
                        '<span class="caret">' + '</span>' +
                        '</button>' +
                        '<ul class="dropdown-menu" role="menu">' +
                        '<li><a href="/OperationManager/PoATransfer/' + operation.BillID + '">View Details</a></li>'
                        + '</ul></div>';
                }, "width": "10%"
            },
        ],
        "order": [[2, "asc"]]
    });

    $('.collapse').click();
}

