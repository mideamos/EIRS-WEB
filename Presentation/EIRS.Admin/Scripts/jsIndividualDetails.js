$(document).ready(function () {
    $('#btnSubmit').click(function () {
        if ($("#frmIndividual").valid()) {
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
                $("#frmIndividual").submit();
            }
        }, {
            html: "<i class='fa fa-times'></i>&nbsp; No",
            "class": "btn btn-default",
            click: function () {
                $(this).dialog("close");
            }
        }]
    });

    $('#cboGender').on("change", jsfn_onGenderChange);

});

function jsfn_onGenderChange() {
    var vGenderID = $('#cboGender').val();

    if (vGenderID != '') {

        var vData = {
            GenderID: vGenderID == '' ? 0 : vGenderID
        };

        jsfn_ajaxPost('/Individual/GetTitle', vData, jsfn_onGenderChangeResponse);
    }
}

function jsfn_onGenderChangeResponse(data) {
    $("#cboTitle").html("");

    $("#cboTitle").append(
            $('<option></option>').val("").html("Select Title"));

    if (data != undefined) {
        $.each(data, function (i, wrd) {
            $("#cboTitle").append(
                $('<option></option>').val(wrd.id).html(wrd.text));
        });

        $('#cboTitle').val('').select2({ "width": "100%" });
    }
}
