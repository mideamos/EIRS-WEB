var vAssessmentRuleDataTable;

jQuery(document).ready(function () {
  
    jsfn_BuildDataTableII();
});

function jsfn_ChangeStatus(AssessmentRuleid) {
    var vData = {
        AssessmentRuleID: AssessmentRuleid
    }
    jsfn_ShowLoading();
    jsfn_ajaxPost('/AssessmentRule/UpdateStatus', vData, jsfn_ChangeStatusResponse);
}

function jsfn_ChangeStatusResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        jsfn_ShowAlert(data.Message, 'success');
        vAssessmentRuleDataTable.draw();
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

function jsfn_BuildDataTableII() {
    debugger
    vAssessmentRuleDataTable = $("#tbAssessmentRuleIIW").DataTable({
        "processing": true, // for show progress bar
        "serverSide": true, // for process server side
        "filter": true,
        "orderMulti": false, // for disable multiple column at once
        "ajax": {
            "url": "/DataControl/LoadData",
            "type": "POST",
            "datatype": "json",
            "data": function (data) {
               

                data.AssessmentRuleCode = $("#txtAssessmentRuleCode").val();
                data.AssessmentRuleName = $("#txtAssessmentRuleName").val();
                data.AssessmentAmount = $("#txtAssessmentAmount").val();
                data.AssessmentItemName = $("#txtAssessmentItems").val();
               // data.TaxbaseAmount = $("#txtTaxbaseAmount").val();
                data.Percentage = $("#txtPercentage").val();
                data.TaxAmount = $("#txtTaxAmount").val();
                data.TaxYear = $("#txtTaxYear").val();
                data.TaxMonth = $("#txtTaxMonth").val();
                data.Active = $("#txtStatus").val();

                debugger
                console.log(data);
            },
        },
        "dom": "<'dt-toolbar'<'col-xs-12 col-sm-6 hidden-xs'f><'col-sm-6 col-xs-12 hidden-xs'C>r>" +
            "t" +
            "<'dt-toolbar-footer'<'col-sm-6 col-xs-12 hidden-xs'i><'col-xs-12 col-sm-6'p>>",

        "columns": [
            { "data": "AssessmentRuleCode", "orderable": true, "name": "AssessmentRuleCode" },
            { "data": "AssessmentRuleName", "orderable": true, "name": "AssessmentRuleName" },
            { "data": "AssessmentItemName", "orderable": true, "name": "AssessmentItemName" },
           // { "data": "TaxbaseAmount", "orderable": true, "name": "TaxbaseAmount" },
            { "data": "Percentage", "orderable": true, "name": "Percentage" },
            { "data": "TaxAmount", "orderable": true, "name": "TaxAmount" },
            { "data": "TaxMonth", "orderable": true, "name": "TaxMonth" },
            {
                "data": "AssessmentAmount", "orderable": true, "name": "AssessmentAmount", "render": function (data, type, asrule) {
                    return asrule.AssessmentAmount.formatMoney();
                }
            },
            {
                "data": "TaxYear", "orderable": true, "name": "TaxYear"
            },
            { "data": "Active", "orderable": true, "name": "Active" },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, asrule) {
                    return '<div class="btn-group">' + '<button class="btn btn-primary dropdown-toggle" type="button" data-toggle="dropdown" aria-expanded="false">' +
                        'Actions  <span class="caret"></span></button><ul class="dropdown-menu" role="menu">'
                        + '<li><a href="/AssessmentRule/Details/' + asrule.AssessmentRuleID + '/' + asrule.AssessmentRuleName.toSeoUrl() + '">View Details</a></li>'
                        + '<li><a href="/AssessmentRule/Edit/' + asrule.AssessmentRuleID + '/' + asrule.AssessmentRuleName.toSeoUrl() + '">Edit Details</a></li>'
                        + '<li><a href="/AssessmentRule/ProfileDetails/' + asrule.AssessmentRuleID + '/' + asrule.AssessmentRuleName.toSeoUrl() + '">View Profile Info</a></li>'
                        + '<li><a href="/AssessmentRule/AssessmentItemDetails/' + asrule.AssessmentRuleID + '/' + asrule.AssessmentRuleName.toSeoUrl() + '">View Assessment Item Info</a></li>'
                        + '<li><a onclick="javascript:jsfn_ChangeStatus(' + asrule.AssessmentRuleID + ')">' + (asrule.Active == 0 ? "Mark Active" : "Mark Inactive") + '</a></li>'
                        + '</ul></div>';
                }
            },
        ],

        "order": [[1, "asc"]]
    });

    $("#tbAssessmentRuleIIW thead th input[type=text]").on('change', function () {
        vAssessmentRuleDataTable
            .column($(this).parent().index() + ':visible')
            .search(this.value)
            .draw();
    });
    //Hide Columns
    vAssessmentRuleDataTable.column(3).visible(false);
    vAssessmentRuleDataTable.column(4).visible(false);
    vAssessmentRuleDataTable.column(7).visible(false);
    vAssessmentRuleDataTable.column(8).visible(false);
    vAssessmentRuleDataTable.column(9).visible(false);
}

