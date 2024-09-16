$(document).ready(function () {
    $('#dvSearchData').hide();

    $("#btnSaveLinkAsset").click(function () {
        var vTaxPayerRoleID = $("#cboTaxPayerRole").val();
        debugger
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
            window.location.href = "/CaptureCorporate/Details" + "/" + vData.TaxPayerID;
        }
        else {
            window.location.href = "/CaptureCorporate/Details" + "/" + vData.TaxPayerID;
            $("#dvLAMessage").html('<div class="alert alert-danger">Select Tax Payer Role</div>');
        }
    });

    $("#btnSearch").click(function () { return jsfn_SearchValidation('frmSearchCorporate'); });
});

function jsfn_LinkAssetResponse(data) {
   
    debugger
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

    jsfn_ConvertToDatableWithCustomSort($('#tblCompany'), vColumnsList, 'No Coporates Found', vSortOrder);
    $(".collapse").click();
}

function jsfn_LinkTaxPayerInformation(compid) {
    $("#hdnAssetID").val($("#hdnBuildingID").val());
    $("#hdnTaxPayerTypeID").val(TaxPayerType.Company);
    $("#hdnAssetTypeID").val(AssetType.Building);
    $("#hdnTaxPayerID").val(compid);
    $("#dvBuildingUnit").hide();

    var vData = {
        AssetTypeID: AssetType.Building,
        TaxPayerTypeID: TaxPayerType.Company,
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

