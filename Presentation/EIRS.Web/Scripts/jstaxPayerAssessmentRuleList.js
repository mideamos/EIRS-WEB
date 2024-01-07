var vAssessmentRuleTable;

$(document).ready(function () {
    vAssessmentRuleTable = $('#tblAssessmentRules').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/Base/GetAssessmentRuleData",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [{
            "data": "TaxYear"
        }, {
            "data": "AssessmentRuleName"
        }, {
            "data": "RuleRunName"
        }, {
            "data": "PaymentFrequencyName"
        }, {
            "data": "AssessmentAmount"
        }, {
            "data": "AssessmentRuleID"
        }],
        "fnRowCallback": function (nRow, aData, iDisplayIndex, iDisplayIndexFull) {
            $('td:eq(5)', nRow).html('<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                + '</button><ul class="dropdown-menu"><li><a onclick="javascript:jsfn_AssessmentRuleDetails(' + aData["AssessmentRuleID"] + ')">Quick View</a></li><li><a onclick="javascript:jsfn_AssessmentRuleItemList(' + aData["AssessmentRuleID"] + ')">View Items </a>'
                + '</li><li><a onclick="javascript:jsfn_ProfileDetails(' + aData["AssessmentRuleID"] + ')">View Profiles </a></li></ul ></div>');
            $('td:eq(4)', nRow).html(aData["AssessmentAmount"].formatMoney());
        },
    });
});

function jsfn_FilterAssessmentRuleTable(vTaxYear) {
    vAssessmentRuleTable.search(vTaxYear).draw();
}

function jsfn_AssessmentRuleItemList(aruleID) {
    var vData = {
        AssessmentRuleID: aruleID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetAssessmentRuleItemList', vData, jsfn_AssessmentRuleItemResponse);
}

function jsfn_AssessmentRuleItemResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $('#dvAssessmentRuleItemModal').modal('show');
        $('#dvAssessmentRuleItemData').html(data.AssessmentRuleItemList);
        jsfn_ConvertToDatableWithCustomSort($('#tblAssessmentRuleItem'), [{ "orderable": true }, { "orderable": true }, { "orderable": true }], 'No Assessment Items Found');
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

