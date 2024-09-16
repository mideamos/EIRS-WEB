$(document).ready(function () {
    $('#dvSearchData').hide();
    $("#dvSearchBuilding").hide();
    $("#dvSearchBusiness").hide();
    $("#dvSearchLand").hide();
    $("#dvSearchVehicle").hide();

    $("#dvAssetType input[name='rbAssetType']").click(function () {
        var vrbAssetType = $("#dvAssetType  input:radio:checked").val();
        if (vrbAssetType === "1") {
            $("#dvSearchBuilding").show();
            $("#dvSearchBusiness").hide();
            $("#dvSearchLand").hide();
            $("#dvSearchVehicle").hide();
            $('#dvSearchData').hide();
        }
        else if (vrbAssetType === "2") {
            $("#dvSearchBuilding").hide();
            $("#dvSearchBusiness").show();
            $("#dvSearchLand").hide();
            $("#dvSearchVehicle").hide();
            $('#dvSearchData').hide();
        }
        else if (vrbAssetType === "3") {
            $("#dvSearchBuilding").hide();
            $("#dvSearchBusiness").hide();
            $("#dvSearchLand").hide();
            $("#dvSearchVehicle").show();
            $('#dvSearchData').hide();
        }
        else if (vrbAssetType === "4") {
            $("#dvSearchBuilding").hide();
            $("#dvSearchBusiness").hide();
            $("#dvSearchLand").show();
            $("#dvSearchVehicle").hide();
            $('#dvSearchData').hide();
        }
    });

    $("#btnSaveLinkAsset").click(function () {
        var vTaxPayerRoleID = $("#cboTaxPayerRole").val();
        var vBuildingUnitID = $("#cboBuildingUnit").val();

        if (vTaxPayerRoleID !== '' && ((vBuildingUnitID !== '' && $("#hdnAssetTypeID").val() === '1') || $("#hdnAssetTypeID").val() !== '1')) {
            var vData = {
                AssetTypeID: $("#hdnAssetTypeID").val(),
                TaxPayerRoleID: vTaxPayerRoleID,
                AssetID: $("#hdnAssetID").val(),
                BuildingUnitID: vBuildingUnitID
            };

            jsfn_ShowLoading();
            jsfn_ajaxPost('/SignupCorporate/LinkAsset', vData, jsfn_LinkAssetResponse);
        }
        else {
            $("#dvLAMessage").html('<div class="alert alert-danger">Select Tax Payer Role and Building Unit</div>');
        }
    });

    $("#btnBuildingSearch").click(function () { return jsfn_SearchValidation('frmSearchBuilding'); });
    $("#btnBusinessSearch").click(function () { return jsfn_SearchValidation('frmSearchBusiness'); });
    $("#btnLandSearch").click(function () { return jsfn_SearchValidation('frmSearchLand'); });
    $("#btnVehicleSearch").click(function () { return jsfn_SearchValidation('frmSearchVehicle'); });
});

function jsfn_LinkAssetResponse(data) {
    jsfn_HideLoading();
    if (data.success) {
        $("#dvLAMessage").html(data.Message);
        window.location.href = '/SignupCorporate/GeneratePassword';
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

function jsfn_BuildingDetails(bldid) {

    var vData = {
        BuildingID: bldid,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/SignupCorporate/GetBuildingDetails', vData, jsfn_BuildingDetailResponse);
}

function jsfn_BusinessDetails(busid) {

    var vData = {
        BusinessID: busid,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/SignupCorporate/GetBusinessDetails', vData, jsfn_BusinessDetailResponse);
}

function jsfn_LandDetails(lndid) {

    var vData = {
        LandID: lndid,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/SignupCorporate/GetLandDetails', vData, jsfn_LandDetailResponse);
}

function jsfn_VehicleDetails(vchid) {

    var vData = {
        VehicleID: vchid,
    };

    jsfn_ShowLoading();
    jsfn_ajaxPost('/SignupCorporate/GetVehicleDetails', vData, jsfn_VehicleDetailResponse);
}

function jsfn_LinkBuilding(bldid) {
    $("#hdnAssetTypeID").val(AssetType.Building);
    $("#hdnAssetID").val(bldid);

    var vData = {
        AssetTypeID: AssetType.Building
    };

    jsfn_ajaxPost('/SignupCorporate/GetTaxPayerRoleAssetBased', vData, jsfn_BindTaxPayerRoleResponse);
    jsfn_ajaxPost('/SignupCorporate/GetBuildingUnitList', { BuildingID: bldid }, jsfn_BindBuildingUnitResponse);
}

function jsfn_LinkBusiness(busid) {
    $("#hdnAssetTypeID").val(AssetType.Business);
    $("#hdnAssetID").val(busid);
    $("#dvBuildingUnit").hide();

    var vData = {
        AssetTypeID: AssetType.Business,
    };

    jsfn_ajaxPost('/SignupCorporate/GetTaxPayerRoleAssetBased', vData, jsfn_BindTaxPayerRoleResponse);
}

function jsfn_LinkLand(lndid) {
    $("#hdnAssetTypeID").val(AssetType.Land);
    $("#hdnAssetID").val(lndid);
    $("#dvBuildingUnit").hide();

    var vData = {
        AssetTypeID: AssetType.Land,
    };

    jsfn_ajaxPost('/SignupCorporate/GetTaxPayerRoleAssetBased', vData, jsfn_BindTaxPayerRoleResponse);
}

function jsfn_LinkVehicle(vchid) {
    $("#hdnAssetTypeID").val(AssetType.Vehicle);
    $("#hdnAssetID").val(vchid);
    $("#dvBuildingUnit").hide();

    var vData = {
        AssetTypeID: AssetType.Vehicle,
    };

    jsfn_ajaxPost('/SignupCorporate/GetTaxPayerRoleAssetBased', vData, jsfn_BindTaxPayerRoleResponse);
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

function jsfn_BindBuildingUnitResponse(data) {
    $("#cboBuildingUnit").html(""); // clear before appending new list

    $("#cboBuildingUnit").append(
        $('<option></option>').val("").html("Select Building Unit"));

    $.each(data, function (i, bunt) {
        $("#cboBuildingUnit").append(
            $('<option></option>').val(bunt.BuildingUnitID).html(bunt.UnitNumber));
    });

    $('#cboBuildingUnit').selectpicker('refresh');
}
