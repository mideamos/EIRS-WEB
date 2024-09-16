jQuery(document).ready(function () {
    $('#cboTaxOffice').on("change", jsfn_onTaxOfficeChange);
});

function jsfn_onTaxOfficeChange() {
    var vTaxOfficeID = $('#cboTaxOffice').val();

    if (vTaxOfficeID !== '') {

        var vData = {
            TaxOfficeID: vTaxOfficeID === '' ? 0 : vTaxOfficeID,
            showmanager: false
        };

        jsfn_ajaxPost('/Base/GetTaxOfficeManger', vData, jsfn_onTaxOfficeChangeResponse);
    }
}

function jsfn_onTaxOfficeChangeResponse(data) {
    $("#TaxOfficerID").html("");

    $("#TaxOfficerID").append(
        $('<option></option>').val("").html("Select Tax Officer"));

    if (data !== undefined) {
        $.each(data, function (i, tit) {
            $("#TaxOfficerID").append(
                $('<option></option>').val(tit.id).html(tit.text));
        });

        $('#TaxOfficerID').selectpicker('refresh');
    }

    $("#ReplacementID").html("");

    $("#ReplacementID").append(
        $('<option></option>').val("").html("Select Replacement"));

    if (data !== undefined) {
        $.each(data, function (i, tit) {
            $("#ReplacementID").append(
                $('<option></option>').val(tit.id).html(tit.text));
        });

        $('#ReplacementID').selectpicker('refresh');
    }
}