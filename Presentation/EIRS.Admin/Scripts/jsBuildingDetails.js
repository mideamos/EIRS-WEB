$(document).ready(function () {
    $('#btnSubmit').click(function () {
        if ($("#frmBuilding").valid()) {
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
                $("#frmBuilding").submit();
            }
        }, {
            html: "<i class='fa fa-times'></i>&nbsp; No",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

  
    $('#cboTown').on("change", jsfn_onTownChange);
    $('#cboLGA').on("change", jsfn_onLGAChange);
});

function jsfn_onTownChange() {
    var vTownID = $('#cboTown').val();

    if (vTownID != '') {

        var vData = {
            TownID: vTownID == '' ? 0 : vTownID
        };

        jsfn_ajaxPost('/Building/GetTownDetails', vData, jsfn_onTownChangeResponse);
    }
}

function jsfn_onTownChangeResponse(data) {
    if (data != undefined) {
        $("#cboLGA").val(data.LGAID).select2({ "width": "100%" });
        jsfn_onLGAChange();
    }
}

function jsfn_onLGAChange() {
    var vLGAID = $('#cboLGA').val();

    var vData = {
        LGAID: vLGAID == '' ? 0 : vLGAID
    };

    jsfn_ajaxPost('/Building/GetWard', vData, jsfn_onLGAChangeResponse);
}

function jsfn_onLGAChangeResponse(data) {
    $("#cboWard").html(""); // clear before appending new list

    $("#cboWard").append(
            $('<option></option>').val("").html("Select Ward"));

    $.each(data, function (i, wrd) {
        $("#cboWard").append(
            $('<option></option>').val(wrd.id).html(wrd.text));
    });

    $('#cboWard').val('').select2({ "width": "100%" });
}
