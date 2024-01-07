$(document).ready(function () {
    $('#cboBusinessType').on("change", jsfn_onBusinessTypeChange);
    $('#cboBusinessCategory').on("change", jsfn_onBusinessCategoryChange);
    $('#cboBusinessSector').on("change", jsfn_onBusinessSectorChange);
});



function jsfn_onBusinessTypeChange() {
    var vBusinessTypeID = $('#cboBusinessType').val();

    var vData = {
        BusinessTypeID: vBusinessTypeID == '' ? 0 : vBusinessTypeID
    };

    jsfn_ajaxPost('/Base/BusinessTypeChange', vData, jsfn_onBusinessTypeChangeResponse);
}

function jsfn_onBusinessTypeChangeResponse(data) {
    $("#cboBusinessCategory").html("");

    $("#cboBusinessCategory").append(
        $('<option></option>').val("").html("Select Business Category"));

    $.each(data.BusinessCategory, function (i, bcat) {
        $("#cboBusinessCategory").append(
            $('<option></option>').val(bcat.id).html(bcat.text));
    });

    $('#cboBusinessCategory').selectpicker('refresh');

    $("#cboBusinessStructure").html("");

    $("#cboBusinessStructure").append(
        $('<option></option>').val("").html("Select Business Structure"));

    $.each(data.BusinessStructure, function (i, bcat) {
        $("#cboBusinessStructure").append(
            $('<option></option>').val(bcat.id).html(bcat.text));
    });

    $('#cboBusinessStructure').selectpicker('refresh');

    $("#cboBusinessOperation").html("");

    $("#cboBusinessOperation").append(
        $('<option></option>').val("").html("Select Business Operation"));

    $.each(data.BusinessOperation, function (i, bcat) {
        $("#cboBusinessOperation").append(
            $('<option></option>').val(bcat.id).html(bcat.text));
    });

    $('#cboBusinessOperation').selectpicker('refresh');
}

function jsfn_onBusinessCategoryChange() {
    var vBusinessCategoryID = $('#cboBusinessCategory').val();

    if (vBusinessCategoryID != '') {

        var vData = {
            BusinessCategoryID: vBusinessCategoryID == '' ? 0 : vBusinessCategoryID
        };

        jsfn_ajaxPost('/Base/GetBusinessSector', vData, jsfn_onBusinessCategoryChangeResponse);
    }
}

function jsfn_onBusinessCategoryChangeResponse(data) {
    $("#cboBusinessSector").html("");

    $("#cboBusinessSector").append(
        $('<option></option>').val("").html("Select Business Sector"));

    if (data != undefined) {
        $.each(data, function (i, bcat) {
            $("#cboBusinessSector").append(
                $('<option></option>').val(bcat.id).html(bcat.text));
        });

        $('#cboBusinessSector').selectpicker('refresh');
    }
}


function jsfn_onBusinessSectorChange() {
    var vBusinessSectorID = $('#cboBusinessSector').val();

    if (vBusinessSectorID != '') {

        var vData = {
            BusinessSectorID: vBusinessSectorID == '' ? 0 : vBusinessSectorID
        };

        jsfn_ajaxPost('/Base/GetBusinessSubSector', vData, jsfn_onBusinessSectorChangeResponse);
    }
}

function jsfn_onBusinessSectorChangeResponse(data) {
    $("#cboBusinessSubSector").html("");

    $("#cboBusinessSubSector").append(
        $('<option></option>').val("").html("Select Business Sub Sector"));

    if (data != undefined) {
        $.each(data, function (i, bcat) {
            $("#cboBusinessSubSector").append(
                $('<option></option>').val(bcat.id).html(bcat.text));
        });

        $('#cboBusinessSubSector').selectpicker('refresh');
    }
}