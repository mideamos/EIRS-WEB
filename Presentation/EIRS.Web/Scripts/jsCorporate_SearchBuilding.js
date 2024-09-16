$(document).ready(function () {
    $('#dvSearchData').hide();

    $("#btnSaveLinkAsset").click(function () {
        var vTaxPayerRoleID = $("#cboTaxPayerRole").val();
        var vBuildingUnitID = $("#cboBuildingUnit").val();

        if (vTaxPayerRoleID != '' && vBuildingUnitID != '') {
            var vData = {
                TaxPayerTypeID: $("#hdnTaxPayerTypeID").val(),
                TaxPayerID: $("#hdnTaxPayerID").val(),
                AssetTypeID: $("#hdnAssetTypeID").val(),
                TaxPayerRoleID: vTaxPayerRoleID,
                AssetID: $("#hdnAssetID").val(),
                BuildingUnitID: vBuildingUnitID
            };

            jsfn_ShowLoading();
            jsfn_ajaxPost('/Base/LinkTaxPayerToAsset', vData);
            jsfn_HideLoading();
            debugger
            $("#dvLinkAssetModal").modal('hide');
            window.location.href = "/CaptureCorporate/Details" + "/" + vData.TaxPayerID;
        }
        else {
            window.location.href = "/CaptureCorporate/Details" + "/" + vData.TaxPayerID;
            $("#dvLAMessage").html('<div class="alert alert-danger">Select Tax Payer Role and Building Unit</div>');
        }
    });

    $("#btnSearch").click(function () { return jsfn_SearchValidation('frmSearchBuilding'); });
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

    jsfn_ConvertToDatableWithCustomSort($('#tblBuildingList'), vColumnsList, 'No Building Found', vSortOrder);
}

function jsfn_LinkAssetInformation(bldid) {
    $("#hdnTaxPayerID").val($("#hdnCompanyID").val());
    $("#hdnTaxPayerTypeID").val(TaxPayerType.Company);
    $("#hdnAssetTypeID").val(AssetType.Building);
    $("#hdnAssetID").val(bldid);

    var vData = {
        AssetTypeID: AssetType.Building,
        TaxPayerTypeID: TaxPayerType.Company,
    };

    jsfn_ajaxPost('/Base/GetTaxPayerRole_ATTP', vData, jsfn_BindTaxPayerRoleResponse);
    jsfn_ajaxPost('/Base/GetBuildingUnitList', { BuildingID: bldid }, jsfn_BindBuildingUnitResponse);
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
}

function jsfn_BindBuildingUnitResponse(data) {
    $("#cboBuildingUnit").html(""); // clear before appending new list

    $("#cboBuildingUnit").append(
        $('<option></option>').val("").html("Select Building Unit"));

    $.each(data, function (i, bunt) {
        $("#cboBuildingUnit").append(
            $('<option></option>').val(bunt.BuildingUnitID).html(bunt.UnitNumber));
    });

    $('#cboBuildingUnit').selectpicker('refresh');
    $("#dvLinkAssetModal").modal('show');
}

