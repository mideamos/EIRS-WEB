$(document).ready(function () {
    $('#cboGender').on("change", jsfn_onGenderChange);
});

function jsfn_onGenderChange() {
    var vGenderID = $('#cboGender').val();

    if (vGenderID != '') {

        var vData = {
            GenderID: vGenderID == '' ? 0 : vGenderID
        };

        jsfn_ajaxPost('/RequestPersonalTCC/GetTitle', vData, jsfn_onGenderChangeResponse);
    }
}

function jsfn_onGenderChangeResponse(data) {
    $("#cboTitle").html("");

    $("#cboTitle").append(
        $('<option></option>').val("").html("Select Title"));

    if (data != undefined) {
        $.each(data, function (i, tit) {
            $("#cboTitle").append(
                $('<option></option>').val(tit.id).html(tit.text));
        });

        $('#cboTitle').selectpicker('refresh');
    }
}
