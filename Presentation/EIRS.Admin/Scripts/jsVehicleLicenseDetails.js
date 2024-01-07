$(document).ready(function () {
    $('#btnSubmit').click(function () {
        if ($("#frmVehicleLicense").valid()) {
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
                $("#frmVehicleLicense").submit();
            }
        }, {
            html: "<i class='fa fa-times'></i>&nbsp; No",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    $('#cboVehicle').on("change", jsfn_onVehicleChange);
});

function jsfn_onVehicleChange() {
    var vVehicleID = $('#cboVehicle').val();

    var vData = {
        VehicleID: vVehicleID == '' ? 0 : vVehicleID
    };

    jsfn_ajaxPost('/VehicleLicense/GetVehicleInsurance', vData, jsfn_onVehicleChangeResponse);
}

function jsfn_onVehicleChangeResponse(data) {
    $("#cboVehicleInsurance").html(""); // clear before appending new list

    $("#cboVehicleInsurance").append(
        $('<option></option>').val("").html("Select Vehicle Insurance"));

    $.each(data, function (i, vi) {
        $("#cboVehicleInsurance").append(
            $('<option></option>').val(vi.id).html(vi.text));
    });

    $('#cboVehicleInsurance').val('').select2({ "width": "100%" });
}