$(document).ready(function () {
    $("#dvAssessmentRuleProfileList").hide();
    $("#dvAssessmentRuleItemList").hide();

    $("#aAssessmentRuleProfile").click(function () {
        $("#dvAssessmentRuleDetails").hide();
        $("#dvAssessmentRuleProfileList").show();
    });

    $("#aAssessmentRuleItem").click(function () {
        $("#dvAssessmentRuleDetails").hide();
        $("#dvAssessmentRuleItemList").show();
    });

    $("#btnARPBack,#btnARIBack").click(function () {
        $("#dvAssessmentRuleDetails").show();
        $("#dvAssessmentRuleProfileList").hide();
        $("#dvAssessmentRuleItemList").hide();
    });

    $('#dvProfileDetails').dialog({
        autoOpen: false,
        width: 800,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Profile Details</h4></div>",
        buttons: [{
            html: "<i class='fa fa-times'></i>&nbsp; Close",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    $('#dvAssessmentItemDetails').dialog({
        autoOpen: false,
        width: 800,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Assessment Item Details</h4></div>",
        buttons: [{
            html: "<i class='fa fa-times'></i>&nbsp; Close",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });
});

function jsfn_ShowProfileDetails(ProfileID) {

    var vData = {
        ProfileID: ProfileID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/AssessmentRule/GetProfileDetails', vData, jsfn_ShowProfileDetailResponse)
}

function jsfn_ShowProfileDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvProfileDetails').dialog('open');
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

function jsfn_ShowAssessmentItemDetails(AssessmentItemID) {

    var vData = {
        AssessmentItemID: AssessmentItemID,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/AssessmentRule/GetAssessmentItemDetails', vData, jsfn_ShowAssessmentItemDetailResponse)
}

function jsfn_ShowAssessmentItemDetailResponse(data) {

    jsfn_HideLoading();
    if (data.success) {
        $('#dvAssessmentItemDetails').dialog('open');
        $('#dvAssessmentItemRefNo').html(data.AssessmentItemDetails.AssessmentItemReferenceNo);
        $('#dvAIAssetType').html(data.AssessmentItemDetails.AssetTypeName);
        $('#dvAssessmentGroup').html(data.AssessmentItemDetails.AssessmentGroupName);
        $('#dvAssessmentSubGroup').html(data.AssessmentItemDetails.AssessmentSubGroupName);
        $('#dvRevenueStream').html(data.AssessmentItemDetails.RevenueStreamName);
        $('#dvRevenueSubStream').html(data.AssessmentItemDetails.RevenueSubStreamName);
        $('#dvAssessmentItemCategory').html(data.AssessmentItemDetails.AssessmentItemCategoryName);
        $('#dvAssessmentItemSubCategory').html(data.AssessmentItemDetails.AssessmentItemSubCategoryName);
        $('#dvRevenueAgency').html(data.AssessmentItemDetails.AgencyName);
        $('#dvAssessmentItemName').html(data.AssessmentItemDetails.AssessmentItemName);
        $('#dvComputation').html(data.AssessmentItemDetails.ComputationName);
        $('#dvTaxBaseAmount').html(data.AssessmentItemDetails.TaxBaseAmount);
        $('#dvPercentage').html(data.AssessmentItemDetails.Percentage);
        $('#dvTaxAmount').html(data.AssessmentItemDetails.TaxAmount);
       
        if (data.AssessmentItemDetails.ComputationID == 1) {
            $('#dvCPercentage').hide();
        }
    }
    else {
        jsfn_ShowAlert(data.Message, 'danger');
    }
}
