$(document).ready(function () {
    $('#cboTown').on("change", jsfn_onTownChange);
    $('#cboLGA').on("change", jsfn_onLGAChange);
    $('#cboLandPurpose').on("change", jsfn_onLandPurposeChange);
});

function jsfn_onLandPurposeChange() {
    var vLandPurposeID = $('#cboLandPurpose').val();

    var vData = {
        LandPurposeID: vLandPurposeID == '' ? 0 : vLandPurposeID
    };

    jsfn_ajaxPost('/Base/GetLandFunction', vData, jsfn_onLandPurposeChangeResponse);
}

function jsfn_onLandPurposeChangeResponse(data) {
    $("#cboLandFunction").html(""); // clear before appending new list

    $("#cboLandFunction").append(
        $('<option></option>').val("").html("Select Land Function"));

    $.each(data, function (i, lfunc) {
        $("#cboLandFunction").append(
            $('<option></option>').val(lfunc.id).html(lfunc.text));
    });

    $('#cboLandFunction').selectpicker('refresh');
}

function jsfn_onTownChange() {
    var vTownID = $('#cboTown').val();

    if (vTownID != '') {

        var vData = {
            TownID: vTownID == '' ? 0 : vTownID
        };

        jsfn_ajaxPost('/Base/GetTownDetails', vData, jsfn_onTownChangeResponse);
    }
}

function jsfn_onTownChangeResponse(data) {
    if (data != undefined) {
        $("#cboLGA").val(data.LGAID).selectpicker('refresh');
        jsfn_onLGAChange();
    }
}

function jsfn_onLGAChange() {
    var vLGAID = $('#cboLGA').val();

    var vData = {
        LGAID: vLGAID == '' ? 0 : vLGAID
    };

    jsfn_ajaxPost('/Base/GetWard', vData, jsfn_onLGAChangeResponse);
}

function jsfn_onLGAChangeResponse(data) {
    $("#cboWard").html(""); // clear before appending new list

    $("#cboWard").append(
        $('<option></option>').val("").html("Select Ward"));

    $.each(data, function (i, wrd) {
        $("#cboWard").append(
            $('<option></option>').val(wrd.id).html(wrd.text));
    });

    $('#cboWard').selectpicker('refresh');
}