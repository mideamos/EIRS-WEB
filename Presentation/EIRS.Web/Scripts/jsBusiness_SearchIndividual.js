﻿$(document).ready(function () {
    $('#dvSearchData').hide();

    $("#btnSaveLinkAsset").click(function () {
        var vTaxPayerRoleID = $("#cboTaxPayerRole").val();

        if (vTaxPayerRoleID != '') {
            var vData = {
                TaxPayerTypeID: $("#hdnTaxPayerTypeID").val(),
                TaxPayerID: $("#hdnTaxPayerID").val(),
                AssetTypeID: $("#hdnAssetTypeID").val(),
                TaxPayerRoleID: vTaxPayerRoleID,
                AssetID: $("#hdnAssetID").val()
            };

            jsfn_ShowLoading();
            jsfn_ajaxPost('/Base/LinkTaxPayerToAsset', vData);
            jsfn_HideLoading();
            debugger
            $("#dvLinkAssetModal").modal('hide');
            window.location.href = "/CaptureIndividual/Details" + "/" + vData.TaxPayerID;
        }
        else {
            window.location.href = "/CaptureIndividual/Details" + "/" + vData.TaxPayerID;
            $("#dvLAMessage").html('<div class="alert alert-danger">Select Tax Payer Role</div>');
        }
    });

    $("#btnSearch").click(function () { return jsfn_SearchValidation('frmSearchIndividual'); });
});

function jsfn_LinkAssetResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $("#dvLAMessage").html(data.Message);
        $("#btnCancel")[0].click();
    }
    else {
        $("#dvLAMessage").html('<div class="alert alert-danger">' + data.Message + '</div>');
    }
}

function jsfn_ShowData() {
    $('#dvSearchData').show();
    jsfn_bindTable();
}

function jsfn_bindTable() {
    var vColumnsList = [{
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": true
    }, {
        "orderable": false
    }];

    var vSortOrder = [];

    jsfn_ConvertToDatableWithCustomSort($('#tblIndividual'), vColumnsList, 'No Individuals Found', vSortOrder);
    $(".collapse").click();
}

function jsfn_LinkTaxPayerInformation(indID) {
    $("#hdnAssetID").val($("#hdnBusinessID").val());
    $("#hdnTaxPayerTypeID").val(TaxPayerType.Individual);
    $("#hdnAssetTypeID").val(AssetType.Business);
    $("#hdnTaxPayerID").val(indID);
    $("#dvBuildingUnit").hide();

    var vData = {
        AssetTypeID: AssetType.Business,
        TaxPayerTypeID: TaxPayerType.Individual,
    };

    jsfn_ajaxPost('/Base/GetTaxPayerRole_ATTP', vData, jsfn_BindTaxPayerRoleResponse);
}

function jsfn_BindTaxPayerRoleResponse(data) {
    $("#cboTaxPayerRole").html(""); // clear before appending new list

    $("#cboTaxPayerRole").append(
        $('<option></option>').val("").html("Select Tax Payer Role"));

    $.each(data, function (i, tprol) {
        $("#cboTaxPayerRole").append(
            $('<option></option>').val(tprol.id).html(tprol.text));
    });

    $('#cboTaxPayerRole').selectpicker('refresh');
    $("#dvLinkAssetModal").modal('show');
}

