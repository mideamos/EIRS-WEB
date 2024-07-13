var vSettlementDataTable;

jQuery(document).ready(function () {
    jsfn_BuildDataTable();
});


function jsfn_BuildDataTable() {
    vSettlementDataTable = $("#tbSettlement").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/Settlement/LoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.SettlementRefNo = $("#txtSettlementRefNo").val();
                data.SettlementDate = $("#txtSettlementDate").val();
                data.BillRefNo = $("#txtAssessmentRefNo").val();
                data.BillAmount = $("#txtAssessmentAmount").val();
                data.SettlementAmount = $("#txtSettlementAmount").val();
                data.SettlementMethodName = $("#txtSettlementMethod").val();
                data.SettlementStatus = $("#txtSettlementStatus").val();
                data.SettlementNotes = $("#txtSettlementNotes").val();
            }
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",

        "columns": [
            { "data": "SettlementRefNo", "orderable": true, "name": "SettlementRefNo" },
            {
                "data": "SettlementDate", "orderable": true, "name": "SettlementDate", "render": function (data, type, sett) {
                    if (sett.SettlementDate != undefined || sett.SettlementDate != null) {
                        return jsfn_FormatJsonDate(sett.SettlementDate, "dd-mmm-yyyy");
                    }
                    else {
                        return '';
                    }
                }
            },
            { "data": "ASRefNo", "orderable": true, "name": "ASRefNo" },
            {
                "data": "ASAmount", "orderable": true, "name": "ASAmount"
                , "render": function (data, type, sett) {
                    return sett.ASAmount.formatMoney();
                }
            },
            {
                "data": "SettlementAmount", "orderable": true, "name": "SettlementAmount"
                , "render": function (data, type, sett) {
                    return sett.SettlementAmount.formatMoney();
                }
            },
            { "data": "SettlementMethodName", "orderable": true, "name": "SettlementMethodName" },
            //{ "data": "SettlementStatusName", "orderable": true, "name": "SettlementStatusName" },
            {
                "data": "SettlementStatusName", "orderable": true, "name": "SettlementStatusName"
                , "render": function (data, type, sett) {
                    let test = sett.ASAmount <= sett.SettlementAmount ? "Settled" : "Partial";

                    return test;
                }
            },
            { "data": "SettlementNotes", "orderable": true, "name": "SettlementNotes" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, sett) {
                    return '<div class="btn-group">' + '<button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="false">' +
                        'Actions  <span class="caret"></span></button><ul class="dropdown-menu" role="menu">'
                        + '<li><a href="/Settlement/Details/' + sett.SettlementID + '/' + sett.SettlementRefNo.toSeoUrl() + '">View Details</a></li>'
                        + '</ul></div>';
                }
            },
        ],
        "order": [[1, "asc"]]
    });


    $("#tbSettlement thead th input[type=text]").on('change', function () {
        vSettlementDataTable
            .column($(this.data).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });

    ////Hide Columns
    //vSettlementDataTable.column(4).visible(false);
 
}

