$(document).ready(function () {

    $(".select2").select2();
    jsfn_TaxPayerSelection();

    $('#cboBusinessType').on("change", jsfn_onBusinessTypeChange);
    $('#cboBusinessCategory').on("change", jsfn_onBusinessCategoryChange);
    $('#cboBusinessSector').on("change", jsfn_onBusinessSectorChange);
    $('#cboTaxPayerType').on("change", jsfn_onTaxPayerTypeChange);
});

function jsfn_TaxPayerSelection() {
    var vTaxPayerTypeID = $('#cboTaxPayerType').val();

    if (vTaxPayerTypeID == '1') {
        $("#fsIndividual").removeClass('hide');
        $("#fsCompany").addClass('hide');
        $("#fsGovernment").addClass('hide');
        $("#fsSpecial").addClass('hide');
    }
    else if (vTaxPayerTypeID == '2') {
        $("#fsIndividual").addClass('hide');
        $("#fsCompany").removeClass('hide');
        $("#fsGovernment").addClass('hide');
        $("#fsSpecial").addClass('hide');
    }
    else if (vTaxPayerTypeID == '3') {
        $("#fsIndividual").addClass('hide');
        $("#fsCompany").addClass('hide');
        $("#fsGovernment").addClass('hide');
        $("#fsSpecial").removeClass('hide');
    }
    else if (vTaxPayerTypeID == '4') {
        $("#fsIndividual").addClass('hide');
        $("#fsCompany").addClass('hide');
        $("#fsGovernment").removeClass('hide');
        $("#fsSpecial").addClass('hide');
    }
    else {
        $("#fsIndividual").addClass('hide');
        $("#fsCompany").addClass('hide');
        $("#fsGovernment").addClass('hide');
        $("#fsSpecial").addClass('hide');
    }

}

function jsfn_onBusinessTypeChange() {
    var vBusinessTypeID = $('#cboBusinessType').val();

    var vData = {
        BusinessTypeID: vBusinessTypeID == '' ? 0 : vBusinessTypeID
    };

    jsfn_ajaxPost('/CBS/BusinessTypeChange', vData, jsfn_onBusinessTypeChangeResponse);
}

function jsfn_onBusinessTypeChangeResponse(data) {
    $("#cboBusinessCategory").html("");

    $("#cboBusinessCategory").append(
        $('<option></option>').val("").html("Select Business Category"));

    $.each(data.BusinessCategory, function (i, bcat) {
        $("#cboBusinessCategory").append(
            $('<option></option>').val(bcat.BusinessCategoryID).html(bcat.BusinessCategoryName));
    });

    $('#cboBusinessCategory').val('').select2({ "width": "100%" });

    $("#cboBusinessStructure").html("");

    $("#cboBusinessStructure").append(
        $('<option></option>').val("").html("Select Business Structure"));

    $.each(data.BusinessStructure, function (i, bcat) {
        $("#cboBusinessStructure").append(
            $('<option></option>').val(bcat.BusinessStructureID).html(bcat.BusinessStructureName));
    });

    $('#cboBusinessStructure').val('').select2({ "width": "100%" });

    $("#cboBusinessOperation").html("");

    $("#cboBusinessOperation").append(
        $('<option></option>').val("").html("Select Business Operation"));

    $.each(data.BusinessOperation, function (i, bcat) {
        $("#cboBusinessOperation").append(
            $('<option></option>').val(bcat.BusinessOperationID).html(bcat.BusinessOperationName));
    });

    $('#cboBusinessOperation').val('').select2({ "width": "100%" });
}

function jsfn_onBusinessCategoryChange() {
    var vBusinessCategoryID = $('#cboBusinessCategory').val();

    if (vBusinessCategoryID != '') {

        var vData = {
            BusinessCategoryID: vBusinessCategoryID == '' ? 0 : vBusinessCategoryID
        };

        jsfn_ajaxPost('/CBS/GetBusinessSector', vData, jsfn_onBusinessCategoryChangeResponse);
    }
}

function jsfn_onBusinessCategoryChangeResponse(data) {
    $("#cboBusinessSector").html("");

    $("#cboBusinessSector").append(
        $('<option></option>').val("").html("Select Business Sector"));

    if (data != undefined) {
        $.each(data, function (i, bcat) {
            $("#cboBusinessSector").append(
                $('<option></option>').val(bcat.BusinessSectorID).html(bcat.BusinessSectorName));
        });

        $('#cboBusinessSector').val('').select2({ "width": "100%" });
    }
}


function jsfn_onBusinessSectorChange() {
    var vBusinessSectorID = $('#cboBusinessSector').val();

    if (vBusinessSectorID != '') {

        var vData = {
            BusinessSectorID: vBusinessSectorID == '' ? 0 : vBusinessSectorID
        };

        jsfn_ajaxPost('/CBS/GetBusinessSubSector', vData, jsfn_onBusinessSectorChangeResponse);
    }
}

function jsfn_onBusinessSectorChangeResponse(data) {
    $("#cboBusinessSubSector").html("");

    $("#cboBusinessSubSector").append(
        $('<option></option>').val("").html("Select Business Sub Sector"));

    if (data != undefined) {
        $.each(data, function (i, bcat) {
            $("#cboBusinessSubSector").append(
                $('<option></option>').val(bcat.BusinessSubSectorID).html(bcat.BusinessSubSectorName));
        });

        $('#cboBusinessSubSector').val('').select2({ "width": "100%" });
    }
}



function jsfn_onTaxPayerTypeChange() {
    jsfn_TaxPayerSelection();
    var vTaxPayerTypeID = $('#cboTaxPayerType').val();
    var vAssetTypeID = $('#cboAssetType').val();

    if (vTaxPayerTypeID != '' && vAssetTypeID != '') {

        var vData = {
            TaxPayerTypeID: vTaxPayerTypeID == '' ? 0 : vTaxPayerTypeID,
            AssetTypeID: vAssetTypeID == '' ? 0 : vAssetTypeID
        };

        jsfn_ajaxPost('/CBS/GetTaxPayerRole', vData, jsfn_onTaxPayerTypeChangeResponse);
    }
}

function jsfn_onTaxPayerTypeChangeResponse(data) {
    $("#cboTaxPayerRole").html("");

    $("#cboTaxPayerRole").append(
        $('<option></option>').val("").html("Select Tax Payer Role"));

    if (data != undefined) {
        $.each(data, function (i, bcat) {
            $("#cboTaxPayerRole").append(
                $('<option></option>').val(bcat.TaxPayerRoleID).html(bcat.TaxPayerRoleName));
        });

        $('#cboTaxPayerRole').val('').select2({ "width": "100%" });
    }
}