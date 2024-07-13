var vAssessmentDataTable;

jQuery(document).ready(function () {
    jsfn_BuildDataTable();
});

function jsfn_ChangeStatus(astid) {
    var vData = {
        AssessmentID: astid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/Assessment/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vAssessmentDataTable.draw();
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_BuildDataTable() {
    vAssessmentDataTable = $("#tbAssessment").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/Assessment/LoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
                data.AssessmentRefNo = $("#txtAssessmentRefNo").val();
                data.AssessmentDate = $("#txtAssessmentDate").val();
                data.TaxPayerTypeName = $("#txtTaxPayerType").val();
                data.TaxPayerName = $("#txtTaxPayerName").val();
                data.TaxPayerRIN = $("#txtTaxPayerRIN").val();
                data.AssessmentAmount = $("#txtAssessmentAmount").val();
                data.SettlementDueDate = $("#txtSettlementDueDate").val();
                data.SettlementStatusName = $("#txtSettlementStatus").val();
                data.SettlementDate = $("#txtSettlementDate").val();
                data.AssessmentNotes = $("#txtAssessmentNotes").val();
                data.ActiveText = $("#txtStatus").val();
            }
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",

        "columns": [
            { "data": "AssessmentRefNo", "orderable": true, "name": "AssessmentRefNo" },
            {
                "data": "AssessmentDate", "orderable": true, "name": "AssessmentDate", "render": function (data, type, ass) {
                    if (ass.AssessmentDate != undefined || ass.AssessmentDate != null) {
                        return jsfn_FormatJsonDate(ass.AssessmentDate, "dd-mmm-yyyy");
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
                "data": "AssessmentAmount", "orderable": true, "name": "AssessmentAmount", "render": function (data, type, ass) {
                    return ass.AssessmentAmount.formatMoney();
                }
            },
            {
                "data": "SettlementDueDate", "orderable": true, "name": "SettlementDueDate", "render": function (data, type, ass) {
                    if (ass.SettlementDueDate != undefined || ass.SettlementDueDate != null) {
                        return jsfn_FormatJsonDate(ass.SettlementDueDate, "dd-mmm-yyyy");
                    }
                    else {
                        return '';
                    }
                }
            },
            { "data": "SettlementStatusName", "orderable": true, "name": "SettlementStatusName" },
            {
                "data": "SettlementDate", "orderable": true, "name": "SettlementDate", "render": function (data, type, ass) {
                    if (ass.SettlementDate != undefined || ass.SettlementDate != null) {
                        return jsfn_FormatJsonDate(ass.SettlementDate, "dd-mmm-yyyy");
                    }
                    else {
                        return '';
                    }
                }
            },
            { "data": "AssessmentNotes", "orderable": true, "name": "AssessmentNotes" },
            { "data": "ActiveText", "orderable": true, "name": "ActiveText" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, ass) {
                    return '<div class="btn-group">' + '<button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="false">' +
                        'Actions  <span class="caret"></span></button><ul class="dropdown-menu" role="menu">'

                        + '<li><a href="/Assessment/Edit/' + ass.AssessmentID + '/' + ass.AssessmentRefNo.toSeoUrl() + '">Edit Details</a></li>'

                        + '<li><a href="/Assessment/Details/' + ass.AssessmentID + '/' + ass.AssessmentRefNo.toSeoUrl() + '">View Details</a></li>'
                        + '<li><a onclick="javascript:jsfn_ChangeStatus(' + ass.AssessmentID + ')">' + (ass.Active == 0 ? "Mark Active" : "Mark Inactive") + '</a></li>'
                        + '</ul></div>';
                }
            },
        ],
        "order": [[1, "asc"]]
    });


    $("#tbAssessment thead th input[type=text]").on('change', function () {
        vAssessmentDataTable
            .column($(this.data).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });


    ////Hide Columns
    vAssessmentDataTable.column(4).visible(false);
    vAssessmentDataTable.column(5).visible(false);
    vAssessmentDataTable.column(6).visible(false);
    vAssessmentDataTable.column(7).visible(false);
    vAssessmentDataTable.column(8).visible(false);
    vAssessmentDataTable.column(10).visible(false);
    vAssessmentDataTable.column(12).visible(false);
}

