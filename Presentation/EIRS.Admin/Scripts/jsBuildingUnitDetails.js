$(document).ready(function () {
    $('#btnSubmit').click(function () {
        if ($("#frmBuildingUnit").valid()) {
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
                $("#frmBuildingUnit").submit();
            }
        }, {
            html: "<i class='fa fa-times'></i>&nbsp; No",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    $('#cboUnitPurpose').on("change", jsfn_onUnitPurposeChange);
});

function jsfn_onUnitPurposeChange() {
    var vUnitPurposeID = $('#cboUnitPurpose').val();

    var vData = {
        UnitPurposeID: vUnitPurposeID == '' ? 0 : vUnitPurposeID
    };

    jsfn_ajaxPost('/Unit/GetUnitFunction', vData, jsfn_onUnitPurposeChangeResponse);
}

function jsfn_onUnitPurposeChangeResponse(data) {
    $("#cboUnitFunction").html(""); // clear before appending new list

    $("#cboUnitFunction").append(
        $('<option></option>').val("").html("Select Unit Function"));


    $.each(data, function (i, bfunc) {
        $("#cboUnitFunction").append(
            $('<option></option>').val(bfunc.id).html(bfunc.text));
    });

    $('#cboUnitFunction').val('').select2({ "width": "100%" });
}