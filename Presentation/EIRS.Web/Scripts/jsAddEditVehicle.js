$(document).ready(function () {
    $('#cboVehiclePurpose').on("change", jsfn_onVehiclePurposeChange);
    $('#cboVehicleType').on("change", jsfn_onVehicleTypeChange);
});


function jsfn_onVehiclePurposeChange() {
    var vVehiclePurposeID = $('#cboVehiclePurpose').val();

    var vData = {
        VehiclePurposeID: vVehiclePurposeID == '' ? 0 : vVehiclePurposeID
    };

    jsfn_ajaxPost('/Base/GetVehicleFunction', vData, jsfn_onVehiclePurposeChangeResponse);
}

function jsfn_onVehiclePurposeChangeResponse(data) {
    $("#cboVehicleFunction").html(""); // clear before appending new list

    $("#cboVehicleFunction").append(
        $('<option></option>').val("").html("Select Vehicle Function"));

    $.each(data, function (i, bfunc) {
        $("#cboVehicleFunction").append(
            $('<option></option>').val(bfunc.id).html(bfunc.text));
    });

    $('#cboVehicleFunction').selectpicker('refresh');
}

function jsfn_onVehicleTypeChange() {
    var vVehicleTypeID = $('#cboVehicleType').val();

    var vData = {
        VehicleTypeID: vVehicleTypeID == '' ? 0 : vVehicleTypeID
    };

    jsfn_ajaxPost('/Base/GetVehicleSubType', vData, jsfn_onVehicleTypeChangeResponse);
}

function jsfn_onVehicleTypeChangeResponse(data) {
    $("#cboVehicleSubType").html(""); // clear before appending new list

    $("#cboVehicleSubType").append(
        $('<option></option>').val("").html("Select Vehicle Sub Type"));

    $.each(data, function (i, vstype) {
        $("#cboVehicleSubType").append(
            $('<option></option>').val(vstype.id).html(vstype.text));
    });

    $('#cboVehicleSubType').selectpicker('refresh');
}