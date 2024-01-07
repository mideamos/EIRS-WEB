$(document).ready(function () {
    $('#btnSubmit').click(function () {
        if ($("#frmVehicle").valid()) {
            $('#dvUpdateDialog').dialog('open');
        }
        return false;

    });

    $('#dvUpdateDialog').removeClass('hide');
    $('#dvUpdateDialog').dialog({
        autoOpen: false,
        width: 600,
        resizable: false,
        modal: true,
        title: "<div class='widget-header'><h4>Edit Record Confirmation</h4></div>",
        buttons: [{
            html: "<i class='fa fa-check'></i>&nbsp; Yes",
            "class": "btn btn-primary",
            click: function () {
                $("#frmVehicle").submit();
            }
        }, {
            html: "<i class='fa fa-times'></i>&nbsp; No",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

   
    $('#cboVehiclePurpose').on("change", jsfn_onVehiclePurposeChange);
    $('#cboVehicleType').on("change", jsfn_onVehicleTypeChange);

});


function jsfn_onVehiclePurposeChange() {
    var vVehiclePurposeID = $('#cboVehiclePurpose').val();

    var vData = {
        VehiclePurposeID: vVehiclePurposeID == '' ? 0 : vVehiclePurposeID
    };

    jsfn_ajaxPost('/Vehicle/GetVehicleFunction', vData, jsfn_onVehiclePurposeChangeResponse);
}

function jsfn_onVehiclePurposeChangeResponse(data) {
    $("#cboVehicleFunction").html(""); // clear before appending new list

    $("#cboVehicleFunction").append(
          $('<option></option>').val("").html("Select Vehicle Function"));

    $.each(data, function (i, bfunc) {
        $("#cboVehicleFunction").append(
            $('<option></option>').val(bfunc.id).html(bfunc.text));
    });

    $('#cboVehicleFunction').val('').select2({ "width": "100%" });
}

function jsfn_onVehicleTypeChange() {
    var vVehicleTypeID = $('#cboVehicleType').val();

    var vData = {
        VehicleTypeID: vVehicleTypeID == '' ? 0 : vVehicleTypeID
    };

    jsfn_ajaxPost('/Vehicle/GetVehicleSubType', vData, jsfn_onVehicleTypeChangeResponse);
}

function jsfn_onVehicleTypeChangeResponse(data) {
    $("#cboVehicleSubType").html(""); // clear before appending new list

    $("#cboVehicleSubType").append(
            $('<option></option>').val("").html("Select Vehicle Sub Type"));

    $.each(data, function (i, vstype) {
        $("#cboVehicleSubType").append(
            $('<option></option>').val(vstype.id).html(vstype.text));
    });

    $('#cboVehicleSubType').val('').select2({ "width": "100%" });
}