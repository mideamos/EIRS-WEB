var vAssessmentRuleTable;

$(document).ready(function () {
    vAssessmentRuleTable = $('#tblAssessmentRules').DataTable({
        "language": {
            "processing": "<div class='overlay custom-loader-background'><i class='fa fa-cog fa-spin custom-loader-color'></i></div>"
        },
        "processing": true,
        "serverSide": true,
        "ajax": {
            "url": "/AssessmentRule/LoadExportData",
            "type": "POST",
            "dataType": "JSON",
            "error": function (jqXHR, exception) {
                if (jqXHR.status == 401) {
                    window.location.href = '/Login/Individual';
                }
            }
        },
        "columns": [
            { "data": "TaxYear", "orderable": true, "name": "TaxYear" },
            { "data": "AssessmentRuleName", "orderable": true, "name": "AssessmentRuleName" },
            { "data": "RuleRunName", "orderable": true, "name": "RuleRunName" },
            { "data": "PaymentFrequencyName", "orderable": true, "name": "PaymentFrequencyName" },
            {
                "data": "AssessmentAmount", "orderable": true, "name": "AssessmentAmount"
                , "render": function (data, type, arule) {
                    return arule.AssessmentAmount.formatMoney();
                }
            },
            {
                "data": "", "orderable": false, "name": "Action", "render": function (data, type, aData) {
                    return '<div class="btn-group"><button type = "button" class= "btn btn-theme dropdown-toggle md-skip btn-xs" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">Action<span class="caret"></span>'
                        + '</button><ul class="dropdown-menu"><li><a onclick="javascript:jsfn_AssessmentRuleDetails(' + aData["AssessmentRuleID"] + ')">Quick View</a></li><li><a onclick="javascript:jsfn_AssessmentRuleItemList(' + aData["AssessmentRuleID"] + ')">View Items </a>'
                        + '</li><li><a onclick="javascript:jsfn_ProfileDetails(' + aData[" AssessmentRuleID"] + ')">View Profiles </a></li></ul ></div>';
                }
            }],
        "order": [[0, "desc"]]
    });
});

function jsfn_AssessmentRuleDetails(aruleID) {
    var vData = {
        AssessmentRuleID: aruleID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetAssessmentRuleDetails', vData, jsfn_ShowAssessmentRuleDetailResponse);
}

function jsfn_ShowAssessmentRuleDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvAssessmentRuleModal').modal('show');
        $('#dvAssessmentRuleRefNo').html(data.AssessmentRuleDetails.AssessmentRuleCode);
        $('#dvAssessmentRuleName').html(data.AssessmentRuleDetails.AssessmentRuleName);
        $('#dvRuleRun').html(data.AssessmentRuleDetails.RuleRunName);
        $('#dvFrequency').html(data.AssessmentRuleDetails.PaymentFrequencyName);
        $('#dvTaxYear').html(data.AssessmentRuleDetails.TaxYear);
        $('#dvSettlementMethod').html(data.AssessmentRuleDetails.SettlementMethodNames);
        $('#dvPaymentOption').html(data.AssessmentRuleDetails.PaymentOptionName);
        $('#dvStatus').html(data.AssessmentRuleDetails.ActiveText);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}

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

function jsfn_ProfileDetails(prfID) {
    var vData = {
        ProfileID: prfID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/Base/GetProfileDetails', vData, jsfn_ShowProfileDetailResponse);
}

function jsfn_ShowProfileDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvProfileModal').modal('show');
        $('#dvProfileRefNo').html(data.ProfileDetails.ProfileReferenceNo);
        $('#dvAssetType').html(data.ProfileDetails.AssetTypeName);
        $('#dvTaxPayerType').html(data.ProfileDetails.TaxPayerTypeNames);
        $('#dvTaxPayerRole').html(data.ProfileDetails.TaxPayerRoleNames);
        $('#dvProfileDescription').html(data.ProfileDetails.ProfileDescription);
        $('#dvAssetStatus').html(data.ProfileDetails.AssetTypeStatusName);
        $('#dvProfileStatus').html(data.ProfileDetails.ActiveText);
        $('#dvProfileSector').html(data.ProfileDetails.ProfileSectorNames);
        $('#dvProfileSubSector').html(data.ProfileDetails.ProfileSubSectorNames);
        $('#dvProfileGroup').html(data.ProfileDetails.ProfileGroupNames);
        $('#dvProfileSubGroup').html(data.ProfileDetails.ProfileSubGroupNames);
        $('#dvSectorElement').html(data.ProfileDetails.ProfileSectorElementNames);
        $('#dvSectorSubElement').html(data.ProfileDetails.ProfileSectorSubElementNames);
        $('#dvProfileAttribute').html(data.ProfileDetails.ProfileAttributeNames);
        $('#dvProfileSubAttribute').html(data.ProfileDetails.ProfileSubAttributeNames);
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}
